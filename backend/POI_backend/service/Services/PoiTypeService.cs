using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.IRepositories;
using POI.service.IServices;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;

namespace POI.service.Services
{
    public class PoiTypeService : GenericService<Poitype>, IPoiTypeService
    {
        private readonly IPoiTypeRepository _poiTypeRepository;
        private readonly IMapper _mapper;

        public PoiTypeService(IPoiTypeRepository poiTypeRepository
                                , IMapper mapper
                                ) : base(poiTypeRepository)
        {
            _mapper = mapper;
            _poiTypeRepository = poiTypeRepository;
        }

        public async Task<CreateEnum> CreateNewPoiType(CreatePoiTypeViewModel poiType)
        {
            if (await FirstOrDefaultAsync(m => m.Name.Equals(poiType.Name), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<Poitype>(poiType);
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

        public DeleteEnum DeactivatePoiType(Guid id)
        {
            Poitype poiType = _poiTypeRepository.GetByID(id);
            if (poiType == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                poiType.Status = (int)PoiTypeEnum.Disable;
                try
                {
                    Update(poiType);
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

        public UpdateEnum UpdatePoiType(UpdatePoiTypeViewModel poiType)
        {
            if (FirstOrDefault(m => m.PoitypeId.Equals(poiType.PoitypeId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Poitype>(poiType);
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
