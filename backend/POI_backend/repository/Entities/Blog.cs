using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace POI.repository.Entities
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
        public Guid TripId { get; set; }
        [JsonIgnore]
        public virtual Trip Trip { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Poiblog> Poiblogs { get; set; }
        [JsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
