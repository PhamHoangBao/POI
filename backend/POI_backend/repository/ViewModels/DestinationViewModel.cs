using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries;


namespace POI.repository.ViewModels
{

    public class GeoLocation : Point
    {
        const int GoogleMapSRID = 4326;
        public GeoLocation(double latitude, double longtitude) : base(x: longtitude, y: latitude) => base.SRID = GoogleMapSRID;

        public double Longtitude => base.X;

        public double Latitude => base.Y;
    }

    public partial class MyPoint
    {
        public MyPoint(double latitude, double longtitude)
        {
            Latitude = latitude;
            Longtitude = longtitude;
        }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }

    }

    public class CreateDestinationViewModel
    {
        public string DestinationName { get; set; }
        public MyPoint Location { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid DestinationTypeId { get; set; }
    }

    public class UpdateDestinationViewModel
    {
        public Guid DestinationId { get; set; }
        public string DestinationName { get; set; }
        public MyPoint Location { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid DestinationTypeId { get; set; }
    }
}
