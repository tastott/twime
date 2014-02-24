define(["knockout", "ViewModels/AnalysisViewModel", ], function (ko, AnalysisViewModel) {

    return function AnalysesViewModel(rides) {
        var self = this;

        self.Rides = ko.observableArray(rides);

        self.Analysis = ko.observable();

        self.LoadAnalysis = function (id) {
            $.getJSON("/Home/GetAnalysis/" + id, function (json) {
                //var analysisViewModel = new AnalysisViewModel(json);
                //self.Analysis(analysisViewModel);
                self.Analysis(json);
            });
        };
    };
});