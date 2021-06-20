using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using System.Threading.Tasks;

namespace POI.service.IServices
{
    public interface IVoteService : IGenericService<Vote>
    {
        Task<CreateEnum> CreateNewVote(CreateVoteViewModel vote);
        UpdateEnum UpdateVote(UpdateVoteViewModel vote);
        //DeleteEnum DeactivateUser(Guid id);
    }
}
