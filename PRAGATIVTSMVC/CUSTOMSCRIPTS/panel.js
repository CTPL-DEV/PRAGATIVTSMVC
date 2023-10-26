window.onload = function () {
    $("#divrow").hide();
}
function showrow() {
    $("#divrow").show();
}
function Save() {

    if ($("#txtpanelname").val() != "") {
        $.post("/Home/CREATE_PANEL", {
            "PANEL_NAME": $("#txtpanelname").val()
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("panel created successfully..");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {
        if ($("#txtname").val() == "") {
            alert("Enter Panel Name..");
            $("#txtname").focus();
        }
    }
}
function UPDATESTATUS(STATUS, PANEL_ID) {

    var sta = "";
    if (STATUS == 0) {
        sta = "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " + sta + " this category")) {
        $.post("/Home/UPDATE_PANEL_STATUS", {
            "PANEL_STATUS": STATUS, "PANEL_ID": PANEL_ID,
        }, function (data) {
            console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("panel " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }

}