namespace Tim.Twime.Models

    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

    type RideAnalysis =
        {
            Distance : float<meter>; 
            Duration : System.TimeSpan;
            Energy : float<kilogram meter^2 / second ^ 2>;
            WindEnergy : float<kilogram meter^2 / second ^ 2>;
            WindClimbEquivalent : float<meter>
            RideProfile : ProfilePoint[]
        }

