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
    public interface IVoteService : IGenericService<Vote>
    {
        Task<CreateEnum> CreateNewVote(CreateVoteViewModel vote);
        UpdateEnum UpdateVote(UpdateVoteViewModel vote);
        //DeleteEnum DeactivateUser(Guid id);
    }
    public class VoteService : GenericService<Vote>, IVoteService
    {
        private readonly IVoteRepository _voteTypeRepository;
        private readonly IMapper _mapper;

        public VoteService(IVoteRepository voteRepository
                                , IMapper mapper
                                ) : base(voteRepository)
        {
            _mapper = mapper;
            _voteTypeRepository = voteRepository;
        }

        public async Task<CreateEnum> CreateNewVote(CreateVoteViewModel vote)
        {
            if (await FirstOrDefaultAsync(m => m.UserId.Equals(vote.UserId), false) != null)
            {
                return CreateEnum.Duplicate;
            }
            else
            {
                var entity = _mapper.Map<Vote>(vote);
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

        public UpdateEnum UpdateVote(UpdateVoteViewModel vote)
        {
            var entity = _mapper.Map<Vote>(vote);
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
