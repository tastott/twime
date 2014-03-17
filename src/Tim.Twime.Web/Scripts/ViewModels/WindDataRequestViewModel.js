define(["knockout", "jquery"], function (ko, $) {

    return function WindDataRequestViewModel(requestToken) {
        var self = this;

        self.Speed = ko.observable();
        self.Bearing = ko.observable();
        self.SiteName = ko.observable();

        self.Update = function () {

            $.getJSON("/Home/_GetWindData/" + requestToken, function (response) {

                if (response.ready) {

                    self.SiteName(response.siteName);
                    //self.Speed(response.speed);
                    //self.Bearing(response.bearing);
                }

            });
        }

        self.Update();
    };
});