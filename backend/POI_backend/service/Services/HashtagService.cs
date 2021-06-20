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
    public interface IHashtagService : IGenericService<Hashtag>
    {
        Task<CreateEnum> CreateNewHashtag(CreateHashtagViewModel hashtag);
        UpdateEnum UpdateHashtag(UpdateHashtagViewModel hashtag);
        DeleteEnum DeactivateHashtag(Guid id);
    }
    public class HashtagService : GenericService<Hashtag>, IHashtagService
    {
        private readonly IHashtagRepository _hashtagRepository;
        private readonly IMapper _mapper;

        public HashtagService(IHashtagRepository hashtagRepository
                                , IMapper mapper
                                ) : base(hashtagRepository)
        {
            _mapper = mapper;
            _hashtagRepository = hashtagRepository;
        }

        public async Task<CreateEnum> CreateNewHashtag(CreateHashtagViewModel hashtag)
        {
            if (await FirstOrDefaultAsync(m => m.Name.Equals(hashtag.Name), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<Hashtag>(hashtag);
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

        public DeleteEnum DeactivateHashtag(Guid id)
        {
            Hashtag hashtag = _hashtagRepository.GetByID(id);
            if (hashtag == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                hashtag.Status = (int)HashtagEnum.Disable;
                try
                {
                    Update(hashtag);
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

        public UpdateEnum UpdateHashtag(UpdateHashtagViewModel hashtag)
        {
            if (FirstOrDefault(m => m.HashtagId.Equals(hashtag.HashtagId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Hashtag>(hashtag);
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
