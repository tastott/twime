using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Twime.Services
{
    using Models;
    using Cycling;
    using ImportExport;

    public class AnalysisService
    {
        private Importer _importer;
        private RideAnalyser _analyser;

        public AnalysisService()
        {
            _analyser = new RideAnalyser();
            _importer = new Importer();
        }

        public RideAnalysis UploadAndAnalyse(UploadedFile file)
        {
            var ride = new Ride(_importer.GetWaypointsFromGpxFile(file.Content).ToArray(), file.Filename);
            var analysis = _analyser.AnalyseRide(ride);

            return analysis;
        }
    }
}
