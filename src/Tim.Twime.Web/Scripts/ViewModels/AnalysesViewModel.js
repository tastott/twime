define(["knockout", "ViewModels/AnalysisViewModel", "jqplot", "charts/tim.chartutils", "Drawing/WindWidget"],
    function (ko, AnalysisViewModel, jqplot, chartutils, ww) {

        ko.bindingHandlers.windWidget = {
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var wind = valueAccessor();
                ww.Draw(jQuery(element), wind.Speed, wind.Bearing);
            }
        };

        ko.bindingHandlers.rideProfileChart = {
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var analysis = valueAccessor();

                var originalProfileSeries = chartutils.RideProfileToJqPlotSeries(analysis.RideProfile);
                var adjustedProfileSeries = chartutils.RideProfileToJqPlotSeries(analysis.WindAdjustedRideProfile);

                var options = {
                    axes: {
                        xaxis: {
                            pad: 0,
                            label: "Distance (km)",
                            labelRenderer: jqplot.CanvasAxisLabelRenderer
                        },
                        yaxis: {
                            label: "Elevation (m)",
                            labelRenderer: jqplot.CanvasAxisLabelRenderer
                        }
                    },
                    seriesDefaults: {
                        markerOptions: {
                            show: false
                        }
                    },
                    series: [
                        {
                            fill: true,
                            fillAlpha: 0.5
                        }
                    ]
                };

                jQuery(element).empty();
                jQuery(element).jqplot([originalProfileSeries, adjustedProfileSeries], options);
            }
        };

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