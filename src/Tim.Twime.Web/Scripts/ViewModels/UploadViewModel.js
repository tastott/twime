define(["knockout", "jquery", "ViewModels/WindDataRequestViewModel"], function (ko, $, WindDataRequestViewModel) {

    function progressHandlingFunction(e) {
        if (e.lengthComputable) {
            $('progress').attr({ value: e.loaded, max: e.total });
        }
    }

    return function UploadViewModel() {
        var self = this;

        self.FileGuid = ko.observable();
        self.Filename = ko.observable();
        self.WindDataRequests = ko.observableArray();
        self.WindSpeed = ko.observable();
        self.WindBearing = ko.observable();

        self.SetWind = function (speed, bearing) {
            self.WindSpeed(speed);
            self.WindBearing(bearing);
        }

        self.UploadFile = function () {

            var formData = new FormData($('form')[0]);
            $.ajax({
                url: '/Home/_Upload',  //Server script to process data
                type: 'POST',
                xhr: function () {  // Custom XMLHttpRequest
                    var myXhr = $.ajaxSettings.xhr();
                    if (myXhr.upload) { // Check if upload property exists
                        myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
                    }
                    return myXhr;
                },
                //Ajax events
                beforeSend: function () { $('progress').show(); },
                complete: function () { $('progress').hide(); },
                success: function (response) {
                    self.FileGuid(response.guid);
                    self.Filename(response.filename);
                    
                    var windDataRequests = $.map(response.windDataTokens, function (token) { return new WindDataRequestViewModel(token); });
                    self.WindDataRequests(windDataRequests);
                },
                //error: errorHandler,
                // Form data
                data: formData,
                //Options to tell jQuery not to process data or worry about content-type.
                cache: false,
                contentType: false,
                processData: false
            });
        };

    };

});