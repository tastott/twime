using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Tim.Twime.ImportExport
{
    using Models;

    public class Importer
    {
        //public Ride ImportRideFromGpxFile(string filepath)
        //{
        //    using (var stream = File.OpenRead(filepath))
        //    {
        //        return ImportRideFromGpxFile(stream);
        //    }
        //}
        public IList<Waypoint> GetWaypointsFromGpxFile(Stream stream)
        {
            var waypoints = new List<Waypoint>();

            var gpxDocument = XDocument.Load(stream);
            var gpxNamespace = gpxDocument.Root.GetDefaultNamespace();
            var trkpts = gpxDocument.Descendants(gpxNamespace + "trkpt").ToList();

            foreach (var trkpt in trkpts)
            {
                double lat, lng, elev;
                DateTime time;

                if (
                        double.TryParse(trkpt.Attribute("lat").Value, out lat) &&
                        double.TryParse(trkpt.Attribute("lon").Value, out lng) &&
                        DateTime.TryParse(trkpt.Element(gpxNamespace + "time").Value, out time) &&
                        double.TryParse(trkpt.Element(gpxNamespace + "ele").Value, out elev)
                    )
                {

                    var waypoint = new Waypoint(lat, lng, time, elev);

                    waypoints.Add(waypoint);
                }
                else throw new ArgumentException(String.Format("trkpt element is invalid: {0}", trkpt.ToString()));
            }

            return waypoints;
        }

        //public Ride ImportRideFromGpxFile(Stream stream)
        //{
        //    var legs = new List<Leg>();

        //    var trkpts = GetWaypointsFromGpxFile(stream);
        //    trkpts = RideUtilities.SmoothRide(trkpts);

        //    var from = trkpts.First();
        //    double totalMetres = 0;

        //    foreach (var to in trkpts.Skip(1))
        //    {
        //        var metres = DistanceBetweenTwoPoints(from.Latitude, from.Longitude, to.Latitude, to.Longitude) * 1000;
        //        totalMetres += metres;

        //        int seconds = to.Time.Subtract(from.Time).Seconds;

        //        var leg = new Leg
        //        {
        //            StartTime = from.Time,
        //            FinishTime = to.Time,
        //            StartLat = from.Latitude,
        //            StartLng = from.Longitude,
        //            Metres = metres,
        //            TotalMetres = totalMetres,
        //            Speed = (metres / seconds) * 3.6,
        //            StartElevation = from.Elevation
        //        };

        //        legs.Add(leg);

        //        from = to;
        //    }

        //    return new Ride
        //    {
        //        Legs = legs
        //    };
        //}

        private double DistanceBetweenTwoPoints(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double e = (3.1415926538 * latitude1 / 180);
            double f = (3.1415926538 * longitude1 / 180);
            double g = (3.1415926538 * latitude2 / 180);
            double h = (3.1415926538 * longitude2 / 180);
            double i = (Math.Cos(e) * Math.Cos(g) * Math.Cos(f) * Math.Cos(h) + Math.Cos(e) * Math.Sin(f) * Math.Cos(g) * Math.Sin(h) + Math.Sin(e) * Math.Sin(g));
            double j = (Math.Acos(i));
            double k = (6371 * j);

            return k;
        }
    }
}
