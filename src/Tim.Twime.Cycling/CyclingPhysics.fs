namespace Tim.Twime.Cycling
module CyclingPhysics =

    open Tim.Twime.Models
    open Tim.Geography.Geography
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

    //http://sheldonbrown.com/rinard/aero/formulas.htm

    let gravity = 9.81<meter/second^2>
    let cRollEx = 0.004
    let gradEx = 0.05
    let airDensityEx = 1.226<kilogram/meter^3>
    let cWindEx = 0.5
    let areaEx = 0.5<meter^2>

    ///Rolling resistance force. cRoll = coefficient of rolling resistance.
    let FRoll cRoll (mass : float<kilogram>) = cRoll * mass * gravity

    ///Force due to gravity on a gradient. 0 gradient is flat, 1 gradient is straight up. mass in kg.
    let FGrad gradient (mass : float<kilogram>) = gradient * mass * gravity

    ///Force due to air resistance. airDensity in kg/m^3. cWind = coefficient of air resistance. area in m^2. vWind = bike velocity + wind velocity in m/s.
    let FWind (airDensity : float<kilogram/meter^3>) cWind (area : float<meter^2>) (vWindBike : float<meter/second>) = airDensity * cWind * area * 0.5 * vWindBike * vWindBike

    let FPedal cRoll mass gradient airDensity cWind area vWindBike (acc : float<meter/second^2>) = 
        (mass * acc) + (FRoll cRoll mass) + (FGrad gradient mass) + (FWind airDensity cWind area vWindBike)

    let PPedal cRoll mass gradient airDensity cWind area vWindBike acc (vBike : float<meter/second>) =
        (FPedal cRoll mass gradient airDensity cWind area vWindBike acc) * vBike

        
    type SimpleCyclingModel(windBearing, windSpeed : float<meter/second>, airDensity, cWind, area, cRoll) =
        interface ICyclingModel with   
            member this.AirForce bearing speed = this._AirForce bearing speed  
            member this.WindForce bearing speed = this._WindForce bearing speed
            member this.RollForce mass = FRoll cRoll mass
            member this.ClimbForce mass gradient = FGrad gradient mass
            member this.WindClimbEquivalent mass bearing speed distance = 
                (this._WindForce bearing speed) * distance / (mass * gravity)

        member private this._AirForce bearing speed =
            let vWind = (cos (float (windBearing - bearing))) * windSpeed
            FWind airDensity cWind area (speed - vWind)

        member private this._WindForce bearing speed = 
            this._AirForce bearing speed - FWind airDensity cWind area speed

    let AnalyseWaypoints (waypoints : Waypoint[]) mass windBearing windSpeed = 
        let legs = [| for i in 1 .. waypoints.Length - 1 do
                        yield new Leg(waypoints.[i-1], waypoints.[i]) |]

        let model = new SimpleCyclingModel(windBearing, windSpeed, airDensityEx, cWindEx, areaEx, cRollEx)

        [| for i in 1 .. legs.Length - 1 do
                        yield new AnalysedLeg(legs.[i], legs.[i-1], mass, model) |]
             
    let GetRideProfile (legs : #Leg[]) (distanceInterval : float<meter>) = 
        let nextInterval = ref 0
        let totalDistance = ref 0.<meter>

        [| 
            for i in 0 .. legs.Length - 1 do
                totalDistance := !totalDistance + legs.[i].Distance
                let nextDistance = float !nextInterval * distanceInterval

                if(!totalDistance >= nextDistance || i = legs.Length - 1) then
                    nextInterval := !nextInterval + 1
                    let adjustedElevation = legs.[i].Start.Elevation + ((!totalDistance - nextDistance) / legs.[i].Distance) * legs.[i].Climb
                    yield {Distance = nextDistance; Elevation = adjustedElevation}  
        |]

    let GetWindAdjustedRideProfile (legs : AnalysedLeg[]) (distanceInterval : float<meter>) = 
        let nextInterval = ref 0
        let totalDistance = ref 0.<meter>
        let totalElevation = ref legs.[0].Start.Elevation

        [| 
            for i in 0 .. legs.Length - 1 do
                totalDistance := !totalDistance + legs.[i].Distance
                
                let nextDistance = float !nextInterval * distanceInterval

                if(!totalDistance >= nextDistance || i = legs.Length - 1) then
                    nextInterval := !nextInterval + 1
                    let adjustedElevation = !totalElevation + ((!totalDistance - nextDistance) / legs.[i].Distance) * (legs.[i].Climb + legs.[i].WindClimbEquivalent)
                    yield {Distance = nextDistance; Elevation = adjustedElevation}  

                totalElevation := !totalElevation + legs.[i].Climb + legs.[i].WindClimbEquivalent
        |]