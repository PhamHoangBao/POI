using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.IRepositories;

namespace POI.repository.Repositories
{
    public class PoiRepository : GenericRepository<Poi>, IPoiRepository
    {
        public PoiRepository(POIContext context) : base(context)
        {

        }
    }
}
