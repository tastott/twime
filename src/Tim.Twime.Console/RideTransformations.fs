module RideTransformations

    open Tim.Twime.Models

    //This doesn't work!
    let smoothRide (waypoints : Waypoint[])  =
        [| 
            for i in 0 .. waypoints.Length - 1 do
                if(i = 0) then 
                    yield waypoints.[i]
                else if(i = waypoints.Length - 1) then
                    yield waypoints.[i]
                else
                    yield {waypoints.[i] with
                            Latitude = (waypoints.[i+1].Latitude + waypoints.[i-1].Latitude) / 2.;
                            Longitude = (waypoints.[i+1].Longitude + waypoints.[i-1].Longitude) / 2.;
                            Elevation = (waypoints.[i+1].Elevation + waypoints.[i-1].Elevation) / 2. }

        |]

