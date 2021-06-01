using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Visit
    {
        public Guid VisitId { get; set; }
        public Guid UserId { get; set; }
        public Guid PoiId { get; set; }
        public Guid TripId { get; set; }
        public DateTime VisitDate { get; set; }
        public double Rating { get; set; }

        public virtual Trip Trip { get; set; }
        public virtual User User { get; set; }
    }
}
