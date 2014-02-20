namespace Tim.Twime.Cycling

    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
    open Tim.Geography.Geography
    open Tim.Twime.Cycling.CyclingPhysics
    open Tim.Twime.Models
    open Tim.Units.Units

    type RideAnalyser() =
        member this.AnalyseRide (request : RideAnalysisRequest) =
            let mass = request.Mass
            let vWind = MphToMps request.WindSpeed
            let windBearing = DegreesToRadians request.WindBearingDegrees
            let legs = analyseWaypoints request.Ride.Waypoints mass windBearing vWind
            let totalWindEnergy = legs |> Array.sumBy(fun x -> x.WindEnergy)
            {Thingy = totalWindEnergy.ToString()}

