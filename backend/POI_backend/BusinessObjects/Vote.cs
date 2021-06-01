using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Vote
    {
        public Guid VoteId { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogId { get; set; }
        public int VoteValue { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}
