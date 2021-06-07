using System;
using System.Collections.Generic;

#nullable disable

namespace POI.repository.Entities
{
    public partial class Poi
    {
        public Poi()
        {
            Poiblogs = new HashSet<Poiblog>();
        }

        public Guid PoiId { get; set; }
        public Guid PoiTypeId { get; set; }
        public Guid? UserId { get; set; }
        public Guid DestinationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Coordinate { get; set; }
        public double Rating { get; set; }
        public int Status { get; set; }

        public virtual Poitype PoiType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Poiblog> Poiblogs { get; set; }
    }
}
