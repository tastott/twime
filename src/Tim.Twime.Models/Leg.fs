namespace Tim.Twime.Models

    open Tim.Units.Units
    open Tim.Geography.Geography
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

    type ICyclingModel =
        abstract member AirForce : bearing : float<radian> -> speed : float<meter/second> -> float<kilogram meter/second^2>
        abstract member WindForce : bearing : float<radian> -> speed: float<meter/second> -> float<kilogram meter/second^2>
        abstract member RollForce : mass: float<kilogram> -> float<kilogram meter/second^2>
        abstract member ClimbForce : mass : float<kilogram> -> gradient : float -> float<kilogram meter/second^2>

    type LegRecord =
        {
            Start : Waypoint;
            Finish : Waypoint;
            Duration : float<second>;
            Distance : float<meter>;
            Speed : float<meter/second>;
            Climb: float<meter>;
            Bearing : float<radian>;
            Gradient : float;
        }

    type AnalysedLegRecord =
        {
            Leg : LegRecord;
            LegBefore : LegRecord;
            Acceleration : float<meter/second>;
            AirForce : float<kilogram meter/second^2>;
            WindForce : float<kilogram meter/second^2>;
            RollForce : float<kilogram meter/second^2>;
            ClimbForce : float<kilogram meter/second^2>;
            AccelerationForce : float<kilogram meter/second^2>;
            PedalForce : float<kilogram meter/second^2>;
            PedalEnergy : float<kilogram meter^2/second^2>;
            PedalPower : float<kilogram meter^2/second^3>;
            WindPower : float<kilogram meter^2/second^3>;
            WindEnergy : float<kilogram meter^2/second^2>;
        }

    type Leg(start : Waypoint, finish: Waypoint) =
        member this.Start = start
        member this.Finish = finish
        member this.Duration = (finish.Time - start.Time).TotalSeconds * 1.<second>
        member this.Distance = EarthDistanceDegrees start.Latitude start.Longitude finish.Latitude finish.Longitude
        member this.Speed = this.Distance / this.Duration
        member this.Climb = finish.Elevation - start.Elevation
        member this.Bearing = initialBearingFromDegrees start.Latitude start.Longitude finish.Latitude finish.Longitude
        member this.Gradient = this.Climb / this.Distance

    //type AnalysedLeg(leg : Leg, legBefore : Leg, cRoll, mass, airDensity, cWind, area, windBearing : float<radian>, windSpeed : float<meter/second>) =
    type AnalysedLeg(leg : Leg, legBefore : Leg, mass : float<kilogram>, model : ICyclingModel) =
        inherit Leg(leg.Start, leg.Finish)
        member this.LegBefore = legBefore
        member this.Acceleration = (this.Speed - legBefore.Speed) / this.Duration
        member this.AirForce = model.AirForce this.Bearing this.Speed
        member this.WindForce = model.WindForce this.Bearing this.Speed
        member this.RollForce = model.RollForce mass
        member this.ClimbForce = model.ClimbForce mass this.Gradient
        member this.AccelerationForce = mass * this.Acceleration
        member this.PedalForce = this.AirForce + this.RollForce + this.ClimbForce + this.AccelerationForce
        member this.PedalEnergy = this.PedalForce * this.Distance
        member this.PedalPower = this.PedalForce * this.Speed
        member this.WindPower = this.WindForce * this.Speed
        member this.WindEnergy = this.WindForce * this.Distance
//        let this.PrettyPrint = 
//            printfn @"\n\nAt:%f,%f
//                     Distance:%.1fm
//                     Gradient:%.2f 
//                     Duration: %.1fs
//                     Speed: %.1fkph = %.1fm/s
//                     Acceleration: %.1fm/s^2
//
//                     Power:%.0fW" 
//                (float this.Start.Latitude) (float this.Start.Longitude) 
//                (float this.Distance) this.Gradient (float this.Duration) 
//                (float (this.Speed / mpsPerKph)) (float this.Speed)
//                (float this.Acceleration)
//                (float this.PedalPower)