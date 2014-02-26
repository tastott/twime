namespace Tim.Twime.Models

    open Tim.Units.Units
    open Tim.Geography.Geography
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

    type Wind =
        { Speed : float<meter/second>;
          Bearing : float<radian>; }

    type RideAnalysisRequest =
        { Ride : Ride;
          Wind : Wind;
          Mass : float<kilogram> }

