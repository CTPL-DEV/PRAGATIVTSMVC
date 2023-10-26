
window.onload = function () {
    $("#btnsave").show();
    $("#btnupdate").hide();
    $(".datepicker").datepicker({
        format: 'yyyy-mm-dd',
    }).datepicker("setDate", "0");
    $("#divrow").hide();
}
    



function showrow() {
    $("#divrow").show();
}


function Save() {
    
    //string DEVICE_NAME, string DEVICE_SERIALNUMBER, string DEVICE_MFGDATE, string SIM_ID, string CATEG_ID
    if ($("#txtname").val() != "" && $("#ddlcategories").val() != 0 && $("#ddlsims").val() != 0) {

        $.post("/Home/_DEVICE", {
            "DEVICE_NAME": $("#txtname").val(), "DEVICE_SERIALNUMBER": $("#txtdeviceserialnumber").val(), "DEVICE_MFGDATE": $("#txtmfgdata").val(), "SIM_ID": $("#ddlsims").val(), "CATEG_ID": $("#ddlcategories").val(), "DEVICE_CREATEDBY": 1
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
        if($("#txtname").val() == "")
        {
            alert("please enter devicename..");
            $("#txtname").focus();
        }
        else if ($("#ddlcategories").val() == 0) {
            alert("please select category..");
            $("#ddlcategories").focus();
        }
        else if ($("#ddlsims").val() == 0) {
            alert("please select sim..");
            $("#ddlsims").focus();
        }
    }
}

function UPDATESTATUS(STATUS, DEVICE_ID) {

    var sta = "";
    if (STATUS == 0) {
        sta = "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " + sta + " this Device")) {
        $.post("/Home/UPDATE_DEVICES_STATUS", {
            "DEVICE_STATUS": STATUS, "DEVICE_ID": DEVICE_ID,
        }, function (data) {
            console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("Device " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }

}


function EDIT(ID)
{
    $("#btnsave").hide();
    $("#btnupdate").show();
    localStorage.setItem("deviceid",null);
    localStorage.setItem("deviceid",ID);
   
    $("#divrow").show();
    $.post("/Home/_editdevice", function (data) {

        var dataT = data['data'];
       
        for (var i = 0; i < dataT.length; i++) {
            if (dataT[i].DEVICE_ID == ID) {
             
               
                $("#txtname").val(dataT[i].DEVICE_NAME);
                $("#ddlsims").val(dataT[i].SIM_ID);
                $("#txtdeviceserialnumber").val(dataT[i].DEVICE_SERIALNUMBER);
                $("#ddlcategories").val(dataT[i].CATEG_ID);
                $("#txtmfgdata").val(dataT[i].DEVICE_MFGDATE);
            }
        }

    });
}


function Update() {

 var id=  localStorage.getItem("deviceid");    
    //string DEVICE_NAME, string DEVICE_SERIALNUMBER, string DEVICE_MFGDATE, string SIM_ID, string CATEG_ID
    if ($("#txtname").val() != "" && $("#ddlcategories").val() != 0 && $("#ddlsims").val() != 0) {

        $.post("/Home/UPDATE_DEVICE", {
            "DEVICE_NAME": $("#txtname").val(), "DEVICE_SERIALNUMBER": $("#txtdeviceserialnumber").val(), "DEVICE_MFGDATE": $("#txtmfgdata").val(), "SIM_ID": $("#ddlsims").val(), "CATEG_ID": $("#ddlcategories").val(), "DEVICE_UPDATEDBY": 1,"DEVICE_ID":id
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
        if($("#txtname").val() == "")
        {
            alert("please enter devicename..");
            $("#txtname").focus();
        }
        else if ($("#ddlcategories").val() == 0) {
            alert("please select category..");
            $("#ddlcategories").focus();
        }
        else if ($("#ddlsims").val() == 0) {
            alert("please select sim..");
            $("#ddlsims").focus();
        }
    }
}
