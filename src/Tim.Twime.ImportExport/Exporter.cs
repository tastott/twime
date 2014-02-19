using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Twime.ImportExport
{
    using Models;

    public class Exporter
    {
        public void ExportLegsToCsvFile(IEnumerable<Leg> legs, string filepath)
        {
            var mappings = new Dictionary<string, Func<Leg, string>>
            {
                {"Time", l => l.Start.Time.ToString("yyyy-MM-dd HH:mm:ss")},
                {"Latitude", l => l.Start.Latitude.ToString()},
                {"Longitude",l => l.Start.Longitude.ToString()},
                {"Elevation", l => l.Start.Elevation.ToString()},
                {"Distance", l => l.Distance.ToString()},
                {"Speed (m/s)",l => l.Speed.ToString()}
            };

            using (var file = System.IO.File.CreateText(filepath))
            {
                file.WriteLine(String.Join(",", mappings.Keys));

                foreach (var leg in legs)
                {
                    var values = mappings.Values.Select(m => m(leg));

                    file.WriteLine(String.Join(",", values));
                }
            }
        }

        public void ExportAnalysedLegsToCsvFile(IEnumerable<AnalysedLeg> legs, string filepath)
        {
            var mappings = new Dictionary<string, Func<AnalysedLeg, string>>
            {
                {"Time", l => l.Start.Time.ToString("yyyy-MM-dd HH:mm:ss")},
                {"Latitude", l => l.Start.Latitude.ToString()},
                {"Longitude",l => l.Start.Longitude.ToString()},
                {"Elevation", l => l.Start.Elevation.ToString()},
                {"Distance", l => l.Distance.ToString()},
                {"Speed (m/s)",l => l.Speed.ToString()},
                {"Pedal Energy (J)", l => l.PedalEnergy.ToString()},
                {"Wind Energy (J)", l => l.WindEnergy.ToString()}
            };

            using (var file = System.IO.File.CreateText(filepath))
            {
                file.WriteLine(String.Join(",", mappings.Keys));

                foreach (var leg in legs)
                {
                    var values = mappings.Values.Select(m => m(leg));

                    file.WriteLine(String.Join(",", values));
                }
            }
        }
    }
}
