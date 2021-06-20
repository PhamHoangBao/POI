using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Repositories;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;

namespace POI.service.Services
{
    public interface IProvinceService : IGenericService<Province>
    {
        public Task<CreateEnum> CreateNewProvince(CreateProvinceViewModel province);
        public DeleteEnum DeactivateProvince(Guid id);
        public UpdateEnum UpdateProvince(UpdateProvinceViewModel province);
    }
    public class ProvinceService : GenericService<Province>, IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;

        public ProvinceService(IProvinceRepository provinceRepository
                                , IMapper mapper
                                ) : base(provinceRepository)
        {
            _mapper = mapper;
            _provinceRepository = provinceRepository;
        }

        public async Task<CreateEnum> CreateNewProvince(CreateProvinceViewModel province)
        {
            if (await FirstOrDefaultAsync(m => m.Name.Equals(province.Name), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<Province>(province);
                try
                {
                    await AddAsync(entity);
                    await SaveChangesAsync();
                    return CreateEnum.Success;
                }
                catch
                {
                    return CreateEnum.ErrorInServer;
                }
            }
        }

        public DeleteEnum DeactivateProvince(Guid id)
        {
            Province province = _provinceRepository.GetByID(id);
            if (province == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                province.Status = (int)ProvinceEnum.Disable;
                try
                {
                    Update(province);
                    Savechanges();
                    return DeleteEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return DeleteEnum.ErrorInServer;
                }
            }
        }

        public UpdateEnum UpdateProvince(UpdateProvinceViewModel province)
        {
            if (FirstOrDefault(m => m.ProvinceId.Equals(province.ProvinceId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Province>(province);
                try
                {
                    Update(entity);
                    Savechanges();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return UpdateEnum.ErrorInServer;
                }
            }
        }

    }
}
