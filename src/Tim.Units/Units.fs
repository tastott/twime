namespace Tim.Units
   
module Units =

    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

    [<Measure>] type kph
    [<Measure>] type mph

    let KphToMps (kph : float<kph>) = kph * 1.<meter/(second kph)>/3.6
    let MphToMps (mph : float<mph>) = mph * 8.<meter/(second mph)>/18.

    [<Measure>] type watt = kilogram meter^2 / second ^ 3

    [<Measure>] type radian
    [<Measure>] type degree

    let DegreesToRadians (degrees : float<degree>) = degrees * System.Math.PI / 180.<degree/radian>
