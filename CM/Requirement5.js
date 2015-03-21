if (!String.prototype.startsWith) {
    String.prototype.startsWith = function(input) {
        return this.indexOf(input) == 0;
    };
}

if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (input) {
        return input === "" || this.indexOf(input) == (this.length - input.length);
    };
}

// Tests

QUnit.test("Startwith Tests", function () {
    ok("hang the dj".startsWith("hang"));
    ok("hang the dj".startsWith("Hang") == false);
    ok("hang the dj".startsWith("I've got a room for rent") == false);
    ok("hang the dj".startsWith(""));
    ok("hang the dj".startsWith("hang the"));
    ok("hang the dj".startsWith("42") == false);
    ok("hang the dj".startsWith({ first: "Johnny" }) == false);
    ok("hang the dj".startsWith(undefined) == false);
    ok("hang the dj".startsWith(null) == false);
});

QUnit.test("Endswith Tests", function () {
    ok("hang the dj".endsWith("dj"));
    ok("hang the dj".endsWith("panic on the streets") == false);
    ok("hang the dj".endsWith(""));
    ok("hang the dj".endsWith("the dj"));
    ok("hang the dj".endsWith("42") == false);
    ok("hang the dj".endsWith({ first: "Johnny" }) == false);
});