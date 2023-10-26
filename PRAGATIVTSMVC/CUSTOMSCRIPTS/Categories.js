window.onload = function () {
    $("#divrow").hide();
    $("#btnupdate").hide();
    $("#btnsave").show();
}
function showrow() {
    $("#divrow").show();
}
function Save() {
    
    if ($("#txtname").val() != "" && $("#txtemail").val() != "" && $("#txtdesignation").val() != "" && $("#ddlcategtype").val() !='') {
        $.post("/Home/INSERT_CATEGORIES", {
            "CATEG_NAME": $("#txtname").val(), "CATEG_EMAILID": $("#txtemail").val(), "CATEG_DESC": $("#txtdesignation").val(),
            "CATEG_CONTACTPERSON": $("#txtcontactperson").val(), "CATEG_MOBILENUMBER": $("#txtmobile").val(), "CATEG_TELEPHONE": $("#txttelephone").val(), "CATEG_WEBSITENAME": $("#txtwebsite").val(),
            "CATEG_ZIPCODE": $("#txtzipcode").val(), "CATEG_ADDRESS": $("#txtaddress").val(), "CATEG_CREATEDBY": 1, "CATEG_TYPE": $("#ddlcategtype").val()
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("Category created successfully..");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {
        if ($("#txtname").val() == "")
        {
            alert("Enter Category Name..");
            $("#txtname").focus();
        }
        else if ($("#txtemail").val() == "")
        {
            alert("Enter Email..");
            $("#txtemail").focus();
        }
        else if ($("#txtdesignation").val() == "") {
            alert("Enter Designation..");
            $("#txtdesignation").focus();
        }
        else if ($("#ddlcategtype").val() == "")
        {
            alert("Select Categorytype..");
            $("#ddlcategtype").focus();
        }
    }
  
}


function EDIT(ID) {

    $("body").scrollTop(0);
    $.post("/Home/edit_categories", function (data) {
        $("#divrow").show();
        $("#btnsave").hide();
        $("#btnupdate").show();
        localStorage.setItem("categid", null);
        localStorage.setItem("categid", ID);

        var dataT = data['data'];
        for (var i = 0; i < dataT.length; i++) {
            if (dataT[i].CATEG_ID == ID) {
                $("#txtname").val(dataT[i].CATEG_NAME);
                $("#txtmobile").val(dataT[i].CATEG_MOBILENUMBER);
                $("#txtemail").val(dataT[i].CATEG_EMAILID);
                $("#txtmobile").val(dataT[i].CATEG_MOBILENUMBER);
                $("#txttelephone").val(dataT[i].CATEG_TELEPHONE);
                $("#txtdesignation").val(dataT[i].CATEG_DESC);
                $("#txtwebsite").val(dataT[i].CATEG_WEBSITENAME);
                $("#txtcontactperson").val(dataT[i].CATEG_CONTACTPERSON);
                $("#txtzipcode").val(dataT[i].CATEG_ZIPCODE);
                $("#txtaddress").val(dataT[i].CATEG_ADDRESS);
                $("#ddlcategtype").val(dataT[i].CATEG_TYPE)
            }
        }

    });
}


function Update() {
   var id= localStorage.getItem("categid")
    if ($("#txtname").val() != "" && $("#txtemail").val() != "" && $("#txtdesignation").val() != "" && $("#ddlcategtype").val() != '') {
        $.post("/Home/CATEGORIES_UPDATE", {
            "CATEG_NAME": $("#txtname").val(), "CATEG_EMAILID": $("#txtemail").val(), "CATEG_DESC": $("#txtdesignation").val(),
            "CATEG_CONTACTPERSON": $("#txtcontactperson").val(), "CATEG_MOBILENUMBER": $("#txtmobile").val(), "CATEG_TELEPHONE": $("#txttelephone").val(), "CATEG_WEBSITENAME": $("#txtwebsite").val(),
            "CATEG_ZIPCODE": $("#txtzipcode").val(), "CATEG_ADDRESS": $("#txtaddress").val(), "CATEG_TYPE": $("#ddlcategtype").val(),"CATEG_ID":id
        }, function (data) {

            var data = data['data'];

            if (data = true) {
                alert("Category updated successfully..");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {
        if ($("#txtname").val() == "") {
            alert("Enter Category Name..");
            $("#txtname").focus();
        }
        else if ($("#txtemail").val() == "") {
            alert("Enter Email..");
            $("#txtemail").focus();
        }
        else if ($("#txtdesignation").val() == "") {
            alert("Enter Designation..");
            $("#txtdesignation").focus();
        }
        else if ($("#ddlcategtype").val() == "") {
            alert("Select Categorytype..");
            $("#ddlcategtype").focus();
        }
    }
}





function UPDATESTATUS(STATUS,CATEG_ID) {
  
    var sta = "";
    if (STATUS == 0)
    {
        sta =  "In Active";
    }
    else {
        sta = "Active";
    }

    if (confirm("are you sure to " +sta+ " this category")) {
        $.post("/Home/UPDATE_CATEGORIES_STATUS", {
            "CATEG_STATUS": STATUS, "CATEG_ID": CATEG_ID, "CATEG_UPDATEDBY": 1
        }, function (data) {
            console.log(data);
            var data = data['data'];

            if (data = true) {
                alert("Category " + sta + " Successfully");
                location.reload();
                $("#divrow").hide();
            }

        });
    }
    else {

    }
   
}