using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Twime.Web.Services
{
    public class WeatherObservation
    {
        public string SiteName { get; set; }
        public double LatitudeDegrees { get; set; }
        public double LongitudeDegrees { get; set; }
        public double WindSpeedMps { get; set; }
        public double WindBearingRadians { get; set; }

    }

    public interface IWeatherService
    {
        WeatherObservation GetWeather(double latitude, double longitude, DateTime dateTime);
    }
}
