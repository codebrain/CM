/// <reference path="lib/he.js" />
/// <reference path="lib/jquery.js" />

// There are a variety of ways to implement this function

function replacenbsp(input) {
    var temp = he.encode(input, {
        'useNamedReferences': true
    });
    return temp.replace(/&nbsp;/gi, " ");
}

if (!String.prototype.stripHtmlWithDom) {
    String.prototype.stripHtmlWithDom = function () {
        var elem = document.createElement("DIV");
        elem.innerHTML = this;
        var unencoded = elem.textContent || elem.innerText || "";
        return replacenbsp(unencoded);
    };
}

if (!String.prototype.stripHtmlWithRegEx) {
    String.prototype.stripHtmlWithRegEx = function () {
        return this.replace(/<[^>]*>?/g, '');
    };
}

if (!String.prototype.stripHtmlWithJQuery) {
    String.prototype.stripHtmlWithJQuery = function () {
        var unencoded = $("<div>").html(this).text();
        return replacenbsp(unencoded);
    };
}

// Tests

QUnit.test("Strip HTML Tests - using DOM", function (assert) {
    assert.equal("<p>Shoplifters of the World <em>Unite</em>!</p>".stripHtmlWithDom(), "Shoplifters of the World Unite!");
    assert.equal("1 &lt; 2".stripHtmlWithDom(), "1 &lt; 2");
});

QUnit.test("Strip HTML Tests - using RegEx", function (assert) {
    assert.equal("<p>Shoplifters of the World <em>Unite</em>!</p>".stripHtmlWithRegEx(), "Shoplifters of the World Unite!");
    assert.equal("1 &lt; 2".stripHtmlWithRegEx(), "1 &lt; 2");
});

QUnit.test("Strip HTML Tests - using jQuery", function (assert) {
    assert.equal("<p>Shoplifters of the World <em>Unite</em>!</p>".stripHtmlWithJQuery(), "Shoplifters of the World Unite!");
    assert.equal("1 &lt; 2".stripHtmlWithJQuery(), "1 &lt; 2");
});