define(["jquery", "./jcanvas.min", "knockout"], function (jQuery, jcanvas, ko) {

    function draw(canvas, windSpeed, windBearing) {
        //var canvas = jQuery('#' + canvasId);
        var diameter = 36;
        var x = 150, y = 100;
        var arrowlength = diameter;

        canvas.clearCanvas();

        canvas.drawLine({
            strokeStyle: '#000',
            strokeWidth: 5,
            //rounded: true,
            endArrow: true,
            arrowRadius: 12,
            arrowAngle: 90,
            x1: x, y1: y,
            x2: x - Math.sin(windBearing) * arrowlength, y2: y + Math.cos(windBearing) * arrowlength
        });

        var radial = canvas.createGradient({
            x1: x - 7, y1: y - 7,
            x2: x - 7, y2: y - 7,
            r1: diameter/8, r2: diameter/2,
            c1: '#aaa',
            c2: '#000'
        });

        canvas.drawArc({
            fillStyle: radial,
            //strokeStyle: '#000',
            //strokeWidth:3,
            x: x, y: y,
            radius: diameter / 2
        });   

        canvas.drawText({
            fillStyle: '#fff',
            //strokeStyle: '#fff',
            //strokeWidth: 1,
            x: x, y: y,
            fontSize: 12,
            fontFamily: 'Verdana, sans-serif',
            fontStyle: 'bold',
            text: windSpeed.toFixed(0)
        });
    }

    ko.bindingHandlers.windWidget = {
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var wind = valueAccessor();
            if(wind.Speed && wind.Bearing) draw(jQuery(element), wind.Speed, wind.Bearing);
        }
    };

    return {
        Draw: draw
    };
});