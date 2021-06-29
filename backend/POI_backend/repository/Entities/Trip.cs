using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace POI.repository.Entities
{
    public partial class Trip
    {
        public Trip()
        {
            Blogs = new HashSet<Blog>();
            TripDestinations = new HashSet<TripDestination>();
        }

        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
        public string TripName { get; set; }
        public int Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Blog> Blogs { get; set; }
        [JsonIgnore]
        public virtual ICollection<TripDestination> TripDestinations { get; set; }
    }
}
