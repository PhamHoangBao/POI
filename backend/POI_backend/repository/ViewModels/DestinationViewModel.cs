using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreateDestinationViewModel
    {
        public string DestinationName { get; set; }
        public string Coordinate { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid DestinationTypeId { get; set; }
    }

    public class UpdateDestinationViewModel
    {
        public Guid DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string Coordinate { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid DestinationTypeId { get; set; }
    }
}
