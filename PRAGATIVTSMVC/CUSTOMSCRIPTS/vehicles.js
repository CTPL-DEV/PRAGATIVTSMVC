
window.onload=function()
{
    $("#btnsave").show();
    $("#btnupdate").hide();
    $("#divrow").hide();
}



function showrow() {
    $("#divrow").show();
}

function Save() {
    if ($("#txtvehiclenumber").val() != "" && $("#ddldevices").val() != 0 && $("#ddlcategories").val() != 0) {
        $.post("/Home/_VEHICLES", {
            "VEHICLE_REGNUMBER": $("#txtvehiclenumber").val(), "DEVICE_ID": $("#ddldevices").val(), "CATEG_ID": $("#ddlcategories").val(), "VEHICLE_ZONE": $("#txtzone").val(), "VEHICLE_MODELNAME": $("#txtvehiclemodelname").val(), "VEHICLE_MAXSPEED": $("#txtvehiclemaxspeed").val(), "VEHICLE_MILLAGE": $("#txtvehiclemillage").val(), "VEHICLE_CREATEDBY": 1
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("device created successfully");
                location.reload();
                $("#divrow").hide();
            }
        });
    }
    else {
        if ($("#txtvehiclenumber").val() == "") {
            alert("please enter vehicle number..");
            $("#txtvehiclenumber").focus();
        }
        else if ($("#ddldevices").val() == 0) {
            alert("please select device..");
            $("#ddldevices").focus();
        }
        else if ($("#ddlcategories").val() == 0) {
            alert("please select category..");
            $("#ddlcategories").focus();
        }
    }
}


function UPDATESTATUS(STATUS, VEHICLE_ID) {

    var sta = "";
    if (STATUS == 0) {
        sta = "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " + sta + " this Vehicle")) {
        $.post("/Home/UPDATE_VEHICLE_STATUS", {
            "VEHICLE_STATUS": STATUS, "VEHICLE_ID": VEHICLE_ID,
        }, function (data) {
            console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("Vehicle " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }

}

function EDIT(ID) {
    $("body").scrollTop(0);
    $("#btnsave").hide();
    $("#btnupdate").show();
    localStorage.setItem("vehicleid", null);
    localStorage.setItem("vehicleid", ID);

    $("#divrow").show();

    $.post("/Home/_editvehicle", function (data) {
        var dataT = data['data'];
        for (var i = 0; i < dataT.length; i++) {
            if (dataT[i].VEHICLE_ID == ID) {
              
                $("#txtvehiclenumber").val(dataT[i].VEHICLE_REGNUMBER);
                $("#ddldevices").val(dataT[i].DEVICE_ID );
                $("#txtvehiclemaxspeed").val(dataT[i].VEHICLE_MAXSPEED);
                $("#txtvehiclemodelname").val(dataT[i].VEHICLE_MODELNAME);
                $("#txtzone").val(dataT[i].VEHICLE_ZONE);
                $("#txtvehiclemillage").val(dataT[i].VEHICLE_MILLAGE);
                $("#ddlcategories").val(dataT[i].CATEG_ID);
            }
        }

    });
}



function Update() {
    var id = localStorage.getItem("vehicleid");
    
    if ($("#txtvehiclenumber").val() != "" && $("#ddldevices").val() != 0 && $("#ddlcategories").val() != 0) {
        $.post("/Home/UPDATE_VEHICLES", {
            "VEHICLE_REGNUMBER": $("#txtvehiclenumber").val(), "DEVICE_ID": $("#ddldevices").val(), "CATEG_ID": $("#ddlcategories").val(), "VEHICLE_ZONE": $("#txtzone").val(), "VEHICLE_MODELNAME": $("#txtvehiclemodelname").val(), "VEHICLE_MAXSPEED": $("#txtvehiclemaxspeed").val(), "VEHICLE_MILLAGE": $("#txtvehiclemillage").val(), "VEHICLE_UPDATEDBY": 1, "VEHICLE_ID": id
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("device updated successfully");
                location.reload();
                $("#divrow").hide();
            }
        });
    }
    else {
        if ($("#txtvehiclenumber").val() == "") {
            alert("please enter vehicle number..");
            $("#txtvehiclenumber").focus();
        }
        else if ($("#ddldevices").val() == 0) {
            alert("please select device..");
            $("#ddldevices").focus();
        }
        else if ($("#ddlcategories").val() == 0) {
            alert("please select category..");
            $("#ddlcategories").focus();
        }
    }

}