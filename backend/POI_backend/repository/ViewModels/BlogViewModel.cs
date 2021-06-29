using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreateBlogViewModel
    {
        public Guid TripId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Guid> PoiIds { get; set; }
    }

    public class UpdateBlogViewModel
    {
        public Guid BlogId { get; set; }
        public Guid TripId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Guid> PoiIds { get; set; }
    }

    public class ResponseBlogViewModel
    {
        public Guid BlogId { get; set; }
        public Guid TripId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public AuthenticatedUserViewModel User { get; set; }
        public string TripName { get; set; }
        public int PosVotes { get; set; }
        public int NegVotes { get; set; }
        public int Status { get; set; }
    }
}
