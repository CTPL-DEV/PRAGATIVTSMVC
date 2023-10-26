
window.onload = function () {
  
    localStorage.clear();
}

function Login()
{
    if ($("#txtOrgname").val() != '') {
        $.post("/Home/__DBNAME", { "ORGNAME": $("#txtOrgname").val() }, function (data) {

            var data = data['data'];
             
            if (data != null) {
            
                if ($("#txtUserName").val() != "" && $("#txtPassword").val() != "") {
                    $.post("/Home/get_User", { "CompanyName": $("#txtOrgname").val(), "UserName": $("#txtUserName").val() }, function (data) {

                      
                        var data = data['data'];

                     

                        var varValue = data.USERS_MENUITEMS;
                      
                        if (data.USERS_CATEGORY_ID == 11)
                        {
                            Response.Redirect("~/frm_dualRedirect.aspx", false);
                        }
                        else if (data.USERS_CATEGORY_ID == 1101)
                        {
                            Response.Redirect("~/frm_dualRedirect.aspx", false);
                        }


                        else if (data.USERS_CATEGORY_ID == 25)
                        {
                            Response.Redirect("~/Services/frm_GridTrack_Distance.aspx", false);
                            if (data.USERS_CATEGORY_ID == 25)
                            {

                                Response.Redirect("~/frm_dualRedirect.aspx", false);
                            }
                        }

                            //abhinandan dual redirect

                        else if (data.USERS_CATEGORY_ID == 44)
                        {

                            Response.Redirect("~/frm_dualRedirect.aspx", false);

                        }

                        else if (data.USERS_CATEGORY_ID == 84)
                        {
                            Response.Redirect("~/Services/frm_GridTrack_Distance.aspx", false);
                        }

                        else if (data.USERS_CATEGORY_ID == 113)
                        {
                            Response.Redirect("~/Services/frm_GridTrack_Distance.aspx", false);
                        }
                        else if (data.USERS_CATEGORY_ID == 119)
                        {
                            Response.Redirect("~/frm_dualRedirect.aspx", false);
                        }
                        else if (data.USERS_CATEGORY_ID == 3)
                        {
                            location.href = "/Home/frm_dualRedirect";
                        }
                        else
                        {

                            if (varValue.indexOf('56') != -1)                     // GeoFence Enabled Dashboard (If true then show Goefence enabled GridTrack else normal grid)
                            {
                                location.href = "/Home/frm_GridTrack_Geo";
                            }
                            else if (varValue.indexOf('82') != -1)               // Dashboard for Cab
                            {
                                location.href = "/Home/frm_GridTrack_CAB";
                            }
                            else if (varValue.indexOf('91') != -1) {// Dashboard for DASHBOARD_DISTANCE

                                location.href = "/Home/frm_GridTrack_Distance";
                            }

                            else if (varValue.indexOf('159') != -1) {// Dashboard for DASHBOARD_DISTANCE


                                location.href = "/Home/frm_consignment_dashboard";
                            }

                            else if (varValue.indexOf('48') != -1)        // Normal dashboard
                            {
                                location.href = "/Home/frm_GridTrack";
                                //Response.Redirect("~/Services/frm_GridTrack.aspx", false);
                            }

                            else if (varValue.indexOf('132') != -1) {// dhanush dashboard for all active vehicles

                                location.href = "/Home/frm_GridTrack_dhanush";

                            }
                            else if (varValue.indexOf('133') != -1)               // dhanush dashboard for auditing only
                            {
                                location.href = "/Home/frm_GridTrack_dhanush1";
                            }
                               


                                    // Response.End();
                                }
                      


































                                //if (dataT[0].STATUS == '1') {

                                //    localStorage.setItem("USERID", dataT[0].USERID);
                                //    localStorage.setItem("USERNAME", dataT[0].USERNAME);
                                //    localStorage.setItem("ROLEID", dataT[0].ROLEID);
                                //    location.href = "/Home/Dashboard";
                                //}
                                //else {

                                //    alert("Invalid Login..");

                                //}
                            });
                        }
                    else {
                    if ($("#txtUserName").val() == "") {
                        alert("please enter username..");
                        $("#txtUserName").focus();
                    }
                    else if ($("#txtPassword").val() == "") {
                        alert("please enter password..");
                        $("#txtPassword").focus();
                    }
                    }
                    }
                else {
                    alert("Invalid organization name..");
                }
            });
        }
        else {
        alert("enter organization name..")
        $("#txtOrgname").focus();
    }
    
    
}