using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Repositories;
using POI.service.IServices;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;

namespace POI.service.Services
{
    public interface IPoiService : IGenericService<Poi>
    {
        public Task<CreateEnum> CreateNewPoi(CreatePoiViewModel province);
        public DeleteEnum DeactivatePoi(Guid id);
        public UpdateEnum UpdatePoi(UpdatePoiViewModel province);
    }
    public class PoiService : GenericService<Poi>, IPoiService
    {
        private readonly IPoiRepository _poiRepository;
        private readonly IMapper _mapper;

        public PoiService(IPoiRepository poiRepository
                                , IMapper mapper
                                ) : base(poiRepository)
        {
            _mapper = mapper;
            _poiRepository = poiRepository;
        }

        public async Task<CreateEnum> CreateNewPoi(CreatePoiViewModel poi)
        {
            if (await FirstOrDefaultAsync(m => m.Name.Equals(poi.Name), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<Poi>(poi);
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

        public DeleteEnum DeactivatePoi(Guid id)
        {
            Poi poi = _poiRepository.GetByID(id);
            if (poi == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                poi.Status = (int)PoiEnum.Inactive;
                try
                {
                    Update(poi);
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

        public UpdateEnum UpdatePoi(UpdatePoiViewModel poi)
        {
            if (FirstOrDefault(m => m.PoiId.Equals(poi.PoiId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Poi>(poi);
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
