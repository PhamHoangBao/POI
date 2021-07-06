using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.Repositories;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using System.Linq.Expressions;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using POI.service.Helpers;
using System.Security.Claims;
using Microsoft.Extensions.Options;




namespace POI.service.Services
{
    public interface IUserService : IGenericService<User>
    {
        Task<CreateEnum> CreateNewUser(CreateUserViewModel user);
        UpdateEnum UpdateUser(UpdateUserViewModel user);
        DeleteEnum DeactivateUser(Guid id);
        User GetUserWithRole(Expression<Func<User, bool>> predicate, bool istracked);
        Task<CreateEnum> RegisterNewUser(RegisterUserRequest model);
        AuthenticatedUserViewModel AuthenticateUser(AuthenticatedUserRequest model);
        Task<UpdateEnum> ChangePassword(Guid userId, string oldPassword, string newPassword);
    }
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly AppSettings _appSettings;

        public UserService(IUserRepository userRepository
            , IMapper mapper
            , IRoleRepository roleRepository
            , IOptions<AppSettings> appSettings) : base(userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<CreateEnum> RegisterNewUser(RegisterUserRequest model)
        {
            if (await FirstOrDefaultAsync(m => m.Email.Equals(model.Email), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<User>(model);
                var userRole = _roleRepository.FirstOrDefault(m => m.RoleName.Equals("User"), false);
                entity.RoleId = userRole.RoleId;
                try
                {
                    await AddAsync(entity);
                    await SaveChangesAsync();
                    return CreateEnum.Success;
                }
                catch
                {
                    return CreateEnum.ErrorInServer;
                }
            }
        }

        public async Task<CreateEnum> CreateNewUser(CreateUserViewModel user)
        {
            if (await FirstOrDefaultAsync(m => m.Email.Equals(user.Email), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<User>(user);
                try
                {
                    await AddAsync(entity);
                    await SaveChangesAsync();
                    return CreateEnum.Success;
                }
                catch
                {
                    return CreateEnum.ErrorInServer;
                }
            }
        }

        public DeleteEnum DeactivateUser(Guid id)
        {

            User user = _userRepository.GetUserWithRole(m => m.UserId.Equals(id), true);
            if (user == null)
            {
                return DeleteEnum.Failed;
            }
            else if (user.Role.RoleName.Equals("Admin"))
            {
                return DeleteEnum.Failed;
            }
            else
            {
                user.Status = (int)UserEnum.Unactive;
                try
                {
                    Update(user);
                    Savechanges();
                    return DeleteEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return DeleteEnum.ErrorInServer;
                }
            }
        }

        public UpdateEnum UpdateUser(UpdateUserViewModel user)
        {
            var currentUser = FirstOrDefault(m => m.UserId.Equals(user.UserId), false);
            if (!currentUser.Email.Equals(user.Email))
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<User>(user);
                entity.Password = currentUser.Password;
                if (entity.RoleId == null || entity.RoleId.Equals(Guid.Empty))
                {
                    entity.RoleId = _roleRepository.FirstOrDefault(m => m.RoleName.Equals("User"), false).RoleId;
                }
                try
                {
                    Update(entity);
                    Savechanges();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return UpdateEnum.ErrorInServer;
                }
            }
        }

        public User GetUserWithRole(Expression<Func<User, bool>> predicate, bool istracked)
        {
            return _userRepository.GetUserWithRole(predicate, false);
        }

        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 3 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.UserId.ToString()) ,
                    new Claim(ClaimTypes.Role, user.Role.RoleName),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AuthenticatedUserViewModel AuthenticateUser(AuthenticatedUserRequest model)
        {
            User user = _userRepository.GetUserWithRole(m => m.Email.Trim().Equals(model.Email.Trim())
                                && m.Password.Trim().Equals(model.Password.Trim())
                                && m.Status == (int)UserEnum.Active, false);
            if (user == null)
            {
                return null;
            }

            if (user.Status == (int)UserEnum.Unactive)
            {
                return null;
            }
            var token = GenerateJwtToken(user);

            AuthenticatedUserViewModel entity = _mapper.Map<AuthenticatedUserViewModel>(user);
            entity.Token = token;
            return entity;
        }

        public async Task<UpdateEnum> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            User user = await _userRepository.FirstOrDefaultAsync(m => m.UserId.Equals(userId), false);
            if (user == null)
            {
                Console.WriteLine("A");
                return UpdateEnum.Error;
            }
            else if (!user.Password.Equals(oldPassword))
            {
                Console.WriteLine("A1");
                return UpdateEnum.Error;
            }
            else
            {
                user.Password = newPassword;
                try
                {
                    Console.WriteLine("A2");
                    Update(user);
                    Savechanges();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return UpdateEnum.ErrorInServer;
                }
            }
        }
    }
}
