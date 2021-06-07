using System;
using System.Collections.Generic;

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

        public virtual ICollection<Poi> Pois { get; set; }
    }
}
