using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class Visit
    {
        public Guid VisitId { get; set; }
        public Guid UserId { get; set; }
        public Guid PoiId { get; set; }
        public Guid TripId { get; set; }
        public DateTime VisitDate { get; set; }
        public double Rating { get; set; }
        public int Status { get; set; }

        [JsonIgnore]
        public virtual Trip Trip { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
