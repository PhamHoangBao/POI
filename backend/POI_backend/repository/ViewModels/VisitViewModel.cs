using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreateVisitViewModel
    {
        //public Guid UserId { get; set; }
        public Guid PoiId { get; set; }
        public Guid TripId { get; set; }
        //public DateTime VisitDate { get; set; }
        public double Rating { get; set; }
    }

    public class UpdateVisitViewModel
    {
        public Guid VisitId { get; set; }
        public Guid UserId { get; set; }
        public Guid PoiId { get; set; }
        public Guid TripId { get; set; }
        public DateTime VisitDate { get; set; }
        public double Rating { get; set; }
    }

    public class ResponseVisitViewModel
    {
        public Guid VisitId { get; set; }
        public Guid PoiId { get; set; }
        public double Rating { get; set; }
        public DateTime VisitDate { get; set; }
        public ResponsePoiViewModel Poi { get; set; }
    }
}
