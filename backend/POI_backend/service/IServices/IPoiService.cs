using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;

namespace POI.service.IServices
{
    public interface IPoiService : IGenericService<Poi>
    {
        public Task<CreateEnum> CreateNewPoi(CreatePoiViewModel province);
        public DeleteEnum DeactivatePoi(Guid id);
        public UpdateEnum UpdatePoi(UpdatePoiViewModel province);
    }
}
