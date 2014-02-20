namespace Tim.Geography

module Geography =
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
    open Tim.Units.Units
   

    
    let EarthRadius = 6371000.<meter>

    let EarthDistance (lat1 : float<radian>) (lon1 : float<radian>) (lat2 : float<radian>) (lon2 : float<radian>) =
        let x = float (lon2-lon1) * cos(float (lat1+lat2)) / 2.
        let y = float (lat2-lat1)
        sqrt (x*x + y*y) * EarthRadius

    let EarthDistanceDegrees (lat1 : float<degree>) (lon1 : float<degree>) (lat2 : float<degree>) (lon2 : float<degree>) =
        EarthDistance (DegreesToRadians lat1) (DegreesToRadians lon1) (DegreesToRadians lat2) (DegreesToRadians lon2)

    let initialBearing (lat1 : float<radian>) (lon1 : float<radian>) (lat2 : float<radian>) (lon2 : float<radian>) =
        let dLon = float(lon2 - lon1)
        let y = sin dLon * cos (float lat2)
        let x = cos (float lat1) * sin (float lat2) - sin (float lat1) * cos (float lat2) * cos dLon
        (atan2 y x) * 1.<radian>

    let initialBearingFromDegrees (lat1 : float<degree>) (lon1 : float<degree>) (lat2 : float<degree>) (lon2 : float<degree>) =
        initialBearing (DegreesToRadians lat1) (DegreesToRadians lon1) (DegreesToRadians lat2) (DegreesToRadians lon2)