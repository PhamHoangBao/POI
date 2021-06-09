using POI.repository.Entities;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using System;
using System.Threading.Tasks;

namespace POI.service.IServices
{
    public interface IDesTypeService : IGenericService<DestinationType>
    {
        public Task<CreateEnum> CreateNewDesType(CreateDesTypeViewModel destination);
        public DeleteEnum DeactivateDesType(Guid id);
        public UpdateEnum UpdateDesType(UpdateDesTypeViewModel destination);
    }
}
