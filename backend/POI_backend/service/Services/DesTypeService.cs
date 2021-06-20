using System;
using POI.repository.Repositories;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;

namespace POI.service.Services
{
    public interface IDesTypeService : IGenericService<DestinationType>
    {
        public Task<CreateEnum> CreateNewDesType(CreateDesTypeViewModel destination);
        public DeleteEnum DeactivateDesType(Guid id);
        public UpdateEnum UpdateDesType(UpdateDesTypeViewModel destination);
    }
    public class DesTypeService : GenericService<DestinationType>, IDesTypeService
    {
        private readonly IDesTypeRepository _desTypeRepository;
        private readonly IMapper _mapper;

        public DesTypeService(IDesTypeRepository desTypeRepository
                                , IMapper mapper
                                ) : base(desTypeRepository)
        {
            _mapper = mapper;
            _desTypeRepository = desTypeRepository;
        }

        public async Task<CreateEnum> CreateNewDesType(CreateDesTypeViewModel desType)
        {
            if (await FirstOrDefaultAsync(m => m.Name.Equals(desType.Name), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<DestinationType>(desType);
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

        public DeleteEnum DeactivateDesType(Guid id)
        {
            DestinationType desType = _desTypeRepository.GetByID(id);
            if (desType == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                desType.Status = (int)DesTypeEnum.Disable;
                try
                {
                    Update(desType);
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

        public UpdateEnum UpdateDesType(UpdateDesTypeViewModel desType)
        {
            if (FirstOrDefault(m => m.DestinationTypeId.Equals(desType.DestinationTypeId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<DestinationType>(desType);
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
