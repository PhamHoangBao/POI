using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;

namespace POI.service.IServices
{
    public interface IVisitService : IGenericService<Visit>
    {
        public Task<CreateEnum> CreateNewVisit(CreateVisitViewModel visit);

        public UpdateEnum UpdateVisit(UpdateVisitViewModel visit);

        public DeleteEnum ArchiveVisit(Guid id);
    }
}
