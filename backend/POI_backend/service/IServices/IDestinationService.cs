using POI.repository.Entities;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using System;
using System.Threading.Tasks;

namespace POI.service.IServices
{
    public interface IDestinationService : IGenericService<Destination>
    {
        public Task<CreateEnum> CreateNewDestination(CreateDestinationViewModel destination);
        public DeleteEnum DeactivateDestination(Guid id);
        public UpdateEnum UpdateDestination(UpdateDestinationViewModel destination);
    }
}
