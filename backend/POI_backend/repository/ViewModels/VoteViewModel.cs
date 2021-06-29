using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class VoteViewModel
    {
        public Guid BlogId { get; set; }
        public int VoteValue { get; set; }
    }

    //public class UpdateVoteViewModel
    //{
    //    public Guid VoteId { get; set; }
    //    public Guid UserId { get; set; }
    //    public Guid BlogId { get; set; }
    //    public int VoteValue { get; set; }
    //}
}
