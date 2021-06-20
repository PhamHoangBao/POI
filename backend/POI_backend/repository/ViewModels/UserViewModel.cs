using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;

namespace POI.repository.ViewModels
{
    public class CreateUserViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
    }

    public class UpdateUserViewModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
    }

    public class AuthenticatedUserViewModel 
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }
        public string Token { get; set; }
    }

    public class AuthenticatedUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
