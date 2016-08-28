$(document).ready(function () {
    $("[data-autocomplete-url]").each(function () {
        (function ($this) {
            var url = $this.data("autocomplete-url");
            $this.autocomplete({
                autoFocus: true,
                minLength: 0,
                source: function (request, response) {
                    var withId = $this.data("autocomplete-with");
                    if (withId != null) {
                        request[withId] = document.getElementById(withId).value;
                    }
                    $.post(url, request, response);
                }
            }).one("focus", function () {
                $this.autocomplete("search", "");
                $this.focus(function () {
                    if ($this.val().length > 0) {
                        $this.autocomplete("search", "");
                    }
                })
            });
        }($(this)));
    });
});