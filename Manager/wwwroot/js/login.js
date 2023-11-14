$(document).ready(function () {
    $('#login').on('click', () => Login())
})

function Login() {
    var usuario = $('#user').val();
    var senha = $('#password').val();
    $.ajax({
        URL: "/Home/SetLogin",
        data: { passwrd: senha, user: usuario },
        datatype: "json",
        type:"Post",
        success: function (data) {
            alert(data.response)
        }
    })
}
