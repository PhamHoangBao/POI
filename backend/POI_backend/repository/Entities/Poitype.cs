using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace POI.repository.Entities
{
    public partial class Poitype
    {
        public Poitype()
        {
            Pois = new HashSet<Poi>();
        }

        public Guid PoitypeId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Icon { get; set; }

        [JsonIgnore]
        public virtual ICollection<Poi> Pois { get; set; }
    }
}
