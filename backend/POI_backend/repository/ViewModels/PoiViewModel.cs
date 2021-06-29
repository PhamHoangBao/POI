using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;

namespace POI.repository.ViewModels
{
    public class CreatePoiViewModel
    {
        public string Name { get; set; }
        public Guid PoiTypeId { get; set; }
        public Guid DestinationId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public MyPoint Location { get; set; }
    }

    

    public class UpdatePoiViewModel
    {
        public Guid PoiId { get; set; }
        public string Name { get; set; }
        public Guid PoiTypeId { get; set; }
        public string ImageUrl { get; set; }
        public Guid DestinationId { get; set; }
        public string Description { get; set; }
        public Guid? UserId { get; set; }
        public MyPoint Location { get; set; }
    }

    public class ResponsePoiViewModel
    {
        public Guid PoiId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MyPoint Location { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public int Status { get; set; }
        public Poitype PoiType { get; set; }
        public AuthenticatedUserViewModel User { get; set; }
        public ResponseDestinationViewModel Destination { get; set; }
    }
}
