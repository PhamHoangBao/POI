using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreateTripViewModel
    {
        public Guid UserId { get; set; }
        public string TripName { get; set; }
    }

    public class UpdateTripViewModel
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
        public string TripName { get; set; }
    }
}
