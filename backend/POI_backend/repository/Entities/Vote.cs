using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class Vote
    {
        public Guid VoteId { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogId { get; set; }
        public int VoteValue { get; set; }
        public int Status { get; set; }
        [JsonIgnore]
        public virtual Blog Blog { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
