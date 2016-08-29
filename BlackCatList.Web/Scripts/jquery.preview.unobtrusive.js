$(document).ready(function () {

    if (typeof FileReader === "undefined") {
        return;
    }

    $("img + input[type='file']").change(function () {
        previewImage(this);
    });

    function previewImage(input) {

        if (input.files == null || input.files[0] == null) {
            return;
        }

        var reader = new FileReader();

        reader.onload = function (e) {
            input.previousElementSibling.src = e.target.result;
        }

        reader.readAsDataURL(input.files[0]);
    }
}(jQuery));