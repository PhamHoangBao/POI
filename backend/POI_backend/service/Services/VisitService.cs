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
    public interface IVisitService : IGenericService<Visit>
    {
        public Task<CreateEnum> CreateNewVisit(CreateVisitViewModel visit, Guid userId);
        public UpdateEnum UpdateVisit(UpdateVisitViewModel visit);
        public DeleteEnum ArchiveVisit(Guid id);
        public double CalculateRating(Guid poiId, double newRating);
    }
    public class VisitService : GenericService<Visit>, IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IMapper _mapper;
        private readonly IPoiRepository _poiRepository;

        public VisitService(IVisitRepository visitRepository
                                , IMapper mapper
                                , IPoiRepository poiRepository
                                ) : base(visitRepository)
        {
            _mapper = mapper;
            _visitRepository = visitRepository;
            _poiRepository = poiRepository;
        }

        public DeleteEnum ArchiveVisit(Guid id)
        {
            Visit visit = _visitRepository.GetByID(id);
            if (visit == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                visit.Status = (int)VisitEnum.Disable;
                try
                {
                    Update(visit);
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

        public double CalculateRating(Guid poiId, double newRating)
        {
            Tuple<double,double> poiRating = _visitRepository.CalculateRating(poiId);
            return (poiRating.Item1) / (poiRating.Item2);
        }

        public async Task<CreateEnum> CreateNewVisit(CreateVisitViewModel visit, Guid userId)
        {
            var model = await _visitRepository.FirstOrDefaultAsync(m => m.PoiId.Equals(visit.PoiId)
                                                                && m.UserId.Equals(userId)
                                                                && m.TripId.Equals(visit.TripId), false);
            if (model == null)
            {
                var entity = _mapper.Map<Visit>(visit);
                entity.UserId = userId;
                try
                {
                    await AddAsync(entity);
                    await SaveChangesAsync();
                    Poi poi = await _poiRepository.FirstOrDefaultAsync(m => m.PoiId.Equals(entity.PoiId), false);
                    var newRating = CalculateRating(poi.PoiId, visit.Rating);
                    poi.Rating = newRating;
                    _poiRepository.Update(poi);
                    await _poiRepository.SaveChangesAsync();
                    return CreateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return CreateEnum.ErrorInServer;
                }
            }
            else
            {
                return CreateEnum.Error;
            }
            
        }

        public UpdateEnum UpdateVisit(UpdateVisitViewModel visit)
        {
            var entity = _mapper.Map<Visit>(visit);
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
