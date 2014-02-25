requirejs.config({
    baseUrl: "../scripts",
    paths: {
        "jquery": "jquery-1.8.2.min",

        // WORKAROUND : jQuery plugins + shims
        "jqplot.core": "charts/jquery-jqplot",
        "jqplot": "jqplot.module"
    },
    shim: {
        "jqplot.core": { deps: ["jquery"] },
        "jqplot": { deps: ["jqplot.core"] }
    }
});