window.onload=function(){
    $("#divrow").hide();
};

function showrow() {
    $("#divrow").show();
}


function UPDATESTATUS(STATUS, USER_ID) {

    var sta = "";
    if (STATUS == 0) {
        sta = "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " + sta + " this User")) {
        $.post("/Home/UPDATE_USER_STATUS", {
            "USER_STATUS": STATUS, "USER_ID": USER_ID,
        }, function (data) {
            console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("User " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }

}

function Save() {
 
    if ($("#txtname").val() != "" && $("#ddlcategories").val() != 0 && $("#ddlrole").val() != 0 && $("#txtemail").val() != '' && $("#txtpassword").val() != '') {
      
        $.post("/Home/_USER", {
            "CATEG_ID": $("#ddlcategories").val(), "ROLE_ID": $("#ddlrole").val(), "USER_NAME": $("#txtname").val(), "USER_EMAILID": $("#txtemail").val(), "USER_PASSWORD": $("#txtpassword").val(), "USER_MOBILENUMBER": $("#txtmobile").val(), "USER_DEVICE_IDS": $("#txtdeviceids").val(),"USER_CREATEDBY":1
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("User created successfully");
                location.reload();
                $("#divrow").hide();
            }
          

        });
    }
    else {
        if ($("#txtname").val() == "") {
            alert("Enter user name..");
            $("#txtname").focus();
        }
        else if ($("#ddlcategories").val() == 0) {

            alert("select categorie..");
            $("#ddlcategories").focus();
        }
        else if ($("#ddlrole").val() == 0) {

            alert("select role..");
            $("#ddlrole").focus();
        }

        else if ($("#txtemail").val() == 0) {

            alert("enter email..");
            $("#txtemail").focus();
        }
        else if ($("#txtpassword").val() == 0) {

            alert("enter password..");
            $("#txtpassword").focus();
        }
    }
}