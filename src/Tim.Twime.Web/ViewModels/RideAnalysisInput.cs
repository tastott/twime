using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tim.Twime.Web.ViewModels
{
    public class RideAnalysisInput
    {
        public HttpPostedFileBase File { get; set; }
        public int WindSpeedMph { get; set; }
        public int WindBearingDegrees { get; set; }
        public int Mass { get; set; }
    }
}