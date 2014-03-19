using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;

namespace Tim.Twime.Services.Weather
{
    using Models;
    using Units;
    using DataGovUkMetOfficeWeatherService;
    using System.Text.RegularExpressions;

    public class MetOfficeWeatherDataProvider : IWeatherDataProvider
    {
        static MetOfficeWeatherDataProvider()
        {
            compassBearings = new Dictionary<string, double>();
            var directions = new string[] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };

            for (int i = 0; i < 16; i++)
            {
                compassBearings[directions[0]] =  Math.PI * i / 8;
            }
        }

        private static IDictionary<string, double> compassBearings;

        private decimal _proximityThreshold = 0.2M;
        private int _maxSites = 10;
        private int _timeoutSeconds = 180;

        public WeatherObservation GetWeather(double latitude, double longitude, DateTime dateTime)
        {
            var webService = new DataGovUKMetOfficeWeatherOpenDataContainer(new Uri("https://api.datamarket.azure.com/DataGovUK/MetOfficeWeatherOpenData/v1/"));
            webService.IgnoreMissingProperties = true;
            webService.Timeout = _timeoutSeconds;
            webService.Credentials = new NetworkCredential("ta.stott@outlook.com", "kRUYcUVvCrsjgyQAWz7T0WQvbA6RR7+srpA1aRY1SsA");

            //Get up to [_maxSites] near-ish sites with server-side filter
            var nearSites = webService.Site.Where
                (
                    s =>
                    (s.Latitude.Value - (decimal)latitude) < _proximityThreshold
                    && (s.Latitude.Value - (decimal)latitude) > -_proximityThreshold
                    && (s.Longitude.Value - (decimal)longitude) < _proximityThreshold
                    && (s.Longitude.Value - (decimal)longitude) > -_proximityThreshold
                    && s.ID < 99999 //Dubious filter for observation sites
                ) 
                .ToList()
                .OrderBy(s => SiteDistance(latitude, longitude, s))
                .Take(_maxSites)
                .ToDictionary(s => s.Name);

            //Get observations for these sites and pick closest
            var siteNameFilter = String.Join(" or ", nearSites.Keys.Select(s => String.Format("(SiteName eq '{0}')", s)));
            var dateFilter = String.Format("ObservationDate eq datetime'{0}'", dateTime.Date.ToString("yyyy-MM-dd"));
            var timeFilter = String.Format("ObservationTime eq {0}", dateTime.Hour);
            var filter = String.Format("({0}) and ({1}) and ({2})", siteNameFilter, dateFilter, timeFilter);

            var nearestObservation = webService.Observation.AddQueryOption("$filter", filter)
                .ToList()
                .OrderBy(o => SiteDistance(latitude, longitude, nearSites[o.SiteName]))
                .FirstOrDefault();


            if (nearestObservation == null) return null;
            else
            {
                if (!nearestObservation.WindDirection.HasValue || nearestObservation.WindDirection.Value < 0 || nearestObservation.WindDirection.Value > 15)
                    throw new Exception("Unexpected wind direction value");
                double windBearing = nearestObservation.WindDirection.Value * Math.PI / 8;

                return new WeatherObservation
                (
                    source: "Met Office",
                    siteName: CleanSiteName(nearestObservation.SiteName),
                    latitude: (double)nearestObservation.Latitude.Value,
                    longitude: (double)nearestObservation.Longitude.Value,
                    windSpeed: Units.MphToMps((double)nearestObservation.WindSpeed.Value),
                    windBearing: windBearing
                );
            }
        }

        private double SiteDistance(double latitude, double longitude, Site s)
        {
            double distance = Tim.Geography.Geography.EarthDistanceDegrees(latitude, longitude, (double)s.Latitude.Value, (double)s.Longitude.Value);

            return distance;
        }

        private static string CleanSiteName(string originalName)
        {
            var siteIdRegex = new Regex(@"\([0-9]+\)");
            var cleanName = siteIdRegex.Replace(originalName, "");

            var wordRegex = new Regex(@"\S+");
            MatchEvaluator properCaseEvaluator = match => match.Value.Substring(0,1).ToUpper() + match.Value.Substring(1).ToLower();
            cleanName = wordRegex.Replace(cleanName, properCaseEvaluator);

           return cleanName.Trim();
        }
    }
}