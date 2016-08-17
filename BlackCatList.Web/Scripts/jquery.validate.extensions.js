/*!
 * jQuery Validation Plugin phone extension
 */
(function ($) {

    var reversePhoneRegEx = /^(\d+\s?(x|\.txe?)\s?)?((\)(\d+[\s\-\.]?)?\d+\(|\d+)[\s\-\.]?)*(\)([\s\-\.]?\d+)?\d+\+?\((?!\+.*)|\d+)(\s?\+)?$/i;

    $.validator.addMethod("phone", function (value, element) {

        if (this.optional(element)) {
            return true;
        }

        var reverseValue = $.trim(value).split("").reverse().join("");
        var match = reversePhoneRegEx.exec(reverseValue);

        return match
            && match.index === 0
            && match[0].length === value.length;
    });

    $.validator.unobtrusive.adapters.addBool("phone");
}(jQuery));