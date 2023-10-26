
window.onload = function () {
   
    $("#btnsave").show();
    $("#btnupdate").hide();
    $("#divrow").hide();
    $("#ddlcategories").select3();
    //$('#tblSims').DataTable({
    //    dom: 'Bfrtip',
    //    buttons: [
    //        //'copy', 'csv', 'excel', 'pdf', 'print'
    //        'excel', 'pdf', 'print'
    //    ]
    //});
    //var categid = localStorage.getItem("USERS_CATEG_ID");
    ////$("#load").show();
    //$("#tbldevices>thead>tr").append("<th>Actions</th>");
    //if (categid != '1') {
    //    $("#btn_addsim").hide();

    //}
}

function showrow() {
    $("#divrow").show();

}

function Back() {
    $("#divrow").hide();
}
$(document).ready(function () {
    //$('#tbl').DataTable({
    //    dom: 'Bfrtip',
    //    buttons: [
    //        //'copy', 'csv', 'excel', 'pdf', 'print'
    //        'excel', 'pdf', 'print'
    //    ]
    //});
});
function showrow() {
    $("#divrow").show();
}
function Save() {
    
    var Status = false;
    if ($("#chkstatus").is(':checked') == true) {
        Status = true;
    }
    var categ_name = $("#ddlcategories option:selected").text();
    //string SIM_NUMBER, string SIM_SERIALNO, string SIM_OPERATORNAME, string SIM_APNWEBSITE, string SIM_APNIP, string CATEG_ID
    if ($("#txtname").val() != "" && $("#ddlcategories").val() != 0 && $("#txtsimnumber").val()!="") {
     
        $.post("/Home/_SIM", {
            "SIM_NUMBER": $("#txtsimnumber").val(), "SIM_SERIALNO": $("#txtserialnumber").val(), "SIM_OPERATORNAME": $("#txtname").val(), "SIM_APNWEBSITE": $("#txtapnwebsite").val(), "SIM_APNIP": $("#txtapnip").val(), "CATEG_ID": $("#ddlcategories").val(), "SIM_CREATEDBY": 1, "SIM_STATUS": Status, "categname": categ_name
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("sim created successfully");
                location.reload();
                $("#divrow").hide();
            }


        });
    }
    else {
        if ($("#txtname").val() == "") {
            alert("Enter Operator name..");
            $("#txtname").focus();
        }
        else if ($("#ddlcategories").val() == 0) {

            alert("select category..");
            $("#ddlcategories").focus();
        }
        else if ($("#txtsimnumber").val() == "") {

            alert("Please Enter SIM Number..");
            $("#txtsimnumber").focus();
        }
    }
}



function UPDATESTATUS(STATUS, SIM_ID) {

    var sta = "";
    if (STATUS == 0) {
        sta = "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " + sta + " this Sim")) {
        $.post("/Home/UPDATE_SIM_STATUS", {
            "SIM_STATUS": STATUS, "SIM_ID": SIM_ID,
        }, function (data) {
            console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("Sim " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }

}


