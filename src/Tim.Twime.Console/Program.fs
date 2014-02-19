namespace Tim.Twime.Console
module Program =

    open System
//    open System.Collections
//    open System.Collections.Generic
    open System.Linq
    open Tim.Twime.Models
    open Tim.Twime.ImportExport
    open Tim.Geography.Geography
    open Tim.Twime.Cycling.CyclingPhysics
    open Tim.Twime.Cycling.RideTransformations
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
    open Newtonsoft.Json

        [<EntryPoint>]
        let main argv = 
            let file = System.IO.File.OpenRead(@"C:\users\tim\downloads\14-02-2014 PM.gpx")
            let importer = new Importer()
            let exporter = new Exporter()
            let waypoints = importer.GetWaypointsFromGpxFile(file).ToArray()
            //let smoothedWaypoints = smoothRide waypoints
            
           
            let massEx = 70.<kilogram>
            let vWindEx = 20.<mph>
            
            let legs = analyseWaypoints waypoints massEx 0.<radian> (vWindEx * mpsPerMph)
            exporter.ExportAnalysedLegsToCsvFile(legs, @"C:\users\tim\desktop\14-02-2014 PM - unsmoothed.csv")

            let maxPower = legs |> Array.maxBy(fun l -> l.PedalPower)

            let maxSpeed = legs |> Array.maxBy(fun l -> l.Speed)
            printfn "%s" (JsonConvert.SerializeObject(maxSpeed, Formatting.Indented))

            let totalDistance = legs |> Array.sumBy(fun l -> l.Distance)
            let totalDuration = legs |> Array.sumBy(fun l -> l.Duration)

            let averageSpeed = totalDistance / totalDuration
            printfn "Distance:%.1fkm\nDuration:%.1fmins\nAverage speed: %.1fkph" (float totalDistance / 1000.) (float totalDuration / 60.) (float (averageSpeed / mpsPerKph))

            printfn "%f" (float legs.[0].Distance)
            let x = System.Console.ReadLine()
            0 // return an integer exit code
