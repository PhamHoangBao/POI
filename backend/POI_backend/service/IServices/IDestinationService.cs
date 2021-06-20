using POI.repository.Entities;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;



namespace POI.service.IServices
{
    public interface IDestinationService : IGenericService<Destination>
    {
        public Task<CreateEnum> CreateNewDestination(CreateDestinationViewModel destination);
        public DeleteEnum DeactivateDestination(Guid id);
        public UpdateEnum UpdateDestination(UpdateDestinationViewModel destination);
        public IQueryable<Destination> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked);

        public IQueryable<Destination> GetDetinationWithProvince(Guid provinceId);
    }
}
