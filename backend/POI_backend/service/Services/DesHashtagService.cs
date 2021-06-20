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
using System.Linq;

namespace POI.service.Services
{
    public interface IDesHashtagService : IGenericService<DesHashtag>
    {
        public Task<CreateEnum> CreateNewDesHashtag(Guid destinationID, Guid hashtagID);
        public IQueryable<DesHashtag> GetDestinationWithHashtagID(Guid hashtagID);
    }
    public class DesHashtagService : GenericService<DesHashtag>, IDesHashtagService
    {
        private readonly IDesHashtagRepository _desHashtagRepository;
        private readonly IMapper _mapper;
        private readonly IHashtagRepository _hashtagRepository;
        private readonly IDestinationRepository _destinationRepository;

        public DesHashtagService(IDesHashtagRepository desHashtagRepository,
                    IMapper mapper,
                    IHashtagRepository hashtagRepository,
                    IDestinationRepository destinationRepository)
            : base(desHashtagRepository)
        {
            _mapper = mapper;
            _desHashtagRepository = desHashtagRepository;
            _hashtagRepository = hashtagRepository;
            _destinationRepository = destinationRepository;
        }

        public async Task<CreateEnum> CreateNewDesHashtag(Guid destinationID, Guid hashtagID)
        {
            var destination = await _destinationRepository
                .FirstOrDefaultAsync(m => m.DestinationId.Equals(destinationID), false);
            var hashtag = await _hashtagRepository
                .FirstOrDefaultAsync(m => m.HashtagId.Equals(hashtagID), false);

            if (destination == null || hashtag == null)
            {
                return CreateEnum.Error;
            }
            else if (destination.Status == (int)DestinationEnum.Disable
                || hashtag.Status == (int)HashtagEnum.Disable)
            {
                return CreateEnum.Error;
            }
            else
            {
                DesHashtag desHashtag = await _desHashtagRepository
                    .FirstOrDefaultAsync(m => m.DestinationId.Equals(destinationID) && m.HashtagId.Equals(hashtagID), false);
                if (desHashtag != null)
                {
                    return CreateEnum.Duplicate;
                }
                else
                {
                    try
                    {
                        var entity = new DesHashtag
                        {
                            DestinationId = destinationID,
                            HashtagId = hashtagID,
                            Status = (int)DesHashtagEnum.Available
                        };
                        await AddAsync(entity);
                        await SaveChangesAsync();
                        return CreateEnum.Success;
                    }
                    catch (Exception e)
                    {
                        return CreateEnum.ErrorInServer;
                    }

                }
            }
        }

        public IQueryable<DesHashtag> GetDestinationWithHashtagID(Guid hashtagID)
        {
            return _desHashtagRepository
                .GetDestHashtag(m => m.HashtagId.Equals(hashtagID), false);
        }
    }
}
