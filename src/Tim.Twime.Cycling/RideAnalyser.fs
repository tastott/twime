namespace Tim.Twime.Cycling

    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
    open Tim.Geography.Geography
    open Tim.Twime.Cycling.CyclingPhysics
    open Tim.Twime.Models

    type RideAnalyser() =
        member this.AnalyseRide (ride : Ride) =
            let massEx = 70.<kilogram>
            let vWindEx = 20.<mph>
            let legs = analyseWaypoints ride.Waypoints massEx 0.<radian> (vWindEx * mpsPerMph)
            let totalWindEnergy = legs |> Array.sumBy(fun x -> x.WindEnergy)
            {Thingy = totalWindEnergy.ToString()}

