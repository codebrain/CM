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
            width: 800,
            opener: function (a, b, c) { window.open(a, b, c); }
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

                settings.opener(this.href, windowGroup, windowOptions);
            });
        });

        return this;
    };
}(jQuery));


// Tests

var windowHref = "";
var windowGroup = "";
var windowOptions = "";
function newOpener(href, group, options) {
    windowHref = href;
    windowGroup = group;
    windowOptions = options;
}

QUnit.test("Check plugin is chainable", function (assert) {
    assert.ok($("a").openNewWindow({ opener: newOpener }).addClass("testing"), "can be chained");
    assert.equal($("a").hasClass("testing"), true, "class was added correctly from chaining");
});

QUnit.test("Check plugin is working", function (assert) {
    $("#google").trigger("click");
    assert.equal(windowHref, "http://www.google.com/", "Url set correctly");
    assert.equal(windowGroup, "_blank", "Group set correctly");
    assert.equal(windowOptions, "channelmode=no,directories=no,fullscreen=no,location=no,menubar=no,resizable=no,scrollbars=no,status=no,titlebar=no,toolbar=no,height=600,width=800", "Options set correctly");

    $("#google-window").trigger("click");
    assert.equal(windowHref, "http://www.google.com/", "Url set correctly");
    assert.equal(windowGroup, "Disney", "Group set correctly");
    assert.equal(windowOptions, "channelmode=no,directories=no,fullscreen=no,location=no,menubar=no,resizable=no,scrollbars=no,status=no,titlebar=no,toolbar=no,height=600,width=800", "Options set correctly");

    $("#disney-window").trigger("click");
    assert.equal(windowHref, "http://www.disney.com/", "Url set correctly");
    assert.equal(windowGroup, "Disney", "Group set correctly");
    assert.equal(windowOptions, "channelmode=no,directories=no,fullscreen=no,location=no,menubar=no,resizable=no,scrollbars=no,status=no,titlebar=no,toolbar=no,height=600,width=800", "Options set correctly");
});