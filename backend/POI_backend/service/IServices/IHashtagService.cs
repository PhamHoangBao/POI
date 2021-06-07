using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;



namespace POI.service.IServices
{
    public interface IHashtagService : IGenericService<Hashtag>
    {
        Task<CreateEnum> CreateNewHashtag(CreateHashtagViewModel hashtag);
        UpdateEnum UpdateHashtag(UpdateHashtagViewModel hashtag);
        DeleteEnum DeactivateHashtag(Guid id);
    }
}
