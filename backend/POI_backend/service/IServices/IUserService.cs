using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using System.Threading.Tasks;

namespace POI.service.IServices
{
    public interface IUserService : IGenericService<User>
    {
        Task<CreateEnum> CreateNewUser(CreateUserViewModel user);
        UpdateEnum UpdateUser(UpdateUserViewModel user);
        DeleteEnum DeactivateUser(Guid id);
    }
}
