namespace Tim.Twime.Models

    open Tim.Units.Units
    open Tim.Geography.Geography
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

    type RideAnalysisRequest =
        { Ride : Ride;
          WindSpeed : float<mph>;
          WindBearingDegrees : float<degree>;
          Mass : float<kilogram> }

