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
        CreateEnum CreateVote(VoteViewModel model, Guid userID);
    }
    public class VoteService : GenericService<Vote>, IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IMapper _mapper;
        private readonly IBlogRepository _blogRepository;

        public VoteService(IVoteRepository voteRepository,
                           IBlogRepository blogRepository,
                           IMapper mapper
                                ) : base(voteRepository)
        {
            _mapper = mapper;
            _voteRepository = voteRepository;
            _blogRepository = blogRepository;
        }

        public CreateEnum CreateVote(VoteViewModel model, Guid userID)
        {
            Vote vote = _mapper.Map<Vote>(model);
            vote.UserId = userID;
            var currentBlog = _blogRepository.FirstOrDefault(m => m.BlogId.Equals(model.BlogId) && m.Status == (int)BlogEnum.Available, false);
            if (currentBlog == null)
            {
                return CreateEnum.Error;
            }
            else
            {
                var currentVote = _voteRepository.FirstOrDefault(m => m.UserId.Equals(userID) && m.BlogId.Equals(vote.BlogId), false);
                try
                {
                    if (currentVote != null)
                    {
                        if (currentVote.VoteValue != vote.VoteValue)
                        {
                            if (vote.VoteValue == (int)VoteValue.Dislike)
                            {
                                if (currentVote.VoteValue == (int)VoteValue.Like)
                                {
                                    currentBlog.PosVotes--;
                                    currentBlog.NegVotes++;
                                }
                                if (currentVote.VoteValue == (int)VoteValue.NoReaction)
                                {
                                    currentBlog.NegVotes++;
                                }
                            }
                            if (vote.VoteValue == (int)VoteValue.Like)
                            {
                                if (currentVote.VoteValue == (int)VoteValue.Dislike)
                                {
                                    currentBlog.PosVotes++;
                                    currentBlog.NegVotes--;
                                }
                                if (currentVote.VoteValue == (int)VoteValue.NoReaction)
                                {
                                    currentBlog.PosVotes++;
                                }
                            }
                            if (vote.VoteValue == (int)VoteValue.NoReaction)
                            {
                                if (currentVote.VoteValue == (int)VoteValue.Like)
                                {
                                    currentBlog.PosVotes--;
                                }
                                if (currentVote.VoteValue == (int)VoteValue.Dislike)
                                {
                                    currentBlog.NegVotes--;
                                }
                            }
                            currentVote.VoteValue = vote.VoteValue;
                            Update(currentVote);
                            _blogRepository.Update(currentBlog);
                        }
                    }
                    else
                    {
                        if (vote.VoteValue == (int)VoteValue.Dislike)
                        {
                            currentBlog.NegVotes++;
                        }
                        if (vote.VoteValue == (int)VoteValue.Like)
                        {
                            currentBlog.PosVotes++;
                        }
                        Add(vote);
                        _blogRepository.Update(currentBlog);
                    }
                    Savechanges();
                    _blogRepository.SaveChanges();
                    return CreateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return CreateEnum.Error;
                }
            }
        }
    }
}
