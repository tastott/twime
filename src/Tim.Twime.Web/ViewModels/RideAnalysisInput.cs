using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tim.Twime.Web.ViewModels
{
    public class RideAnalysisInput
    {
        public Guid RideId { get; set; }
        public int WindSpeed { get; set; }
        public int WindBearing { get; set; }
        public int Mass { get; set; }
    }
}