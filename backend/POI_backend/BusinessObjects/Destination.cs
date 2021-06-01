using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
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
        public string Coordinate { get; set; }
        public Guid DestinationTypeId { get; set; }
        public Guid ProvinceId { get; set; }

        public virtual DestinationType DestinationType { get; set; }
        public virtual Province Province { get; set; }
        public virtual ICollection<DesHashtag> DesHashtags { get; set; }
        public virtual ICollection<TripDestination> TripDestinations { get; set; }
    }
}
