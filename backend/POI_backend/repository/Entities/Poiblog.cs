using System;
using System.Collections.Generic;

#nullable disable

namespace POI.repository.Entities
{
    public partial class Poiblog
    {
        public Guid PoiblogId { get; set; }
        public Guid PoiId { get; set; }
        public Guid BlogId { get; set; }
        public int Status { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Poi Poi { get; set; }
    }
}
