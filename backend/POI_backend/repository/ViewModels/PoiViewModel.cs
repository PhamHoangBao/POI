using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreatePoiViewModel
    {
        public string Name { get; set; }
        public Guid PoiTypeId { get; set; }
        public Guid? UserId { get; set; }
        public Guid DestinationId { get; set; }
        public string Description { get; set; }
        public string Coordinate { get; set; }
    }

    public class CreatePoiByUserViewModel : CreatePoiViewModel
    {
    }

    public class UpdatePoiViewModel
    {
        public Guid PoiId { get; set; }
        public string Name { get; set; }
        public Guid PoiTypeId { get; set; }
        public Guid? UserId { get; set; }
        public Guid DestinationId { get; set; }
        public string Description { get; set; }
        public string Coordinate { get; set; }
    }
}
