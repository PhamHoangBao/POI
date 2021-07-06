using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
namespace POI.repository.ViewModels
{
    public class CreateTripViewModel
    {
        public string TripName { get; set; }
    }

    public class UpdateTripViewModel
    {
        public Guid TripId { get; set; }
        public string TripName { get; set; }
    }

    public class ResponseTripViewModel
    {
        public Guid TripId { get; set; }
        public string TripName { get; set; }
        public int Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public AuthenticatedUserViewModel User { get; set; }
        public List<ResponseDestinationViewModel> Destinations { get; set; }
        public List<ResponseVisitViewModel> Visits { get; set; }
    }
}
