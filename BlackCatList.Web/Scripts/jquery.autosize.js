/*!
 * jQuery Autoresize Plugin
 */
(function ($) {

    $("textarea").on("input", function () {
        resize(this);
    });

    function resize(element) {
        $(element).css({
            "height": "auto",
            "overflow": "hidden"
        }).height(element.scrollHeight);
    }
}(jQuery));