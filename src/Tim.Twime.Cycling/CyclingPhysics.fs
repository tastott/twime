namespace Tim.Twime.Cycling
module CyclingPhysics =

    open Tim.Twime.Models
    open Tim.Geography.Geography
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames


    type Vector2D<[<Measure>] 'u>(x : float<'u>, y : float<'u>) = 
        /// The length of the vector, computed when the object is constructed
        let length = sqrt (x*x + y*y)

        // 'this' specifies a name for the object's self identifier.
        // In instance methods, it must appear before the member name.
        member this.X = x  

        member this.Y = y

        member this.Length = length

        member this.Scale(k) = Vector2D(k * this.X, k * this.Y)

    //http://sheldonbrown.com/rinard/aero/formulas.htm

    let gravity = 9.81<meter/second^2>
    let cRollEx = 0.004
    let gradEx = 0.05
    let airDensityEx = 1.226<kilogram/meter^3>
    let cWindEx = 0.5
    let areaEx = 0.5<meter^2>

    ///Rolling resistance force. cRoll = coefficient of rolling resistance.
    let fRoll cRoll (mass : float<kilogram>) = cRoll * mass * gravity

    ///Force due to gravity on a gradient. 0 gradient is flat, 1 gradient is straight up. mass in kg.
    let fGrad gradient (mass : float<kilogram>) = gradient * mass * gravity

    ///Force due to air resistance. airDensity in kg/m^3. cWind = coefficient of air resistance. area in m^2. vWind = bike velocity + wind velocity in m/s.
    let fWind (airDensity : float<kilogram/meter^3>) cWind (area : float<meter^2>) (vWindBike : float<meter/second>) = airDensity * cWind * area * 0.5 * vWindBike * vWindBike

    let fPedal cRoll mass gradient airDensity cWind area vWindBike (acc : float<meter/second^2>) = 
        (mass * acc) + (fRoll cRoll mass) + (fGrad gradient mass) + (fWind airDensity cWind area vWindBike)

    let pPedal cRoll mass gradient airDensity cWind area vWindBike acc (vBike : float<meter/second>) =
        (fPedal cRoll mass gradient airDensity cWind area vWindBike acc) * vBike

        
    type SimpleCyclingModel(windBearing, windSpeed : float<meter/second>, airDensity, cWind, area, cRoll) =
        interface ICyclingModel with   
            member this.AirForce bearing speed = this._AirForce bearing speed  
            member this.WindForce bearing speed = this._AirForce bearing speed - fWind airDensity cWind area speed
            member this.RollForce mass = fRoll cRoll mass
            member this.ClimbForce mass gradient = fGrad gradient mass

        member private this._AirForce bearing speed =
            let vWind = (cos (float (windBearing - bearing))) * windSpeed
            fWind airDensity cWind area (speed - vWind)



//    type AnalysedLeg(leg : Leg, legBefore : Leg, cRoll, mass, airDensity, cWind, area, windBearing : float<radian>, windSpeed : float<meter/second>) =
//        inherit Leg(leg.Start, leg.Finish)
//        member this.LegBefore = legBefore
//        member this.Acceleration = (this.Speed - legBefore.Speed) / this.Duration
//        member this.AirForce = 
//            let vWind = (cos (float (windBearing - this.Bearing))) * windSpeed
//            fWind airDensity cWind area (this.Speed - vWind)
//        member this.WindForce = this.AirForce - fWind airDensity cWind area this.Speed
//        member this.RollForce = fRoll cRoll mass
//        member this.ClimbForce = fGrad this.Gradient mass
//        member this.AccelerationForce = mass * this.Acceleration
//        member this.PedalForce = this.AirForce + this.RollForce + this.ClimbForce + this.AccelerationForce
//        member this.PedalEnergy = this.PedalForce * this.Distance
//        member this.PedalPower = this.PedalForce * this.Speed
//        member this.WindPower = this.WindForce * this.Speed
//        member this.WindEnergy = this
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

    let analyseWaypoints (waypoints : Waypoint[]) mass windBearing windSpeed = 
        let legs = [| for i in 1 .. waypoints.Length - 1 do
                        yield new Leg(waypoints.[i-1], waypoints.[i]) |]

        let model = new SimpleCyclingModel(windBearing, windSpeed, airDensityEx, cWindEx, areaEx, cRollEx)

        [| for i in 1 .. legs.Length - 1 do
                        yield new AnalysedLeg(legs.[i], legs.[i-1], mass, model) |]
                        
                        
   
