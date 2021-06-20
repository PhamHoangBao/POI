using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using POI.repository.Entities;
using POI.repository.IRepositories;
using POI.service.IServices;
using System.Threading.Tasks;
using POI.repository.ResultEnums;


namespace POI.service.IServices
{
    public interface IDesHashtagService : IGenericService<DesHashtag>
    {
        public Task<CreateEnum> CreateNewDesHashtag(Guid destinationID, Guid hashtagID);
        public IQueryable<DesHashtag> GetDestinationWithHashtagID(Guid hashtagID);
    }
}
