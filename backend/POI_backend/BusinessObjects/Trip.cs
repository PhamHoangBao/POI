﻿using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class Trip
    {
        public Trip()
        {
            TripDestinations = new HashSet<TripDestination>();
        }

        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
        public string TripName { get; set; }
        public int Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<TripDestination> TripDestinations { get; set; }
    }
}
