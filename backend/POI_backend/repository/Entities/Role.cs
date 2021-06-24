using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
