using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using POI.repository.Entities;
using POI.service.Services;
using POI.repository.AutoMapper;
using POI.repository.ViewModels;
using POI.repository.ResultEnums;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using POI.repository.Utils;


namespace POI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <remarks>
        /// Get all users in POI system
        /// 
        ///     No parameter
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The users is retrieved", typeof(IEnumerable<User>))]
        [SwaggerResponse(404, "No user is found")]
        public IActionResult Get()
        {
            _logger.LogInformation("All users is queried");
            return Ok(_userService.GetAll());
        }


        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <remarks>
        /// Get user in POI system with ID
        /// 
        ///    ID : ID of user 
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "The user is retrieved", typeof(User))]
        [SwaggerResponse(404, "The user is not found")]
        public IActionResult Get(Guid id)
        {
            User user = _userService.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        /// <summary>
        /// Register new user (Post method)
        /// </summary>
        /// <remarks>
        /// Create new user 
        /// </remarks>

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(201, "Create user successfully")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(400, "The user is not created")]
        [SwaggerResponse(409, "The user id is conflicted")]
        public async Task<IActionResult> Post(CreateUserViewModel userViewModel)
        {
            _logger.LogInformation("Post request is called");
            CreateEnum resultCode = await _userService.CreateNewUser(userViewModel);
            if (resultCode == CreateEnum.Success)
            {
                return CreatedAtAction("Get", null);
            }
            else if (resultCode == CreateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return Conflict();
            }
        }


        /// <summary>
        /// Update account (Put method)
        /// </summary>
        /// <remarks>
        /// Update your account with password, firstname, lastname, phone only   
        /// </remarks>
        [HttpPut]
        [Authorize(Roles = "User, Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "ID is not allowed to update")]
        public IActionResult Put(UpdateUserViewModel userViewModel)
        {
            _logger.LogInformation("Put request is called");
            User currentUser = (User)HttpContext.Items["User"];
            userViewModel.RoleId = currentUser.RoleId;
            UpdateEnum resultCode = _userService.UpdateUser(userViewModel);
            if (resultCode == UpdateEnum.Success)
            {
                return Ok();
            }
            else if (resultCode == UpdateEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Deactivate an account (Delete method)
        /// </summary>
        /// <remarks>
        /// Deactivate user by their id   
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        [SwaggerResponse(401, "Request in unauthorized")]
        [SwaggerResponse(200, "Delete successfully")]
        [SwaggerResponse(400, "ID is not allowed to delete")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Delete Request is called");
            DeleteEnum resultCode = _userService.DeactivateUser(id);
            if (resultCode == DeleteEnum.Success)
            {
                return Ok();
            }
            else if (resultCode == DeleteEnum.ErrorInServer)
            {
                return StatusCode(500);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Login with user Role
        /// </summary>
        /// <remarks>
        /// Login with user Role 
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Login successfully", typeof(AuthenticatedUserViewModel))]
        [SwaggerResponse(404, "No user found")]
        public IActionResult Login(AuthenticatedUserRequest model)
        {
            _logger.LogInformation("Login Request is called");
            model.Password = PasswordUtils.HashPassword(model.Password);
            AuthenticatedUserViewModel user = _userService.AuthenticateUser(model);
     
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Register with user Role
        /// </summary>
        /// <remarks>
        /// Register with user Role 
        /// </remarks>
        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerResponse(201, "Register successfully")]
        [SwaggerResponse(404, "No user found")]
        public async Task<IActionResult> Register(RegisterUserRequest model)
        {
            _logger.LogInformation("Login Request is called");
            //model.Password = PasswordUtils.HashPassword(model.Password);
            CreateEnum result = await _userService.RegisterNewUser(model);
            switch (result)
            {
                case CreateEnum.Duplicate:
                    return BadRequest("Email is already register!");
                case CreateEnum.Success:
                    return StatusCode(201);
                case CreateEnum.ErrorInServer:
                    return StatusCode(500);
                default:
                    return StatusCode(500);
            }
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// Change password 
        /// </remarks>
        [HttpPost("change-password")]
        [Authorize(Roles = "Admin, Moderator, User")]
        [SwaggerResponse(200, "Change password successfully")]
        [SwaggerResponse(400, "User is not defined or old password is incorrect")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            User currentUser = (User)HttpContext.Items["User"];
            model.OldPassword = PasswordUtils.HashPassword(model.OldPassword);
            model.NewPassword = PasswordUtils.HashPassword(model.NewPassword);
            UpdateEnum result = await _userService.ChangePassword(currentUser.UserId, model.OldPassword, model.NewPassword);
            switch (result)
            {
                case UpdateEnum.Error:
                    return BadRequest("User is not defined or old password is incorrect");
                case UpdateEnum.Success:
                    return StatusCode(200);
                case UpdateEnum.ErrorInServer:
                    return StatusCode(500);
                default:
                    return StatusCode(500);
            }
        }
    }
}
