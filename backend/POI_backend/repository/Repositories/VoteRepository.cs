using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using POI.repository.IRepositories;


namespace POI.repository.Repositories
{
    public interface IVoteRepository : IGenericRepository<Vote>
    {
    }
    public class VoteRepository : GenericRepository<Vote>, IVoteRepository
    {
        public VoteRepository(POIContext context) : base(context)
        {

        }
    }
}
