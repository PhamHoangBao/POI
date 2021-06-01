using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Province
    {
        public Province()
        {
            Destinations = new HashSet<Destination>();
        }

        public Guid ProvinceId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Destination> Destinations { get; set; }
    }
}
