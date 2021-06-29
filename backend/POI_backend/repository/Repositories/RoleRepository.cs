using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;

namespace POI.repository.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {

    }
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(POIContext context) : base(context)
        {

        }
    }
}
