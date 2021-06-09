using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;

namespace POI.service.IServices
{
    public interface IPoiTypeService : IGenericService<Poitype>
    {
        public Task<CreateEnum> CreateNewPoiType(CreatePoiTypeViewModel province);
        public DeleteEnum DeactivatePoiType(Guid id);
        public UpdateEnum UpdatePoiType(UpdatePoiTypeViewModel province);
    }
}
