namespace Tim.Twime.Models

open Tim.Units.Units
open Tim.Geography.Geography
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

type WeatherObservation = 
    {   Source : string;
        SiteName : string;
        Latitude : float<degree>;
        Longitude : float<degree>;
        WindSpeed : float<meter/second>;
        WindBearing : float<radian>; }
