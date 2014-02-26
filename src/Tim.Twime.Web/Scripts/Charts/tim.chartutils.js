define(["jquery"], function (jQuery) {

    return {
        RideProfileToJqPlotSeries: function (profile) {

            var series = jQuery.map(profile, function (point) {
                return [[point.Distance / 1000, point.Elevation]];
            });

            return series;
        }
    };

});


