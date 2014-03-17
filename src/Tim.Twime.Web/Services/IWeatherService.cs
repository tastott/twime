using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Twime.Web.Services
{
    public class WeatherObservations
    {
        public string SiteName { get; set; }
    }

    public interface IWeatherService
    {
        Task<WeatherObservations> GetWeatherAsync(double latitude, double longitude, DateTime dateTime);
    }
}
