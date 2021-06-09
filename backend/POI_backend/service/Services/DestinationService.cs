using System;
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
