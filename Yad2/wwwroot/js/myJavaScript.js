
function UserChack() {
    console.log("mo")
    var username = $("#userName").val();
    $("#Status").html("Checking...");
    $.ajax({
        type: 'get',
        url: '/User/CheckUserNameAvailability',
        dataType: 'json',
        data: { id: $("#userName").val() },
        success: function (data) {
            if (data == 0) {
                $("#Status").html('<font color="Green">Available !.</font>');
                $("#userName").css("border-color", "Green");
                console.log()
            }
            else {
                $("#Status").html('<font color="Red">That name is taken.Try another.</font>');
                $("#userName").css("border-color", "Red");
            }
        }

    })
}



