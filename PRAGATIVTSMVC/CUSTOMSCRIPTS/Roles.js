

window.onload=function(){
    $("#divrow").hide();
}


function showrow() {
    $("#divrow").show();
}

function Save() {

    if ($("#txtname").val() != "")
    {
        $.post("/Home/_ROLES", {
            "ROLE_NAME": $("#txtname").val(),
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("Role created successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {
        if($("#txtname").val() == "")
        {
            alert("Enter Role name..");
            $("#txtname").focus();
        }
    }
   
}
function UPDATESTATUS(STATUS, ROLE_ID) {
    
    var sta = "";
    if (STATUS == 0) {
        sta = "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " + sta + " this Role")) {
        $.post("/Home/UPDATE_ROLE_STATUS", {
            "ROLE_STATUS": STATUS, "ROLE_ID": ROLE_ID,
        }, function (data) {
            console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("Role " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }

}