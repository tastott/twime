using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Tim.Twime.Web.Services
{
    using System.Net;
    using DataGovUkMetOfficeWeatherService;

    public class MetOfficeWeatherService : IWeatherService
    {
        public async Task<WeatherObservations> GetWeatherAsync(double latitude, double longitude, DateTime dateTime)
        {
            var webService = new DataGovUKMetOfficeWeatherOpenDataContainer(new Uri("https://api.datamarket.azure.com/DataGovUK/MetOfficeWeatherOpenData/v1/"));
            webService.IgnoreMissingProperties = true;
            webService.Credentials = new NetworkCredential("ta.stott@outlook.com", "kRUYcUVvCrsjgyQAWz7T0WQvbA6RR7+srpA1aRY1SsA");
    
            //var nearestSite = webService.Site.Execute()
            //    .Where(s => s.Latitude.HasValue && s.Longitude.HasValue)
            //    .OrderBy(s => Tim.Geography.Geography.EarthDistanceDegrees(latitude, longitude, (double)s.Latitude.Value, (double)s.Longitude.Value))
            //    .First();

            var allSites = webService.Site.ToList();

            var nearestSite = webService.Site
                .ToList()
                .OrderBy(s => Wank(latitude, longitude, s))
                .First();

            var distance = Tim.Geography.Geography.EarthDistanceDegrees(latitude, longitude, (double)nearestSite.Latitude.Value, (double)nearestSite.Longitude.Value);

            return new WeatherObservations
            {
                SiteName = nearestSite.Name
            };
        }

        private double Wank(double latitude, double longitude, Site s)
        {
            double distance = Tim.Geography.Geography.EarthDistanceDegrees(latitude, longitude, (double)s.Latitude.Value, (double)s.Longitude.Value);

            if (s.Name == "BENSON (3658)")
            {
            }

            return distance;
        }
    }
}