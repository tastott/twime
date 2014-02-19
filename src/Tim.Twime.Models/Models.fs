﻿namespace Tim.Twime.Models

open Tim.Geography.Geography
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

type Waypoint =
        { Latitude: float<degree>;
          Longitude: float<degree>;
          Time: System.DateTime;
          Elevation: float<meter> }