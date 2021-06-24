using POI.repository.Entities;

namespace POI.repository.Repositories
{
    public interface IDesTypeRepository : IGenericRepository<DestinationType>
    {
    }
    public class DesTypeRepository : GenericRepository<DestinationType>, IDesTypeRepository
    {
        public DesTypeRepository(POIContext context) : base(context)
        {
           
        }
    }
}
