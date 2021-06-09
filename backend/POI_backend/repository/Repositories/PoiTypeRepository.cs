using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.IRepositories;

namespace POI.repository.Repositories
{
    public class PoitypeRepository : GenericRepository<Poitype>, IPoiTypeRepository
    {
        public PoitypeRepository(POIContext context) : base(context)
        {

        }
    }
}
