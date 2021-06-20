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
        public Task<CreateEnum> CreateNewVisit(CreateVisitViewModel visit);

        public UpdateEnum UpdateVisit(UpdateVisitViewModel visit);

        public DeleteEnum ArchiveVisit(Guid id);
    }
    public class VisitService : GenericService<Visit>, IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IMapper _mapper;

        public VisitService(IVisitRepository visitRepository
                                , IMapper mapper
                                ) : base(visitRepository)
        {
            _mapper = mapper;
            _visitRepository = visitRepository;
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

        public async Task<CreateEnum> CreateNewVisit(CreateVisitViewModel visit)
        {

            var entity = _mapper.Map<Visit>(visit);
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
