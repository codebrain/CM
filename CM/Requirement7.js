/// <reference path="lib/he.js" />
/// <reference path="lib/jquery.js" />

(function ($) {
    $.fn.openNewWindow = function (options) {

        // http://www.w3schools.com/jsref/met_win_open.asp
        var settings = $.extend({
            channelmode: "no",
            directories: "no",
            fullscreen: "no",
            location: "no",
            menubar: "no",
            resizable: "no",
            scrollbars: "no",
            status: "no",
            titlebar: "no",
            toolbar: "no",
            top: undefined,
            left: undefined,
            height: 600,
            width: 800
        }, options);

        this.filter("a").each(function () {
            $(this).click(function (e) {
                e.preventDefault();
                var windowGroup = $(this).data("window-group") || "_blank";
                var windowOptions = "channelmode=" + settings.channelmode +
                                    ",directories=" + settings.directories +
                                    ",fullscreen=" + settings.fullscreen +
                                    ",location=" + settings.location +
                                    ",menubar=" + settings.menubar +
                                    ",resizable=" + settings.resizable +
                                    ",scrollbars=" + settings.scrollbars +
                                    ",status=" + settings.status +
                                    ",titlebar=" + settings.titlebar +
                                    ",toolbar=" + settings.toolbar +
                                    (settings.top != undefined ? ",top=" + settings.top : "") +
                                    (settings.left != undefined ? ",left=" + settings.left : "") +
                                    (settings.height != undefined ? ",height=" + settings.height : "") +
                                    (settings.width != undefined ? ",width=" + settings.width : "");

                window.open(this.href, windowGroup, windowOptions);
            });
        });

        return this;
    };
}(jQuery));

// Usage
$(function() {
    $("a").openNewWindow();
});