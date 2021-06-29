using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Repositories;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using System.Linq;
using System.Linq.Expressions;

namespace POI.service.Services
{

    public interface IRoleService : IGenericService<Role>
    {

    }
    public class RoleService : GenericService<Role>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper) : base(roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }
    }
}
