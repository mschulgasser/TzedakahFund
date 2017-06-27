$(() => {
    var userExists = false;
    $(".form-control").on('keydown', function () {
        $("#register").prop('disabled', !isValid());
    });
    $("#email").on('keydown', function () {
        $.get("/home/userexists", { email: $("#email").val() }, function (data) {
            userExists =  data.userExists;
        });
    });

    function isValid() {
        return $("#first-name").val() != "" &&
               $("#last-name").val() != "" && $("#email").val() != "" &&
               $("#password1").val() != "" && $("#password1").val() === $("#password2").val() &&
               !userExists;
    }
});