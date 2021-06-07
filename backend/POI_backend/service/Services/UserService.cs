using System;
using System.Collections.Generic;
using System.Text;
using POI.service.IServices;
using POI.repository.Entities;
using POI.repository.Repositories;
using POI.repository.IRepositories;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;

namespace POI.service.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper) : base(userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
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
            if (!FirstOrDefault(m => m.UserId.Equals(user.UserId), false).Email.Equals(user.Email))
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<User>(user);
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
    }
}
