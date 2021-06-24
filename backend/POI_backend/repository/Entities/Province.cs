using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class Province
    {
        public Province()
        {
            Destinations = new HashSet<Destination>();
        }

        public Guid ProvinceId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        [JsonIgnore]
        public virtual ICollection<Destination> Destinations { get; set; }
    }
}
