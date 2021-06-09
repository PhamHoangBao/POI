using POI.repository.Entities;
using POI.repository.IRepositories;

namespace POI.repository.Repositories
{
    public class DesTypeRepository : GenericRepository<DestinationType>, IDesTypeRepository
    {
        public DesTypeRepository(POIContext context) : base(context)
        {

        }
    }
}
