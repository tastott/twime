function RideProfileToJqPlotSeries(profile) {

    var series = $.map(profile, function (point) {
        return [[point.Distance/1000, point.Elevation]];
    });

    return series;
}