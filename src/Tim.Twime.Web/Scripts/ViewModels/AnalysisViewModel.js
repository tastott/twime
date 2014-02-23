define(["knockout", "sprintf"], function (ko) {

    return function AnalysisViewModel(json) {
        var self = this;

        self.DistanceKm = (json.Distance / 1000).toFixed(1);
        self.Duration = sprintf("%02s:%02s:%02s", json.Duration.Hours, json.Duration.Minutes, json.Duration.Seconds);
        self.EnergykJ = (json.Energy / 1000).toFixed(0);
        self.WindEnergykJ = (json.WindEnergy / 1000).toFixed(0);
        self.WindClimbEquivalent = json.WindClimbEquivalent.toFixed(0);
        //self.SelectedRide = ko.observable(rides[0]);
    };
});