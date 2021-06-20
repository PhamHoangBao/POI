using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using System.Threading.Tasks;


namespace POI.service.IServices
{
    public interface ITripService : IGenericService<Trip>
    {
        public Task<CreateEnum> CreateNewTrip(CreateTripViewModel trip);

        public UpdateEnum UpdateTrip(UpdateTripViewModel trip);

        public DeleteEnum ArchiveTrip(Guid id);

        public UpdateEnum FinishTrip(Guid id);
    }
}
