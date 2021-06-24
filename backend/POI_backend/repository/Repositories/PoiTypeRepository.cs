using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;

namespace POI.repository.Repositories
{
    public interface IPoiTypeRepository : IGenericRepository<Poitype>
    {
    }
    public class PoitypeRepository : GenericRepository<Poitype>, IPoiTypeRepository
    {
        public PoitypeRepository(POIContext context) : base(context)
        {

        }
    }
}
