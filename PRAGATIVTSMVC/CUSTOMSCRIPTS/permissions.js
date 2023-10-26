
window.onload = function () {
    $("#btnupdate").hide();
    $("#ddlusers").select2();
    getall();
}

function checkview() {
    var length = localStorage.getItem("length")
    for (var i = 0; i < length; i++) {

        if ($("#chkview").is(':checked') == true) {
            $("#chkview_" + i).prop('checked', true);
        }
        else {
            $("#chkview_" + i).prop('checked', false);
        }
    }
}


function checkedit() {
    var length = localStorage.getItem("length")

    for (var i = 0; i < length; i++) {

        if ($("#chkedit").is(':checked') == true) {
            $("#chkedit_" + i).prop('checked', true);
        }
        else {
            $("#chkedit_" + i).prop('checked', false);
        }

    }
}


function checkcreate() {

    var length = localStorage.getItem("length")

    for (var i = 0; i < length; i++) {

        if ($("#chkcreate").is(':checked') == true) {
            $("#chkcreate_" + i).prop('checked', true);
        }
        else {
            $("#chkcreate_" + i).prop('checked', false);
        }

    }
}
var PNAMES = [];
var PVIEW = [];
var PEDIT = [];
var PCREATE = [];
function getall() {

    $.post("/Home/_Permissions", function (data) {
        var dataT = data['data'];
        localStorage.setItem("length", dataT.length);
        for (var i = 0; i < dataT.length; i++) {
            tr = $('<tr/>');
            PNAMES.push(dataT[i].PAGE_NAME)
            tr.append("<td>" + (i + 1) + "</td>");
            tr.append("<td>" + dataT[i].PAGE_NAME + "</td>");
            tr.append("<td><input type='checkbox' id='chkview_" + i + "' style='width:13px' class='form-control' /></td>");
            tr.append("<td><input type='checkbox' id='chkedit_" + i + "' style='width:13px' class='form-control' /></td>");
            tr.append("<td><input type='checkbox' id='chkcreate_" + i + "' style='width:13px' class='form-control' /></td>");
            $('#tblpermissions').append(tr);
        }

        $('#tblpermissions').DataTable({
            dom: 'rtip',
            // "ordering": false,
            "ordering": false,
            "paging": false,
            buttons: [
                'excel', 'pdf', 'print',
            ],
            "scrollX": true,
            "scrollY": true,
            scrollX: "300px",
            scrollY: "300px",
            scrollCollapse: true,
            orderCellsTop: true,
            fixedHeader: true
        });

      
    });
}

function savepermissions() {

     //PNAMES = [];
     PVIEW = [];
     PEDIT = [];
     PCREATE = [];

     localStorage.setItem("pnames", null);
     localStorage.setItem("pviews", null);
     localStorage.setItem("pedit", null);
     localStorage.setItem("pcreate", null);

    var length = localStorage.getItem("length");
   
    for (var i = 0; i < length; i++) {


        if ($("#chkview_" + i).is(':checked') == true) {

            PVIEW.push(1);
        }
        else {
            PVIEW.push(0);
        }

        if ($("#chkedit_" + i).is(':checked') == true) {
            PEDIT.push(1);
        }
        else {
            PEDIT.push(0);
        }


        if($("#chkcreate_"+i).is(':checked') == true)
        {
            PCREATE.push(1);
        }
        else {
            PCREATE.push(0);
        }

    }

    
    //console.log(PNAMES);
    //console.log(PVIEW);
    //console.log(PEDIT);
    //console.log(PCREATE);

    //string[] name,string[] view,string[] edit,string[] create

         localStorage.setItem("pnames",PNAMES);
         localStorage.setItem("pviews",PVIEW);
         localStorage.setItem("pedit",PEDIT);
         localStorage.setItem("pcreate",PCREATE);


         var pnames=localStorage.getItem("pnames");
         var pview=localStorage.getItem("pviews");
         var pedit=localStorage.getItem("pedit");
         var pcreate=localStorage.getItem("pcreate");
      
         if ($("#ddlusers").val() != 0) {

         window.setTimeout(function(){
             $.post("/Home/_Permissionssave", { "categid": $("#ddlusers").val(), "name": pnames, "view": pview, "edit": pedit, "create": pcreate }, function (data) {
    
                 var data = data['data'];

                 if (data = true) {
                     alert("permissions added successfully");
                     location.reload();
                     
                 }


             });
         
         },1000)
         }
         else {
             alert("please select user..");
             $("#ddlusers").focus();
         }

}

function updatepermissions() {

   

    //PNAMES = [];
    PVIEW = [];
    PEDIT = [];
    PCREATE = [];

    localStorage.setItem("pnames", null);
    localStorage.setItem("pviews", null);
    localStorage.setItem("pedit", null);
    localStorage.setItem("pcreate", null);

    var length = localStorage.getItem("length");

    for (var i = 0; i < length; i++) {


        if ($("#chkview_" + i).is(':checked') == true) {

            PVIEW.push(1);
        }
        else {
            PVIEW.push(0);
        }

        if ($("#chkedit_" + i).is(':checked') == true) {
            PEDIT.push(1);
        }
        else {
            PEDIT.push(0);
        }


        if ($("#chkcreate_" + i).is(':checked') == true) {
            PCREATE.push(1);
        }
        else {
            PCREATE.push(0);
        }

    }


    //console.log(PNAMES);
    //console.log(PVIEW);
    //console.log(PEDIT);
    //console.log(PCREATE);
    //string[] name,string[] view,string[] edit,string[] create

    localStorage.setItem("pnames", PNAMES);
    localStorage.setItem("pviews", PVIEW);
    localStorage.setItem("pedit", PEDIT);
    localStorage.setItem("pcreate", PCREATE);


    var pnames = localStorage.getItem("pnames");
    var pview = localStorage.getItem("pviews");
    var pedit = localStorage.getItem("pedit");
    var pcreate = localStorage.getItem("pcreate");

    if ($("#ddlusers").val() != 0) {

        window.setTimeout(function () {
            $.post("/Home/_Permissionsupdate", { "name": pnames, "view": pview, "edit": pedit, "create": pcreate, "categid": $("#ddlusers").val() }, function (data) {

                var data = data['data'];

                if (data = true) {
                    alert("permissions update successfully");
                    location.reload();

                }


            });

        }, 1000)
    }
    else {
        alert("please select user..");
        $("#ddlusers").focus();
    }


}


function CHECKPERMISSIONS() {
  
    var leng = localStorage.getItem("length");
   

    var USER_ID = $("#ddlusers").val()
  
   if (USER_ID != '0')
   {
       $.post("/Home/_CHECK_PERMISSIONS", { "CATEG_ID": USER_ID }, function (data) {

           var dataT = data['data'];
         
           if (dataT.length > 1) {
               $("#btnsubmit").hide();
               $("#btnupdate").show();

               for (var i = 0; i < dataT.length; i++) {
                   if (dataT[i].PP_VIEW == '1') {
                       $("#chkview_" + i).prop('checked', true);
                   }
                   if (dataT[i].PP_VIEW == '0') {
                       $("#chkview_" + i).prop('checked', false);
                   }
                   if (dataT[i].PP_EDIT == '1') {
                       $("#chkedit_" + i).prop('checked', true);
                   }
                   if (dataT[i].PP_EDIT == '0') {
                       $("#chkedit_" + i).prop('checked', false);
                   }
                   if (dataT[i].PP_CREATE == '1') {
                       $("#chkcreate_" + i).prop('checked', true);
                   }
                   else {
                       $("#chkcreate_" + i).prop('checked', false);

                   }
               }
           }
           else
           {
               $("#btnsubmit").show();
               $("#btnupdate").hide();
               for (var i = 0; i < leng; i++) {
                  
                       $("#chkview_" + i).prop('checked', false);
                 
                       $("#chkedit_" + i).prop('checked', false);
                
                       $("#chkcreate_" + i).prop('checked', false);
                  
               }
           }
           
       });
   }
   else {
       $("#btnsubmit").show();
       $("#btnupdate").hide();
       for (var i = 0; i < leng; i++) {

           $("#chkview_" + i).prop('checked', false);

           $("#chkedit_" + i).prop('checked', false);

           $("#chkcreate_" + i).prop('checked', false);

       }
   }
   

}

