using POI.repository.Entities;
using POI.repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.Repositories
{
    public class DestinationRepository : GenericRepository<Destination>, IDestinationRepository
    {
        public DestinationRepository(POIContext context) : base(context)
        {

        }
    }
}
