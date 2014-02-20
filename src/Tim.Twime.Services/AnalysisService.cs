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
        private RideAnalyser _analyser;

        public AnalysisService(RideAnalyser analyser)
        {
            _analyser = analyser;
        }

        public RideAnalysis Analyse(RideAnalysisRequest request)
        {
            var analysis = _analyser.AnalyseRide(request);

            return analysis;
        }
    }
}
