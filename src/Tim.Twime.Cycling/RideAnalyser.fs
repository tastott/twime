namespace Tim.Twime.Cycling

    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
    open Tim.Geography.Geography
    open Tim.Twime.Cycling.CyclingPhysics
    open Tim.Twime.Models
    open Tim.Units.Units

    type RideAnalyser() =
        member this.AnalyseRide (request : RideAnalysisRequest) =
            let vWind = MphToMps request.WindSpeed
            let windBearing = DegreesToRadians request.WindBearingDegrees
            let legs = AnalyseWaypoints request.Ride.Waypoints request.Mass windBearing vWind
            let totalWindEnergy = legs |> Array.sumBy(fun x -> x.WindEnergy)
            {
                WindSpeed = vWind;
                WindBearing = windBearing;
                Mass = request.Mass;
                Distance = legs |> Array.sumBy(fun x -> x.Distance);
                Duration = new System.TimeSpan(0, 0, (int (legs |> Array.sumBy(fun x -> x.Duration))));
                Energy = legs |> Array.sumBy(fun x -> x.PedalEnergy);
                WindEnergy = legs |> Array.sumBy(fun x -> x.WindEnergy);
                WindClimbEquivalent = legs |> Array.sumBy(fun x -> x.WindClimbEquivalent);
                RideProfile = GetRideProfile legs 200.<meter>;
                WindAdjustedRideProfile = GetWindAdjustedRideProfile legs 200.<meter>
            }

