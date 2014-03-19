using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Twime.Services.Weather
{
    using Models;

    public class WeatherService
    {
        private IEnumerable<IWeatherDataProvider> _providers;

        private IDictionary<Guid, Func<WeatherObservation>> _requests;

        public WeatherService(IEnumerable<IWeatherDataProvider> providers)
        {
            _providers = providers;
            _requests = new Dictionary<Guid, Func<WeatherObservation>>();
        }

        public IEnumerable<Guid> GenerateWindDataRequestsForRide(Models.Ride ride)
        {
            var waypoint = ride.Waypoints.First();

            foreach (var provider in _providers)
            {
                var guid = Guid.NewGuid();
                Func<WeatherObservation> request = () => provider.GetWeather(waypoint.Latitude, waypoint.Longitude, waypoint.Time);
                _requests[guid] = request;

                yield return guid;
            }
        }

        public Models.Assertion<Models.WeatherObservation> GetWindData(Guid token)
        {
            Func<WeatherObservation> request;
            WeatherObservation weather;

            if (!_requests.TryGetValue(token, out request)) return new Assertion<WeatherObservation>("Request not found");

            try
            {
                weather = request();
            }
            catch (Exception ex)
            {
                return new Assertion<WeatherObservation>("Error executing request");
            }

            return new Assertion<WeatherObservation>(weather);
        }
    }
}
