using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Blog
    {
        public Blog()
        {
            Poiblogs = new HashSet<Poiblog>();
            Votes = new HashSet<Vote>();
        }

        public Guid BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public int PosVotes { get; set; }
        public int NegVotes { get; set; }
        public int Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Poiblog> Poiblogs { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
