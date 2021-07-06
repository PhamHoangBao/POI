using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using POI.repository.Utils;
using POI.repository.ViewModels;

#nullable disable

namespace POI.repository.Entities
{
    public partial class Poi
    {
        public Poi()
        {
            Poiblogs = new HashSet<Poiblog>();
            Visits = new HashSet<Visit>();
        }

        public Guid PoiId { get; set; }
        public Guid PoiTypeId { get; set; }
        public Guid? UserId { get; set; }
        public Guid DestinationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Geometry Location { get; set; }
        public double? Rating { get; set; }
        public int Status { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public virtual Destination Destination { get; set; }
        [JsonIgnore]
        public virtual Poitype PoiType { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Poiblog> Poiblogs { get; set; }
        [JsonIgnore]
        public virtual ICollection<Visit> Visits { get; set; }
        
        public double GetDistance(MyPoint location)
        {
            var currentLocation = new Point(location.Longtitude, location.Latitude);
            var destinationLocation = new Point(this.Location.Coordinate.Y, this.Location.Coordinate.X);
            double distance = LocationUtils.HaversineDistance(currentLocation, destinationLocation, LocationUtils.DistanceUnit.Kilometers);
            return distance;
        }
    }
}
