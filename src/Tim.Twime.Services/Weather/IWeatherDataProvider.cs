using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.Twime.Models;

namespace Tim.Twime.Services.Weather
{
    public interface IWeatherDataProvider
    {
        WeatherObservation GetWeather(double latitude, double longitude, DateTime dateTime);
    }
}
