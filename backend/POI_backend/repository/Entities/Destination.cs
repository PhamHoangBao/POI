using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class Destination
    {
        public Destination()
        {
            DesHashtags = new HashSet<DesHashtag>();
            TripDestinations = new HashSet<TripDestination>();
        }

        public Guid DestinationId { get; set; }
        public string DestinationName { get; set; }
        public int Status { get; set; }
        public Geometry Location { get; set; }
        public Guid DestinationTypeId { get; set; }
        public Guid ProvinceId { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public virtual DestinationType DestinationType { get; set; }
        [JsonIgnore]
        public virtual Province Province { get; set; }
        [JsonIgnore]
        public virtual ICollection<Poi> Pois { get; set; }
        [JsonIgnore]
        public virtual ICollection<DesHashtag> DesHashtags { get; set; }
        [JsonIgnore]
        public virtual ICollection<TripDestination> TripDestinations { get; set; }
    }
}
