using System;
using POI.repository.Repositories;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace POI.service.Services
{
    public interface IDestinationService : IGenericService<Destination>
    {
        public Task<CreateEnum> CreateNewDestination(CreateDestinationViewModel destination);
        public DeleteEnum DeactivateDestination(Guid id);
        public UpdateEnum UpdateDestination(UpdateDestinationViewModel destination);
        public List<ResponseDestinationViewModel> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked);

        public IQueryable<Destination> GetDetinationWithProvince(Guid provinceId);
    }
    public class DestinationService : GenericService<Destination>, IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IMapper _mapper;

        public DestinationService(IDestinationRepository destinationRepository
                                , IMapper mapper
                                ) : base(destinationRepository)
        {
            _mapper = mapper;
            _destinationRepository = destinationRepository;
        }

        public async Task<CreateEnum> CreateNewDestination(CreateDestinationViewModel destination)
        {
            if (await FirstOrDefaultAsync(m => m.DestinationName.Equals(destination.DestinationName), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<Destination>(destination);
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

        public DeleteEnum DeactivateDestination(Guid id)
        {
            Destination destination = _destinationRepository.GetByID(id);
            if (destination == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                destination.Status = (int)DestinationEnum.Disable;
                try
                {
                    Update(destination);
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

        public List<ResponseDestinationViewModel> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked)
        {
            IQueryable<Destination> destinations = _destinationRepository.GetDestination(predicate, istracked);
            List<Destination> destinationList = destinations.ToList();
            List<ResponseDestinationViewModel> responses = _mapper.Map<List<ResponseDestinationViewModel>>(destinationList);
            return responses;
        }

        public IQueryable<Destination> GetDetinationWithProvince(Guid provinceId)
        {
            return _destinationRepository.GetDestination(m => m.ProvinceId.Equals(provinceId), false);
        }

        public UpdateEnum UpdateDestination(UpdateDestinationViewModel destination)
        {
            if (FirstOrDefault(m => m.DestinationId.Equals(destination.DestinationId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Destination>(destination);
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
