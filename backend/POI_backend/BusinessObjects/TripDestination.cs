using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class TripDestination
    {
        public Guid TripDestinationId { get; set; }
        public Guid TripId { get; set; }
        public Guid DestinationId { get; set; }
        public int Order { get; set; }

        public virtual Destination Destination { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
