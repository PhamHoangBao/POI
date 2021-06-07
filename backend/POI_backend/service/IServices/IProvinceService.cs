using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;

namespace POI.service.IServices
{
    public interface IProvinceService : IGenericService<Province>
    {
        public Task<CreateEnum> CreateNewProvince(CreateProvinceViewModel province);
        public DeleteEnum DeactivateProvince(Guid id);
        public UpdateEnum UpdateProvince(UpdateProvinceViewModel province);
    }
}
