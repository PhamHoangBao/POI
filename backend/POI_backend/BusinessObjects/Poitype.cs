using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Poitype
    {
        public Poitype()
        {
            Pois = new HashSet<Poi>();
        }

        public Guid PoitypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Poi> Pois { get; set; }
    }
}
