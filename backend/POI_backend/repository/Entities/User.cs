using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class User
    {
        public User()
        {
            Blogs = new HashSet<Blog>();
            Pois = new HashSet<Poi>();
            Trips = new HashSet<Trip>();
            Votes = new HashSet<Vote>();
        }

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public int Status { get; set; }
        public string Avatar { get; set; }

        [JsonIgnore]
        public virtual Role Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<Blog> Blogs { get; set; }
        [JsonIgnore]
        public virtual ICollection<Poi> Pois { get; set; }
        [JsonIgnore]
        public virtual ICollection<Trip> Trips { get; set; }
        [JsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
