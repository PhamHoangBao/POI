using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.IRepositories;

namespace POI.repository.Repositories
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
    }
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        public TripRepository(POIContext context) : base(context)
        {

        }

    }
}
