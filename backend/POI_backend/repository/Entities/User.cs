using System;
using System.Collections.Generic;

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

        public virtual Role Role { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Poi> Pois { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
