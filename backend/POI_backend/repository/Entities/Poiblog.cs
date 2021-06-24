using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class Poiblog
    {
        public Guid PoiblogId { get; set; }
        public Guid PoiId { get; set; }
        public Guid BlogId { get; set; }
        public int Status { get; set; }

        [JsonIgnore]
        public virtual Blog Blog { get; set; }
        [JsonIgnore]
        public virtual Poi Poi { get; set; }
    }
}
