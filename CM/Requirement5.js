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

QUnit.test("Startwith Tests", function (assert) {
    assert.ok("hang the dj".startsWith("hang"));
    assert.ok("hang the dj".startsWith("Hang") == false);
    assert.ok("hang the dj".startsWith("I've got a room for rent") == false);
    assert.ok("hang the dj".startsWith(""));
    assert.ok("hang the dj".startsWith("hang the"));
    assert.ok("hang the dj".startsWith("42") == false);
    assert.ok("hang the dj".startsWith({ first: "Johnny" }) == false);
    assert.ok("hang the dj".startsWith(undefined) == false);
    assert.ok("hang the dj".startsWith(null) == false);
});

QUnit.test("Endswith Tests", function (assert) {
    assert.ok("hang the dj".endsWith("dj"));
    assert.ok("hang the dj".endsWith("panic on the streets") == false);
    assert.ok("hang the dj".endsWith(""));
    assert.ok("hang the dj".endsWith("the dj"));
    assert.ok("hang the dj".endsWith("42") == false);
    assert.ok("hang the dj".endsWith({ first: "Johnny" }) == false);
});