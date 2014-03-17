define(["knockout", "jquery", "Drawing/WindWidget"], function (ko, $) {

    return function WindDataRequestViewModel(requestToken) {
        var self = this;

        self.Wind = ko.observable(
            {
                Speed: 0,
                Bearing: 0
            });

        self.SiteName = ko.observable();

        self.Update = function () {

            $.getJSON("/Home/_GetWindData/" + requestToken, function (response) {

                if (response.status == "success") {

                    self.SiteName(response.observation.SiteName);
                    
                    //self.Wind().Speed = response.observation.WindSpeedMps;
                    //self.Wind().Bearing = response.observation.WindBearingRadians;
                    self.Wind({ Speed: response.observation.WindSpeedMps, Bearing: response.observation.WindBearingRadians });
                    //self.Bearing(response.bearing);
                }

            });
        }

        self.Update();
    };
});