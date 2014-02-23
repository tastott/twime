function RideProfileToJqPlotSeries(profile) {

    var series = $.map(profile, function (point) {
        return [[point.Distance, point.Elevation]];
    });

    return series;
}