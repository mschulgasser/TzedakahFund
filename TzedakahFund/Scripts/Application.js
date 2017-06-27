$(function () {
    $("#category").on('change', function () {
        checkValidation();
    });
    $("#amount").on('keyup', function () {
        checkValidation();
    });

    function checkValidation() {
        $("#submit").prop("disabled", !($("#category").val() != 0 && $("#amount").val() > 0));
    }
});