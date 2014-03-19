using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tim.Twime.Services.Weather
{
    public class MockWeatherDataProvider : IWeatherDataProvider
    {
        public Models.WeatherObservation GetWeather(double latitude, double longitude, DateTime dateTime)
        {
            Thread.Sleep(2000);

            return new Models.WeatherObservation("Mock", "A site", 0, 0, 4, Math.PI * .25);
        }
    }
}
