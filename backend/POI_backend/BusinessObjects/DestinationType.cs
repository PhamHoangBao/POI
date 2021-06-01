using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class DestinationType
    {
        public DestinationType()
        {
            Destinations = new HashSet<Destination>();
        }

        public Guid DestinationTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Destination> Destinations { get; set; }
    }
}
