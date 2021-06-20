using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.IRepositories;

namespace POI.repository.Repositories
{
    public interface IProvinceRepository : IGenericRepository<Province>
    {
    }
    public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(POIContext context) : base(context)
        {

        }
    }
}
