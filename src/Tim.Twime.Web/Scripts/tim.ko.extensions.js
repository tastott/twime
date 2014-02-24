require(["knockout","sprintf"], function (ko) {
    ko.bindingHandlers.numericText = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            var precision = ko.utils.unwrapObservable(allBindingsAccessor().precision);
            var units = ko.utils.unwrapObservable(allBindingsAccessor().units) || "";

            var formattedValue = value.toFixed(precision) + units;

            ko.bindingHandlers.text.update(element, function () { return formattedValue; });
        },
        defaultPrecision: 1
    };

    ko.bindingHandlers.timeSpanText = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            //var precision = ko.utils.unwrapObservable(allBindingsAccessor().precision);
            var formattedValue = sprintf("%02s:%02s:%02s", value.Hours, value.Minutes, value.Seconds);

            ko.bindingHandlers.text.update(element, function () { return formattedValue; });
        },
        defaultPrecision: 1
    };
});