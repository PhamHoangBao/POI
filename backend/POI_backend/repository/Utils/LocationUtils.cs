using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries;
using POI.repository.ViewModels;

namespace POI.repository.Utils
{
    public class LocationUtils
    {
        public enum DistanceUnit
        {
            Miles,
            Kilometers
        }

        public static double ToRadian(double a)
        {
            return a * (Math.PI / 180);
        }

        public static double HaversineDistance(Point pointA, Point pointB, DistanceUnit unit)
        {
            var latA = pointA.Y;
            var latB = pointB.Y;
            Console.WriteLine("latA : " + latA.ToString() + " latB : " + latB.ToString());
            var longA = pointA.X;
            var longB = pointB.X;
            Console.WriteLine("longA : " + longA.ToString() + " longB : " + longB.ToString());
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var dLat = ToRadian(latB - latA);
            var dLong = ToRadian(longB - longA);
            var h1 = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                + Math.Cos(ToRadian(latA)) * Math.Cos(ToRadian(latB))
                * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            Console.WriteLine("Distance : " + R * h2);
            return R * h2;
        }

        public static double EuclideDistance(MyPoint pointA, MyPoint pointB)
        {
            var dLat = pointA.Latitude - pointB.Latitude;
            var dLng = pointA.Longtitude - pointB.Longtitude;
            var diff = dLat * dLat + dLng * dLng;
            return Math.Sqrt(diff);
        }
    }
}
