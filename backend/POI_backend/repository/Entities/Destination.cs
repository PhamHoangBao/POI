using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using POI.repository.Utils;
using POI.repository.ViewModels;

#nullable disable

namespace POI.repository.Entities
{
    public partial class Destination
    {
        public Destination()
        {
            DesHashtags = new HashSet<DesHashtag>();
            Pois = new HashSet<Poi>();
            TripDestinations = new HashSet<TripDestination>();
        }

        public Guid DestinationId { get; set; }
        public string DestinationName { get; set; }
        public int Status { get; set; }
        public Geometry Location { get; set; }
        public Guid DestinationTypeId { get; set; }
        public Guid ProvinceId { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public virtual DestinationType DestinationType { get; set; }
        [JsonIgnore]
        public virtual Province Province { get; set; }
        [JsonIgnore]
        public virtual ICollection<DesHashtag> DesHashtags { get; set; }
        [JsonIgnore]
        public virtual ICollection<Poi> Pois { get; set; }
        [JsonIgnore]
        public virtual ICollection<TripDestination> TripDestinations { get; set; }

        public bool GetDistance(MyPoint location, double radius)
        {
            var currentLocation = new Point(location.Longtitude, location.Latitude);
            var destinationLocation = new Point(this.Location.Coordinate.Y, this.Location.Coordinate.X);
            double distance = LocationUtils.HaversineDistance(currentLocation, destinationLocation, LocationUtils.DistanceUnit.Kilometers);
            return distance < radius;
        }

        //public bool Func<Destination,bool>{

        //} 

        //private static
    }
}
