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
using System.Linq.Expressions;
using System.Linq;


namespace POI.service.Services
{
    public interface IVisitService : IGenericService<Visit>
    {
        public Task<Tuple<CreateEnum, Guid>> CreateNewVisit(CreateVisitViewModel visit, Guid userId);
        public Task<UpdateEnum> UpdateVisit(UpdateVisitViewModel visit, Guid userId);
        public DeleteEnum ArchiveVisit(Guid id);
        public List<ResponseVisitViewModel> GetVisits(Expression<Func<Visit, bool>> predicate, bool istracked);
        public Task<UpdateEnum> UpdateRating(Guid tripId, double rating);
        public double CalculateRatingOfPoi(Guid poiId);
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
        public double CalculateRatingOfPoi(Guid poiId)
        {
            Tuple<double, double> poiRating = _visitRepository.CalculateRating(poiId);
            return (poiRating.Item1) / (poiRating.Item2);
        }
        public async Task<Tuple<CreateEnum,Guid>> CreateNewVisit(CreateVisitViewModel visit, Guid userId)
        {
            var model = await _visitRepository.FirstOrDefaultAsync(m => m.PoiId.Equals(visit.PoiId)
                                                                && m.UserId.Equals(userId)
                                                                && m.TripId.Equals(visit.TripId), false);
            if (model == null)
            {
                var entity = _mapper.Map<Visit>(visit);
                entity.UserId = userId;
                entity.Rating = -1;
                try
                {
                    await AddAsync(entity);
                    await SaveChangesAsync();
                    //Poi poi = await _poiRepository.FirstOrDefaultAsync(m => m.PoiId.Equals(entity.PoiId), false);
                    //var newRating = CalculateRating(poi.PoiId, visit.Rating);
                    //poi.Rating = newRating;
                    //_poiRepository.Update(poi);
                    //await _poiRepository.SaveChangesAsync();
                    return new Tuple<CreateEnum, Guid>(CreateEnum.Success, entity.VisitId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new Tuple<CreateEnum, Guid>(CreateEnum.ErrorInServer, Guid.Empty);
                }
            }
            else
            {
                return new Tuple<CreateEnum, Guid>(CreateEnum.Error, Guid.Empty);
            }

        }
        public async Task<UpdateEnum> UpdateRating(Guid tripId, double rating)
        {
            Visit visit = _visitRepository.FirstOrDefault(m => m.TripId.Equals(tripId), false);
            if (visit != null)
            {
                visit.Rating = rating;
                try
                {
                    Update(visit);
                    Savechanges();
                    Poi poi = await _poiRepository.FirstOrDefaultAsync(m => m.PoiId.Equals(visit.PoiId), false);
                    var newRating = CalculateRatingOfPoi(poi.PoiId);
                    poi.Rating = newRating;
                    _poiRepository.Update(poi);
                    await _poiRepository.SaveChangesAsync();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return UpdateEnum.ErrorInServer;
                }
            }
            return UpdateEnum.Error;
        }
        public async Task<UpdateEnum> UpdateVisit(UpdateVisitViewModel visit, Guid userId)
        {
            var entity = _mapper.Map<Visit>(visit);
            var currentVisit = await _visitRepository.FirstOrDefaultAsync(m => m.VisitId.Equals(entity.VisitId), false);

            if (currentVisit != null)
            {
                if (currentVisit.UserId.Equals(userId))
                {
                    try
                    {
                        currentVisit.Rating = entity.Rating;
                        Update(currentVisit);
                        Savechanges();
                        Poi poi = await _poiRepository.FirstOrDefaultAsync(m => m.PoiId.Equals(currentVisit.PoiId), false);
                        var newRating = CalculateRatingOfPoi(poi.PoiId);
                        poi.Rating = newRating;
                        _poiRepository.Update(poi);
                        await _poiRepository.SaveChangesAsync();
                        return UpdateEnum.Success;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return UpdateEnum.ErrorInServer;
                    }
                }
            }
            return UpdateEnum.Error;
        }
        public List<ResponseVisitViewModel> GetVisits(Expression<Func<Visit, bool>> predicate, bool istracked)
        {
            IQueryable<Visit> visits = _visitRepository.GetVisits(predicate, istracked);
            List<Visit> visitList = visits.ToList();
            List<ResponseVisitViewModel> responses = _mapper.Map<List<ResponseVisitViewModel>>(visitList);

            for(int i = 0; i< responses.Count(); i++)
            {
                var response = responses[i];
                var visit = visitList[i];
                response.Poi = _mapper.Map<ResponsePoiViewModel>(visit.Poi);
            }
            return responses;
        }


    }
}
