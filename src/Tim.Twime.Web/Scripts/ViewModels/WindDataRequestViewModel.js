define(["knockout", "jquery", "Drawing/WindWidget"], function (ko, $) {

    return function WindDataRequestViewModel(requestToken) {
        var self = this;

        self.Wind = ko.observable(
            {
                Speed: 0,
                Bearing: 0
            });

        self.SiteName = ko.observable();
        self.Source = ko.observable();

        self.Update = function () {

            $.getJSON("/Home/_GetWindData/" + requestToken, function (response) {

                if (response.status == "success") {

                    self.SiteName(response.observation.SiteName);
                    self.Source(response.observation.Source);
                    //self.Wind().Speed = response.observation.WindSpeedMps;
                    //self.Wind().Bearing = response.observation.WindBearingRadians;
                    self.Wind({ Speed: response.observation.WindSpeed, Bearing: response.observation.WindBearing });
                    //self.Bearing(response.bearing);
                }

            });
        }

        self.Update();
    };
});