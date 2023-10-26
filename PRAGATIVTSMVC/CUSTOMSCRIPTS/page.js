

window.onload = function () {
    $("#divrow").hide();
    $("#btnupdate").hide();
    $("#btnsave").show();
}

function showrow() {
    $("#divrow").show();
}
function Save() {
    if ($("#txtpagename").val() != '' && $("#ddlpageunder").val() != 0) {
        $.post("/Home/_PAGES", {
            "PAGE_NAME": $("#txtpagename").val(), "PAGE_UNDER": $("#ddlpageunder").val(), "PAGE_CREATEDBY": 1
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("Page created successfully");
                location.reload();
                $("#divrow").hide();
            }
        });
    }
    else {
        if ($("#txtpagename").val() == '') {
            alert("Enter page name..");
            $("#txtpagename").focus();
        }
        else if ($("#ddlpageunder").val() == 0) {
            alert("select pageunder..");
            $("#ddlpageunder").focus();
        }

    }

}



function EDIT(ID) {

    $("body").scrollTop(0);
    $.post("/Home/_EDIT_PAGES", function (data) {
        $("#divrow").show();
        $("#btnsave").hide();
        $("#btnupdate").show();
        localStorage.setItem("pageid", null);
        localStorage.setItem("pageid", ID);

        var dataT = data['data'];
        for (var i = 0; i < dataT.length; i++) {
            if (dataT[i].PAGE_ID == ID) {

                $("#txtpagename").val(dataT[i].PAGE_NAME);
                $("#ddlpageunder").val(dataT[i].PAGE_UNDER);
            }
        }

    });
}

function Update() {
   
    var ID = localStorage.getItem("pageid");

    if ($("#txtpagename").val() != '' && $("#ddlpageunder").val() !=0) {
        $.post("/Home/PAGE_UPDATE", { "PAGE_NAME": $("#txtpagename").val(), "PAGE_UNDER": $("#ddlpageunder").val(), "PAGE_ID": ID }, function (data) {
            var data = data['data'];

            if (data = true) {
                alert("page updated successfully");
                location.reload();
                $("#divrow").hide();
            }
        });
    }
    else {
        if ($("#txtpagename").val() == '') {
            alert("Enter page name..");
            $("#txtpagename").focus();
        }
        else if ($("#ddlpageunder").val() == 0) {
            alert("select pageunder..");
            $("#ddlpageunder").focus();
        }
       
    }

}


function UPDATESTATUS(STATUS, PAGE_ID) {

    var sta = "";
    if (STATUS == 0) {
        sta = "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " + sta + " this Page")) {
        $.post("/Home/UPDATE_PAGES_STATUS", {
            "PAGE_STATUS": STATUS, "PAGE_ID": PAGE_ID,
        }, function (data) {
            //console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("Page " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }

}
