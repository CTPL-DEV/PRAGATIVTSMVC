using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRAGATIVTS_MVC.Models;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using Newtonsoft.Json;
//using Kendo.Mvc.UI;
//using Kendo.Mvc.Extensions;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.OleDb;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http;
using Microsoft.Ajax.Utilities;
using System.Drawing.Imaging;
using System.Net.Http.Headers;

namespace PRAGATIVTSMVC.Controllers
{

    public class HomeController : BaseController
    {
        private static Random rng = new Random();
        private static List<OTPCode> OTPCodes = new List<OTPCode>();
        [SessionAuthorize]
        public JsonResult _pages_PERMISSIONS(string CATEGNAME)
        {
            int categid = 0;
            BLL OBJ = new BLL();
            List<PVTS_CATEGORIES> obj1 = new List<PVTS_CATEGORIES>();
            List<PVTS_CATEGORIES> obj2 = new List<PVTS_CATEGORIES>();
            obj1 = OBJ.GETALL_PANELS();

            //  string dbname = "Aor2T0SveXPKBbMcC7tSjrv+vNImP9nNMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rznBz47OI45T+I00H+mKDZtnMOdeZlpCzckukpOKqMbXjlktkV49jJ";
            string dbname = (ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
            DataTable dtc = BLL.getcategoryid_db(CATEGNAME, dbname);
            if (dtc.Rows.Count > 0)
            {
                categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);
            }
            else
            {
                categid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }


            return Json(new { data = obj1, data1 = obj2 });
        }
        //public JsonResult __DBNAME(string ORGNAME)
        //{
        //    BLL obj = new BLL();
        //    var data = obj.getDatabase(ORGNAME);
        //    Session["DBNAME"] = data;
        //    return Json(new { data = data });
        //}

        //by Ajith
        [HttpPost]
        public JsonResult __DBNAME(string Username, string Password)
        {


            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            var data = BLL.getDatabaseForUser(Username, Password);
            string website = Session["DomainName"].ToString();
            if (data.Rows.Count > 0)
            {
                if (Session["DomainName"].ToString() != "103.117.174.35" && Session["DomainName"].ToString() != "localhost" && Session["DomainName"].ToString() != "tranopro.com" && Session["DomainName"].ToString() != "tranopro.azurewebsites.net")
                {


                    if (Session["DomainName"].ToString() == data.Rows[0]["CATEG_WEBSITENAME"].ToString())
                    {
                        Session["DBNAME"] = data.Rows[0][1].ToString();

                        obj.Add(new SMVTS_CATEGORIES
                        {
                            CATEG_NAME = data.Rows[0][0].ToString(),
                            CATEG_DBNAME = data.Rows[0][1].ToString(),

                        });
                        return Json(new { data = obj });
                    }
                    else
                    {

                        return Json(new { data = "0" });
                    }
                }
                else
                {
                    //Session["DBNAME"] = data.Rows[0][1].ToString();
                    Session["DBNAME"] = ConfigurationManager.ConnectionStrings["connection_prod"].ToString();

                    obj.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_NAME = data.Rows[0][0].ToString(),
                        CATEG_DBNAME = data.Rows[0][1].ToString(),

                    });
                    return Json(new { data = obj });
                }
            }
            return Json(new { data = obj });

        }
        public JsonResult get_User(string CompanyName, string UserName, string rememberChkBox, string Password)


        {
            try
            {
                string dbname = Session["DBNAME"].ToString();
                var data = BLL.get_User(CompanyName, UserName, dbname);
                string MSG = "false";
                SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();
                if (data != null)
                {
                    _obj_Smvts_Users = data;
                    string cpw = BLL.Decrypt(_obj_Smvts_Users.USERS_PASSWORD);
                    if (cpw == Password)
                    {
                        MSG = "true";
                        //


                        //
                        _obj_Smvts_Users.USERS_DBNAME = dbname;


                        ViewBag.R_username = UserName;
                        ViewBag.R_Password = Password;
                        ViewBag.R_CompanyName = CompanyName;

                        Session["R_username"] = UserName;
                        Session["R_Password"] = Password;
                        Session["R_CompanyName"] = CompanyName;

                        Session.Add("USERINFO", _obj_Smvts_Users);

                        if (rememberChkBox == "on")
                        {
                            HttpCookie cookie = new HttpCookie("Credentials");

                            cookie.Values.Add("Username", UserName);// your x value

                            cookie.Values.Add("Password", Password); // your y value

                            cookie.Values.Add("CompanyName", CompanyName);

                            cookie.Expires = DateTime.Now.AddMonths(6); // --------> cookie.Expires is the property you can set timeout

                            //HttpContext.current.Response.AppendCookie(cookie);
                            HttpContext.Response.Cookies.Set(cookie);
                            //user name and password binded on textbox that purpose
                            HttpCookie cookie1 = new HttpCookie("namepassword");
                            cookie1.Values.Add("user_N", UserName);
                            cookie1.Values.Add("user_P", Password);
                            cookie1.Values.Add("user_C", CompanyName);
                            cookie1.Expires = DateTime.Now.AddMonths(6);
                            HttpContext.Response.Cookies.Set(cookie1);
                        }
                        else
                        {
                            HttpCookie checkCookie = Request.Cookies.Get("namepassword");
                            if (checkCookie != null)
                            {
                                checkCookie.Expires = DateTime.Now.AddDays(-10);
                                checkCookie.Value = null;
                                HttpContext.Response.Cookies.Set(checkCookie);
                            }
                        }
                    }
                }
                else
                {
                    MSG = "false";
                }
                return Json(new { data = data, dataM = MSG });
            }
            catch (Exception ex)
            {
                return Json(new { data = "", dataM = ex.ToString() });
            }
        }
        [SessionAuthorize]
        public ActionResult frm_GridTrack_Distance()
        {
            checkcategoryid();

            SMVTS_CATEGORIES _obj_Smvts_categories = new SMVTS_CATEGORIES();
            _obj_Smvts_categories.OPERATION = operation.Select;
            List<SMVTS_CATEGORIES> obj_categ = new List<SMVTS_CATEGORIES>();
            DataTable dt5 = BLL.get_CategoriesPro(_obj_Smvts_categories);

            for (int m = 0; m < dt5.Rows.Count; m++)
            {
                obj_categ.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = dt5.Rows[m]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt5.Rows[m]["CATEG_ID"]),
                });
            }
            ViewBag.categories = obj_categ;

            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();

            if (Session["USERINFO"] == null)
            {

            }
            DataTable dt = BLL.get_GridTrackDistance(_obj_Smvts_GridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                int spe = 0;
                if (dt.Rows[i]["SPEED"] == DBNull.Value)
                {
                    spe = 0;
                }
                else
                {
                    spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                }
                DateTime Exp_Date = new DateTime();
                DataTable dt_expiry = BLL.check_order_expiry(Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()));
                if (dt_expiry.Rows.Count > 0)
                {
                    if (dt_expiry.Rows[0][0].ToString() != "" && dt_expiry.Rows[0][0].ToString() != null)
                    {
                        Exp_Date = Convert.ToDateTime(dt_expiry.Rows[0][0]);
                    }


                }
                Exp_Date = DateTime.Now.Date.AddDays(10);
                if (Exp_Date > DateTime.Now)
                {
                    obj.Add(new SMVTS_GRIDTRACK
                    {
                        SPEED = spe,
                        IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                        DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                        VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                        DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                        PLACE = dt.Rows[i]["PLACE"].ToString(),
                        DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                        DURATION = dt.Rows[i]["duration"].ToString(),
                        DKM = dt.Rows[i]["distance_day"].ToString(),
                        TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                        LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                        LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                        COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                        DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                        Lastdate = dt.Rows[i]["DATE"].ToString(),
                        VEHICLE_IMAGE = dt.Rows[i]["VEHMODELIMAGES_IMAGEURL"].ToString(),
                        ac_status = dt.Rows[i]["AC_STATUS"].ToString(),
                    });
                }
            }

            ViewBag.DASHBOARDDATA = obj;

            return View();

        }
        [SessionAuthorize]
        public ActionResult frm_GridTrack()
        {
            return View();
        }

        public JsonResult Load_Vehicles(string ClientId = null)
        {
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            List<SMVTS_VEHICLES> obj1 = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            try
            {
                string parClientID = "";
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    parClientID = ClientId;
                }
                else
                {
                    parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                }

                //  string parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);

                _obj_Smvts_Vehicles.OPERATION = operation.Empty;
                _obj_Smvts_Vehicles.CREATEDBY = 0;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(parClientID);


                DataTable dt;
                if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "4")
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = -1;
                }
                else
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                }

                dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);
                if (dt.Rows.Count < 2)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj.Add(new SMVTS_VEHICLES
                        {
                            VEHICLES_REGNUMBER = dt.Rows[0]["VEHICLES_REGNUMBER"].ToString(),
                            VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[0]["VEHICLES_DEVICE_ID"]),
                        });
                    }
                }

                //dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj1.Add(new SMVTS_VEHICLES
                    {
                        VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_DEVICE_ID"]),
                    });
                }

            }

            catch
            {

            }

            return Json(new { data = obj, data1 = obj1 });
        }

        public JsonResult Load_VehiclesCategWise(string ClientId)
        {
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            List<SMVTS_VEHICLES> obj1 = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            string parClientID = "";
            try
            {

                //if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                //{
                //    parClientID = ClientId;
                //}
                //else
                //{
                //    parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                //}

                //  string parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                parClientID = ClientId;

                _obj_Smvts_Vehicles.OPERATION = operation.Empty;
                _obj_Smvts_Vehicles.CREATEDBY = 0;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(parClientID);


                DataTable dt;
                //if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "4")
                //{
                //    _obj_Smvts_Vehicles.LASTMDFBY = -1;
                //}
                //else
                //{
                //    _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                //}

                dt = BLL.get_VehiclesCategWise(_obj_Smvts_Vehicles);
                if (dt.Rows.Count < 2)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj.Add(new SMVTS_VEHICLES
                        {
                            VEHICLES_REGNUMBER = dt.Rows[0]["VEHICLES_REGNUMBER"].ToString(),
                            VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[0]["VEHICLES_DEVICE_ID"]),
                        });
                    }
                }

                //dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj1.Add(new SMVTS_VEHICLES
                    {
                        VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_DEVICE_ID"]),
                    });
                }

            }

            catch
            {

            }

            return Json(new { data1 = obj1 });
        }

        public JsonResult VEHICLECHANGE(string Device_ID)
        {

            SMVTS_USERS _obj_Smvts_Roles = new SMVTS_USERS();
            string Operation = "PAN";
            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            string Category_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
            DataTable dt;
            string username = Session["R_username"].ToString();

            int distance = 0;
            string distance_plant = "";
            string DISTANCE_PORT = "";

            if (username == "BAJAJ" || username == "JLPL" || username == "OMKAR IB" || username == "Satish" || username == "Bafna" || username == "shreenathji ib" || username == "AUTOMOBILE ENGINEERS")
            {
                distance = 1;
                dt = BLL.get_Tracking("GETLOC_BAJAJ", User_ID, Category_ID, Device_ID);
            }
            else
            {
                dt = BLL.get_Tracking("GETLOC_TEST", User_ID, Category_ID, Device_ID);
            }


            if (Device_ID != "0")
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (distance == 1)
                    {
                        try
                        {
                            distance_plant = dt.Rows[i]["DISTANCE"].ToString();
                            DISTANCE_PORT = dt.Rows[i]["DISTANCE_PORT"].ToString();
                        }
                        catch
                        {
                            distance_plant = "";
                            DISTANCE_PORT = "";
                        }

                    }
                    else
                    {
                        distance_plant = "";
                        DISTANCE_PORT = "";
                    }
                    try
                    {
                        obj.Add(new SMVTS_GRIDTRACK
                        {
                            SPEED = Convert.ToInt32(dt.Rows[i]["TRIPDATA_SPEED"]),
                            IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                            DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                            VEHICLENUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                            TRIPDATA_TRIPDATE = dt.Rows[i]["TRIPDATA_TRIPDATE"].ToString(),
                            PLACE = dt.Rows[i]["ADDRESS"].ToString(),
                            TOTAL_KM_DAY = dt.Rows[i]["distance_day"].ToString(),
                            // DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                            //  DURATION = dt.Rows[i]["duration"].ToString(),
                            // DKM = dt.Rows[i]["distance_day"].ToString(),
                            //  TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                            DISTANCE = distance_plant,
                            DKM = DISTANCE_PORT,
                            LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                            LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                            COLOR = dt.Rows[i]["VEHICLE_COLOR"].ToString(),
                            TRIPDATA_DIRECTION = dt.Rows[i]["TRIPDATA_DEGREEINDIRECTION"].ToString(),
                            ODOMETER = Convert.ToInt32(dt.Rows[i]["TRIPDATA_C_ODOMETER"]),
                        });
                    }
                    catch
                    {

                    }

                }
            if (Device_ID == "0")
            {
                DataTable dt1 = new DataTable();
                dt1 = BLL.get_Tracking("GETALL", User_ID, Category_ID, Device_ID);
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_GRIDTRACK
                    {
                        SPEED = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_SPEED"]),
                        IGNITION = dt1.Rows[i]["IGNITION"].ToString(),
                        DRIVER_NAME = dt1.Rows[i]["DRIVER_NAME"].ToString(),
                        VEHICLENUMBER = dt1.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        TRIPDATA_TRIPDATE = dt1.Rows[i]["TRIPDATA_TRIPDATE"].ToString(),
                        PLACE = dt1.Rows[i]["ADDRESS"].ToString(),
                        TOTAL_KM_DAY = dt1.Rows[i]["distance_day"].ToString(),
                        // DRIVERNUMBER = dt1.Rows[i]["driver_number"].ToString(),
                        //  DURATION = dt1.Rows[i]["duration"].ToString(),
                        // DKM = dt1.Rows[i]["distance_day"].ToString(),
                        //  TRIPPLAN = dt1.Rows[i]["tripplan"].ToString(),
                        LATITUDE = dt1.Rows[i]["tripdata_latitude"].ToString(),
                        LONGITUDE = dt1.Rows[i]["tripdata_longitude"].ToString(),
                        COLOR = dt1.Rows[i]["VEHICLE_COLOR"].ToString(),
                        TRIPDATA_DIRECTION = dt1.Rows[i]["TRIPDATA_DEGREEINDIRECTION"].ToString(),
                        ODOMETER = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_C_ODOMETER"]),
                    });
                }
            }
            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }




        public JsonResult allvehicles_status(string COLOR, string ClientId = null)
        {
            string Device_ID = "0";
            SMVTS_USERS _obj_Smvts_Roles = new SMVTS_USERS();

            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);


            string Category_ID = "";
            if (ClientId != null && ClientId != "")
            {
                Category_ID = ClientId;

            }
            else if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                Category_ID = ClientId;
            }
            else
            {
                Category_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }


            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();


            DataTable selectedTable = new DataTable();
            selectedTable = BLL.get_Tracking("GETALL", User_ID, Category_ID, Device_ID);



            DataTable dt1 = selectedTable.AsEnumerable()
                            .Where(r => r.Field<string>("VEHICLE_COLOR") == COLOR)
                            .CopyToDataTable();



            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                int spe = 0;
                int ODOMETER = 0;
                if (dt1.Rows[i]["TRIPDATA_SPEED"] == DBNull.Value)
                {
                    spe = 0;
                }
                else
                {
                    spe = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_SPEED"]);// o = value;
                }
                if (dt1.Rows[i]["TRIPDATA_C_ODOMETER"] == DBNull.Value)
                {
                    ODOMETER = 0;
                }
                else
                {
                    ODOMETER = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_C_ODOMETER"]);// o = value;
                }


                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = spe,
                    IGNITION = dt1.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt1.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt1.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    DATE = Convert.ToDateTime(dt1.Rows[i]["TRIPDATA_TRIPDATE"]),
                    PLACE = dt1.Rows[i]["ADDRESS"].ToString(),
                    // DRIVERNUMBER = dt1.Rows[i]["driver_number"].ToString(),
                    //  DURATION = dt1.Rows[i]["duration"].ToString(),
                    // DKM = dt1.Rows[i]["distance_day"].ToString(),
                    //  TRIPPLAN = dt1.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt1.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt1.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt1.Rows[i]["VEHICLE_COLOR"].ToString(),
                    TRIPDATA_DIRECTION = dt1.Rows[i]["TRIPDATA_DEGREEINDIRECTION"].ToString(),
                    ODOMETER = ODOMETER,
                });

            }
            return Json(new { data = obj });
        }


        public JsonResult allvehicledetails(string ClientId = null)
        {
            string Device_ID = "0";
            SMVTS_USERS _obj_Smvts_Roles = new SMVTS_USERS();

            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);


            string Category_ID = "";
            if (ClientId != null && ClientId != "")
            {
                Category_ID = ClientId;

            }
            else if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                Category_ID = ClientId;
            }
            else
            {
                Category_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }


            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();


            DataTable dt1 = new DataTable();
            dt1 = BLL.get_Tracking("GETALL", User_ID, Category_ID, Device_ID);


            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                int spe = 0;
                int ODOMETER = 0;
                if (dt1.Rows[i]["TRIPDATA_SPEED"] == DBNull.Value)
                {
                    spe = 0;
                }
                else
                {
                    spe = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_SPEED"]);// o = value;
                }
                if (dt1.Rows[i]["TRIPDATA_C_ODOMETER"] == DBNull.Value)
                {
                    ODOMETER = 0;
                }
                else
                {
                    ODOMETER = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_C_ODOMETER"]);// o = value;
                }


                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = spe,
                    IGNITION = dt1.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt1.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt1.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    TRIPDATA_TRIPDATE = dt1.Rows[i]["TRIPDATA_TRIPDATE"].ToString(),
                    PLACE = dt1.Rows[i]["ADDRESS"].ToString(),
                    // DRIVERNUMBER = dt1.Rows[i]["driver_number"].ToString(),
                    //  DURATION = dt1.Rows[i]["duration"].ToString(),
                    // DKM = dt1.Rows[i]["distance_day"].ToString(),
                    //  TRIPPLAN = dt1.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt1.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt1.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt1.Rows[i]["VEHICLE_COLOR"].ToString(),
                    TRIPDATA_DIRECTION = dt1.Rows[i]["TRIPDATA_DEGREEINDIRECTION"].ToString(),
                    ODOMETER = ODOMETER,

                    TOTAL_KM_DAY = dt1.Rows[i]["distance_day"].ToString(),
                });

            }

            var jsonData = new
            {
                data = obj,
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        //get_Tracking_workorders(string Operation,int UserId,int Workorderid)

        public JsonResult allvehicledetails_workorders(string workorderid)
        {
            string Device_ID = "0";
            SMVTS_USERS _obj_Smvts_Roles = new SMVTS_USERS();

            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            string Category_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();


            DataTable dt1 = new DataTable();
            dt1 = BLL.get_Tracking_workorders(Convert.ToInt32(User_ID), Convert.ToInt32(workorderid));
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_SPEED"]),
                    IGNITION = dt1.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt1.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt1.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    DATE = Convert.ToDateTime(dt1.Rows[i]["TRIPDATA_TRIPDATE"]),
                    PLACE = dt1.Rows[i]["ADDRESS"].ToString(),
                    // DRIVERNUMBER = dt1.Rows[i]["driver_number"].ToString(),
                    //  DURATION = dt1.Rows[i]["duration"].ToString(),
                    // DKM = dt1.Rows[i]["distance_day"].ToString(),
                    //  TRIPPLAN = dt1.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt1.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt1.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt1.Rows[i]["VEHICLE_COLOR"].ToString(),
                    TRIPDATA_DIRECTION = dt1.Rows[i]["TRIPDATA_DEGREEINDIRECTION"].ToString(),
                    ODOMETER = Convert.ToInt32(dt1.Rows[i]["TRIPDATA_C_ODOMETER"]),
                });

            }
            return Json(new { data = obj });
        }


        public class tblShiftt
        {
            public string SHIFTVALUES_Val { get; set; }
            public int SHIFTVALUES_ID { get; set; }
            public bool Status { get; set; }
        }
        [SessionAuthorize]
        public ActionResult Shifttimings()
        {
            return View();
        }

        public JsonResult _Shifttimings()
        {
            List<tblShiftt> obj = new List<tblShiftt>();
            int CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            DataTable dt = BLL.get_shifttimings(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblShiftt
                {
                    SHIFTVALUES_ID = Convert.ToInt32(dt.Rows[i]["SHIFTVALUES_ID"]),
                    SHIFTVALUES_Val = dt.Rows[i]["SHIFTVALUES_Val"].ToString(),
                    Status = Convert.ToBoolean(dt.Rows[i]["SHIFTValues_Status"])
                });
            }

            return Json(new { data = obj });
        }

        public JsonResult Insert_shifttimings(string shift_val)
        {
            string CATEG_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            bool b = BLL.shifttimings_insert(shift_val, CATEG_ID);

            return Json(new { data = b });
        }

        public JsonResult Update_shifttimings(string shift_val, int ID)
        {
            bool b = BLL.shifttimings_update(shift_val, ID);

            return Json(new { data = b });
        }
        [SessionAuthorize]
        public ActionResult Routes()
        {
            List<tblroutes> obj = new List<tblroutes>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_routes(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblroutes
                {
                    ROUTES_ID = Convert.ToInt32(dt.Rows[i]["ROUTES_ID"]),
                    ROUTES_NAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                    ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString(),
                    ROUTES_STATUS = Convert.ToBoolean(dt.Rows[i]["ROUTES_STATUS"])
                });
            }
            ViewBag.routes = obj;
            return View();
        }

        public JsonResult _RoutesBYID(int ROUTES_ID)
        {
            List<tblroutes> obj = new List<tblroutes>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_routesbyid(ROUTES_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblroutes
                {
                    ROUTES_ID = Convert.ToInt32(dt.Rows[i]["ROUTES_ID"]),
                    ROUTES_NAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                    ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString(),
                    ROUTES_STATUS = Convert.ToBoolean(dt.Rows[i]["ROUTES_STATUS"])
                });
            }

            return Json(new { data = obj });
        }

        public JsonResult _stationsbyroute(int ROUTES_ID)
        {
            List<tblstations> obj = new List<tblstations>();
            DataTable dt = BLL.get_stations(ROUTES_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblstations
                {
                    STATION_ID = Convert.ToInt32(dt.Rows[i]["STATION_ID"]),
                    STATION_Lat = dt.Rows[i]["STATION_Lat"].ToString(),
                    STATION_Long = dt.Rows[i]["STATION_Lng"].ToString(),
                    STATION_GOOGLEName = dt.Rows[i]["STATION_GOOGLEName"].ToString(),
                    STATION_ShortCode = dt.Rows[i]["STATION_ShortCode"].ToString(),
                    STATION_PLACE = dt.Rows[i]["STATION_PLACE"].ToString(),
                    SNUMBER = Convert.ToInt32(dt.Rows[i]["SNUMBER"]),
                });
            }


            return Json(new { data = obj });
        }

        public JsonResult _stationsbyroutereverse(int ROUTES_ID)
        {
            List<tblstations> obj = new List<tblstations>();
            DataTable dt = BLL.get_stations_reverse(ROUTES_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblstations
                {
                    STATION_ID = Convert.ToInt32(dt.Rows[i]["STATION_ID"]),
                    STATION_Lat = dt.Rows[i]["STATION_Lat"].ToString(),
                    STATION_Long = dt.Rows[i]["STATION_Lng"].ToString(),
                    STATION_GOOGLEName = dt.Rows[i]["STATION_GOOGLEName"].ToString(),
                    STATION_ShortCode = dt.Rows[i]["STATION_ShortCode"].ToString(),
                    STATION_PLACE = dt.Rows[i]["STATION_PLACE"].ToString(),
                    SNUMBER = Convert.ToInt32(dt.Rows[i]["SNUMBER"]),
                });
            }


            return Json(new { data = obj });
        }

        public JsonResult bindvehiclesbycategid()
        {
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            _obj_Smvts_Vehicles.OPERATION = operation.Empty;
            _obj_Smvts_Vehicles.CREATEDBY = 0;
            _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = CATEG_ID;

            if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "4")
            {
                _obj_Smvts_Vehicles.LASTMDFBY = -1;
            }
            else
            {
                _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            }

            // DataTable DT = BLL.CHAKANAKURDI_VEHICLES(_obj_Smvts_Vehicles);
            DataTable DT = BLL.get_ActiveVehicles(_obj_Smvts_Vehicles);
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLES
                {

                    VEHICLES_REGNUMBER = DT.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    VEHICLES_DEVICE_ID = Convert.ToInt32(DT.Rows[i]["VEHICLES_DEVICE_ID"]),
                });
            }

            return Json(new { data = obj });

        }

        public JsonResult STATIONS_DELETE(string STATION_ID)
        {
            bool b = BLL.STATIONS_DELETE(Convert.ToInt32(STATION_ID));
            return Json(new { data = b });
        }

        public class tblroutes
        {
            public string ROUTES_NAME { get; set; }
            public string ROUTES_CODE { get; set; }
            public int ROUTES_ID { get; set; }
            public bool ROUTES_STATUS { get; set; }
        }


        public class tblvehiclesBus
        {
            public string VehicleRegNo { get; set; }

        }
        public class tblstations
        {
            public int STATION_ID { get; set; }
            public string STATION_Lat { get; set; }
            public string STATION_Long { get; set; }
            public string STATION_GOOGLEName { get; set; }
            public string STATION_ShortCode { get; set; }
            public string STATION_PLACE { get; set; }
            public int SNUMBER { get; set; }
        }
        public JsonResult _ROUTES(string ROUTES_NAME, string ROUTES_CODE, string ROUTESID)
        {
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            int USER_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int id = 0;
            if (ROUTESID == null || ROUTESID == "null" || ROUTESID == "")
            {

                id = BLL.routes_insert(ROUTES_NAME, ROUTES_CODE, CATEG_ID, USER_ID);
                Session["ROUTES_ID"] = id;

            }
            else
            {
                id = Convert.ToInt32(ROUTESID);
                bool b = BLL.routes_update(ROUTES_NAME, ROUTES_CODE, CATEG_ID, USER_ID, Convert.ToInt32(ROUTESID));
                Session["ROUTES_ID"] = id;
            }
            return Json(new { data = id });
        }
        public JsonResult _STATTIONS(string STATION_GOOGLEName, string STATION_ShortCode, string STATION_Lat, string STATION_Lng, string STATION_PLACE, string ROUTE_ID, string SNUMBER)
        {
            int ROUTEID = Convert.ToInt32(Session["ROUTES_ID"]);
            int USER_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);

            if (ROUTE_ID == null || ROUTE_ID == "null" || ROUTE_ID == "")
            {

                bool B = BLL.STATIONS_INSERT(STATION_GOOGLEName, STATION_ShortCode, ROUTEID, STATION_Lat, STATION_Lng, USER_ID, STATION_PLACE, SNUMBER);

            }
            else
            {
                bool B = BLL.STATIONS_UPDATE(STATION_GOOGLEName, STATION_ShortCode, ROUTEID, STATION_Lat, STATION_Lng, USER_ID, STATION_PLACE, SNUMBER);
            }

            return Json(new { data = "MSG" });
        }
        [SessionAuthorize]
        public ActionResult scheduler()
        {


            List<tblvehiclesBus> obj1 = new List<tblvehiclesBus>();
            List<tblroutes> obj = new List<tblroutes>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_routes(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblroutes
                {
                    ROUTES_ID = Convert.ToInt32(dt.Rows[i]["ROUTES_ID"]),
                    ROUTES_NAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                    ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString(),
                    ROUTES_STATUS = Convert.ToBoolean(dt.Rows[i]["ROUTES_STATUS"])
                });
            }
            ViewBag.routes00 = obj;

            DataTable dt2 = BLL.getvehiclesallBus(CATEG_ID);

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                obj1.Add(new tblvehiclesBus
                {
                    VehicleRegNo = dt2.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                });
            }
            ViewBag.buses00 = obj1;

            return View();
        }
        public JsonResult _sheduler_insert(string VNO, string ROUTES_ID, string SCHEDULER_STARTDATE, string SCHEDULER_ENDDATE, string SHIFT_TIME, string SHIFT_TYPE, string VEHICLE_REGNUMBER)
        {
            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            int b = BLL.sheduler_insert(VNO, Convert.ToInt32(ROUTES_ID), SCHEDULER_STARTDATE, SCHEDULER_ENDDATE, CATEG_ID, Convert.ToInt32(User_ID), SHIFT_TIME, SHIFT_TYPE, VEHICLE_REGNUMBER);
            Session["SCHEDULER_ID"] = b;
            return Json(new { data = b });
        }

        public JsonResult _sheduler_insertBus(int id, string VNO, string ROUTES_ID, string VEHICLE_REGNUMBER)
        {

            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            int b = BLL.sheduler_insertBus(id, VNO, Convert.ToInt32(ROUTES_ID), CATEG_ID, VEHICLE_REGNUMBER);
            Session["SCHEDULER_ID"] = b;
            return Json(new { data = b });
        }
        public JsonResult insert_sheduler_details(string SCHEDULERDETAILS_STATIONID, string SCHEDULERDETAILS_Type, string SCHEDULERDETAILS_Time, string SCHEDULERDETAILS_PassengerIDs, int Device_ID)
        {
            int ID = Convert.ToInt32(Session["SCHEDULER_ID"]);
            bool b = false; bool v = false;
            if (ID != 0)
            {
                b = BLL.sheduler_details_insert(Convert.ToInt32(ID), Convert.ToInt32(SCHEDULERDETAILS_STATIONID), SCHEDULERDETAILS_Type, SCHEDULERDETAILS_Time, SCHEDULERDETAILS_PassengerIDs);
                v = BLL.update_user_vehicle(Device_ID, SCHEDULERDETAILS_PassengerIDs);
            }
            return Json(new { data = b });
        }
        //string VNO, int ROUTES_ID, string SCHEDULER_STARTDATE, string SCHEDULER_ENDDATE, int SCHEDULER_ModifiedBY, string SHIFT_TIME, int SCHEDULER_ID 
        public JsonResult _sheduler_update(string VNO, string ROUTES_ID, string SCHEDULER_STARTDATE, string SCHEDULER_ENDDATE, string SHIFT_TIME, string SCHEDULER_ID, string SHIFT_TYPE)
        {
            Session["SCHEDULER_ID"] = SCHEDULER_ID;
            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            bool b = BLL.sheduler_update(VNO, Convert.ToInt32(ROUTES_ID), SCHEDULER_STARTDATE, SCHEDULER_ENDDATE, Convert.ToInt32(User_ID), SHIFT_TIME, Convert.ToInt32(SCHEDULER_ID), SHIFT_TYPE);

            return Json(new { data = b });
        }
        public JsonResult get_schedulerdetails_id(string SCHEDULERDETAILS_SCHEDULERID, string SHIFT_TIME)
        {
            List<scheduler_details> obj = new List<scheduler_details>();
            DataTable dt = BLL.get_schedulerdetails_schedulerid(Convert.ToInt32(SCHEDULERDETAILS_SCHEDULERID), SHIFT_TIME);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new scheduler_details
                {
                    SCHEDULERDETAILS_ID = Convert.ToInt32(dt.Rows[i]["SCHEDULERDETAILS_ID"]),
                    SCHEDULERDETAILS_STATIONID = Convert.ToInt32(dt.Rows[i]["SCHEDULERDETAILS_STATIONID"]),
                    SCHEDULERDETAILS_Type = dt.Rows[i]["SCHEDULERDETAILS_Type"].ToString(),
                    SCHEDULERDETAILS_Time = dt.Rows[i]["SCHEDULERDETAILS_Time"].ToString(),
                    SCHEDULERDETAILS_PassendersIDs = dt.Rows[i]["SCHEDULERDETAILS_PassengerIDs"].ToString(),
                });
            }
            return Json(new { data = obj });
        }
        //get_schedulerdetails_schedulerid_reverse
        public JsonResult get_schedulerdetails_id_reverse(string SCHEDULERDETAILS_SCHEDULERID, string SHIFT_TIME)
        {
            List<scheduler_details> obj = new List<scheduler_details>();
            DataTable dt = BLL.get_schedulerdetails_schedulerid_reverse(Convert.ToInt32(SCHEDULERDETAILS_SCHEDULERID), SHIFT_TIME);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new scheduler_details
                {
                    SCHEDULERDETAILS_ID = Convert.ToInt32(dt.Rows[i]["SCHEDULERDETAILS_ID"]),
                    SCHEDULERDETAILS_STATIONID = Convert.ToInt32(dt.Rows[i]["SCHEDULERDETAILS_STATIONID"]),
                    SCHEDULERDETAILS_Type = dt.Rows[i]["SCHEDULERDETAILS_Type"].ToString(),
                    SCHEDULERDETAILS_Time = dt.Rows[i]["SCHEDULERDETAILS_Time"].ToString(),
                    SCHEDULERDETAILS_PassendersIDs = dt.Rows[i]["SCHEDULERDETAILS_PassengerIDs"].ToString(),
                });
            }
            return Json(new { data = obj });
        }
        public class scheduler_details
        {
            public int SCHEDULERDETAILS_SCHEDULERID { get; set; }
            public int SCHEDULERDETAILS_STATIONID { get; set; }
            public int SCHEDULERDETAILS_ID { get; set; }
            public string SCHEDULERDETAILS_Type { get; set; }
            public string SCHEDULERDETAILS_Time { get; set; }
            public string SCHEDULERDETAILS_PassendersIDs { get; set; }

        }
        public JsonResult CLOSED_TRIP(string SCHEDULER_ID, string SCHEDULER_ENDDATE)
        {
            bool b = BLL.closed_trip(Convert.ToInt32(SCHEDULER_ID), SCHEDULER_ENDDATE);
            return Json(new { data = true });
        }
        public JsonResult _scheduler()
        {
            List<tblscheduler> obj = new List<tblscheduler>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            // DataTable dt = BLL.get_chk_okrd_sheduler();
            DataTable dt = BLL.get_sheduler(CATEG_ID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblscheduler
                {
                    SCHEDULER_ID = Convert.ToInt32(dt.Rows[i]["SCHEDULER_ID"]),
                    SCHEDULER_ROUTEID = Convert.ToInt32(dt.Rows[i]["SCHEDULER_ROUTEID"]),
                    SCHEDULER_VEHICLENO = dt.Rows[i]["SCHEDULER_VEHICLENo"].ToString(),
                    SCHEDULER_STARTDATE = dt.Rows[i]["sdate"].ToString(),
                    SCHEDULER_ENDDATE = dt.Rows[i]["edate"].ToString(),
                    SHIFT_TIME = dt.Rows[i]["SHIFT_TIME"].ToString(),
                    SCHEDULER_STATUS = Convert.ToBoolean(dt.Rows[i]["SCHEDULER_STATUS"]),
                    ROUTES_NAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                    ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString(),
                    SHIFT_TYPE = dt.Rows[i]["SHIFT_TYPE"].ToString(),
                    SCHEDULER_Categid = Convert.ToInt32(dt.Rows[i]["SCHEDULER_Categid"]),
                    LOGINCATEGID = CATEG_ID,
                });
            }
            return Json(new { data = obj });
        }
        public JsonResult _schedulerbyid(string ID)
        {
            List<tblscheduler> obj = new List<tblscheduler>();

            DataTable dt = BLL.get_sheduler_id(Convert.ToInt32(ID));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblscheduler
                {
                    SCHEDULER_ID = Convert.ToInt32(dt.Rows[i]["SCHEDULER_ID"]),
                    SCHEDULER_ROUTEID = Convert.ToInt32(dt.Rows[i]["SCHEDULER_ROUTEID"]),
                    SCHEDULER_VEHICLENO = dt.Rows[i]["SCHEDULER_VEHICLENo"].ToString(),
                    SCHEDULER_STARTDATE = dt.Rows[i]["sdate"].ToString(),
                    SCHEDULER_ENDDATE = dt.Rows[i]["edate"].ToString(),
                    SHIFT_TIME = dt.Rows[i]["SHIFT_TIME"].ToString(),
                    SHIFT_TYPE = dt.Rows[i]["SHIFT_TYPE"].ToString(),
                    SCHEDULER_STATUS = Convert.ToBoolean(dt.Rows[i]["SCHEDULER_STATUS"]),
                    ROUTES_NAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                });
            }
            return Json(new { data = obj });
        }
        public JsonResult trips_vno(string VNO)
        {
            List<tblscheduler> obj = new List<tblscheduler>();
            DataTable dt = BLL.get_trips_vno(VNO);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblscheduler
                {
                    SCHEDULER_ID = Convert.ToInt32(dt.Rows[i]["SCHEDULER_ID"]),
                    SHIFT_TIME = dt.Rows[i]["SHIFT_TIME"].ToString(),
                });
            }

            return Json(new { data = obj });
        }
        //get_trips_route_shift
        public JsonResult trips_route_shift(string SCHEDULER_ROUTEID, string SHIFT_TIME)
        {
            List<tblscheduler> obj = new List<tblscheduler>();
            DataTable dt = BLL.get_trips_route_shift(Convert.ToInt32(SCHEDULER_ROUTEID), SHIFT_TIME);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblscheduler
                {
                    SCHEDULER_ID = Convert.ToInt32(dt.Rows[i]["SCHEDULER_ID"]),
                    SHIFT_TIME = dt.Rows[i]["SHIFT_TIME"].ToString(),
                });
            }

            return Json(new { data = obj });
        }

        public class tblscheduler
        {
            public int SCHEDULER_ID { get; set; }
            public string SCHEDULER_VEHICLENO { get; set; }
            public int SCHEDULER_ROUTEID { get; set; }
            public string SCHEDULER_STARTDATE { get; set; }
            public string SCHEDULER_ENDDATE { get; set; }
            public int SCHEDULER_Categid { get; set; }
            public bool SCHEDULER_STATUS { get; set; }
            public string SHIFT_TIME { get; set; }
            public string ROUTES_NAME { get; set; }

            public string ROUTES_CODE { get; set; }
            public string SHIFT_TYPE { get; set; }

            public int LOGINCATEGID { get; set; }
        }

        public JsonResult REGENRERATETRIP(string VNO, string DATE)
        {
            DataTable dt = BLL.get_regeneratetrip(VNO, DATE);
            return Json(new { data = "true" });
        }

        [SessionAuthorize]
        public ActionResult passangers()
        {
            return View();
        }
        public JsonResult _passangers()
        {
            List<tblpassanger> obj = new List<tblpassanger>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_passanger(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblpassanger
                {
                    PASSANGER_ID = Convert.ToInt32(dt.Rows[i]["PASSENGER_ID"]),
                    PASSANGER_NAME = dt.Rows[i]["PASSENGER_NAME"].ToString(),
                    PASSANGER_address = dt.Rows[i]["PASSENGER_address"].ToString(),
                    PASSANGER_mobileno = dt.Rows[i]["PASSENGER_mobileno"].ToString(),
                });
            }
            return Json(new { data = obj });
        }
        public JsonResult passanger_insert(string PASSENGER_NAME, string PASSENGER_address, string PASSENGER_mobileNo)
        {
            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            bool b = BLL.passanger_insert(PASSENGER_NAME, Convert.ToInt32(CATEG_ID), PASSENGER_address, PASSENGER_mobileNo, Convert.ToInt32(User_ID));
            return Json(new { data = b });
        }
        public JsonResult passanger_update(string PASSENGER_NAME, string PASSENGER_address, string PASSENGER_mobileNo, string PASSENGER_ID)
        {
            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            bool b = BLL.passanger_update(PASSENGER_NAME, PASSENGER_address, PASSENGER_mobileNo, Convert.ToInt32(User_ID), Convert.ToInt32(PASSENGER_ID));
            return Json(new { data = b });
        }

        public class tblpassanger
        {
            public int PASSANGER_ID { get; set; }
            public string PASSANGER_NAME { get; set; }
            public string PASSANGER_address { get; set; }
            public string PASSANGER_status { get; set; }
            public string PASSANGER_mobileno { get; set; }
            public int PASSANGER_categid { get; set; }

        }
        [SessionAuthorize]
        public ActionResult Actual_Report()
        {
            return View();
        }
        [SessionAuthorize]
        public JsonResult _Distancereport(string Enddate)
        {
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt1 = BLL.SCHEDULER_SUMMARY_REPORT(CATEG_ID, Enddate);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            if (dt1 != null)
            {
                foreach (DataRow dr1 in dt1.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dt1.Columns)
                    {
                        row1.Add(col.ColumnName, dr1[col]);
                    }
                    rows.Add(row1);
                }

            }
            //return Json(rows);

            var jsonData = new
            {
                Report = rows
            };
            return Json(jsonData);
        }
        [SessionAuthorize]
        public ActionResult Device_Report()
        {
            return View();
        }
        //_summaryreport
        public JsonResult _summaryreport(string Enddate)
        {
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.SCHEDULER_DEVICE_REPORT(CATEG_ID, Enddate);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

            }
            //return Json(rows);

            var jsonData = new
            {
                Report = rows
            };
            return Json(jsonData);
        }
        public JsonResult _detailedreport(string Enddate)
        {
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.SCHEDULER_DETAILED_REPORT(CATEG_ID, Enddate);
            List<schedulerreport> OBJ = new List<schedulerreport>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OBJ.Add(new schedulerreport
                {
                    REPORT_VEHICLENO = dt.Rows[i]["REPORT_VEHICLENO"].ToString(),
                    REPORT_ROUTENAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                    REPORT_STATIONPLACE = dt.Rows[i]["REPORT_STATIONPLACE"].ToString(),
                    PLANEDTIME = dt.Rows[i]["REPORT_SCHEDULER_PLANNED_TIME"].ToString(),
                    ACTUALTIME = dt.Rows[i]["REPORT_SCHEDULER_ACTUAL_TIME"].ToString(),
                    SHIFT_TIME = dt.Rows[i]["report_shiftime"].ToString(),
                    REPORT_DATE = dt.Rows[i]["report_date"].ToString(),
                });
            }

            var jsonData = new
            {
                Report = OBJ
            };
            return Json(jsonData);
        }

        public class schedulerreport
        {
            public int REPORT_ID { get; set; }
            public string REPORT_VEHICLENO { get; set; }
            public string ROUTES_NAME { get; set; }
            public string REPORT_ROUTENAME { get; set; }
            public string REPORT_STATIONPLACE { get; set; }
            public string PLANEDTIME { get; set; }
            public string ACTUALTIME { get; set; }
            public string SHIFT_TIME { get; set; }
            public string REPORT_DATE { get; set; }

        }
        [SessionAuthorize]
        public ActionResult Tracking()
        {
            return View();
        }
        [SessionAuthorize]
        public ActionResult TrackingHistory()
        {
            return View();
        }
        public JsonResult Tracking_history(string Startdate, string Enddate, string Dev_ID)
        {
            DataTable dt = new DataTable();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            string Users_id = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID.ToString();
            string Users_categId = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID.ToString();
            string operation = "GETLOC";

            //string start=(Startdate == null ? string.Empty : Convert.ToDateTime(Startdate).ToString("MM/dd/yyyy hh:mm tt"));
            //string End = (Enddate == null ? string.Empty : Convert.ToDateTime(Enddate).ToString("MM/dd/yyyy hh:mm tt"));

            //dt = BLL.get_TripHistory("GETLOC", ((SMVTS_USERS)Session["USERINFO"]).USERS_ID.ToString(), ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID.ToString(), Dev_ID, (Startdate == null ? string.Empty : Convert.ToDateTime(Startdate).ToString("MM/dd/yyyy hh:mm tt")), (Enddate == null ? string.Empty : Convert.ToDateTime(Enddate).ToString("MM/dd/yyyy hh:mm tt")));
            dt = BLL.get_TripHistory(operation, Users_id, Users_categId, Dev_ID, Startdate, Enddate);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_GRIDTRACK
                {
                    LATITUDE = dt.Rows[i]["TRIPDATA_LATITUDE"].ToString(),
                    LONGITUDE = dt.Rows[i]["TRIPDATA_LONGITUDE"].ToString(),
                    PLACE = dt.Rows[i]["ADDRESS"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    TRIPDATAT_SPEED = dt.Rows[i]["TRIPDATA_SPEED"].ToString(),
                    VEHICLE_NO = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                    ODOMETER = Convert.ToInt32(dt.Rows[i]["TRIPDATA_C_ODOMETER"]),
                    TRIPDATA_TRIPDATE = dt.Rows[i]["TRIPDATA_TRIPDATE"].ToString(),
                });
            }
            return Json(new { TripData = obj });
        }

        [SessionAuthorize]
        public ActionResult frm_Categories(string ID)
        {
            System.Text.StringBuilder linesSB = new System.Text.StringBuilder();
            Session["Querystring"] = ID;
            ViewBag.Querystring = ID;
            SMVTS_CATEGORIES _obj_Smvts_categories = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            List<SMVTS_CATEGORIES> obj5 = new List<SMVTS_CATEGORIES>();
            List<SMVTS_COUNTRIES> obj1 = new List<SMVTS_COUNTRIES>();
            List<SMVTS_SALESREPRESENTATIVE> obj2 = new List<SMVTS_SALESREPRESENTATIVE>();
            SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();

            SMVTS_COUNTRIES _obj_Smvts_Countries = new SMVTS_COUNTRIES();
            _obj_Smvts_Countries.OPERATION = operation.Insert;

            SMVTS_SALESREPRESENTATIVE _obj_smvts_salesrep = new SMVTS_SALESREPRESENTATIVE();
            DataTable dt1 = BLL.get_Country(_obj_Smvts_Countries);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj1.Add(new SMVTS_COUNTRIES
                {
                    COUNTRY_NAME = dt1.Rows[i]["COUNTRY_NAME"].ToString(),
                    COUNTRY_ID = Convert.ToInt32(dt1.Rows[i]["COUNTRY_ID"]),
                });
            }
            ViewBag.countries = obj1;
            _obj_smvts_salesrep.OPERATION = operation.SelectAll;
            DataTable dt4 = BLL.get_salesrep_dropdown(_obj_smvts_salesrep);

            for (int j = 0; j < dt4.Rows.Count; j++)
            {
                obj2.Add(new SMVTS_SALESREPRESENTATIVE
                {
                    SALESREP_FULLNAME = dt4.Rows[j]["SALESREP_FULLNAME"].ToString(),
                    SALESREP_USER_ID = Convert.ToInt32(dt4.Rows[j]["SALESREP_USER_ID"]),
                });
            }
            ViewBag.sales = obj2;

            _obj_Smvts_categories.OPERATION = operation.Empty;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID != 1)
            {
                _obj_Smvts_categories.OPERATION = operation.Select;
                _obj_Smvts_categories.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            DataTable dt5 = BLL.get_Categories(_obj_Smvts_categories);

            for (int m = 0; m < dt5.Rows.Count; m++)
            {
                obj5.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = dt5.Rows[m]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt5.Rows[m]["CATEG_ID"]),
                });
            }
            ViewBag.partners = obj5;
            //Get customer packages
            int categ_id = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt_packages = BLL.Get_Packages(categ_id);
            List<SMVTS_CUSTOMERPACKAGE> obj3 = new List<SMVTS_CUSTOMERPACKAGE>();
            for (int k = 0; k < dt_packages.Rows.Count; k++)
            {
                obj3.Add(new SMVTS_CUSTOMERPACKAGE
                {
                    PACKAGE_ID = Convert.ToInt32(dt_packages.Rows[k]["PACKAGE_ID"]),
                    PACKAGE_NAME = dt_packages.Rows[k]["PACKAGE_NAME"].ToString()
                });
            }
            ViewBag.packages = obj3;
            //Get Category Type
            List<CategType> obj_categtype = new List<CategType>();
            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                obj_categtype.Add(new CategType { Type = "Distributor", Value = 5 });
                obj_categtype.Add(new CategType { Type = "Dealer", Value = 6 });
                obj_categtype.Add(new CategType { Type = "Customer", Value = 3 });
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 5)
            {
                obj_categtype.Add(new CategType { Type = "Dealer", Value = 6 });
                obj_categtype.Add(new CategType { Type = "Customer", Value = 3 });
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 6)
            {
                obj_categtype.Add(new CategType { Type = "Customer", Value = 3 });
            }
            ViewBag.categtype = obj_categtype;
            try
            {
                string strQryString = ID;
                DataTable DT = new DataTable();
                _obj_Smvts_Users.USERS_DBNAME = Session["DBNAME"].ToString();
                switch (strQryString)
                {
                    case "PARTNERS":
                        if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
                        {
                            _obj_Smvts_categories.OPERATION = operation.SelectPartners;
                            _obj_Smvts_categories.CATEG_PARENT_ID = 1;
                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);

                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES
                                {
                                    PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                    username = DT.Rows[k]["USERS_USERNAME"].ToString(),
                                    password = BLL.Decrypt(DT.Rows[k]["USERS_PASSWORD"].ToString()),
                                });
                            }

                        }
                        else
                        {
                            _obj_Smvts_categories.OPERATION = operation.SelectPartnersAdmin;
                            //_obj_Smvts_categories.CATEG_CATETYPE_ID = 4;
                            _obj_Smvts_categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);

                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);

                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES()
                                {
                                    PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                    username = DT.Rows[k]["USERS_USERNAME"].ToString(),
                                    password = BLL.Decrypt(DT.Rows[k]["USERS_PASSWORD"].ToString()),
                                });
                            }
                        }
                        ViewBag.partnersandclients = obj;
                        break;
                    case "CLIENTS":

                        if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
                        {
                            _obj_Smvts_categories.OPERATION = operation.SelectClients;

                            _obj_Smvts_categories.CATEG_PARENT_ID = 1;
                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);

                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES
                                {
                                    PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                    username = DT.Rows[k]["USERS_USERNAME"].ToString(),
                                    password = BLL.Decrypt(DT.Rows[k]["USERS_PASSWORD"].ToString())
                                });
                            }

                            ViewBag.partnersandclients = obj;
                        }
                        else //Partner
                        {
                            //_obj_Smvts_categories.CATEG_CATETYPE_ID = 3;
                            _obj_Smvts_categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                            _obj_Smvts_categories.OPERATION = operation.SelectClientsAdmin;
                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);
                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES
                                {
                                    PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                    username = (DT.Rows[k]["USERS_USERNAME"].ToString()),
                                    password = BLL.Decrypt(DT.Rows[k]["USERS_PASSWORD"].ToString()),
                                });
                            }
                            ViewBag.partnersandclients = obj;
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }


            return View();
        }
        public bool Sendmail(string str, string EmailId)
        {

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            //smtpClient.Host = "secure.emailsrvr.com";
            smtpClient.Host = "zsmtp.hybridzimbra.com";
            smtpClient.Credentials = new System.Net.NetworkCredential("smtpmail@dhanushinfotech.com", "Mi8F4hDs!%@");
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress("info@smartvts.com");
            //mailMessage.From = new System.Net.Mail.MailAddress("jaikjadhav@dhanushinfotech.com");
            mailMessage.To.Add(new System.Net.Mail.MailAddress(EmailId));
            mailMessage.Bcc.Add(new System.Net.Mail.MailAddress("info@smartvts.com"));
            mailMessage.Subject = "Customer_ID-" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
            // mailMessage.Subject = "Hi Pragatipadh - " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
            mailMessage.Priority = System.Net.Mail.MailPriority.High;

            //mailMessage.Body = str;

            AlternateView av1 = InlineAttachment(str);
            mailMessage.AlternateViews.Add(av1);
            mailMessage.IsBodyHtml = true;

            // System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(xlfilename1);

            // mailMessage.Attachments.Add(attachment);
            // Write the string to a file.
            string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();


            System.IO.StreamWriter file = new System.IO.StreamWriter(paths + "\\dashboardLOG.txt", true);

            try
            {

                //Attachment mAtt = new Attachment(@"D:\PROJECTS\SMVTS_MAIL\SMVTSMAIL\SMVTSMAIL\dashboard.html");
                //mailMessage.Attachments.Add(mAtt);
                smtpClient.Send(mailMessage);

                //Console.WriteLine("Mail sent." + EmailId + " At " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                file.WriteLine("Error: can not send mail to " + EmailId + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
            }
            file.WriteLine("Mail sent to " + EmailId + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
            file.Close();
            return true;
            //file close
        }

        public AlternateView InlineAttachment(string str)
        {
            string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();
            paths = paths.Remove(paths.LastIndexOf("\\"), paths.Length - paths.LastIndexOf("\\"));
            paths = paths.Remove(paths.LastIndexOf("\\"), paths.Length - paths.LastIndexOf("\\"));
            paths = paths.Remove(paths.LastIndexOf("\\"), paths.Length - paths.LastIndexOf("\\"));

            //string path = paths + "\\image\\car_yellow.gif";
            //string path = paths + "\\Images\\LOGO.JPG";

            string path = Server.MapPath("~/IMAGES/paynowemail_btn.jpg");
            //string path = paths + "\\Images\\paynowemail_btn.jpg";
            LinkedResource logo_Y = new LinkedResource(path);
            logo_Y.ContentId = "companylogo_Y";

            str = str.Replace("@@IMAGE_R@@", "cid:companylogo_Y");


            str = str.Replace("@@IMAGE_Y@@", "cid:companylogo_Y");




            AlternateView av1 = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            av1.LinkedResources.Add(logo_Y);
            //av1.LinkedResources.Add(logo_G);
            //av1.LinkedResources.Add(logo_R);
            return av1;

        }
        public JsonResult getstate(int CountryID)
        {
            List<SMVTS_STATES> obj = new List<SMVTS_STATES>();
            SMVTS_STATES _obj_Smvts_States = new SMVTS_STATES();
            _obj_Smvts_States.OPERATION = operation.Empty;
            _obj_Smvts_States.STATE_COUNTRY_ID = Convert.ToInt32(CountryID);

            DataTable dt = BLL.get_State(_obj_Smvts_States);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_STATES
                {
                    STATE_NAME = dt.Rows[i]["STATE_NAME"].ToString(),
                    STATE_ID = Convert.ToInt32(dt.Rows[i]["STATE_ID"])
                });
            }
            return Json(new { data = obj });
        }
        public JsonResult getcity(int StateID)
        {
            List<SMVTS_CITIES> obj = new List<SMVTS_CITIES>();
            SMVTS_CITIES _obj_Smvts_Cities = new SMVTS_CITIES();
            _obj_Smvts_Cities.OPERATION = operation.Empty;
            _obj_Smvts_Cities.CITY_STATE_ID = Convert.ToInt32(StateID);
            DataTable dt = BLL.get_Cities(_obj_Smvts_Cities);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_CITIES
                {
                    CITY_NAME = dt.Rows[i]["CITY_NAME"].ToString(),
                    CITY_CITYID = Convert.ToInt32(dt.Rows[i]["CITY_CITYID"])
                });
            }
            return Json(new { data = obj });
        }
        public JsonResult getdbname()
        {
            List<SMVTS_Database> obj = new List<SMVTS_Database>();
            SMVTS_Database _obj_database = new SMVTS_Database();

            DataTable dt = BLL.get_Database_Names(_obj_database);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_Database
                {
                    Database_Name = dt.Rows[i]["ARCHIVE_DBNAME"].ToString(),
                    Database_ID = Convert.ToInt32(dt.Rows[i]["ARCHIVEID"])
                });
            }
            //Rcmb_Database.DataTextField = "ARCHIVE_DBNAME";
            //Rcmb_Database.DataValueField = "ARCHIVEID";
            return Json(new { data = obj });
        }
        public JsonResult editcategory(int CATEG_ID)
        {
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            DataTable dt = BLL.get_Categories(new SMVTS_CATEGORIES(Convert.ToInt32(Convert.ToString(CATEG_ID))));

            string dbname = Convert.ToString(dt.Rows[0]["CATEG_DBNAME"]);

            Session["CATEG_PARENT_ID"] = dt.Rows[0]["CATEG_PARENT_ID"];
            string db = BLL.Decrypt(dbname);



            obj.Add(new SMVTS_CATEGORIES
            {
                CATEG_ID = Convert.ToInt32(dt.Rows[0]["CATEG_ID"]),
                CATEG_ADDRESS = Convert.ToString(dt.Rows[0]["CATEG_ADDRESS"]),
                CATEG_CONTACTPERSON = Convert.ToString(dt.Rows[0]["CATEG_CONTACTPERSON"]),
                CATEG_DESC = Convert.ToString(dt.Rows[0]["CATEG_DESC"]),
                CATEG_EMAILID = Convert.ToString(dt.Rows[0]["CATEG_EMAILID"]),
                CATEG_FAX = Convert.ToString(dt.Rows[0]["CATEG_FAX"]),
                CATEG_MOBILENUMBER = Convert.ToString(dt.Rows[0]["CATEG_MOBILENUMBER"]),
                CATEG_NAME = Convert.ToString(dt.Rows[0]["CATEG_NAME"]),
                CATEG_NOOFUSERS = Convert.ToInt32(dt.Rows[0]["CATEG_NOOFUSERS"]),
                CATEG_STATUS = Convert.ToString(Convert.ToString(dt.Rows[0]["CATEG_STATUS"])),
                CATEG_TELEPHONE = Convert.ToString(dt.Rows[0]["CATEG_TELEPHONE"]),
                CATEG_WEBSITENAME = Convert.ToString(dt.Rows[0]["CATEG_WEBSITENAME"]),
                CATEG_ZIPCODE = Convert.ToString(dt.Rows[0]["CATEG_ZIPCODE"]),
                CATEG_UOMSPEED = Convert.ToString(dt.Rows[0]["CATEG_UOMSPEED"]),
                CATEG_UOMVOLUME = Convert.ToString(dt.Rows[0]["CATEG_UOMVOLUME"]),
                CATG_NOOFDEVICES = Convert.ToInt32(dt.Rows[0]["CATEG_NOOFDEVICES"]),
                CATEG_DBNAME = db,
                //Database_ID = Convert.ToInt32(dt.Rows[0]["ARCHIVEID"]),
                CATEG_PARENT_ID = Convert.ToInt32(dt.Rows[0]["CATEG_PARENT_ID"]),
                CATEG_COUNTRY_ID = Convert.ToInt32(dt.Rows[0]["CATEG_COUNTRY_ID"]),
                CATEG_STATE_ID = Convert.ToInt32(dt.Rows[0]["CATEG_STATE_ID"]),
                CATEG_CITY_ID = Convert.ToInt32(dt.Rows[0]["CATEG_CITY_ID"]),
                CATEG_PACKAGE_ID = Convert.ToInt32(dt.Rows[0]["CATEG_PACKAGE_ID"]),
                CATEG_CATETYPE_ID = Convert.ToInt32(dt.Rows[0]["CATEG_CATETYPE_ID"]),
                PARENT_NAME = Convert.ToString(dt.Rows[0]["parentname"])
            });



            return Json(new { data = obj });
        }
        [HttpPost]
        public JsonResult Save_category(FormCollection form)
        {
            string org_name = form["org_categname"]; string parent_name = "";
            if (org_name != "")
            {
                int parent_id = Convert.ToInt32(Session["CATEG_PARENT_ID"]);
                DataTable dt_parent = Dal.ExecuteQuery("select categ_name from SMVTS_CATEGORIES where categ_id=" + parent_id + "");
                parent_name = dt_parent.Rows[0][0].ToString();
            }

            string name = form["CATEG_NAME"];
            string fileurl = "";

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;

                file.SaveAs(Server.MapPath("~/IMAGES/Logos/") + fileName); //File will be saved in application root
                fileurl = "/IMAGES/Logos/" + fileName;
            }

            string strConn = "";
            string data = "";
            string btn = form["btn"];
            int CATEG_PARENT_ID = Convert.ToInt32(form["CATEG_PARENT_ID"]);
            if (btn == "BTN_UPDATE")
            {
                CATEG_PARENT_ID = Convert.ToInt32(Session["CATEG_PARENT_ID"]);
            }


            // Validation For isExists Name
            string Org_Name_DropDown = "";
            DataTable DT = new DataTable();
            int count = 0;
            int Role_Type = 0;
            SMVTS_CATEGORIES _obj_Smvts_Categories = new SMVTS_CATEGORIES();
            _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(CATEG_PARENT_ID);
            _obj_Smvts_Categories.CATEG_NAME = BLL.ReplaceQuote(form["CATEG_NAME"].ToString());
            _obj_Smvts_Categories.OPERATION = operation.Check;

            // if ((lbl_CATEGID.Text) != "")  //Edit case
            _obj_Smvts_Categories.CATEG_ID = Convert.ToInt32(form["CATEG_ID"]);

            DT = BLL.get_Categories(_obj_Smvts_Categories);
            if (DT.Rows.Count > 0)
                count = Convert.ToInt32(DT.Rows[0][0]);

            _obj_Smvts_Categories = new SMVTS_CATEGORIES();

            string strQryString = Session["Querystring"].ToString();

            switch (strQryString)
            {
                case "PARTNERS":
                    _obj_Smvts_Categories.CATEG_NOOFUSERS = 0;
                    Role_Type = 2;
                    _obj_Smvts_Categories.CATEG_WLP = 1;
                    if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
                    {
                        _obj_Smvts_Categories.CATEG_CATETYPE_ID = Convert.ToInt32(form["CATEG_CATETYPE_ID"]);
                        _obj_Smvts_Categories.CATEG_PARENT_ID = 1;

                    }
                    //else if(((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2)
                    //{
                    //    _obj_Smvts_Categories.CATEG_CATETYPE_ID = 4;
                    //    _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                    //    Org_Name_DropDown = ((SMVTS_USERS)Session["USERINFO"]).CATEG_NAME.ToString();
                    //}
                    else
                    {
                        _obj_Smvts_Categories.CATEG_CATETYPE_ID = Convert.ToInt32(form["CATEG_CATETYPE_ID"]);
                        _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                        Org_Name_DropDown = ((SMVTS_USERS)Session["USERINFO"]).CATEG_NAME.ToString();
                    }
                    break;
                case "CLIENTS":
                    Org_Name_DropDown = form["ParentNAME"].ToString();
                    Role_Type = 3;
                    string noofusers = (form["noofusers"]).ToString();
                    if (noofusers == string.Empty)
                    {
                        //  BLL.ShowMessage(this, "No of users required.");
                        // return;
                    }
                    _obj_Smvts_Categories.CATEG_NOOFUSERS = Convert.ToInt32(noofusers);
                    _obj_Smvts_Categories.CATEG_REFER_ID = Convert.ToInt32(form["CATEG_REFER_ID"]);
                    //if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
                    //{
                    //    _obj_Smvts_Categories.CATEG_CATETYPE_ID = 3;
                    //    _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(CATEG_PARENT_ID);

                    //}
                    //else //Partner
                    //{
                    //    _obj_Smvts_Categories.CATEG_CATETYPE_ID = 3;
                    //    _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                    //    _obj_Smvts_Categories.CATEG_WLP = 1;
                    //    Org_Name_DropDown= ((SMVTS_USERS)Session["USERINFO"]).CATEG_NAME.ToString();
                    //}
                    _obj_Smvts_Categories.CATEG_CATETYPE_ID = Convert.ToInt32(form["CATEG_CATETYPE_ID"]);
                    _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);

                    Org_Name_DropDown = ((SMVTS_USERS)Session["USERINFO"]).CATEG_NAME.ToString();

                    break;
                default:
                    break;
            }

            string password = "";
            password = BLL.Encrypt(form["CATEG_MOBILENUMBER"].Replace(" ", ""));
            _obj_Smvts_Categories.CATEG_NAME = BLL.ReplaceQuote(form["CATEG_NAME"].Replace(" ", ""));
            _obj_Smvts_Categories.CATEG_DESC = BLL.ReplaceQuote(form["CATEG_DESC"]);
            _obj_Smvts_Categories.CATEG_CONTACTPERSON = BLL.ReplaceQuote(form["CATEG_CONTACTPERSON"]);
            _obj_Smvts_Categories.CATEG_MOBILENUMBER = BLL.ReplaceQuote(form["CATEG_MOBILENUMBER"].Replace(" ", ""));



            _obj_Smvts_Categories.CATEG_TELEPHONE = BLL.ReplaceQuote(form["CATEG_TELEPHONE"]);
            _obj_Smvts_Categories.CATEG_FAX = BLL.ReplaceQuote(form["CATEG_FAX"]);
            _obj_Smvts_Categories.CATEG_WEBSITENAME = BLL.ReplaceQuote(form["CATEG_WEBSITENAME"].Replace(" ", ""));
            _obj_Smvts_Categories.CATEG_EMAILID = BLL.ReplaceQuote(form["CATEG_EMAILID"].Replace(" ", ""));

            int Email_count = 0;
            DataTable dt_email = Dal.ExecuteQuery_Prod("select categ_id from SMVTS_categories WHERE CATEG_EMAILID='" + _obj_Smvts_Categories.CATEG_EMAILID + "'");
            if (dt_email.Rows.Count > 0)
            {
                Email_count = Convert.ToInt32(dt_email.Rows[0][0]);
            }
            //  string EmailID = BLL.ReplaceQuote(rtxt_CategEmailId.Text.Trim());
            _obj_Smvts_Categories.CATEG_ADDRESS = BLL.ReplaceQuote(form["CATEG_ADDRESS"]);
            _obj_Smvts_Categories.CATEG_COUNTRY_ID = Convert.ToInt32(form["CATEG_COUNTRY_ID"]);
            _obj_Smvts_Categories.CATEG_STATE_ID = Convert.ToInt32(form["CATEG_STATE_ID"]);
            _obj_Smvts_Categories.CATEG_CITY_ID = Convert.ToInt32(form["CATEG_CITY_ID"]);
            _obj_Smvts_Categories.CATEG_ZIPCODE = BLL.ReplaceQuote(form["CATEG_ZIPCODE"]);
            _obj_Smvts_Categories.CATEG_STATUS = form["CATEG_STATUS"];
            _obj_Smvts_Categories.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Categories.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Categories.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Categories.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_Categories.CATEG_UOMSPEED = BLL.ReplaceQuote(form["CATEG_UOMSPEED"]);
            _obj_Smvts_Categories.CATEG_UOMVOLUME = BLL.ReplaceQuote(form["CATEG_UOMVOLUME"]);
            _obj_Smvts_Categories.CATG_NOOFDEVICES = (Convert.ToInt32(BLL.ReplaceQuote(form["CATG_NOOFDEVICES"])));
            _obj_Smvts_Categories.CATEG_REFER_ID = Convert.ToInt32(form["CATEG_REFER_ID"]);
            _obj_Smvts_Categories.Logo_Url = fileurl;
            //_obj_Smvts_Categories.CATEG_PACKAGE_ID = PACKAGE_ID;

            //Get Packages 
            int packageid = 0;
            DataTable dt_package = BLL.Get_PackagesBy_Id(packageid);
            string formIds = "", columnIds = ""; int Num_Days = 0;
            if (dt_package.Rows.Count > 0)
            {
                formIds = dt_package.Rows[0]["PACKAGE_FORM_IDS"].ToString();
                columnIds = dt_package.Rows[0]["PACKAGE_COLUMNIDS"].ToString();
                Num_Days = Convert.ToInt32(dt_package.Rows[0]["NUM_OF_DAYS"].ToString());
            }
            _obj_Smvts_Categories.CATEG_VALID_TO = DateTime.Now.AddDays(Num_Days);
            //string orgname = rtxt_CategName.Text;
            //   string dbname = BLL.getDatabase(orgname);
            string Patnerid = Convert.ToString(CATEG_PARENT_ID);
            string orgname = form["CATEG_NAME"] + "(C)";
            _obj_Smvts_Categories.CATEG_PACKAGE_ID = packageid;
            //  string dbname = BLL.getDatabase_categ(Patnerid);

            string DatabaseName = "";
            if (Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID) == 2)
            {
                DataTable dt_partner = new DataTable();
                dt_partner = BLL.get_partnerconnection(Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
                if (dt_partner.Rows.Count > 0)
                {
                    // Org_Name_DropDown = "Dhanush";
                    DatabaseName = Convert.ToString(dt_partner.Rows[0][0]);
                }
                else
                {
                    DatabaseName = form["Rcmb_Database"].ToString();
                }
            }
            else
            {
                DatabaseName = form["Rcmb_Database"].ToString();
            }
            if (DatabaseName.Length > 12)
            {
                string[] t = DatabaseName.Split(']');
                // string[] r=Regex.Split(DatabaseName,".[");

                string S1 = t[0].Remove(0, 1);
                string S2 = t[1].Remove(0, 2);
                strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
                //strConn = strConn.Replace("Data Source=ctpl.database.windows.net", "Data Source=" + S1);
                strConn = strConn.Replace("Data Source=localhost", "Data Source=" + S1);
                strConn = strConn.Replace("Initial Catalog=VTS_CONFIG", "Initial Catalog=" + S2);
            }
            else
            {
                strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
                strConn = strConn.Replace("Initial Catalog=VTS_CONFIG", "Initial Catalog=" + DatabaseName);
            }

            _obj_Smvts_Categories.CATEG_PRODNAME = DatabaseName;
            _obj_Smvts_Categories.CATEG_DBNAME = BLL.Encrypt(strConn);
            //int CATEGID = _obj_Smvts_Categories.CATEG_ID;
            string categname = _obj_Smvts_Categories.CATEG_NAME;
            DateTime receviedate = DateTime.Now;
            System.Text.StringBuilder linesSB = new System.Text.StringBuilder();
            //linesSB.Append("<html xmlns='http://www.w3.org/1999/xhtml'><head><title></title></head><body bgcolor=white lang=EN-US link=blue vlink=purple style='margin-top:15.0pt;margin-bottom:15.0pt'><div class=WordSection1><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=760 style='width:570.0pt'> <tr><td width=760 valign=top style='width:570.0pt;background:white;padding:15.0pt 15.0pt 15.0pt 15.0pt'><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=50% style='width:100.0%'><tr><td valign=top style='border:solid #DBDFE6 1.0pt;background:white;padding:11.25pt 15.0pt 11.25pt 15.0pt'> <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=100% style='width:100.0%'><tr style='height:13.5pt'><td width=60% valign=top style='width:60.0%;padding:0in 0in 0in 0in;height:13.5pt'><p class=MsoNormal style='line-height:12.0pt'><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><br>#1-8-448, Lakshmi Building, 6th Floor,Begumpet, Secunderabad<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A.P., India, Pin 500 003<br><strong><span style='font-family:Arial,sans-serif'>Email:</span></strong> <a href=\"mailto:info@smartvts.com \">info@smartvts.com</a> <br><strong><span style='font-family:Arial,sans-serif'>Website:</span></strong> <a href=\"http://www.pragativts.com\">http://www.pragativts.com</a> <o:p></o:p></span></p></td><td width=40% valign=top style='width:40.0%;padding:0in 0in 0in 0in;height:13.5pt'><div align=right><table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width=95% style='width:95.0%;border:solid #E4E6EB 1.0pt'><tr><td valign=top style='border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt; border-right-style: none; border-right-color: inherit; border-right-width: medium; border-top-style: none; border-top-color: inherit; border-top-width: medium;' class=\"auto-style1\"><p class=MsoNormal align=right style='text-align:right'><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Client ID:</span></strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>  <o:p></o:p></span></p></td><td width=40% valign=top style='width:40.0%;border-top:none;border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;border-right:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class=MsoNormal style=\"font-size:14px;\">" + Convert.ToInt32(lbl_CATEGID.Text) + "</p></td></tr><tr> <td valign=top style='border-top:none;border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;border-right:none;padding:3.75pt 3.75pt 3.75pt 3.75pt' class=\"auto-style1\"><p class=MsoNormal align=right style='text-align:right'><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Recevied Date:</strong> <o:p></o:p></p></td><td valign=top style='border-top:none;border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;border-right:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class=MsoNormal style=\"font-size:14px;\">" + receviedate + "</p></td></tr></table></div></td></tr></table><p class=MsoNormal><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><o:p>&nbsp;</o:p></span></p><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=100% style='width:100.0%'><tr style='height:26.25pt'><td style='padding:0in 0in 0in 0in;height:26.25pt'><p class=MsoNormal><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Customer ID:</span></strong> <span style=\"font-size:14px;\">" + Convert.ToInt32(lbl_CATEGID.Text) + "</span><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><strong><span style='font-family:Arial,sans-serif'>&nbsp;</p> </td></tr><tr style='height:26.25pt'><td style='padding:0in 0in 0in 0in;height:26.25pt'><p class=MsoNormal><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Customer Name: </span></strong> <span style=\"font-size:14px;\">" + categname + "</span><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><strong><span style='font-family:Arial,sans-serif'>&nbsp;</p></td></tr></table><p class=MsoNormal><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><o:p>&nbsp;</o:p></span></p><p class=MsoNormal style='margin-bottom:12.0pt'><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><o:p>&nbsp;</o:p></span></p> <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=100% style='width:100.0%'><tr><td valign=top style='padding:0in 0in 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>For Pragatipadh Informatics Private Limited<br><br><br>Authorized Signatory<o:p></o:p></span></p></td></tr></table></td></tr></table></div><p class=MsoNormal align=center style='text-align:center'><span style='font-size:9.0pt;font-family:Arial,sans-serif;color:#373737'><br><a href=\"http://pragativts.com/frm_quickpay1.aspx?CategID=" + Convert.ToInt32(lbl_CATEGID.Text) + "&amp;categname=" + categname + " \"><span style='text-decoration:none'><img border=0 width=71 height=26 id=\"_x0000_i1025\" src='@@IMAGE_Y@@' alt=\"Quick Pay\"></span></a><br><br>OR <br><br>Copy/Paste the link in your browser address field <br><a href=\"http://pragativts.com/frm_quickpay1.aspx?CategID=" + Convert.ToInt32(lbl_CATEGID.Text) + "&amp;categname=" + categname + " \">http://pragativts.com/frm_quickpay1.aspx?CategID=" + Convert.ToInt32(lbl_CATEGID.Text) + "&amp;categname=" + categname + "</a> <o:p></o:p></span></p></td></tr></table></div><p class=MsoNormal><o:p>&nbsp;</o:p></p></div></body></html>");


            //if (Checksendmail == true)
            //{
            switch (btn)
            {

                case "BTN_SAVE":
                    try
                    {
                        if (count == 0)
                        {
                            if (Email_count == 0)
                            {

                                bool s = false;
                                _obj_Smvts_Categories.OPERATION = operation.Insert;
                                // _obj_Smvts_Categories.CATEG_DBNAME = "VTS_Master";
                                bool r = BLL.SETCATEGORY(_obj_Smvts_Categories, "", Org_Name_DropDown, formIds, columnIds, Role_Type, password, org_name, parent_name);
                                if (r == true)
                                {
                                    data = "true";
                                    //bool Checksendmail = Convert.ToBoolean(form["Checksendmail"]);
                                    //if (Checksendmail == true)
                                    //{
                                    //    Sendmail(linesSB.ToString(), form["CATEG_EMAILID"]);
                                    //}
                                    //  if (ChkBoxdp.Checked)
                                    //{
                                    //contract
                                    //SMVTS_CONTRACTS _obj_Smvts_Contract = new SMVTS_CONTRACTS();
                                    //string categname1 = _obj_Smvts_Categories.CATEG_NAME;
                                    //DataTable dtc = BLL.getcategoryid(categname1);
                                    //int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);
                                    //_obj_Smvts_Contract.CONTRACTS_DESC = BLL.ReplaceQuote(_obj_Smvts_Categories.CATEG_NAME);
                                    //_obj_Smvts_Contract.CONTRACTS_NAME = BLL.ReplaceQuote(_obj_Smvts_Categories.CATEG_NAME);
                                    //_obj_Smvts_Contract.CONTRACTS_STARTDATE = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                                    //_obj_Smvts_Contract.CONTRACTS_ENDDATE = Convert.ToDateTime(DateTime.Now.AddYears(4).ToString("MM/dd/yyyy"));
                                    //_obj_Smvts_Contract.CONTRACTS_STATUS = Convert.ToBoolean(true);

                                    ////pass categ_name and get contract id
                                    //_obj_Smvts_Contract.CONTRACTS_CATEGORY_ID = Convert.ToInt32(categid);
                                    //_obj_Smvts_Contract.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                                    //_obj_Smvts_Contract.CREATEDDATE = DateTime.Now;
                                    //_obj_Smvts_Contract.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                                    //_obj_Smvts_Contract.LASTMDFDATE = DateTime.Now;

                                    //_obj_Smvts_Contract.OPERATION = operation.Insert;
                                    //string DatabaseName1 = _obj_Smvts_Categories.CATEG_DBNAME;
                                    //if (s == true)
                                    //{
                                    //    SMVTS_ROLES _obj_Smvts_Roles = new SMVTS_ROLES();

                                    //    DataTable dt_role = BLL.getroles();
                                    //    if (dt_role.Rows.Count > 0)
                                    //    {
                                    //        int b = 0;
                                    //        int a = dt_role.Rows.Count;
                                    //        for (int i = 0; i < dt_role.Rows.Count; i++)
                                    //        // while (b <= a - 1)
                                    //        {
                                    //            _obj_Smvts_Roles.OPERATION = operation.Insert;
                                    //            _obj_Smvts_Roles.ROLES_CATEGORY_ID = categid;
                                    //            _obj_Smvts_Roles.ROLES_ROLETYPE = Convert.ToInt32(dt_role.Rows[i]["ROLES_ROLETYPE"]);
                                    //            _obj_Smvts_Roles.ROLES_FORMSID = Convert.ToString(dt_role.Rows[i]["ROLES_FORMSID"]);
                                    //            _obj_Smvts_Roles.ROLES_COLUMNSID = Convert.ToString(dt_role.Rows[i]["ROLES_COLUMNIDS"]);
                                    //            _obj_Smvts_Roles.ROLES_DASHBOARD = Convert.ToString(dt_role.Rows[i]["ROLES_DASHBOARD"]);
                                    //            _obj_Smvts_Roles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ;
                                    //            _obj_Smvts_Roles.CREATEDDATE = DateTime.Now;
                                    //            _obj_Smvts_Roles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ;
                                    //            _obj_Smvts_Roles.LASTMDFDATE = DateTime.Now;
                                    //            bool r1 = BLL.set_Roles1(_obj_Smvts_Roles, DatabaseName1, orgname);
                                    //            //  b++;
                                    //        }
                                    //    }


                                    //}
                                    //bool c = BLL.set_Contracts(_obj_Smvts_Contract, DatabaseName1, orgname);
                                    //if (c == true)
                                    //{
                                    //    //service
                                    //    SMVTS_SERVICECONTRACTS _obj_Smvts_ServCont = new SMVTS_SERVICECONTRACTS();
                                    //    string contractname = _obj_Smvts_Contract.CONTRACTS_NAME;
                                    //    DataTable dts = BLL.getcontractid(contractname);
                                    //    int contractid = Convert.ToInt32(dts.Rows[0]["CONTRACTS_ID"]);
                                    //    DataTable dt_service = BLL.get_service_contract();

                                    //    if (dt_service.Rows.Count > 0)
                                    //    {
                                    //        int j = 0;
                                    //        int i = dt_service.Rows.Count;
                                    //        while (j <= i - 1)
                                    //        {
                                    //            _obj_Smvts_ServCont.OPERATION = operation.Insert;
                                    //            _obj_Smvts_ServCont.SERVCONT_SERVICES_ID = Convert.ToInt32(dt_service.Rows[j]["SERVCONT_SERVICES_ID"]);
                                    //            _obj_Smvts_ServCont.SERVCONT_CONTRACTS_ID = Convert.ToInt32(contractid);
                                    //            _obj_Smvts_ServCont.SERVCONT_STARTDATE = DateTime.Now;
                                    //            _obj_Smvts_ServCont.SERVCONT_ENDDATE = DateTime.Now;
                                    //            _obj_Smvts_ServCont.SERVCONT_STATUS = true;
                                    //            _obj_Smvts_ServCont.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ; // ### Need to Get the Session
                                    //            _obj_Smvts_ServCont.CREATEDDATE = DateTime.Now;
                                    //            _obj_Smvts_ServCont.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ; // ### Need to Get the Session
                                    //            _obj_Smvts_ServCont.LASTMDFDATE = DateTime.Now;

                                    //            s = BLL.set_ServCont1(_obj_Smvts_ServCont, DatabaseName1, contractname, orgname);
                                    //            j++;
                                    //        }


                                    //    }

                                    //}

                                }
                            }
                        }
                        else
                        {
                            switch (strQryString)
                            {
                                case "PARTNERS":
                                    data = "Party name already exist";
                                    break;

                                case "CLIENTS":
                                    data = "Client name already exist";
                                    break;

                                default:
                                    break;
                            }
                            data = "EMail already exist";
                        }

                    }
                    catch (Exception ex)
                    {
                        BLL.errorLog("Categories_Save", ex.ToString());
                    }
                    break;


                case "BTN_UPDATE":
                    try
                    {
                        if ((count == 1) || (count == 0))
                        {
                            _obj_Smvts_Categories.OPERATION = operation.Update;
                            _obj_Smvts_Categories.CATEG_ID = Convert.ToInt32(form["CATEG_ID"]);
                            //_obj_Smvts_Categories.CATEG_DBNAME = dbname;
                            bool r = BLL.SETCATEGORY(_obj_Smvts_Categories, "", Org_Name_DropDown, formIds, columnIds, Role_Type, password, org_name, parent_name);
                            if (r == true)
                            {
                                data = "true";
                                //bool Checksendmail = Convert.ToBoolean(form["Checksendmail"]);
                                //if(Checksendmail == true)
                                //{
                                //    Sendmail(linesSB.ToString(), form["CATEG_EMAILID"].ToString());
                                //}


                            }
                            else
                            {
                                //BLL.ShowMessage(this, BLL.msg_UnSaved);
                                data = "false";
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.errorLog("Categories_Update", ex.ToString());
                    }
                    break;
                default:
                    break;
            }

            return Json(new { data = data });
        }


        public JsonResult Save_Click(bool Checksendmail, string CATEG_NAME, int CATEG_ID, string noofusers,
            string CATEG_DESC, string btn, string CATEG_CONTACTPERSON, string CATEG_MOBILENUMBER, string CATEG_TELEPHONE, string CATEG_FAX,
            string CATEG_WEBSITENAME, string CATEG_EMAILID, string CATEG_ADDRESS, int CATEG_COUNTRY_ID, int CATEG_STATE_ID, int CATEG_CITY_ID, string CATEG_ZIPCODE,
            string CATEG_STATUS, string CATEG_UOMSPEED, string CATEG_UOMVOLUME, string CATG_NOOFDEVICES, int CATEG_REFER_ID, string Rcmb_Database, int CATEG_PARENT_ID, string ParentNAME, int PACKAGE_ID)
        {
            string strConn = "";
            string data = "";
            if (btn == "BTN_UPDATE")
            {
                CATEG_PARENT_ID = Convert.ToInt32(Session["CATEG_PARENT_ID"]);
            }


            // Validation For isExists Name
            string Org_Name_DropDown = "";
            DataTable DT = new DataTable();
            int count = 0;
            int Role_Type = 0;
            SMVTS_CATEGORIES _obj_Smvts_Categories = new SMVTS_CATEGORIES();
            _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(CATEG_PARENT_ID);
            _obj_Smvts_Categories.CATEG_NAME = BLL.ReplaceQuote(CATEG_NAME);
            _obj_Smvts_Categories.OPERATION = operation.Check;

            // if ((lbl_CATEGID.Text) != "")  //Edit case
            _obj_Smvts_Categories.CATEG_ID = Convert.ToInt32(CATEG_ID);

            DT = BLL.get_Categories(_obj_Smvts_Categories);
            if (DT.Rows.Count > 0)
                count = Convert.ToInt32(DT.Rows[0][0]);

            _obj_Smvts_Categories = new SMVTS_CATEGORIES();

            string strQryString = Session["Querystring"].ToString();

            switch (strQryString)
            {
                case "PARTNERS":
                    _obj_Smvts_Categories.CATEG_NOOFUSERS = 0;
                    _obj_Smvts_Categories.CATEG_CATETYPE_ID = 2;
                    _obj_Smvts_Categories.CATEG_PARENT_ID = 1;
                    _obj_Smvts_Categories.CATEG_WLP = 1;
                    Role_Type = 2;
                    break;
                case "CLIENTS":
                    Org_Name_DropDown = ParentNAME;
                    Role_Type = 3;
                    if (noofusers == string.Empty)
                    {
                        //  BLL.ShowMessage(this, "No of users required.");
                        // return;
                    }
                    _obj_Smvts_Categories.CATEG_NOOFUSERS = Convert.ToInt32(noofusers);
                    _obj_Smvts_Categories.CATEG_REFER_ID = Convert.ToInt32(CATEG_REFER_ID);
                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
                    {
                        _obj_Smvts_Categories.CATEG_CATETYPE_ID = 3;
                        _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(CATEG_PARENT_ID);

                    }
                    else //Partner
                    {
                        _obj_Smvts_Categories.CATEG_CATETYPE_ID = 3;
                        _obj_Smvts_Categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                        _obj_Smvts_Categories.CATEG_WLP = 1;
                    }
                    break;
                default:
                    break;
            }

            string password = "";
            password = BLL.Encrypt(CATEG_MOBILENUMBER);
            _obj_Smvts_Categories.CATEG_NAME = BLL.ReplaceQuote(CATEG_NAME);
            _obj_Smvts_Categories.CATEG_DESC = BLL.ReplaceQuote(CATEG_DESC);
            _obj_Smvts_Categories.CATEG_CONTACTPERSON = BLL.ReplaceQuote(CATEG_CONTACTPERSON);
            _obj_Smvts_Categories.CATEG_MOBILENUMBER = BLL.ReplaceQuote(CATEG_MOBILENUMBER);
            _obj_Smvts_Categories.CATEG_TELEPHONE = BLL.ReplaceQuote(CATEG_TELEPHONE);
            _obj_Smvts_Categories.CATEG_FAX = BLL.ReplaceQuote(CATEG_FAX);
            _obj_Smvts_Categories.CATEG_WEBSITENAME = BLL.ReplaceQuote(CATEG_WEBSITENAME);
            _obj_Smvts_Categories.CATEG_EMAILID = BLL.ReplaceQuote(CATEG_EMAILID);
            //  string EmailID = BLL.ReplaceQuote(rtxt_CategEmailId.Text.Trim());
            _obj_Smvts_Categories.CATEG_ADDRESS = BLL.ReplaceQuote(CATEG_ADDRESS);
            _obj_Smvts_Categories.CATEG_COUNTRY_ID = Convert.ToInt32(CATEG_COUNTRY_ID);
            _obj_Smvts_Categories.CATEG_STATE_ID = Convert.ToInt32(CATEG_STATE_ID);
            _obj_Smvts_Categories.CATEG_CITY_ID = Convert.ToInt32(CATEG_CITY_ID);
            _obj_Smvts_Categories.CATEG_ZIPCODE = BLL.ReplaceQuote(CATEG_ZIPCODE);
            _obj_Smvts_Categories.CATEG_STATUS = CATEG_STATUS;
            _obj_Smvts_Categories.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Categories.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Categories.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Categories.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_Categories.CATEG_UOMSPEED = BLL.ReplaceQuote(CATEG_UOMSPEED);
            _obj_Smvts_Categories.CATEG_UOMVOLUME = BLL.ReplaceQuote(CATEG_UOMVOLUME);
            _obj_Smvts_Categories.CATG_NOOFDEVICES = (Convert.ToInt32(BLL.ReplaceQuote(CATG_NOOFDEVICES)));
            _obj_Smvts_Categories.CATEG_REFER_ID = Convert.ToInt32(CATEG_REFER_ID);
            // _obj_Smvts_Categories.CATEG_PACKAGE_ID = PACKAGE_ID;

            //Get Packages 
            int packageid = Convert.ToInt32(PACKAGE_ID);
            DataTable dt_package = BLL.Get_PackagesBy_Id(packageid);
            string formIds = "", columnIds = "";
            if (dt_package.Rows.Count > 0)
            {
                formIds = dt_package.Rows[0][0].ToString();
                columnIds = dt_package.Rows[0][1].ToString();
            }

            //string orgname = rtxt_CategName.Text;
            //   string dbname = BLL.getDatabase(orgname);
            string Patnerid = Convert.ToString(CATEG_PARENT_ID);
            string orgname = CATEG_NAME + "(C)";

            //  string dbname = BLL.getDatabase_categ(Patnerid);

            string DatabaseName = "";
            if (Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID) == 2)
            {
                DataTable dt_partner = new DataTable();
                dt_partner = BLL.get_partnerconnection(Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
                if (dt_partner.Rows.Count > 0)
                {
                    // Org_Name_DropDown = "Dhanush";
                    DatabaseName = Convert.ToString(dt_partner.Rows[0][0]);
                }
                else
                {
                    DatabaseName = Rcmb_Database;
                }
            }
            else
            {
                DatabaseName = Rcmb_Database;
            }
            if (DatabaseName.Length > 12)
            {
                string[] t = DatabaseName.Split(']');
                // string[] r=Regex.Split(DatabaseName,".[");

                string S1 = t[0].Remove(0, 1);
                string S2 = t[1].Remove(0, 2);
                strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
                strConn = strConn.Replace("Data Source=192.168.1.26", "Data Source=" + S1);
                strConn = strConn.Replace("Initial Catalog=VTS_CONFIG", "Initial Catalog=" + S2);
            }
            else
            {
                strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
                strConn = strConn.Replace("Initial Catalog=VTS_CONFIG", "Initial Catalog=" + DatabaseName);
            }

            _obj_Smvts_Categories.CATEG_PRODNAME = DatabaseName;
            _obj_Smvts_Categories.CATEG_DBNAME = BLL.Encrypt(strConn);
            //int CATEGID = _obj_Smvts_Categories.CATEG_ID;
            string categname = _obj_Smvts_Categories.CATEG_NAME;
            DateTime receviedate = DateTime.Now;
            System.Text.StringBuilder linesSB = new System.Text.StringBuilder();
            //linesSB.Append("<html xmlns='http://www.w3.org/1999/xhtml'><head><title></title></head><body bgcolor=white lang=EN-US link=blue vlink=purple style='margin-top:15.0pt;margin-bottom:15.0pt'><div class=WordSection1><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=760 style='width:570.0pt'> <tr><td width=760 valign=top style='width:570.0pt;background:white;padding:15.0pt 15.0pt 15.0pt 15.0pt'><div align=center><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=50% style='width:100.0%'><tr><td valign=top style='border:solid #DBDFE6 1.0pt;background:white;padding:11.25pt 15.0pt 11.25pt 15.0pt'> <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=100% style='width:100.0%'><tr style='height:13.5pt'><td width=60% valign=top style='width:60.0%;padding:0in 0in 0in 0in;height:13.5pt'><p class=MsoNormal style='line-height:12.0pt'><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><br>#1-8-448, Lakshmi Building, 6th Floor,Begumpet, Secunderabad<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A.P., India, Pin 500 003<br><strong><span style='font-family:Arial,sans-serif'>Email:</span></strong> <a href=\"mailto:info@smartvts.com \">info@smartvts.com</a> <br><strong><span style='font-family:Arial,sans-serif'>Website:</span></strong> <a href=\"http://www.pragativts.com\">http://www.pragativts.com</a> <o:p></o:p></span></p></td><td width=40% valign=top style='width:40.0%;padding:0in 0in 0in 0in;height:13.5pt'><div align=right><table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width=95% style='width:95.0%;border:solid #E4E6EB 1.0pt'><tr><td valign=top style='border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt; border-right-style: none; border-right-color: inherit; border-right-width: medium; border-top-style: none; border-top-color: inherit; border-top-width: medium;' class=\"auto-style1\"><p class=MsoNormal align=right style='text-align:right'><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Client ID:</span></strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>  <o:p></o:p></span></p></td><td width=40% valign=top style='width:40.0%;border-top:none;border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;border-right:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class=MsoNormal style=\"font-size:14px;\">" + Convert.ToInt32(lbl_CATEGID.Text) + "</p></td></tr><tr> <td valign=top style='border-top:none;border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;border-right:none;padding:3.75pt 3.75pt 3.75pt 3.75pt' class=\"auto-style1\"><p class=MsoNormal align=right style='text-align:right'><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Recevied Date:</strong> <o:p></o:p></p></td><td valign=top style='border-top:none;border-left:solid #E4E6EB 1.0pt;border-bottom:solid #E4E6EB 1.0pt;border-right:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class=MsoNormal style=\"font-size:14px;\">" + receviedate + "</p></td></tr></table></div></td></tr></table><p class=MsoNormal><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><o:p>&nbsp;</o:p></span></p><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=100% style='width:100.0%'><tr style='height:26.25pt'><td style='padding:0in 0in 0in 0in;height:26.25pt'><p class=MsoNormal><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Customer ID:</span></strong> <span style=\"font-size:14px;\">" + Convert.ToInt32(lbl_CATEGID.Text) + "</span><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><strong><span style='font-family:Arial,sans-serif'>&nbsp;</p> </td></tr><tr style='height:26.25pt'><td style='padding:0in 0in 0in 0in;height:26.25pt'><p class=MsoNormal><strong><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>Customer Name: </span></strong> <span style=\"font-size:14px;\">" + categname + "</span><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><strong><span style='font-family:Arial,sans-serif'>&nbsp;</p></td></tr></table><p class=MsoNormal><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><o:p>&nbsp;</o:p></span></p><p class=MsoNormal style='margin-bottom:12.0pt'><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'><o:p>&nbsp;</o:p></span></p> <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=100% style='width:100.0%'><tr><td valign=top style='padding:0in 0in 0in 0in'><p class=MsoNormal align=right style='text-align:right'><span style='font-size:8.5pt;font-family:Arial,sans-serif;color:#373737'>For Pragatipadh Informatics Private Limited<br><br><br>Authorized Signatory<o:p></o:p></span></p></td></tr></table></td></tr></table></div><p class=MsoNormal align=center style='text-align:center'><span style='font-size:9.0pt;font-family:Arial,sans-serif;color:#373737'><br><a href=\"http://pragativts.com/frm_quickpay1.aspx?CategID=" + Convert.ToInt32(lbl_CATEGID.Text) + "&amp;categname=" + categname + " \"><span style='text-decoration:none'><img border=0 width=71 height=26 id=\"_x0000_i1025\" src='@@IMAGE_Y@@' alt=\"Quick Pay\"></span></a><br><br>OR <br><br>Copy/Paste the link in your browser address field <br><a href=\"http://pragativts.com/frm_quickpay1.aspx?CategID=" + Convert.ToInt32(lbl_CATEGID.Text) + "&amp;categname=" + categname + " \">http://pragativts.com/frm_quickpay1.aspx?CategID=" + Convert.ToInt32(lbl_CATEGID.Text) + "&amp;categname=" + categname + "</a> <o:p></o:p></span></p></td></tr></table></div><p class=MsoNormal><o:p>&nbsp;</o:p></p></div></body></html>");


            //if (Checksendmail == true)
            //{
            switch (btn)
            {

                case "BTN_SAVE":
                    try
                    {
                        if (count == 0)
                        {

                            bool s = false;
                            _obj_Smvts_Categories.OPERATION = operation.Insert;
                            // _obj_Smvts_Categories.CATEG_DBNAME = "VTS_Master";
                            bool r = BLL.SETCATEGORY(_obj_Smvts_Categories, "", Org_Name_DropDown, formIds, columnIds, Role_Type, password, "", "");
                            if (r == true)
                            {
                                data = "true";
                                if (Checksendmail == true)
                                {
                                    Sendmail(linesSB.ToString(), CATEG_EMAILID);
                                }
                                //  if (ChkBoxdp.Checked)
                                //{
                                //contract
                                SMVTS_CONTRACTS _obj_Smvts_Contract = new SMVTS_CONTRACTS();
                                string categname1 = _obj_Smvts_Categories.CATEG_NAME;
                                DataTable dtc = BLL.getcategoryid(categname1);
                                int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);
                                _obj_Smvts_Contract.CONTRACTS_DESC = BLL.ReplaceQuote(_obj_Smvts_Categories.CATEG_NAME);
                                _obj_Smvts_Contract.CONTRACTS_NAME = BLL.ReplaceQuote(_obj_Smvts_Categories.CATEG_NAME);
                                _obj_Smvts_Contract.CONTRACTS_STARTDATE = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                                _obj_Smvts_Contract.CONTRACTS_ENDDATE = Convert.ToDateTime(DateTime.Now.AddYears(4).ToString("MM/dd/yyyy"));
                                _obj_Smvts_Contract.CONTRACTS_STATUS = Convert.ToBoolean(true);

                                //pass categ_name and get contract id
                                _obj_Smvts_Contract.CONTRACTS_CATEGORY_ID = Convert.ToInt32(categid);
                                _obj_Smvts_Contract.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                                _obj_Smvts_Contract.CREATEDDATE = DateTime.Now;
                                _obj_Smvts_Contract.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                                _obj_Smvts_Contract.LASTMDFDATE = DateTime.Now;

                                _obj_Smvts_Contract.OPERATION = operation.Insert;
                                string DatabaseName1 = _obj_Smvts_Categories.CATEG_DBNAME;
                                if (s == true)
                                {
                                    SMVTS_ROLES _obj_Smvts_Roles = new SMVTS_ROLES();

                                    DataTable dt_role = BLL.getroles();
                                    if (dt_role.Rows.Count > 0)
                                    {
                                        int b = 0;
                                        int a = dt_role.Rows.Count;
                                        for (int i = 0; i < dt_role.Rows.Count; i++)
                                        // while (b <= a - 1)
                                        {
                                            _obj_Smvts_Roles.OPERATION = operation.Insert;
                                            _obj_Smvts_Roles.ROLES_CATEGORY_ID = categid;
                                            _obj_Smvts_Roles.ROLES_ROLETYPE = Convert.ToInt32(dt_role.Rows[i]["ROLES_ROLETYPE"]);
                                            _obj_Smvts_Roles.ROLES_FORMSID = Convert.ToString(dt_role.Rows[i]["ROLES_FORMSID"]);
                                            _obj_Smvts_Roles.ROLES_COLUMNSID = Convert.ToString(dt_role.Rows[i]["ROLES_COLUMNIDS"]);
                                            _obj_Smvts_Roles.ROLES_DASHBOARD = Convert.ToString(dt_role.Rows[i]["ROLES_DASHBOARD"]);
                                            _obj_Smvts_Roles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ;
                                            _obj_Smvts_Roles.CREATEDDATE = DateTime.Now;
                                            _obj_Smvts_Roles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ;
                                            _obj_Smvts_Roles.LASTMDFDATE = DateTime.Now;
                                            bool r1 = BLL.set_Roles1(_obj_Smvts_Roles, DatabaseName1, orgname);
                                            //  b++;
                                        }
                                    }


                                }
                                //bool c = BLL.set_Contracts(_obj_Smvts_Contract, DatabaseName1, orgname);
                                //if (c == true)
                                //{
                                //    //service
                                //    SMVTS_SERVICECONTRACTS _obj_Smvts_ServCont = new SMVTS_SERVICECONTRACTS();
                                //    string contractname = _obj_Smvts_Contract.CONTRACTS_NAME;
                                //    DataTable dts = BLL.getcontractid(contractname);
                                //    int contractid = Convert.ToInt32(dts.Rows[0]["CONTRACTS_ID"]);
                                //    DataTable dt_service = BLL.get_service_contract();

                                //    if (dt_service.Rows.Count > 0)
                                //    {
                                //        int j = 0;
                                //        int i = dt_service.Rows.Count;
                                //        while (j <= i - 1)
                                //        {
                                //            _obj_Smvts_ServCont.OPERATION = operation.Insert;
                                //            _obj_Smvts_ServCont.SERVCONT_SERVICES_ID = Convert.ToInt32(dt_service.Rows[j]["SERVCONT_SERVICES_ID"]);
                                //            _obj_Smvts_ServCont.SERVCONT_CONTRACTS_ID = Convert.ToInt32(contractid);
                                //            _obj_Smvts_ServCont.SERVCONT_STARTDATE = DateTime.Now;
                                //            _obj_Smvts_ServCont.SERVCONT_ENDDATE = DateTime.Now;
                                //            _obj_Smvts_ServCont.SERVCONT_STATUS = true;
                                //            _obj_Smvts_ServCont.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ; // ### Need to Get the Session
                                //            _obj_Smvts_ServCont.CREATEDDATE = DateTime.Now;
                                //            _obj_Smvts_ServCont.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID; ; // ### Need to Get the Session
                                //            _obj_Smvts_ServCont.LASTMDFDATE = DateTime.Now;

                                //            s = BLL.set_ServCont1(_obj_Smvts_ServCont, DatabaseName1, contractname, orgname);
                                //            j++;
                                //        }


                                //    }

                                //}

                            }
                        }
                        else
                        {
                            switch (strQryString)
                            {
                                case "PARTNERS":
                                    data = "Party name already exist";
                                    break;

                                case "CLIENTS":
                                    data = "Client name already exist";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.errorLog("Categories_Save", ex.ToString());
                    }
                    break;


                case "BTN_UPDATE":
                    try
                    {
                        if ((count == 1) || (count == 0))
                        {
                            _obj_Smvts_Categories.OPERATION = operation.Update;
                            _obj_Smvts_Categories.CATEG_ID = Convert.ToInt32(CATEG_ID);
                            //_obj_Smvts_Categories.CATEG_DBNAME = dbname;
                            bool r = BLL.SETCATEGORY(_obj_Smvts_Categories, "", Org_Name_DropDown, formIds, columnIds, Role_Type, password, "", "");
                            if (r == true)
                            {
                                data = "true";
                                if (Checksendmail == true)
                                {
                                    Sendmail(linesSB.ToString(), CATEG_EMAILID);
                                }


                            }
                            else
                            {
                                //BLL.ShowMessage(this, BLL.msg_UnSaved);
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.errorLog("Categories_Update", ex.ToString());
                    }
                    break;
                default:
                    break;
            }
            //   }
            return Json(new { data = data });
        }
        [SessionAuthorize]
        public ActionResult frm_Users()
        {
            SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();
            List<SMVTS_USERS> obj = new List<SMVTS_USERS>();



            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID == 1)
            {
                _obj_Smvts_Users.OPERATION = operation.Delete;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 4)
            {
                _obj_Smvts_Users.OPERATION = operation.Select;
                _obj_Smvts_Users.USERS_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 3) //client admin
            {
                _obj_Smvts_Users.OPERATION = operation.Update;
                _obj_Smvts_Users.USERS_DEVICE_IDS = "0";
                _obj_Smvts_Users.USERS_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Users.USERS_ROLE_ID = 4;
            }
            else
            {
                _obj_Smvts_Users.OPERATION = operation.Update;
                _obj_Smvts_Users.USERS_DEVICE_IDS = "0";
                _obj_Smvts_Users.USERS_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Users.USERS_ROLE_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID;
            }
            DataTable DT = BLL.get_Users(_obj_Smvts_Users);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj.Add(new SMVTS_USERS
                {
                    USERS_ID = Convert.ToInt32(DT.Rows[i]["USERS_ID"]),
                    USERS_USERNAME = DT.Rows[i]["USERS_USERNAME"].ToString(),
                    USERS_FULLNAME = DT.Rows[i]["USERS_FULLNAME"].ToString(),
                    USERROLE_NAME = DT.Rows[i]["USERROLE_NAME"].ToString(),
                    USERS_PASSWORD = BLL.Decrypt(DT.Rows[i]["USERS_PASSWORD"].ToString()),
                    USERS_STATUS = DT.Rows[i]["USERS_STATUS"].ToString(),
                    CATEG_NAME = DT.Rows[i]["CATEG_NAME"].ToString(),
                    Mobilenumber = DT.Rows[i]["categ_mobilenumber"].ToString(),
                    Email = DT.Rows[i]["categ_emailid"].ToString(),
                });
            }
            ViewBag.users = obj;
            return View();
        }

        public JsonResult Bindroles()
        {
            List<SMVTS_USERROLES> obj = new List<SMVTS_USERROLES>();
            SMVTS_USERROLES _obj_Smvts_UserRoles = new SMVTS_USERROLES();



            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)  //Org
            {
                _obj_Smvts_UserRoles.OPERATION = operation.Update;
                _obj_Smvts_UserRoles.USERROLE_CATEGORYTYPE_ID = Convert.ToInt32("2");
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 5)
            {

                _obj_Smvts_UserRoles.OPERATION = operation.SelectRoleTypes;
            }
            else //client
            {
                _obj_Smvts_UserRoles.OPERATION = operation.SelectRoleTypes;
            }

            DataTable dt = BLL.get_UserRoles(_obj_Smvts_UserRoles);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_USERROLES
                {
                    USERROLE_NAME = dt.Rows[i]["USERROLE_NAME"].ToString(),
                    USERROLE_ID = Convert.ToInt32(dt.Rows[i]["USERROLE_ID"]),
                });
            }

            return Json(new { data = obj });
        }
        //Eidted by Ajith
        public JsonResult bindclient(int ID)
        {
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            if (Convert.ToInt32(ID) != 0)
            {
                SMVTS_CATEGORIES _obj_Smvts_Categories = new SMVTS_CATEGORIES();


                if (Convert.ToInt32(ID) == 2)   //Partners admin
                {
                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
                    {

                        _obj_Smvts_Categories.OPERATION = operation.SelectPartners;
                    }
                    else
                    {
                        _obj_Smvts_Categories.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                        _obj_Smvts_Categories.OPERATION = operation.SelectPartnersAdmin;
                    }


                }
                else if ((Convert.ToInt32(ID) == 3))    //Cleint admin
                {
                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
                    {
                        _obj_Smvts_Categories.OPERATION = operation.SelectClients;
                    }
                    else
                    {
                        _obj_Smvts_Categories.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                        _obj_Smvts_Categories.OPERATION = operation.SelectClientsAdmin;
                    }


                }
                else
                {
                    _obj_Smvts_Categories.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID; ;
                    _obj_Smvts_Categories.OPERATION = operation.Select;
                }

                //if(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID!=1)
                //{
                //    _obj_Smvts_Categories.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                //    _obj_Smvts_Categories.OPERATION = operation.Select;
                //    _obj_Smvts_Categories.CATEG_STATUS = "1";

                //}

                DataTable dt = BLL.GetCategoriesForUserCreation(_obj_Smvts_Categories);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                    });
                }
            }
            else
            {

            }
            return Json(new { data = obj });
        }

        public JsonResult getCustomerForDeviceShift()
        {
            DataTable dt = new DataTable();
            dt = BLL.getCustomersforDeviceShift();
            List<shift_device_customers> obj = new List<shift_device_customers>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new shift_device_customers
                    {
                        categ_id = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        customerName = dt.Rows[i]["categ_name"].ToString() + "(C)",
                        DealerName = dt.Rows[i]["DealerName"].ToString() + "(P)"
                    });
                }

            }
            return Json(new { data = obj });
        }
        public JsonResult btn_User_Save_Click(string btn, string ClientName, int USER_CATEG_ID, string USER_NAME, string USER_ID, string USER_FULLNAME, string USER_PASSWORD, string USER_TYPE, string USER_STATUS, string ORG_USERNAME)
        {
            string data = "";

            DataTable DT = new DataTable();
            int count = 0; int flag = 0;
            // cntPartner = 0, cntClientAdmin = 0;
            SMVTS_USERS _obj_Smvts_Users_check = new SMVTS_USERS();


            int CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            int CATEG_TYPEID = 0;
            //Checking for Already Existing Users
            SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();
            _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);

            //if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)  //Org
            //{
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
            //}
            //else if(((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2)
            //    {
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
            //}
            //else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 4)
            //{
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
            //}
            //else
            //{
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            //}
            SMVTS_CATEGORIES obj_categ = new SMVTS_CATEGORIES();
            obj_categ.CATEG_ID = Convert.ToInt32(USER_CATEG_ID);
            obj_categ.OPERATION = operation.SelectCatgType;
            DataTable dt_categ = BLL.GetCategoriesForUserCreation(obj_categ);
            if (dt_categ.Rows.Count > 0)
            {
                CATEG_TYPEID = Convert.ToInt32(dt_categ.Rows[0][0]);
            }

            _obj_Smvts_Users.USERS_USERNAME = BLL.ReplaceQuote(USER_NAME);
            _obj_Smvts_Users.OPERATION = operation.Check;
            //if ((lbl_UserID.Text) != "")  //Edit case
            //{
            //    _obj_Smvts_Users.USERS_ID = Convert.ToInt32(USER_ID);
            //}

            DT = BLL.get_Users(_obj_Smvts_Users);

            if (DT.Rows.Count > 0)
            {
                count = Convert.ToInt32(DT.Rows[0][0]);
            }


            //------- saving code
            _obj_Smvts_Users = new SMVTS_USERS();
            _obj_Smvts_Users.USERS_FULLNAME = BLL.ReplaceQuote(USER_FULLNAME).Replace(" ", "");
            _obj_Smvts_Users.USERS_PASSWORD = BLL.Encrypt(BLL.ReplaceQuote(USER_PASSWORD).Replace(" ", ""));
            _obj_Smvts_Users.USERS_USERNAME = BLL.ReplaceQuote(USER_NAME.Replace(" ", ""));


            _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
            _obj_Smvts_Users.USERS_ROLE_ID = Convert.ToInt32(USER_TYPE);
            flag = 1;

            //if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)  //Org
            //{
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
            //    _obj_Smvts_Users.USERS_ROLE_ID = Convert.ToInt32(USER_TYPE);
            //    flag = 1;
            //}
            //else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2)
            //{
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
            //    _obj_Smvts_Users.USERS_ROLE_ID = Convert.ToInt32(USER_TYPE);
            //    flag = 1;
            //}
            //else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 4)
            //{
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
            //    _obj_Smvts_Users.USERS_ROLE_ID = Convert.ToInt32(USER_TYPE);
            //    flag = 1;
            //}
            //else //client admin
            //{
            //    _obj_Smvts_Users.USERS_CATEGORY_ID = (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            //    _obj_Smvts_Users.USERS_ROLE_ID = Convert.ToInt32(USER_TYPE); 
            //    flag = 1;
            //}

            _obj_Smvts_Users.USERS_STATUS = Convert.ToString(USER_STATUS);
            //_obj_Smvts_Users.USERS_PERMISSION = lbl_panel1.Text;
            _obj_Smvts_Users.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Users.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Users.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Users.LASTMDFDATE = DateTime.Now;

            string strDeviceId = String.Empty;
            //if ((rlst_DeviceSource.Items.Count > 0) || (rlst_DeviceDestination.Items.Count > 0))
            //{
            //    for (int idx = 0; idx < rlst_DeviceDestination.Items.Count; idx++)
            //    {
            //        strDeviceId = strDeviceId + rlst_DeviceDestination.Items[idx].Value;

            //        if (idx != rlst_DeviceDestination.Items.Count - 1)
            //            strDeviceId = strDeviceId + ",";
            //    }
            //}
            if (strDeviceId == "")
                _obj_Smvts_Users.USERS_DEVICE_IDS = "0";
            else
                _obj_Smvts_Users.USERS_DEVICE_IDS = strDeviceId;

            string dbname = "";
            string orgname = "";
            if (flag == 1)
            {
                orgname = ClientName;
                //  BLL obj = new BLL();
                dbname = BLL.getDatabase(orgname);

            }
            if (_obj_Smvts_Users.USERS_ROLE_ID == 4)
            {
                orgname = ClientName;
                dbname = ((SMVTS_USERS)Session["USERINFO"]).USERS_DBNAME;
            }
            switch (btn)
            {
                case "BTN_SAVE":

                    if (count == 0)
                    {
                        if (count == 0 && flag == 1)
                        {
                            _obj_Smvts_Users.OPERATION = operation.Insert;

                            if (BLL.set_Users(_obj_Smvts_Users, dbname, orgname, CATEG_ID, CATEG_TYPEID, ORG_USERNAME))
                            {
                                data = "true";
                                //BLL.ShowMessage(this, BLL.msg_Saved);
                            }
                            else
                            {
                                data = "false";
                                //BLL.ShowMessage(this, BLL.msg_UnSaved);
                                //Rm_Users_page.SelectedIndex = 0;
                                //LoadGrid();
                                //Rg_Users.DataBind();
                            }

                        }

                        else if (count == 0 && _obj_Smvts_Users.USERS_ROLE_ID == 4)
                        {
                            _obj_Smvts_Users.OPERATION = operation.Insert;
                            if (BLL.set_Users_users(_obj_Smvts_Users, dbname, orgname))
                            {
                                //BLL.ShowMessage(this, BLL.msg_Saved);
                                data = "true";
                            }

                            else
                            {
                                data = "false";
                                //BLL.ShowMessage(this, BLL.msg_UnSaved);

                            }

                            //rtxt_UserName.Focus();

                            //Rm_Users_page.SelectedIndex = 0;
                            //LoadGrid();
                            //Rg_Users.DataBind();
                            //return;
                        }
                        else
                        {
                            _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
                            _obj_Smvts_Users.OPERATION = operation.Insert;
                            BLL.SET_USERS(_obj_Smvts_Users, dbname);
                            //BLL.ShowMessage(this, BLL.msg_Saved);
                            //rtxt_UserName.Focus();
                            //return;
                            //Rm_Users_page.SelectedIndex = 0;
                            //LoadGrid();
                            //Rg_Users.DataBind();
                        }
                    }
                    else
                    {
                        data = "User with this username already exists";
                    }
                    break;


                case "BTN_UPDATE":
                    if ((count == 0) || (count == 1))
                    {
                        _obj_Smvts_Users.OPERATION = operation.Update;
                        _obj_Smvts_Users.USERS_ID = Convert.ToInt32(USER_ID);
                        if (BLL.set_Users(_obj_Smvts_Users, dbname, orgname, CATEG_ID, CATEG_TYPEID, ORG_USERNAME))
                        {
                            //BLL.ShowMessage(this, BLL.msg_Updated);
                            data = "true";
                        }
                        else
                        {
                            data = "false";
                            //BLL.ShowMessage(this, BLL.msg_UnSaved);
                            //Rm_Users_page.SelectedIndex = 0;
                            //LoadGrid();
                            //Rg_Users.DataBind();
                        }

                    }
                    else
                    {
                        _obj_Smvts_Users.USERS_CATEGORY_ID = Convert.ToInt32(USER_CATEG_ID);
                        bool b = BLL.SET_USERS(_obj_Smvts_Users, "");
                        data = "true";
                        //BLL.ShowMessage(this, BLL.msg_Updated);
                        //rtxt_UserName.Focus();
                        //return;
                        //Rm_Users_page.SelectedIndex = 0;
                        //LoadGrid();
                        //Rg_Users.DataBind();
                    }
                    break;

                default:
                    break;

            }

            return Json(new { data = data });
        }
        public JsonResult frm_edit_user(int USER_ID)
        {
            List<SMVTS_USERS> obj1 = new List<SMVTS_USERS>();

            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 11)
            {

            }
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)  //Org
            {

            }


            else if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID == 2)
            {

            }

            else //client admin
            {

            }

            DataTable dt = BLL.get_Users(new SMVTS_USERS(USER_ID));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string password = Convert.ToString(dt.Rows[i]["USERS_PASSWORD"]);
                string status = Convert.ToString(dt.Rows[i]["USERS_STATUS"]);
                password = BLL.Decrypt(password);
                obj1.Add(new SMVTS_USERS
                {
                    USERS_ID = Convert.ToInt32(dt.Rows[i]["USERS_ID"]),
                    USERROLE_NAME = Convert.ToString(dt.Rows[i]["USERROLE_NAME"]),
                    USERS_ROLE_ID = Convert.ToInt32(dt.Rows[i]["USERS_ROLE_ID"]),
                    USERS_CATEGORY_ID = Convert.ToInt32(dt.Rows[i]["USERS_CATEGORY_ID"]),
                    USERS_USERNAME = Convert.ToString(dt.Rows[i]["USERS_USERNAME"]),
                    USERS_FULLNAME = Convert.ToString(dt.Rows[i]["USERS_FULLNAME"]),
                    USERS_PASSWORD = password,
                    USER_ST = status,
                    CATEG_NAME = Convert.ToString(dt.Rows[i]["CATEG_NAME"])
                });
            }
            return Json(new { data = obj1 });

        }
        [SessionAuthorize]
        public ActionResult frm_Roles()
        {
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            List<SMVTS_CATEGORIES> obj1 = new List<SMVTS_CATEGORIES>();
            SMVTS_CATEGORIES _obj_Smvts_Categories = new SMVTS_CATEGORIES();
            DataTable DT = BLL.get_Roles(new SMVTS_ROLES());


            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = DT.Rows[i]["CATEG_NAME"].ToString(),
                    USERROLE_NAME = DT.Rows[i]["USERROLE_NAME"].ToString(),
                    ROLES_CATEGORY_ID = Convert.ToInt32(DT.Rows[i]["ROLES_CATEGORY_ID"]),
                    ROLES_ID = Convert.ToInt32(DT.Rows[i]["ROLES_ID"]),
                });
            }


            _obj_Smvts_Categories.OPERATION = operation.Select;

            DataTable DT1 = BLL.get_Categories(_obj_Smvts_Categories);
            Session["RoleType"] = DT1;


            for (int j = 0; j < DT1.Rows.Count; j++)
            {
                obj1.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = DT1.Rows[j]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(DT1.Rows[j]["CATEG_ID"]),
                });
            }

            ViewBag.clientnames = obj1;

            ViewBag.roles = obj;



            return View();
        }

        public JsonResult bindroletype(string id)
        {
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            DataTable dt_roleTypes = (DataTable)Session["RoleType"];
            string lbl_RoleType_ID = "";
            List<string> obj2 = new List<string>();
            foreach (DataRow dr in dt_roleTypes.Rows)
            {

                if (dr[0].ToString() == id)
                {
                    lbl_RoleType_ID = dr[1].ToString();
                    break;
                }

            }
            if (lbl_RoleType_ID != "1" && lbl_RoleType_ID != "2")
            {
                List<string> item = new List<string>();
                item.Add("");
                obj.Add(new SMVTS_CATEGORIES
                {
                    Name = "Client_Admin",
                    NameId = "3",
                });
                obj.Add(new SMVTS_CATEGORIES
                {
                    Name = "Client_User",
                    NameId = "4",

                });
                //rcmb_RoleType.Items.Add(new RadComboBoxItem("Client_Admin", "3"));
                //rcmb_RoleType.Items.Add(new RadComboBoxItem("Client_User", "4"));
                //rcmb_RoleType.Enabled = true;
            }
            else if (lbl_RoleType_ID == "1")
            {
                obj.Add(new SMVTS_CATEGORIES
                {
                    Name = "Org_Admin",
                    NameId = "1",
                    //CLIENT_ADMIN = "Org_Admin",
                    //CLIENT_USER = "1",
                });
                //rcmb_RoleType.Items.Clear();
                //rcmb_RoleType.Items.Add(new RadComboBoxItem("Org_Admin", "1"));
                //rcmb_RoleType.Enabled = false;
            }

            else if (lbl_RoleType_ID == "2")
            {
                obj.Add(new SMVTS_CATEGORIES
                {
                    Name = "Partner_Admin",
                    NameId = "2",
                    //CLIENT_ADMIN = "Partner_Admin",
                    //CLIENT_USER = "2",
                });
                //rcmb_RoleType.Items.Clear();
                //rcmb_RoleType.Items.Add(new RadComboBoxItem("Partner_Admin", "2"));
                //rcmb_RoleType.Enabled = false;
            }


            return Json(new { data = obj });
        }

        public JsonResult Dashboardchange(string Value)
        {
            List<SMVTS_FORMS> obj = new List<SMVTS_FORMS>();
            DataTable DT = BLL.ExecuteQuery("EXEC USP_SMVTS_GRIDTRACK_COLUMNS @OPERATION='" + Value + "'");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj.Add(new SMVTS_FORMS
                {
                    COLUMN_ID = Convert.ToInt32(DT.Rows[i]["COLUMN_ID"]),
                    COLUMN_NAME = DT.Rows[i]["COLUMN_NAME"].ToString(),
                });
            }

            return Json(new { data = obj });

        }

        public JsonResult loadforms()
        {

            List<SMVTS_FORMS> obj2 = new List<SMVTS_FORMS>();
            SMVTS_FORMS _obj_Smvts_Forms = new SMVTS_FORMS();
            _obj_Smvts_Forms.OPERATION = operation.Select;
            DataTable DT2 = BLL.get_Forms(_obj_Smvts_Forms);
            for (int i = 0; i < DT2.Rows.Count; i++)
            {
                obj2.Add(new SMVTS_FORMS
                {
                    FORMS_NAME = DT2.Rows[i]["FORMS_NAME"].ToString(),
                    FORMS_ID = Convert.ToInt32(DT2.Rows[i]["FORMS_ID"]),
                });
            }
            ViewBag.forms = obj2;
            return Json(new { data = obj2 });
        }
        public JsonResult ROLE_Save_Click(string Clientname, string ROLEID, string ROLLETYPE, string FORMIDS, string COLUMNIDS, string DASHBOARDNAME, string btn, string org_name)
        {
            string data = "false";
            SMVTS_ROLES _obj_Smvts_Roles = new SMVTS_ROLES();
            //Already Exist condition
            int count = 0;
            DataTable dt = new DataTable();
            _obj_Smvts_Roles.OPERATION = operation.Check;
            if (ROLEID != "")
            {
                _obj_Smvts_Roles.ROLES_ID = Convert.ToInt32(ROLEID);
            }
            _obj_Smvts_Roles.ROLES_ROLENAME = Clientname;
            if (ROLLETYPE != "1" && ROLLETYPE != "2")
            {
                _obj_Smvts_Roles.ROLES_ROLETYPE = Convert.ToInt32(ROLLETYPE);
            }
            else
            {
                _obj_Smvts_Roles.ROLES_ROLETYPE = Convert.ToInt32(ROLLETYPE);
            }
            dt = BLL.get_Roles(_obj_Smvts_Roles);
            if (dt.Rows.Count > 0)
            {
                count = Convert.ToInt32(dt.Rows[0][0]);
            }

            _obj_Smvts_Roles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            //_obj_Smvts_Roles.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Roles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            //  _obj_Smvts_Roles.LASTMDFDATE = DateTime.Now;

            string strFormsId = String.Empty;
            if (FORMIDS.Length > 0)
            {
                strFormsId = FORMIDS;
            }
            if (strFormsId == "")
                _obj_Smvts_Roles.ROLES_FORMSID = "0";
            else
                _obj_Smvts_Roles.ROLES_FORMSID = strFormsId;
            string columns = "";
            columns = COLUMNIDS;

            if (COLUMNIDS.Length > 0)
            {
                _obj_Smvts_Roles.ROLES_COLUMNSID = COLUMNIDS;
            }

            _obj_Smvts_Roles.ROLES_DASHBOARD = DASHBOARDNAME;
            string orgname = org_name;
            // BLL obj = new BLL();
            string dbname = BLL.getDatabase(orgname);

            switch (btn)
            {
                case "BTN_SAVE":
                    if (count == 0)
                    {
                        _obj_Smvts_Roles.OPERATION = operation.Insert;
                        if (BLL.set_Roles(_obj_Smvts_Roles, dbname, orgname))
                        {
                            data = "true";
                        }
                        else
                        {
                            data = "false";
                        }
                        // BLL.ShowMessage(this, BLL.msg_Saved);

                        //BLL.ShowMessage(this, BLL.msg_UnSaved);
                    }
                    else
                    {
                        //  BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
                        //          rtxt_RoleName.Focus();
                        // return;
                    }
                    break;


                case "BTN_UPDATE":
                    if ((count == 0) || (count == 1))
                    {
                        _obj_Smvts_Roles.OPERATION = operation.Update;
                        if (BLL.set_Roles(_obj_Smvts_Roles, dbname, orgname))
                        {
                            data = "true";
                        }
                        else
                        {
                            data = "false";
                        }
                        // BLL.ShowMessage(this, BLL.msg_Updated);

                        // BLL.ShowMessage(this, BLL.msg_UnSaved);
                    }
                    else
                    {
                        data = "Role Already Assigned to Selected Client";
                        // BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
                        // return;
                    }
                    break;

                default:
                    break;
            }
            return Json(new { data = data });
        }
        public JsonResult Edit_role(int ROLEID)
        {
            List<SMVTS_ROLES> obj = new List<SMVTS_ROLES>();
            DataTable dt = BLL.get_Roles(new SMVTS_ROLES(Convert.ToInt32(ROLEID)));

            obj.Add(new SMVTS_ROLES
            {
                ROLES_CATEGORY_ID = Convert.ToInt32(dt.Rows[0]["ROLES_CATEGORY_ID"]),
                ROLES_ROLETYPE = Convert.ToInt32(dt.Rows[0]["ROLES_ROLETYPE"]),
                ROLES_DASHBOARD = Convert.ToString(dt.Rows[0]["ROLES_DASHBOARD"]),
                ROLES_FORMSID = Convert.ToString(dt.Rows[0]["ROLES_FORMSID"]),
                ROLES_COLUMNSID = Convert.ToString(dt.Rows[0]["ROLES_COLUMNIDS"]),

            });
            return Json(new { data = obj });
        }
        [SessionAuthorize]
        public ActionResult frm_closeticket()
        {
            return View();
        }
        public JsonResult _closedticket(int ID)
        {
            List<SMVTS_RAISETICKET> obj = new List<SMVTS_RAISETICKET>();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {

                SMVTS_RAISETICKET _obj_DemoReq = new SMVTS_RAISETICKET();
                if (ID == 0)
                {
                    _obj_DemoReq.OPERATION = operation.Update;
                    //_obj_DemoReq.DEMOREQUEST_STATUS=statusid
                    DataTable DT = BLL.get_closed(_obj_DemoReq);
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        obj.Add(new SMVTS_RAISETICKET
                        {
                            TICKET_ID = Convert.ToInt32(DT.Rows[i]["TICKET_ID"]),

                            TICKET_REQUESTORNAME = DT.Rows[i]["TICKET_REQUESTORNAME"].ToString(),
                            TICKET_DATETIME = DT.Rows[i]["TICKET_DATETIME"].ToString(),
                            TICKET_COMPANYNAME = DT.Rows[i]["TICKET_COMPANYNAME"].ToString(),
                            TICKET_TICKETDESC = DT.Rows[i]["TICKET_TICKETDESC"].ToString(),
                            TICKET_MOBILENO = DT.Rows[i]["TICKET_MOBILENO"].ToString(),
                        });
                    }

                }
                else
                {
                    _obj_DemoReq.OPERATION = operation.Check;
                    DataTable DT = BLL.get_closed(_obj_DemoReq);
                    if (DT.Rows.Count > 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            obj.Add(new SMVTS_RAISETICKET
                            {
                                TICKET_ID = Convert.ToInt32(DT.Rows[i]["TICKET_ID"]),

                                TICKET_REQUESTORNAME = DT.Rows[i]["TICKET_REQUESTORNAME"].ToString(),
                                TICKET_DATETIME = DT.Rows[i]["TICKET_DATETIME"].ToString(),
                                TICKET_COMPANYNAME = DT.Rows[i]["TICKET_COMPANYNAME"].ToString(),
                                TICKET_TICKETDESC = DT.Rows[i]["TICKET_TICKETDESC"].ToString(),
                                TICKET_MOBILENO = DT.Rows[i]["TICKET_MOBILENO"].ToString(),
                            });
                        }

                    }

                }
            }
            else
            {

            }
            return Json(new { data = obj });
        }
        public JsonResult ticket_Send_Click(string TicketID, string RequestorName, string CompanyName, string Subject, string CompletedBy, string CompleteDate, string Comments, string EmailId)
        {

            var data = "false";
            SMVTS_RAISETICKET _obj_DemoReq = new SMVTS_RAISETICKET();
            DataTable DT = new DataTable();
            int count1 = 0;
            _obj_DemoReq.DEMOREQUEST_ID = Convert.ToInt32(TicketID);
            _obj_DemoReq.OPERATION = operation.Delete;
            if (TicketID != "")
            {
                _obj_DemoReq.DEMOREQUEST_ID = Convert.ToInt32(TicketID);
            }

            DT = BLL.get_count(_obj_DemoReq);
            if (DT.Rows.Count > 0)
            {
                count1 = Convert.ToInt32(DT.Rows[0][0]);
            }

            _obj_DemoReq.DEMOREQUEST_ID = Convert.ToInt32(TicketID);
            _obj_DemoReq.DEMOREQUEST_REQUESTORNAME = RequestorName;
            _obj_DemoReq.DEMOREQUEST_COMPANYNAME = CompanyName;
            _obj_DemoReq.DEMOREQUEST_DESC = Subject;
            _obj_DemoReq.DEMOREQUEST_COMPLETEDBY = CompletedBy;
            _obj_DemoReq.DEMOREQUEST_COMPLETEDDATE = (CompleteDate);
            _obj_DemoReq.DEMOREQUEST_COMMENTS = BLL.ReplaceQuote(Comments);
            _obj_DemoReq.DEMOREQUEST_STATUS = 1;
            _obj_DemoReq.DEMOREQUEST_EMAILID = EmailId;

            string completedby = _obj_DemoReq.DEMOREQUEST_COMPLETEDBY;
            string completeddate = _obj_DemoReq.DEMOREQUEST_COMPLETEDDATE;
            string comments = _obj_DemoReq.DEMOREQUEST_COMMENTS;
            int ticketid = _obj_DemoReq.DEMOREQUEST_ID;
            string vts = "PragatiVTS";

            System.Text.StringBuilder linesSB = new System.Text.StringBuilder();
            if (count1 == 0)
            {
                _obj_DemoReq.OPERATION = operation.Update;
                linesSB.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>PragatiVTS</title></head><body><table><tr><td>Dear " + RequestorName + ",<br /></td></tr><tr><td></td></tr><tr><td>Your Ticket has been Completed.<br/></td></tr><tr><td></td></tr></table><br><table border=\"1\"><tr><td><b>Company Name</b></td><td width=\"200\"><b>Ticket ID</b></td><td width=\"200\"><b>Service Request</b></td><td width=\"200\"><b>Completed By</b></td><td width=\"200\"><b>Completed Date</b></td></tr><tr><td width=\"200\">" + CompanyName + "</td><td width=\"200\">" + ticketid + " </td><td width=\"200\">" + Subject + "</td><td width=\"200\">" + completedby + "</td><td width=\"200\">" + completeddate + "</td></tr><tr><td colspan=\"5\"><b>Task Comments</b> : " + comments + "</td></tr></table>   <table><tr><td>Regards,</td></tr><tr><td><strong style=\"color: rgb(0, 0, 51); font-family: 'Trebuchet MS'; font-size: 15px; font-style: normal; font-variant: normal; letter-spacing: normal; line-height: 22.5px; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);\">" + completedby + "<br />" + vts + "</strong>.</td></tr></table></body></html>");
                if (BLL.set_count(_obj_DemoReq))
                {
                    if (Sendmailticket(linesSB.ToString(), EmailId, ticketid))
                    {
                        data = "true";
                        //BLL.ShowMessage(this, "Thank you for Closing a Ticket!!!");
                        //lbltrackerID.Text = DropDownList1.SelectedItem.Value;
                        //GeData(Convert.ToInt32(DropDownList1.SelectedItem.Value));
                        //Rg_Driver.DataBind();
                        //Rm_Driver_page.SelectedIndex = 0;
                    }
                }
                else
                {
                    data = "false";
                    //  BLL.ShowMessage(this, "Ticket Closing failed please try after sometime");
                }
            }
            else
            {

                // BLL.ShowMessage(this, "Ticket Id Already Completed.");
            }
            return Json(new { data = data });
        }

        public bool Sendmailticket(string str, string EmailId, int trackid)
        {
            try
            {
                string strConnect = ("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGL4CY+O3mKqzXfd60oum+YaATXejNpf60UccZw/xfz9gbvinLUYnP6shgdIMQicpZqyJMAysRhs0NPugSf85OK8=");
                DataTable DT = Dal.ExecuteQueryDB1("SELECT * FROM SMVTS_SMTP_LOGIN(NOLOCK)", strConnect);
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.Host = DT.Rows[0][2].ToString();
                smtpClient.Port = Convert.ToInt32(DT.Rows[0][4].ToString());
                smtpClient.EnableSsl = DT.Rows[0][5].ToString() == "SSL" ? true : false;
                smtpClient.Credentials = new System.Net.NetworkCredential(DT.Rows[0][1].ToString(), DT.Rows[0][3].ToString());



                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("info@smartvts.com");
                mailMessage.To.Add(new System.Net.Mail.MailAddress(EmailId));
                mailMessage.CC.Add(new System.Net.Mail.MailAddress("info@smartvts.com"));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress("dathakumar.s@pragatipadh.com"));
                mailMessage.Subject = "TicketID-" + trackid + " " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
                // mailMessage.Subject = "Hi Pragatipadh - " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
                mailMessage.Priority = System.Net.Mail.MailPriority.High;

                //mailMessage.Body = str;

                AlternateView av1 = InlineAttachment(str);
                mailMessage.AlternateViews.Add(av1);
                mailMessage.IsBodyHtml = true;

                //string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();


                //System.IO.StreamWriter file = new System.IO.StreamWriter(paths + "\\mainlog.txt", true);

                try
                {


                    smtpClient.Send(mailMessage);

                    //Console.WriteLine("Mail sent." + EmailId + " At " + DateTime.Now.ToString());
                }
                catch (Exception)
                {
                    Response.Write("Error: can not send mail to " + EmailId + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                }
                Response.Write("Mail sent to " + EmailId + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                Response.Close();
            }
            catch (Exception ex)
            {
                // BLL.ShowMessage(this, ex.Message);
            }
            return true;
        }



        [SessionAuthorize]
        public ActionResult frm_Devices()
        {
            SMVTS_CATEGORIES _obj_Smvts_Devices = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID == 2)
            {

                _obj_Smvts_Devices.CATEG_CATETYPE_ID = 3;
                _obj_Smvts_Devices.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                DataTable dt = BLL.get_Categories(_obj_Smvts_Devices);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    obj.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_NAME = Convert.ToString(dt.Rows[i]["CATEG_NAME"]),
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                    });
                }
                ViewBag.categ = obj;
            }
            else
            {
                //    rcmb_DeviceCategID.Items.Clear();
                DataTable dt = BLL.get_Categories(new SMVTS_CATEGORIES());
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    obj.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_NAME = Convert.ToString(dt.Rows[i]["NAME"]),
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                    });
                }
                ViewBag.categ = obj;
            }
            SMVTS_COUNTRIES _obj_Smvts_Countries = new SMVTS_COUNTRIES();
            List<SMVTS_COUNTRIES> obj1 = new List<SMVTS_COUNTRIES>();

            DataTable dt1 = BLL.get_Country(_obj_Smvts_Countries);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj1.Add(new SMVTS_COUNTRIES
                {
                    COUNTRY_NAME = Convert.ToString(dt1.Rows[i]["COUNTRY_NAME"]),
                    COUNTRY_ID = Convert.ToInt32(dt1.Rows[i]["COUNTRY_ID"]),
                });
            }
            ViewBag.countries = obj1;

            //Load Device Names
            SMVTS_DEVICETYPES obj_types = new SMVTS_DEVICETYPES();
            DataTable dt_types = new DataTable();
            List<SMVTS_DEVICETYPES> obj_alltypes = new List<SMVTS_DEVICETYPES>();
            dt_types = BLL.GetDeviceNames();
            if (dt_types.Rows.Count > 0)
            {
                for (int k = 0; k < dt_types.Rows.Count; k++)
                {
                    obj_alltypes.Add(new SMVTS_DEVICETYPES
                    {
                        ID = Convert.ToInt32(dt_types.Rows[k]["ID"]),
                        DEVICE_TYPENAME = dt_types.Rows[k]["DEVICE_TYPENAME"].ToString()
                    });
                }
            }

            ViewBag.deviceTypes = obj_alltypes;

            return View();




        }

        public JsonResult device_categ_onchange(string CATEG_ID, string categname)

        {
            SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
            List<SMVTS_SIMS> obj = new List<SMVTS_SIMS>();

            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {
                _obj_Smvts_Sims.SIM_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
            }
            else //Client
            {
                _obj_Smvts_Sims.SIM_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            }

            // string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
            string dbname = (ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
            string CategQuery = " SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + categname.Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "") + "'";
            //abc = "SELECT * FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME=categname, and CATEG_DBNAME=dbname";
            DataTable dt_categ = new DataTable();

            dt_categ = Dal.ExecuteQueryDB1(CategQuery, dbname);

            // _obj_Smvts_Sims.SIM_CATEGORY_ID =Convert.ToInt32(dt_categ.Rows[0][0].ToString());
            DataTable dt = new DataTable();
            dt = BLL.get_Sims(_obj_Smvts_Sims);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_SIMS
                    {
                        SIM_NUMBER = dt.Rows[i]["SIM_NUMBER"].ToString(),
                        SIM_ID = Convert.ToInt32(dt.Rows[i]["SIM_ID"]),
                    });
                }
            }
            else
            {

            }
            return Json(new { data = obj });
        }
        ////public JsonResult DeviceShiftingOTP()
        ////{
        ////    string status;
        ////    string numbers = "0123456789";
        ////    Random objrandom = new Random();
        ////    string strrandom = string.Empty;
        ////    //DataTable dt = new DataTable();
        ////    searchdata obj = new searchdata();
        ////    for (int i = 0; i < 4; i++)
        ////    {
        ////        int temp = objrandom.Next(0, numbers.Length);
        ////        strrandom += temp;

        ////    }
        ////    //obj.OTP = data.OTP.ToString;
        ////    ViewBag.otp = strrandom;
        ////    Session["OTP"] = strrandom;
        ////    string phonenumber = ConfigurationManager.AppSettings["Mobile"];
        ////    string _messageText = "Your One Time Password is " + HttpUtility.UrlEncode(strrandom);
        ////    OTPCodes.Add(new OTPCode { PhoneNumber = phonenumber, Code = strrandom });

        ////    string phone_number = HttpUtility.UrlEncode(phonenumber);


        ////    string url = "http://173.45.76.227/send.aspx?username=contdemo&pass=cont1234&route=trans1&senderid=eTRANO&numbers=" + phone_number + "&message=" + _messageText + "";
        ////    try
        ////    {
        ////        // creating web request to send sms 
        ////        HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(url);
        ////        // execute the request
        ////        HttpWebResponse response = (HttpWebResponse)
        ////            _createRequest.GetResponse();

        ////        // we will read data via the response stream
        ////        Stream Answer = response.GetResponseStream();
        ////        StreamReader _Answer = new StreamReader(Answer);
        ////        string result_value = _Answer.ReadToEnd();

        ////        if (result_value.ToUpper().Trim() != "4")
        ////        {
        ////            status = "sent";
        ////        }
        ////        _Answer.Close();
        ////        response.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }


        ////    return Json(new { data = strrandom });

        ////}

        public JsonResult _frmdetails()
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();

            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();

            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {
                _obj_Smvts_Devices.OPERATION = operation.Select;

            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 5 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 6)
            {
                _obj_Smvts_Devices.OPERATION = operation.SelectSemi;
                _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            else //Client
            {
                _obj_Smvts_Devices.OPERATION = operation.Update;
                _obj_Smvts_Devices.DEVICE_STATUS = "true";
                _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            }

            DataTable DT = BLL.get_Devices(_obj_Smvts_Devices);



            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {

                    DEVICE_ID = Convert.ToInt32(DT.Rows[i]["DEVICE_ID"]),
                    DEVICE_NAME = Convert.ToString(DT.Rows[i]["DEVICE_NAME"]),
                    CLIENT_NAME = Convert.ToString(DT.Rows[i]["CATEG_NAME"]),
                    DEVICE_SERIALNUMBER = Convert.ToString(DT.Rows[i]["DEVICE_SERIALNUMBER"]),
                    SIM_NUMBER = Convert.ToString(DT.Rows[i]["SIM_NUMBER"]),
                    DEVICE_CALLNUMBER1 = Convert.ToString(DT.Rows[i]["DEVICE_CALLNUMBER1"]),
                    DEVICE_STATUS = Convert.ToString(DT.Rows[i]["DEVICE_STATUS"]),

                });
            }


            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        public JsonResult btn_device_Save_Click(string btn, string DEVICEID, string DEVICENAME, string DEVICECALLNO1, string DEVICECALLNO2, string DEVICESERIALNO, string DEVICESALEDATE,
            string DEVICEMFGDATE, string COUNTRYID, string SIMID, string DEVICEDATADURATION, string DEVICESPEEDDURATION, string DEVICESTOPDURATION, string STATUS,
            string CATEG_ID, string CATEG_NAME, string SIM_NUMBER)
        {

            string data = "false";

            DataTable DT = new DataTable();
            int count = 0;

            // validation to check Repeate Device Name
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
            _obj_Smvts_Devices.OPERATION = operation.Check;
            _obj_Smvts_Devices.DEVICE_NAME = DEVICENAME;
            _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DEVICEID);
            if (DEVICEID != "")
            {
                _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DEVICEID);
            }
            DT = BLL.get_Devices(_obj_Smvts_Devices);
            if (DT.Rows.Count > 0)
            {
                count = Convert.ToInt32(DT.Rows[0][0]);
            }
            _obj_Smvts_Devices = new SMVTS_DEVICES();
            _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DEVICEID);
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_Devices.DEVICE_CALLNUMBER1 = BLL.ReplaceQuote(DEVICECALLNO1);
            _obj_Smvts_Devices.DEVICE_CALLNUMBER2 = BLL.ReplaceQuote(DEVICECALLNO2);
            _obj_Smvts_Devices.DEVICE_NAME = DEVICENAME;
            _obj_Smvts_Devices.DEVICE_SERIALNUMBER = BLL.ReplaceQuote(DEVICESERIALNO);
            //if (DEVICESALEDATE == null)
            //    _obj_Smvts_Devices.DEVICE_DATEOFSALE = null;
            //else
            //    _obj_Smvts_Devices.DEVICE_DATEOFSALE = Convert.ToDateTime(DEVICESALEDATE);

            //if (DEVICEMFGDATE == null)
            //    _obj_Smvts_Devices.DEVICE_MFGDATE = null;
            //else
            //    _obj_Smvts_Devices.DEVICE_MFGDATE = Convert.ToDateTime(DEVICEMFGDATE);
            _obj_Smvts_Devices.DEVICE_COUNTRY_ID = Convert.ToInt32(COUNTRYID);
            _obj_Smvts_Devices.DEVICE_SIM_ID = Convert.ToInt32(SIMID);
            _obj_Smvts_Devices.DEVICE_DATADURATION = Convert.ToInt32((DEVICEDATADURATION == "" ? "0" : DEVICEDATADURATION));
            _obj_Smvts_Devices.DEVICE_OVERSPEEDDURATION = Convert.ToInt32((DEVICESPEEDDURATION == "" ? "0" : DEVICESPEEDDURATION));
            _obj_Smvts_Devices.DEVICE_STOPDURATION = Convert.ToInt32((DEVICESTOPDURATION == "" ? "0" : DEVICESTOPDURATION));
            _obj_Smvts_Devices.DEVICE_STATUS = Convert.ToString(STATUS);
            _obj_Smvts_Devices.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Devices.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices.LASTMDFDATE = DateTime.Now;
            //Session["DEVICE_CATEGORY_ID"]
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
            string Patnerid = CATEG_ID;
            string Org_Name_DropDown = CATEG_NAME;
            string dbname = "";
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                dbname = BLL.getDatabase_categ(Patnerid);
            }
            else
            {
                dbname = BLL.getDatabase_deviceupdate(Patnerid);
            }

            switch (btn)
            {
                case "BTN_SAVE":
                    if (count == 0)
                    {
                        DataTable dtLastRec = new DataTable();
                        dtLastRec = BLL.get_Devices(_obj_Smvts_Devices);
                        if (dtLastRec.Rows.Count > 0)
                        {
                            DEVICEID = Convert.ToString(dtLastRec.Rows[0][0]);
                        }
                        _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DEVICEID);
                        _obj_Smvts_Devices.OPERATION = operation.Insert;
                        if (BLL.set_Devices(_obj_Smvts_Devices, dbname, Org_Name_DropDown, SIM_NUMBER))
                        {
                            data = "true";
                            //  BLL.ShowMessage(this, BLL.msg_Saved);

                        }
                        else
                        {
                            data = "true";
                        }
                        // BLL.ShowMessage(this, BLL.msg_Saved);

                        //BLL.ShowMessage(this, BLL.msg_UnSaved);
                    }
                    else
                    {
                        data = "Device Name Already Exist.";
                        //  BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
                        //          rtxt_RoleName.Focus();
                        // return;
                    }
                    break;
                case "BTN_UPDATE":
                    if ((count == 0) || (count == 1))
                    {
                        _obj_Smvts_Devices.OPERATION = operation.Update;
                        _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DEVICEID);
                        if (BLL.set_Devices(_obj_Smvts_Devices, dbname, Org_Name_DropDown, SIM_NUMBER))
                        {
                            data = "true";
                            // BLL.ShowMessage(this, BLL.msg_Updated);
                            DataTable dtLastrec = new DataTable();
                            dtLastrec = BLL.get_Devices(_obj_Smvts_Devices);
                            if (dtLastrec.Rows.Count > 0)
                            {
                                DEVICEID = Convert.ToString(dtLastrec.Rows[0][0]);
                            }
                        }
                        else
                        {
                            data = "false";
                        }
                    }
                    else
                    {
                        data = "Device Name Already Exist.";
                        // BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
                        // return;
                    }
                    break;

                default:
                    break;
            }

            return Json(new { data = data });
        }

        public JsonResult device_Edit_Command(string ID)
        {

            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();

            DataTable dt = BLL.get_Devices(new SMVTS_DEVICES(Convert.ToInt32(ID)));



            //lbl_DevicesID.Text = Convert.ToString(dt.Rows[0]["DEVICE_ID"]);
            //rtxt_DeviceID.Text = Convert.ToString(dt.Rows[0]["DEVICE_ID"]);
            //rtxt_DevicesName.Text = Convert.ToString(dt.Rows[0]["DEVICE_NAME"]);
            //rtxt_DevicesSerialNo.Text = Convert.ToString(dt.Rows[0]["DEVICE_SERIALNUMBER"]);
            //rtxt_DeviceCallNo1.Text = Convert.ToString(dt.Rows[0]["DEVICE_CALLNUMBER1"]);
            //rtxt_DeviceCallNo2.Text = Convert.ToString(dt.Rows[0]["DEVICE_CALLNUMBER2"]);
            //rntxt_DeviceOverSpeedDuration.Text = Convert.ToString(dt.Rows[0]["DEVICE_OVERSPEEDDURATION"]);
            //rntxt_DeviceStopDuration.Text = Convert.ToString(dt.Rows[0]["DEVICE_STOPDURATION"]);
            //rntxt_DeviceDataDuration.Text = Convert.ToString(dt.Rows[0]["DEVICE_DATADURATION"]);
            //rcmb_CountryName.SelectedIndex = rcmb_CountryName.Items.FindItemIndexByValue(Convert.ToString(dt.Rows[0]["DEVICE_COUNTRY_ID"]));

            SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
            _obj_Smvts_Sims.OPERATION = operation.Update;
            _obj_Smvts_Sims.SIM_STATUS = true;
            int simid = -1;
            if (Convert.ToInt32(dt.Rows[0]["DEVICE_SIM_ID"]) == 0)
            {
                simid = -1;
            }
            else //Client
            {
                simid = Convert.ToInt32(dt.Rows[0]["DEVICE_SIM_ID"]);
            }
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {

            }
            else //Client
            {
                _obj_Smvts_Sims.SIM_CATEGORY_ID = Convert.ToInt32(Convert.ToString(dt.Rows[0]["DEVICE_CATEGORY_ID"]));
            }
            Session["DEVICE_CATEGORY_ID"] = Convert.ToInt32(Convert.ToString(dt.Rows[0]["DEVICE_CATEGORY_ID"]));
            obj.Add(new SMVTS_DEVICES
            {
                DEVICE_ID = Convert.ToInt32(dt.Rows[0]["DEVICE_ID"]),
                DEVICE_NAME = Convert.ToString(dt.Rows[0]["DEVICE_NAME"]),
                CLIENT_NAME = Convert.ToString(dt.Rows[0]["CATEG_NAME"]),
                DEVICE_SERIALNUMBER = Convert.ToString(dt.Rows[0]["DEVICE_SERIALNUMBER"]),
                SIM_NUMBER = Convert.ToString(dt.Rows[0]["SIM_NUMBER"]),
                DEVICE_CALLNUMBER1 = Convert.ToString(dt.Rows[0]["DEVICE_CALLNUMBER1"]),
                DEVICE_CALLNUMBER2 = Convert.ToString(dt.Rows[0]["DEVICE_CALLNUMBER2"]),
                DEVICE_STATUS = Convert.ToString(dt.Rows[0]["DEVICE_STATUS"]),
                DEVICE_COUNTRY_ID = Convert.ToInt32(dt.Rows[0]["DEVICE_COUNTRY_ID"]),
                DEVICE_SIM_ID = simid,
                DEVICE_DATEOFSALE_ONE = Convert.ToString(dt.Rows[0]["DEVICE_DATEOFSALE"]),
                DEVICE_MFGDATE_ONE = Convert.ToString(dt.Rows[0]["DEVICE_MFGDATE"]),
                DEVICE_CATEGORY_ID = Convert.ToInt32(Convert.ToString(dt.Rows[0]["DEVICE_CATEGORY_ID"])),
                DEVICE_STOPDURATION_ONE = Convert.ToString(dt.Rows[0]["DEVICE_STOPDURATION"]),
                DEVICE_DATADURATION_ONE = Convert.ToString(dt.Rows[0]["DEVICE_DATADURATION"]),
                DEVICE_OVERSPEEDDURATION_ONE = Convert.ToString(dt.Rows[0]["DEVICE_OVERSPEEDDURATION"]),
            });

            //get All SIMS
            DataTable dt_sims = BLL.getSIMSByCategory(Convert.ToInt32(dt.Rows[0]["DEVICE_CATEGORY_ID"]), Convert.ToInt32(dt.Rows[0]["DEVICE_SIM_ID"]));
            List<CUSTOMER_SIMS> obj_sims = new List<CUSTOMER_SIMS>();
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj_sims.Add(new CUSTOMER_SIMS
                    {
                        SIM_ID = Convert.ToInt32(dt_sims.Rows[i]["SIM_ID"]),
                        SIM_NUMBER = dt_sims.Rows[i]["SIM_SIMNUMBER"].ToString()

                    });
                }
            }

            return Json(new { data = obj, data2 = obj_sims });

        }
        [SessionAuthorize]
        public ActionResult frm_allnotworkingdevices()
        {

            SMVTS_CATEGORIES _obj_Smvts_Categories = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            _obj_Smvts_Categories.OPERATION = operation.Select;
            _obj_Smvts_Categories.CATEG_CATETYPE_ID = 3;

            DataTable dt = BLL.get_Categories(_obj_Smvts_Categories);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_CATEGORIES
                {

                    CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"])
                });
                ViewBag.categs = obj;
            }

            return View();
        }

        public JsonResult _frmnotworkingdevices(int ID)
        {
            string data = "";
            List<pvts_notworkingdevices> obj = new List<pvts_notworkingdevices>();
            pvts_notworkingdevices _obj_working = new pvts_notworkingdevices();
            DateTime date = DateTime.Now;
            string query = "select CATEG_ID,CATEG_DBNAME,CATEG_NAME from SMVTS_CATEGORIES(nolock) where CATEG_ID=" + ID + "";
            DataTable ddt_db = BLL.ExecuteQuery(query);
            string dbname = ddt_db.Rows[0]["CATEG_DBNAME"].ToString();
            string cname = ddt_db.Rows[0]["CATEG_NAME"].ToString();
            string query_user = "select CATEG_ID ,USERS_ID,CATEG_DBNAME,CATEG_NAME from SMVTS_CATEGORIES inner join SMVTS_USERS on CATEG_ID=USERS_CATEGORY_ID where CATEG_NAME='" + cname + "' and USERS_ROLE_ID=3";
            DataTable dt1 = Dal.ExecuteQueryDB1(query_user, dbname);
            int userid = Convert.ToInt32(dt1.Rows[0]["USERS_ID"].ToString());
            string query1 = "EXEC USP_SMVTS_NOTWORKING_DEVICES @USER_ID = " + userid + ",@RPT_DATE='" + date.ToString("MM/dd/yyyy") + "',@OPERATION='UP',@vehicleNo=NULL,@REMARKS=NULL,@up_date=NULL";
            DataTable dt = Dal.ExecuteQueryDB1(query1, dbname);
            //DateTime DATE1 = Convert.ToDateTime(date.ToString("MM/dd/yyyy"));
            //DataTable dt = BLL.get_notworking(_obj_working, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID), date.ToString("MM/dd/yyyy"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new pvts_notworkingdevices
                    {
                        NW_DEVICEID = Convert.ToInt32(dt.Rows[i]["NW_DEVICEID"]),
                        NW_VNO = dt.Rows[i]["NW_VNO"].ToString(),
                        NW_LASTRECDATE_ONE = Convert.ToString(dt.Rows[i]["NW_LASTRECDATE"]),
                        NW_LASTLOCATION = Convert.ToString(dt.Rows[i]["NW_LASTLOCATION"]),
                        NW_SERVICE_REMARKS = Convert.ToString(dt.Rows[i]["NW_SERVICE_REMARKS"]),
                    });
                }
            }
            else
            {
                data = "No Yellow Vehicles for Selected Client";
            }
            var jsondata = new
            {
                data = obj
            };
            var JsonResult = Json(jsondata, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;

        }

        public JsonResult Rg_Contracts_update(string CATEG_ID, string VNO, string REMARKS)
        {
            string data = "";
            try
            {
                DateTime VALUE = DateTime.Now;
                //DateTime VALUE1 = Convert.ToDateTime(VALUE.ToString("MM/dd/yyyy"));

                string query1 = "select CATEG_ID,CATEG_DBNAME,CATEG_NAME from SMVTS_CATEGORIES(nolock) where CATEG_ID=" + Convert.ToInt32(CATEG_ID) + "";
                DataTable ddt_db = BLL.ExecuteQuery(query1);
                string dbname = ddt_db.Rows[0]["CATEG_DBNAME"].ToString();
                string cname = ddt_db.Rows[0]["CATEG_NAME"].ToString();
                string query_user = "select CATEG_ID ,USERS_ID,CATEG_DBNAME,CATEG_NAME from SMVTS_CATEGORIES inner join SMVTS_USERS on CATEG_ID=USERS_CATEGORY_ID where CATEG_NAME='" + cname + "' and USERS_ROLE_ID=3";
                DataTable dt1 = Dal.ExecuteQueryDB1(query_user, dbname);
                string USERID = dt1.Rows[0]["USERS_ID"].ToString();

                // string query1 = "UPDATE SMVTS_NOTWORKING_DEVICES SET NW_SERVICE_REMARKS='" + BLL.ReplaceQuote(TextBox1.Text.Trim()) + "',NW_SERVICE_DATE='" + VALUE + "' WHERE NW_ID=" + id + "";

                DataTable dt = Dal.ExecuteQueryDB1("SELECT NW_VNO FROM SMVTS_NOTWORKING_DEVICES(NOLOCK) WHERE  NW_VNO='" + VNO + "'", dbname);
                if (dt.Rows.Count > 0)
                {
                    string query = "EXEC USP_SMVTS_NOTWORKING_DEVICES @USER_ID ='" + USERID + "',@RPT_DATE=NULL,@OPERATION='UPDATE',@vehicleNo='" + dt.Rows[0]["NW_VNO"].ToString() + "',@REMARKS='" + REMARKS + "',@up_date='" + VALUE + "'";
                    bool status = Dal.ExecuteNonQueryDB1(query, dbname);
                    if (status == true)
                    {
                        data = "true";
                        // BLL.ShowMessage(this, "Remark Updated Sucessfully");
                    }
                    else
                    {
                        data = "Please try again";
                        //  BLL.ShowMessage(this, "Please try again");
                    }
                }
            }
            catch (Exception ex)
            {
                data = "Please try again";
            }
            return Json(new { data = data });
        }


        public JsonResult alldevices()
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
            _obj_Smvts_Devices.OPERATION = operation.Select;
            DataTable dt = BLL.get_Devices(_obj_Smvts_Devices);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {
                    DEVICE_NAME = dt.Rows[i]["DEVICE_NAME"].ToString(),
                    DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"])
                });
            }
            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        public JsonResult availabledevices(int CATEG_ID)
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
            //   rcmb_VehiclesDeviceID.Items.Clear();
            _obj_Smvts_Devices.OPERATION = operation.Empty;
            _obj_Smvts_Devices.DEVICE_STATUS = "true";
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = CATEG_ID;


            _obj_Smvts_Devices.DEVICE_ID = 0;

            DataTable dt = BLL.get_Devices(_obj_Smvts_Devices);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {
                    DEVICE_NAME = dt.Rows[i]["DEVICE_NAME"].ToString(),
                    DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"])
                });
            }
            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }




        public JsonResult allsims()
        {
            List<PIPL_SIMS> obj = new List<PIPL_SIMS>();
            PIPL_SIMS _obj_Pipl_Sims = new PIPL_SIMS();
            _obj_Pipl_Sims.OPERATION = operation.Select;
            DataTable dt = BLL.get_Sims_Pipl1();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new PIPL_SIMS
                {
                    SIM_MNO = Convert.ToString(dt.Rows[i]["SIM_MNO"]),
                    SIM_SNO = Convert.ToString(dt.Rows[i]["SIM_SNO"]),
                });
            }
            var jsondata = new
            {
                data = obj
            };
            var jsonResult = Json(jsondata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult allvehicles()
        {
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            _obj_Smvts_Vehicles.OPERATION = operation.Select;
            DataTable dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLES
                {
                    VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    VEHICLES_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_ID"]),
                });
            }
            var jsondata = new
            {
                data = obj
            };
            var JsonResult = Json(jsondata, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }

        public JsonResult allvehicledetails(int categId)
        {
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            _obj_Smvts_Vehicles.OPERATION = operation.Select;
            DataTable dt = BLL.get_SchoolsVehicles(categId);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLES
                {
                    VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    VEHICLES_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_ID"]),
                });
            }
            var jsondata = new
            {
                data = obj
            };
            var JsonResult = Json(jsondata, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }



        public JsonResult clientonchange(string CATEG_ID, string CATEG_NAME)
        {
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Sims_Vehicles = new SMVTS_VEHICLES();
            _obj_Sims_Vehicles.OPERATION = operation.Empty;

            _obj_Sims_Vehicles.VEHICLES_STATUS = true;

            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {

            }
            else //Client
            {
                _obj_Sims_Vehicles.VEHICLES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            }
            _obj_Sims_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
            int categid = Convert.ToInt32(CATEG_ID);
            string categname = CATEG_NAME;
            string r = categname.Replace("(C)", "");

            DataTable dtc = BLL.getcategorydbname(categid);
            string categdbname = Convert.ToString(dtc.Rows[0]["CATEG_DBNAME"]);//dbname

            DataTable dt1 = BLL.get_categoryid_categname(categdbname, r);
            int categid1 = Convert.ToInt32(dt1.Rows[0]["CATEG_ID"]);

            DataTable dtv = new DataTable();
            dtv = BLL.get_Vehicles(categdbname, categid1);
            if (dtv.Rows.Count > 0)
            {

                for (int i = 0; i < dtv.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_VEHICLES
                    {
                        VEHICLES_REGNUMBER = dtv.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        VEHICLES_ID = Convert.ToInt32(dtv.Rows[i]["VEHICLES_ID"]),
                    });
                }
            }
            else
            {

            }
            var jsondata = new
            {
                data = obj
            };
            var JsonResult = Json(jsondata, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;

        }





        public JsonResult vehicle__IndexChanged(string CATEG_NAME, string CATEG_ID, string VNO)
        {
            string DEVICE_NAME = "";
            int DEVICE_ID = 0;


            string devicename = "";

            int vdevid = 0;
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            string vehname = VNO;
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
            _obj_Smvts_Devices.OPERATION = operation.Select;
            _obj_Smvts_Devices.DEVICE_STATUS = "true";
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {

            }
            else //Client
            {
                _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            }
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
            int categid = Convert.ToInt32(CATEG_ID);
            string categname = CATEG_NAME;
            string r = categname.Replace("(C)", "");

            DataTable dtc = BLL.getcategorydbname(categid);
            string categdbname = Convert.ToString(dtc.Rows[0]["CATEG_DBNAME"]);//dbname

            DataTable dt = BLL.get_DevID1(categdbname, vehname);
            if (dt.Rows.Count > 0)
            {
                vdevid = Convert.ToInt32(dt.Rows[0]["VEHICLES_DEVICE_ID"]);
                DataTable dt2 = BLL.get_DEVICENAME(categdbname, vdevid);
                devicename = Convert.ToString(dt2.Rows[0]["DEVICE_NAME"]);
                Session["previous_devicename"] = devicename;
                Session["previous_deviceid"] = vdevid;



                //get remaining devices for The client

                int categid2 = Convert.ToInt32(CATEG_ID);
                string categname2 = CATEG_NAME;
                string r2 = categname2.Replace("(C)", "");



                DataTable dt3 = BLL.getcategorydbname(categid2);
                string categdbname2 = Convert.ToString(dt3.Rows[0]["CATEG_DBNAME"]);


                DataTable dt5 = BLL.get_categoryid_categname(categdbname2, r2);
                int categid3 = Convert.ToInt32(dt5.Rows[0]["CATEG_ID"]);
                SMVTS_DEVICES _obj_Smvts_Devices1 = new SMVTS_DEVICES();
                _obj_Smvts_Devices1.OPERATION = operation.Empty;
                _obj_Smvts_Devices1.DEVICE_STATUS = "true";
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
                {

                }
                else //Client
                {
                    _obj_Smvts_Devices1.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                }
                _obj_Smvts_Devices1.DEVICE_CATEGORY_ID = categid3;
                _obj_Smvts_Devices1.DEVICE_ID = vdevid;
                _obj_Smvts_Devices1.DEVICE_STATUS = "true";
                DataTable dt4 = new DataTable();
                dt3 = BLL.get_DEVICES_unassigned(_obj_Smvts_Devices1, categdbname2);

                if (dt3.Rows.Count > 0)
                {
                    DEVICE_NAME = dt3.Rows[0]["DEVICE_NAME"].ToString();
                    DEVICE_ID = Convert.ToInt32(dt3.Rows[0]["DEVICE_ID"]);
                }
                else
                {

                }
            }
            else
            {

            }
            return Json(new { DEVICE_NAME = devicename, DEVICE_ID = vdevid });
        }


        public JsonResult deviceonchange(string CATEG_ID, string VNO, string CATEG_NAME)
        {
            string drivername = ""; string drvrno = "";
            List<PIPL_SIMS> obj = new List<PIPL_SIMS>();
            int categid = Convert.ToInt32(CATEG_ID);
            DataTable dtc = BLL.getcategorydbname(categid);
            string categdbname = Convert.ToString(dtc.Rows[0]["CATEG_DBNAME"]);
            DataTable dt = BLL.get_DevID1(categdbname, VNO);
            int vdevid = Convert.ToInt32(dt.Rows[0]["VEHICLES_DEVICE_ID"]);
            SMVTS_SIMS _obj_Smvts_Sims1 = new SMVTS_SIMS();
            _obj_Smvts_Sims1.OPERATION = operation.Select;
            _obj_Smvts_Sims1.SIM_ID = vdevid;
            DataTable dt1 = BLL.get_DEVIC_SIMID(categdbname, vdevid);
            int dev_simid = Convert.ToInt32(dt1.Rows[0]["DEVICE_SIM_ID"]);
            if (dev_simid == 0)
            {

                DataTable dtd1 = new DataTable();
                dtd1 = BLL.get_Sims_Pipl1();
                if (dt1.Rows.Count > 0)
                {
                    obj.Add(new PIPL_SIMS
                    {
                        SIM_MNO = dt1.Rows[0]["SIM_MNO"].ToString(),
                        SIM_SNO = dt1.Rows[0]["SIM_SNO"].ToString()
                    });
                }
                else
                {
                    // rcmb_sim_ID.Items.Clear();
                }
                int categid1 = Convert.ToInt32(CATEG_ID);
                string categname1 = Convert.ToString(CATEG_NAME);
                string r1 = categname1.Replace("(C)", "");
                DataTable dtc1 = BLL.getcategorydbname(categid1);
                string categdbname1 = Convert.ToString(dtc1.Rows[0]["CATEG_DBNAME"]);//dbname
                DataTable dt10 = BLL.get_DRIVRID1(categdbname1, VNO);
                int vdrivrid = Convert.ToInt32(dt10.Rows[0]["VEHICLES_DRIVER_ID"]);
                DataTable dtdr = BLL.get_Vehicle_Drivers1(categdbname1, vdrivrid);
                drivername = Convert.ToString(dtdr.Rows[0]["DRIVER_NAME"]);
                Session["prev_driver_name"] = drivername;
                drvrno = Convert.ToString(dtdr.Rows[0]["DRIVER_MOBILENO"]);
            }
            else
            {
                DataTable dt11 = BLL.get_SIMNO(categdbname, dev_simid);
                string simno = Convert.ToString(dt11.Rows[0]["SIM_NUMBER"]);
                int simid = dev_simid;
                Session["previous_sim_no"] = simno;
                Session["previous_sim_id"] = simid;
                string operation1 = "Check";
                DataTable dt22 = BLL.get_Sims_Check(operation1, simno);
                if (dt22.Rows.Count >= 1)
                {
                    obj.Add(new PIPL_SIMS
                    {
                        SIM_MNO = dt22.Rows[0]["SIM_MNO"].ToString(),
                        SIM_SNO = dt22.Rows[0]["SIM_SNO"].ToString()
                    });
                }
                else
                {

                }
                int categid1 = Convert.ToInt32(CATEG_ID);
                string categname1 = Convert.ToString(CATEG_NAME);
                string r1 = categname1.Replace("(C)", "");
                DataTable dtc1 = BLL.getcategorydbname(categid1);
                string categdbname1 = Convert.ToString(dtc1.Rows[0]["CATEG_DBNAME"]);//dbname
                DataTable dt10 = BLL.get_DRIVRID1(categdbname1, VNO);
                int vdrivrid = Convert.ToInt32(dt10.Rows[0]["VEHICLES_DRIVER_ID"]);
                DataTable dtdr = BLL.get_Vehicle_Drivers1(categdbname1, vdrivrid);
                drivername = Convert.ToString(dtdr.Rows[0]["DRIVER_NAME"]);
                Session["prev_driver_name"] = drivername;
                drvrno = Convert.ToString(dtdr.Rows[0]["DRIVER_MOBILENO"]);
            }


            return Json(new { data1 = obj, DriverName = drivername, DriverNo = drvrno });
        }

        public JsonResult simonchange(string CATEG_ID, string CATEG_NAME, string VNO)
        {
            SMVTS_DRIVERS _obj_Sims_Drivers = new SMVTS_DRIVERS();
            _obj_Sims_Drivers.OPERATION = operation.Select;
            _obj_Sims_Drivers.DRIVER_STATUS = true;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {

            }
            else //Client
            {
                _obj_Sims_Drivers.DRIVER_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            }
            _obj_Sims_Drivers.DRIVER_CATEGORY_ID = Convert.ToInt32(CATEG_ID);

            int categid1 = Convert.ToInt32(CATEG_ID);
            string categname1 = Convert.ToString(CATEG_NAME);
            string r1 = categname1.Replace("(C)", "");

            DataTable dtc1 = BLL.getcategorydbname(categid1);
            string categdbname1 = Convert.ToString(dtc1.Rows[0]["CATEG_DBNAME"]);//dbname
            DataTable dt1 = BLL.get_DRIVRID1(categdbname1, VNO);
            int vdrivrid = Convert.ToInt32(dt1.Rows[0]["VEHICLES_DRIVER_ID"]);
            DataTable dtdr = BLL.get_Vehicle_Drivers1(categdbname1, vdrivrid);
            string drivername = Convert.ToString(dtdr.Rows[0]["DRIVER_NAME"]);
            Session["prev_driver_name"] = drivername;
            string drvrno = Convert.ToString(dtdr.Rows[0]["DRIVER_MOBILENO"]);
            return Json(new { DriverName = drivername, DriverNo = drvrno });
        }


        public JsonResult activation_update_Click(string CATEG_ID, string CATEG_NAME, string VNO, string DEVICE_ID, string DEVICE_NAME, string SIM_MO,
            string DRIVER_NAME, string DRIVER_NUMBER, string VID)
        {

            int categid = Convert.ToInt32(CATEG_ID);
            string categname = CATEG_NAME;
            string r = categname.Replace("(C)", "");

            int deviceid = Convert.ToInt32(DEVICE_ID);
            string curr_devicename = Convert.ToString(DEVICE_NAME);

            DataTable dt = BLL.getcategorydbname(categid);
            string categdbname = Convert.ToString(dt.Rows[0]["CATEG_DBNAME"]);


            DataTable dt1 = BLL.get_categoryid_categname(categdbname, r);
            int categid1 = Convert.ToInt32(dt1.Rows[0]["CATEG_ID"]);


            string curr_simno = Convert.ToString(SIM_MO);
            //int curr_simid = Convert.ToInt32(rcmb_sim_ID.SelectedValue);
            string vehicleno = Convert.ToString(VNO);
            string drivername1 = DRIVER_NAME;
            string drivernumbr1 = DRIVER_NUMBER;

            if (Convert.ToString(Session["previous_devicename"]) != curr_devicename)
            {
                string njn = Session["previous_sim_no"].ToString();
                if (curr_simno != Convert.ToString(Session["previous_sim_no"]))
                {
                    //sim insert
                    SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
                    _obj_Smvts_Sims.SIM_CATEGORY_ID = categid1;
                    _obj_Smvts_Sims.SIM_NUMBER = Convert.ToString(curr_simno);
                    _obj_Smvts_Sims.SIM_SERIALNO = "";
                    _obj_Smvts_Sims.SIM_OPERATORNAME = "";
                    _obj_Smvts_Sims.SIM_STATE_ID = 0;
                    _obj_Smvts_Sims.SIM_STATUS = true;
                    _obj_Smvts_Sims.SIM_APNIP = "";
                    _obj_Smvts_Sims.SIM_APNWEBSITE = "";
                    _obj_Smvts_Sims.SIM_COUNTRY_ID = 0;
                    _obj_Smvts_Sims.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Sims.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Sims.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Sims.LASTMDFDATE = DateTime.Now;
                    _obj_Smvts_Sims.OPERATION = operation.Insert;
                    _obj_Smvts_Sims.SIM_STATUS = true;

                    if (BLL.set_Sims1(_obj_Smvts_Sims, categdbname))
                    {
                        ///Call Dal Layer with connection string
                        // BLL.ShowMessage(this, BLL.msg_Saved);
                        PIPL_SIMS _obj_Pipl_Sims2 = new PIPL_SIMS();
                        string operation1 = "Update";
                        string prev_simno = Convert.ToString((Session["previous_sim_no"]));
                        _obj_Pipl_Sims2.OPERATION = operation.Update;
                        _obj_Pipl_Sims2.SIM_SNO = Convert.ToString((Session["previous_sim_no"]));
                        //_obj_Pipl_Sims2.SIM_ASSIGNED_STATUS = 1;
                        //   bool c1 = BLL.update_pipl_sims1(operation1, prev_simno);
                    }

                    else
                    {
                        // BLL.ShowMessage(this, BLL.msg_UnSaved);
                    }
                    //get new simid details
                    SMVTS_SIMS _obj_Smvts_Sims1 = new SMVTS_SIMS();
                    _obj_Smvts_Sims1.SIM_CATEGORY_ID = categid1;
                    _obj_Smvts_Sims1.SIM_NUMBER = curr_simno;
                    _obj_Smvts_Sims1.OPERATION = operation.Select;
                    DataTable dt2 = BLL.get_SIMID1(categdbname, curr_simno, categid1);
                    int simid = Convert.ToInt32(dt2.Rows[0]["SIM_ID"]);

                    //update sim status
                    PIPL_SIMS _obj_Pipl_Sims1 = new PIPL_SIMS();
                    _obj_Pipl_Sims1.OPERATION = operation.Update;
                    _obj_Pipl_Sims1.SIM_SNO = curr_simno;
                    _obj_Pipl_Sims1.SIM_ASSIGNED_STATUS = 0;
                    bool c = BLL.update_pipl_sims(_obj_Pipl_Sims1, curr_simno);

                    //update device
                    SMVTS_DEVICES _obj_Smvts_Devices1 = new SMVTS_DEVICES();
                    _obj_Smvts_Devices1.OPERATION = operation.Update;
                    _obj_Smvts_Devices1.DEVICE_CATEGORY_ID = categid1;
                    _obj_Smvts_Devices1.DEVICE_SIM_ID = simid;
                    _obj_Smvts_Devices1.DEVICE_ID = deviceid;
                    _obj_Smvts_Devices1.DEVICE_NAME = curr_devicename;
                    _obj_Smvts_Devices1.DEVICE_OVERSPEEDDURATION = 0;
                    _obj_Smvts_Devices1.DEVICE_SERIALNUMBER = "";
                    _obj_Smvts_Devices1.DEVICE_STOPDURATION = 0;
                    _obj_Smvts_Devices1.DEVICE_DATADURATION = 0;
                    _obj_Smvts_Devices1.DEVICE_MFGDATE = DateTime.Now;
                    _obj_Smvts_Devices1.DEVICE_DATEOFSALE = DateTime.Now;
                    _obj_Smvts_Devices1.DEVICE_COUNTRY_ID = 0;
                    _obj_Smvts_Devices1.DEVICE_CALLNUMBER1 = "";
                    _obj_Smvts_Devices1.DEVICE_CALLNUMBER2 = "";

                    _obj_Smvts_Devices1.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Devices1.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Devices1.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Devices1.LASTMDFDATE = DateTime.Now;

                    _obj_Smvts_Devices1.DEVICE_STATUS = "true";

                    if (BLL.set_Devices(_obj_Smvts_Devices1, categdbname, categname, ""))      ///Call Dal Layer with connection string
                    {

                    }
                    else
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    SMVTS_DEVICES _obj_Smvts_Devices2 = new SMVTS_DEVICES();
                    _obj_Smvts_Devices2.DEVICE_CATEGORY_ID = categid1;
                    _obj_Smvts_Devices2.OPERATION = operation.Select;
                    DataTable dt3 = BLL.get_DEVID(categdbname, curr_devicename);
                    int devid = Convert.ToInt32(dt3.Rows[0]["DEVICE_ID"]);




                    SMVTS_DRIVERS _obj_Smvts_Drivers1 = new SMVTS_DRIVERS();
                    _obj_Smvts_Drivers1.DRIVER_CATEGORY_ID = categid1;
                    _obj_Smvts_Drivers1.DRIVER_NAME = drivername1;
                    _obj_Smvts_Drivers1.OPERATION = operation.Select;
                    string drivernam2 = Convert.ToString(Session["prev_driver_name"]);
                    DataTable dt4 = BLL.get_DRIVRID(categdbname, drivernam2);
                    int driverid1 = Convert.ToInt32(dt4.Rows[0]["DRIVER_ID"]);

                    //update driver
                    SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
                    _obj_Smvts_Drivers.DRIVER_ID = driverid1;
                    _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = categid1;
                    _obj_Smvts_Drivers.DRIVER_MOBILENO = Convert.ToString(drivernumbr1);
                    _obj_Smvts_Drivers.DRIVER_LICENSENO = "";
                    _obj_Smvts_Drivers.DRIVER_LANGUAGES = "";
                    _obj_Smvts_Drivers.DRIVER_ISSUEDATE = DateTime.Now;
                    _obj_Smvts_Drivers.DRIVER_FORMAN = "";
                    _obj_Smvts_Drivers.DRIVER_EXPIRYDATE = DateTime.Now;
                    _obj_Smvts_Drivers.DRIVER_DESC = "";
                    _obj_Smvts_Drivers.DRIVER_BLOODGROUP = "";
                    _obj_Smvts_Drivers.DRIVER_ADDRESS = "";

                    _obj_Smvts_Drivers.DRIVER_PHOTO_PATH = "";
                    _obj_Smvts_Drivers.DRIVER_NAME = drivername1;
                    _obj_Smvts_Drivers.OPERATION = operation.Update;
                    _obj_Smvts_Drivers.DRIVER_STATUS = true;
                    _obj_Smvts_Drivers.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Drivers.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Drivers.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Drivers.LASTMDFDATE = DateTime.Now;
                    if (BLL.set_Drivers1(_obj_Smvts_Drivers, categdbname, categname))
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    else
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    //update vehicle
                    SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
                    _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = categid1;
                    _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VID);
                    _obj_Smvts_Vehicles.VEHICLES_REGNUMBER = vehicleno;
                    _obj_Smvts_Vehicles.VEHICLES_CAPACITY = "0";
                    _obj_Smvts_Vehicles.VEHICLES_CURRENTODOMETER = "0";
                    _obj_Smvts_Vehicles.VEHICLES_MAXSPEED = "";
                    _obj_Smvts_Vehicles.VEHICLES_MILEAGE = "";
                    _obj_Smvts_Vehicles.VEHICLES_OFFICEMASTER_ID = 0;
                    _obj_Smvts_Vehicles.VEHICLES_OPENINGODOMETER = "0";
                    _obj_Smvts_Vehicles.VEHICLES_RESERVEVOLUME = "";
                    _obj_Smvts_Vehicles.VEHICLES_SEATINGCAPACITY = 0;
                    _obj_Smvts_Vehicles.VEHICLES_TANKCAPACITY = "0";
                    _obj_Smvts_Vehicles.VEHICLES_VEHICLEMAKEMODEL_ID = 0;
                    _obj_Smvts_Vehicles.VEHICLES_VENDORMASTER_ID = 0;
                    _obj_Smvts_Vehicles.VEHICLES_STATUS = true;
                    _obj_Smvts_Vehicles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Vehicles.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Vehicles.LASTMDFDATE = DateTime.Now;
                    _obj_Smvts_Vehicles.VEHICLES_DEVICE_ID = devid;
                    _obj_Smvts_Vehicles.VEHICLES_DRIVER_ID = driverid1;
                    _obj_Smvts_Vehicles.OPERATION = operation.Update;
                    if (BLL.set_Vehicles1(_obj_Smvts_Vehicles, categdbname, categname))
                    {
                        //  BLL.ShowMessage(this, BLL.msg_Updated);
                        //   clearControls();
                        Session.Remove("previous_sim_no");
                        Session.Remove("previous_deviceid");
                        Session.Remove("previous_devicename");
                        Session.Remove("previous_sim_id");
                        Session.Remove("prev_driver_name");
                    }
                    else
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }

                }

                else
                {

                    //update sim
                    SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
                    _obj_Smvts_Sims.SIM_ID = Convert.ToInt32(Session["previous_sim_id"]);
                    _obj_Smvts_Sims.SIM_CATEGORY_ID = categid1;
                    _obj_Smvts_Sims.SIM_NUMBER = Convert.ToString(curr_simno);
                    _obj_Smvts_Sims.SIM_SERIALNO = "";
                    _obj_Smvts_Sims.SIM_OPERATORNAME = "";
                    _obj_Smvts_Sims.SIM_STATE_ID = 0;
                    _obj_Smvts_Sims.SIM_STATUS = true;
                    _obj_Smvts_Sims.SIM_APNIP = "";
                    _obj_Smvts_Sims.SIM_APNWEBSITE = "";
                    _obj_Smvts_Sims.SIM_COUNTRY_ID = 0;
                    _obj_Smvts_Sims.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Sims.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Sims.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Sims.LASTMDFDATE = DateTime.Now;
                    _obj_Smvts_Sims.OPERATION = operation.Update;
                    _obj_Smvts_Sims.SIM_STATUS = true;

                    if (BLL.set_Sims1(_obj_Smvts_Sims, categdbname))
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    else
                    {
                        // BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    // //update device 
                    SMVTS_DEVICES _obj_Smvts_Devices1 = new SMVTS_DEVICES();
                    _obj_Smvts_Devices1.OPERATION = operation.Update;
                    _obj_Smvts_Devices1.DEVICE_CATEGORY_ID = categid1;
                    _obj_Smvts_Devices1.DEVICE_SIM_ID = Convert.ToInt32(Session["previous_sim_id"]);
                    _obj_Smvts_Devices1.DEVICE_ID = deviceid;
                    _obj_Smvts_Devices1.DEVICE_NAME = curr_devicename;
                    _obj_Smvts_Devices1.DEVICE_OVERSPEEDDURATION = 0;
                    _obj_Smvts_Devices1.DEVICE_SERIALNUMBER = "";
                    _obj_Smvts_Devices1.DEVICE_STOPDURATION = 0;
                    _obj_Smvts_Devices1.DEVICE_DATADURATION = 0;
                    _obj_Smvts_Devices1.DEVICE_MFGDATE = DateTime.Now;
                    _obj_Smvts_Devices1.DEVICE_DATEOFSALE = DateTime.Now;
                    _obj_Smvts_Devices1.DEVICE_COUNTRY_ID = 0;
                    _obj_Smvts_Devices1.DEVICE_CALLNUMBER1 = "";
                    _obj_Smvts_Devices1.DEVICE_CALLNUMBER2 = "";

                    _obj_Smvts_Devices1.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Devices1.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Devices1.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Devices1.LASTMDFDATE = DateTime.Now;

                    _obj_Smvts_Devices1.DEVICE_STATUS = "true";

                    if (BLL.set_Devices(_obj_Smvts_Devices1, categdbname, categname, ""))      ///Call Dal Layer with connection string
                    {
                        //BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    else
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }

                    SMVTS_DEVICES _obj_Smvts_Devices2 = new SMVTS_DEVICES();
                    _obj_Smvts_Devices2.DEVICE_CATEGORY_ID = categid1;
                    _obj_Smvts_Devices2.OPERATION = operation.Select;
                    DataTable dt3 = BLL.get_DEVID(categdbname, curr_devicename);
                    int devid = Convert.ToInt32(dt3.Rows[0]["DEVICE_ID"]);


                    SMVTS_DRIVERS _obj_Smvts_Drivers1 = new SMVTS_DRIVERS();
                    _obj_Smvts_Drivers1.DRIVER_CATEGORY_ID = categid1;
                    _obj_Smvts_Drivers1.DRIVER_NAME = drivername1;
                    _obj_Smvts_Drivers1.OPERATION = operation.Select;
                    string drivernam2 = Convert.ToString(Session["prev_driver_name"]);
                    DataTable dt4 = BLL.get_DRIVRID(categdbname, drivernam2);
                    int driverid1 = Convert.ToInt32(dt4.Rows[0]["DRIVER_ID"]);

                    //update driver
                    SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
                    _obj_Smvts_Drivers.DRIVER_ID = driverid1;
                    _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = categid1;
                    _obj_Smvts_Drivers.DRIVER_MOBILENO = Convert.ToString(drivernumbr1);
                    _obj_Smvts_Drivers.DRIVER_LICENSENO = "";
                    _obj_Smvts_Drivers.DRIVER_LANGUAGES = "";
                    _obj_Smvts_Drivers.DRIVER_ISSUEDATE = DateTime.Now;
                    _obj_Smvts_Drivers.DRIVER_FORMAN = "";
                    _obj_Smvts_Drivers.DRIVER_EXPIRYDATE = DateTime.Now;
                    _obj_Smvts_Drivers.DRIVER_DESC = "";
                    _obj_Smvts_Drivers.DRIVER_BLOODGROUP = "";
                    _obj_Smvts_Drivers.DRIVER_ADDRESS = "";

                    _obj_Smvts_Drivers.DRIVER_PHOTO_PATH = "";
                    _obj_Smvts_Drivers.DRIVER_NAME = drivername1;
                    _obj_Smvts_Drivers.OPERATION = operation.Update;
                    _obj_Smvts_Drivers.DRIVER_STATUS = true;
                    _obj_Smvts_Drivers.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Drivers.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Drivers.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Drivers.LASTMDFDATE = DateTime.Now;
                    if (BLL.set_Drivers1(_obj_Smvts_Drivers, categdbname, categname))
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    else
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                    //update vehicle
                    SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
                    _obj_Smvts_Vehicles.OPERATION = operation.Update;
                    _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = categid1;
                    _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VID);
                    _obj_Smvts_Vehicles.VEHICLES_REGNUMBER = vehicleno;
                    _obj_Smvts_Vehicles.VEHICLES_CAPACITY = "0";
                    _obj_Smvts_Vehicles.VEHICLES_CURRENTODOMETER = "0";
                    _obj_Smvts_Vehicles.VEHICLES_MAXSPEED = "";
                    _obj_Smvts_Vehicles.VEHICLES_MILEAGE = "";
                    _obj_Smvts_Vehicles.VEHICLES_OFFICEMASTER_ID = 0;
                    _obj_Smvts_Vehicles.VEHICLES_OPENINGODOMETER = "0";
                    _obj_Smvts_Vehicles.VEHICLES_RESERVEVOLUME = "";
                    _obj_Smvts_Vehicles.VEHICLES_SEATINGCAPACITY = 0;
                    _obj_Smvts_Vehicles.VEHICLES_TANKCAPACITY = "0";
                    _obj_Smvts_Vehicles.VEHICLES_VEHICLEMAKEMODEL_ID = 0;
                    _obj_Smvts_Vehicles.VEHICLES_VENDORMASTER_ID = 0;
                    _obj_Smvts_Vehicles.VEHICLES_STATUS = true;
                    _obj_Smvts_Vehicles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Vehicles.CREATEDDATE = DateTime.Now;
                    _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                    _obj_Smvts_Vehicles.LASTMDFDATE = DateTime.Now;
                    _obj_Smvts_Vehicles.VEHICLES_DEVICE_ID = devid;
                    _obj_Smvts_Vehicles.VEHICLES_DRIVER_ID = driverid1;
                    if (BLL.set_Vehicles1(_obj_Smvts_Vehicles, categdbname, categname))
                    {
                        // BLL.ShowMessage(this, BLL.msg_Updated);
                        Session.Remove("previous_sim_no");
                        Session.Remove("previous_deviceid");
                        Session.Remove("previous_devicename");
                        Session.Remove("previous_sim_id");
                        Session.Remove("prev_driver_name");
                        //   clearControls();
                    }
                    else
                    {
                        //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                    }
                }

            }
            else
            {
                // update sim
                SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
                _obj_Smvts_Sims.SIM_ID = Convert.ToInt32(Session["previous_sim_id"]);
                _obj_Smvts_Sims.SIM_CATEGORY_ID = categid1;
                _obj_Smvts_Sims.SIM_NUMBER = Convert.ToString(curr_simno);
                _obj_Smvts_Sims.SIM_SERIALNO = "";
                _obj_Smvts_Sims.SIM_OPERATORNAME = "";
                _obj_Smvts_Sims.SIM_STATE_ID = 0;
                _obj_Smvts_Sims.SIM_STATUS = true;
                _obj_Smvts_Sims.SIM_APNIP = "";
                _obj_Smvts_Sims.SIM_APNWEBSITE = "";
                _obj_Smvts_Sims.SIM_COUNTRY_ID = 0;
                _obj_Smvts_Sims.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Sims.CREATEDDATE = DateTime.Now;
                _obj_Smvts_Sims.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Sims.LASTMDFDATE = DateTime.Now;
                _obj_Smvts_Sims.OPERATION = operation.Update;
                _obj_Smvts_Sims.SIM_STATUS = true;

                if (BLL.set_Sims1(_obj_Smvts_Sims, categdbname))
                {
                    // BLL.ShowMessage(this, BLL.msg_NotUpdated);
                }
                else
                {
                    //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                }

                //update device
                SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
                _obj_Smvts_Devices.OPERATION = operation.Update;
                _obj_Smvts_Devices.DEVICE_CATEGORY_ID = categid1;
                _obj_Smvts_Devices.DEVICE_SIM_ID = Convert.ToInt32(Session["previous_sim_id"]);
                _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(Session["previous_deviceid"]);
                _obj_Smvts_Devices.DEVICE_NAME = curr_devicename;
                _obj_Smvts_Devices.DEVICE_OVERSPEEDDURATION = 0;
                _obj_Smvts_Devices.DEVICE_SERIALNUMBER = "";
                _obj_Smvts_Devices.DEVICE_STOPDURATION = 0;
                _obj_Smvts_Devices.DEVICE_DATADURATION = 0;
                _obj_Smvts_Devices.DEVICE_MFGDATE = DateTime.Now;
                _obj_Smvts_Devices.DEVICE_DATEOFSALE = DateTime.Now;
                _obj_Smvts_Devices.DEVICE_COUNTRY_ID = 0;
                _obj_Smvts_Devices.DEVICE_CALLNUMBER1 = "";
                _obj_Smvts_Devices.DEVICE_CALLNUMBER2 = "";

                _obj_Smvts_Devices.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Devices.CREATEDDATE = DateTime.Now;
                _obj_Smvts_Devices.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Devices.LASTMDFDATE = DateTime.Now;

                _obj_Smvts_Devices.DEVICE_STATUS = "true";

                if (BLL.set_Devices(_obj_Smvts_Devices, categdbname, categname, ""))      ///Call Dal Layer with connection string
                {
                    // BLL.ShowMessage(this, BLL.msg_NotUpdated);
                }
                else
                {
                    //BLL.ShowMessage(this, BLL.msg_NotUpdated);
                }



                SMVTS_DEVICES _obj_Smvts_Devices2 = new SMVTS_DEVICES();
                _obj_Smvts_Devices2.DEVICE_CATEGORY_ID = categid1;
                _obj_Smvts_Devices2.OPERATION = operation.Select;
                DataTable dt3 = BLL.get_DEVID(categdbname, curr_devicename);
                int devid = Convert.ToInt32(dt3.Rows[0]["DEVICE_ID"]);

                SMVTS_DRIVERS _obj_Smvts_Drivers1 = new SMVTS_DRIVERS();
                _obj_Smvts_Drivers1.DRIVER_CATEGORY_ID = categid1;
                _obj_Smvts_Drivers1.DRIVER_NAME = drivername1;
                _obj_Smvts_Drivers1.OPERATION = operation.Select;
                string drivernam = Convert.ToString(Session["prev_driver_name"]);
                DataTable dt4 = BLL.get_DRIVRID(categdbname, drivernam);
                int driverid1 = Convert.ToInt32(dt4.Rows[0]["DRIVER_ID"]);

                //update driver
                SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
                _obj_Smvts_Drivers.DRIVER_ID = driverid1;
                _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = categid1;
                _obj_Smvts_Drivers.DRIVER_MOBILENO = Convert.ToString(drivernumbr1);
                _obj_Smvts_Drivers.DRIVER_LICENSENO = "";
                _obj_Smvts_Drivers.DRIVER_LANGUAGES = "";
                _obj_Smvts_Drivers.DRIVER_ISSUEDATE = DateTime.Now;
                _obj_Smvts_Drivers.DRIVER_FORMAN = "";
                _obj_Smvts_Drivers.DRIVER_EXPIRYDATE = DateTime.Now;
                _obj_Smvts_Drivers.DRIVER_DESC = "";
                _obj_Smvts_Drivers.DRIVER_BLOODGROUP = "";
                _obj_Smvts_Drivers.DRIVER_ADDRESS = "";
                _obj_Smvts_Drivers.DRIVER_PHOTO_PATH = "";
                _obj_Smvts_Drivers.DRIVER_NAME = drivername1;
                _obj_Smvts_Drivers.OPERATION = operation.Update;
                _obj_Smvts_Drivers.DRIVER_STATUS = true;
                _obj_Smvts_Drivers.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Drivers.CREATEDDATE = DateTime.Now;
                _obj_Smvts_Drivers.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Drivers.LASTMDFDATE = DateTime.Now;
                if (BLL.set_Drivers1(_obj_Smvts_Drivers, categdbname, categname))
                {
                    // BLL.ShowMessage(this, BLL.msg_NotUpdated);

                }
                else
                {
                    //   BLL.ShowMessage(this, BLL.msg_NotUpdated);
                }
                SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
                _obj_Smvts_Vehicles.OPERATION = operation.Update;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = categid1;
                _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VID);
                _obj_Smvts_Vehicles.VEHICLES_REGNUMBER = vehicleno;
                _obj_Smvts_Vehicles.VEHICLES_CAPACITY = "0";
                _obj_Smvts_Vehicles.VEHICLES_CURRENTODOMETER = "0";
                _obj_Smvts_Vehicles.VEHICLES_MAXSPEED = "";
                _obj_Smvts_Vehicles.VEHICLES_MILEAGE = "";
                _obj_Smvts_Vehicles.VEHICLES_OFFICEMASTER_ID = 0;
                _obj_Smvts_Vehicles.VEHICLES_OPENINGODOMETER = "0";
                _obj_Smvts_Vehicles.VEHICLES_RESERVEVOLUME = "";
                _obj_Smvts_Vehicles.VEHICLES_SEATINGCAPACITY = 0;
                _obj_Smvts_Vehicles.VEHICLES_TANKCAPACITY = "0";
                _obj_Smvts_Vehicles.VEHICLES_VEHICLEMAKEMODEL_ID = 0;
                _obj_Smvts_Vehicles.VEHICLES_VENDORMASTER_ID = 0;
                _obj_Smvts_Vehicles.VEHICLES_STATUS = true;
                _obj_Smvts_Vehicles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Vehicles.CREATEDDATE = DateTime.Now;
                _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Vehicles.LASTMDFDATE = DateTime.Now;
                _obj_Smvts_Vehicles.VEHICLES_DEVICE_ID = devid;
                _obj_Smvts_Vehicles.VEHICLES_DRIVER_ID = driverid1;
                if (BLL.set_Vehicles1(_obj_Smvts_Vehicles, categdbname, categname))
                {
                    //  BLL.ShowMessage(this, BLL.msg_Updated);
                    Session.Remove("previous_sim_no");
                    Session.Remove("previous_deviceid");
                    Session.Remove("previous_devicename");
                    Session.Remove("previous_sim_id");
                    Session.Remove("prev_driver_name");
                    //clearControls();
                }
                else
                {
                    //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                }
            }
            return Json(new { data = "MSG" });
        }

        public JsonResult activation_save_Click(string CATEG_ID, string CATEG_NAME, string VNO, string DEVICE_ID, string DEVICE_NAME, string SIM_MO, string SIM_SNO,
            string DRIVER_NAME, string DRIVER_NUMBER, string VID)
        {

            int categid = Convert.ToInt32(CATEG_ID);
            string categname = Convert.ToString(CATEG_NAME);
            string r = categname.Replace("(C)", "");

            int deviceid = Convert.ToInt32(DEVICE_ID);
            string devicename = Convert.ToString(DEVICE_NAME);

            //get connection string based on categid from VTS_Config ... (

            DataTable dt = BLL.getcategorydbname(categid);
            string categdbname = Convert.ToString(dt.Rows[0]["CATEG_DBNAME"]);

            //get categid in database with above connection string
            DataTable dt1 = BLL.get_categoryid_categname(categdbname, r);
            int categid1 = Convert.ToInt32(dt1.Rows[0]["CATEG_ID"]);


            string simno = Convert.ToString(SIM_MO);
            string simserialno = Convert.ToString(SIM_SNO);
            string vehicleno = Convert.ToString(VNO);
            string drivername = Convert.ToString(DRIVER_NAME);
            string drivernumbr = Convert.ToString(DRIVER_NUMBER);



            //saving sim
            SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
            _obj_Smvts_Sims.SIM_CATEGORY_ID = categid1;
            _obj_Smvts_Sims.SIM_NUMBER = simno;
            _obj_Smvts_Sims.SIM_SERIALNO = simserialno;
            _obj_Smvts_Sims.SIM_OPERATORNAME = "";
            _obj_Smvts_Sims.SIM_STATE_ID = 0;
            _obj_Smvts_Sims.SIM_STATUS = true;
            _obj_Smvts_Sims.SIM_APNIP = "";
            _obj_Smvts_Sims.SIM_APNWEBSITE = "";
            _obj_Smvts_Sims.SIM_COUNTRY_ID = 0;
            _obj_Smvts_Sims.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Sims.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Sims.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Sims.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_Sims.OPERATION = operation.Insert;
            _obj_Smvts_Sims.SIM_STATUS = true;

            if (BLL.set_Sims1(_obj_Smvts_Sims, categdbname))
            {
                //BLL.ShowMessage(this, BLL.msg_Saved);
            }
            else
            {
                // BLL.ShowMessage(this, BLL.msg_UnSaved);
            }
            SMVTS_SIMS _obj_Smvts_Sims1 = new SMVTS_SIMS();
            _obj_Smvts_Sims1.SIM_CATEGORY_ID = categid1;
            _obj_Smvts_Sims1.SIM_NUMBER = simno;
            _obj_Smvts_Sims1.OPERATION = operation.Select;
            DataTable dt2 = BLL.get_SIMID1(categdbname, simno, categid1);
            int simid = Convert.ToInt32(dt2.Rows[0]["SIM_ID"]);

            PIPL_SIMS _obj_Pipl_Sims1 = new PIPL_SIMS();
            _obj_Pipl_Sims1.OPERATION = operation.Update;
            _obj_Pipl_Sims1.SIM_SNO = simno;
            _obj_Pipl_Sims1.SIM_ASSIGNED_STATUS = 0;
            bool c = BLL.update_pipl_sims(_obj_Pipl_Sims1, simno);

            SMVTS_DEVICES _obj_Smvts_Devices1 = new SMVTS_DEVICES();
            _obj_Smvts_Devices1.DEVICE_CATEGORY_ID = categid1;
            _obj_Smvts_Devices1.DEVICE_NAME = devicename;
            _obj_Smvts_Devices1.OPERATION = operation.Select;
            DataTable dt3 = BLL.get_DEVID(categdbname, devicename);
            int devid = Convert.ToInt32(dt3.Rows[0]["DEVICE_ID"]);


            //device update
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
            _obj_Smvts_Devices.OPERATION = operation.Update;
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = categid1;
            _obj_Smvts_Devices.DEVICE_SIM_ID = simid;
            _obj_Smvts_Devices.DEVICE_ID = deviceid;
            _obj_Smvts_Devices.DEVICE_NAME = devicename;
            _obj_Smvts_Devices.DEVICE_OVERSPEEDDURATION = 0;
            _obj_Smvts_Devices.DEVICE_SERIALNUMBER = Convert.ToString(deviceid);
            _obj_Smvts_Devices.DEVICE_STOPDURATION = 0;
            _obj_Smvts_Devices.DEVICE_DATADURATION = 0;
            _obj_Smvts_Devices.DEVICE_MFGDATE = DateTime.Now;
            _obj_Smvts_Devices.DEVICE_DATEOFSALE = DateTime.Now;
            _obj_Smvts_Devices.DEVICE_COUNTRY_ID = 0;
            _obj_Smvts_Devices.DEVICE_CALLNUMBER1 = "";
            _obj_Smvts_Devices.DEVICE_CALLNUMBER2 = "";

            _obj_Smvts_Devices.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Devices.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices.LASTMDFDATE = DateTime.Now;

            _obj_Smvts_Devices.DEVICE_STATUS = "true";

            if (BLL.set_Devices(_obj_Smvts_Devices, categdbname, categname, ""))
            {

            }
            else
            {
                //  BLL.ShowMessage(this, BLL.msg_UnSaved);
            }
            //insert driver
            SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
            _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = categid1;
            _obj_Smvts_Drivers.DRIVER_MOBILENO = Convert.ToString(drivernumbr);
            _obj_Smvts_Drivers.DRIVER_LICENSENO = "";
            _obj_Smvts_Drivers.DRIVER_LANGUAGES = "";
            _obj_Smvts_Drivers.DRIVER_ISSUEDATE = DateTime.Now;
            _obj_Smvts_Drivers.DRIVER_FORMAN = "";
            _obj_Smvts_Drivers.DRIVER_EXPIRYDATE = DateTime.Now;
            _obj_Smvts_Drivers.DRIVER_DESC = "";
            _obj_Smvts_Drivers.DRIVER_BLOODGROUP = "";
            _obj_Smvts_Drivers.DRIVER_ADDRESS = "";

            _obj_Smvts_Drivers.DRIVER_PHOTO_PATH = "";
            _obj_Smvts_Drivers.DRIVER_NAME = drivername;
            _obj_Smvts_Drivers.OPERATION = operation.Insert;
            _obj_Smvts_Drivers.DRIVER_STATUS = true;
            _obj_Smvts_Drivers.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Drivers.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Drivers.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Drivers.LASTMDFDATE = DateTime.Now;
            if (BLL.set_Drivers1(_obj_Smvts_Drivers, categdbname, categname))
            {
                //  BLL.ShowMessage(this, BLL.msg_Saved);
            }
            else
            {
                //  BLL.ShowMessage(this, BLL.msg_UnSaved);
            }

            //get driver details
            SMVTS_DRIVERS _obj_Smvts_Drivers1 = new SMVTS_DRIVERS();
            _obj_Smvts_Drivers1.DRIVER_CATEGORY_ID = categid1;
            _obj_Smvts_Drivers1.DRIVER_NAME = drivername;
            _obj_Smvts_Drivers1.OPERATION = operation.Select;
            DataTable dt4 = BLL.get_DRIVRID(categdbname, drivername);
            int driverid = Convert.ToInt32(dt4.Rows[0]["DRIVER_ID"]);

            //insert vehicle
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = categid1;
            _obj_Smvts_Vehicles.VEHICLES_REGNUMBER = vehicleno;
            //   _obj_Smvts_Vehicles.VEHICLES_AVGSPEED = 0;
            // _obj_Smvts_Vehicles.VEHICLES_AXLETYPE = "";
            _obj_Smvts_Vehicles.VEHICLES_CAPACITY = "0";
            //_obj_Smvts_Vehicles.VEHICLES_CHASSIS_NO ="";
            // _obj_Smvts_Vehicles.VEHICLES_COLOR = "";
            _obj_Smvts_Vehicles.VEHICLES_CURRENTODOMETER = "0";
            //_obj_Smvts_Vehicles.VEHICLES_EC = "";
            // _obj_Smvts_Vehicles.VEHICLES_EN = "";
            // _obj_Smvts_Vehicles.VEHICLES_ENGINE_NUMBER = "";
            // _obj_Smvts_Vehicles.VEHICLES_FC_DT = "";
            // _obj_Smvts_Vehicles.VEHICLES_FORMTYPE = "";
            // _obj_Smvts_Vehicles.VEHICLES_FTC = "";
            // _obj_Smvts_Vehicles.VEHICLES_FUEL = "";
            //_obj_Smvts_Vehicles.VEHICLES_HT_DT = "";
            // _obj_Smvts_Vehicles.VEHICLES_INS = "";
            //  _obj_Smvts_Vehicles.VEHICLES_ISP = "";
            // _obj_Smvts_Vehicles.VEHICLES_ISP_DT = "";
            //  _obj_Smvts_Vehicles.VEHICLES_MANUFACTURER = "";
            _obj_Smvts_Vehicles.VEHICLES_MAXSPEED = "60";
            _obj_Smvts_Vehicles.VEHICLES_MILEAGE = "4";
            // _obj_Smvts_Vehicles.VEHICLES_NP = "";
            // _obj_Smvts_Vehicles.VEHICLES_NP_DT = "";
            _obj_Smvts_Vehicles.VEHICLES_OFFICEMASTER_ID = 0;
            _obj_Smvts_Vehicles.VEHICLES_OPENINGODOMETER = "0";
            //   _obj_Smvts_Vehicles.VEHICLES_PC = "";
            _obj_Smvts_Vehicles.VEHICLES_RESERVEVOLUME = "";
            //    _obj_Smvts_Vehicles.VEHICLES_SC = "";
            _obj_Smvts_Vehicles.VEHICLES_SEATINGCAPACITY = 0;
            _obj_Smvts_Vehicles.VEHICLES_TANKCAPACITY = "0";
            // _obj_Smvts_Vehicles.VEHICLES_TRAVELDISTINDAY = "";
            // _obj_Smvts_Vehicles.VEHICLES_TRAVELDISTINDAY = "";

            _obj_Smvts_Vehicles.VEHICLES_VEHICLEMAKEMODEL_ID = 0;
            _obj_Smvts_Vehicles.VEHICLES_VENDORMASTER_ID = 0;


            _obj_Smvts_Vehicles.VEHICLES_STATUS = true;
            _obj_Smvts_Vehicles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Vehicles.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Vehicles.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_Vehicles.VEHICLES_DEVICE_ID = devid;
            _obj_Smvts_Vehicles.VEHICLES_DRIVER_ID = driverid;
            _obj_Smvts_Vehicles.OPERATION = operation.Insert;

            DateTime date = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            DataTable dt_ = new DataTable();
            string vno = Convert.ToString(_obj_Smvts_Vehicles.VEHICLES_REGNUMBER);
            dt_ = Dal.ExecuteQuery1("select CATEG_ID,CATEG_NAME,CATEG_EMAILID from SMVTS_CATEGORIES(nolock) where CATEG_ID=" + categid + " and CATEG_STATUS=1");

            string tomail = Convert.ToString(dt_.Rows[0]["CATEG_EMAILID"]);
            if (BLL.set_Vehicles1(_obj_Smvts_Vehicles, categdbname, categname))
            {
                string status = "Active";


                if (dt_.Rows.Count != 0)
                {
                    sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">").Append("<head>").Append("<title>PragatiVTS</title>");
                    sb.Append("</head>");
                    sb.Append("<body>");
                    sb.Append("<center><p><font face = 'Arial' size='3'><B>New Device Installation Details</B></font></p></center>");
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("Dear Team").Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("We are thankful for the opportunity provided to us  by your esteemed organization. Kindly find the below activation details.").Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("   ").Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("Date").Append("</td>");
                    sb.Append("<td>").Append(":").Append("</td>");
                    sb.Append("<td>").Append(date).Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("Client").Append("</td>");
                    sb.Append("<td>").Append(":").Append("</td>");
                    sb.Append("<td>").Append(Convert.ToString(CATEG_NAME)).Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("Device").Append("</td>");
                    sb.Append("<td>").Append(":").Append("</td>");
                    sb.Append("<td>").Append(Convert.ToString(DEVICE_NAME)).Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("Sim").Append("</td>");
                    sb.Append("<td>").Append(":").Append("</td>");
                    sb.Append("<td>").Append(Convert.ToString(SIM_MO)).Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("VehicleNo").Append("</td>");
                    sb.Append("<td>").Append(":").Append("</td>");
                    sb.Append("<td>").Append(BLL.ReplaceQuote(VNO)).Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("DriverName").Append("</td>");
                    sb.Append("<td>").Append(":").Append("</td>");
                    sb.Append("<td>").Append(DRIVER_NAME).Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("DriverNumber").Append("</td>");
                    sb.Append("<td>").Append(":").Append("</td>");
                    sb.Append("<td>").Append(DRIVER_NUMBER).Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("Regards,").Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>").Append("<strong style=\"color: rgb(0, 0, 51); font-family: 'Trebuchet MS'; font-size: 15px; font-style: normal; font-variant: normal; letter-spacing: normal; line-height: 22.5px; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);\">PragatiVTS Team.").Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</body>");
                    sb.Append("</html>");
                    Sendmailsave(string.Format(sb.ToString()), tomail, "info@smartvts.com", vno);
                    // BLL.ShowMessage(this, "Information Saved Successfully");

                }
                bool deviup = SmvtsDeviceUpdate(date,
                                                  Convert.ToInt32(DEVICE_ID),//new device id
                                                  BLL.ReplaceQuote(VNO),//new vehilce reg number
                                                  DRIVER_NAME,//new driver name
                                                  status,
                                                  ((SMVTS_USERS)Session["USERINFO"]).USERS_ID,//created by
                                                  null,//old device id
                                                  null,//old vehicle reg number
                                                  null//old driver name
                                                  );
                if (deviup == true)
                {

                    //rcmb_client_ID1.SelectedIndex = -1;
                    //rcmb_sim_ID1.SelectedIndex = -1;
                    //rcmb_device_ID1.SelectedIndex = -1;
                    //vehicle_id.Text = String.Empty;
                    //driver_name.Text = String.Empty;
                    //driver_numbr.Text = String.Empty;
                    //BLL.ShowMessage(this, "Information Saved Successfully");

                    //clearControls1();

                }



            }
            else
            {
                // BLL.ShowMessage(this, BLL.msg_UnSaved);
            }
            return Json(new { MSG = "SUCCESS" });

        }
        public bool SmvtsDeviceUpdate(DateTime Date,
                                    int New_Device,
                                    string New_VehilceRegNo,
                                    string New_DriverName,
                                    string Status, int CreatedByUserId,
                                    int? Old_Device,
                                    string Old_VehilceRegNo = null,
                                    string Old_DriverName = null)
        {
            bool st = false;
            if (Old_Device == null && Old_VehilceRegNo == null && Old_DriverName == null) //NEW RECORD : INSERT
            {
                st = BLL.InsertUpDateDevice("INSERT", Date, New_Device, New_VehilceRegNo, New_DriverName, Status, CreatedByUserId, Old_Device, Old_VehilceRegNo, Old_DriverName);
            }
            else //OLD RECORD : UPDATE
            {
                st = BLL.InsertUpDateDevice("INSERT", Date, New_Device, New_VehilceRegNo, New_DriverName, Status, CreatedByUserId, Old_Device, Old_VehilceRegNo, Old_DriverName);
            }
            return st;
        }

        public bool Sendmailsave(string str, string toomail, string cc, string vehno)
        {
            try
            {

                string strConnect = ("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGL4CY+O3mKqzXfd60oum+YaATXejNpf60UccZw/xfz9gbvinLUYnP6shgdIMQicpZqyJMAysRhs0NPugSf85OK8=");
                DataTable DT = Dal.ExecuteQueryDB1("SELECT * FROM SMVTS_SMTP_LOGIN(NOLOCK)", strConnect);
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.Host = DT.Rows[0][2].ToString();
                smtpClient.Port = Convert.ToInt32(DT.Rows[0][4].ToString());
                smtpClient.EnableSsl = DT.Rows[0][5].ToString() == "SSL" ? true : false;
                smtpClient.Credentials = new System.Net.NetworkCredential(DT.Rows[0][1].ToString(), DT.Rows[0][3].ToString());



                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("info@pragatipadh.com");
                mailMessage.To.Add(new System.Net.Mail.MailAddress(toomail));
                mailMessage.CC.Add(new System.Net.Mail.MailAddress(cc));
                //  mailMessage.Bcc.Add(new System.Net.Mail.MailAddress("jaya.b@pragatipadh.com"));
                mailMessage.Subject = "New Device Installation Details for '" + vehno + "'," + DateTime.Now.ToLongTimeString();
                // mailMessage.Subject = "Hi Pragatipadh - " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
                mailMessage.Priority = System.Net.Mail.MailPriority.High;

                //mailMessage.Body = str;

                AlternateView av1 = InlineAttachmentsave(str);
                mailMessage.AlternateViews.Add(av1);
                mailMessage.IsBodyHtml = true;

                //string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();

                string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();
                System.IO.StreamWriter file = new System.IO.StreamWriter(paths + "\\dashboardLOG.txt", true);

                try
                {


                    smtpClient.Send(mailMessage);

                    //Console.WriteLine("Mail sent." + EmailId + " At " + DateTime.Now.ToString());
                }
                catch (Exception)
                {
                    file.WriteLine("Error: can not send mail to " + toomail + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                    // Response.Write("Error: can not send mail to " + mailMessage.From + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                }
                //Response.Write("Mail sent to " + mailMessage.From + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                //Response.Close();
                file.WriteLine("Mail sent to " + toomail + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                file.Close();
            }
            catch (Exception ex)
            {
                // BLL.ShowMessage(this, ex.Message);
            }
            return true;
        }
        AlternateView InlineAttachmentsave(string str)
        {
            AlternateView av1 = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            return av1;
        }









        [SessionAuthorize]
        public ActionResult Tracksmart()
        {
            checkcategoryid();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();

            if (Session["USERINFO"] == null)
            {

            }
            DataTable dt = BLL.get_GridTrackDistance(_obj_Smvts_GridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));

            for (int i = 0;
                i < dt.Rows.Count; i++)
            {

                int spe = 0;
                if (dt.Rows[i]["SPEED"] == DBNull.Value)
                {
                    spe = 0;
                }
                else
                {
                    spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                }
                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = spe,
                    IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                    DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                    PLACE = dt.Rows[i]["PLACE"].ToString(),
                    DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                    DURATION = dt.Rows[i]["duration"].ToString(),
                    DKM = dt.Rows[i]["distance_day"].ToString(),
                    TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                    DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                });
            }
            ViewBag.DASHBOARDDATA = obj;
            return View();
        }


        public ActionResult TracksmartSchool()
        {
            checkcategoryid();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();

            if (Session["USERINFO"] == null)
            {

            }
            DataTable dt = BLL.get_GridTrackDistance(_obj_Smvts_GridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));

            for (int i = 0;
                i < dt.Rows.Count; i++)
            {

                int spe = 0;
                if (dt.Rows[i]["SPEED"] == DBNull.Value)
                {
                    spe = 0;
                }
                else
                {
                    spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                }
                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = spe,
                    IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                    DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                    PLACE = dt.Rows[i]["PLACE"].ToString(),
                    DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                    DURATION = dt.Rows[i]["duration"].ToString(),
                    DKM = dt.Rows[i]["distance_day"].ToString(),
                    TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                    DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                });
            }
            ViewBag.DASHBOARDDATA = obj;
            return View();
        }




        public ActionResult Test00()
        {

            return View();
        }

        [SessionAuthorize]
        public ActionResult Test001()
        {
            checkcategoryid();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();

            if (Session["USERINFO"] == null)
            {

            }
            DataTable dt = BLL.get_GridTrackDistance(_obj_Smvts_GridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));

            for (int i = 0;
                i < dt.Rows.Count; i++)
            {

                int spe = 0;
                if (dt.Rows[i]["SPEED"] == DBNull.Value)
                {
                    spe = 0;
                }
                else
                {
                    spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                }
                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = spe,
                    IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                    DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                    PLACE = dt.Rows[i]["PLACE"].ToString(),
                    DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                    DURATION = dt.Rows[i]["duration"].ToString(),
                    DKM = dt.Rows[i]["distance_day"].ToString(),
                    TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                    DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                });
            }
            ViewBag.DASHBOARDDATA = obj;
            return View();
        }



        [SessionAuthorize]
        public ActionResult frm_device_activation()
        {
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            SMVTS_CATEGORIES _obj_Smvts_Categories = new SMVTS_CATEGORIES();
            _obj_Smvts_Categories.OPERATION = operation.Select;

            DataTable dt = BLL.get_Clients();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_CATEGORIES
                {

                    CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"])
                });
                ViewBag.categs = obj;
            }
            return View();
        }




        public JsonResult FREE_DEVICES(string CATEGID, string CATEG_NAME)
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            string dbname = BLL.getDatabase_categ(CATEGID);
            Session["CATEG_DBNAME"] = dbname;

            DataTable dtc = BLL.getcategoryid_db(CATEG_NAME, dbname);
            int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);


            if (dbname != "" && dbname != "null")
            {
                DataTable dt = BLL.FREE_DEVICES(categid, 1, dbname);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_DEVICES
                    {
                        DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"]),
                        DEVICE_NAME = Convert.ToString(dt.Rows[i]["DEVICE_NAME"]),
                    });
                }
            }

            return Json(new { data = obj });
        }



        public JsonResult ALL_DEVICES(string CATEGID, string CATEG_NAME)
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            string dbname = BLL.getDatabase_categ(CATEGID);
            Session["CATEG_DBNAME"] = dbname;

            DataTable dtc = BLL.getcategoryid_db(CATEG_NAME, dbname);
            int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);


            if (dbname != "" && dbname != "null")
            {
                DataTable dt = BLL.ALL_DEVICES(categid, dbname);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_DEVICES
                    {
                        DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"]),
                        DEVICE_NAME = Convert.ToString(dt.Rows[i]["DEVICE_NAME"]),
                    });
                }
            }

            return Json(new { data = obj });
        }




        public JsonResult FREE_SIMS()
        {
            List<PIPL_SIMS> obj = new List<PIPL_SIMS>();

            DataTable dt = BLL.FREE_SIMS();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new PIPL_SIMS
                {
                    SIM_SNO = Convert.ToString(dt.Rows[i]["SIM_SNO"]),
                    SIM_MNO = Convert.ToString(dt.Rows[i]["SIM_MNO"]),
                });
            }
            return Json(new { data = obj });
        }




        public JsonResult ALL_SIMS()
        {
            List<PIPL_SIMS> obj = new List<PIPL_SIMS>();

            DataTable dt = BLL.ALL_SIMS();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new PIPL_SIMS
                {
                    SIM_SNO = Convert.ToString(dt.Rows[i]["SIM_SNO"]),
                    SIM_MNO = Convert.ToString(dt.Rows[i]["SIM_MNO"]),
                });
            }
            return Json(new { data = obj });
        }


        //GETVEHICLEDETAILS_VNO

        public JsonResult GETVEHICLEDETAILS_VNO(string CATEGID, string CATEGNAME, string VNO)
        {
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();


            string dbname = BLL.getDatabase_categ(CATEGID);

            DataTable dtc = BLL.getcategoryid_db(CATEGNAME, dbname);

            int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);

            DataTable dt = BLL.GETVEHICLEDETAILS_VNO(categid, VNO, dbname);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Session["VNO"] = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString();
                Session["DEVICEID"] = dt.Rows[i]["DEVICE_ID"].ToString();
                Session["DRIVERNAME"] = dt.Rows[i]["DRIVER_NAME"].ToString();
                obj.Add(new SMVTS_GRIDTRACK
                {
                    VEHICLE_NO = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    DEVICE_ID = dt.Rows[i]["DEVICE_ID"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    DRIVERNUMBER = dt.Rows[i]["DRIVER_MOBILENO"].ToString(),
                    SIM_MNO = Convert.ToString(dt.Rows[i]["SIM_NUMBER"]),
                    DRIVER_ID = Convert.ToInt32(dt.Rows[i]["DRIVER_ID"]),
                    DEVICE_SIM_ID = Convert.ToString(dt.Rows[i]["SIM_ID"]),
                    MODEL = Convert.ToString(dt.Rows[i]["VEHICLES_VEHICLEMAKEMODEL_ID"]),
                    VEHICLES_RESERVEVOLUME = Convert.ToString(dt.Rows[i]["VEHICLES_RESERVEVOLUME"]),
                });
            }
            return Json(new { data = obj });
        }

        public JsonResult SAVE_DATAENTRY(string CATEGID, string CATEGNAME, string DEVICEID, string VNO, string SIMNO, string SIMTYPE, string DRIVERNAME, string DRIVERNUMBER)
        {

            string dbname = Session["CATEG_DBNAME"].ToString();

            DataTable dtc = BLL.getcategoryid_db(CATEGNAME, dbname);
            int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);


            if (SIMTYPE == "CUSTOMER")
            {
                SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
                _obj_Smvts_Sims.SIM_CATEGORY_ID = categid;
                _obj_Smvts_Sims.SIM_NUMBER = SIMNO;
                _obj_Smvts_Sims.SIM_SERIALNO = SIMNO;
                _obj_Smvts_Sims.SIM_OPERATORNAME = "";
                _obj_Smvts_Sims.SIM_STATE_ID = 0;
                _obj_Smvts_Sims.SIM_STATUS = true;
                _obj_Smvts_Sims.SIM_APNIP = "";
                _obj_Smvts_Sims.SIM_APNWEBSITE = "";
                _obj_Smvts_Sims.SIM_COUNTRY_ID = 0;
                _obj_Smvts_Sims.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Sims.CREATEDDATE = DateTime.Now;
                _obj_Smvts_Sims.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Sims.LASTMDFDATE = DateTime.Now;
                _obj_Smvts_Sims.OPERATION = operation.Insert;
                _obj_Smvts_Sims.SIM_STATUS = true;

                if (BLL.set_Sims1(_obj_Smvts_Sims, dbname))
                {
                    //BLL.ShowMessage(this, BLL.msg_Saved);
                }
                else
                {
                    // BLL.ShowMessage(this, BLL.msg_UnSaved);
                }
            }

            string MSG = "";
            List<SMVTS_VEHICLE_MODELS> obj = new List<SMVTS_VEHICLE_MODELS>();
            int USERID = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;

            DataTable dt = BLL.VEHICLE_MODEL(Convert.ToInt32(CATEGID), dbname);
            string MODELNAME = "";
            if (dt.Rows.Count > 0)
            {
                MODELNAME = dt.Rows[0]["VEHLEMM_NAME"].ToString();
            }
            else
            {
                MODELNAME = "TATA";
            }

            DataTable dt1 = BLL.CHECK_VEHICLE_EXISTORNOT(Convert.ToInt32(CATEGID), VNO, dbname);

            if (dt1.Rows.Count > 0)
            {
                MSG = "VEHICLE ALREADY EXIST..";
            }
            else
            {
                try
                {

                    //DataTable dtc = BLL.getcategoryid_db(CATEGNAME,dbname);

                    //int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);


                    bool b = BLL.INSERT_DATAENTRY(VNO, MODELNAME, DEVICEID, SIMNO, DRIVERNAME, DRIVERNUMBER, categid, dbname);

                    if (b == true)
                    {
                        bool c = BLL.UPDATE_SIM_STATUS(SIMNO);
                        MSG = "true";

                        DataTable dt001_ = Dal.ExecuteQuery1("select CATEG_ID,CATEG_NAME,CATEG_EMAILID from SMVTS_CATEGORIES(nolock) where CATEG_ID=" + CATEGID + " and CATEG_STATUS=1");
                        string tomail = Convert.ToString(dt001_.Rows[0]["CATEG_EMAILID"]);
                        StringBuilder sb = new StringBuilder();


                        sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">").Append("<head>").Append("<title>PragatiVTS</title>");
                        sb.Append("</head>");
                        sb.Append("<body>");
                        sb.Append("<center><p><font face = 'Arial' size='3'><B>New Device Installation Details</B></font></p></center>");
                        sb.Append("<table>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("Dear Team").Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("We are thankful for the opportunity provided to us  by your esteemed organization. Kindly find the below activation details.").Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("<table>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("   ").Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("<table>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("Date").Append("</td>");
                        sb.Append("<td>").Append(":").Append("</td>");
                        sb.Append("<td>").Append(DateTime.Now).Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("Client").Append("</td>");
                        sb.Append("<td>").Append(":").Append("</td>");
                        sb.Append("<td>").Append(Convert.ToString(CATEGNAME)).Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("Device").Append("</td>");
                        sb.Append("<td>").Append(":").Append("</td>");
                        sb.Append("<td>").Append(Convert.ToString(DEVICEID)).Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("Sim").Append("</td>");
                        sb.Append("<td>").Append(":").Append("</td>");
                        sb.Append("<td>").Append(Convert.ToString(SIMNO)).Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("VehicleNo").Append("</td>");
                        sb.Append("<td>").Append(":").Append("</td>");
                        sb.Append("<td>").Append(BLL.ReplaceQuote(VNO)).Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("DriverName").Append("</td>");
                        sb.Append("<td>").Append(":").Append("</td>");
                        sb.Append("<td>").Append(DRIVERNAME).Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("DriverNumber").Append("</td>");
                        sb.Append("<td>").Append(":").Append("</td>");
                        sb.Append("<td>").Append(DRIVERNUMBER).Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("<table>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("Regards,").Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td>").Append("<strong style=\"color: rgb(0, 0, 51); font-family: 'Trebuchet MS'; font-size: 15px; font-style: normal; font-variant: normal; letter-spacing: normal; line-height: 22.5px; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);\">PragatiVTS Team.").Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</body>");
                        sb.Append("</html>");
                        Sendmailsave(string.Format(sb.ToString()), tomail, "info@smartvts.com", VNO);
                        // BLL.ShowMessage(this, "Information Saved Successfully");
                    }
                    else
                    {
                        MSG = "insertion failed";
                    }
                    BLL.INSERT_LOG(VNO, DEVICEID, SIMNO, (Convert.ToInt32(CATEGID)), MSG, USERID);
                }
                catch (Exception ex)
                {
                    string smssage = ex.Message;
                    bool c = BLL.INSERT_LOG(VNO, DEVICEID, SIMNO, (Convert.ToInt32(CATEGID)), smssage, USERID);

                }
                //	EXEC SMVTS_ENTERDATA  @VEHICLENO = 'TS07UF9980', @MODEL = 'AL-API',  @DEVICEID = '70101968' ,  @SIMNO ='70101968' ,  @SIMPROVIDER = 'Idea' ,  @DRIVERNAME = 'DRIVER_70101968' ,  @DRIVERCONTACT ='',  @ODOMETER ='0',  @CLIENTID ='3'	

            }



            return Json(new { data = MSG });
        }






        public JsonResult UPDATE_DATAENTRY(string CATEGID, string CATEGNAME, string VNO, string VID, string SIMID, string SIMNO, string DEVICEID, string DEVICENAME,
            string DRIVERID, string DRIVERNUMBER, string DRIVERNAME, string MODEL, string RVOULME)
        {
            int USERID = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            string MSG = "";

            string dbname = BLL.getDatabase_categ(CATEGID);

            DataTable dt2 = BLL.get_SIMID1(dbname, SIMNO, Convert.ToInt32(CATEGID));
            int simid = 0;
            if (dt2.Rows.Count > 0)
            {
                simid = Convert.ToInt32(dt2.Rows[0]["SIM_ID"]);
            }
            else
            {
                SMVTS_SIMS _obj_Smvts_Sims = new SMVTS_SIMS();
                _obj_Smvts_Sims.SIM_CATEGORY_ID = Convert.ToInt32(CATEGID);
                _obj_Smvts_Sims.SIM_NUMBER = Convert.ToString(SIMNO);
                _obj_Smvts_Sims.SIM_SERIALNO = SIMNO;
                _obj_Smvts_Sims.SIM_OPERATORNAME = "AIRTEL";
                _obj_Smvts_Sims.SIM_STATE_ID = 0;
                _obj_Smvts_Sims.SIM_STATUS = true;
                _obj_Smvts_Sims.SIM_APNIP = "";
                _obj_Smvts_Sims.SIM_APNWEBSITE = "";
                _obj_Smvts_Sims.SIM_COUNTRY_ID = 1;
                _obj_Smvts_Sims.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Sims.CREATEDDATE = DateTime.Now;
                _obj_Smvts_Sims.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Sims.LASTMDFDATE = DateTime.Now;
                _obj_Smvts_Sims.OPERATION = operation.Insert;
                _obj_Smvts_Sims.SIM_STATUS = true;
                if (BLL.set_Sims1(_obj_Smvts_Sims, dbname))
                {
                    bool c = BLL.UPDATE_SIM_STATUS(SIMNO);
                    MSG = "true";

                }
                DataTable dt3 = BLL.get_SIMID1(dbname, SIMNO, Convert.ToInt32(CATEGID));

                if (dt3.Rows.Count > 0)
                {
                    simid = Convert.ToInt32(dt2.Rows[0]["SIM_ID"]);
                }
                //if (dt2.Rows.Count > 0)
                //{
                //    simid = Convert.ToInt32(dt2.Rows[0]["SIM_ID"]);
                //}
            }





            SMVTS_DEVICES _obj_Smvts_Devices1 = new SMVTS_DEVICES();
            _obj_Smvts_Devices1.OPERATION = operation.Update;
            _obj_Smvts_Devices1.DEVICE_CATEGORY_ID = Convert.ToInt32(CATEGID);
            _obj_Smvts_Devices1.DEVICE_SIM_ID = simid;
            _obj_Smvts_Devices1.DEVICE_ID = Convert.ToInt32(DEVICEID);
            _obj_Smvts_Devices1.DEVICE_NAME = DEVICENAME;
            _obj_Smvts_Devices1.DEVICE_OVERSPEEDDURATION = 0;
            _obj_Smvts_Devices1.DEVICE_SERIALNUMBER = SIMNO;
            _obj_Smvts_Devices1.DEVICE_STOPDURATION = 0;
            _obj_Smvts_Devices1.DEVICE_DATADURATION = 0;
            _obj_Smvts_Devices1.DEVICE_MFGDATE = DateTime.Now;
            _obj_Smvts_Devices1.DEVICE_DATEOFSALE = DateTime.Now;
            _obj_Smvts_Devices1.DEVICE_COUNTRY_ID = 1;
            _obj_Smvts_Devices1.DEVICE_CALLNUMBER1 = "";
            _obj_Smvts_Devices1.DEVICE_CALLNUMBER2 = "";
            _obj_Smvts_Devices1.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices1.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Devices1.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices1.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_Devices1.DEVICE_STATUS = "true";





            DataTable dtc = BLL.getcategoryid_db(CATEGNAME, dbname);
            int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);

            try
            {
                if (BLL.set_Devices(_obj_Smvts_Devices1, dbname, CATEGNAME, ""))      ///Call Dal Layer with connection string
                {
                    MSG = "true";
                    bool c = BLL.UPDATE_SIM_STATUS(SIMNO);



                    if (c == true)
                    {
                        MSG = "true";
                        SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
                        _obj_Smvts_Drivers.DRIVER_ID = Convert.ToInt32(DRIVERID);
                        _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = categid;
                        _obj_Smvts_Drivers.DRIVER_MOBILENO = Convert.ToString(DRIVERNUMBER);
                        _obj_Smvts_Drivers.DRIVER_LICENSENO = "";
                        _obj_Smvts_Drivers.DRIVER_LANGUAGES = "";
                        _obj_Smvts_Drivers.DRIVER_ISSUEDATE = DateTime.Now;
                        _obj_Smvts_Drivers.DRIVER_FORMAN = "";
                        _obj_Smvts_Drivers.DRIVER_EXPIRYDATE = DateTime.Now;
                        _obj_Smvts_Drivers.DRIVER_DESC = "";
                        _obj_Smvts_Drivers.DRIVER_BLOODGROUP = "";
                        _obj_Smvts_Drivers.DRIVER_ADDRESS = "";

                        _obj_Smvts_Drivers.DRIVER_PHOTO_PATH = "";
                        _obj_Smvts_Drivers.DRIVER_NAME = DRIVERNAME;
                        _obj_Smvts_Drivers.OPERATION = operation.Update;
                        _obj_Smvts_Drivers.DRIVER_STATUS = true;
                        _obj_Smvts_Drivers.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                        _obj_Smvts_Drivers.CREATEDDATE = DateTime.Now;
                        _obj_Smvts_Drivers.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                        _obj_Smvts_Drivers.LASTMDFDATE = DateTime.Now;
                        if (BLL.set_Drivers1(_obj_Smvts_Drivers, dbname, CATEGNAME))
                        {
                            MSG = "true";
                            //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
                            _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = categid;
                            _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VID);
                            _obj_Smvts_Vehicles.VEHICLES_REGNUMBER = VNO;
                            _obj_Smvts_Vehicles.VEHICLES_CAPACITY = "0";
                            _obj_Smvts_Vehicles.VEHICLES_CURRENTODOMETER = "0";
                            _obj_Smvts_Vehicles.VEHICLES_MAXSPEED = "60";
                            _obj_Smvts_Vehicles.VEHICLES_MILEAGE = "4";
                            _obj_Smvts_Vehicles.VEHICLES_OFFICEMASTER_ID = 0;
                            _obj_Smvts_Vehicles.VEHICLES_OPENINGODOMETER = "0";
                            _obj_Smvts_Vehicles.VEHICLES_RESERVEVOLUME = "0";
                            _obj_Smvts_Vehicles.VEHICLES_SEATINGCAPACITY = 0;
                            _obj_Smvts_Vehicles.VEHICLES_TANKCAPACITY = "600";
                            _obj_Smvts_Vehicles.VEHICLES_VEHICLEMAKEMODEL_ID = Convert.ToInt32(MODEL);
                            _obj_Smvts_Vehicles.VEHICLES_VENDORMASTER_ID = 0;
                            _obj_Smvts_Vehicles.VEHICLES_STATUS = true;
                            _obj_Smvts_Vehicles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                            _obj_Smvts_Vehicles.CREATEDDATE = DateTime.Now;
                            _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                            _obj_Smvts_Vehicles.LASTMDFDATE = DateTime.Now;
                            _obj_Smvts_Vehicles.VEHICLES_DEVICE_ID = Convert.ToInt32(DEVICEID);
                            _obj_Smvts_Vehicles.VEHICLES_DRIVER_ID = Convert.ToInt32(DRIVERID);
                            _obj_Smvts_Vehicles.OPERATION = operation.Update;
                            if (BLL.set_Vehicles1(_obj_Smvts_Vehicles, dbname, CATEGNAME))
                            {
                                MSG = "true";
                                //try
                                //{
                                //    StringBuilder sb = new StringBuilder();
                                //    sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">").Append("<head>").Append("<title>PragatiVTS</title>");
                                //    sb.Append("</head>");
                                //    sb.Append("<body>");
                                //    sb.Append("<center><p><font face = 'Arial' size='3'><B>Device Update</B></font></p></center>");
                                //    sb.Append("<table>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("Dear Team").Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("We have attended to the device in vehicle '" + BLL.ReplaceQuote(VNO) + "'. Kindly find the Below Updated details:").Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("</table>");
                                //    sb.Append("<table>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("    ").Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("</table>");
                                //    sb.Append("<table>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("Date").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("<td>").Append(DateTime.Now).Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("Old DeviceID").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("<td>").Append(Session["DEVICEID"]).Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("New DeviceID").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("<td>").Append(Convert.ToInt32(DEVICEID)).Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("Old VehicleNo").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("<td>").Append(Session["VNO"].ToString()).Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("New VehicleNo").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("<td>").Append(BLL.ReplaceQuote(VNO)).Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("Old DriverName").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("<td>").Append(Session["DRIVERNAME"].ToString()).Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("New DriverName").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("<td>").Append(DRIVERNAME).Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("Status").Append("</td>");
                                //    sb.Append("<td>").Append(":").Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("</table>");
                                //    sb.Append("<table>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("Regards,").Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("").Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("<tr>");
                                //    sb.Append("<td>").Append("<strong style=\"color: rgb(0, 0, 51); font-family: 'Trebuchet MS'; font-size: 15px; font-style: normal; font-variant: normal; letter-spacing: normal; line-height: 22.5px; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);\">PragatiVTS Team.").Append("</td>");
                                //    sb.Append("</tr>");
                                //    sb.Append("</table>");
                                //    sb.Append("</body>");
                                //    sb.Append("</html>");
                                //    DataTable dt001_ = new DataTable();
                                //    try
                                //    {
                                //        dt001_ = Dal.ExecuteQuery1("select CATEG_ID,CATEG_NAME,CATEG_EMAILID from SMVTS_CATEGORIES(nolock) where CATEG_ID=" + CATEGID + " and CATEG_STATUS=1");
                                //    }
                                //    catch
                                //    {

                                //    }

                                //    string tomail = Convert.ToString(dt001_.Rows[0]["CATEG_EMAILID"]);


                                //    if (Sendmail1(string.Format(sb.ToString()), Convert.ToInt32(DEVICEID), BLL.ReplaceQuote(VNO), DRIVERNAME, tomail))
                                //    {

                                //    }
                                //}
                                //catch
                                //{

                                //    }

                            }
                            else
                            {
                                MSG = "false";
                                //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                            }
                        }
                        else
                        {
                            MSG = "false";
                            //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                        }
                    }
                }
                else
                {
                    MSG = "false";
                    //  BLL.ShowMessage(this, BLL.msg_NotUpdated);
                }
            }

            catch (Exception ex)
            {
                BLL.INSERT_LOG(VNO, DEVICEID, SIMNO, (Convert.ToInt32(CATEGID)), ex.Message, USERID);
            }




            return Json(new { data = MSG });
        }

        public bool Sendmail1(string str, int Mail_deviceid, string mail_vno, string mail_drivername, string categ_emailid)
        {
            try
            {

                string strConnect = ("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGL4CY+O3mKqzXfd60oum+YaATXejNpf60UccZw/xfz9gbvinLUYnP6shgdIMQicpZqyJMAysRhs0NPugSf85OK8=");
                DataTable DT = Dal.ExecuteQueryDB1("SELECT * FROM SMVTS_SMTP_LOGIN(NOLOCK)", strConnect);
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.Host = DT.Rows[0][2].ToString();
                smtpClient.Port = Convert.ToInt32(DT.Rows[0][4].ToString());
                smtpClient.EnableSsl = DT.Rows[0][5].ToString() == "SSL" ? true : false;
                smtpClient.Credentials = new System.Net.NetworkCredential(DT.Rows[0][1].ToString(), DT.Rows[0][3].ToString());



                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("Support@pragativts.com");
                mailMessage.To.Add(new System.Net.Mail.MailAddress(categ_emailid));
                mailMessage.CC.Add(new System.Net.Mail.MailAddress("info@smartvts.com"));
                //  mailMessage.Bcc.Add(new System.Net.Mail.MailAddress("vikranth.d@pragatipadh.com"));
                mailMessage.Subject = "Device Update Alert for '" + mail_vno + "'," + DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString();
                // mailMessage.Subject = "Hi Pragatipadh - " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
                mailMessage.Priority = System.Net.Mail.MailPriority.High;

                //mailMessage.Body = str;

                AlternateView av1 = InlineAttachment(str);
                mailMessage.AlternateViews.Add(av1);
                mailMessage.IsBodyHtml = true;

                //string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();


                //System.IO.StreamWriter file = new System.IO.StreamWriter(paths + "\\mainlog.txt", true);

                try
                {


                    smtpClient.Send(mailMessage);

                    //Console.WriteLine("Mail sent." + EmailId + " At " + DateTime.Now.ToString());
                }
                catch (Exception)
                {
                    Response.Write("Error: can not send mail to " + mailMessage.From + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                }
                Response.Write("Mail sent to " + mailMessage.From + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                Response.Close();
            }
            catch (Exception ex)
            {
                // BLL.ShowMessage(this, ex.Message);
            }
            return true;
        }
        public JsonResult allvehicles_categ(string CATEG_ID, string CATEGNAME)
        {
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            string dbname = Session["CATEG_DBNAME"].ToString();


            DataTable dtc = BLL.getcategoryid_db(CATEGNAME, dbname);

            int categid = Convert.ToInt32(dtc.Rows[0]["CATEG_ID"]);

            DataTable dt = BLL.ALL_VEHICLE_CATEG(categid, dbname);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLES
                {
                    VEHICLES_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_ID"]),
                    VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString()
                });
            }
            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //ALL_VEHICLE_CATEG


        }

        public ActionResult Test002()
        {
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult NewLohin()
        {
            return View();
        }
        public ActionResult NewLogin()
        {

            return View();
        }

        public ActionResult ScheduleRoute()
        {

            return View();
        }
        public ActionResult Login()

        {

            string url = Request.Url.Host;

            Session["DomainName"] = url;
            //string url = "www.tranopro.com";
            if (url.ToLower() == "tranopro.com")
            {
                url = "www." + url;
            }
            Session["DomainName"] = url;
            //get WLP Detials
            DataTable dt = new DataTable();
            if (url == "www.tranopro.com")
            {
                ViewBag.logo = "/IMAGES/Logos/Tranopro.png";
                ViewBag.telephone = "9100777440";
                ViewBag.Phonenumber = "9100777440";
                ViewBag.email = "info@containe.in";
            }
            else
            {
                dt = BLL.getWLPDetails(url);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.logo = dt.Rows[0]["LOGOS_URL"].ToString();
                    ViewBag.telephone = dt.Rows[0]["CATEG_TELEPHONE"].ToString();
                    ViewBag.Phonenumber = dt.Rows[0]["CATEG_MOBILENUMBER"].ToString();
                    ViewBag.email = dt.Rows[0]["CATEG_EMAILID"].ToString();
                }
            }



            HttpCookie cookieforbind = Request.Cookies.Get("namepassword");
            if (cookieforbind != null)
            {
                ViewBag.u_P = cookieforbind["user_P"].ToString();
                ViewBag.u_N = cookieforbind["user_N"].ToString();
            }
            // var username = Session["UserName"];
            // var password = Session["Password"];
            HttpCookie LoginCookie = Request.Cookies.Get("Credentials");
            if (LoginCookie != null)
            {
                string x = LoginCookie["Username"].ToString();
                string y = LoginCookie["Password"].ToString();
                string z = LoginCookie["CompanyName"].ToString();
                ViewBag.user_N = x;
                ViewBag.user_P = y;
                //BLL obj = new BLL();
                var data = BLL.getDatabase(z);
                Session["DBNAME"] = data;
                get_User(z, x, "on", y);
                return RedirectToAction("frm_GridTrack_Distance", "Home");
            }
            ViewBag.url = url;
            return View();
        }
        public ActionResult Logout()
        {
            HttpCookie LoginCookie = Request.Cookies.Get("Credentials");
            if (LoginCookie != null)
            {

                LoginCookie.Expires = DateTime.Now.AddDays(-10);
                LoginCookie.Value = null;
                HttpContext.Response.Cookies.Set(LoginCookie);

            }
            Session.Abandon();
            return RedirectToAction("Login", "Home", "Home");
        }
        public ActionResult Dashboard()
        {
            int USER_ID = Convert.ToInt32(Session["USERID"]);
            //if (PAGE_PERMISSIONS(USER_ID, "Dashboard"))
            //{
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Error");
            //}

        }

        public ActionResult Home()
        {
            return View();
        }
        [SessionAuthorize]
        public ActionResult Trip_Status()
        {

            return View();
        }
        [SessionAuthorize]
        public ActionResult frm_TrackingNearestVehicles()
        {
            return View();
        }
        public JsonResult _TrackingNearestVehicles(string lat, string lng, string radius)
        {
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();

            DataTable dt = BLL.get_nearestvehicles(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID), lat, lng, Convert.ToInt32(radius));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_GRIDTRACK
                {
                    VEHICLE_NO = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    SPEED = Convert.ToInt32(dt.Rows[i]["TRIPDATA_SPEED"]),
                    TRIPDATA_TRIPDATE = dt.Rows[i]["TRIPDATA_TRIPDATE"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    PLACE = dt.Rows[i]["ADDRESS"].ToString(),
                    LATITUDE = dt.Rows[i]["TRIPDATA_LATITUDE"].ToString(),
                    LONGITUDE = dt.Rows[i]["TRIPDATA_LONGITUDE"].ToString(),
                    COLOR = dt.Rows[i]["VEHICLE_COLOR"].ToString(),
                });
            }

            return Json(new { TripData = obj });

        }
        //dt_Points1 = BLL.get_nearestvehicles(HttpContext.Current.Session["dt_Points1UserId"].ToString(), lat, lng, radius);
        [SessionAuthorize]
        public ActionResult frm_ViewReports(string RPT)
        {
            frmVehicles();
            ViewBag.vehicles = TempData["vehicles"];
            if (RPT == "Dashboard")
            {
                ViewBag.ptype = "Dashboard";
            }
            else if (RPT == "Trip")
            {
                ViewBag.ptype = "Trip";
            }
            else if (RPT == "Violation")
            {
                ViewBag.ptype = "Violation";
            }
            else if (RPT == "Landmark")
            {
                ViewBag.ptype = "Landmark";
            }
            else if (RPT == "Vdeatils")
            {
                ViewBag.ptype = "VehicleDetails";

            }
            else if (RPT == "Geofence")
            {
                ViewBag.ptype = "Geofence";
                DataTable dt = BLL.getGeofenceLandmarks(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
                List<SMVTS_LANDMARKS> obj = new List<SMVTS_LANDMARKS>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_LANDMARKS
                    {
                        LANDMARKS_ADDRESS = dt.Rows[i]["LANDMARKS_ADDRESS"].ToString(),
                        LANDMARKS_ID = Convert.ToInt32(dt.Rows[i]["LANDMARKS_ID"]),

                    });

                }
                ViewBag.landmarks = obj;

            }
            else if (RPT == "Stoppage")
            {
                ViewBag.ptype = "Stoppage";
            }
            else if (RPT == "Timezone")
            {
                ViewBag.ptype = "Specifictimezone";
            }
            else if (RPT == "Consignment")
            {

            }
            else if (RPT == "Tripdetailed")
            {

            }
            else if (RPT == "24hoursrunanalysis")
            {
                ViewBag.ptype = "24hoursrunanalysis";
            }
            else if (RPT == "24hoursanalysis")
            {

            }
            else if (RPT == "Archivestoppage")
            {

            }

            return View();
        }

        [SessionAuthorize]
        public JsonResult _geofence(string checkgeofenceonly, string frmdate, string todate, string landmarkid, int interval)
        {
            List<SMVTS_LANDMARKS> obj = new List<SMVTS_LANDMARKS>();

            DataTable dt = new DataTable();
            if (checkgeofenceonly == "true")
            {
                dt = BLL.ExecuteQuery("EXEC USP_CURRENTLYWITHIN_GEOFENCE @USERID='" + ((SMVTS_USERS)Session["USERINFO"]).USERS_ID + "',@LANDMARKSID='" + landmarkid + "'");
            }
            else
            {
                dt = BLL.get_geofenceData(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                            (frmdate == null ? string.Empty : frmdate),
                            (todate == null ? string.Empty : todate),
                            (landmarkid == "-1" ? string.Empty : landmarkid), interval);


            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (checkgeofenceonly == "true")
                {
                    obj.Add(new SMVTS_LANDMARKS
                    {
                        VEHICLE_NUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                        IN_DATE = dt.Rows[i]["IN_DATE"].ToString(),
                        OUT_DATE = dt.Rows[i]["OUT_DATE"].ToString(),
                        DURATION = dt.Rows[i]["DURATION"].ToString(),
                    });
                }
                else
                {
                    obj.Add(new SMVTS_LANDMARKS
                    {
                        VEHICLE_NUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                        IN_DATE = dt.Rows[i]["IN_DATE"].ToString(),
                        OUT_DATE = dt.Rows[i]["OUT_DATE"].ToString(),
                        DURATION = dt.Rows[i]["DURATION"].ToString(),
                        LOCATIONTYPE = dt.Rows[i]["LOCATIONTYPE"].ToString(),
                    });
                }

            }
            return Json(new { data = obj });
        }

        [SessionAuthorize]
        public JsonResult _GetgeofenceTripcount(string checkgeofenceonly, string frmdate, string todate, string landmarkid, int interval)
        {
            List<SMVTS_LANDMARKS> obj = new List<SMVTS_LANDMARKS>();

            DataTable dt = new DataTable();

            dt = BLL.get_geofenceData_tripcount(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                        (frmdate == null ? string.Empty : frmdate),
                        (todate == null ? string.Empty : todate),
                        (landmarkid == "-1" ? string.Empty : landmarkid), interval);



            for (int i = 0; i < dt.Rows.Count; i++)
            {


                obj.Add(new SMVTS_LANDMARKS
                {
                    VEHICLE_NUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                    IN_DATE = dt.Rows[i]["DATE"].ToString(),
                    TRIPCOUNT = dt.Rows[i]["Trips"].ToString(),
                    LANDMARKS_ADDRESS = dt.Rows[i]["ADDRESS"].ToString(),
                });


            }
            return Json(new { data = obj });
        }


        [SessionAuthorize]
        public JsonResult _view_Violation(string VNO, string FRMDATE, string TODATE, string strOperation, string Interval = null, string chkgeofence = null, string chklastrecord = null)
        {
            DataTable dt = new DataTable();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _objGridTrack = new SMVTS_GRIDTRACK();


            dt = BLL.get_ReportsData(strOperation, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                  (VNO == "0" ? string.Empty : VNO),
                  (FRMDATE == null ? string.Empty : FRMDATE),
                  (TODATE == null ? string.Empty : TODATE), "", false, "", "");



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strOperation == "1")
                {
                    obj.Add(new SMVTS_GRIDTRACK
                    {
                        VEHICLENUMBER = dt.Rows[i]["REGNUMBER"].ToString(),
                        TRIPDATA_TRIPDATE = dt.Rows[i]["RECORDEDDATE"].ToString(),
                        PLACE = dt.Rows[i]["LANDMARKS_ADDRESS"].ToString(),
                        AVGSPEED = dt.Rows[i]["AVGSPEED"].ToString(),
                        MAXSPEED = dt.Rows[i]["MAXSPEED"].ToString(),
                        OVERSPEED = dt.Rows[i]["OVERSPEED_TRIP_MAXSPEED"].ToString(),
                        //   DURATION = dt.Rows[i]["duration"].ToString(),
                        LATITUDE = dt.Rows[i]["TRIPDATA_LATITUDE"].ToString(),
                        LONGITUDE = dt.Rows[i]["TRIPDATA_LONGITUDE"].ToString(),
                    });
                }
                else if (strOperation == "4")
                {
                    obj.Add(new SMVTS_GRIDTRACK
                    {
                        VEHICLENUMBER = dt.Rows[i]["REGNUMBER"].ToString(),
                        TRIPDATA_TRIPDATE = dt.Rows[i]["TRIPDATE"].ToString(),
                        TIME = dt.Rows[i]["TRIPTIME"].ToString(),
                        TRIPDATAT_SPEED = dt.Rows[i]["SPEED"].ToString(),
                        PLACE = dt.Rows[i]["LOCATIONNAME"].ToString(),
                    });
                }

                else if (strOperation == "11")
                {
                    obj.Add(new SMVTS_GRIDTRACK
                    {
                        VEHICLENUMBER = dt.Rows[i]["REGNUMBER"].ToString(),
                        VEHICLEMFG = dt.Rows[i]["VEHICLEMFG"].ToString(),
                        MODEL = dt.Rows[i]["MODEL"].ToString(),
                        TYPE = dt.Rows[i]["TYPE"].ToString(),
                        TANKCAPACITY = dt.Rows[i]["TANKCAPACITY"].ToString(),
                        MAXSPEED = dt.Rows[i]["MAXSPEED"].ToString(),
                    });
                }
                else if (strOperation == "12")
                {

                    obj.Add(new SMVTS_GRIDTRACK
                    {

                        VEHICLENUMBER = dt.Rows[i]["vehiclenumber"].ToString(),
                        st_date = dt.Rows[i]["STARTDATE"].ToString(),
                        ed_date = dt.Rows[i]["ENDDATE"].ToString(),
                        TIME = dt.Rows[i]["timediff"].ToString(),
                        PLACE = dt.Rows[i]["LANDMARKSADDRESS"].ToString(),
                    });
                }
                else if (strOperation == "15")
                {

                    obj.Add(new SMVTS_GRIDTRACK
                    {

                        VEHICLENUMBER = dt.Rows[i]["VEH_NUM"].ToString(),
                        st_date = dt.Rows[i]["STARTDT"].ToString(),
                        ed_date = dt.Rows[i]["ENDDT"].ToString(),
                        TIME = dt.Rows[i]["TIMEDIFF"].ToString(),
                        VEHSTATUS = dt.Rows[i]["STATUS"].ToString(),
                        PLACE = dt.Rows[i]["LNDMKADD"].ToString(),
                    });
                }

                else if (strOperation == "25")
                {

                    obj.Add(new SMVTS_GRIDTRACK
                    {

                        VEHICLENUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        STARTLOCATION = dt.Rows[i]["STARTLOCATION"].ToString(),
                        ENDLOCATION = dt.Rows[i]["ENDLOCATION"].ToString(),
                        TOTALRUNNINGTIME = dt.Rows[i]["TOTALRUNNINGTIME"].ToString(),
                        TOTALSTOPTIME = dt.Rows[i]["TOTALSTOPTIME"].ToString(),
                        NIGHTSTOPTIME = dt.Rows[i]["NIGHTSTOPTIME"].ToString(),
                        DAYRUN = dt.Rows[i]["DAYRUN"].ToString(),
                        DAYSTOPS = dt.Rows[i]["DAYSTOPS"].ToString(),
                        NIGHTRUN = dt.Rows[i]["NIGHTRUN"].ToString(),
                        ACTUALKMS = dt.Rows[i]["ACTUALKMS"].ToString(),
                        IDLEKMS = dt.Rows[i]["IDLEKMS"].ToString(),
                        ACERAGE_RUNNNING_SPEED = dt.Rows[i]["AVERAGE_RUNNING_SPEED"].ToString(),
                        ACERAGE_SPEED = dt.Rows[i]["AVERAGE_SPEED"].ToString(),
                        OS = dt.Rows[i]["OS"].ToString(),
                        RA = dt.Rows[i]["RA"].ToString(),
                        RD = dt.Rows[i]["RD"].ToString(),
                        ND = dt.Rows[i]["ND"].ToString(),
                        CD = dt.Rows[i]["CD"].ToString(),
                        driverhome_stop = dt.Rows[i]["driverShome_stop"].ToString(),
                        servicetime_stop = dt.Rows[i]["servicetime_stop"].ToString(),
                        loadingtime_stop = dt.Rows[i]["loadingtime_stop"].ToString(),
                    });
                }
            }
            return Json(new { data = obj });
        }

        [SessionAuthorize]
        public JsonResult _frm_ViewReports()
        {
            DataTable dt = new DataTable();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _objGridTrack = new SMVTS_GRIDTRACK();
            dt = BLL.get_GridTrack(_objGridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = Convert.ToInt32(dt.Rows[i]["SPEED"]),
                    IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                    TRIPDATA_TRIPDATE = dt.Rows[i]["DATE"].ToString(),
                    PLACE = dt.Rows[i]["PLACE"].ToString(),
                    DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                    DURATION = dt.Rows[i]["duration"].ToString(),
                    //     DKM = dt.Rows[i]["distance_day"].ToString(),
                    //     TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                    DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                    ODOMETER = Convert.ToInt32(dt.Rows[i]["ODOMETER"]),
                    COLORSTATUS = dt.Rows[i]["COLOR_STATUS"].ToString(),
                    TRIPDATAT_SPEED = dt.Rows[i]["DAYS:HRS:MINS"].ToString(),
                    VehicleType = dt.Rows[i]["VEHLEMM_NAME"].ToString()
                });
            }
            return Json(new { data = obj });
        }
        [SessionAuthorize]
        public JsonResult _view_tripreport(string VNO, string startdate, string Enddate)
        {
            DataTable dt = new DataTable();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _objGridTrack = new SMVTS_GRIDTRACK();

            string operation = "Tripwise_Report";
            //  dt=BLL.get_tripdata(operation, (VNO == "0" ? "-1" : VNO), (startdate == null ? string.Empty : startdate), (Enddate == null ? string.Empty : Enddate));

            dt = BLL.get_ReportsData("16", "",
                          (VNO == "0" ? "-1" : VNO),
                          (startdate == null ? string.Empty : (startdate).ToString()),
                          (Enddate == null ? string.Empty : (Enddate).ToString()), "", false,
                         "25", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
            //  dt = BLL.get_ReportsData_bustrip((VNO == "0" ? "-1" : VNO), (date == null ? string.Empty : date));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_GRIDTRACK
                {
                    SPEED = Convert.ToInt32(dt.Rows[i]["SPEED"]),
                    IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                    TRIPDATA_TRIPDATE = dt.Rows[i]["DATE"].ToString(),
                    PLACE = dt.Rows[i]["PLACE"].ToString(),
                    DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                    DURATION = dt.Rows[i]["duration"].ToString(),
                    //     DKM = dt.Rows[i]["distance_day"].ToString(),
                    //     TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                    LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                    LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                    COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                    DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                    ODOMETER = Convert.ToInt32(dt.Rows[i]["ODOMETER"]),
                    COLORSTATUS = dt.Rows[i]["COLOR_STATUS"].ToString(),
                    TRIPDATAT_SPEED = dt.Rows[i]["DAYS:HRS:MINS"].ToString(),

                });
            }
            return Json(new { data = obj });


        }


        [SessionAuthorize]
        public void frmVehicles()
        {

            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            DataTable dt = BLL.get_ReportDevices("DEVICES", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {

                    DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"]),
                    DEVICE_NAME = dt.Rows[i]["DEVICE_NAME"].ToString(),

                });
            }
            //   ViewBag.vehicles = obj;
            TempData["vehicles"] = obj;
        }
        [SessionAuthorize]
        public ActionResult frm_ViewTripData()
        {
            frmVehicles();
            ViewBag.vehicles = TempData["vehicles"];
            //   Session["return"] = "frm_ViewTripData";
            //HtmlHelper helper = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "Index"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            //helper.RenderAction("frmVehicles");//call your action
            return View();
        }

        public JsonResult _frm_view_tripdata(string VNO, string FROMDATE, string TODATE, string INTERVAL)
        {
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();




            DataTable dt = BLL.get_TripData_notusers(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                      (VNO == "0" ? string.Empty : VNO),
                      (FROMDATE == null ? string.Empty : Convert.ToDateTime(FROMDATE).ToString("MM/dd/yyyy hh:mm tt")),
                      (TODATE == null ? string.Empty : Convert.ToDateTime(TODATE).ToString("MM/dd/yyyy hh:mm tt")), INTERVAL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_GRIDTRACK
                {
                    VEHICLE_NO = dt.Rows[i]["REGNUMBER"].ToString(),
                    TRIPDATA_TRIPDATE = dt.Rows[i]["TRIPDATE"].ToString(),
                    TIME = dt.Rows[i]["TRIPDATE_TIME"].ToString(),
                    ODOMETER = Convert.ToInt32(dt.Rows[i]["C_ODOMETER"]),
                    TRIPDATAT_SPEED = dt.Rows[i]["SPEED"].ToString(),
                    PLACE = dt.Rows[i]["ADDRESS"].ToString(),
                    DISTANCE = dt.Rows[i]["DISTANCE"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    DEGREES = dt.Rows[i]["Temperature"].ToString(),
                    TRIPID = Convert.ToInt32(dt.Rows[i]["TRIPID"]),
                });
            }

            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }



        public ActionResult frm_Archive_ViewTripData()
        {
            frmVehicles();
            ViewBag.vehicles = TempData["vehicles"];
            return View();
        }


        public JsonResult _frm_archive_report(string VNO = null, string FRMDATE = null, string TODATE = null, string Interval = null)
        {
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            DataTable dt = BLL.get_TripData_notusers_archivetrack(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                  (VNO == "0" ? string.Empty : VNO),
                  (FRMDATE == null ? string.Empty : FRMDATE),
                  (TODATE == null ? string.Empty : TODATE), Interval);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_GRIDTRACK
                {
                    VEHICLE_NO = dt.Rows[i]["REGNUMBER"].ToString(),
                    TRIPDATA_TRIPDATE = dt.Rows[i]["TRIPDATE"].ToString(),
                    TIME = dt.Rows[i]["TRIPDATE_TIME"].ToString(),
                    ODOMETER = Convert.ToInt32(dt.Rows[i]["C_ODOMETER"]),
                    TRIPDATAT_SPEED = dt.Rows[i]["SPEED"].ToString(),
                    PLACE = dt.Rows[i]["ADDRESS"].ToString(),
                    DISTANCE = dt.Rows[i]["DISTANCE"].ToString(),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    DEGREES = dt.Rows[i]["Temperature"].ToString(),
                });
            }
            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [SessionAuthorize]
        public ActionResult frm_Vehicles()
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();

            _obj_Smvts_Devices.OPERATION = operation.Empty;
            _obj_Smvts_Devices.DEVICE_STATUS = "true";
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt = BLL.get_Devices(_obj_Smvts_Devices);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {
                    DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"]),
                    DEVICE_NAME = dt.Rows[i]["DEVICE_NAME"].ToString(),
                });
            }
            ViewBag.devices = obj;

            List<SMVTS_DRIVERS> obj1 = new List<SMVTS_DRIVERS>();
            SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
            _obj_Smvts_Drivers.OPERATION = operation.Update;
            _obj_Smvts_Drivers.DRIVER_STATUS = true;
            _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            //   _obj_Smvts_Drivers.DRIVER_ID = Convert.ToInt32(Convert.ToString(dt.Rows[0]["VEHICLES_DRIVER_ID"]));
            DataTable dt1 = BLL.get_Drivers(_obj_Smvts_Drivers);

            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                obj1.Add(new SMVTS_DRIVERS
                {
                    DRIVER_ID = Convert.ToInt32(dt1.Rows[j]["DRIVER_ID"]),
                    DRIVER_NAME = dt1.Rows[j]["DRIVER_NAME"].ToString(),
                });
            }
            ViewBag.drivers = obj1;



            List<SMVTS_VEHLEMM> obj2 = new List<SMVTS_VEHLEMM>();

            SMVTS_VEHLEMM _obj_Smvts_Vehlemm = new SMVTS_VEHLEMM();
            _obj_Smvts_Vehlemm.OPERATION = operation.Select;
            _obj_Smvts_Vehlemm.VEHLEMM_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt2 = BLL.get_Vehlemm(_obj_Smvts_Vehlemm);
            for (int k = 0; k < dt2.Rows.Count; k++)
            {
                obj2.Add(new SMVTS_VEHLEMM
                {
                    VEHLEMM_MODEL = dt2.Rows[k]["VEHLEMM_NAME"].ToString(),
                    VEHLEMM_ID = Convert.ToInt32(dt2.Rows[k]["VEHLEMM_ID"]),
                });
            }

            ViewBag.vehiclemodel = obj2;




            return View();
        }

        public JsonResult _frm_Vehicles()
        {
            List<SMVTS_GRIDTRACK> OBJ = new List<SMVTS_GRIDTRACK>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                _obj_Smvts_Vehicles.OPERATION = operation.Select;

            }
            else
            {
                _obj_Smvts_Vehicles.OPERATION = operation.Insert;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }

            DataTable dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OBJ.Add(new SMVTS_GRIDTRACK
                {
                    Vehicles_id = Convert.ToInt32(dt.Rows[i]["Vehicles_id"]),
                    VEHICLE_NO = dt.Rows[i]["vehicles_regnumber"].ToString(),
                    MODEL = dt.Rows[i]["vehicles_vehiclemakemodel_id"].ToString(),
                    MAXSPEED = dt.Rows[i]["vehicles_maxspeed"].ToString(),
                    Mileage = dt.Rows[i]["vehicles_mileage"].ToString(),
                    odometer = dt.Rows[i]["vehicles_openingodometer"].ToString(),
                    DEVICE_ID = dt.Rows[i]["vehicles_device_id"].ToString(),
                    VEHSTATUS = dt.Rows[i]["vehicles_status"].ToString(),
                });
            }
            var jsonData = new
            {
                data = OBJ
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult frm_vehicles_Save(string CurrentOdometer, string OpeningOdometer, string TankCapacity, string Reservevolume,
            string DeviceID, string VehicleID, string DriverID, string VNO, string btn, string VehicleModelID,
            string VehicleMaxspeed, string Mileage, string ChkStatus, string LoadCapacity, string DriverName)
        {
            ///////////

            string MSG = "false";
            SMVTS_VEHICLES _obj_Smvts_Vehicles;
            int catid;
            catid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            // if (BLL.Check_Session(Page))
            // {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            DataTable DT = new DataTable();
            if (Convert.ToDouble(CurrentOdometer) < Convert.ToDouble(OpeningOdometer))
            {
                //BLL.ShowMessage(this, "Current Odometer Reading Should be Greater than or eqaul to Opening Odometer");
                //rtxt_VehiclesCurrentODOMeter.Focus();
                //return;
            }

            if (Convert.ToDouble(TankCapacity) < Convert.ToDouble(Reservevolume))
            {
                //BLL.ShowMessage(this, "Tank Capacity Should be Greater than or equal to Reserve Volume");
                //rtxt_VehiclesCurrentODOMeter.Focus();
                //return;
            }


            if (DeviceID != "0")
            {
                _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Vehicles.VEHICLES_DEVICE_ID = Convert.ToInt32(DeviceID);
                _obj_Smvts_Vehicles.OPERATION = operation.Check;
                if (VehicleID != "")
                {
                    _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VehicleID);
                }

                DT = BLL.get_Vehicles(_obj_Smvts_Vehicles);
                if (DT.Rows.Count > 0)
                {
                    count1 = Convert.ToInt32(DT.Rows[0][0]);
                }
            }
            if (Convert.ToString(DriverID) != "0")
            {
                /// Checking Repeated Sim0 Number exists
                _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Vehicles.VEHICLES_DRIVER_ID = Convert.ToInt32(DriverID);
                _obj_Smvts_Vehicles.OPERATION = operation.Delete;
                if (VehicleID != "")
                {
                    _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VehicleID);
                }

                DT = BLL.get_Vehicles(_obj_Smvts_Vehicles);
                if (DT.Rows.Count > 0)
                {
                    count2 = Convert.ToInt32(DT.Rows[0][0]);
                }
            }

            // Checking Register number ************************
            _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_Vehicles.VEHICLES_REGNUMBER = BLL.ReplaceQuote(VNO);
            _obj_Smvts_Vehicles.OPERATION = operation.Update;
            if (VehicleID != "")
            {
                _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VehicleID);
            }
            DT = BLL.get_Vehicles(_obj_Smvts_Vehicles);
            if (DT.Rows.Count > 0)
            {
                count3 = Convert.ToInt32(DT.Rows[0][0]);
            }

            _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_Vehicles.VEHICLES_REGNUMBER = VNO;
            _obj_Smvts_Vehicles.VEHICLES_VEHICLEMAKEMODEL_ID = Convert.ToInt32(VehicleModelID);
            _obj_Smvts_Vehicles.VEHICLES_MAXSPEED = VehicleMaxspeed;
            _obj_Smvts_Vehicles.VEHICLES_MILEAGE = Mileage;
            _obj_Smvts_Vehicles.VEHICLES_OPENINGODOMETER = OpeningOdometer;
            _obj_Smvts_Vehicles.VEHICLES_CURRENTODOMETER = CurrentOdometer;
            _obj_Smvts_Vehicles.VEHICLES_DEVICE_ID = Convert.ToInt32(DeviceID);
            _obj_Smvts_Vehicles.VEHICLES_DRIVER_ID = Convert.ToInt32(DriverID);
            _obj_Smvts_Vehicles.VEHICLES_RESERVEVOLUME = Reservevolume;
            _obj_Smvts_Vehicles.VEHICLES_TANKCAPACITY = Convert.ToString(TankCapacity);
            _obj_Smvts_Vehicles.VEHICLES_STATUS = Convert.ToBoolean(ChkStatus);
            _obj_Smvts_Vehicles.VEHICLES_CAPACITY = Convert.ToString(LoadCapacity);
            _obj_Smvts_Vehicles.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Vehicles.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Vehicles.LASTMDFDATE = DateTime.Now;
            if (catid == 11)
            {
                //pradeep
                //if (rtxt_vehgroupzone.SelectedItem.Text != "Other")
                //{
                //    _obj_Smvts_Vehicles.vehicles_groupzone = Convert.ToString(rtxt_vehgroupzone.SelectedItem.Text);
                //}
                //else
                //{
                //    _obj_Smvts_Vehicles.vehicles_groupzone = txt_vehzone.Text;
                //}
            }
            else
            {
                _obj_Smvts_Vehicles.vehicles_groupzone = string.Empty;
            }



            // code for checking vehicles for BPO service
            // pradeep
            //if (Convert.ToString(ViewState["VECHILESFORBPO"]) == "TRUE")
            //{
            //    if (rcmb_OfficeName.Items.Count > 1) _obj_Smvts_Vehicles.VEHICLES_OFFICEMASTER_ID = Convert.ToInt32(rcmb_OfficeName.SelectedValue);
            //    if (rcmb_VendorName.Items.Count > 1) _obj_Smvts_Vehicles.VEHICLES_VENDORMASTER_ID = Convert.ToInt32(rcmb_VendorName.SelectedValue);
            //    if (rtxt_SeatingCapacity.Text != string.Empty) _obj_Smvts_Vehicles.VEHICLES_SEATINGCAPACITY = Convert.ToInt32(rtxt_SeatingCapacity.Text);
            //}
            //else
            //{
            _obj_Smvts_Vehicles.VEHICLES_OFFICEMASTER_ID = 0;
            _obj_Smvts_Vehicles.VEHICLES_VENDORMASTER_ID = 0;
            _obj_Smvts_Vehicles.VEHICLES_SEATINGCAPACITY = 0;
            //}
            DateTime date = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            DataTable dt_ = new DataTable();
            dt_ = Dal.ExecuteQuery("select CATEG_ID,CATEG_NAME,CATEG_EMAILID from SMVTS_CATEGORIES(nolock) where CATEG_ID=" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + " and CATEG_STATUS=1");
            switch (btn)
            {
                case "BTN_SAVE":
                    if (count1 == 0)
                    {
                        if (count2 == 0)
                        {
                            if (count3 == 0)
                            {
                                _obj_Smvts_Vehicles.OPERATION = operation.Insert;
                                if (BLL.set_Vehicles(_obj_Smvts_Vehicles))
                                {
                                    //new vehicle registration mail alert
                                    if (dt_.Rows.Count != 0)
                                    {
                                        sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">").Append("<head>").Append("<title>PragatiVTS</title>");
                                        sb.Append("</head>");
                                        sb.Append("<body>");
                                        sb.Append("<center><p><font face = 'Arial' size='3'><B>New Device Installation Details</B></font></p></center>");
                                        sb.Append("<table>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("Dear Team").Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("We are thankful for the opportunity provided to us  by your esteemed organization. Kindly find the below activation details.").Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("</table>");
                                        sb.Append("<table>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("   ").Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("</table>");
                                        sb.Append("<table>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("Date").Append("</td>");
                                        sb.Append("<td>").Append(":").Append("</td>");
                                        sb.Append("<td>").Append(date).Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("DeviceID").Append("</td>");
                                        sb.Append("<td>").Append(":").Append("</td>");
                                        sb.Append("<td>").Append(Convert.ToInt32(DeviceID)).Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("VehicleNo").Append("</td>");
                                        sb.Append("<td>").Append(":").Append("</td>");
                                        sb.Append("<td>").Append(VNO).Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("DriverName").Append("</td>");
                                        sb.Append("<td>").Append(":").Append("</td>");
                                        sb.Append("<td>").Append(DriverID).Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("</table>");
                                        sb.Append("<table>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("Regards,").Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("<tr>");
                                        sb.Append("<td>").Append("<strong style=\"color: rgb(0, 0, 51); font-family: 'Trebuchet MS'; font-size: 15px; font-style: normal; font-variant: normal; letter-spacing: normal; line-height: 22.5px; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);\">PragatiVTS Team.").Append("</td>");
                                        sb.Append("</tr>");
                                        sb.Append("</table>");
                                        sb.Append("</body>");
                                        sb.Append("</html>");

                                        //if (Sendmailvehicles(string.Format(sb.ToString()), Convert.ToInt32(DeviceID), VNO, DriverName, dt_.Rows[0]["CATEG_EMAILID"].ToString()))
                                        //{

                                        //}

                                    }
                                    MSG = "true";
                                    //BLL.ShowMessage(this, BLL.msg_Saved);



                                    //SmvtsDeviceUpdate(date,
                                    //                  Convert.ToInt32(DeviceID),//new device id
                                    //                  VNO,//new vehilce reg number
                                    //                  DriverID,//new driver name
                                    //                  Convert.ToBoolean(ChkStatus) ? "Active" : "Inactive",//status 
                                    //                  ((SMVTS_USERS)Session["USERINFO"]).USERS_ID,//created by
                                    //                  null,//old device id
                                    //                  null,//old vehicle reg number
                                    //                  null//old driver name
                                    //                  );
                                }
                                else
                                {
                                    //BLL.ShowMessage(this, BLL.msg_UnSaved);
                                    MSG = "Failed";
                                }
                                // break;
                            }
                            else
                            {
                                MSG = "Registration Number Already Exist.";
                                //BLL.ShowMessage(this, "Registration Number Already Exist.");
                                //rtxt_VehiclesRegNumber.Focus();
                                //return;

                            }
                        }
                        else
                        {
                            MSG = "Driver Already Exist.";
                            //BLL.ShowMessage(this, "Driver Already Exist.");
                            //rcmb_VehiclesDriverID.Focus();
                            //return;

                        }
                    }
                    else
                    {
                        MSG = "Device Already Exist.";
                        //BLL.ShowMessage(this, "Device Already Exist.");
                        //rcmb_VehiclesDeviceID.Focus();
                        //return;

                    }
                    break;
                case "BTN_UPDATE":
                    if (count1 == 0 || count1 == 1)
                    {
                        if (count2 == 0 || count2 == 1)
                        {
                            if (count3 == 0 || count3 == 1)
                            {
                                _obj_Smvts_Vehicles.OPERATION = operation.Update;
                                _obj_Smvts_Vehicles.VEHICLES_ID = Convert.ToInt32(VehicleID);
                                if (BLL.set_Vehicles(_obj_Smvts_Vehicles))
                                {

                                    MSG = "true";

                                }
                                else
                                {
                                    //BLL.ShowMessage(this, BLL.msg_UnSaved);
                                    MSG = "Failed";
                                }
                                break;
                            }
                            else
                            {
                                MSG = "Registration Number Already Exist.";
                                //BLL.ShowMessage(this, "Registration Number Already Exist.");
                                //rtxt_VehiclesRegNumber.Focus();
                                //return;
                            }
                        }
                        else
                        {
                            MSG = "Driver Already Exist.";
                            //BLL.ShowMessage(this, "Driver Already Exist.");
                            //rcmb_VehiclesDriverID.Focus();
                            //return;
                        }
                    }
                    else
                    {
                        MSG = "Device Already Exist.";
                        //BLL.ShowMessage(this, "Device Already Exist.");
                        //rcmb_VehiclesDeviceID.Focus();
                        //return;
                    }
                    break;
                default:
                    break;
            }
            return Json(new { data = MSG });
        }









        public bool Sendmailvehicles(string str, int Mail_deviceid, string mail_vno, string mail_drivername, string categ_emailid)
        {
            try
            {

                string strConnect = ("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGL4CY+O3mKqzXfd60oum+YaATXejNpf60UccZw/xfz9gbvinLUYnP6shgdIMQicpZqyJMAysRhs0NPugSf85OK8=");
                DataTable DT = Dal.ExecuteQueryDB1("SELECT * FROM SMVTS_SMTP_LOGIN(NOLOCK)", strConnect);
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.Host = DT.Rows[0][2].ToString();
                smtpClient.Port = Convert.ToInt32(DT.Rows[0][4].ToString());
                smtpClient.EnableSsl = DT.Rows[0][5].ToString() == "SSL" ? true : false;
                smtpClient.Credentials = new System.Net.NetworkCredential(DT.Rows[0][1].ToString(), DT.Rows[0][3].ToString());



                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("info@ctpl.com");
                mailMessage.To.Add(new System.Net.Mail.MailAddress(categ_emailid));
                mailMessage.CC.Add(new System.Net.Mail.MailAddress("info@ctpl.com"));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress("prashanth.a@containe.com"));
                mailMessage.Subject = "New Device Installation Details for '" + mail_vno + "'," + DateTime.Now.ToLongTimeString();
                // mailMessage.Subject = "Hi Pragatipadh - " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
                mailMessage.Priority = System.Net.Mail.MailPriority.High;

                //mailMessage.Body = str;

                AlternateView av1 = InlineAttachment(str);
                mailMessage.AlternateViews.Add(av1);
                mailMessage.IsBodyHtml = true;

                //string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();


                //System.IO.StreamWriter file = new System.IO.StreamWriter(paths + "\\mainlog.txt", true);

                try
                {


                    smtpClient.Send(mailMessage);

                    //Console.WriteLine("Mail sent." + EmailId + " At " + DateTime.Now.ToString());
                }
                catch (Exception)
                {
                    Response.Write("Error: can not send mail to " + mailMessage.From + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                }
                Response.Write("Mail sent to " + mailMessage.From + ". (" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + ")");
                Response.Close();
            }
            catch (Exception ex)
            {
                //  BLL.ShowMessage(this, ex.Message);
            }
            return true;
        }








        public JsonResult Edit_Vehicles(string VEHICLEID)
        {

            int catid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            List<SMVTS_DEVICES> obj1 = new List<SMVTS_DEVICES>();
            DataTable dt = BLL.get_Vehicles(new SMVTS_VEHICLES(Convert.ToInt32(VEHICLEID)));
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                _obj_Smvts_Devices.OPERATION = operation.Select;
            }
            else
            {
                _obj_Smvts_Devices.OPERATION = operation.Empty;
                _obj_Smvts_Devices.DEVICE_STATUS = "true";
                _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }


            if ((Convert.ToInt32(dt.Rows[0]["VEHICLES_DEVICE_ID"]) == 0) || (Convert.ToString(dt.Rows[0]["VEHICLES_DEVICE_ID"]) == string.Empty))
            {
                _obj_Smvts_Devices.DEVICE_ID = -1;  //false logic to get the correct method exectute
            }
            else
            {
                _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(Convert.ToString(dt.Rows[0]["VEHICLES_DEVICE_ID"]));
            }
            DataTable dt1 = BLL.get_Devices(_obj_Smvts_Devices);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj1.Add(new SMVTS_DEVICES
                {

                    DEVICE_ID = Convert.ToInt32(dt1.Rows[i]["DEVICE_ID"]),
                    DEVICE_NAME = Convert.ToString(dt1.Rows[i]["DEVICE_NAME"]),

                });
            }




            SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
            List<SMVTS_DRIVERS> obj2 = new List<SMVTS_DRIVERS>();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                _obj_Smvts_Drivers.OPERATION = operation.Select;
                _obj_Smvts_Drivers.DRIVER_ID = Convert.ToInt32(Convert.ToString(dt.Rows[0]["VEHICLES_DRIVER_ID"]));
            }
            else
            {
                _obj_Smvts_Drivers.OPERATION = operation.Update;
                _obj_Smvts_Drivers.DRIVER_STATUS = true;
                _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Drivers.DRIVER_ID = Convert.ToInt32(Convert.ToString(dt.Rows[0]["VEHICLES_DRIVER_ID"]));
            }

            DataTable dt3 = BLL.get_Drivers(_obj_Smvts_Drivers);

            for (int k = 0; k < dt3.Rows.Count; k++)
            {
                obj2.Add(new SMVTS_DRIVERS
                {
                    DRIVER_ID = Convert.ToInt32(dt3.Rows[k]["DRIVER_ID"]),
                    DRIVER_NAME = Convert.ToString(dt3.Rows[k]["DRIVER_NAME"]),

                });
            }

            SMVTS_VEHLEMM _obj_Smvts_Vehlemm = new SMVTS_VEHLEMM();
            List<SMVTS_VEHLEMM> obj4 = new List<SMVTS_VEHLEMM>();
            _obj_Smvts_Vehlemm.OPERATION = operation.Select;
            _obj_Smvts_Vehlemm.VEHLEMM_ID = Convert.ToInt32(Convert.ToString(dt.Rows[0]["VEHICLES_VEHICLEMAKEMODEL_ID"]));

            DataTable dt4 = BLL.get_Vehlemm(_obj_Smvts_Vehlemm);


            for (int m = 0; m < dt4.Rows.Count; m++)
            {
                obj4.Add(new SMVTS_VEHLEMM
                {
                    VEHLEMM_ID = Convert.ToInt32(dt4.Rows[m]["VEHLEMM_ID"]),
                    VEHLEMM_NAME = Convert.ToString(dt4.Rows[m]["VEHLEMM_NAME"]),

                });
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLES
                {
                    VEHICLES_REGNUMBER = dt.Rows[0]["VEHICLES_REGNUMBER"].ToString(),
                    VEHICLES_VEHICLEMAKEMODEL_ID = Convert.ToInt32(dt.Rows[0]["VEHICLES_VEHICLEMAKEMODEL_ID"]),
                    VEHICLES_MAXSPEED = Convert.ToString(dt.Rows[0]["VEHICLES_MAXSPEED"]),
                    VEHICLES_MILEAGE = Convert.ToString(dt.Rows[0]["VEHICLES_MILEAGE"]),
                    VEHICLES_OPENINGODOMETER = Convert.ToString(dt.Rows[0]["VEHICLES_OPENINGODOMETER"]),
                    VEHICLES_CURRENTODOMETER = Convert.ToString(dt.Rows[0]["VEHICLES_CURRENTODOMETER"]),
                    VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[0]["VEHICLES_DEVICE_ID"]),
                    VEHICLES_DRIVER_ID = Convert.ToInt32(dt.Rows[0]["VEHICLES_DRIVER_ID"]),
                    VEHICLES_TANKCAPACITY = Convert.ToString(dt.Rows[0]["VEHICLES_TANKCAPACITY"]),
                    VEHICLES_RESERVEVOLUME = Convert.ToString(dt.Rows[0]["VEHICLES_RESERVEVOLUME"]),
                    VEHICLES_CAPACITY = Convert.ToString(dt.Rows[0]["VEHICLES_CAPACITY"]),
                    VEHICLES_STATUS = Convert.ToBoolean(dt.Rows[0]["VEHICLES_STATUS"]),
                });
            }

            return (Json(new { data = obj, data1 = obj1, data2 = obj2, data3 = obj4 }));


        }



        [SessionAuthorize]
        public ActionResult frm_VehicleModel()
        {


            return View();
        }

        public JsonResult _frm_vehicleModel()
        {
            List<SMVTS_VEHLEMM> obj = new List<SMVTS_VEHLEMM>();
            SMVTS_VEHLEMM _obj_Smvts_Vehlemm = new SMVTS_VEHLEMM();
            _obj_Smvts_Vehlemm.OPERATION = operation.Select;
            _obj_Smvts_Vehlemm.VEHLEMM_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable DT = BLL.get_Vehlemm(_obj_Smvts_Vehlemm);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHLEMM
                {
                    VEHLEMM_ID = Convert.ToInt32(DT.Rows[i]["VEHLEMM_ID"]),
                    VEHLEMM_NAME = Convert.ToString(DT.Rows[i]["VEHLEMM_NAME"]),
                    VEHLEMM_DESC = Convert.ToString(DT.Rows[i]["VEHLEMM_DESC"]),
                    VEHLEMM_MAKE = Convert.ToString(DT.Rows[i]["VEHLEMM_MAKE"]),
                    VEHLEMM_MODEL = Convert.ToString(DT.Rows[i]["VEHLEMM_MODEL"]),
                    VEHLEMM_YEAR = Convert.ToString(DT.Rows[i]["VEHLEMM_YEAR"]),
                    VEHICLE_REGNUMBER = Convert.ToString(DT.Rows[i]["VEHICLES_REGNUMBER"])
                });
            }
            return Json(new { data = obj });
        }

        public JsonResult EditVehicleModel(int VEHMDL_ID)
        {
            List<SMVTS_VEHLEMM> obj = new List<SMVTS_VEHLEMM>();
            SMVTS_VEHLEMM _obj_Smvts_Vehlemm = new SMVTS_VEHLEMM();
            _obj_Smvts_Vehlemm.OPERATION = operation.Select;
            _obj_Smvts_Vehlemm.VEHLEMM_ID = VEHMDL_ID;
            DataTable DT = BLL.get_Vehlemm(_obj_Smvts_Vehlemm);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHLEMM
                {
                    VEHLEMM_ID = Convert.ToInt32(DT.Rows[i]["VEHLEMM_ID"]),
                    VEHLEMM_NAME = Convert.ToString(DT.Rows[i]["VEHLEMM_NAME"]),
                    VEHLEMM_DESC = Convert.ToString(DT.Rows[i]["VEHLEMM_DESC"]),
                    VEHLEMM_MAKE = Convert.ToString(DT.Rows[i]["VEHLEMM_MAKE"]),
                    VEHLEMM_MODEL = Convert.ToString(DT.Rows[i]["VEHLEMM_MODEL"]),
                    VEHLEMM_YEAR = Convert.ToString(DT.Rows[i]["VEHLEMM_YEAR"]),

                });
            }

            return Json(new { data = obj });
        }

        [SessionAuthorize]
        public ActionResult frm_Drivers()
        {


            return View();
        }



        public JsonResult frm_getdrivers()
        {
            List<SMVTS_DRIVERS> obj = new List<SMVTS_DRIVERS>();
            SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
            _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_Drivers.OPERATION = operation.Select;
            DataTable dt = BLL.get_Drivers(_obj_Smvts_Drivers);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DRIVERS
                {
                    DRIVER_ID = Convert.ToInt32(dt.Rows[i]["DRIVER_ID"]),
                    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                    DRIVER_MOBILENO = dt.Rows[i]["DRIVER_MOBILENO"].ToString(),
                    DRIVER_LICENSENO = dt.Rows[i]["DRIVER_LICENSENO"].ToString(),
                    DRIVER_LICENSE_ISSUEDATE = Convert.ToString(dt.Rows[i]["DRIVER_ISSUEDATE"]),
                    DRIVER_LICENSE_EXPIRYDATE = Convert.ToString(dt.Rows[i]["DRIVER_EXPIRYDATE"]),
                    DRIVER_BLOODGROUP = dt.Rows[i]["DRIVER_BLOODGROUP"].ToString(),
                    DRIVERSTATUS = Convert.ToString(dt.Rows[i]["DRIVER_STATUS"]),
                    DRIVER_PHOTO_PATH = dt.Rows[i]["DRIVER_PHOTO_PATH"].ToString(),
                    VEHICLE_NUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString()
                });
            }

            return Json(new { data = obj });
        }

        public JsonResult SaveVehicleModel(string Name, string Make, string VehicleType, string Tank, string Unit, string Description, string Model, string year, string tankCapacity)
        {

            SMVTS_VEHICLEMODELS obj = new SMVTS_VEHICLEMODELS();
            obj.VEHLEMM_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            obj.VEHLEMM_CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            obj.VEHLEMM_CREATEDDATE = DateTime.Now;
            obj.VEHLEMM_DESC = Description;
            obj.VEHLEMM_IMAGEVALUE = VehicleType;
            obj.VEHLEMM_MAKE = Make;
            obj.VEHLEMM_MEASURINGUNIT = Unit;
            obj.VEHLEMM_MODEL = Model;
            obj.VEHLEMM_NAME = Name;
            obj.VEHLEMM_NOOFTANKS = Convert.ToInt32(Tank);
            obj.VEHLEMM_TANKCAPACITY = tankCapacity;
            obj.VEHLEMM_YEAR = year;
            bool status = BLL.Insert_VehicleModels(obj);

            return Json(new { data = status });
        }

        public JsonResult UpdateVehicleModel(string Name, string Make, string VehicleType, string VehicleTypeName, string Tank, string Unit, string Description, string Model, string year, string tankCapacity, int VehmodelId)
        {
            SMVTS_VEHICLEMODELS obj = new SMVTS_VEHICLEMODELS();
            obj.VEHLEMM_ID = VehmodelId;
            obj.VEHLEMM_MODIFIEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            obj.VEHLEMM_MODIFIEDDATE = DateTime.Now;
            obj.VEHLEMM_NAME = VehicleTypeName;
            obj.VEHLEMM_DESC = Description;
            obj.VEHLEMM_IMAGEVALUE = VehicleType;
            obj.VEHLEMM_MAKE = Make;
            obj.VEHLEMM_MEASURINGUNIT = Unit;
            obj.VEHLEMM_MODEL = Model;
            obj.VEHLEMM_NAME = Name;
            obj.VEHLEMM_NOOFTANKS = Convert.ToInt32(Tank);
            obj.VEHLEMM_TANKCAPACITY = tankCapacity;
            obj.VEHLEMM_YEAR = year;
            bool status = BLL.Update_VehicleModels(obj);
            return Json(new { data = status });
        }





        public JsonResult _frn_drivers(HttpPostedFileBase MyImages, string ImagePath, string btn, string DRIVERNAME, string DriverID, string FORMAN, string DRIVERMOBILENO, string DESCRIPTION, string ADDRESS, string LICENSENO, string LICENSEEXPIRYDATE, string LICENSEISSUEDATE, string LANGUAGESKNOWN, string BLOODGROUP, string STATUS)
        {
            string msg = "";
            try
            {
                string ipath = "";
                if (MyImages != null)
                {
                    string _FileName = Path.GetFileName(MyImages.FileName);
                    Random r = new Random();
                    int number = r.Next(100, 10000000);
                    string _path = Path.Combine(Server.MapPath("~/IMAGES/DRIVERS"), number + _FileName);
                    MyImages.SaveAs(_path);
                    ipath = number + _FileName;
                }
                else
                {
                    if (ImagePath != "" && ImagePath != null)
                    {
                        ipath = ImagePath;
                    }

                }
                //Already Exist condition Driver Name
                int count = 0, cntLiceneNo = 0;
                DataTable dt = new DataTable();
                SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
                _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Drivers.DRIVER_NAME = DRIVERNAME;
                _obj_Smvts_Drivers.OPERATION = operation.Check;
                if (DriverID != "")
                {
                    _obj_Smvts_Drivers.DRIVER_ID = Convert.ToInt32(DriverID);
                }

                dt = BLL.get_Drivers(_obj_Smvts_Drivers);
                if (dt.Rows.Count > 0)
                {
                    count = Convert.ToInt32(dt.Rows[0][0]);
                }

                //Already Exist condition licence no       
                _obj_Smvts_Drivers = new SMVTS_DRIVERS();
                _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Drivers.DRIVER_LICENSENO = LICENSENO;
                _obj_Smvts_Drivers.OPERATION = operation.Insert;
                if (DriverID != "")
                {
                    _obj_Smvts_Drivers.DRIVER_ID = Convert.ToInt32(DriverID);
                }

                dt = BLL.get_Drivers(_obj_Smvts_Drivers);
                if (dt.Rows.Count > 0)
                {
                    cntLiceneNo = Convert.ToInt32(dt.Rows[0][0]);
                }


                _obj_Smvts_Drivers = new SMVTS_DRIVERS();
                if (LICENSEEXPIRYDATE == null)
                    _obj_Smvts_Drivers.DRIVER_EXPIRYDATE = null;
                else
                    _obj_Smvts_Drivers.DRIVER_EXPIRYDATE = Convert.ToDateTime(LICENSEEXPIRYDATE);

                if (LICENSEISSUEDATE == null)
                    _obj_Smvts_Drivers.DRIVER_ISSUEDATE = null;
                else
                    _obj_Smvts_Drivers.DRIVER_ISSUEDATE = Convert.ToDateTime(LICENSEISSUEDATE);

                _obj_Smvts_Drivers.DRIVER_ADDRESS = BLL.ReplaceQuote(ADDRESS);
                _obj_Smvts_Drivers.DRIVER_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_Drivers.DRIVER_DESC = DESCRIPTION;
                _obj_Smvts_Drivers.DRIVER_LICENSENO = LICENSENO;
                _obj_Smvts_Drivers.DRIVER_MOBILENO = DRIVERMOBILENO;
                _obj_Smvts_Drivers.DRIVER_NAME = DRIVERNAME;
                _obj_Smvts_Drivers.DRIVER_STATUS = Convert.ToBoolean(STATUS);
                _obj_Smvts_Drivers.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Drivers.CREATEDDATE = DateTime.Now;
                _obj_Smvts_Drivers.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_Drivers.LASTMDFDATE = DateTime.Now;



                _obj_Smvts_Drivers.DRIVER_BLOODGROUP = BLOODGROUP;
                _obj_Smvts_Drivers.DRIVER_LANGUAGES = LANGUAGESKNOWN;
                _obj_Smvts_Drivers.DRIVER_PHOTO_PATH = ipath;
                _obj_Smvts_Drivers.DRIVER_FORMAN = FORMAN;




                switch (btn)
                {
                    case "BTN_SAVE":
                        if (count == 0)
                        {
                            if ((cntLiceneNo == 0) || (cntLiceneNo >= 1))
                            {
                                _obj_Smvts_Drivers.OPERATION = operation.Insert;
                                if (BLL.set_Drivers(_obj_Smvts_Drivers))
                                {
                                    msg = "true";
                                    //  BLL.ShowMessage(this, BLL.msg_Saved);
                                }
                                else
                                {
                                    msg = "please try again..";
                                    //  BLL.ShowMessage(this, BLL.msg_UnSaved);
                                }

                            }
                            else
                            {
                                msg = "Licence Number Already Exists.";
                                //  BLL.ShowMessage(this, "Licence Number Already Exists.");
                                // rtxt_DriverLicenseNo.Focus();
                                //  return;
                            }
                        }
                        else
                        {
                            msg = "Driver Name Already Exists.";
                            // BLL.ShowMessage(this, "Driver Name Already Exists.");
                            // rtxt_DriverName.Focus();
                            // return;
                        }
                        break;
                    case "BTN_UPDATE":
                        if ((count == 0) || (count == 1))
                        {
                            if ((cntLiceneNo == 0) || (cntLiceneNo == 1) || (cntLiceneNo >= 2))
                            {
                                _obj_Smvts_Drivers.OPERATION = operation.Update;
                                _obj_Smvts_Drivers.DRIVER_ID = Convert.ToInt32(DriverID);
                                if (BLL.set_Drivers(_obj_Smvts_Drivers))
                                {
                                    msg = "true";
                                    //  BLL.ShowMessage(this, BLL.msg_Updated);
                                }

                                else
                                {
                                    msg = "please try again..";
                                    //  BLL.ShowMessage(this, BLL.msg_UnSaved);
                                }

                            }
                            else
                            {
                                msg = "Licence Number Already Exists.";
                                //  BLL.ShowMessage(this, "Licence Number Already Exists.");
                                // rtxt_DriverLicenseNo.Focus();
                                // return;
                            }
                        }
                        else
                        {
                            msg = "Driver Name Already Exists.";
                            //  BLL.ShowMessage(this, "Driver Name Already Exists.");
                            //  rtxt_DriverName.Focus();
                            //  return;
                        }
                        break;


                    default:
                        break;
                }


            }
            catch (Exception ex)
            {
                BLL.errorLog("frm_Driver_Save", ex.ToString());
            }
            return Json(new { data = msg });
        }




        public JsonResult Edit_drivers(string DriverID)
        {
            List<SMVTS_DRIVERS> obj = new List<SMVTS_DRIVERS>();
            SMVTS_DRIVERS _obj_Smvts_Drivers = new SMVTS_DRIVERS();
            try
            {
                DataTable dt = BLL.get_Drivers(new SMVTS_DRIVERS(Convert.ToInt32(DriverID)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_DRIVERS
                    {
                        DRIVER_ID = Convert.ToInt32(dt.Rows[i]["DRIVER_ID"]),
                        DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                        DRIVER_MOBILENO = dt.Rows[i]["DRIVER_MOBILENO"].ToString(),
                        DRIVER_LICENSENO = dt.Rows[i]["DRIVER_LICENSENO"].ToString(),
                        DRIVER_LICENSE_ISSUEDATE = Convert.ToString(dt.Rows[i]["DRIVER_ISSUEDATE"]),
                        DRIVER_LICENSE_EXPIRYDATE = Convert.ToString(dt.Rows[i]["DRIVER_EXPIRYDATE"]),
                        DRIVER_BLOODGROUP = dt.Rows[i]["DRIVER_BLOODGROUP"].ToString(),
                        DRIVERSTATUS = Convert.ToString(dt.Rows[i]["DRIVER_STATUS"]),
                        DRIVER_PHOTO_PATH = dt.Rows[i]["DRIVER_PHOTO_PATH"].ToString(),
                        DRIVER_DESC = dt.Rows[i]["DRIVER_DESC"].ToString(),
                        DRIVER_ADDRESS = dt.Rows[i]["DRIVER_ADDRESS"].ToString(),
                        DRIVER_FORMAN = dt.Rows[i]["DRIVER_FORMAN"].ToString(),
                        DRIVER_LANGUAGES = dt.Rows[i]["DRIVER_LANGUAGES"].ToString(),
                    });
                }

            }
            catch (Exception ex)
            {

            }

            return Json(new { data = obj });

        }
        [SessionAuthorize]
        public ActionResult frm_GeoZone_Dashboard()
        {
            SMVTS_ASSOCIATED _obj_smvts_associated = new SMVTS_ASSOCIATED();
            List<SMVTS_USERS> obj = new List<SMVTS_USERS>();
            _obj_smvts_associated.associated = ((SMVTS_USERS)(Session["USERINFO"])).USERS_ID.ToString();
            _obj_smvts_associated.OPERATION = operation.Select;
            DataTable dt = BLL.get_associated(_obj_smvts_associated);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_USERS
                {
                    USERS_USERNAME = dt.Rows[i]["USERS_USERNAME"].ToString(),
                    USERS_ID = Convert.ToInt32(dt.Rows[i]["USERS_ID"])
                });
            }
            ViewBag.gpsdashboard = obj;
            return View();
        }
        [SessionAuthorize]
        public JsonResult frm_asotriplezoneOLD(string NAME, string ID)
        {
            DataTable DT_ASSUSERS = new DataTable();
            if (NAME == "Tglobal")
            {
                DT_ASSUSERS = BLL.ExecuteQuery("select USERS_ID,USERS_USERNAME,CATEG_ID from SMVTS_USERS(nolock) inner join SMVTS_CATEGORIES(NOLOCK) on CATEG_ID=USERS_CATEGORY_ID where USERS_USERNAME='" + NAME + "'");
            }
            else
            {
                DT_ASSUSERS = BLL.ExecuteQuery("SELECT USERS_ID,USERS_USERNAME,CATEG_ID FROM SMVTS_ASSCGRID(NOLOCK) INNER JOIN SMVTS_USERS(NOLOCK) ON USERS_ID=ASSOCGRID_USERID INNER JOIN SMVTS_CATEGORIES(NOLOCK) ON CATEG_ID=USERS_CATEGORY_ID where ASSOCGRID_USERID=" + ID + "");
            }
            DataTable DT = BLL.ExecuteQuery("select * from SMVTS_CONTROLROOM_DYNAMIC WHERE DYNAMIC_CATEGID=" + 29 + " AND DYNAMIC_SCREEN='SCREEN9'");
            //  CalltglobalMap_URL_tripple(Convert.ToString(DT.Rows[0]["DYNAMIC_CITY"]), Convert.ToString(DT.Rows[0]["DYNAMIC_STATE"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_LAT"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_LONG"]), Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LAT"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LONG"]), Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_RADIUS"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_RADIUS"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_ZOOM"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_ZOOM"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_LAT"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_LONG"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_RADIUS"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_ZOOM"]), Convert.ToString(DT.Rows[0]["DYNAMIC_LANDMARKID"]),
            //Convert.ToString(DT_ASSUSERS.Rows[0]["CATEG_ID"]), Convert.ToString(DT_ASSUSERS.Rows[0]["USERS_ID"]));

            string categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            string user_id = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);

            string city = Convert.ToString(DT.Rows[0]["DYNAMIC_CITY"]);
            string State = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE"]);
            string citylat = "18.749213";// Request.QueryString["CityLat"].ToString();
            string citylong = "73.799927";//Request.QueryString["CityLong"].ToString();
            string statelat = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LAT"]);
            string statelong = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LONG"]);
            string cityRadius = "70";// Request.QueryString["CityRad"].ToString();//Request.QueryString["CityRad"].ToString()//40
            string stateRadius = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_RADIUS"]);//Request.QueryString["StateRad"].ToString()
            string cityZoom = "10"; // Request.QueryString["CityZoom"].ToString();
            string stateZoom = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_ZOOM"]);
            // string maharastralat = Request.QueryString["Maharastralat"].ToString();
            //   string maharastralong = Request.QueryString["Maharastralong"].ToString();
            string maharastraradius = "50";//Request.QueryString["MaharastraRadius"].ToString()//50
            string maharastrazoom = "9";// Request.QueryString["MaharastraZoom"].ToString();
            //   string maharastralandmark_ID = Convert.ToString(DT.Rows[0]["DYNAMIC_LANDMARKID"]);


            // _obj_Smvts_Users.USERS_DBNAME = Session["DBNAME"].ToString();

            DataTable dt1 = new DataTable();
            DataSet ds = (BLL.get_GeoZoneDashboard_triple2(user_id, statelat, statelong,
                Convert.ToInt32(stateRadius), categid, citylat, citylong, Convert.ToInt32(cityRadius),
                "RIGHTSCREEN", Convert.ToString(((SMVTS_USERS)(Session["USERINFO"])).USERS_DBNAME)));
            dt1 = ds.Tables[0];
            DataTable dt_excel = BLL.ExecuteQuery("EXEC [USP_SMVTS_EXCEL_SCREEN]");
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt_excel.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt_excel.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            //   return (Json(new { data = dt_excel }));
            var jsonData = new
            {
                data = rows
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;





        }








        public JsonResult frm_asotriplezone(string NAME, string ID)
        {

            DataTable DT_ASSUSERS = new DataTable();
            if (NAME == "Tglobal")
            {
                DT_ASSUSERS = BLL.ExecuteQuery("select USERS_ID,USERS_USERNAME,CATEG_ID from SMVTS_USERS(nolock) inner join SMVTS_CATEGORIES(NOLOCK) on CATEG_ID=USERS_CATEGORY_ID where USERS_USERNAME='" + NAME + "'");
            }
            else
            {
                DT_ASSUSERS = BLL.ExecuteQuery("SELECT USERS_ID,USERS_USERNAME,CATEG_ID FROM SMVTS_ASSCGRID(NOLOCK) INNER JOIN SMVTS_USERS(NOLOCK) ON USERS_ID=ASSOCGRID_USERID INNER JOIN SMVTS_CATEGORIES(NOLOCK) ON CATEG_ID=USERS_CATEGORY_ID where ASSOCGRID_USERID=" + ID + "");
            }
            DataTable DT = BLL.ExecuteQuery("select * from SMVTS_CONTROLROOM_DYNAMIC WHERE DYNAMIC_CATEGID=" + 29 + " AND DYNAMIC_SCREEN='SCREEN9'");
            //  CalltglobalMap_URL_tripple(Convert.ToString(DT.Rows[0]["DYNAMIC_CITY"]), Convert.ToString(DT.Rows[0]["DYNAMIC_STATE"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_LAT"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_LONG"]), Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LAT"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LONG"]), Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_RADIUS"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_RADIUS"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY_ZOOM"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_ZOOM"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_LAT"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_LONG"]), Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_RADIUS"]), 
            //Convert.ToString(DT.Rows[0]["DYNAMIC_CITY1_ZOOM"]), Convert.ToString(DT.Rows[0]["DYNAMIC_LANDMARKID"]),
            //Convert.ToString(DT_ASSUSERS.Rows[0]["CATEG_ID"]), Convert.ToString(DT_ASSUSERS.Rows[0]["USERS_ID"]));

            string categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            string user_id = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);

            string city = Convert.ToString(DT.Rows[0]["DYNAMIC_CITY"]);
            string State = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE"]);
            string citylat = "18.749213";// Request.QueryString["CityLat"].ToString();
            string citylong = "73.799927";//Request.QueryString["CityLong"].ToString();
            string statelat = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LAT"]);
            string statelong = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_LONG"]);
            string cityRadius = "70";// Request.QueryString["CityRad"].ToString();//Request.QueryString["CityRad"].ToString()//40
            string stateRadius = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_RADIUS"]);//Request.QueryString["StateRad"].ToString()
            string cityZoom = "10"; // Request.QueryString["CityZoom"].ToString();
            string stateZoom = Convert.ToString(DT.Rows[0]["DYNAMIC_STATE_ZOOM"]);
            // string maharastralat = Request.QueryString["Maharastralat"].ToString();
            //   string maharastralong = Request.QueryString["Maharastralong"].ToString();
            string maharastraradius = "50";//Request.QueryString["MaharastraRadius"].ToString()//50
            string maharastrazoom = "9";// Request.QueryString["MaharastraZoom"].ToString();
            //   string maharastralandmark_ID = Convert.ToString(DT.Rows[0]["DYNAMIC_LANDMARKID"]);


            // _obj_Smvts_Users.USERS_DBNAME = Session["DBNAME"].ToString();

            DataTable dt1 = new DataTable();
            DataSet ds = (BLL.get_GeoZoneDashboard_triple2(user_id, statelat, statelong,
                Convert.ToInt32(stateRadius), categid, citylat, citylong, Convert.ToInt32(cityRadius),
                "RIGHTSCREEN", Convert.ToString(((SMVTS_USERS)(Session["USERINFO"])).USERS_DBNAME)));
            dt1 = ds.Tables[0];
            //  DataTable dt_excel = BLL.ExecuteQuery("EXEC [USP_SMVTS_EXCEL_SCREEN]");


            DataTable dt = new DataTable();

            //dt.Columns.Add("SNO");
            //dt.Columns.Add("LOCATION");
            //dt.Columns.Add("TRUCKNO");
            //dt.Columns.Add("REMARK");
            //   DataRow dr = dt.NewRow();
            int NO = 0;
            int FN = 0;
            int lN = 0;
            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string data = dt1.Rows[j]["VNO"].ToString();


                string[] values = data.Split(',');


                DataRow dr = dt.NewRow();
                for (int k = 0; k < values.Length; k++)
                {
                    if (k == 0)
                    {
                        dt.Columns.Add("SNO" + dt1.Rows[j]["LOCATION"].ToString() + "");
                        dt.Columns.Add(dt1.Rows[j]["LOCATION"].ToString());
                        dt.Columns.Add("REMARK" + dt1.Rows[j]["LOCATION"].ToString() + "");
                        Session["NAME"] = dt1.Rows[j]["LOCATION"].ToString();
                        lN++;
                        if (lN == 1)
                        {
                            dr = dt.NewRow();
                            dr["SNO" + Session["NAME"].ToString() + ""] = 1;
                            dr[Session["NAME"].ToString()] = values[k];
                            dr["REMARK" + Session["NAME"].ToString() + ""] = "";
                            dt.Rows.Add(dr);
                            FN++;
                        }
                        else
                        {
                            //dr = dt.NewRow();
                            //dr["SNO" + j] = k;
                            //dr[Session["NAME"].ToString()] = values[k];
                            //dr["REMARK" + j] = "";
                            //dt.Rows.Add(dr);
                            dr = dt.NewRow();

                            dt.Rows[0]["SNO" + Session["NAME"].ToString() + ""] = k + 1;
                            dt.Rows[0][Session["NAME"].ToString()] = values[k];
                            dt.Rows[0]["REMARK" + Session["NAME"].ToString() + ""] = "";
                            dt.Rows.Add(dr);
                        }

                    }
                    else
                    {
                        if (NO == 0)
                        {
                            dr = dt.NewRow();
                            dr["SNO" + Session["NAME"].ToString() + ""] = k + 1;
                            dr[Session["NAME"].ToString()] = values[k];
                            dr["REMARK" + Session["NAME"].ToString() + ""] = "";
                            dt.Rows.Add(dr);
                            int index = dt.Rows.IndexOf(dr);
                            // dt.Rows[k][Session["NAME"].ToString()] = values[k];
                            FN++;
                        }
                        else
                        {
                            if (FN >= k)
                            {

                                dr = dt.NewRow();

                                dt.Rows[k]["SNO" + Session["NAME"].ToString() + ""] = k + 1;
                                dt.Rows[k][Session["NAME"].ToString()] = values[k];
                                dt.Rows[k]["REMARK" + Session["NAME"].ToString() + ""] = "";
                                dt.Rows.Add(dr);
                            }
                            else
                            {

                                //dr = dt.NewRow();
                                //dr["SNO" + j] = k;
                                //dr[Session["NAME"].ToString()] = values[k];
                                //dr["REMARK" + j] = "";
                                //dt.Rows.Add(dr);

                                // dt.Rows[k][Session["NAME"].ToString()] = values[k];
                                //  FN++;
                            }

                        }

                    }

                }
                NO++;
            }
            List<clspdf> obj = new List<clspdf>();
            for (int n = 0; n < dt.Rows.Count; n++)
            {
                obj.Add(new clspdf
                {
                    SNOKOLAR0 = Convert.ToString(dt.Rows[n]["SNOKOLAR"]),
                    SNOWALUJPLANT1 = Convert.ToString(dt.Rows[n]["SNOWALUJ PLANT"]),
                    SNOWALUJPARKING2 = Convert.ToString(dt.Rows[n]["SNOWALUJ PARKING"]),
                    SNOCHAKAN3 = Convert.ToString(dt.Rows[n]["SNOCHAKAN"]),
                    KOLAR = Convert.ToString(dt.Rows[n]["KOLAR"]),
                    CHAKAN = Convert.ToString(dt.Rows[n]["CHAKAN"]),
                    WALUJPARKING = Convert.ToString(dt.Rows[n]["WALUJ PARKING"]),
                    WALUJPLANT = Convert.ToString(dt.Rows[n]["WALUJ PLANT"]),
                    REMARK0 = Convert.ToString(dt.Rows[n]["REMARKKOLAR"]),
                    REMARK1 = Convert.ToString(dt.Rows[n]["REMARKWALUJ PLANT"]),
                    REMARK2 = Convert.ToString(dt.Rows[n]["REMARKWALUJ PARKING"]),
                    REMARK3 = Convert.ToString(dt.Rows[n]["REMARKCHAKAN"]),
                });
            }


            var jsondata = new
            {
                data = obj,
                tno = FN,
            };
            var jsonResult = Json(jsondata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }




        public class clspdf
        {
            public string SNOKOLAR0 { get; set; }
            public string SNOWALUJPLANT1 { get; set; }
            public string SNOWALUJPARKING2 { get; set; }
            public string SNOCHAKAN3 { get; set; }

            public string KOLAR { get; set; }
            public string CHAKAN { get; set; }
            public string WALUJPARKING { get; set; }
            public string WALUJPLANT { get; set; }

            public string REMARK0 { get; set; }
            public string REMARK1 { get; set; }
            public string REMARK2 { get; set; }
            public string REMARK3 { get; set; }
        }











        public void LoadAssociateUsers()
        {

            //rcmb_associate.DataTextField = "USERS_USERNAME";
            //rcmb_associate.DataValueField = "USERS_ID";

            //rcmb_associate.DataBind();
            //rcmb_associate.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select", "0"));
            //rcmb_associate.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Tglobal", "0"));
        }

        //public JsonResult _CheckLogin(string UserName, string Password)
        //{
        //    BLL obj = new BLL();

        //    ViewBag.panels1 = obj.GETALL_PANELS();
        //    var data = obj.get_User(UserName, Password);
        //    Session["USERID"] = data[0].USERID;
        //    Session["ROLEID"] = data[0].ROLEID;




        //    //  int id = Convert.ToInt32(Session["USERID"]);
        //    return Json(new { data = data });
        //}


        //public ActionResult Categories()
        //{
        //    int USER_ID = Convert.ToInt32(Session["USERID"]);
        //    //if (PAGE_PERMISSIONS(USER_ID, "Categories"))
        //    //{
        //    BLL obj = new BLL();
        //    ViewBag.data = obj.GETALL_CATEGORIES();
        //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Error");
        //    //}

        //}


        //public JsonResult edit_categories()
        //{
        //    BLL obj = new BLL();
        //    var data = obj.GETALL_CATEGORIES();
        //    return Json(new { data = data });
        //}

        //public JsonResult INSERT_CATEGORIES(string CATEG_NAME, string CATEG_EMAILID, string CATEG_DESC, string CATEG_CONTACTPERSON,
        //    string CATEG_MOBILENUMBER, string CATEG_TELEPHONE, string CATEG_WEBSITENAME, string CATEG_ZIPCODE, string CATEG_ADDRESS, int CATEG_CREATEDBY, string CATEG_TYPE)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);
        //    BLL obj = new BLL();
        //    bool b = obj.INSERT_CATEGORIES(CATEG_NAME, CATEG_EMAILID, CATEG_DESC, CATEG_CONTACTPERSON,
        //    CATEG_MOBILENUMBER, CATEG_TELEPHONE, CATEG_WEBSITENAME, CATEG_ZIPCODE, CATEG_ADDRESS, id, CATEG_TYPE);
        //    return Json(new { data = b });
        //}

        //public JsonResult CATEGORIES_UPDATE(string CATEG_NAME, string CATEG_EMAILID, string CATEG_DESC, string CATEG_CONTACTPERSON,
        //  string CATEG_MOBILENUMBER, string CATEG_TELEPHONE, string CATEG_WEBSITENAME, string CATEG_ZIPCODE, string CATEG_ADDRESS, string CATEG_TYPE, int CATEG_ID)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);
        //    BLL obj = new BLL();
        //    bool b = obj.CATEGORIES_UPDATE(CATEG_NAME, CATEG_EMAILID, CATEG_DESC, CATEG_CONTACTPERSON,
        //    CATEG_MOBILENUMBER, CATEG_TELEPHONE, CATEG_WEBSITENAME, CATEG_ZIPCODE, CATEG_ADDRESS, id, CATEG_TYPE, CATEG_ID);
        //    return Json(new { data = b });
        //}

        //public JsonResult UPDATE_CATEGORIES_STATUS(int CATEG_STATUS, int CATEG_ID, int CATEG_UPDATEDBY)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);

        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_CATEGORIES_STATUS(CATEG_STATUS, CATEG_ID, id);
        //    return Json(new { data = b });
        //}
        //public ActionResult Roles()
        //{
        //    int USER_ID = Convert.ToInt32(Session["USERID"]);
        //    //if (PAGE_PERMISSIONS(USER_ID, "Roles"))
        //    //{
        //    BLL obj = new BLL();
        //    ViewBag.ROLES = obj.GETALL_ROLES();
        //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Error");
        //    //}

        //}
        //public JsonResult _ROLES(string ROLE_NAME)
        //{

        //    BLL obj = new BLL();
        //    bool b = obj.INSERT_ROLES(ROLE_NAME);
        //    return Json(new { data = b });
        //}

        //public JsonResult UPDATE_ROLE_STATUS(int ROLE_STATUS, int ROLE_ID)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);

        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_ROLE_STATUS(ROLE_STATUS, ROLE_ID);
        //    return Json(new { data = b });
        //}
        //public ActionResult Users()
        //{
        //    int USER_ID = Convert.ToInt32(Session["USERID"]);
        //    //if (PAGE_PERMISSIONS(USER_ID, "Users"))
        //    //{
        //    BLL obj = new BLL();
        //    ViewBag.USERS = obj.GETALL_USERS();
        //    ViewBag.categories = obj.GETALL_CATEGORIES();
        //    ViewBag.roles = obj.GETALL_ROLES();

        //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Error");
        //    //}

        //}
        //public JsonResult _USER(int CATEG_ID, int ROLE_ID, string USER_NAME, string USER_EMAILID, string USER_PASSWORD, string USER_MOBILENUMBER, string USER_DEVICE_IDS, int USER_CREATEDBY)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);
        //    BLL obj = new BLL();
        //    bool b = obj.CREATE_USER(CATEG_ID, ROLE_ID, USER_NAME, USER_EMAILID, USER_PASSWORD, USER_MOBILENUMBER, USER_DEVICE_IDS, id);
        //    return Json(new { data = b });
        //}

        //public JsonResult UPDATE_USER_STATUS(int USER_STATUS, int USER_ID)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);

        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_USER_STATUS(USER_STATUS, USER_ID);
        //    return Json(new { data = b });
        //}
        [SessionAuthorize]
        public ActionResult Sims()
        {
            int USER_ID = Convert.ToInt32(Session["USERID"]);
            SMVTS_CATEGORIES _obj_Smvts_categories = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            List<SMVTS_SIMS> obj_sims = new List<SMVTS_SIMS>();
            //get categories


            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                _obj_Smvts_categories.OPERATION = operation.Select;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 4)
            {
                _obj_Smvts_categories.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                //  _obj_Smvts_categories.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
                _obj_Smvts_categories.OPERATION = operation.Select;
                _obj_Smvts_categories.CATEG_STATUS = "1";
            }

            DataTable dt5 = BLL.get_Categories(_obj_Smvts_categories);
            for (int m = 0; m < dt5.Rows.Count; m++)
            {
                obj.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = dt5.Rows[m]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt5.Rows[m]["CATEG_ID"]),
                });
            }


            ViewBag.categories = obj;

            return View();
        }

        public JsonResult getSIMS()
        {
            //get All sims
            SMVTS_SIMS obj_Smvts_Sims = new SMVTS_SIMS();
            List<SMVTS_SIMS> obj_sims = new List<SMVTS_SIMS>();
            DataTable DT = new DataTable();

            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID == 1)
            {
                obj_Smvts_Sims.OPERATION = operation.Select;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 5 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 6)
            {
                obj_Smvts_Sims.SIM_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                obj_Smvts_Sims.OPERATION = operation.SelectSemi;
                obj_Smvts_Sims.SIM_ID = 0;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID == 3) // Only for Client Login
            {
                obj_Smvts_Sims.OPERATION = operation.Select;
                obj_Smvts_Sims.SIM_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            }


            DT = BLL.get_Sims(obj_Smvts_Sims);
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                obj_sims.Add(new SMVTS_SIMS
                {
                    SIM_OPERATORNAME = DT.Rows[i]["SIM_OPERATORNAME"].ToString(),
                    SIM_SERIALNO = DT.Rows[i]["SIM_SERIALNO"].ToString(),
                    SIM_NUMBER = DT.Rows[i]["SIM_NUMBER"].ToString(),
                    SIM_APNIP = DT.Rows[i]["SIM_APNIP"].ToString(),
                    SIM_ID = Convert.ToInt32(DT.Rows[i]["SIM_ID"]),
                    STATUS = DT.Rows[i]["SIM_STATUS"].ToString(),
                    SIM_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID,
                    SIM_CATEGORY_TYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID
                });
            }
            var jsonData = new
            {
                data = obj_sims
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult _SIM(string SIM_NUMBER, string SIM_SERIALNO, string SIM_OPERATORNAME, string SIM_APNWEBSITE, string SIM_APNIP, string CATEG_ID, int SIM_CREATEDBY, bool SIM_STATUS, string categname)
        {
            int id = Convert.ToInt32(Session["USERID"]);
            // string dbname = Session["DBNAME"].ToString();
            string Patnerid = CATEG_ID;

            string dbname = "";
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                dbname = BLL.getDatabase_categ(Patnerid);
            }
            else
            {
                dbname = BLL.getDatabase_deviceupdate(Patnerid);
            }
            SMVTS_SIMS obj_insert = new SMVTS_SIMS();
            obj_insert.OPERATION = operation.Insert;
            obj_insert.SIM_APNIP = SIM_APNIP;
            obj_insert.SIM_APNWEBSITE = SIM_APNWEBSITE;
            obj_insert.SIM_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
            obj_insert.SIM_COUNTRY_ID = 1;
            obj_insert.SIM_NUMBER = SIM_NUMBER;
            obj_insert.SIM_OPERATORNAME = SIM_OPERATORNAME;
            obj_insert.SIM_SERIALNO = SIM_SERIALNO;
            obj_insert.SIM_STATUS = SIM_STATUS;
            obj_insert.CREATEDBY = id;
            obj_insert.CREATEDDATE = DateTime.Now;
            obj_insert.LASTMDFBY = id;
            obj_insert.LASTMDFDATE = DateTime.Now;
            bool b = BLL.set_Sims(obj_insert, dbname, categname, "");
            return Json(new { data = b });
        }

        //public JsonResult UPDATE_SIM_STATUS(int SIM_STATUS, int SIM_ID)
        //{

        //    int id = Convert.ToInt32(Session["USERID"]);

        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_SIM_STATUS(SIM_STATUS, SIM_ID);
        //    return Json(new { data = b });
        //}

        public JsonResult _editsims(int simid)
        {
            SMVTS_SIMS obj_sim = new SMVTS_SIMS();
            List<SMVTS_SIMS> obj = new List<SMVTS_SIMS>();
            obj_sim.OPERATION = operation.Select;
            obj_sim.SIM_ID = simid;
            DataTable dt = BLL.get_Sims(obj_sim);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_SIMS
                    {
                        SIM_ID = Convert.ToInt32(dt.Rows[i]["SIM_ID"]),
                        SIM_OPERATORNAME = dt.Rows[i]["SIM_OPERATORNAME"].ToString(),
                        SIM_APNIP = dt.Rows[i]["SIM_APNIP"].ToString(),
                        SIM_NUMBER = dt.Rows[i]["SIM_NUMBER"].ToString(),
                        SIM_SERIALNO = dt.Rows[i]["SIM_SERIALNO"].ToString(),
                        SIM_APNWEBSITE = dt.Rows[i]["SIM_APNWEBSITE"].ToString(),
                        SIM_CATEGORY_ID = Convert.ToInt32(dt.Rows[i]["SIM_CATEGORY_ID"])
                    });
                }
            }


            return Json(new { data = obj });
        }

        public JsonResult UPDATE_SIM(string SIM_NUMBER, string SIM_SERIALNO, string SIM_OPERATORNAME, string SIM_APNWEBSITE, string SIM_APNIP, string CATEG_ID, int SIM_UPDATEDBY, int SIM_ID, string categname, string OldSIM)
        {
            int id = Convert.ToInt32(Session["USERID"]);
            string dbname = Session["DBNAME"].ToString();
            SMVTS_SIMS obj_insert = new SMVTS_SIMS();

            obj_insert.OPERATION = operation.Update;
            obj_insert.SIM_ID = SIM_ID;
            obj_insert.SIM_APNIP = SIM_APNIP;
            obj_insert.SIM_APNWEBSITE = SIM_APNWEBSITE;
            obj_insert.SIM_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
            obj_insert.SIM_COUNTRY_ID = 1;
            obj_insert.SIM_NUMBER = SIM_NUMBER;
            obj_insert.SIM_OPERATORNAME = SIM_OPERATORNAME;
            obj_insert.SIM_SERIALNO = SIM_SERIALNO;
            obj_insert.SIM_STATUS = false;
            obj_insert.CREATEDBY = id;
            obj_insert.CREATEDDATE = DateTime.Now;
            obj_insert.LASTMDFBY = id;
            obj_insert.LASTMDFDATE = DateTime.Now;
            bool b = BLL.set_Sims(obj_insert, dbname, categname, OldSIM);
            return Json(new { data = b });
        }



        //public ActionResult Device()
        //{
        //    int USER_ID = Convert.ToInt32(Session["USERID"]);
        //    //if (PAGE_PERMISSIONS(USER_ID, "Device"))
        //    //{
        //    BLL obj = new BLL();
        //    ViewBag.sims = obj.GETALL_SIMS();
        //    ViewBag.categories = obj.GETALL_CATEGORIES();
        //    ViewBag.devices = obj.GETALL_DEVICES();
        //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Error");
        //    //}

        //}

        //public JsonResult _DEVICE(string DEVICE_NAME, string DEVICE_SERIALNUMBER, string DEVICE_MFGDATE, string SIM_ID, string CATEG_ID, int DEVICE_CREATEDBY)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);
        //    BLL obj = new BLL();
        //    bool b = obj.CREATE_DEVICE(DEVICE_NAME, DEVICE_SERIALNUMBER, DEVICE_MFGDATE, SIM_ID, CATEG_ID, id);
        //    return Json(new { data = b });
        //}
        //public JsonResult UPDATE_DEVICES_STATUS(int DEVICE_STATUS, int DEVICE_ID)
        //{

        //    int id = Convert.ToInt32(Session["USERID"]);

        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_DEVICES_STATUS(DEVICE_STATUS, DEVICE_ID);
        //    return Json(new { data = b });
        //}


        //public JsonResult _editdevice()
        //{
        //    BLL obj = new BLL();
        //    var data = obj.GETALL_DEVICES();
        //    return Json(new { data = data });
        //}
        //public JsonResult UPDATE_DEVICE(string DEVICE_NAME, string DEVICE_SERIALNUMBER, string DEVICE_MFGDATE, string SIM_ID, string CATEG_ID, int DEVICE_UPDATEDBY, int DEVICE_ID)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);

        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_DEVICE(DEVICE_NAME, DEVICE_SERIALNUMBER, DEVICE_MFGDATE, SIM_ID, CATEG_ID, id, DEVICE_ID);
        //    return Json(new { data = b });
        //}

        //public ActionResult vehicles()
        //{
        //    int USER_ID = Convert.ToInt32(Session["USERID"]);
        //    //if (PAGE_PERMISSIONS(USER_ID, "vehicles"))
        //    //{
        //    BLL obj = new BLL();
        //    ViewBag.devices = obj.GETALL_DEVICES();
        //    ViewBag.categories = obj.GETALL_CATEGORIES();
        //    ViewBag.vehicles = obj.GETALL_VEHILCES();
        //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Error");
        //    //}
        //}
        //public JsonResult _VEHICLES(string VEHICLE_REGNUMBER, int DEVICE_ID, int CATEG_ID, string VEHICLE_ZONE, string VEHICLE_MODELNAME, string VEHICLE_MAXSPEED, string VEHICLE_MILLAGE, int VEHICLE_CREATEDBY)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);
        //    BLL obj = new BLL();
        //    bool b = obj.CREATE_VEHICLES(VEHICLE_REGNUMBER, DEVICE_ID, CATEG_ID, VEHICLE_ZONE, VEHICLE_MODELNAME, VEHICLE_MAXSPEED, VEHICLE_MILLAGE, id);
        //    return Json(new { data = b });
        //}


        //public JsonResult _editvehicle()
        //{
        //    BLL obj = new BLL();
        //    var data = obj.GETALL_VEHILCES();
        //    return Json(new { data = data });
        //}

        //public JsonResult UPDATE_VEHICLES(string VEHICLE_REGNUMBER, int DEVICE_ID, int CATEG_ID, string VEHICLE_ZONE, string VEHICLE_MODELNAME, string VEHICLE_MAXSPEED, string VEHICLE_MILLAGE, int VEHICLE_UPDATEDBY, int VEHICLE_ID)
        //{
        //    int id = Convert.ToInt32(Session["USERID"]);
        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_VEHICLES(VEHICLE_REGNUMBER, DEVICE_ID, CATEG_ID, VEHICLE_ZONE, VEHICLE_MODELNAME, VEHICLE_MAXSPEED, VEHICLE_MILLAGE, id, VEHICLE_ID);
        //    return Json(new { data = b });
        //}



        //public JsonResult UPDATE_VEHICLE_STATUS(int VEHICLE_STATUS, int VEHICLE_ID)
        //{

        //    int id = Convert.ToInt32(Session["USERID"]);

        //    BLL obj = new BLL();
        //    bool b = obj.UPDATE_VEHICLE_STATUS(VEHICLE_STATUS, VEHICLE_ID);
        //    return Json(new { data = b });
        //}

        public ActionResult Pages()
        {

            int USER_ID = Convert.ToInt32(Session["USERID"]);
            //if (PAGE_PERMISSIONS(USER_ID, "Pages"))
            //{
            BLL OBJ = new BLL();
            ViewBag.panels = OBJ.GETALL_PANELS();
            ViewBag.getallpages = OBJ.GETALL_PAGES();
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Error");
            //}
        }
        public JsonResult _PAGES(string PAGE_NAME, int PAGE_CREATEDBY, string PAGE_UNDER)
        {
            int id = Convert.ToInt32(Session["USERID"]);
            BLL obj = new BLL();
            bool b = obj.CREATE_PAGES(PAGE_NAME, id, PAGE_UNDER);
            return Json(new { data = b });
        }
        public JsonResult UPDATE_PAGES_STATUS(int PAGE_STATUS, int PAGE_ID)
        {
            int id = Convert.ToInt32(Session["USERID"]);
            BLL obj = new BLL();
            bool b = obj.UPDATE_PAGES_STATUS(PAGE_STATUS, PAGE_ID);
            return Json(new { data = b });
        }

        public ActionResult Permissions()
        {
            int USER_ID = Convert.ToInt32(Session["USERID"]);
            //   BLL obj = new BLL();
            //   ViewBag.USERS = obj.GETALL_USERS();



            SMVTS_CATEGORIES _obj_Smvts_Devices = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            //  if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID == 2)
            // {

            _obj_Smvts_Devices.CATEG_CATETYPE_ID = 3;
            _obj_Smvts_Devices.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt = BLL.get_Categories(_obj_Smvts_Devices);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                obj.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = Convert.ToString(dt.Rows[i]["CATEG_NAME"]),
                    CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                });
            }
            ViewBag.USERS_PERMISSIONS = obj;

            // }
            return View();
        }
        public JsonResult _Permissions()
        {
            BLL OBJ = new BLL();
            var data = OBJ.GETALL_PAGES();

            return Json(new { data = data });
        }
        public JsonResult _Permissionssave(int categid, string name, string view, string edit, string create)
        {
            BLL obj = new BLL();
            obj.CREATE_PERMISSIONS(categid, name, view, edit, create);
            bool b = true;
            return Json(new { data = b });
        }

        public JsonResult _Permissionsupdate(string name, string view, string edit, string create, int categid)
        {
            BLL obj = new BLL();
            obj.UPDATE_PERMISSIONS(name, view, edit, create, categid);
            bool b = true;
            return Json(new { data = b });
        }

        public JsonResult _CHECK_PERMISSIONS(int CATEG_ID)
        {
            BLL OBJ = new BLL();
            var data = OBJ.GETALL_PAGE_PERMISSIONS(CATEG_ID);

            return Json(new { data = data });
        }
        public ActionResult Panel()
        {
            int USER_ID = Convert.ToInt32(Session["USERID"]);
            //if (PAGE_PERMISSIONS(USER_ID, "Panel"))
            //{
            BLL OBJ = new BLL();
            ViewBag.panels = OBJ.GETALL_PANELS();
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Error");
            //}
        }

        public JsonResult _panel()
        {
            BLL OBJ = new BLL();
            var data = OBJ.GETALL_PANELS();
            return Json(new { data = data });
        }



        public JsonResult CREATE_PANEL(string PANEL_NAME)
        {
            int USER_ID = Convert.ToInt32(Session["USERID"]);
            BLL obj = new BLL();
            obj.CREATE_PANEL(PANEL_NAME, USER_ID);
            bool b = true;
            return Json(new { data = b });
        }

        public JsonResult UPDATE_PANEL_STATUS(string PANEL_STATUS, int PANEL_ID)
        {
            int USER_ID = Convert.ToInt32(Session["USERID"]);
            BLL obj = new BLL();
            obj.UPDATE_PANEL_STATUS(PANEL_STATUS, USER_ID, PANEL_ID);
            bool b = true;
            return Json(new { data = b });
        }

        public JsonResult _EDIT_PAGES()
        {
            BLL obj = new BLL();
            var data = obj.GETALL_PAGES();
            return Json(new { data = data });
        }

        public JsonResult PAGE_UPDATE(string PAGE_NAME, string PAGE_UNDER, int PAGE_ID)
        {
            int USER_ID = Convert.ToInt32(Session["USERID"]);
            BLL obj = new BLL();
            obj.PAGE_UPDATE(PAGE_NAME, PAGE_UNDER, USER_ID, PAGE_ID);
            bool b = true;
            return Json(new { data = b });
        }


        //public JsonResult UPDATE_USERPROFILE(HttpPostedFileBase MyImages,string USERNAME,string PASSWORD)
        //{
        //    string ipath = "";
        //    if(MyImages !=null)
        //    {
        //        string _FileName = Path.GetFileName(MyImages.FileName);
        //        Random r = new Random();
        //        int number = r.Next(100, 10000000);
        //        string _path = Path.Combine(Server.MapPath("~/IMAGES/users"), number + _FileName);
        //        MyImages.SaveAs(_path);
        //        ipath = number + _FileName;
        //    }


        //    BLL obj=new BLL();
        //    int USER_ID = Convert.ToInt32(Session["USERID"]);
        //    obj.USER_UPDATE(USERNAME, PASSWORD, ipath, USER_ID, USER_ID);

        //    bool b = true;
        //    return Json(new { data = b });
        //}

        //public JsonResult editusers()
        //{
        //    BLL obj = new BLL();
        //   var data = obj.GETALL_USERS();
        //   return Json(new { data = data });
        //}


        public ActionResult Error()
        {
            return View();
        }


        public ActionResult lay()
        {
            return View();
        }
        public ActionResult lay1()
        {
            return View();
        }
        //Created by Ajith
        [SessionAuthorize]
        public ActionResult SMS_Configuration()
        {

            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            try
            {
                string parClientID = "";
                string ClientId = null;
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    parClientID = ClientId;
                }
                else
                {
                    parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                }

                //  string parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);

                _obj_Smvts_Vehicles.OPERATION = operation.Empty;
                _obj_Smvts_Vehicles.CREATEDBY = 0;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(parClientID);


                DataTable dt;
                if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "4")
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = -1;
                }
                else
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                }

                dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_VEHICLES
                    {
                        VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_DEVICE_ID"]),
                    });
                }
                ViewBag.Vehicles = obj;
            }

            catch
            {

            }



            return View();
        }

        public JsonResult Save_SIMConfiguration(List<AlertTypes> Alertdata)
        {

            //  SMVTS_FLEETCONFIG _objConfig = new SMVTS_FLEETCONFIG();
            SMVTS_ALERTS_CONFIG _objConfig = new SMVTS_ALERTS_CONFIG();



            int categ_id = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            int userid = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            string categName = ((SMVTS_USERS)Session["USERINFO"]).CATEG_NAME;
            string abc = "SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + categName + "' ";

            DataTable dt_categ = new DataTable();
            dt_categ = Dal.ExecuteConfig(abc);
            int categId = 0;
            if (dt_categ.Rows.Count > 0)
            {
                categId = Convert.ToInt32(dt_categ.Rows[0][0]);
            }
            int count = 0; int check_Comb = 0;
            string status = "";
            string repeatStatus = "";
            foreach (var data in Alertdata)
            {

                _objConfig.CONFIG_CATEGID = categId;
                _objConfig.CONFIG_USERID = userid;
                _objConfig.CONFIG_STATUS = 1;
                _objConfig.CONFIG_TYPE = data.AlertType.ToString();
                _objConfig.CONFIG_OPTIONAL1 = ((SMVTS_USERS)Session["USERINFO"]).Mobilenumber;

                DataTable dt = BLL.checkAlertTypeExistence(_objConfig);




                if (dt.Rows.Count > 0)
                {
                    check_Comb++;
                    string message = "This Alert Type " + data.AlertType.ToString() + " already exists!,Try again..";
                    //BLL.ShowMessage(this, "This combination " + vehRegNum.ToString() + " and " + data.AlertType.ToString() + " already exists!,Try again..");
                    repeatStatus = repeatStatus + "</br>" + message;
                    break;
                }
                else
                {
                    BLL.set_SMS_Configuration(_objConfig);
                    count++;
                }

            }
            //if(count>0 && check_Comb>0)
            // {
            //    status = repeatStatus;
            // }
            //else if(count>0 && check_Comb==0)
            // {
            //     status = "true";
            // }
            //else if(check_Comb > 0)
            // {
            //     status = repeatStatus;
            // }

            if (count > 0)
            {
                status = "true";
            }
            else
            {
                status = repeatStatus;
            }
            return Json(new { data = status });
        }

        public JsonResult GetAll_Sim_configurationData()
        {
            ////SMVTS_FLEETCONFIG _objConfig = new SMVTS_FLEETCONFIG();
            ////List<SMVTS_FLEETCONFIG> obj = new List<SMVTS_FLEETCONFIG>();
            ////_objConfig.CATEGORYID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            ////_objConfig.OPERATION = operation.Select;
            ////_objConfig.ID = 0;

            ////DataTable DT = BLL.get_FleetSmsconfig(_objConfig);

            ////for (int i = 0; i < DT.Rows.Count; i++)
            ////{
            ////    obj.Add(new SMVTS_FLEETCONFIG
            ////    {

            ////        REGNUMBER = (DT.Rows[i]["SMS_REGNUMBER"]).ToString(),
            ////        ALERT_TYPE = Convert.ToString(DT.Rows[i]["SMS_ALERT"]),
            ////        SIM = Convert.ToString(DT.Rows[i]["SMS_SIM"]),

            ////    });
            ////}

            List<SMVTS_ALERTS_CONFIG> obj_alerts = new List<SMVTS_ALERTS_CONFIG>();
            DataTable dt_alerts = new DataTable();
            int userid = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            dt_alerts = BLL.GetAlertConfig(userid);

            if (dt_alerts.Rows.Count > 0)
            {
                for (int i = 0; i < dt_alerts.Rows.Count; i++)
                {
                    obj_alerts.Add(new SMVTS_ALERTS_CONFIG
                    {

                        CONFIG_TYPE = (dt_alerts.Rows[i]["CONFIG_TYPE"]).ToString(),
                        CONFIG_OPTIONAL1 = Convert.ToString(dt_alerts.Rows[i]["CONFIG_OPTIONAL1"]),
                        CONFIG_ID = Convert.ToInt32(dt_alerts.Rows[i]["CONFIG_ID"])

                    });
                }
            }



            var jsonData = new
            {
                data = obj_alerts
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [SessionAuthorize]
        public ActionResult Route_Planning()
        {
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            try
            {
                string parClientID = "";
                string ClientId = null;
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    parClientID = ClientId;
                }
                else
                {
                    parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                }

                //  string parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);

                _obj_Smvts_Vehicles.OPERATION = operation.Empty;
                _obj_Smvts_Vehicles.CREATEDBY = 0;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(parClientID);


                DataTable dt;
                if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "4")
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = -1;
                }
                else
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                }

                dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_VEHICLES
                    {
                        VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_DEVICE_ID"]),
                    });
                }
                ViewBag.Vehicles = obj;
            }

            catch
            {

            }

            return View();
        }

        public JsonResult Save_RoutePlanning(string VehilceNo, string RouteName, string StartDate, string EndDate, string Status, string Weight, string TripNo, string CompletedDate, string NumDays)
        {
            DataTable dt = new DataTable();
            string status = null;

            SMVTS_ROUTEPLAN _obj_Smvts_RoutePlan = new SMVTS_ROUTEPLAN();

            _obj_Smvts_RoutePlan.VEHROUTE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_RoutePlan.VEHROUTE_DEVICE_ID = Convert.ToInt32(VehilceNo);
            _obj_Smvts_RoutePlan.VEHROUTE_ROUTE_ID = Convert.ToInt32(RouteName);
            _obj_Smvts_RoutePlan.VEHROUTE_STARTDATE = Convert.ToDateTime(StartDate);
            _obj_Smvts_RoutePlan.VEHROUTE_ENDDATE = Convert.ToDateTime(EndDate);
            _obj_Smvts_RoutePlan.VEHROUTE_STATUS = Convert.ToInt32(Status);
            _obj_Smvts_RoutePlan.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_RoutePlan.CREATEDDATE = DateTime.Now;
            _obj_Smvts_RoutePlan.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_RoutePlan.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_RoutePlan.VEHROUTE_LOAD = Convert.ToDecimal(Weight);
            _obj_Smvts_RoutePlan.VEHROUTE_TRIPNO = Convert.ToString(TripNo);
            _obj_Smvts_RoutePlan.VEHROUTE_TOTAL_ALLOWED_DAYS = Convert.ToInt32(NumDays);
            //_obj_Smvts_RoutePlan.VEHROUTE_LAST_TRIP_COMPLETION_DATE = Convert.ToString(CompletedDate);

            if (Status == "2")
            {

                _obj_Smvts_RoutePlan.VEHROUTE_COMPLETEDDATE = Convert.ToDateTime(CompletedDate);
            }

            _obj_Smvts_RoutePlan.OPERATION = operation.Insert;
            bool route_status = BLL.set_RoutePlan(_obj_Smvts_RoutePlan);

            return Json(new { data = status });
        }

        public JsonResult Get_RoutePlanningData()
        {
            List<SMVTS_ROUTEPLAN> obj = new List<SMVTS_ROUTEPLAN>();
            SMVTS_ROUTEPLAN _obj_Smvts_RoutePlan = new SMVTS_ROUTEPLAN();
            _obj_Smvts_RoutePlan.OPERATION = operation.Select;
            _obj_Smvts_RoutePlan.VEHROUTE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable DT = BLL.get_RoutePlan(_obj_Smvts_RoutePlan);
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_ROUTEPLAN
                    {
                        VEHICLES_REGNUMBER = DT.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                        ROUTE_NAME = DT.Rows[i]["ROUTES_NAME"].ToString(),
                        STARTDATE = Convert.ToDateTime(DT.Rows[i]["VEHROUTE_STARTDATE"]),
                        ENDDATE = Convert.ToDateTime(DT.Rows[i]["VEHROUTE_ENDDATE"])
                    });
                }
            }


            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [SessionAuthorize]
        public ActionResult Routes1()
        {

            SMVTS_ROUTES _obj_Smvts_Routes = new SMVTS_ROUTES();
            _obj_Smvts_Routes.OPERATION = operation.Select;
            _obj_Smvts_Routes.ROUTES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable DT = BLL.get_Routes(_obj_Smvts_Routes);
            List<SMVTS_ROUTES> obj = new List<SMVTS_ROUTES>();
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_ROUTES
                    {
                        ROUTES_NAME = DT.Rows[i]["ROUTES_NAME"].ToString(),
                        ROUTES_STARTLAT = DT.Rows[i]["ROUTES_STARTLAT"].ToString(),
                        ROUTES_STARTLONG = DT.Rows[i]["ROUTES_STARTLONG"].ToString(),
                        ROUTES_ENDLAT = DT.Rows[i]["ROUTES_ENDLAT"].ToString(),
                        ROUTES_ENDLONG = DT.Rows[i]["ROUTES_ENDLONG"].ToString(),
                        STATUS = DT.Rows[i]["ROUTES_STATUS"].ToString()
                    });
                }

            }
            ViewBag.routes = obj;
            return View();
        }

        public JsonResult SaveRoutes(SMVTS_ROUTES obj)
        {
            SMVTS_ROUTES _obj_Smvts_Routes = new SMVTS_ROUTES();
            DataTable DT = new DataTable();
            int count = 0;
            int count1 = 0;
            // validation to check Repeat Name
            _obj_Smvts_Routes = new SMVTS_ROUTES();
            _obj_Smvts_Routes.ROUTES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_Routes.ROUTES_NAME = BLL.ReplaceQuote(obj.ROUTES_NAME).ToString();
            _obj_Smvts_Routes.OPERATION = operation.Update;
            //if (lbl_RoutesID.Text != "")
            //{
            //    _obj_Smvts_Routes.ROUTES_ID = Convert.ToInt32(lbl_RoutesID.Text);
            //}
            DT = BLL.get_Routes(_obj_Smvts_Routes);
            if (DT.Rows.Count > 0)
            {
                count1 = Convert.ToInt32(DT.Rows[0][0]);
            }

            // validation to check Repeat lat and Logs
            _obj_Smvts_Routes = new SMVTS_ROUTES();
            _obj_Smvts_Routes.ROUTES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_Routes.ROUTES_STARTLAT = BLL.ReplaceQuote(obj.ROUTES_STARTLAT);
            _obj_Smvts_Routes.ROUTES_STARTLONG = BLL.ReplaceQuote(obj.ROUTES_STARTLONG.ToString());
            _obj_Smvts_Routes.ROUTES_ENDLAT = BLL.ReplaceQuote(obj.ROUTES_ENDLAT.ToString());
            _obj_Smvts_Routes.ROUTES_ENDLONG = BLL.ReplaceQuote(obj.ROUTES_ENDLONG.ToString());

            _obj_Smvts_Routes.OPERATION = operation.Check;
            //if (lbl_RoutesID.Text != "")
            //{
            //    _obj_Smvts_Routes.ROUTES_ID = Convert.ToInt32(lbl_RoutesID.Text);
            //}
            /* DT = BLL.get_Routes(_obj_Smvts_Routes);
             if (DT.Rows.Count > 0)
             {
                 count = Convert.ToInt32(DT.Rows[0][0]);
             }*/

            // Saving details in to SMVTS_ROUTES table.

            _obj_Smvts_Routes = new SMVTS_ROUTES();
            _obj_Smvts_Routes.ROUTES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            _obj_Smvts_Routes.ROUTES_NAME = BLL.ReplaceQuote(obj.ROUTES_NAME.ToString());
            _obj_Smvts_Routes.ROUTES_STARTLAT = BLL.ReplaceQuote(obj.ROUTES_STARTLAT.ToString());
            _obj_Smvts_Routes.ROUTES_STARTLONG = BLL.ReplaceQuote(obj.ROUTES_STARTLONG.ToString());
            _obj_Smvts_Routes.ROUTES_ENDLAT = BLL.ReplaceQuote(obj.ROUTES_ENDLAT.ToString());
            _obj_Smvts_Routes.ROUTES_ENDLONG = BLL.ReplaceQuote(obj.ROUTES_ENDLONG.ToString());
            //_obj_Smvts_Routes.ROUTES_POINTS = BLL.ReplaceQuote("");
            _obj_Smvts_Routes.ROUTES_POINTS = BLL.ReplaceQuote(obj.ROUTES_POINTS.ToString());
            _obj_Smvts_Routes.ROUTES_STATUS = Convert.ToBoolean(obj.ROUTES_STATUS);
            _obj_Smvts_Routes.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Routes.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Routes.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Routes.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_Routes.ROUTES_FROM = BLL.ReplaceQuote(obj.ROUTES_FROM.ToString());
            _obj_Smvts_Routes.ROUTES_TO = BLL.ReplaceQuote(obj.ROUTES_TO.ToString());
            _obj_Smvts_Routes.ROUTES_VIA = BLL.ReplaceQuote(obj.ROUTES_VIA.ToString());
            _obj_Smvts_Routes.ROUTES_DEVIATION_LIMIT = BLL.ReplaceQuote(obj.ROUTES_DEVIATION_LIMIT.ToString());
            _obj_Smvts_Routes.ROUTES_DISTANCE = BLL.ReplaceQuote(obj.ROUTES_DISTANCE.ToString());

            string status = "";
            if (count1 == 0)
            {
                if (count == 0)
                {
                    if (Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID) == 2)
                    {
                        int j = 0;
                        _obj_Smvts_Routes.OPERATION = operation.Insert;
                        BLL.set_Routes(_obj_Smvts_Routes);
                        DataTable dt = Dal.ExecuteQuery("(select USERS_CATEGORY_ID from smvts_users where users_category_id in (select categ_id from smvts_categories where categ_parent_id in (select users_category_id from smvts_users where users_id=" + ((SMVTS_USERS)Session["USERINFO"]).USERS_ID + ")))");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                _obj_Smvts_Routes.ROUTES_CATEGORY_ID = Convert.ToInt32(dt.Rows[i]["USERS_CATEGORY_ID"]);
                                if (BLL.set_Routes(_obj_Smvts_Routes))
                                    j += 1;
                                else
                                {

                                }
                                //BLL.ShowMessage(this, BLL.msg_UnSaved);
                            }
                            if (dt.Rows.Count == j)
                            {
                                status = "true";
                            }
                        }

                    }
                    else
                    {
                        _obj_Smvts_Routes.OPERATION = operation.Insert;
                        if (BLL.set_Routes(_obj_Smvts_Routes))
                            status = "true";
                        else
                            status = "false";

                    }

                }
                else
                {
                    status = "Latitude and Longitude already Exist";

                }
            }
            else
            {
                status = "Route Name already Exist";

            }

            return Json(new { data = status });
        }

        [SessionAuthorize]
        public ActionResult Landmarks()
        {
            SMVTS_LOCATIONTYPE obj_loctype = new SMVTS_LOCATIONTYPE();
            obj_loctype.OPERATION = operation.Select;
            DataTable dt = new DataTable();
            //Get Location Type
            dt = BLL.get_LocationType(obj_loctype);
            List<SMVTS_LOCATIONTYPE> obj = new List<SMVTS_LOCATIONTYPE>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_LOCATIONTYPE
                    {
                        LOCTYPE_NAME = dt.Rows[i]["LOCTYPE_NAME"].ToString(),
                        LOCTYPE_ID = Convert.ToInt32(dt.Rows[i]["LOCTYPE_ID"])
                    });
                }

            }
            ViewBag.locationTypes = obj;

            return View();
        }
        public JsonResult SaveLandmarks(SMVTS_LANDMARKS data, string flag, string btype)
        {
            string status = "";
            if (flag == "0")
            {
                DataTable DT = new DataTable();
                int count = 0;

                if (data.LANDMARKS_POLYPOINTS != null && data.LANDMARKS_POLYPOINTS != "")
                {
                    data.LANDMARKS_POLYPOINTS = data.LANDMARKS_POLYPOINTS.Substring(0, data.LANDMARKS_POLYPOINTS.ToString().Length - 1);
                }
                SMVTS_LANDMARKS _obj_Smvts_LandMarks = new SMVTS_LANDMARKS();
                _obj_Smvts_LandMarks.LANDMARKS_LATITUDE = BLL.ReplaceQuote(data.LANDMARKS_LATITUDE.ToString().Trim());
                _obj_Smvts_LandMarks.LANDMARKS_LONGITUDE = BLL.ReplaceQuote(data.LANDMARKS_LONGITUDE.ToString().Trim());
                _obj_Smvts_LandMarks.OPERATION = operation.Update;
                _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_LandMarks.LANDMARKS_GEOFENCETYPE = Convert.ToInt32(data.LANDMARKS_GEOFENCETYPE);
                _obj_Smvts_LandMarks.LANDMARKS_POLYPOINTS = data.LANDMARKS_POLYPOINTS.ToString();
                _obj_Smvts_LandMarks.LANDMARKS_TYPE = 2;
                _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_LandMarks.LANDMARKS_ADDRESS = BLL.ReplaceQuote(data.LANDMARKS_ADDRESS.ToString().Trim());

                DT = BLL.get_LandMarks(_obj_Smvts_LandMarks);

                if (DT.Rows.Count > 0)
                {

                    count = Convert.ToInt32(DT.Rows[0][0]);

                }

                _obj_Smvts_LandMarks.LANDMARKS_LOCATIONTYPE = Convert.ToInt32(data.LANDMARKS_LOCATIONTYPE);
                _obj_Smvts_LandMarks.LANDMARKS_CONTPERSONS = null;
                _obj_Smvts_LandMarks.LANDMARKS_MOBILENUMBER = null;
                _obj_Smvts_LandMarks.LANDMARKS_TYPE = 2;

                _obj_Smvts_LandMarks.LANDMARKS_STATUS = Convert.ToBoolean(data.LANDMARKS_STATUS);
                _obj_Smvts_LandMarks.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_LandMarks.CREATEDDATE = DateTime.Now;
                _obj_Smvts_LandMarks.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_LandMarks.LASTMDFDATE = DateTime.Now;
                _obj_Smvts_LandMarks.LANDMARKS_GEOSTATUS = data.LANDMARKS_GEOSTATUS;
                _obj_Smvts_LandMarks.RADIUS = data.RADIUS;

                _obj_Smvts_LandMarks.LANDMARKS_STATE = data.LANDMARKS_STATE.ToString();
                _obj_Smvts_LandMarks.LANDMARKS_ZONE = data.LANDMARKS_ZONE.ToString();
                _obj_Smvts_LandMarks.LANDMARKS_NEARCITY = data.LANDMARKS_NEARCITY.ToString();
                _obj_Smvts_LandMarks.LANDMARK_GEOFENCE_ID = data.LANDMARK_GEOFENCE_ID;

                if (btype == "BTN_UPDATE")
                {
                    _obj_Smvts_LandMarks.OPERATION = operation.Update;
                    _obj_Smvts_LandMarks.LANDMARKS_ID = Convert.ToInt32(data.LANDMARKS_ID);
                    if (BLL.set_LandMarks(_obj_Smvts_LandMarks))
                        status = "true";
                    else
                        status = "false";

                }


                else
                {
                    if (count == 0)
                    {


                        if (Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID) == 2)
                        {
                            int j = 0;
                            _obj_Smvts_LandMarks.OPERATION = operation.Insert;
                            BLL.set_LandMarks(_obj_Smvts_LandMarks);
                            DataTable dt = Dal.ExecuteQuery("(select USERS_CATEGORY_ID from smvts_users where users_category_id in (select categ_id from smvts_categories where categ_parent_id in (select users_category_id from smvts_users where users_id=" + ((SMVTS_USERS)Session["USERINFO"]).USERS_ID + ")))");
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = Convert.ToInt32(dt.Rows[i]["USERS_CATEGORY_ID"]);
                                    if (BLL.set_LandMarks(_obj_Smvts_LandMarks))
                                        j += 1;


                                    else
                                        status = "false";
                                }
                                if (dt.Rows.Count == j)
                                {
                                    status = "true";
                                }

                            }

                        }
                        else
                        {
                            _obj_Smvts_LandMarks.OPERATION = operation.Insert;
                            if (BLL.set_LandMarks(_obj_Smvts_LandMarks))
                                status = "true";
                            else
                                status = "false";

                        }
                    }
                    else
                    {
                        status = "Latitude and Longitude already Exist";

                    }
                }

            }
            else
            {
                status = "LandMark Cannot be Outside the geofence";

            }
            return Json(new { data = status });
        }

        public JsonResult Edit_Landmarks(SMVTS_LANDMARKS id)
        {

            List<SMVTS_LANDMARKS> obj = new List<SMVTS_LANDMARKS>();
            SMVTS_LANDMARKS _obj_Smvts_LandMarks = new SMVTS_LANDMARKS();

            List<SMVTS_LOCATIONTYPE> obj1 = new List<SMVTS_LOCATIONTYPE>();
            SMVTS_LOCATIONTYPE _obj_Smvts_LocationType = new SMVTS_LOCATIONTYPE();





            if (id.LANDMARKS_ID.ToString() != "0")
            {
                DataTable dt = new DataTable();
                _obj_Smvts_LandMarks.LANDMARKS_ID = id.LANDMARKS_ID;
                _obj_Smvts_LandMarks.OPERATION = operation.Select;
                dt = BLL.get_LandMarks(_obj_Smvts_LandMarks);
                _obj_Smvts_LandMarks.LANDMARKS_LOCATIONTYPE = Convert.ToInt32(dt.Rows[0]["LANDMARKS_LOCATIONTYPE"]);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_LANDMARKS
                    {
                        LANDMARKS_ID = Convert.ToInt32(dt.Rows[i]["LANDMARKS_ID"]),
                        LANDMARKS_LATITUDE = dt.Rows[i]["LANDMARKS_LATITUDE"].ToString(),
                        LANDMARKS_LONGITUDE = dt.Rows[i]["LANDMARKS_LONGITUDE"].ToString(),
                        LANDMARKS_ADDRESS = dt.Rows[i]["LANDMARKS_ADDRESS"].ToString(),
                        LANDMARKS_LOCATIONTYPE = Convert.ToInt32(dt.Rows[i]["LANDMARKS_LOCATIONTYPE"]),
                        LANDMARKS_STATUS = Convert.ToBoolean(dt.Rows[i]["LANDMARKS_STATUS"]),
                        LANDMARKS_GEOSTATUS = Convert.ToBoolean(dt.Rows[i]["LANDMARKS_GEOSTATUS"]),
                        LANDMARKS_STATE = dt.Rows[i]["LANDMARKS_STATE"].ToString(),
                        LANDMARKS_GEOFENCETYPE = Convert.ToInt32(dt.Rows[i]["LANDMARKS_GEOFENCETYPE"]),
                        RADIUS = Convert.ToInt32(dt.Rows[i]["RADIUS"]),
                    });
                }


            }


            if (_obj_Smvts_LandMarks.LANDMARKS_LOCATIONTYPE.ToString() != "0")
            {
                DataTable dt1 = new DataTable();
                _obj_Smvts_LocationType.LOCTYPE_ID = Convert.ToInt32(_obj_Smvts_LandMarks.LANDMARKS_LOCATIONTYPE);
                _obj_Smvts_LocationType.OPERATION = operation.Select;
                dt1 = BLL.get_LocationType(_obj_Smvts_LocationType);
                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    obj1.Add(new SMVTS_LOCATIONTYPE
                    {
                        LOCTYPE_ID = Convert.ToInt32(dt1.Rows[k]["LOCTYPE_ID"]),
                        LOCTYPE_NAME = dt1.Rows[k]["LOCTYPE_NAME"].ToString(),
                    });
                }
            }
            SMVTS_LOCATIONTYPE obj_loctype = new SMVTS_LOCATIONTYPE();
            obj_loctype.OPERATION = operation.Select;
            DataTable dt2 = new DataTable();
            //Get Location Type
            dt2 = BLL.get_LocationType(obj_loctype);
            List<SMVTS_LOCATIONTYPE> obj2 = new List<SMVTS_LOCATIONTYPE>();
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    obj2.Add(new SMVTS_LOCATIONTYPE
                    {
                        LOCTYPE_NAME = dt2.Rows[i]["LOCTYPE_NAME"].ToString(),
                        LOCTYPE_ID = Convert.ToInt32(dt2.Rows[i]["LOCTYPE_ID"])
                    });
                }

            }




            return Json(new { data = obj, data1 = obj1, data2 = obj2 });

        }




        public JsonResult GetAllLandmarks()
        {
            //Get All Data


            SMVTS_LANDMARKS _obj_Smvts_LandMarks = new SMVTS_LANDMARKS();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                _obj_Smvts_LandMarks.OPERATION = operation.Select;
                _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_LandMarks.LANDMARKS_TYPE = 2;
            }
            else
            {
                _obj_Smvts_LandMarks.OPERATION = operation.Empty;
                _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            return GetAllLandmarks_CategId(_obj_Smvts_LandMarks);
        }

        public List<SMVTS_LANDMARKS> getLandmarksDataObj(SMVTS_LANDMARKS _obj_Smvts_LandMarks)
        {
            DataTable DT = BLL.get_LandMarks(_obj_Smvts_LandMarks);
            List<SMVTS_LANDMARKS> list_obj = new List<SMVTS_LANDMARKS>();
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    list_obj.Add(new SMVTS_LANDMARKS


                    {
                        LANDMARKS_ID = Convert.ToInt32(DT.Rows[i]["LANDMARKS_ID"]),
                        LANDMARKS_LATITUDE = DT.Rows[i]["LANDMARKS_LATITUDE"].ToString(),
                        LANDMARKS_LONGITUDE = DT.Rows[i]["LANDMARKS_LONGITUDE"].ToString(),
                        LANDMARKS_ADDRESS = DT.Rows[i]["LANDMARKS_ADDRESS"].ToString(),
                        LOCATIONTYPE_NAME = DT.Rows[i]["LOCTYPE_NAME"].ToString(),
                        LOCATION_STATUS = DT.Rows[i]["LANDMARKS_STATUS"].ToString(),
                        GEOFENCE_STATUS = DT.Rows[i]["LANDMARKS_GEOSTATUS"].ToString()
                    });
                }

            }
            return list_obj;
        }
        public List<SMVTS_LANDMARKS> getLandmarksDataObj1(SMVTS_LANDMARKS _obj_Smvts_LandMarks)
        {
            DataTable DT = BLL.get_LandMarks1(_obj_Smvts_LandMarks);
            List<SMVTS_LANDMARKS> list_obj = new List<SMVTS_LANDMARKS>();
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    list_obj.Add(new SMVTS_LANDMARKS


                    {
                        LANDMARKS_ID = Convert.ToInt32(DT.Rows[i]["LANDMARKS_ID"]),
                        LANDMARKS_LATITUDE = DT.Rows[i]["LANDMARKS_LATITUDE"].ToString(),
                        LANDMARKS_LONGITUDE = DT.Rows[i]["LANDMARKS_LONGITUDE"].ToString(),
                        LANDMARKS_ADDRESS = DT.Rows[i]["LANDMARKS_ADDRESS"].ToString(),
                        LOCATIONTYPE_NAME = DT.Rows[i]["LOCTYPE_NAME"].ToString(),
                        LOCATION_STATUS = DT.Rows[i]["LANDMARKS_STATUS"].ToString(),
                        GEOFENCE_STATUS = DT.Rows[i]["LANDMARKS_GEOSTATUS"].ToString()
                    });
                }

            }
            return list_obj;
        }
        public JsonResult GetAllLandmarks_CategId(SMVTS_LANDMARKS _obj_Smvts_LandMarks)
        {

            List<SMVTS_LANDMARKS> list_obj = getLandmarksDataObj(_obj_Smvts_LandMarks);
            var jsonData = new
            {
                data = list_obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [SessionAuthorize]
        public ActionResult RoutePlan()
        {

            return View();
        }
        [HttpPost]
        public JsonResult UploadExcel()
        {
            string status = "";
            if (Request.Files.Count > 0)
            {

                try
                {

                    System.Data.DataTable dt_excel = new System.Data.DataTable();

                    DataColumn col_vno = new DataColumn("VEHICLES_REGNUMBER", typeof(string));
                    DataColumn col_start = new DataColumn("STARTDATE", typeof(string));
                    DataColumn col_end = new DataColumn("ENDATE", typeof(string));
                    DataColumn col_from = new DataColumn("From", typeof(string));
                    DataColumn col_to = new DataColumn("To", typeof(string));
                    DataColumn col_via = new DataColumn("Via", typeof(string));
                    DataColumn col_dist = new DataColumn("Distance", typeof(string));
                    DataColumn col_remark = new DataColumn("Remark", typeof(string));

                    dt_excel.Columns.Add(col_vno);
                    dt_excel.Columns.Add(col_start);
                    dt_excel.Columns.Add(col_end);
                    dt_excel.Columns.Add(col_from);
                    dt_excel.Columns.Add(col_to);
                    dt_excel.Columns.Add(col_via);
                    dt_excel.Columns.Add(col_dist);
                    dt_excel.Columns.Add(col_remark);
                    // rgrd_ExcelUpload.DataSource = dt_excel;
                    // rgrd_ExcelUpload.DataBind();
                    Session["DTEXCEL"] = dt_excel;
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int j = 0; j < files.Count; j++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[j];
                        string extension = Path.GetExtension(file.FileName);
                        //  string excelPath = Path.GetFileName(excelupload.PostedFile.FileName).Replace(" ", String.Empty);
                        //  excelupload.SaveAs(Server.MapPath("~/Master/ExcelUploads/") + excelPath);
                        string filename = Path.GetFileName(file.FileName);

                        string fileTemp = ((DateTime.Now.ToString()) + "_" + filename).Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", "");
                        string savefile = Server.MapPath(@"~/Master/ExcelUploads/" + fileTemp);
                        file.SaveAs(savefile);
                        // string excelPath = Server.MapPath("~/ExcelUploads/") + Path.GetFileName(excelupload.PostedFile.FileName);
                        // excelupload.SaveAs(excelPath);
                        OleDbConnection conString = null;
                        if (extension == ".xls" || extension == ".xlsx")
                        {
                            switch (extension)
                            {
                                case ".xls": //Excel 97-03
                                             //conString = ConfigurationManager.ConnectionStrings["Exel03ConString"].ConnectionString;
                                             //conString = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=0\"");
                                    conString = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=0\"");
                                    break;
                                case ".xlsx": //Excel 07 or higher
                                              //conString = ConfigurationManager.ConnectionStrings["Exel07plusConString"].ConnectionString;
                                              // conString = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"");
                                    conString = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"");
                                    break;

                            }
                            //conString = string.Format(conString, excelPath);
                            //using (OleDbConnection excel_con = new OleDbConnection(conString))
                            //{
                            //excel_con.Open();
                            //string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["SMVTS_ROUTE_BUDGET "].ToString();
                            System.Data.DataTable dtExcelData = new System.Data.DataTable();


                            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [Current Status$]", conString))
                            {
                                oda.Fill(dtExcelData);
                            }
                            //excel_con.Close();
                            if (dtExcelData.Rows.Count > 0)
                            {
                                System.Data.DataTable dt_data = new System.Data.DataTable();
                                string conn = ((SMVTS_USERS)Session["USERINFO"]).USERS_DBNAME;
                                bool result = false;
                                //int count;
                                //count = BLL.insertexcel(dt);
                                //count = BLL.insertexcel(dtExcelData);
                                for (int i = 3; i < dtExcelData.Rows.Count; i++)
                                {
                                    //pass all parameters to above procedures
                                    try
                                    {
                                        string querry = "EXEC USP_ER_EXCELUPLOADEXP @ER_CATEGID=" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + ",@OPERATION='EXCELUPLOAD',@ER_VEHICLENO='" + dtExcelData.Rows[i][1] + "',@ER_VEHICLETYPE='" + dtExcelData.Rows[i][2] + "',@ER_BOOKBRANCH='" + dtExcelData.Rows[i][3] + "',@ER_BOOKZONE='" + dtExcelData.Rows[i][4] + "',@ER_DISPATCHDATE='" + dtExcelData.Rows[i][5] + "',@ER_FROM='" + dtExcelData.Rows[i][6] + "',@ER_TO='" + dtExcelData.Rows[i][7] + "',@ER_PARTYNAME='" + dtExcelData.Rows[i][8] + "',@ER_DELIVERYBRANCH='" + dtExcelData.Rows[i][10] + "',@ER_TO_ZONE='" + dtExcelData.Rows[i][11] + "',@ER_EXPECTED_DATE='" + dtExcelData.Rows[i][11] + "',@ER_REPORTING_DATE='" + dtExcelData.Rows[i][13] + "',@ER_CURRENT_STATUS='" + dtExcelData.Rows[i][15] + "',@ER_LOCATION='" + dtExcelData.Rows[i][16] + "',@ER_ACK_STATUS='" + dtExcelData.Rows[i][17] + "',@ER_DRIVER_NAME='" + dtExcelData.Rows[i][18] + "',@ER_DRIVER_PHONE='" + dtExcelData.Rows[i][19] + "',@ER_CNTR_BRANCH='" + dtExcelData.Rows[i][20] + "',@ER_FORMAN_DETAILS='" + dtExcelData.Rows[i][21] + "',@ER_DEST_CONTROL_BRANCH='" + dtExcelData.Rows[i][22] + "',@ER_FORMAN_BRANCH='" + dtExcelData.Rows[i][23] + "'";
                                        // string querry = "EXEC USP_ER_EXCELUPLOAD1 @ER_CATEGID=" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + ",@OPERATION='operation',@ER_ID=" + dt1.Rows[i][0] + ",@ER_VEHICLENO='" + dt1.Rows[i][1] + "',@ER_VEHICLETYPE='" + dt1.Rows[i][2] + "',@ER_BOOKBRANCH='" + dt1.Rows[i][3] + "',@ER_BOOKZONE='" + dt1.Rows[i][4] + "',@ER_DISPATCHDATE='" + dt1.Rows[i][5] + "',@ER_FROM='" + dt1.Rows[i][6] + "',@ER_TO='" + dt1.Rows[i][7] + "',@ER_PARTYNAME='" + dt1.Rows[i][8] + "',@ER_DELIVERYBRANCH='" + dt1.Rows[i][9] + "',@ER_TO_ZONE='" + dt1.Rows[i][10] + "',@ER_EXPECTED_DATE='" + dt1.Rows[i][11] + "',@ER_REPORTING_DATE='" + dt1.Rows[i][12] + "',@ER_CURRENT_DATE='" + dt1.Rows[i][12] + "',@ER_CURRENT_STATUS='" + dt1.Rows[i][13] + "',@ER_LOCATION='" + dt1.Rows[i][14] + "',@ER_ACK_STATUS='" + dt1.Rows[i][15] + "',@ER_DRIVER_NAME='" + dt1.Rows[i][16] + "',@ER_DRIVER_PHONE='" + dt1.Rows[i][17] + "',@ER_CNTR_BRANCH='" + dt1.Rows[i][18] + "',@ER_FORMAN_DETAILS='" + dt1.Rows[i][19] + "',@ER_DEST_CONTROL_BRANCH='" + dt1.Rows[i][20] + "',@ER_FORMAN_BRANCH='" + dt1.Rows[i][21] + "',@DATE='" + dt1.Rows[i][22] + "',@ER_CHALLON_NO='" + dt1.Rows[i][23] + "',@ER_REMARKS='" + dt1.Rows[i][24] + "',@ER3='" + dt1.Rows[i][25] + "',@ER4='" + dt1.Rows[i][26] + "',@ER5='" + dt1.Rows[i][27] + "'";
                                        result = Dal.ExecuteNonQueryDB2(querry, conn);
                                    }
                                    catch (Exception ex)
                                    {
                                        //BLL.ShowMessage(this, "" + dt1.Rows[i][0] + "");
                                    }

                                }
                                if (result == true)
                                {
                                    System.Data.DataTable dt_RouteDetails = BLL.ExecuteQuery("SELECT * FROM SMVTS_ER_TRIPINFO(NOLOCK) WHERE ER_CATEGID='" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + "'");
                                    foreach (DataRow dr in dt_RouteDetails.Rows)
                                    {


                                        try
                                        {

                                            // ASSIGNDRIVER(dr[1].ToString(), dr[17].ToString(), dr[18].ToString(), dr[20].ToString());


                                            //checking Startdate and enddate.. if missing.. continue.. the loop
                                            if ((dr[5].ToString() == string.Empty))
                                            {
                                                System.Data.DataTable dt_NoStartEndDate = (System.Data.DataTable)Session["DTEXCEL"];
                                                DataRow dr_err = dt_NoStartEndDate.NewRow();
                                                dr_err[0] = dr[1].ToString();
                                                dr_err[1] = dr[5].ToString();
                                                dr_err[2] = dr[11].ToString();
                                                dr_err[3] = dr[6].ToString();
                                                dr_err[4] = dr[7].ToString();
                                                dr_err[5] = "";
                                                dr_err[6] = "";
                                                dr_err[7] = "Start/End Date missing or is in wrong format";

                                                dt_NoStartEndDate.Rows.Add(dr_err);
                                                Session["DTEXCEL"] = dt_NoStartEndDate;

                                            }

                                            #region "Checking"

                                            else if (dr[11].ToString() != string.Empty)
                                            {
                                                string statrtdate = Convert.ToDateTime(dr[5]).ToString();
                                                string enddate = Convert.ToDateTime(dr[11]).ToString();
                                                if (Convert.ToDateTime(dr[5].ToString().Replace("-", "/")) < Convert.ToDateTime(dr[11].ToString().Replace("-", "/")))
                                                {
                                                    Plan_VehicleRoute_WithConsignment(dr);
                                                }
                                                else
                                                {
                                                    System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                                                    DataRow dr_err = dt_errors.NewRow();
                                                    dr_err[0] = dr[1].ToString();
                                                    dr_err[1] = dr[5].ToString();
                                                    dr_err[2] = dr[11].ToString();
                                                    dr_err[3] = dr[6].ToString();
                                                    dr_err[4] = dr[7].ToString();
                                                    dr_err[5] = "";
                                                    dr_err[6] = "";
                                                    dr_err[7] = "Start Date Greater than End date";

                                                    dt_errors.Rows.Add(dr_err);
                                                    Session["DTEXCEL"] = dt_errors;
                                                }
                                            }
                                            #endregion
                                            #region "EnDDate"
                                            else
                                            {
                                                System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                                                DataRow dr_err = dt_errors.NewRow();
                                                dr_err[0] = dr[1].ToString();
                                                dr_err[1] = dr[5].ToString();
                                                dr_err[2] = dr[11].ToString();
                                                dr_err[3] = dr[6].ToString();
                                                dr_err[4] = dr[7].ToString();
                                                dr_err[5] = "";
                                                dr_err[6] = "";
                                                dr_err[7] = "End Date is Empty";

                                                dt_errors.Rows.Add(dr_err);
                                                Session["DTEXCEL"] = dt_errors;
                                            }
                                            #endregion


                                        }
                                        catch (Exception ex) { }
                                    }
                                    //  BLL.ShowMessage(this, "Excel uploaded successfully");
                                    status = "Excel uploaded successfully";
                                }
                                else
                                {
                                    //BLL.ShowMessage(this, "Failed to upload Excel");
                                    status = "Failed to upload Excel";
                                }


                            }
                            else
                            {
                                //BLL.ShowMessage(this, "Excel file is empty cannot be uploaded.");
                                status = "Excel file is empty cannot be uploaded.";
                            }
                            //}
                        }
                        else
                        {
                            //  BLL.ShowMessage(this, "Selected file is not an Excel file.Please select Excel File.");
                            status = "Selected file is not an Excel file.Please select Excel File.";
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return Json(new { data = status });
        }

        private void Plan_VehicleRoute_WithConsignment(DataRow dr)
        {


            string vehicleNo = dr[1].ToString(), startDate = dr[5].ToString().Replace("-", "/");
            string ExpectDate = dr[11].ToString().Replace("-", "/"), Distance = string.Empty;
            string StartPoint = dr[6].ToString(), EndPoint = dr[7].ToString(), via = string.Empty;



            if (StartPoint.Contains("+"))
            {
                string[] points = StartPoint.Split('+');
                StartPoint = points[0].Trim();
                for (int i = 1; i < points.Length; i++)
                {
                    if (points[i] != string.Empty)
                    {
                        via = via + points[i] + "|";
                    }
                }

            }


            if (EndPoint.Contains("+"))
            {
                string[] points = EndPoint.Split('+');
                EndPoint = points[points.Length - 1].Trim();
                for (int i = 0; i < points.Length - 1; i++)
                {
                    if (points[i] != string.Empty)
                    {
                        via = via + points[i] + "|";
                    }
                }

            }


            string consgDetails = dr[8].ToString(), reachdate = "", loadingDate = dr[5].ToString();
            string ExpDeliveryDate = dr[11].ToString(), reportingDate = dr[12].ToString(), unloadingDate = dr[12].ToString();
            string consgRemarks = dr[21].ToString();
            string forman = dr[20].ToString();
            string driver_no = dr[18].ToString();
            string zone = dr[10].ToString();
            string Operation = "Insert_Rec";
            string ROUTES_NAME = StartPoint.Trim() + "_" + via.Trim() + "_" + EndPoint.Trim();
            string ROUTES_STATUS = "True";
            string ROUTES_CREATEDDATE = DateTime.Now.ToShortDateString().Replace("-", "/");
            string ROUTES_MODIFIEDDATE = DateTime.Now.ToShortDateString().Replace("-", "/");
            string ROUTES_FROM = StartPoint;
            string ROUTES_TO = EndPoint;
            string ROUTES_VIA = via;
            string ROUTES_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID.ToString(); //session value from the page.
            string ROUTES_CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID.ToString();
            string ROUTES_MODIFIEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID.ToString();
            string ROUTES_ST_LAT, ROUTES_ST_LNG, ROUTES_END_LAT, ROUTES_END_LNG, ROUTES_DISTANCE = "0.00";
            string ROUTES_POINTS = "", ROUTES_INTRMED_POINTSDATA = "";
            double TotDist = 0, TotDistCnt = 10;

            string ROUTE_ID;
            //----------------------------------------
            //string strIsRoute = "SELECT TOP 1 ROUTES_ID FROM SMVTS_ROUTES (NOLOCK) WHERE ROUTES_FROM='" + ROUTES_FROM + "' AND ROUTES_TO='" + ROUTES_TO + "' AND ROUTES_VIA='" + ROUTES_VIA + "' AND ROUTES_CATEGORY_ID='" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + "'";
            string strIsRoute = "SELECT TOP 1 ROUTES_ID,ROUTES_CATEGORY_ID,ROUTES_NAME,ROUTES_STARTLAT,ROUTES_ENDLONG,ROUTES_POINTS,ROUTES_STATUS,ROUTES_FROM,ROUTES_TO,ROUTES_VIA,ROUTES_DEVIATION_LIMIT,ROUTES_DISTANCE,ROUTES_STARTLONG,ROUTES_ENDLAT FROM SMVTS_ROUTES (NOLOCK)WHERE ROUTES_FROM='" + ROUTES_FROM + "' AND ROUTES_TO='" + ROUTES_TO + "' AND ROUTES_VIA='" + ROUTES_VIA + "' AND ROUTES_CATEGORY_ID='" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + "'";
            System.Data.DataTable dt_route = BLL.ExecuteQuery(strIsRoute);
            //BLL.ShowMessage(this, "hello"+dt_route.Rows.Count);

            //----------------------------------------
            try
            {
                if (dt_route.Rows.Count < 1)
                {

                    List<DirectionSteps> _obj = GMapUtil.GetDirections(StartPoint, EndPoint, via);
                    //totalPoints = totalPoints + "(" + directionStep.StLat + "," + directionStep.StLong+"),";
                    if (_obj.Count > 0)
                    {
                        ROUTES_ST_LAT = _obj[0].Steps[0].StLat;
                        ROUTES_ST_LNG = _obj[0].Steps[0].StLong;
                        ROUTES_END_LAT = _obj[_obj.Count - 1].Steps[_obj[_obj.Count - 1].Steps.Count - 1].EndLat;
                        ROUTES_END_LNG = _obj[_obj.Count - 1].Steps[_obj[_obj.Count - 1].Steps.Count - 1].EndLong;

                        for (int i = 0; i < _obj.Count; i++)
                        {
                            ROUTES_DISTANCE = System.Convert.ToString(System.Convert.ToDouble(ROUTES_DISTANCE) +

System.Convert.ToDouble(_obj[i].TotalDistance.Replace("km", "").Trim()));
                            string s = _obj[i].Steps.Count.ToString();
                            for (int j = 0; j < _obj[i].Steps.Count; j++)
                            {
                                ROUTES_POINTS = ROUTES_POINTS + "(" + _obj[i].Steps[j].StLat + "," + _obj[i].Steps

[j].StLong + "),";
                                if (_obj[i].Steps[j].Distance.ToUpper().Contains("KM"))
                                    TotDist = TotDist + System.Convert.ToDouble(_obj[i].Steps[j].Distance.ToUpper

().Replace("KM", "").Trim());
                                else if (_obj[i].Steps[j].Distance.ToUpper().Contains("M"))
                                    TotDist = TotDist + System.Convert.ToDouble(_obj[i].Steps[j].Distance.ToUpper

().Replace("M", "").Trim()) / 1000;


                                if (TotDistCnt <= TotDist)
                                {
                                    ROUTES_INTRMED_POINTSDATA = ROUTES_INTRMED_POINTSDATA + _obj[i].Steps[j].StLat + "," +

_obj[i].Steps[j].StLong + "," + (TotDist * 1000).ToString() + ";";
                                    TotDistCnt = TotDist + 10;
                                }
                            }
                        }
                    }
                    else
                    {
                        System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                        DataRow dr_err = dt_errors.NewRow();
                        dr_err[0] = vehicleNo;
                        dr_err[1] = startDate;
                        dr_err[2] = ExpectDate;
                        dr_err[3] = ROUTES_FROM;
                        dr_err[4] = ROUTES_TO;
                        dr_err[5] = ROUTES_VIA;
                        dr_err[6] = Distance;
                        dr_err[7] = "No Route Found";

                        dt_errors.Rows.Add(dr_err);
                        Session["DTEXCEL"] = dt_errors;
                        return;
                    }


                    string StrQuery = "EXEC USP_SMVTS_ROUTES @Operation ='" + Operation + "', @ROUTES_CATEGORY_ID='" +

ROUTES_CATEGORY_ID + "', @ROUTES_NAME='" + ROUTES_NAME;
                    StrQuery = StrQuery + "', @ROUTES_STARTLAT='" + ROUTES_ST_LAT + "', @ROUTES_STARTLONG='" +

ROUTES_ST_LNG + "', @ROUTES_ENDLAT='" + ROUTES_END_LAT;
                    StrQuery = StrQuery + "', @ROUTES_ENDLONG='" + ROUTES_END_LNG + "', @ROUTES_POINTS='" +

ROUTES_POINTS.Substring(0, ROUTES_POINTS.Length - 1) + "', @ROUTES_STATUS='" + ROUTES_STATUS;
                    StrQuery = StrQuery + "', @ROUTES_CREATEDBY='" + ROUTES_CREATEDBY + "', @ROUTES_CREATEDDATE='" +

ROUTES_CREATEDDATE + "', @ROUTES_MODIFIEDBY='" + ROUTES_MODIFIEDBY;
                    StrQuery = StrQuery + "', @ROUTES_MODIFIEDDATE='" + ROUTES_MODIFIEDDATE + "', @ROUTES_FROM='" +

ROUTES_FROM + "', @ROUTES_TO='" + ROUTES_TO + "', @ROUTES_VIA='" + ROUTES_VIA;
                    StrQuery = StrQuery + "', @ROUTES_DEVIATION_LIMIT='4', @ROUTES_DISTANCE='" + ROUTES_DISTANCE + "', @ROUTES_INTRMED_POINTSDATA='" + ROUTES_INTRMED_POINTSDATA.Substring(0, ROUTES_INTRMED_POINTSDATA.Length - 1) + "'";

                    dt_route = BLL.ExecuteQuery(StrQuery);
                }
            }
            catch (Exception e)
            {
                System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                DataRow dr_err = dt_errors.NewRow();
                dr_err[0] = vehicleNo;
                dr_err[1] = startDate;
                dr_err[2] = ExpectDate;
                dr_err[3] = ROUTES_FROM;
                dr_err[4] = ROUTES_TO;
                dr_err[5] = ROUTES_VIA;
                dr_err[6] = Distance;
                dr_err[7] = "Unable to create Route";

                dt_errors.Rows.Add(dr_err);
                Session["DTEXCEL"] = dt_errors;
                return;
            }
            //Route Planning Block.
            ROUTE_ID = dt_route.Rows[0][0].ToString();

            if (vehicleNo != null)
            {
                int DevId = 0;
                //int DevId = Convert.ToInt32(((RadComboBoxItem)rcmb_ReportVehicle.Items.FindItemByText(vehicleNo)).Value);
                DataTable dt_v = BLL.get_VehicleDeviceId(vehicleNo);
                if (dt_v.Rows.Count > 0)
                {
                    DevId = Convert.ToInt32(dt_v.Rows[0][0]);
                }

                SMVTS_ROUTEPLAN _obj_Smvts_RoutePlan = new SMVTS_ROUTEPLAN();
                _obj_Smvts_RoutePlan.VEHROUTE_STARTDATE = Convert.ToDateTime(startDate.Replace("-", "/"));
                _obj_Smvts_RoutePlan.VEHROUTE_ENDDATE = Convert.ToDateTime(ExpectDate.Replace("-", "/"));
                _obj_Smvts_RoutePlan.VEHROUTE_DEVICE_ID = DevId;
                _obj_Smvts_RoutePlan.OPERATION = operation.Update;
                System.Data.DataTable dtCheckDates = BLL.get_RoutePlan(_obj_Smvts_RoutePlan);
                // string ec = BLL.get_RoutePlan1(_obj_Smvts_RoutePlan);
                //BLL.ShowMessage(this, startDate.ToString() + ExpectDate.ToString() + DevId.ToString());
                // BLL.ShowMessage(this, ec);
                // Response.Write(ec);
                //System.Data.DataTable dtCheckDates;
                int cntCheckDates = dtCheckDates.Rows.Count;

                //********  Checking Time **************
                System.Data.DataTable dt = new System.Data.DataTable();
                string status = null;
                _obj_Smvts_RoutePlan = new SMVTS_ROUTEPLAN();
                _obj_Smvts_RoutePlan.VEHROUTE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_RoutePlan.VEHROUTE_DEVICE_ID = DevId;
                _obj_Smvts_RoutePlan.VEHROUTE_STARTDATE = Convert.ToDateTime(startDate);
                _obj_Smvts_RoutePlan.VEHROUTE_ENDDATE = Convert.ToDateTime(ExpectDate);
                _obj_Smvts_RoutePlan.VEHROUTE_STATUS = 0;
                _obj_Smvts_RoutePlan.OPERATION = operation.Check;
                dt = BLL.get_RoutePlan(_obj_Smvts_RoutePlan);
                if (dt.Rows.Count > 0)
                {
                    status = Convert.ToString(dt.Rows[0][0]);
                    //BLL.ShowMessage(this, "hello3");
                }

                _obj_Smvts_RoutePlan = new SMVTS_ROUTEPLAN();
                _obj_Smvts_RoutePlan.VEHROUTE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                _obj_Smvts_RoutePlan.VEHROUTE_DEVICE_ID = DevId;

                _obj_Smvts_RoutePlan.VEHROUTE_ROUTE_ID = Convert.ToInt32(ROUTE_ID);
                _obj_Smvts_RoutePlan.VEHROUTE_STARTDATE = Convert.ToDateTime(startDate);
                _obj_Smvts_RoutePlan.VEHROUTE_ENDDATE = Convert.ToDateTime(ExpectDate);
                _obj_Smvts_RoutePlan.VEHROUTE_STATUS = 1;
                _obj_Smvts_RoutePlan.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_RoutePlan.CREATEDDATE = DateTime.Now;
                _obj_Smvts_RoutePlan.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                _obj_Smvts_RoutePlan.LASTMDFDATE = DateTime.Now;
                _obj_Smvts_RoutePlan.VEHROUTE_AVGSPEED = 40;
                double distance = 400;
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 44)
                {
                    distance = 350;
                }

                _obj_Smvts_RoutePlan.VEHROUTE_TRAVELDISTINDAY = distance;
                _obj_Smvts_RoutePlan.VEHROUTE_DISTANCEROUTE = (Convert.ToString(dt_route.Rows[0][0]) != string.Empty) ?

Convert.ToDouble(Convert.ToString(dt_route.Rows[0][0])) : 10;

                SMVTS_CONSIGNMENT _obj_Smvts_Consignment = new SMVTS_CONSIGNMENT();
                _obj_Smvts_RoutePlan.CONSIGNMENT_TYPE = consgDetails.Trim();
                _obj_Smvts_RoutePlan.CONSIGNMENT_DETAILS = consgDetails.Trim();
                _obj_Smvts_RoutePlan.CONSIGNMENT_FROM = StartPoint.Trim();
                _obj_Smvts_RoutePlan.CONSIGNMENT_TO = EndPoint.Trim();
                _obj_Smvts_RoutePlan.CONSIGNMENT_CONSIGNEE_DETAILS = "";

                _obj_Smvts_RoutePlan.CONSIGNMENT_REACHDAY_INSOUREC = (reachdate != string.Empty) ? Convert.ToString

(reachdate) : string.Empty;
                _obj_Smvts_RoutePlan.CONSIGNMENT_LOADINGDATE = (loadingDate != string.Empty) ? Convert.ToString

(loadingDate) : string.Empty;
                _obj_Smvts_RoutePlan.CONSIGNMENT_EXPDATEOFDELIVERY = (ExpDeliveryDate != string.Empty) ? Convert.ToString

(ExpDeliveryDate) : string.Empty;
                _obj_Smvts_RoutePlan.CONSIGNMENT_REPORTINGDATE = (reportingDate != string.Empty) ? Convert.ToString

(reportingDate) : string.Empty;
                _obj_Smvts_RoutePlan.CONSIGNMENT_UNLOADINGDATE = (unloadingDate != string.Empty) ? Convert.ToString

(unloadingDate) : string.Empty;
                _obj_Smvts_RoutePlan.CONSIGNMENT_REMARK = Convert.ToString(consgRemarks);
                _obj_Smvts_RoutePlan.CONSIGNMENT_FORMAN = forman;
                _obj_Smvts_RoutePlan.CONSIGNMENT_DRIVER_NUMBER = driver_no;
                _obj_Smvts_RoutePlan.CONSIGNMENT_ZONE = zone;


                _obj_Smvts_RoutePlan.OPERATION = operation.Check;

                if (cntCheckDates > 0)
                {
                    System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                    DataRow dr_err = dt_errors.NewRow();
                    dr_err[0] = vehicleNo;
                    dr_err[1] = startDate;
                    dr_err[2] = ExpectDate;
                    dr_err[3] = ROUTES_FROM;
                    dr_err[4] = ROUTES_TO;
                    dr_err[5] = ROUTES_VIA;
                    dr_err[6] = Distance;
                    dr_err[7] = "Dates already assigned";

                    dt_errors.Rows.Add(dr_err);
                    Session["DTEXCEL"] = dt_errors;
                    return;
                }
                if (status.ToUpper() == "NO")
                {
                    _obj_Smvts_RoutePlan.OPERATION = operation.Insert;

                    if (BLL.set_RoutePlan(_obj_Smvts_RoutePlan)) //&& BLL.set_Vehicles(_obj_Smvts_Vehicles))
                    {

                    }
                    else
                    {
                        System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                        DataRow dr_err = dt_errors.NewRow();
                        dr_err[0] = vehicleNo;
                        dr_err[1] = startDate;
                        dr_err[2] = ExpectDate;
                        dr_err[3] = ROUTES_FROM;
                        dr_err[4] = ROUTES_TO;
                        dr_err[5] = ROUTES_VIA;
                        dr_err[6] = Distance;
                        dr_err[7] = "Unable to Save";

                        dt_errors.Rows.Add(dr);
                        Session["DTEXCEL"] = dt_errors;
                    }

                }
                else
                {
                    System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                    DataRow dr_err = dt_errors.NewRow();
                    dr_err[0] = vehicleNo;
                    dr_err[1] = startDate;
                    dr_err[2] = ExpectDate;
                    dr_err[3] = ROUTES_FROM;
                    dr_err[4] = ROUTES_TO;
                    dr_err[5] = ROUTES_VIA;
                    dr_err[6] = Distance;
                    dr_err[7] = "Time Already Asigned";

                    dt_errors.Rows.Add(dr);
                    Session["DTEXCEL"] = dt_errors;
                    return;


                }
            }

            else
            {
                System.Data.DataTable dt_errors = (System.Data.DataTable)Session["DTEXCEL"];
                DataRow dr_err = dt_errors.NewRow();
                dr_err[0] = vehicleNo;
                dr_err[1] = startDate;
                dr_err[2] = ExpectDate;
                dr_err[3] = ROUTES_FROM;
                dr_err[4] = ROUTES_TO;
                dr_err[5] = ROUTES_VIA;
                dr_err[6] = Distance;
                dr_err[7] = "Invalid VehicleNo";

                dt_errors.Rows.Add(dr_err);
                Session["DTEXCEL"] = dt_errors;
            }
        }


        #region "Directions service"

        public class GMapUtil
        {

            public static List<DirectionSteps> GetDirections(string origin, string destination, string waypoints)
            {

                var requestUrl = string.Format("http://maps.google.com/maps/api/directions/xml?origin={0}&destination={1}&waypoints={2}&sensor=false", origin, destination, waypoints);

                try
                {
                    var client = new WebClient();
                    var result = client.DownloadString(requestUrl);
                    return ParseDirectionResults(result);
                }

                catch (Exception)
                {

                    return null;

                }

            }



            private static List<DirectionSteps> ParseDirectionResults(string result)
            {
                var directionStepsList = new List<DirectionSteps>();
                var xmlDoc = new XmlDocument { InnerXml = result };

                if (xmlDoc.HasChildNodes)
                {
                    var directionsResponseNode = xmlDoc.SelectSingleNode("DirectionsResponse");
                    if (directionsResponseNode != null)
                    {
                        var statusNode = directionsResponseNode.SelectSingleNode("status");
                        if (statusNode != null && statusNode.InnerText.Equals("OK"))
                        {
                            var legs = directionsResponseNode.SelectNodes("route/leg");
                            //var totalPoints = "";
                            //var IntrmedPointsData = "";
                            //double TotDist = 0,TotDistCnt=10;

                            foreach (XmlNode leg in legs)
                            {

                                int stepCount = 1;

                                var stepNodes = leg.SelectNodes("step");

                                var steps = new List<DirectionStep>();

                                foreach (XmlNode stepNode in stepNodes)
                                {
                                    var directionStep = new DirectionStep();
                                    directionStep.Index = stepCount++;
                                    directionStep.Distance = stepNode.SelectSingleNode("distance/text").InnerText;
                                    directionStep.Duration = stepNode.SelectSingleNode("duration/text").InnerText;
                                    directionStep.Description = Regex.Replace(stepNode.SelectSingleNode("html_instructions").InnerText, "<[^<]+?>", "");
                                    directionStep.StLat = stepNode.SelectSingleNode("start_location/lat").InnerText;
                                    directionStep.StLong = stepNode.SelectSingleNode("start_location/lng").InnerText;

                                    directionStep.EndLat = stepNode.SelectSingleNode("end_location/lat").InnerText;
                                    directionStep.EndLong = stepNode.SelectSingleNode("end_location/lng").InnerText;
                                    //totalPoints = totalPoints +  "(" + directionStep.StLat + "," + directionStep.StLong+"),";

                                    //if(stepNode.SelectSingleNode("distance/text").InnerText.ToUpper().Contains("KM"))
                                    //    TotDist = TotDist + System.Convert.ToDouble(stepNode.SelectSingleNode("distance/text").InnerText.ToUpper().Replace("KM", "").Trim());
                                    //else
                                    //    TotDist=TotDist+System.Convert.ToDouble(stepNode.SelectSingleNode("distance/text").InnerText.ToUpper().Replace("M","").Trim())/1000;
                                    //if (TotDistCnt <= TotDist)
                                    //{
                                    //    IntrmedPointsData = IntrmedPointsData + directionStep.StLat + "," + directionStep.StLong + "," + TotDist.ToString() + ";";
                                    //    TotDistCnt = TotDist + 10;
                                    //}
                                    steps.Add(directionStep);

                                }

                                var directionSteps = new DirectionSteps();
                                directionSteps.OriginAddress = leg.SelectSingleNode("start_address").InnerText;
                                directionSteps.DestinationAddress = leg.SelectSingleNode("end_address").InnerText;
                                directionSteps.TotalDistance = leg.SelectSingleNode("distance/text").InnerText;
                                directionSteps.TotalDuration = leg.SelectSingleNode("duration/text").InnerText;
                                directionSteps.Steps = steps;
                                directionStepsList.Add(directionSteps);

                            }


                        }

                    }

                }

                return directionStepsList;

            }

        }

        public class DirectionStep
        {

            public int Index { get; set; }

            public string Description { get; set; }

            public string Distance { get; set; }

            public string Duration { get; set; }

            public string StLat { get; set; }
            public string StLong { get; set; }
            public string EndLat { get; set; }
            public string EndLong { get; set; }

        }

        public class DirectionSteps
        {

            public string TotalDuration { get; set; }

            public string TotalDistance { get; set; }

            public string OriginAddress { get; set; }

            public string DestinationAddress { get; set; }

            public List<DirectionStep> Steps { get; set; }

        }
        #endregion




        [HttpPost]
        public JsonResult get_FormsBy_Category(int CATEGID)
        {
            string forms_data = "";
            DataTable dt_forms = BLL.GetFormsByCategory(CATEGID);
            if (dt_forms.Rows.Count > 0)
            {
                forms_data = dt_forms.Rows[0]["ROLES_FORMSID"].ToString();
            }


            return Json(new { data = forms_data });
        }
        [HttpPost]
        public JsonResult getUsersByCategory(int CategID)
        {
            DataTable dt = new DataTable();
            dt = BLL.getUsersByCategory(CategID);
            List<SMVTS_USERS> obj = new List<SMVTS_USERS>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_USERS
                    {
                        USERS_FULLNAME = dt.Rows[i]["USERS_FULLNAME"].ToString(),
                        USERS_ID = Convert.ToInt32(dt.Rows[i]["USERS_ID"])
                    });
                }
            }
            return Json(new { data = obj });
        }

        [HttpPost]
        public JsonResult getDataByUser(string distributorID, string dealerID, string CUSTID)



        {
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();

            if (Session["USERINFO"] == null)
            {

            }
            SMVTS_CATEGORIES obj_categ = new SMVTS_CATEGORIES();


            DataTable dt_user = BLL.getCustomerUserID(distributorID, dealerID, CUSTID);

            if (dt_user.Rows.Count > 0)
            {

                for (int j = 0; j < dt_user.Rows.Count; j++)
                {
                    string USERID = dt_user.Rows[j]["USERID"].ToString();
                    DataTable dt = BLL.get_GridTrackDistancePro(_obj_Smvts_GridTrack, USERID);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        int spe = 0;
                        if (dt.Rows[i]["SPEED"] == DBNull.Value)
                        {
                            spe = 0;
                        }
                        else
                        {
                            spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                        }
                        obj.Add(new SMVTS_GRIDTRACK
                        {
                            SPEED = spe,
                            IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                            DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                            VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                            DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                            PLACE = dt.Rows[i]["PLACE"].ToString(),
                            DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                            DURATION = dt.Rows[i]["duration"].ToString(),
                            DKM = dt.Rows[i]["distance_day"].ToString(),
                            TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                            LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                            LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                            COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                            DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                            PARENT_NAME = dt_user.Rows[j]["PARENT_NAME"].ToString(),
                            DEALER_NAME = dt_user.Rows[j]["DEALER_NAME"].ToString(),
                            CUSTOMER_NAME = dt_user.Rows[j]["CUSTOMER_NAME"].ToString(),
                            Lastdate = dt.Rows[i]["DATE"].ToString(),
                            VEHICLE_IMAGE = dt.Rows[i]["VEHMODELIMAGES_IMAGEURL"].ToString(),
                            SIM_MNO = dt.Rows[i]["SIM_NUMBER"].ToString(),
                            IMEI = dt.Rows[i]["IMEI"].ToString(),
                            DeviceName = dt.Rows[i]["DEVICENAME"].ToString(),
                            InstallDate = dt.Rows[i]["STARTDATE"].ToString(),
                            ExpDate = dt.Rows[i]["EXPDATE"].ToString()

                        });
                    }
                }
            }
            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [SessionAuthorize]
        public ActionResult AssignedGeofence()
        {
            int clientID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            //Load  Vehicles
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            _obj_Smvts_Vehicles.OPERATION = operation.Empty;
            _obj_Smvts_Vehicles.CREATEDBY = 0;
            _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(clientID);
            DataTable dt = new DataTable();
            dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLES
                {
                    VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_ID"]),
                });
            }
            ViewBag.Vehicles = obj;


            //Load Lanmarks
            SMVTS_LANDMARKS obj_landmarks = new SMVTS_LANDMARKS();
            DataTable dt_land = BLL.getGeofenceLandmarks(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
            List<SMVTS_LANDMARKS> obj_landmark = new List<SMVTS_LANDMARKS>();
            for (int i = 0; i < dt_land.Rows.Count; i++)
            {
                obj_landmark.Add(new SMVTS_LANDMARKS
                {
                    LANDMARKS_ADDRESS = dt_land.Rows[i]["LANDMARKS_ADDRESS"].ToString(),
                    LANDMARKS_ID = Convert.ToInt32(dt_land.Rows[i]["LANDMARKS_ID"]),

                });

            }
            ViewBag.landmarks = obj_landmark;
            return View();
        }

        public JsonResult getVehicles()
        {
            int clientID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            //Load  Vehicles
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            _obj_Smvts_Vehicles.OPERATION = operation.Empty;
            _obj_Smvts_Vehicles.CREATEDBY = 0;
            _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(clientID);
            DataTable dt = new DataTable();
            dt = BLL.get_Vehicles(_obj_Smvts_Vehicles);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLES
                {
                    VEHICLES_REGNUMBER = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    VEHICLES_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_ID"]),
                });
            }
            var jsonData = new
            {
                data = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult Saveconfig(string chkedvehicles, string mobileNumbers, string LandMark)
        {
            string result = "";
            SMVTS_ASSIGNGEOFENCE_LANDMARKS obj = new SMVTS_ASSIGNGEOFENCE_LANDMARKS();

            obj.ASSIGNGEOFENCE_CATEGID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            obj.USERS_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            obj.ASSIGNGEOFENCE_LANDID = Convert.ToInt32(LandMark);
            obj.ASSIGNGEOFENCE_VEHICLEID = chkedvehicles.ToString();
            obj.ASSIGNGEOFENCE_SIMS = mobileNumbers;
            obj.ASSIGNGEOFENCE_STATUS = true;
            DataTable dt = BLL.get_assigned_vehicles(obj);
            if (dt.Rows.Count > 0)
            {
                string vehicles = dt.Rows[0]["ASSIGNGEOFENCE_VEHICLEID"].ToString();

                List<string> first = new List<string>(vehicles.Split(','));

                string[] values = chkedvehicles.Split(',');
                int i = 0; int length = values.Length;
                foreach (string value in values)
                {
                    i++;
                    if (first.Contains(value))
                    {
                        int deviceid = Convert.ToInt32(value);
                        DataTable dt_vehicle = Dal.ExecuteQuery_Prod("select VEHICLES_REGNUMBER from smvts_vehicles where VEHICLES_DEVICE_ID=" + deviceid + "");
                        result = "Vehicle " + dt_vehicle.Rows[0][0].ToString() + " Already exists Please Select different Vehicle";
                        break;
                    }
                    else
                    {

                    }
                    if (i == length)
                    {
                        obj.ASSIGNGEOFENCE_VEHICLEID = vehicles + "," + chkedvehicles.ToString();
                        bool upd_status = BLL.update_assigned_vehicles(obj);
                        if (upd_status == true)
                        {
                            result = "true";
                        }
                        else { result = "false"; }
                    }
                }
                // for (; i<chk_vehicles.Length;i++)
                // {
                //     if(vehicles.Substring(chk_vehicles[i].ToString()))
                //         {
                //         int deviceid=Convert.ToInt32(chk_vehicles[i]);
                //         DataTable dt_vehicle = Dal.ExecuteQuery_Prod("select VEHICLES_REGNUMBER from smvts_vehicles where VEHICLES_DEVICE_ID=" + deviceid + "");
                //         result = "Vehicle "+dt_vehicle.Rows[0][0].ToString()+" Already exists Please Select different Vehicle";
                //         break;
                //         }
                // }
                //if(i== chk_vehicles.Length)
                // {
                //     obj.ASSIGNGEOFENCE_VEHICLEID= vehicles+","+ chkedvehicles.ToString();
                //     bool upd_status = BLL.update_assigned_vehicles(obj);
                //     if(upd_status==true)
                //     {
                //         result = "true";
                //     }
                //     else { result = "false"; }
                // }
            }
            else
            {
                bool status = BLL.set_Geofenceconfig(obj);
                if (status == true)
                {
                    result = "true";
                }
                else { result = "false"; }
            }


            return Json(new { data = result });
        }

        public JsonResult getAllGeofenceData()
        {
            int categID = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            DataTable dt = BLL.GetAllGeofences(categID);
            SMVTS_ASSIGNGEOFENCE_LANDMARKS obj = new SMVTS_ASSIGNGEOFENCE_LANDMARKS();

            List<SMVTS_ASSIGNGEOFENCE_LANDMARKS> obj_all = new List<SMVTS_ASSIGNGEOFENCE_LANDMARKS>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string vehicle_Numbers = "";
                    string[] allvehicles = dt.Rows[i]["ASSIGNGEOFENCE_VEHICLEID"].ToString().Split(',');
                    if (allvehicles.Length > 0)
                    {
                        for (int j = 0; j < allvehicles.Length; j++)
                        {
                            DataTable dt_vehicle = BLL.getVehicleRegNumber(allvehicles[j]);
                            if (vehicle_Numbers == "")
                            {
                                vehicle_Numbers = dt_vehicle.Rows[0][0].ToString();
                            }
                            else
                            {
                                vehicle_Numbers = vehicle_Numbers + ", " + dt_vehicle.Rows[0][0].ToString();
                            }
                        }
                    }
                    obj_all.Add(new SMVTS_ASSIGNGEOFENCE_LANDMARKS
                    {
                        VehicleNumbers = vehicle_Numbers,
                        LandMark = dt.Rows[i]["Landmark"].ToString(),
                        ASSIGNGEOFENCE_SIMS = dt.Rows[i]["ASSIGNGEOFENCE_SIMS"].ToString(),
                    });


                }
            }
            var jsonData = new
            {
                data = obj_all
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [SessionAuthorize]
        public ActionResult ORDERS()
        {

            SMVTS_CATEGORIES _obj_Smvts_categories = new SMVTS_CATEGORIES();

            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                _obj_Smvts_categories.OPERATION = operation.Empty;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 4)
            {
                _obj_Smvts_categories.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                //  _obj_Smvts_categories.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
                _obj_Smvts_categories.OPERATION = operation.Select;
                _obj_Smvts_categories.CATEG_STATUS = "1";
            }

            DataTable dt5 = BLL.get_Categories(_obj_Smvts_categories);
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            for (int m = 0; m < dt5.Rows.Count; m++)
            {
                obj.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = dt5.Rows[m]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt5.Rows[m]["CATEG_ID"]),
                });
            }
            ViewBag.categ = obj;


            //get Orders
            int categid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            DataTable dt_orders = BLL.get_orders(categid);
            List<SMVTS_PACKAGE_ORDERS> obj_orders = new List<SMVTS_PACKAGE_ORDERS>();
            if (dt_orders.Rows.Count > 0)
            {
                for (int i = 0; i < dt_orders.Rows.Count; i++)
                {
                    obj_orders.Add(new SMVTS_PACKAGE_ORDERS
                    {
                        ORDER_CATEGORY_NAME = dt_orders.Rows[i]["ORDER_CATEGORY_NAME"].ToString(),
                        ORDER_AMOUNT = Convert.ToDecimal(dt_orders.Rows[i]["ORDER_AMOUNT"]),
                        ORDER_CREATEDDATE = Convert.ToDateTime(dt_orders.Rows[i]["ORDER_CREATEDDATE"])
                    });
                }
            }
            ViewBag.orders = obj_orders;
            return View();
        }
        public JsonResult SaveOrders(string categId, string categName, string Amount)
        {
            SMVTS_PACKAGE_ORDERS obj = new SMVTS_PACKAGE_ORDERS();
            obj.ORDER_CATEGORY_ID = Convert.ToInt32(categId);
            obj.ORDER_CATEGORY_NAME = categName;
            obj.ORDER_AMOUNT = Convert.ToDecimal(Amount);
            obj.ORDER_CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            bool status = BLL.save_orders(obj);

            return Json(new { data = status });
        }

        [SessionAuthorize]
        public ActionResult SubscriptionPackages()
        {
            SMVTS_CUSTOMERPACKAGE obj = new SMVTS_CUSTOMERPACKAGE();
            int categID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt_subscriptions = BLL.GetSubscriptionPackages(categID);
            List<SMVTS_CUSTOMERPACKAGE> obj_sub = new List<SMVTS_CUSTOMERPACKAGE>();
            if (dt_subscriptions.Rows.Count > 0)
            {
                for (int i = 0; i < dt_subscriptions.Rows.Count; i++)
                {
                    obj_sub.Add(new SMVTS_CUSTOMERPACKAGE
                    {
                        PACKAGE_NAME = dt_subscriptions.Rows[i]["PACKAGE_NAME"].ToString(),
                        PACKAGE_PRICE = Convert.ToDecimal(dt_subscriptions.Rows[i]["PACKAGE_PRICE"]),
                        NUM_OF_DAYS = Convert.ToInt32(dt_subscriptions.Rows[i]["NUM_OF_DAYS"])
                    });
                }
            }
            ViewBag.packages = obj_sub;
            return View();
        }
        public JsonResult Save_Subscriptions(string Name, string billingDays, string Price, string FORMIDS, string columnids)
        {
            SMVTS_CUSTOMERPACKAGE obj = new SMVTS_CUSTOMERPACKAGE();
            obj.PACKAGE_NAME = Name;
            obj.PACKAGE_FORM_IDS = FORMIDS;
            obj.PACKAGE_COLUMNIDS = columnids;
            obj.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            obj.PACKAGE_PRICE = Convert.ToDecimal(Price);
            obj.NUM_OF_DAYS = Convert.ToInt32(billingDays);
            obj.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            bool status = BLL.Save_subscription(obj);

            return Json(new { data = status });
        }

        [SessionAuthorize]
        public ActionResult frm_cities()
        {
            //Load ciuntries
            SMVTS_COUNTRIES _obj_Smvts_Countries = new SMVTS_COUNTRIES();
            _obj_Smvts_Countries.OPERATION = operation.Insert;
            DataTable dt1 = BLL.get_Country(_obj_Smvts_Countries);
            List<SMVTS_COUNTRIES> obj1 = new List<SMVTS_COUNTRIES>();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj1.Add(new SMVTS_COUNTRIES
                {
                    COUNTRY_NAME = dt1.Rows[i]["COUNTRY_NAME"].ToString(),
                    COUNTRY_ID = Convert.ToInt32(dt1.Rows[i]["COUNTRY_ID"]),
                });
            }
            ViewBag.countries = obj1;


            //Load States
            List<SMVTS_STATES> obj_states = new List<SMVTS_STATES>();
            SMVTS_STATES _obj_Smvts_States = new SMVTS_STATES();
            _obj_Smvts_States.OPERATION = operation.Empty;
            _obj_Smvts_States.STATE_COUNTRY_ID = Convert.ToInt32(1);

            DataTable dt = BLL.get_State(_obj_Smvts_States);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj_states.Add(new SMVTS_STATES
                {
                    STATE_NAME = dt.Rows[i]["STATE_NAME"].ToString(),
                    STATE_ID = Convert.ToInt32(dt.Rows[i]["STATE_ID"])
                });
            }
            ViewBag.States = obj_states;

            //Load Cities
            List<SMVTS_CITIES> obj_cities = new List<SMVTS_CITIES>();
            DataTable dt_cities = BLL.Getcities();
            for (int i = 0; i < dt_cities.Rows.Count; i++)
            {
                obj_cities.Add(new SMVTS_CITIES
                {
                    STATE_NAME = dt_cities.Rows[i]["State"].ToString(),
                    CITY_NAME = dt_cities.Rows[i]["CITY_NAME"].ToString()
                });
            }
            ViewBag.Cities = obj_cities;
            return View();
        }

        public JsonResult addCities(string State_ID, string CityName)
        {
            SMVTS_CITIES obj = new SMVTS_CITIES();
            obj.CITY_NAME = CityName;
            obj.CITY_STATE_ID = Convert.ToInt32(State_ID);
            obj.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            obj.CREATEDDATE = DateTime.Now;
            bool status = BLL.SaveCities(obj);
            return Json(new { data = status });
        }

        [SessionAuthorize]
        public ActionResult frm_States()
        {
            //Load ciuntries
            SMVTS_COUNTRIES _obj_Smvts_Countries = new SMVTS_COUNTRIES();
            _obj_Smvts_Countries.OPERATION = operation.Insert;
            DataTable dt1 = BLL.get_Country(_obj_Smvts_Countries);
            List<SMVTS_COUNTRIES> obj1 = new List<SMVTS_COUNTRIES>();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj1.Add(new SMVTS_COUNTRIES
                {
                    COUNTRY_NAME = dt1.Rows[i]["COUNTRY_NAME"].ToString(),
                    COUNTRY_ID = Convert.ToInt32(dt1.Rows[i]["COUNTRY_ID"]),
                });
            }
            ViewBag.countries = obj1;

            List<SMVTS_STATES> obj = new List<SMVTS_STATES>();

            DataTable dt = BLL.GetStates();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_STATES
                {
                    STATE_NAME = dt.Rows[i]["STATE_NAME"].ToString(),
                    COUNTRY_NAME = dt.Rows[i]["Country"].ToString()
                });
            }

            ViewBag.states = obj;

            return View();
        }

        public JsonResult addStates(string Country_ID, string StateName)
        {
            SMVTS_STATES obj = new SMVTS_STATES();
            obj.STATE_NAME = StateName;
            obj.STATE_DESC = StateName;
            obj.STATE_COUNTRY_ID = Convert.ToInt32(Country_ID);
            obj.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            obj.CREATEDDATE = DateTime.Now;
            bool status = BLL.SaveStates(obj);
            return Json(new { data = status });
        }

        public JsonResult GetUserAlertType(string CONFIG_ID)
        {
            string mobileNumbers = "";
            DataTable dt = new DataTable();
            dt = BLL.getAlertTypeByID(CONFIG_ID);
            if (dt.Rows.Count > 0)
            {
                mobileNumbers = dt.Rows[0][0].ToString();
            }
            return Json(new { data = mobileNumbers });
        }
        public JsonResult UpdateAlertType(string CONFIG_ID, string MobileNumbers)
        {
            bool status = BLL.updateAlertType(CONFIG_ID, MobileNumbers);
            return Json(new { data = status });
        }

        [SessionAuthorize]
        public ActionResult DeviceWizard()
        {
            SMVTS_CATEGORIES _obj_Smvts_Devices = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();

            //Load Distrubutots
            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {


                ViewBag.categ = obj;
            }
            else
            {
                //    rcmb_DeviceCategID.Items.Clear();
                DataTable dt = BLL.get_Categories(new SMVTS_CATEGORIES());
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    obj.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_NAME = Convert.ToString(dt.Rows[i]["NAME"]),
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                    });
                }
                ViewBag.categ = obj;
            }

            //Load Device Names
            SMVTS_DEVICETYPES obj_types = new SMVTS_DEVICETYPES();
            DataTable dt_types = new DataTable();
            List<SMVTS_DEVICETYPES> obj_alltypes = new List<SMVTS_DEVICETYPES>();
            dt_types = BLL.GetDeviceNames();
            if (dt_types.Rows.Count > 0)
            {
                for (int k = 0; k < dt_types.Rows.Count; k++)
                {
                    obj_alltypes.Add(new SMVTS_DEVICETYPES
                    {
                        ID = Convert.ToInt32(dt_types.Rows[k]["ID"]),
                        DEVICE_TYPENAME = dt_types.Rows[k]["DEVICE_TYPENAME"].ToString()
                    });
                }
            }

            ViewBag.deviceTypes = obj_alltypes;

            //Load Sims


            return View();
        }

        public JsonResult LoadOrders(string CATEG_ID, string categname, int ParentID)
        {
            SMVTS_ORDERS _obj_Smvts_orders = new SMVTS_ORDERS();
            List<SMVTS_ORDERS> obj = new List<SMVTS_ORDERS>();
            string data;
            //if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            //{

            //}
            //else //Client
            //{
            //    _obj_Smvts_orders.ORDER_CATEGORYID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            //}
            _obj_Smvts_orders.ORDER_CATEGORYID = Convert.ToInt32(CATEG_ID);
            // string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
            string dbname = (ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
            string CategQuery = " SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + categname.Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "") + "' and CATEG_PARENT_ID=" + ParentID + "";

            DataTable dt_categ = new DataTable();

            dt_categ = Dal.ExecuteQueryDB1(CategQuery, dbname);
            if (dt_categ.Rows.Count > 0)
            {
                _obj_Smvts_orders.ORDER_CATEGORYID = Convert.ToInt32(dt_categ.Rows[0][0].ToString());
                DataTable dt = new DataTable();
                dt = BLL.GetUnusedOrders(_obj_Smvts_orders);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj.Add(new SMVTS_ORDERS
                        {
                            ORDER_ID = Convert.ToInt32(dt.Rows[i]["ORDER_ID"]),
                            ORDER_NAME = dt.Rows[i]["ORDER_NAME"].ToString(),

                        });
                    }
                }
                else
                {
                    data = "No Orders Found";
                }
            }
            else
            {
                data = "No Orders Found";
            }
            return Json(new { data = obj });
        }
        public JsonResult LoadIMEINumbers(string CATEG_ID, string categname)
        {
            smvts_tt_devid obj = new smvts_tt_devid();
            DataTable dt = new DataTable();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
            {
                dt = BLL.getIMEINumbers(CATEG_ID);
            }
            else
            {
                //  string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
                string dbname = (ConfigurationManager.ConnectionStrings["connection"].ToString());
                string CategQuery = " SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK)WHERE CATEG_NAME = '" + categname.Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "").Replace("(SWLP)", "").Replace("(WLP)", "") + "'";

                DataTable dt_categ = new DataTable();

                dt_categ = Dal.ExecuteQueryDB1(CategQuery, dbname);
                dt = BLL.getIMEINumbers(dt_categ.Rows[0][0].ToString());
            }



            List<smvts_tt_devid> obj_all = new List<smvts_tt_devid>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_all.Add(new smvts_tt_devid
                    {
                        DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"]),
                        IMEI = dt.Rows[i]["IMEI"].ToString()
                    });
                }
            }
            else
            {

            }

            return Json(new { data = obj_all });
        }

        public JsonResult LoadSIMS(string CATEG_ID, string categname)
        {
            SMVTS_ALL_SIMS obj = new SMVTS_ALL_SIMS();
            DataTable dt = new DataTable();

            //  string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
            string dbname = (ConfigurationManager.ConnectionStrings["connection"].ToString());
            string CategQuery = " SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + categname.Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "").Replace("(SWLP)", "").Replace("(WLP)", "") + "'";

            DataTable dt_categ = new DataTable();

            dt_categ = Dal.ExecuteQueryDB1(CategQuery, dbname);

            dt = BLL.getDealerSIMS(dt_categ.Rows[0][0].ToString());
            List<SMVTS_ALL_SIMS> obj_all = new List<SMVTS_ALL_SIMS>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_all.Add(new SMVTS_ALL_SIMS
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["ID"]),
                        SIMS = dt.Rows[i]["SIM_NUMBER"].ToString()
                    });
                }
            }
            else
            {

            }

            return Json(new { data = obj_all });
        }
        public JsonResult Save_Device(string btn, string DEVICENAME, string DEVICESERIALNO, string DeviceID, string VehicleNo, string VehicleType, string VehcleName, string SIMnumber, string STATUS, string CATEG_ID, string CATEG_NAME, string OrderId, string DealerID, string Vehimagemodel)
        {

            string data = "false";

            DataTable DT = new DataTable();
            int count = 0; int device_ID = 0;

            // validation to check Repeate Device Name
            SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
            // _obj_Smvts_Devices.OPERATION = operation.Check;
            _obj_Smvts_Devices.DEVICE_NAME = DEVICENAME;
            _obj_Smvts_Devices.DEVICE_SERIALNUMBER = DEVICESERIALNO;

            _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DeviceID);
            if (DEVICESERIALNO != "")
            {
                _obj_Smvts_Devices.DEVICE_SERIALNUMBER = DEVICESERIALNO;
                _obj_Smvts_Devices.OPERATION = operation.checkIMEI;
            }
            DT = BLL.get_Devices(_obj_Smvts_Devices);
            if (DT.Rows.Count > 0)
            {
                count = Convert.ToInt32(DT.Rows[0][0]);
            }
            int sim_count = 0;
            DataTable dt_sims = Dal.ExecuteQuery_Prod("select SIM_ID from SMVTS_SIMS WHERE SIM_NUMBER='" + SIMnumber + "'");
            if (dt_sims.Rows.Count > 0)
            {
                sim_count = Convert.ToInt32(dt_sims.Rows[0][0]);
            }


            //GET DEVICEID 




            _obj_Smvts_Devices = new SMVTS_DEVICES();
            //_obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DEVICEID);
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

            _obj_Smvts_Devices.DEVICE_SERIALNUMBER = BLL.ReplaceQuote(DEVICESERIALNO);

            _obj_Smvts_Devices.DEVICE_STATUS = Convert.ToString(STATUS);
            _obj_Smvts_Devices.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices.CREATEDDATE = DateTime.Now;
            _obj_Smvts_Devices.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            _obj_Smvts_Devices.LASTMDFDATE = DateTime.Now;
            //Session["DEVICE_CATEGORY_ID"]
            _obj_Smvts_Devices.DEVICE_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
            string Patnerid = CATEG_ID;
            string Org_Name_DropDown = CATEG_NAME;
            string dbname = "";
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                dbname = BLL.getDatabase_categ(Patnerid);
            }
            else
            {
                //dbname = BLL.getDatabase_deviceupdate(Patnerid);
                dbname = "Aor2T0SveXN3Qx99ow3pJdxI6ruR88McJyyA+m2RzQ7GhTWCzxCZGIf5D1n4CCQUJxaM2lG1Dw54Ew7+Lt094KFUqE9WKmftI15Bs/dDX74J5+Uak0lGlaJEX9NVCHgn";
            }

            switch (btn)
            {
                case "BTN_SAVE":
                    //  if (count == 0)
                    {
                        if (sim_count == 0)
                        {
                            //Get last record for Device id
                            DataTable dt_lastrec = new DataTable();
                            _obj_Smvts_Devices.OPERATION = operation.Insert;
                            dt_lastrec = BLL.get_Devices(_obj_Smvts_Devices);

                            if (dt_lastrec.Rows.Count > 0)
                            {
                                if (dt_lastrec.Rows[0][0].ToString() != "")
                                {
                                    device_ID = Convert.ToInt32(dt_lastrec.Rows[0][0]) + 1;
                                }
                                else
                                {
                                    device_ID = 1;
                                }
                            }

                            _obj_Smvts_Devices.DEVICE_ID = device_ID;

                            _obj_Smvts_Devices.OPERATION = operation.Insert;

                            DataTable dt_available = new DataTable();
                            dt_available = BLL.GetWalletData(Convert.ToInt32(DealerID));
                            if (dt_available.Rows.Count > 0)
                            {
                                int available = 0;
                                if ((dt_available.Rows[0]["AVAILABLE"]).ToString() != "")
                                {
                                    available = Convert.ToInt32(dt_available.Rows[0]["AVAILABLE"]);
                                }
                                else
                                {
                                    available = Convert.ToInt32(dt_available.Rows[0]["TOTAL"]);
                                }
                                if (available.ToString() != "" && available > 0)
                                {
                                    if (BLL.DeviceInstallation(CATEG_ID, device_ID, DEVICESERIALNO, DEVICENAME, SIMnumber, VehicleNo, VehicleType, VehcleName, ((SMVTS_USERS)Session["USERINFO"]).USERS_ID.ToString(), OrderId, CATEG_NAME, Vehimagemodel, ""))
                                    {
                                        data = "true";
                                        bool status = BLL.updateIMEIandSIM(device_ID, SIMnumber, DealerID, CATEG_NAME);
                                        bool status2 = BLL.UpdateDeviceWallet(CATEG_ID, DealerID);

                                    }

                                    else
                                    {
                                        data = "true";
                                    }
                                }
                                else
                                {
                                    data = "Sorry Insufficient Balance";
                                }
                            }
                            else
                            {
                                data = "Please Recharge your wallet";
                            }

                        }
                        else
                        {
                            data = "SIM  Already Exists.";
                        }
                    }
                    /*else
                    {
                        data = "Device  Already Exists.";
                        //  BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
                        //          rtxt_RoleName.Focus();
                        // return;
                    }*/
                    break;
                case "BTN_UPDATE":
                    if ((count == 0) || (count == 1))
                    {
                        _obj_Smvts_Devices.OPERATION = operation.Update;
                        _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DeviceID);
                        if (BLL.set_Devices(_obj_Smvts_Devices, dbname, Org_Name_DropDown, ""))
                        {
                            data = "true";
                            // BLL.ShowMessage(this, BLL.msg_Updated);
                            DataTable dtLastrec = new DataTable();
                            dtLastrec = BLL.get_Devices(_obj_Smvts_Devices);
                            if (dtLastrec.Rows.Count > 0)
                            {
                                DeviceID = Convert.ToString(dtLastrec.Rows[0][0]);
                            }
                        }
                        else
                        {
                            data = "false";
                        }
                    }
                    else
                    {
                        data = "Device Name Already Exist.";
                        // BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
                        // return;
                    }
                    break;

                default:
                    break;
            }

            return Json(new { data = data });
        }


        //public JsonResult Save_Device(string btn, string DEVICENAME, string DEVICESERIALNO, string DeviceID, string VehicleNo, string VehicleType, string VehcleName, string SIMnumber, string STATUS, string CATEG_ID, string CATEG_NAME, string OrderId, string DealerID)
        //{

        //    string data = "false";

        //    DataTable DT = new DataTable();
        //    int count = 0; int device_ID = 0;

        //    // validation to check Repeate Device Name
        //    SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
        //    // _obj_Smvts_Devices.OPERATION = operation.Check;
        //    _obj_Smvts_Devices.DEVICE_NAME = DEVICENAME;
        //    _obj_Smvts_Devices.DEVICE_SERIALNUMBER = DEVICESERIALNO;

        //    _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DeviceID);
        //    if (DEVICESERIALNO != "")
        //    {
        //        _obj_Smvts_Devices.DEVICE_SERIALNUMBER = DEVICESERIALNO;
        //        _obj_Smvts_Devices.OPERATION = operation.checkIMEI;
        //    }
        //    DT = BLL.get_Devices(_obj_Smvts_Devices);
        //    if (DT.Rows.Count > 0)
        //    {
        //        count = Convert.ToInt32(DT.Rows[0][0]);
        //    }
        //    int sim_count = 0;
        //    DataTable dt_sims = Dal.ExecuteQuery_Prod("select SIM_ID from SMVTS_SIMS WHERE SIM_NUMBER='" + SIMnumber + "'");
        //    if (dt_sims.Rows.Count > 0)
        //    {
        //        sim_count = Convert.ToInt32(dt_sims.Rows[0][0]);
        //    }
        //    _obj_Smvts_Devices = new SMVTS_DEVICES();
        //    //_obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DEVICEID);
        //    _obj_Smvts_Devices.DEVICE_CATEGORY_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

        //    _obj_Smvts_Devices.DEVICE_SERIALNUMBER = BLL.ReplaceQuote(DEVICESERIALNO);

        //    _obj_Smvts_Devices.DEVICE_STATUS = Convert.ToString(STATUS);
        //    _obj_Smvts_Devices.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
        //    _obj_Smvts_Devices.CREATEDDATE = DateTime.Now;
        //    _obj_Smvts_Devices.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
        //    _obj_Smvts_Devices.LASTMDFDATE = DateTime.Now;
        //    //Session["DEVICE_CATEGORY_ID"]
        //    _obj_Smvts_Devices.DEVICE_CATEGORY_ID = Convert.ToInt32(CATEG_ID);
        //    string Patnerid = CATEG_ID;
        //    string Org_Name_DropDown = CATEG_NAME;
        //    string dbname = "";
        //    if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
        //    {
        //        dbname = BLL.getDatabase_categ(Patnerid);
        //    }
        //    else
        //    {
        //        dbname = BLL.getDatabase_deviceupdate(Patnerid);
        //    }

        //    switch (btn)
        //    {
        //        case "BTN_SAVE":
        //            if (count == 0)
        //            {
        //                if (sim_count == 0)
        //                {
        //                    //Get last record for Device id
        //                    DataTable dt_lastrec = new DataTable();
        //                    _obj_Smvts_Devices.OPERATION = operation.Insert;
        //                    dt_lastrec = BLL.get_Devices(_obj_Smvts_Devices);

        //                    if (dt_lastrec.Rows.Count > 0)
        //                    {
        //                        if (dt_lastrec.Rows[0][0].ToString() != "")
        //                        {
        //                            device_ID = Convert.ToInt32(dt_lastrec.Rows[0][0]) + 1;
        //                        }
        //                        else
        //                        {
        //                            device_ID = 1;
        //                        }
        //                    }

        //                    _obj_Smvts_Devices.DEVICE_ID = device_ID;

        //                    _obj_Smvts_Devices.OPERATION = operation.Insert;

        //                    DataTable dt_available = new DataTable();
        //                    dt_available = BLL.GetWalletData(Convert.ToInt32(DealerID));
        //                    if (dt_available.Rows.Count > 0)
        //                    {
        //                        int available = 0;
        //                        if ((dt_available.Rows[0]["AVAILABLE"]).ToString() != "")
        //                        {
        //                            available = Convert.ToInt32(dt_available.Rows[0]["AVAILABLE"]);
        //                        }
        //                        else
        //                        {
        //                            available = Convert.ToInt32(dt_available.Rows[0]["TOTAL"]);
        //                        }
        //                        if (available.ToString() != "" && available > 0)
        //                        {
        //                            if (BLL.DeviceInstallation(CATEG_ID, device_ID, DEVICESERIALNO, DEVICENAME, SIMnumber, VehicleNo, VehicleType, VehcleName, ((SMVTS_USERS)Session["USERINFO"]).USERS_ID.ToString(), OrderId, CATEG_NAME, ""))
        //                            {
        //                                data = "true";
        //                                bool status = BLL.updateIMEIandSIM(device_ID, SIMnumber, DealerID, CATEG_NAME);
        //                                bool status2 = BLL.UpdateDeviceWallet(CATEG_ID, DealerID);

        //                            }

        //                            else
        //                            {
        //                                data = "true";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            data = "Sorry Insufficient Balance";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        data = "Please Recharge your wallet";
        //                    }

        //                }
        //                else
        //                {
        //                    data = "SIM  Already Exists.";
        //                }
        //            }
        //            else
        //            {
        //                data = "Device  Already Exists.";
        //                //  BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
        //                //          rtxt_RoleName.Focus();
        //                // return;
        //            }
        //            break;
        //        case "BTN_UPDATE":
        //            if ((count == 0) || (count == 1))
        //            {
        //                _obj_Smvts_Devices.OPERATION = operation.Update;
        //                _obj_Smvts_Devices.DEVICE_ID = Convert.ToInt32(DeviceID);
        //                if (BLL.set_Devices(_obj_Smvts_Devices, dbname, Org_Name_DropDown, ""))
        //                {
        //                    data = "true";
        //                    // BLL.ShowMessage(this, BLL.msg_Updated);
        //                    DataTable dtLastrec = new DataTable();
        //                    dtLastrec = BLL.get_Devices(_obj_Smvts_Devices);
        //                    if (dtLastrec.Rows.Count > 0)
        //                    {
        //                        DeviceID = Convert.ToString(dtLastrec.Rows[0][0]);
        //                    }
        //                }
        //                else
        //                {
        //                    data = "false";
        //                }
        //            }
        //            else
        //            {
        //                data = "Device Name Already Exist.";
        //                // BLL.ShowMessage(this, "Role Already Assigned to Selected Client");
        //                // return;
        //            }
        //            break;

        //        default:
        //            break;
        //    }

        //    return Json(new { data = data });
        //}

        //get Distributors
        public JsonResult getDistributorsConfig(string CategID)
        {
            SMVTS_CATEGORIES obj = new SMVTS_CATEGORIES();

            DataTable dt = new DataTable();

            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                obj.CATEG_ID = 1;
                obj.CATEG_CATETYPE_ID = 1;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 5)
            {
                obj.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                obj.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
            }
            dt = BLL.getDistributorConfig(obj);

            List<SMVTS_CATEGORIES> obj_distr = new List<SMVTS_CATEGORIES>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_distr.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString()
                    });
                }
            }

            return Json(new { data = obj_distr });
        }

        //get Dealers
        public JsonResult getDealersConfig(string CategID)
        {
            SMVTS_CATEGORIES obj = new SMVTS_CATEGORIES();

            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 6)
            {
                obj.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                obj.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
            }
            else
            {
                obj.CATEG_ID = Convert.ToInt32(CategID);
            }

            DataTable dt = new DataTable();
            dt = BLL.getDealersConfig(obj);

            List<SMVTS_CATEGORIES> obj_dealers = new List<SMVTS_CATEGORIES>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_dealers.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),

                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        CATEG_PARENT_ID = Convert.ToInt32(dt.Rows[i]["categ_parent_id"].ToString())
                    });
                }
            }

            return Json(new { data = obj_dealers });
        }

        //get Distributors
        public JsonResult getDistributors(string CategID)
        {
            SMVTS_CATEGORIES obj = new SMVTS_CATEGORIES();

            DataTable dt = new DataTable();

            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                obj.CATEG_ID = 1;
                obj.CATEG_CATETYPE_ID = 1;
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 5)
            {
                obj.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                obj.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
            }
            dt = BLL.getDistributor(obj);

            List<SMVTS_CATEGORIES> obj_distr = new List<SMVTS_CATEGORIES>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_distr.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString()
                    });
                }
            }

            return Json(new { data = obj_distr });
        }

        //get Dealers
        public JsonResult getDealers(string CategID)
        {
            SMVTS_CATEGORIES obj = new SMVTS_CATEGORIES();

            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 6)
            {
                obj.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                obj.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
            }
            else
            {
                obj.CATEG_ID = Convert.ToInt32(CategID);
            }

            DataTable dt = new DataTable();
            dt = BLL.getDealers(obj);

            List<SMVTS_CATEGORIES> obj_dealers = new List<SMVTS_CATEGORIES>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_dealers.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),

                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        CATEG_PARENT_ID = Convert.ToInt32(dt.Rows[i]["categ_parent_id"].ToString())
                    });
                }
            }

            return Json(new { data = obj_dealers });
        }

        //get Customers
        public JsonResult getCustomers(string CategID)

        {
            SMVTS_CATEGORIES obj = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj_cust = new List<SMVTS_CATEGORIES>();
            try
            {
                obj.CATEG_ID = Convert.ToInt32(CategID);
                DataTable dt = new DataTable();
                dt = BLL.getCustomers(obj);


                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj_cust.Add(new SMVTS_CATEGORIES
                        {
                            CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                            CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString()
                        });
                    }
                }
            }
            catch { }

            return Json(new { data = obj_cust });
        }
        [SessionAuthorize]
        public ActionResult AddOrders()
        {
            //Get customer packages
            int categID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt_packages = BLL.Get_Packages(categID);
            List<SMVTS_CUSTOMERPACKAGE> obj3 = new List<SMVTS_CUSTOMERPACKAGE>();
            for (int k = 0; k < dt_packages.Rows.Count; k++)
            {
                obj3.Add(new SMVTS_CUSTOMERPACKAGE
                {
                    PACKAGE_ID = Convert.ToInt32(dt_packages.Rows[k]["PACKAGE_ID"]),
                    PACKAGE_NAME = dt_packages.Rows[k]["PACKAGE_NAME"].ToString()
                });
            }
            ViewBag.packages = obj3;

            //get customer orders

            return View();
        }

        public JsonResult getOrders(string CATEG_ID, string categname, int ParentID)
        {
            SMVTS_ORDERS _obj_Smvts_orders = new SMVTS_ORDERS();
            List<SMVTS_ORDERS> obj = new List<SMVTS_ORDERS>();


            _obj_Smvts_orders.ORDER_CATEGORYID = Convert.ToInt32(CATEG_ID);
            // string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
            string dbname = (ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
            string CategQuery = " SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + categname.Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "") + "' and CATEG_PARENT_ID=" + ParentID + "";

            DataTable dt_categ = new DataTable();

            dt_categ = Dal.ExecuteQueryDB1(CategQuery, dbname);

            _obj_Smvts_orders.ORDER_CATEGORYID = Convert.ToInt32(dt_categ.Rows[0][0].ToString());
            DataTable dt = new DataTable();
            dt = BLL.GetOrders(_obj_Smvts_orders);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_ORDERS
                    {
                        ORDER_ID = Convert.ToInt32(dt.Rows[i]["ORDER_ID"]),
                        ORDER_NAME = dt.Rows[i]["ORDER_NAME"].ToString(),
                        ORDER_STATUS = Convert.ToInt32(dt.Rows[i]["ORDER_STATUS"]),
                        DATE = dt.Rows[i]["createddate"].ToString(),
                        ORDER_PACKAGE = dt.Rows[i]["ORDER_PACKAGE"].ToString(),
                        ORDER_PRICE = Convert.ToDecimal(dt.Rows[i]["ORDER_PRICE"]),
                        startdate = dt.Rows[i]["startdate"].ToString(),
                        enddate = dt.Rows[i]["expdate"].ToString(),
                        categName = dt.Rows[i]["CATEG_NAME"].ToString(),
                        SerialNumber = dt.Rows[i]["DEVICE_SERIALNUMBER"].ToString(),
                        Status = dt.Rows[i]["status"].ToString(),
                        //   ORDER_EXPDATE =Convert.ToDateTime(dt.Rows[i]["ORDER_EXPDATE"]),
                        ExpDate = dt.Rows[i]["ORDER_EXPDATE"].ToString(),

                        DEVICE_ID = (dt.Rows[i]["DEVICE_ID"]).ToString(),
                        ORDER_CATEGORYID = Convert.ToInt32(dt.Rows[i]["ORDER_CATEGORYID"])
                    });
                }
            }
            else
            {

            }
            return Json(new { data = obj });
        }
        //get Acconts
        public JsonResult getAccounts(string CategID)
        {
            SMVTS_CATEGORIES obj = new SMVTS_CATEGORIES();
            obj.CATEG_ID = Convert.ToInt32(CategID);
            DataTable dt = new DataTable();
            dt = BLL.getDealers(obj);

            List<SMVTS_CATEGORIES> obj_distr = new List<SMVTS_CATEGORIES>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_distr.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString()
                    });
                }
            }

            return Json(new { data = obj_distr });
        }

        public JsonResult CreateOrder(string Categ_ID, string PackageId, string Categ_Name, int Number)
        {
            string result = "";
            bool status = false; int available = 0;
            SMVTS_ORDERS obj = new SMVTS_ORDERS();
            int packageid = Convert.ToInt32(PackageId);
            DataTable dt_package = BLL.Get_PackagesBy_Id(packageid);
            DataTable dt_wallet = new DataTable();
            dt_wallet = BLL.GetWalletData(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            if (dt_wallet.Rows[0]["AVAILABLE"].ToString() == "" || dt_wallet.Rows[0]["AVAILABLE"].ToString() == null)
            {
                available = Convert.ToInt32(dt_wallet.Rows[0]["TOTAL"]);
            }
            else
            {
                available = Convert.ToInt32(dt_wallet.Rows[0]["AVAILABLE"]);
            }
            if (available >= Number)
            {
                if (Convert.ToInt32(Number) > 0)
                {
                    for (int i = 0; i < Number; i++)
                    {
                        DateTime dt = DateTime.Now;
                        obj.ORDER_NAME = Categ_Name.Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "").Replace("(SWLP)", "").Replace("(WLP)", "") + "_" + dt.Day + dt.Month + dt.Year + "_" + (i + 1);

                        obj.ORDER_PACKAGE = dt_package.Rows[0]["PACKAGE_NAME"].ToString();
                        obj.ORDER_PRICE = Convert.ToDecimal(dt_package.Rows[0]["PACKAGE_PRICE"]);
                        obj.ORDER_CATEGORYID = Convert.ToInt32(Categ_ID);
                        obj.ORDER_DAYS = Convert.ToInt32(dt_package.Rows[0]["NUM_OF_DAYS"]);
                        obj.ORDER_STATUS = 1;
                        obj.ORDER_CREATEDATE = DateTime.Now;

                        status = BLL.SaveOrder(obj);
                    }
                    if (status == true)
                    {
                        result = "True";
                    }
                    else { result = "False"; }
                }
            }
            else
            {
                result = "Currently only " + available + " Devices available";
            }
            return Json(new { data = result });
        }
        [SessionAuthorize]
        public ActionResult DeviceWallet()
        {


            //Get All Wallet Data

            DataTable dt_wallet = new DataTable();
            dt_wallet = BLL.GetWalletData(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            if (dt_wallet.Rows.Count > 0)
            {
                ViewBag.avaible = dt_wallet.Rows[0]["Available"].ToString();
                ViewBag.allocated = dt_wallet.Rows[0]["ALLOCATED"].ToString();
                ViewBag.total = dt_wallet.Rows[0]["TOTAL"].ToString();
                ViewBag.installed = dt_wallet.Rows[0]["INSTALLED"].ToString();
            }

            //Get Dashboard data
            List<SMVTS_DEVICE_WALLET> obj = new List<SMVTS_DEVICE_WALLET>();
            DataTable dt_dashboard = new DataTable();
            dt_dashboard = BLL.getWalletDataforDashboard(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            if (dt_dashboard.Rows.Count > 0)
            {
                for (int i = 0; i < dt_dashboard.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_DEVICE_WALLET
                    {
                        CATEG_ID = Convert.ToInt32(dt_dashboard.Rows[i]["CATEG_ID"]),
                        CATEG_NAME = dt_dashboard.Rows[i]["categname"].ToString(),
                        PARENT_NAME = dt_dashboard.Rows[i]["parentname"].ToString(),
                        TOTAL = Convert.ToInt32(dt_dashboard.Rows[i]["TOTAL"]),
                        ALLOCATED = Convert.ToInt32(dt_dashboard.Rows[i]["ALLOCATED"]),
                        INSTALLED = Convert.ToInt32(dt_dashboard.Rows[i]["INSTALLED"]),
                        STATUS = Convert.ToInt32(dt_dashboard.Rows[i]["STATUS"]),
                        CREAREDDATE = Convert.ToDateTime(dt_dashboard.Rows[i]["CREAREDDATE"])
                    });
                }
            }

            ViewBag.dashboard = obj;
            return View();
        }

        public JsonResult RechargeWallet(string Categ_ID, string Categ_Name, string Num_Devices)
        {
            SMVTS_DEVICE_WALLET obj = new SMVTS_DEVICE_WALLET();
            int dealerId = 0;
            int catType = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                string CategQuery = " SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + Categ_Name.Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "").Replace("(SWLP)", "").Replace("(WLP)", "") + "'";

                DataTable dt_categ = new DataTable();

                dt_categ = Dal.ExecutePRODDB(CategQuery);

                obj.CATEG_ID = Convert.ToInt32(dt_categ.Rows[0][0]);
                obj.TOTAL = Convert.ToInt32(Num_Devices);
                obj.AVAILABLE = Convert.ToInt32(Num_Devices);
                obj.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                dealerId = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            else
            {
                obj.ALLOCATED = Convert.ToInt32(Num_Devices);
                obj.AVAILABLE = Convert.ToInt32(Num_Devices);
                obj.TOTAL = Convert.ToInt32(Num_Devices);
                dealerId = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                obj.CATEG_ID = Convert.ToInt32(Categ_ID);
            }
            obj.CREAREDDATE = DateTime.Now;

            string status = BLL.SaveDeviceWallet(obj, Categ_Name, dealerId, catType);
            return Json(new { data = status });
        }

        [SessionAuthorize]
        public ActionResult AllUsers()
        {
            SMVTS_CATEGORIES obj_categ = new SMVTS_CATEGORIES();
            SMVTS_USERS obj = new SMVTS_USERS();
            DataTable dt_users = new DataTable();
            int catType = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
            List<SMVTS_USERS> obj_users = new List<SMVTS_USERS>();
            if (catType == 1)
            {
                obj_categ.CATEG_CATETYPE_ID = 1;
                obj_categ.CATEG_ID = 1;
            }
            else
            {
                obj_categ.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
                obj_categ.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            dt_users = BLL.getAllUsers(obj_categ);

            if (dt_users.Rows.Count > 0)
            {
                for (int i = 0; i < dt_users.Rows.Count; i++)
                {
                    obj_users.Add(new SMVTS_USERS
                    {
                        CATEG_NAME = dt_users.Rows[i]["Name"].ToString(),
                        USERS_USERNAME = dt_users.Rows[i]["USERS_USERNAME"].ToString(),
                        USERS_PASSWORD = BLL.Decrypt(dt_users.Rows[i]["USERS_PASSWORD"].ToString()),
                        Mobilenumber = dt_users.Rows[i]["Mobilenumber"].ToString(),
                        Email = dt_users.Rows[i]["Email"].ToString(),
                    });
                }

            }
            ViewBag.users = obj_users;
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
        public JsonResult ApproveWallet()
        {
            int CATEG_ID = 0; string mobile_number = "", status = "";
            int catType = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                CATEG_ID = Convert.ToInt32(1);

            }
            else
            {


                CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }


            DataTable dt_mobile = BLL.getUserMobileNumber(CATEG_ID);
            if (dt_mobile.Rows.Count > 0)
            {
                if (dt_mobile.Rows[0][0].ToString() != "" && dt_mobile.Rows[0][0].ToString() != null)
                {
                    //mobile_number = dt_mobile.Rows[0][0].ToString();
                    mobile_number = "9100777440";

                    Session["MobileNumber"] = mobile_number;
                    status = RequestOTP(mobile_number);

                }
                else
                {
                    status = "Mobile Number Not Exists";
                }
            }
            return Json(new { data = status });
        }

        public string RequestOTP(string phonenumber)
        {

            string responseString = "";
            int value = rng.Next(1000, 9999); //1
            string code = value.ToString("0000");
            string _messageText = "Your One Time Password is " + HttpUtility.UrlEncode(code);
            OTPCodes.Add(new OTPCode { PhoneNumber = phonenumber, Code = code });

            string phone_number = HttpUtility.UrlEncode(phonenumber);


            string url = "http://173.45.76.227/send.aspx?username=contdemo&pass=cont1234&route=trans1&senderid=eTRANO&numbers=" + phone_number + "&message=" + _messageText + "";
            try
            {
                // creating web request to send sms 
                HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(url);
                // execute the request
                HttpWebResponse response = (HttpWebResponse)
                    _createRequest.GetResponse();

                // we will read data via the response stream
                Stream Answer = response.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                string result_value = _Answer.ReadToEnd();

                if (result_value.ToUpper().Trim() != "4")
                {
                    responseString = "sent";
                }
                _Answer.Close();
                response.Close();

            }
            catch (Exception ex)
            {

            }
            return responseString;
        }
        public string verifyotp(string phonenumber, string code)
        {
            string response = "";
            if (OTPCodes.Any(otp => otp.PhoneNumber == phonenumber && otp.Code == code))
            {
                response = "OK";

            }
            else
            {
                response = "NotFound";

            }
            return response;
        }
        public JsonResult VerifyUserOTP(string OTP, string CATEGID)
        {
            string response = "";
            int categid = Convert.ToInt32(CATEGID);
            string mobilenumber = Session["MobileNumber"].ToString();
            string check_otp = verifyotp(mobilenumber, OTP);
            if (check_otp == "OK")
            {
                bool status = BLL.approveWallet(categid);
                if (status == true)
                {
                    response = "Wallet Approved successfully";

                    Session["MobileNumber"] = null;
                }
                else
                {
                    response = "Failed to Approve Wallet";
                }
            }
            else
            {
                response = "Invalid OTP";
            }
            return Json(new { data = response });
        }

        [SessionAuthorize]
        public ActionResult MiningData()
        {

            return View();
        }
        public JsonResult LoadVehicles(string CATEG_ID)
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            DataTable dt = BLL.get_VehiclesForMining(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {

                    DEVICE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_DEVICE_ID"]),
                    DEVICE_NAME = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),

                });
            }
            return Json(new { data = obj });
        }
        public JsonResult AddMahaMiningAssociation(int distID, int dealerID, int custId, string VehicleId)
        {
            string[] vehicles = VehicleId.Split(',');
            bool status = false;
            for (int i = 0; i < vehicles.Length; i++)
            {
                status = BLL.InsertMahaMiningVehicles(distID, dealerID, custId, Convert.ToInt32(vehicles[i]));
            }
            return Json(new { data = status });
        }
        public string LoadMahaMiningData(int distID, int dealerID)
        {
            DataTable dt = BLL.getMahaMiningVehiclesData(distID, dealerID);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
            }

            return serializer.Serialize(rows);
        }

        public JsonResult DeleteMahaMiningAssociation(int ID)
        {
            bool status = BLL.deleteAssociation(ID);
            return Json(new { data = status });
        }
        public ActionResult Certificate(int deviceId)
        {
            DataTable dt_renewal = Dal.ExecuteQuery_Prod("select * FRom SMVTS_DEVICE_RENEWALS WHERE RENEW_DEVICEID=" + deviceId + "");
            if (dt_renewal.Rows.Count > 0)
            {
                return RedirectToAction("generateCertificate", "Home", new { DeviceId = deviceId });
            }
            else
            {



                DataTable dt = Dal.ExecuteQuery_Prod("EXEC USP_SMVTS_DEVICES @Operation = 'select', @DEVICE_ID =" + deviceId + "");
                if (dt.Rows.Count > 0)
                {
                    ViewBag.deviceName = dt.Rows[0]["device_name"].ToString();
                    ViewBag.IMEI = dt.Rows[0]["device_serialnumber"].ToString();
                    ViewBag.SimNumber = dt.Rows[0]["SIM_NUMBER"].ToString();
                }
                DataTable dt_vehicle = Dal.ExecuteQuery_Prod("select * from SMVTS_VEHICLES where VEHICLES_DEVICE_ID=" + deviceId + "");
                ViewBag.vehRegNum = dt_vehicle.Rows[0]["VEHICLES_REGNUMBER"].ToString();
                ViewBag.DeviceID = deviceId;
                ViewBag.FitmentDate = dt_vehicle.Rows[0]["VEHICLES_CREATEDDATE"].ToString();
                ViewBag.InstallDate = dt_vehicle.Rows[0]["VEHICLES_CREATEDDATE"].ToString();
                ViewBag.RenewDate = Convert.ToDateTime(dt_vehicle.Rows[0]["VEHICLES_CREATEDDATE"]).AddDays(365).ToString();

                DataTable dt_customer = Dal.ExecuteQuery_Prod("SELECT * FROM SMVTS_CATEGORIES INNER JOIN SMVTS_DEVICES ON DEVICE_CATEGORY_ID=CATEG_ID WHERE DEVICE_ID=" + deviceId + "");
                ViewBag.CustomerName = dt_customer.Rows[0]["CATEG_NAME"].ToString();
                ViewBag.StateId = dt_customer.Rows[0]["CATEG_STATE_ID"].ToString();
                ViewBag.CityId = dt_customer.Rows[0]["CATEG_CITY_ID"].ToString();
                ViewBag.CategAddress = dt_customer.Rows[0]["CATEG_ADDRESS"].ToString();
                ViewBag.MobileNo = dt_customer.Rows[0]["CATEG_MOBILENUMBER"].ToString();
                ViewBag.PinCode = dt_customer.Rows[0]["CATEG_ZIPCODE"].ToString();


                SMVTS_COUNTRIES _obj_Smvts_Countries = new SMVTS_COUNTRIES();
                _obj_Smvts_Countries.OPERATION = operation.Insert;


                DataTable dt1 = BLL.get_Country(_obj_Smvts_Countries);
                List<SMVTS_COUNTRIES> obj1 = new List<SMVTS_COUNTRIES>();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    obj1.Add(new SMVTS_COUNTRIES
                    {
                        COUNTRY_NAME = dt1.Rows[i]["COUNTRY_NAME"].ToString(),
                        COUNTRY_ID = Convert.ToInt32(dt1.Rows[i]["COUNTRY_ID"]),
                    });
                }
                ViewBag.countries = obj1;
            }
            return View();
        }
        public JsonResult Save_RenewalData(FormCollection form)
        {
            SMVTS_DEVICE_RENEWALS obj = new SMVTS_DEVICE_RENEWALS();
            string fileurl = ""; string result = "";
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    if (i == 0)
                    {
                        file.SaveAs(Server.MapPath("~/IMAGES/Renewals/Vehicles/") + fileName); //File will be saved in application root
                        fileurl = "/IMAGES/Renewals/Vehicles/" + fileName;
                        obj.VEHICLE_IMAGE_URL = fileurl;
                    }
                    else if (i == 1)
                    {
                        file.SaveAs(Server.MapPath("~/IMAGES/Renewals/RC/") + fileName); //File will be saved in application root
                        fileurl = "/IMAGES/Renewals/RC/" + fileName;

                        obj.VEHICLE_RC_IMAGE = fileurl;
                    }
                    else if (i == 2)
                    {
                        file.SaveAs(Server.MapPath("~/IMAGES/Renewals/Device/") + fileName); //File will be saved in application root
                        fileurl = "/IMAGES/Renewals/Device/" + fileName;
                        obj.DEVICE_IMAGE_URL = fileurl;
                    }
                }

                obj.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                obj.CUSTOMER_ADDRESS = form["Address"];
                obj.CUSTOMER_MOBILE = form["MobileNumber"];
                obj.CUSTOMER_NAME = form["CustomerName"];
                obj.CUSTOMER_PINCODE = form["pincode"];

                obj.DEVICE_IMEI = form["DeviceIMEI"];
                obj.DEVICE_NAME = form["DeviceName"];
                obj.FITMENT_DATE = (form["fitment_date"]);
                obj.INSTALL_DATE = (form["installDate"]);
                //  obj.RENEWAL_CERTIFICATE =(form["Renew_Date"]);

                if (form["Renew_Date"] != "")
                {
                    obj.RENEWAL_DATE = (form["Renew_Date"]);
                }
                obj.RENEW_TYPE = Convert.ToInt32(form["RENEW_TYPE"]);
                obj.SIM_NUMBER = form["SimNumber"];
                obj.SIM_OPERATOR = form["Operator"];


                obj.VEH_CHASSIS_NUMBER = form["ChassisNumber"];
                obj.VEH_ENGINE_NUMBER = form["EngineNumber"];
                obj.VEH_MAKER = form["VehMaker"];
                obj.VEH_MODEL = form["VehModel"];
                obj.VEH_MODEL_YEAR = form["year"];
                obj.VEH_REG_DATE = (form["RegDate"]);
                obj.VEH_TYPE = form["VehType"];
                obj.DIVISION = form["division"];
                obj.STATE = form["state"];
                obj.RENEW_DEVICEID = Convert.ToInt32(form["DeviceId"]);
                obj.VEH_REGNUMBER = form["Veh_reg"];

                bool status = BLL.Insert_RenewalData(obj);
                if (status == true)
                {
                    //generate QR Codes
                    string qcode = "http://tranopro.com/Home/FitmentCertificate/" + Convert.ToInt32(form["DeviceId"]) + "";
                    var url = string.Format("http://chart.apis.google.com/chart?cht=qr&chs={1}x{2}&chl={0}", qcode, 500, 500);
                    WebResponse response = default(WebResponse);
                    Stream remoteStream = default(Stream);
                    StreamReader readStream = default(StreamReader);
                    WebRequest request = WebRequest.Create(url);
                    response = request.GetResponse();
                    remoteStream = response.GetResponseStream();
                    readStream = new StreamReader(remoteStream);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
                    img.Save(Server.MapPath("~/IMAGES/QRCodes/") + form["DeviceIMEI"].ToString() + ".png");
                    response.Close();
                    remoteStream.Close();
                    readStream.Close();


                    result = "true";
                }
                else
                {
                    result = "false";
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return Json(new { data = result });
        }

        public JsonResult getAddressByPincode(string Pincode)
        {
            DataTable dt = BLL.getAddress(Pincode);
            SMVTS_ADDRESS obj = new SMVTS_ADDRESS();
            List<SMVTS_ADDRESS> obj_list = new List<SMVTS_ADDRESS>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj_list.Add(new SMVTS_ADDRESS
                    {
                        city_id = Convert.ToInt32(dt.Rows[i]["CITY_CITYID"]),
                        city_name = dt.Rows[i]["CITY_NAME"].ToString(),
                        area_id = Convert.ToInt32(dt.Rows[i]["AREA_ID"]),
                        area_name = dt.Rows[i]["AREA_NAME"].ToString(),
                        state_id = Convert.ToInt32(dt.Rows[i]["STATE_ID"]),
                        state_name = dt.Rows[i]["STATE_NAME"].ToString(),
                        country_id = Convert.ToInt32(dt.Rows[i]["COUNTRY_ID"]),
                        country_name = dt.Rows[i]["COUNTRY_NAME"].ToString(),
                    });
                }

            }
            return Json(new { data = obj_list });
        }
        public JsonResult getDivisions(int StateId)
        {
            DataTable dt = BLL.getDivisions(StateId);
            List<smvts_divisions> obj = new List<smvts_divisions>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new smvts_divisions
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["ID"]),
                        DIVISION_NAME = dt.Rows[i]["DIVISION_NAME"].ToString()
                    });
                }

            }
            return Json(new { data = obj });
        }
        public ActionResult generateCertificate(string DeviceId)
        {
            DataTable dt = BLL.getDateForCertificate(DeviceId);
            if (dt.Rows.Count > 0)
            {
                ViewBag.RTO = dt.Rows[0]["STATE"].ToString();
                ViewBag.division = dt.Rows[0]["DIVISION"].ToString();
                ViewBag.certificateNo = (DateTime.Now.Year + "-" + DateTime.Now.Month + dt.Rows[0]["DEVICE_IMEI"]).ToString().Replace("-", "");
                ViewBag.fitmentDate = dt.Rows[0]["FITMENT_DATE"].ToString();
                ViewBag.renewalDate = dt.Rows[0]["RENEWAL_DATE"].ToString();
                ViewBag.vehicleNo = dt.Rows[0]["RENEWAL_DATE"].ToString();
                ViewBag.chassisNo = dt.Rows[0]["VEH_CHASSIS_NUMBER"].ToString();
                ViewBag.engineNo = dt.Rows[0]["VEH_ENGINE_NUMBER"].ToString();
                ViewBag.vehType = dt.Rows[0]["VEH_TYPE"].ToString();
                ViewBag.vehMake = dt.Rows[0]["VEH_MAKER"].ToString();
                ViewBag.VEH_MODEL = dt.Rows[0]["VEH_MODEL"].ToString();
                ViewBag.regDate = dt.Rows[0]["VEH_REG_DATE"].ToString();
                ViewBag.year = dt.Rows[0]["VEH_MODEL_YEAR"].ToString();
                ViewBag.customerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                ViewBag.address = dt.Rows[0]["CUSTOMER_ADDRESS"].ToString();

                //get Dealer Data
                int categid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                string abc = "";
                abc = " SELECT CATEG_NAME,CATEG_ADDRESS FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_ID='" + categid + "'";

                DataTable dt_categ = new DataTable();
                dt_categ = Dal.ExecuteQuery_Prod(abc);

                ViewBag.dealerName = dt_categ.Rows[0]["CATEG_NAME"].ToString();
                ViewBag.dealerAddress = dt_categ.Rows[0]["CATEG_ADDRESS"].ToString();

                ViewBag.IMEI = dt.Rows[0]["DEVICE_IMEI"].ToString();
                if (Convert.ToInt32(dt.Rows[0]["RENEW_TYPE"]) == 1)
                {
                    ViewBag.Type = "New";
                }
                else if (Convert.ToInt32(dt.Rows[0]["RENEW_TYPE"]) == 2)
                {
                    ViewBag.Type = "Renewal";
                }
                ViewBag.vehNum = dt.Rows[0]["VEH_REGNUMBER"].ToString();
                ViewBag.deviceType = dt.Rows[0]["DEVICE_NAME"].ToString();
                ViewBag.simNum = dt.Rows[0]["SIM_NUMBER"].ToString();
                ViewBag.vehImage = dt.Rows[0]["VEHICLE_IMAGE_URL"].ToString();
                ViewBag.RCImage = dt.Rows[0]["VEHICLE_RC_IMAGE"].ToString();
                ViewBag.deviceImage = dt.Rows[0]["DEVICE_IMAGE_URL"].ToString();
                ViewBag.custMobile = dt.Rows[0]["CUSTOMER_MOBILE"].ToString();
                ViewBag.Date = DateTime.Now.ToShortDateString();
                ViewBag.Time = DateTime.Now.ToShortTimeString();
                ViewBag.QRCode = "http://tranopro.com/IMAGES/QRCodes/" + dt.Rows[0]["DEVICE_IMEI"].ToString() + ".png";

            }
            return View();
        }
        public JsonResult OrderRenewal(int DeviceID, int Package, int CategID, int OrderId)
        {

            string result = "";
            bool status = false; int available = 0;
            SMVTS_ORDERS obj = new SMVTS_ORDERS();
            int packageid = Convert.ToInt32(Package);
            DataTable dt_package = BLL.Get_PackagesBy_Id(packageid);
            DataTable dt_wallet = new DataTable();
            dt_wallet = BLL.GetWalletData(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            if (dt_wallet.Rows[0]["AVAILABLE"].ToString() == "" || dt_wallet.Rows[0]["AVAILABLE"].ToString() == null)
            {
                available = Convert.ToInt32(dt_wallet.Rows[0]["TOTAL"]);
            }
            else
            {
                available = Convert.ToInt32(dt_wallet.Rows[0]["AVAILABLE"]);
            }
            if (available >= 0)
            {

                DateTime dt = DateTime.Now;
                string abc = "";
                abc = " SELECT CATEG_NAME FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_ID='" + CategID + "'";

                DataTable dt_categ = new DataTable();
                dt_categ = Dal.ExecuteQuery_Prod(abc);

                obj.ORDER_NAME = dt_categ.Rows[0]["CATEG_NAME"].ToString().Replace("(C)", "").Trim().Replace("(P)", "").Replace("(O)", "").Replace("(SWLP)", "").Replace("(WLP)", "") + "_" + dt.Day + dt.Month + dt.Year + "_" + (1);

                obj.ORDER_PACKAGE = dt_package.Rows[0]["PACKAGE_NAME"].ToString();
                obj.ORDER_PRICE = Convert.ToDecimal(dt_package.Rows[0]["PACKAGE_PRICE"]);
                obj.ORDER_CATEGORYID = Convert.ToInt32(CategID);
                obj.ORDER_DAYS = Convert.ToInt32(dt_package.Rows[0]["NUM_OF_DAYS"]);
                obj.ORDER_STATUS = 0;
                obj.ORDER_CREATEDATE = DateTime.Now;
                obj.ORDER_STARTDATE = DateTime.Now;
                obj.ORDER_EXPDATE = DateTime.Now.AddDays(Convert.ToInt32(dt_package.Rows[0]["NUM_OF_DAYS"]));
                obj.DEVICE_ID = DeviceID.ToString();
                DataTable dt_orders = BLL.RenewOrder(obj);
                if (dt_orders.Rows.Count > 0)
                {
                    result = "True";
                    int order_Id = Convert.ToInt32(dt_orders.Rows[0]["OrderID"]);
                    bool status2 = Dal.ExecuteNonQueryPROD("Update SMVTS_DEVICES set DEVICE_ORDER_ID=" + order_Id + " where DEVICE_ID=" + DeviceID + "");
                    bool statue3 = Dal.ExecuteNonQueryPROD("Update SMVTS_ORDERS set ORDER_STATUS=2,ORDER_MODIFIEDBY=" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + ",ORDER_MODIFIEDDATE='" + DateTime.Now.ToString() + "' where ORDER_ID=" + OrderId + "");
                }

                else { result = "False"; }

            }
            else
            {
                result = "Currently only " + available + " Devices available";
            }
            return Json(new { data = result });
        }

        public ActionResult ConvertPasswords()
        {
            return View();
        }
        public JsonResult EncryptPassword(int FromID, int ToID, int DBvalue)
        {
            DataTable dt = new DataTable();
            bool status = false;
            if (DBvalue == 1)
            {
                dt = Dal.ExecuteQuery_Prod("select USERS_ID,USERS_PASSWORD from SMVTS_USERS where USERS_ID between " + FromID + " AND " + ToID + "");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string password = BLL.Encrypt(dt.Rows[i]["USERS_PASSWORD"].ToString());
                        int userid = Convert.ToInt32(dt.Rows[i]["USERS_ID"]);
                        status = Dal.ExecuteNonQueryPROD("update SMVTS_USERS set USERS_PASSWORD='" + password + "' where USERS_ID=" + userid + "");
                    }
                }
            }
            else if (DBvalue == 2)
            {
                dt = Dal.ExecuteConfig("select USERS_ID,USERS_PASSWORD from SMVTS_USERS where USERS_ID between " + FromID + " AND " + ToID + "");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string password = BLL.Encrypt(dt.Rows[i]["USERS_PASSWORD"].ToString());
                        int userid = Convert.ToInt32(dt.Rows[i]["USERS_ID"]);
                        status = Dal.ExecuteNonQueryConfig("update SMVTS_USERS set USERS_PASSWORD='" + password + "' where USERS_ID=" + userid + "");
                    }
                }
            }

            return Json(new { data = status });
        }

        public ActionResult UploadDevices()
        {
            return View();
        }
        public ActionResult NEWPage()
        {
            return View();
        }
        public JsonResult OTPgenerate(string orderid)
        {
            string status;
            string numbers = "0123456789";
            Random objrandom = new Random();
            string strrandom = string.Empty;
            //DataTable dt = new DataTable();
            searchdata obj = new searchdata();
            for (int i = 0; i < 4; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                strrandom += temp;

            }
            //obj.OTP = data.OTP.ToString;
            ViewBag.otp = strrandom;
            bool dt = BLL.Update_otp(orderid, strrandom);
            if (dt == true)
            {
                status = "true";
            }
            else
            {
                status = "false";
            }
            // string phonenumber = ConfigurationManager.AppSettings["Mobile"];
            string phonenumber = "9100777440";
            string _messageText = "Your One Time Password is " + HttpUtility.UrlEncode(strrandom) + " For Device Renewal";
            OTPCodes.Add(new OTPCode { PhoneNumber = phonenumber, Code = strrandom });

            string phone_number = HttpUtility.UrlEncode(phonenumber);


            string url = "http://173.45.76.227/send.aspx?username=contdemo&pass=cont1234&route=trans1&senderid=eTRANO&numbers=" + phone_number + "&message=" + _messageText + "";
            try
            {
                // creating web request to send sms 
                HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(url);
                // execute the request
                HttpWebResponse response = (HttpWebResponse)
                    _createRequest.GetResponse();

                // we will read data via the response stream
                Stream Answer = response.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                string result_value = _Answer.ReadToEnd();

                if (result_value.ToUpper().Trim() != "4")
                {
                    status = "sent";
                }
                _Answer.Close();
                response.Close();

            }
            catch (Exception ex)
            {

            }


            return Json(new { data = status });

        }

        public JsonResult UpdateOrderExp(string EndDate, string OTP, string orderid)
        {
            string status;
            searchdata obj = new searchdata();
            obj.EndDate = EndDate.ToString();
            obj.OTP = OTP.ToString();
            bool dt = BLL.UpdatingEndDate(orderid, obj);
            if (dt == true)
            {
                status = "true";
            }
            else
            {
                status = "false";
            }
            return Json(new { data = status });
        }
        public JsonResult getDataByType(int TypeId, string TypeValue)





        {
            DataTable dt_user = new DataTable();
            List<searchdata> obj_search = new List<searchdata>();
            try
            {


                int categID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;

                if (TypeId == 1)
                {
                    if (categID == 1)
                    {
                        dt_user = BLL.GetDataByCustomerName(TypeValue);
                    }
                    else
                    {
                        dt_user = BLL.GetDataByDLRName(TypeValue, categID);
                    }
                }
                if (TypeId == 2)
                {
                    if (categID == 1)
                    {
                        dt_user = BLL.GetDataByIMEI(TypeValue);
                    }
                    else
                    {
                        dt_user = BLL.GetDataByDLRIMEI(TypeValue, categID);
                    }
                }
                if (TypeId == 3)
                {
                    if (categID == 1)
                    {
                        dt_user = BLL.GetDataByVehicleNo(TypeValue);
                    }
                    else
                    {
                        dt_user = BLL.GetDataByDLRVehicleNo(TypeValue, categID);
                    }
                }
                if (TypeId == 4)
                {
                    if (categID == 1)
                    {
                        dt_user = BLL.GetDataByMobileNo(TypeValue);
                    }
                    else
                    {
                        dt_user = BLL.GetDataByDLRMobileNo(TypeValue, categID);
                    }
                }

                if (TypeId == 5)
                {
                    if (categID == 1)
                    {
                        dt_user = BLL.GetDataBySimNumber(TypeValue);
                    }
                    else
                    {
                        dt_user = BLL.GetDataByDLRSimNumber(TypeValue, categID);
                    }
                }




                try
                {

                    if (dt_user.Rows.Count > 0)
                    {

                        for (int j = 0; j < dt_user.Rows.Count; j++)
                        {

                            obj_search.Add(new searchdata
                            {
                                Status = dt_user.Rows[j]["VEHMODELIMAGES_IMAGEURL"].ToString(),
                                Distributorname = dt_user.Rows[j]["PARENT_NAME"].ToString(),
                                DealerName = dt_user.Rows[j]["DEALER_NAME"].ToString(),
                                CustomerName = dt_user.Rows[j]["CUSTOMER_NAME"].ToString(),
                                UserName = dt_user.Rows[j]["USERS_USERNAME"].ToString(),
                                UserPassword = BLL.Decrypt(dt_user.Rows[j]["USERS_PASSWORD"].ToString()),
                                VehicleRegNumebr = dt_user.Rows[j]["VEHICLES_REGNUMBER"].ToString(),
                                MobileNo = dt_user.Rows[j]["CATEG_MOBILENUMBER"].ToString(),
                                IMEI = dt_user.Rows[j]["DEVICE_SERIALNUMBER"].ToString(),
                                SimNumber = dt_user.Rows[j]["SIM_NUMBER"].ToString(),
                                SIMOperator = dt_user.Rows[j]["SIM_OPERATORNAME"].ToString(),
                                StartDate = dt_user.Rows[j]["ORDER_StartDate"].ToString(),
                                EndDate = dt_user.Rows[j]["ORDER_ExpDate"].ToString(),
                                DEVICE_ID = Convert.ToInt32(dt_user.Rows[j]["DEVICE_ID"].ToString()),
                                CATEG_ID = Convert.ToInt32(dt_user.Rows[j]["CATEG_ID"].ToString()),
                                verified = Convert.ToInt32(dt_user.Rows[j]["verified"].ToString()),
                                Location = dt_user.Rows[j]["PLACE"].ToString(),
                                LastUpdated = dt_user.Rows[j]["TRIPDATE"].ToString(),
                                Device_Name = dt_user.Rows[j]["DEVICE_NAME"].ToString(),
                                order_id = dt_user.Rows[j]["ORDER_ID"].ToString()
                            });

                        }
                    }

                }
                catch (Exception ex)
                {
                    string msg = ex.ToString();
                }
                var jsonData = new
                {
                    data = obj_search
                };
                var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { data = 1 });
            }

        }
        public JsonResult get_device_shift(int DeviceID)
        {

            List<SHIFT_DEVICE> obj = new List<SHIFT_DEVICE>();
            try
            {
                DataTable dt = BLL.get_shift_device_details(DeviceID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj.Add(new SHIFT_DEVICE
                        {
                            CustomerName = dt.Rows[i]["CATEG_NAME"].ToString(),
                            VehicleNumber = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                            DevicenNumber = dt.Rows[i]["DEVICE_SERIALNUMBER"].ToString(),
                            SIMNumber = dt.Rows[i]["SIM_NUMBER"].ToString(),
                            DealerName = dt.Rows[i]["DealerName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { data = obj });
        }
        public JsonResult shift_device(int CategID, int DeviceID, string Categ_Name)
        {
            string DealerName = Categ_Name.Split('_')[1];
            bool status = BLL.shift_device(CategID, DeviceID, Categ_Name.Split('_')[0], DealerName);
            return Json(new { data = status });
        }
        public ActionResult frm_search()
        {
            //Load Device Names
            SMVTS_DEVICETYPES obj_types = new SMVTS_DEVICETYPES();
            DataTable dt_types = new DataTable();
            List<SMVTS_DEVICETYPES> obj_alltypes = new List<SMVTS_DEVICETYPES>();
            dt_types = BLL.GetDeviceNames();
            if (dt_types.Rows.Count > 0)
            {
                for (int k = 0; k < dt_types.Rows.Count; k++)
                {
                    obj_alltypes.Add(new SMVTS_DEVICETYPES
                    {
                        ID = Convert.ToInt32(dt_types.Rows[k]["ID"]),
                        DEVICE_TYPENAME = dt_types.Rows[k]["DEVICE_TYPENAME"].ToString()
                    });
                }
            }

            ViewBag.deviceTypes = obj_alltypes;
            return View();
        }
        public ActionResult getLoginUser(string Key, string Username, string Password)
        {

            string result = "";

            try
            {
                DataTable dt_key = BLL.getIOSkey(Key);

                if (dt_key.Rows.Count > 0)
                {
                    List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
                    var data = BLL.getDatabaseForUser(Username, Password);

                    if (data.Rows.Count > 0)
                    {
                        string CompanyName = data.Rows[0][0].ToString();
                        string dbname = data.Rows[0][1].ToString();
                        var data_user = BLL.get_User(CompanyName, Username, dbname);
                        string MSG = "false";
                        SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();
                        if (data_user != null)
                        {
                            _obj_Smvts_Users = data_user;
                            string cpw = BLL.Decrypt(_obj_Smvts_Users.USERS_PASSWORD);
                            if (cpw == Password)
                            {
                                MSG = "true";
                                _obj_Smvts_Users.USERS_DBNAME = dbname;
                                //  if (rememberChkBox == "on")
                                // {
                                HttpCookie cookie = new HttpCookie("Credentials");

                                cookie.Values.Add("Username", Username);// your x value

                                cookie.Values.Add("Password", Password); // your y value

                                cookie.Values.Add("CompanyName", CompanyName);

                                cookie.Expires = DateTime.Now.AddMonths(6); // --------> cookie.Expires is the property you can set timeout

                                // HttpContext.current.Response.AppendCookie(cookie);
                                HttpContext.Response.Cookies.Set(cookie);
                                //user name and password binded on textbox that purpose
                                HttpCookie cookie1 = new HttpCookie("namepassword");
                                cookie1.Values.Add("user_N", Username);
                                cookie1.Values.Add("user_P", Password);
                                cookie1.Values.Add("user_C", CompanyName);
                                cookie1.Expires = DateTime.Now.AddMonths(6);
                                HttpContext.Response.Cookies.Set(cookie1);

                                //  }
                                //else
                                //{
                                //    HttpCookie checkCookie = Request.Cookies.Get("namepassword");
                                //    if (checkCookie != null)
                                //    {
                                //        checkCookie.Expires = DateTime.Now.AddDays(-10);
                                //        checkCookie.Value = null;
                                //        HttpContext.Response.Cookies.Set(checkCookie);
                                //    }
                                //}


                                result = "http://www.tranopro.com/HOME/frm_GridTrack_Distance";
                                return RedirectToAction("frm_GridTrack_Distance", "Home");
                            }
                        }
                        else
                        {

                            result = "Invalid UserName/Password";
                        }

                    }
                    else
                    {

                        result = "Invalid UserName/Password";
                    }
                }
                else
                {

                    result = "Invalid UserName/Password";
                }

                var jsonData = new
                {
                    data = result
                };
                var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception ex)
            {
                var jsonData = new
                {
                    data = ex.ToString()
                };

                var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }


        }

        public ActionResult FitmentCertificate(string id)
        {
            string DeviceId = id;
            DataTable dt = BLL.getDateForCertificate(DeviceId);
            if (dt.Rows.Count > 0)
            {
                ViewBag.RTO = dt.Rows[0]["STATE"].ToString();
                ViewBag.division = dt.Rows[0]["DIVISION"].ToString();
                ViewBag.certificateNo = (DateTime.Now.Year + "-" + DateTime.Now.Month + dt.Rows[0]["DEVICE_IMEI"]).ToString().Replace("-", "");
                ViewBag.fitmentDate = dt.Rows[0]["FITMENT_DATE"].ToString();
                ViewBag.renewalDate = dt.Rows[0]["RENEWAL_DATE"].ToString();
                ViewBag.vehicleNo = dt.Rows[0]["RENEWAL_DATE"].ToString();
                ViewBag.chassisNo = dt.Rows[0]["VEH_CHASSIS_NUMBER"].ToString();
                ViewBag.engineNo = dt.Rows[0]["VEH_ENGINE_NUMBER"].ToString();
                ViewBag.vehType = dt.Rows[0]["VEH_TYPE"].ToString();
                ViewBag.vehMake = dt.Rows[0]["VEH_MAKER"].ToString();
                ViewBag.VEH_MODEL = dt.Rows[0]["VEH_MODEL"].ToString();
                ViewBag.regDate = dt.Rows[0]["VEH_REG_DATE"].ToString();
                ViewBag.year = dt.Rows[0]["VEH_MODEL_YEAR"].ToString();
                ViewBag.customerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                ViewBag.address = dt.Rows[0]["CUSTOMER_ADDRESS"].ToString();

                //get Dealer Data
                //int categid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                string abc = "";
                abc = "SELECT CATEG_NAME,CATEG_ADDRESS FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_ID=(select CATEG_PARENT_ID from smvts_categories c inner join smvts_devices d on c.categ_id=d.DEVICE_CATEGORY_ID where device_id='" + DeviceId + "')";

                DataTable dt_categ = new DataTable();
                dt_categ = Dal.ExecuteQuery_Prod(abc);

                ViewBag.dealerName = dt_categ.Rows[0]["CATEG_NAME"].ToString();
                ViewBag.dealerAddress = dt_categ.Rows[0]["CATEG_ADDRESS"].ToString();

                ViewBag.IMEI = dt.Rows[0]["DEVICE_IMEI"].ToString();
                if (Convert.ToInt32(dt.Rows[0]["RENEW_TYPE"]) == 1)
                {
                    ViewBag.Type = "New";
                }
                else if (Convert.ToInt32(dt.Rows[0]["RENEW_TYPE"]) == 2)
                {
                    ViewBag.Type = "Renewal";
                }
                ViewBag.vehNum = dt.Rows[0]["VEH_REGNUMBER"].ToString();
                ViewBag.deviceType = dt.Rows[0]["DEVICE_NAME"].ToString();
                ViewBag.simNum = dt.Rows[0]["SIM_NUMBER"].ToString();
                ViewBag.vehImage = dt.Rows[0]["VEHICLE_IMAGE_URL"].ToString();
                ViewBag.RCImage = dt.Rows[0]["VEHICLE_RC_IMAGE"].ToString();
                ViewBag.deviceImage = dt.Rows[0]["DEVICE_IMAGE_URL"].ToString();
                ViewBag.custMobile = dt.Rows[0]["CUSTOMER_MOBILE"].ToString();
                ViewBag.Date = DateTime.Now.ToShortDateString();
                ViewBag.Time = DateTime.Now.ToShortTimeString();
                ViewBag.QRCode = "http://tranopro.com/IMAGES/QRCodes/" + dt.Rows[0]["DEVICE_IMEI"].ToString() + ".png";
                /// ViewBag.QRCode = "http://localhost:49981//IMAGES/QRCodes/" + dt.Rows[0]["DEVICE_IMEI"].ToString() + ".png";
            }
            return View();
        }
        public ActionResult OdishaMining()
        {
            return View();
        }


        public JsonResult Odissa_Mining(int distID, int dealerID, int custId, string VehicleId)
        {
            DataTable dt = Dal.ExecuteQuery_Prod("select * from SMVTS_USERS where Users_id=" + custId + "");
            string User_devices = dt.Rows[0]["USERS_DEVICE_IDS"].ToString() + "," + VehicleId;
            if (VehicleId != "")
            {

                bool status = BLL.Update_Odissa_mining(custId, User_devices);

                return Json(new { data = status });
            }
            else
            {
                bool status = false;
                return Json(new { data = status });
            }

        }
        public ActionResult SendMessages()
        {
            return View();
        }
        public ActionResult InstallationReport()
        {

            return View();
        }

        public JsonResult getInstallationsData(string distributorID, string dealerID, string CUSTID, string StartDate, string EndDate)
        {
            List<Expiry_data> obj_Expiry = new List<Expiry_data>();
            try
            {


                List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
                SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();

                if (Session["USERINFO"] == null)
                {

                }
                SMVTS_CATEGORIES obj_categ = new SMVTS_CATEGORIES();
                if (distributorID == "0" && dealerID == "0" && CUSTID == "0")
                {
                    DataTable dt = BLL.Get_Expiry_data(StartDate, EndDate);

                    for (int j = 0; j < dt.Rows.Count; j++)

                    {
                        obj_Expiry.Add(new Expiry_data
                        {

                            Distributorname = dt.Rows[j]["DISTRIBUTOR_NAME"].ToString(),
                            DealerName = dt.Rows[j]["DEALER_NAME"].ToString(),
                            CustomerName = dt.Rows[j]["CUSTOMER_NAME"].ToString(),
                            DeviceName = dt.Rows[j]["DEVICE_NAME"].ToString(),
                            VehicleRegNumebr = dt.Rows[j]["VEHICLES_REGNUMBER"].ToString(),
                            CustomerMobile = dt.Rows[j]["CATEG_MOBILENUMBER"].ToString(),
                            IMEI = dt.Rows[j]["DEVICE_SERIALNUMBER"].ToString(),
                            SimNumber = dt.Rows[j]["SIM_NUMBER"].ToString(),

                            StartDate = dt.Rows[j]["ORDER_StartDate"].ToString(),
                            EndDate = dt.Rows[j]["ORDER_ExpDate"].ToString()
                        });

                    }

                }

                else
                {
                    DataTable dt_user = BLL.getCustomerUserID(distributorID, dealerID, CUSTID);

                    if (dt_user.Rows.Count > 0)
                    {

                        for (int j = 0; j < dt_user.Rows.Count; j++)
                        {
                            string USERID = dt_user.Rows[j]["USERID"].ToString();
                            DataTable dt = BLL.get_GridTrackDistancePro(_obj_Smvts_GridTrack, USERID);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {

                                int spe = 0;
                                if (dt.Rows[i]["SPEED"] == DBNull.Value)
                                {
                                    spe = 0;
                                }
                                else
                                {
                                    spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                                }
                                DateTime Exp_Date = new DateTime();
                                DataTable dt_expiry = BLL.check_order_expiry(Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()));
                                if (dt_expiry.Rows.Count > 0)
                                {
                                    if (dt_expiry.Rows[0][0].ToString() != "" && dt_expiry.Rows[0][0].ToString() != null)
                                    {
                                        Exp_Date = Convert.ToDateTime(dt_expiry.Rows[0][0]);
                                    }


                                }
                                if (Exp_Date >= Convert.ToDateTime(StartDate) && Exp_Date <= Convert.ToDateTime(EndDate))
                                {
                                    //obj.Add(new SMVTS_GRIDTRACK
                                    //{
                                    //    SPEED = spe,
                                    //    IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                                    //    DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                                    //    VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                                    //    DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                                    //    PLACE = dt.Rows[i]["PLACE"].ToString(),
                                    //    DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                                    //    DURATION = dt.Rows[i]["duration"].ToString(),
                                    //    DKM = dt.Rows[i]["distance_day"].ToString(),
                                    //    TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                                    //    LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                                    //    LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                                    //    COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                                    //    DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                                    //    PARENT_NAME = dt_user.Rows[j]["PARENT_NAME"].ToString(),
                                    //    DEALER_NAME = dt_user.Rows[j]["DEALER_NAME"].ToString(),
                                    //    CUSTOMER_NAME = dt_user.Rows[j]["CUSTOMER_NAME"].ToString(),
                                    //    Lastdate = dt.Rows[i]["DATE"].ToString(),
                                    //    VEHICLE_IMAGE = dt.Rows[i]["VEHMODELIMAGES_IMAGEURL"].ToString(),
                                    //    SIM_MNO = dt.Rows[i]["SIM_NUMBER"].ToString(),
                                    //    IMEI = dt.Rows[i]["IMEI"].ToString(),
                                    //    DeviceName = dt.Rows[i]["DEVICENAME"].ToString(),
                                    //    InstallDate = dt.Rows[i]["STARTDATE"].ToString(),
                                    //    ExpDate = dt.Rows[i]["EXPDATE"].ToString()

                                    //});
                                    obj_Expiry.Add(new Expiry_data
                                    {

                                        Distributorname = dt_user.Rows[j]["PARENT_NAME"].ToString(),
                                        DealerName = dt_user.Rows[j]["DEALER_NAME"].ToString(),
                                        CustomerName = dt_user.Rows[j]["CUSTOMER_NAME"].ToString(),

                                        VehicleRegNumebr = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                                        DeviceName = dt.Rows[i]["DEVICENAME"].ToString(),
                                        IMEI = dt.Rows[i]["IMEI"].ToString(),
                                        SimNumber = dt.Rows[i]["SIM_NUMBER"].ToString(),

                                        StartDate = dt.Rows[i]["STARTDATE"].ToString(),
                                        EndDate = dt.Rows[i]["EXPDATE"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                string error = EX.ToString();
            }

            var jsonData = new
            {
                data = obj_Expiry
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult LoginNew()
        {
            string url = Request.Url.Host;
            //string url = "www.tranopro.com";
            if (url.ToLower() == "tranopro.com")
            {
                url = "www." + url;
            }
            Session["DomainName"] = url;
            //get WLP Detials

            DataTable dt = new DataTable();
            dt = BLL.getWLPDetails(url);
            if (dt.Rows.Count > 0)
            {
                ViewBag.logo = dt.Rows[0]["LOGOS_URL"].ToString();
                ViewBag.telephone = dt.Rows[0]["CATEG_TELEPHONE"].ToString();
                ViewBag.Phonenumber = dt.Rows[0]["CATEG_MOBILENUMBER"].ToString();
                ViewBag.email = dt.Rows[0]["CATEG_EMAILID"].ToString();
            }
            HttpCookie cookieforbind = Request.Cookies.Get("namepassword");
            if (cookieforbind != null)
            {
                ViewBag.u_P = cookieforbind["user_P"].ToString();
                ViewBag.u_N = cookieforbind["user_N"].ToString();
            }
            // var username = Session["UserName"];
            // var password = Session["Password"];
            HttpCookie LoginCookie = Request.Cookies.Get("Credentials");
            if (LoginCookie != null)
            {
                string x = LoginCookie["Username"].ToString();
                string y = LoginCookie["Password"].ToString();
                string z = LoginCookie["CompanyName"].ToString();
                ViewBag.user_N = x;
                ViewBag.user_P = y;
                //BLL obj = new BLL();
                var data = BLL.getDatabase(z);
                Session["DBNAME"] = data;
                get_User(z, x, "on", y);
                return RedirectToAction("frm_GridTrack_Distance", "Home");
            }
            ViewBag.url = url;
            return View();

        }
        public ActionResult Installation(string ID)
        {
            ////System.Text.StringBuilder linesSB = new System.Text.StringBuilder();
            ////Session["Querystring"] = ID;
            ////ViewBag.Querystring = ID;
            SMVTS_CATEGORIES _obj_Smvts_categories = new SMVTS_CATEGORIES();
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            List<SMVTS_CATEGORIES> obj5 = new List<SMVTS_CATEGORIES>();
            List<SMVTS_COUNTRIES> obj1 = new List<SMVTS_COUNTRIES>();
            //List<SMVTS_SALESREPRESENTATIVE> obj2 = new List<SMVTS_SALESREPRESENTATIVE>();
            SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();

            SMVTS_COUNTRIES _obj_Smvts_Countries = new SMVTS_COUNTRIES();
            _obj_Smvts_Countries.OPERATION = operation.Insert;

            //SMVTS_SALESREPRESENTATIVE _obj_smvts_salesrep = new SMVTS_SALESREPRESENTATIVE();
            DataTable dt1 = BLL.get_Country(_obj_Smvts_Countries);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj1.Add(new SMVTS_COUNTRIES
                {
                    COUNTRY_NAME = dt1.Rows[i]["COUNTRY_NAME"].ToString(),
                    COUNTRY_ID = Convert.ToInt32(dt1.Rows[i]["COUNTRY_ID"]),
                });
            }
            ViewBag.countries = obj1;
            ////_obj_smvts_salesrep.OPERATION = operation.SelectAll;
            ////DataTable dt4 = BLL.get_salesrep_dropdown(_obj_smvts_salesrep);

            //for (int j = 0; j < dt4.Rows.Count; j++)
            //{
            //    obj2.Add(new SMVTS_SALESREPRESENTATIVE
            //    {
            //        SALESREP_FULLNAME = dt4.Rows[j]["SALESREP_FULLNAME"].ToString(),
            //        SALESREP_USER_ID = Convert.ToInt32(dt4.Rows[j]["SALESREP_USER_ID"]),
            //    });
            //}
            //ViewBag.sales = obj2;

            _obj_Smvts_categories.OPERATION = operation.Empty;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID != 1)
            {
                _obj_Smvts_categories.OPERATION = operation.Select;
                _obj_Smvts_categories.CATEG_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            DataTable dt5 = BLL.get_Categories(_obj_Smvts_categories);

            for (int m = 0; m < dt5.Rows.Count; m++)
            {
                obj5.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = dt5.Rows[m]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt5.Rows[m]["CATEG_ID"]),
                });
            }
            ViewBag.partners = obj5;
            //Get customer packages
            int categ_id = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt_packages = BLL.Get_Packages(categ_id);
            List<SMVTS_CUSTOMERPACKAGE> obj3 = new List<SMVTS_CUSTOMERPACKAGE>();
            for (int k = 0; k < dt_packages.Rows.Count; k++)
            {
                obj3.Add(new SMVTS_CUSTOMERPACKAGE
                {
                    PACKAGE_ID = Convert.ToInt32(dt_packages.Rows[k]["PACKAGE_ID"]),
                    PACKAGE_NAME = dt_packages.Rows[k]["PACKAGE_NAME"].ToString()
                });
            }
            ViewBag.packages = obj3;

            //Load Device Names
            SMVTS_DEVICETYPES obj_types = new SMVTS_DEVICETYPES();
            DataTable dt_types = new DataTable();
            List<SMVTS_DEVICETYPES> obj_alltypes = new List<SMVTS_DEVICETYPES>();
            dt_types = BLL.GetDeviceNames();
            if (dt_types.Rows.Count > 0)
            {
                for (int k = 0; k < dt_types.Rows.Count; k++)
                {
                    obj_alltypes.Add(new SMVTS_DEVICETYPES
                    {
                        ID = Convert.ToInt32(dt_types.Rows[k]["ID"]),
                        DEVICE_TYPENAME = dt_types.Rows[k]["DEVICE_TYPENAME"].ToString()
                    });
                }
            }

            ViewBag.deviceTypes = obj_alltypes;
            //Get Category Type
            List<CategType> obj_categtype = new List<CategType>();
            if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
            {
                obj_categtype.Add(new CategType { Type = "Distributor", Value = 5 });
                obj_categtype.Add(new CategType { Type = "Dealer", Value = 6 });
                obj_categtype.Add(new CategType { Type = "Customer", Value = 3 });
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 5)
            {
                obj_categtype.Add(new CategType { Type = "Dealer", Value = 6 });
                obj_categtype.Add(new CategType { Type = "Customer", Value = 3 });
            }
            else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 6)
            {
                obj_categtype.Add(new CategType { Type = "Customer", Value = 3 });
            }
            ViewBag.categtype = obj_categtype;
            try
            {
                string strQryString = ID;
                DataTable DT = new DataTable();
                _obj_Smvts_Users.USERS_DBNAME = Session["DBNAME"].ToString();
                switch (strQryString)
                {
                    case "PARTNERS":
                        if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
                        {
                            _obj_Smvts_categories.OPERATION = operation.SelectPartners;
                            _obj_Smvts_categories.CATEG_PARENT_ID = 1;
                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);

                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES
                                {
                                    PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                });
                            }

                        }
                        else
                        {
                            _obj_Smvts_categories.OPERATION = operation.SelectPartnersAdmin;
                            //_obj_Smvts_categories.CATEG_CATETYPE_ID = 4;
                            _obj_Smvts_categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);

                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);

                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES
                                {
                                    PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                });
                            }
                        }
                        ViewBag.partnersandclients = obj;
                        break;
                    case "CLIENTS":

                        if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1) //Org
                        {
                            _obj_Smvts_categories.OPERATION = operation.SelectClients;

                            _obj_Smvts_categories.CATEG_PARENT_ID = 1;
                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);

                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES
                                {
                                    //PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                });
                            }
                            ViewBag.partnersandclients = obj;
                        }
                        else //Partner
                        {
                            //_obj_Smvts_categories.CATEG_CATETYPE_ID = 3;
                            _obj_Smvts_categories.CATEG_PARENT_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                            _obj_Smvts_categories.OPERATION = operation.SelectClientsAdmin;
                            DT = BLL.GetCategoriesForUserCreation(_obj_Smvts_categories);
                            for (int k = 0; k < DT.Rows.Count; k++)
                            {
                                obj.Add(new SMVTS_CATEGORIES
                                {
                                    // PARENT_NAME = DT.Rows[k]["PARENT_NAME"].ToString(),
                                    CATEG_NAME = DT.Rows[k]["CATEG_NAME"].ToString(),
                                    CATEG_CATETYPE_ID = Convert.ToInt32(DT.Rows[k]["CATEG_CATETYPE_ID"]),
                                    CATEG_PARENT_ID = Convert.ToInt32(DT.Rows[k]["CATEG_PARENT_ID"]),
                                    CATEG_CONTACTPERSON = DT.Rows[k]["CATEG_CONTACTPERSON"].ToString(),
                                    CATEG_MOBILENUMBER = DT.Rows[k]["CATEG_MOBILENUMBER"].ToString(),
                                    CATEG_STATUS = Convert.ToString(DT.Rows[k]["CATEG_STATUS"]),
                                    CATEG_ID = Convert.ToInt32(DT.Rows[k]["CATEG_ID"]),
                                });
                            }
                            ViewBag.partnersandclients = obj;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }

            return View();
        }

        public JsonResult save_customer(FormCollection form)
        {
            string status = "";
            try
            {

                SMVTS_ORDERS obj_orders = new SMVTS_ORDERS();
                // int packageid = Convert.ToInt32(PackageId);
                //  DataTable dt_package = BLL.Get_PackagesBy_Id_For_API(1);
                DateTime date = DateTime.Now;
                ////  obj_orders.ORDER_NAME = (form["txtcustname"]) + DateTime.Now.ToShortDateString();
                //  obj_orders.ORDER_PACKAGE = dt_package.Rows[0]["PACKAGE_NAME"].ToString();
                // obj_orders.ORDER_PRICE = Convert.ToDecimal(dt_package.Rows[0]["PACKAGE_PRICE"]) + Convert.ToDecimal(dt_package.Rows[0]["GST"]);
                //obj_orders.ORDER_STARTDATE = DateTime.Now;
                // obj_orders.ORDER_EXPDATE = DateTime.Now.AddDays(Convert.ToInt32(dt_package.Rows[0]["NUM_OF_DAYS"]));
                //   obj_orders.ORDER_CREATEDATE = DateTime.Now;
                // bool status = BLL.SaveOrder(obj);

                string db_name = "Aor2T0SveXPKBbMcC7tSjrv+vNImP9nNMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rznBz47OI45T+I00H+mKDZtnMOdeZlpCzckukpOKqMbXjlktkV49jJ";
                int Cust_Id = 0;
                // int dealerId = 0;

                //check for Dealer
                string abc = "";
                abc = "SELECT * FROM SMVTS_CATEGORIES(NOLOCK) WHERE categ_ID=" + form["DEALER_ID"] + " AND CATEG_CATETYPE_ID=6";
                DataTable dt_dealer = new DataTable();
                dt_dealer = Dal.ExecuteQuery_Prod(abc);
                if (dt_dealer.Rows.Count > 0)
                {
                    string dealer_Name = dt_dealer.Rows[0]["CATEG_NAME"].ToString();
                    int dealer_id = Convert.ToInt32(dt_dealer.Rows[0]["CATEG_ID"]);
                    //check customer 
                    DataTable dt_customer = BLL.Get_Customer((form["CATEG_NAME"]), dealer_id);
                    if (dt_customer.Rows.Count > 0)
                    {
                        Cust_Id = Convert.ToInt32(dt_customer.Rows[0]["CATEG_ID"].ToString());
                        status = "Customer Already exists";
                    }

                    else
                    {


                        //Insert new Client

                        SMVTS_CATEGORIES obj_cust = new SMVTS_CATEGORIES();


                        obj_cust.CATEG_CATETYPE_ID = 3;
                        obj_cust.CATEG_ADDRESS = (form["CATEG_ADDRESS"]);
                        obj_cust.CATEG_CITY_ID = Convert.ToInt32(form["CATEG_CITY_id"]);
                        obj_cust.CATEG_CONTACTPERSON = "Prashanth";
                        obj_cust.CATEG_COUNTRY_ID = Convert.ToInt32(form["CATEG_COUNTRY_ID"]);
                        obj_cust.CATEG_DBNAME = db_name;
                        obj_cust.CATEG_DESC = (form["txtcustname"]);
                        obj_cust.CATEG_EMAILID = (form["CATEG_EMAILID"]);
                        obj_cust.CATEG_MOBILENUMBER = (form["CATEG_MOBILENUMBER"]);
                        obj_cust.CATEG_NAME = (form["CATEG_NAME"]);
                        obj_cust.CATEG_NOOFUSERS = 10;

                        obj_cust.CATEG_PARENT_ID = dealer_id;
                        obj_cust.CATEG_PRODNAME = "SMVTS_PROD01";
                        obj_cust.CATEG_STATE_ID = Convert.ToInt32(form["CATEG_STATE_ID"]);
                        obj_cust.CATEG_STATUS = "1";
                        obj_cust.CATEG_UOMSPEED = "Km/Hr";
                        obj_cust.CATEG_UOMVOLUME = "Litre";
                        obj_cust.CATEG_WEBSITENAME = "www.tranopro.com";
                        obj_cust.CATEG_ZIPCODE = (form["CATEG_ZIPCODE"]);

                        //  DataTable cust_status = BLL.insert_new_customer(obj_cust, db_name, PackageId, obj_orders.ORDER_EXPDATE);
                        //  Cust_Id = Convert.ToInt32(cust_status.Rows[0]["CustID"]);
                        string password = BLL.Encrypt((form["CATEG_MOBILENUMBER"]).ToString());

                        Cust_Id = BLL.UploadCustomers(obj_cust, "", dealer_Name, "", "", 3, password);
                        if (Cust_Id != 0)
                        {
                            status = "Saved Successfully";
                        }
                        else
                        {
                            status = "Failed";
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }


            bool CustId = false;
            return Json(new { data = status });
        }


        public JsonResult send_messages(string categ_ids, string MESSAGE)
        {
            bool status = false;
            string[] categ = categ_ids.Split(',');
            if (categ.Length > 0)
            {
                for (int i = 0; i < categ.Length; i++)
                {
                    if (categ[i] != null && categ[i] != "")
                    {
                        DataTable dt_user = Dal.ExecuteQuery_Prod("select USERS_ID from SMVTS_USERS where Users_CATEGORY_ID=" + categ[i] + "");

                        status = BLL.Insert_Broadcasting(Convert.ToInt32(categ[i]), Convert.ToInt32(dt_user.Rows[0][0]), MESSAGE);
                    }

                }

            }

            return Json(new { data = status });

        }

        //public JsonResult SaveNewSim(string OTP, string sim_operator, string sim_number, string Reason, string mobilenumber, string DeviceID, string CategID, string OldSIMNumber)
        //{
        //    bool status = false;
        //    string check_otp = verifyotp(mobilenumber, OTP);
        //    if (check_otp == "OK")
        //    {
        //        //status = BLL.insert_newSIM(sim_number, sim_operator, Reason, DeviceID, CategID);
        //        status = BLL.changeSIM(sim_number, sim_operator, Reason, DeviceID, CategID, OldSIMNumber);
        //    }
        //    else
        //    {
        //        status = false;
        //    }
        //    return Json(new { data = status });
        //}
        public JsonResult SaveNewSim(string password, string sim_number, string Reason, string DeviceID, string CategID, string OldSIMNumber, string cust_name, int sim_id)
        {
            string result = "";
            bool status = false;
            if (password == "Ctpl@1216")
            {

                //status = BLL.insert_newSIM(sim_number, sim_operator, Reason, DeviceID, CategID);
                status = BLL.changeSIM(sim_number, "", Reason, DeviceID, CategID, OldSIMNumber);
                if (status == true)
                {
                    result = "true";
                    bool status2 = BLL.updateSimStatus(OldSIMNumber, sim_id, cust_name, Reason);
                }
                else
                {
                    result = "false";
                }
            }
            else
            {
                result = "Invalid Password";
            }
            return Json(new { data = result });
        }
        public ActionResult SearchByFields()
        {
            return View();
        }
        //public JsonResult VerifySim(string sim_number, string operatorname, string serialnumber)
        //{
        //    bool status = false;
        //    status = BLL.verifysim(sim_number, operatorname, serialnumber);
        //    return Json(new { data = status });
        //}
        public JsonResult VerifySim(string sim_number, string DealerName, string customerName)
        {
            bool status = false;
            string result = "";
            DataTable dt_categ = new DataTable();
            dt_categ = Dal.ExecuteQuery("select CATEG_ID from smvts_categories where CATEG_NAME='" + DealerName + "'");
            if (dt_categ.Rows.Count > 0)
            {
                DataTable dt_sim = new DataTable();
                dt_sim = BLL.getSimsByNumber(sim_number);
                if (dt_sim.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt_sim.Rows[0]["SIM_STATUS"]) == 0 || Convert.ToInt32(dt_sim.Rows[0]["SIM_STATUS"]) == 1)
                    {
                        status = BLL.verifysim(sim_number, dt_categ.Rows[0][0].ToString(), Convert.ToInt32(dt_sim.Rows[0]["ID"]), customerName);
                        if (status == true)
                        {
                            result = "true";
                        }
                        else
                        {
                            result = "false";
                        }
                    }
                    else
                    {
                        result = "Sorry the SIM is already Used or Terminated Please check once.";
                    }
                }
                else
                {
                    result = "No SIM Found in Stock.Please Add it in Stock first";
                }
            }
            else
            {
                result = "No Dealer Found";
            }
            return Json(new { data = result });
        }
        public JsonResult SaveNewIMEI(string oldIMEI, string IMEI_number, string device_type, string Reason, string DeviceID, string CategID)
        {
            string status = "false";
            //string check_otp = verifyotp(mobilenumber, OTP);


            status = BLL.updateIMEI(oldIMEI, IMEI_number, device_type, Reason, DeviceID, CategID);


            return Json(new { data = status });
        }
        public ActionResult SmsWallet()
        {
            return View();
        }
        public JsonResult getSMSwallet(int categ_id)
        {
            DataTable dt = BLL.getSMSwallet(categ_id);

            List<smswallet> obj = new List<smswallet>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new smswallet
                    {
                        available = Convert.ToInt32(dt.Rows[i]["AVAILABLE"]),
                        used = Convert.ToInt32(dt.Rows[i]["USED"]),
                        total = Convert.ToInt32(dt.Rows[i]["TOTAL"]),
                        rechargeDate = dt.Rows[i]["RechargeDate"].ToString(),
                        customerName = dt.Rows[i]["customerName"].ToString()

                    });
                }
            }
            return Json(new { data = obj });
        }
        public JsonResult rechargeSMSwallet(int categ_id, int count)
        {
            bool status = false;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID == 1)
            {
                DataTable dt = BLL.getSMSwallet(categ_id);
                if (dt.Rows.Count > 0)
                {
                    int available = Convert.ToInt32(dt.Rows[0]["AVAILABLE"]) + count;
                    int used = Convert.ToInt32(dt.Rows[0]["USED"]);
                    int total = available + used;
                    status = BLL.recharge_sms_wallet(categ_id, available, used, total);
                }
                else
                {

                    status = BLL.recharge_sms_wallet(categ_id, count, 0, count);
                }
            }


            return Json(new { data = status });
        }

        public ActionResult newpage2()

        {
            return View();
        }
        [SessionAuthorize]
        public ActionResult DeviceTest()
        {
            return View();
        }

        public ActionResult DeviceTestLog()
        {
            return View();
        }
        public JsonResult getPacketsAIS140(string imei, string Startdate, string Enddate)
        {

            DataTable dt1 = BLL.get_live_packets(imei);
            string deviceid = (dt1.Rows[0]["DEVICE_ID"]).ToString();

            DataTable dt = BLL.get_live_packets1(deviceid, Startdate, Enddate);

            List<packets> obj = new List<packets>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new packets
                    {

                        newdata = dt.Rows[i]["RAWTRIPDATA_DETAILS"].ToString(),
                        date = dt.Rows[i]["RAWTRIPDATA_DATE"].ToString()
                    });
                }
            }
            return Json(new { data = obj });

        }
        public JsonResult getPackets(string imei, string Startdate, string Enddate)
        {

            DataTable dt1 = BLL.get_live_packets(imei);
            //if (dt1.Rows.Count > 0)
            //{dt1.Rows[0][0].ToString()
            string deviceid = (dt1.Rows[0]["DEVICE_ID"]).ToString();

            DataTable dt = BLL.get_live_packets1(deviceid, Startdate, Enddate);

            List<packets> obj = new List<packets>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rawdata = dt.Rows[i]["RAWTRIPDATA_DETAILS"].ToString();
                    string raw = ConvertHex(rawdata);
                    obj.Add(new packets
                    {
                        newdata = raw.ToString(),
                        //newdata = dt.Rows[i]["RAWTRIPDATA_DETAILS"].ToString(),
                        date = dt.Rows[i]["RAWTRIPDATA_DATE"].ToString()
                    });
                }
            }
            //}
            return Json(new { data = obj });

        }
        public static string ConvertHex(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }

        public JsonResult getKeralaPackets(string imei)
        {

            DataTable dt = BLL.get_kerala_packets(imei);
            List<packets> obj = new List<packets>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] data = dt.Rows[i]["PACKET"].ToString().Split('=');
                    if (data.Length >= 2)
                    {
                        string main_packet = data[1].ToString();
                        if (main_packet.Substring(3, 15).ToString() == imei)
                        {
                            obj.Add(new packets
                            {
                                newdata = dt.Rows[i]["PACKET"].ToString()

                            });
                        }

                    }
                }
            }
            return Json(new { data = obj });
        }

        public ActionResult DeviceStatus()
        {
            SMVTS_DEVICETYPES obj_types = new SMVTS_DEVICETYPES();
            DataTable dt_types = new DataTable();
            List<SMVTS_DEVICETYPES> obj_alltypes = new List<SMVTS_DEVICETYPES>();
            dt_types = BLL.GetDeviceNames();
            if (dt_types.Rows.Count > 0)
            {
                for (int k = 0; k < dt_types.Rows.Count; k++)
                {
                    obj_alltypes.Add(new SMVTS_DEVICETYPES
                    {
                        ID = Convert.ToInt32(dt_types.Rows[k]["ID"]),
                        DEVICE_TYPENAME = dt_types.Rows[k]["DEVICE_TYPENAME"].ToString()
                    });
                }
            }

            ViewBag.deviceTypes = obj_alltypes;
            return View();
        }
        public JsonResult Get_Databy_Devicetype(string Device_Name)
        {
            List<Device_Type> obj = new List<Device_Type>();


            DataTable dt = BLL.Device_type(Device_Name);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new Device_Type
                {

                    Vehicle_Color = Convert.ToString(dt.Rows[i]["VEHICLE_COLOR"])
                });
            }

            return Json(new { data = obj });
        }


        [SessionAuthorize]
        public ActionResult StockUpload()


        {
            int USER_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            if (USER_ID == 1)
            {
                SMVTS_CATEGORIES _obj_Smvts_categories = new SMVTS_CATEGORIES();
                List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
                List<SMVTS_SIMS> obj_sims = new List<SMVTS_SIMS>();
                //get categories


                if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 1)
                {
                    _obj_Smvts_categories.OPERATION = operation.Select;
                }
                else if (((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 2 || ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID == 4)
                {
                    _obj_Smvts_categories.CATEG_PARENT_ID = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                    //  _obj_Smvts_categories.CATEG_CATETYPE_ID = ((SMVTS_USERS)Session["USERINFO"]).CATEG_CATETYPE_ID;
                    _obj_Smvts_categories.OPERATION = operation.Select;
                    _obj_Smvts_categories.CATEG_STATUS = "1";
                }

                DataTable dt5 = BLL.get_Categories(_obj_Smvts_categories);
                for (int m = 0; m < dt5.Rows.Count; m++)
                {
                    obj.Add(new SMVTS_CATEGORIES
                    {
                        CATEG_NAME = dt5.Rows[m]["CATEG_NAME"].ToString(),
                        CATEG_ID = Convert.ToInt32(dt5.Rows[m]["CATEG_ID"]),
                    });
                }

                ViewBag.categories = obj;

                //get All SIM stock
                List<Sim_stock> obj1 = new List<Sim_stock>();
                DataTable dt_sims = BLL.getAllSims();
                if (dt_sims.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_sims.Rows.Count; i++)
                    {
                        obj1.Add(new Sim_stock
                        {
                            sim_id = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                            sim_network = dt_sims.Rows[i]["SIM_NETWORK"].ToString(),
                            sim_iccid = dt_sims.Rows[i]["SIM_ICCD"].ToString(),
                            sim_number = dt_sims.Rows[i]["SIM_NUMBER"].ToString(),
                            sim_status = dt_sims.Rows[i]["Status"].ToString(),
                            uploadedDate = dt_sims.Rows[i]["UploadedDate"].ToString(),
                            sim_apn = dt_sims.Rows[i]["SIM_APN"].ToString(),
                            DistributorName = dt_sims.Rows[i]["DistributorName"].ToString(),
                            DealerName = dt_sims.Rows[i]["DealerName"].ToString(),
                            status = Convert.ToInt32(dt_sims.Rows[i]["SIM_STATUS"]),
                            accountId = dt_sims.Rows[i]["ACCOUNT_ID"].ToString(),
                            activeDate = dt_sims.Rows[i]["ActiveDate"].ToString(),
                            plan = dt_sims.Rows[i]["SIM_PLAN"].ToString(),
                            Remarks = dt_sims.Rows[i]["SIM_REMARKS1"].ToString()
                        });
                    }
                }


                ViewBag.stock = obj1;

                //get All Networks
                List<SMVTS_SIM_NETWORKS> obj_networks = new List<SMVTS_SIM_NETWORKS>();
                DataTable dt_network = new DataTable();
                dt_network = BLL.getAllNetworks();
                if (dt_network.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_network.Rows.Count; i++)
                    {
                        obj_networks.Add(new SMVTS_SIM_NETWORKS
                        {
                            ID = Convert.ToInt32(dt_network.Rows[i]["ID"]),
                            network = dt_network.Rows[i]["NETWORK"].ToString()
                        });
                    }

                }
                ViewBag.networks = obj_networks;


                return View();
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public JsonResult getAllSimsStock()
        {
            List<Sim_stock> obj = new List<Sim_stock>();
            DataTable dt_sims = BLL.getAllSims();
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj.Add(new Sim_stock
                    {
                        sim_id = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                        sim_network = dt_sims.Rows[i]["SIM_NETWORK"].ToString(),
                        sim_iccid = dt_sims.Rows[i]["SIM_ICCD"].ToString(),
                        sim_number = dt_sims.Rows[i]["SIM_NUMBER"].ToString(),
                        sim_status = dt_sims.Rows[i]["Status"].ToString(),
                        uploadedDate = dt_sims.Rows[i]["UploadedDate"].ToString(),
                        sim_apn = dt_sims.Rows[i]["SIM_APN"].ToString(),
                        DistributorName = dt_sims.Rows[i]["DistributorName"].ToString(),
                        DealerName = dt_sims.Rows[i]["DealerName"].ToString(),
                        status = Convert.ToInt32(dt_sims.Rows[i]["SIM_STATUS"]),
                        accountId = dt_sims.Rows[i]["ACCOUNT_ID"].ToString(),
                        activeDate = dt_sims.Rows[i]["ActiveDate"].ToString(),
                        plan = dt_sims.Rows[i]["SIM_PLAN"].ToString(),
                        Remarks = dt_sims.Rows[i]["SIM_REMARKS1"].ToString()
                    });
                }
            }
            return Json(new { data = obj });
        }
        public JsonResult getAllSimsByDealer(int dist_Id, int Dealer_Id)
        {
            List<Sim_stock> obj = new List<Sim_stock>();
            DataTable dt_sims = BLL.getAllSims_By_Dealer(Dealer_Id);
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj.Add(new Sim_stock
                    {
                        sim_id = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                        sim_network = dt_sims.Rows[i]["SIM_NETWORK"].ToString(),
                        sim_iccid = dt_sims.Rows[i]["SIM_ICCD"].ToString(),
                        sim_number = dt_sims.Rows[i]["SIM_NUMBER"].ToString(),
                        sim_status = dt_sims.Rows[i]["Status"].ToString(),
                        uploadedDate = dt_sims.Rows[i]["UploadedDate"].ToString(),
                        sim_apn = dt_sims.Rows[i]["SIM_APN"].ToString(),
                        DistributorName = dt_sims.Rows[i]["DistributorName"].ToString(),
                        DealerName = dt_sims.Rows[i]["DealerName"].ToString(),
                        status = Convert.ToInt32(dt_sims.Rows[i]["SIM_STATUS"]),
                        accountId = dt_sims.Rows[i]["ACCOUNT_ID"].ToString(),
                        activeDate = dt_sims.Rows[i]["ActiveDate"].ToString(),
                        plan = dt_sims.Rows[i]["SIM_PLAN"].ToString(),
                        Remarks = dt_sims.Rows[i]["SIM_REMARKS1"].ToString()
                    });
                }
            }
            return Json(new { data = obj });
        }
        public JsonResult UploadStock(string SIM_NUMBER, string SIM_SERIALNO, string SIM_OPERATORNAME, string SIM_APNWEBSITE, string SIM_PLAN, string SIM_PRICE, string ActivateDate, string AccountId)
        {
            string result = "";
            SMVTS_ALL_SIMS_stock obj = new SMVTS_ALL_SIMS_stock();
            obj.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            obj.SIM_APN = SIM_APNWEBSITE;
            obj.SIM_ICCD = SIM_SERIALNO;
            obj.SIM_NETWORK = SIM_OPERATORNAME;
            obj.SIM_NUMBER = SIM_NUMBER;
            obj.SIM_PLAN = SIM_PLAN;
            obj.SIM_PRICE = Convert.ToDecimal(SIM_PRICE);
            obj.ACTIVE_DATE = Convert.ToDateTime(ActivateDate);
            obj.ACCOUNT_ID = AccountId;

            DataTable dt = BLL.check_sim(SIM_NUMBER);
            if (dt.Rows.Count > 0)
            {
                result = "Sim Already Exists";
            }
            else
            {
                bool status = BLL.insert_stock(obj);
                if (status == true)
                {
                    result = "Stock uploaded successfully";
                }
                else
                {
                    result = "Failed to upload stock";
                }
            }


            return Json(new { data = result });
        }
        public JsonResult AllocateStock(int distributorID, int dealerID, string selected_sims)
        {
            bool status = false;
            if (distributorID != 0 && dealerID != 0)
            {
                string[] sel_sims = selected_sims.Split(',');
                for (int i = 0; i < sel_sims.Length; i++)
                {
                    status = BLL.allocateToDealer(dealerID, sel_sims[i], distributorID);
                }

            }
            else if (distributorID != 0 && dealerID == 0)
            {

                string[] sel_sims = selected_sims.Split(',');
                for (int i = 0; i < sel_sims.Length; i++)
                {
                    status = BLL.allocateToDistributor(distributorID, sel_sims[i]);
                }
            }
            return Json(new { data = status });
        }
        public JsonResult EditStock(int sim_id)
        {
            List<Sim_stock> obj = new List<Sim_stock>();
            try
            {

                DataTable dt_sims = BLL.getSimDataById(sim_id);
                if (dt_sims.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_sims.Rows.Count; i++)
                    {
                        obj.Add(new Sim_stock
                        {
                            sim_id = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                            sim_network = dt_sims.Rows[i]["SIM_NETWORK"].ToString(),
                            sim_iccid = dt_sims.Rows[i]["SIM_ICCD"].ToString(),
                            sim_number = dt_sims.Rows[i]["SIM_NUMBER"].ToString(),


                            sim_apn = dt_sims.Rows[i]["SIM_APN"].ToString(),

                            status = Convert.ToInt32(dt_sims.Rows[i]["SIM_STATUS"]),
                            accountId = dt_sims.Rows[i]["ACCOUNT_ID"].ToString(),
                            ACTIVE_DATE = dt_sims.Rows[i]["ACTIVE_DATE"].ToString(),
                            plan = dt_sims.Rows[i]["SIM_PLAN"].ToString(),
                            price = Convert.ToDecimal(dt_sims.Rows[i]["SIM_PRICE"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return Json(new { data = obj });
        }

        public JsonResult UpdateStockDetails(string SIM_NUMBER, string SIM_SERIALNO, string SIM_OPERATORNAME, string SIM_APNWEBSITE, string SIM_PLAN, string SIM_PRICE, string ActivateDate, string AccountId, int sim_id)
        {
            string result = "";
            SMVTS_ALL_SIMS_stock obj = new SMVTS_ALL_SIMS_stock();
            obj.SIM_ID = sim_id;
            obj.CREATEDBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
            obj.SIM_APN = SIM_APNWEBSITE;
            obj.SIM_ICCD = SIM_SERIALNO;
            obj.SIM_NETWORK = SIM_OPERATORNAME;
            obj.SIM_NUMBER = SIM_NUMBER;
            obj.SIM_PLAN = SIM_PLAN;
            obj.SIM_PRICE = Convert.ToDecimal(SIM_PRICE);
            obj.ACTIVE_DATE = Convert.ToDateTime(ActivateDate);
            obj.ACCOUNT_ID = AccountId;


            bool status = BLL.update_stock(obj);
            if (status == true)
            {
                result = "Stock updated successfully";
            }
            else
            {
                result = "Failed to update stock";
            }

            return Json(new { data = result });
        }
        public JsonResult RollBackAllocation(int sim_id, string password)
        {
            //if(password=="Ctpl@1216")
            //{
            //    bool status = BLL.rollback_allocation(sim_id);
            //}
            bool status = BLL.rollback_allocation(sim_id);
            return Json(new { data = status });
        }
        public JsonResult LoadSIMSByCustomer(string categ_name)
        {
            DataTable dt_sims = new DataTable();
            DataTable dt_categ = new DataTable();
            dt_categ = Dal.ExecuteQuery("select CATEG_ID from smvts_categories where CATEG_NAME='" + categ_name + "' AND CATEG_STATUS=1");
            dt_sims = BLL.getSimsByCustomer(dt_categ.Rows[0][0].ToString());
            List<SMVTS_ALL_SIMS> obj_all = new List<SMVTS_ALL_SIMS>();
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj_all.Add(new SMVTS_ALL_SIMS
                    {
                        ID = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                        SIMS = dt_sims.Rows[i]["SIM_NUMBER"].ToString()
                    });
                }
            }
            return Json(new { data = obj_all });
        }
        public JsonResult Terminate_Sim(int sim_id, string sim_status, string sim_password, string sim_reason)
        {
            string result = "";

            if (sim_password == "Ctpl@1216")
            {
                bool status = BLL.terminateSim(sim_id, sim_status, sim_reason);
                if (status == true)
                {
                    result = "true";
                }
                else
                {
                    result = "false";
                }
            }
            else
            {
                result = "Invaid Password";
            }

            return Json(new { data = result });
        }
        public ActionResult AllocateBarcode()
        {
            //get All SIM stock
            List<Sim_stock> obj1 = new List<Sim_stock>();
            DataTable dt_sims = BLL.getAllSims();
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj1.Add(new Sim_stock
                    {
                        sim_id = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                        sim_network = dt_sims.Rows[i]["SIM_NETWORK"].ToString(),
                        sim_iccid = dt_sims.Rows[i]["SIM_ICCD"].ToString(),
                        sim_number = dt_sims.Rows[i]["SIM_NUMBER"].ToString(),
                        sim_status = dt_sims.Rows[i]["Status"].ToString(),
                        uploadedDate = dt_sims.Rows[i]["UploadedDate"].ToString(),
                        sim_apn = dt_sims.Rows[i]["SIM_APN"].ToString(),
                        DistributorName = dt_sims.Rows[i]["DistributorName"].ToString(),
                        DealerName = dt_sims.Rows[i]["DealerName"].ToString(),
                        status = Convert.ToInt32(dt_sims.Rows[i]["SIM_STATUS"]),
                        accountId = dt_sims.Rows[i]["ACCOUNT_ID"].ToString(),
                        activeDate = dt_sims.Rows[i]["ActiveDate"].ToString(),
                        plan = dt_sims.Rows[i]["SIM_PLAN"].ToString(),
                        Remarks = dt_sims.Rows[i]["SIM_REMARKS1"].ToString()
                    });
                }
            }

            ViewBag.stock = obj1;
            return View();
        }
        public JsonResult LoadSIMApn(int sim_id)
        {
            DataTable dt_sims = new DataTable();


            dt_sims = BLL.getSimApns(sim_id);
            List<SMVTS_SIM_APNS> obj_apn = new List<SMVTS_SIM_APNS>();
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj_apn.Add(new SMVTS_SIM_APNS
                    {
                        ID = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                        apn_name = dt_sims.Rows[i]["APN_NAME"].ToString()
                    });
                }
            }
            return Json(new { data = obj_apn });
        }
        public JsonResult LoadSIMPlans(int sim_id)
        {
            DataTable dt_sims = new DataTable();


            dt_sims = BLL.getSimPlans(sim_id);
            List<SMVTS_SIM_PLANS> obj_plans = new List<SMVTS_SIM_PLANS>();
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj_plans.Add(new SMVTS_SIM_PLANS
                    {
                        ID = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                        plan_name = dt_sims.Rows[i]["PLAN_NAME"].ToString()
                    });
                }
            }
            return Json(new { data = obj_plans });
        }

        public JsonResult LoadSIMAccounts(int sim_id)
        {
            DataTable dt_sims = new DataTable();


            dt_sims = BLL.getSimAccounts(sim_id);
            List<SMVTS_SIM_ACCOUNTS> obj_account = new List<SMVTS_SIM_ACCOUNTS>();
            if (dt_sims.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sims.Rows.Count; i++)
                {
                    obj_account.Add(new SMVTS_SIM_ACCOUNTS
                    {
                        ID = Convert.ToInt32(dt_sims.Rows[i]["ID"]),
                        account_name = dt_sims.Rows[i]["ACCOUNT_NAME"].ToString()
                    });
                }
            }
            return Json(new { data = obj_account });
        }
        public JsonResult CheckSim_Exists(string sim_number)
        {
            DataTable dt = new DataTable();
            dt = BLL.ChecksimAlreadyExists(sim_number);
            List<check_sim> obj = new List<check_sim>();
            if (dt.Rows.Count > 1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new check_sim
                    {
                        device_serialnumber = dt.Rows[i]["DEVICE_SERIALNUMBER"].ToString(),
                        sim_number = dt.Rows[i]["SIM_NUMBER"].ToString(),
                        status = true
                    });
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new check_sim
                    {
                        device_serialnumber = dt.Rows[i]["DEVICE_SERIALNUMBER"].ToString(),
                        sim_number = dt.Rows[i]["SIM_NUMBER"].ToString(),
                        status = false
                    });


                }
            }
            return Json(new { data = obj });
        }
        public ActionResult Students()
        {
            List<tblvehiclesBus> obj1 = new List<tblvehiclesBus>();
            List<tblroutes> obj = new List<tblroutes>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_routes(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblroutes
                {
                    ROUTES_ID = Convert.ToInt32(dt.Rows[i]["ROUTES_ID"]),
                    ROUTES_NAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                    ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString(),
                    ROUTES_STATUS = Convert.ToBoolean(dt.Rows[i]["ROUTES_STATUS"])
                });
            }
            ViewBag.routes00 = obj;
            return View();
        }

        public ActionResult BranchStudents()
        {
            List<tblvehiclesBus> obj1 = new List<tblvehiclesBus>();
            List<tblroutes> obj = new List<tblroutes>();
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_routes(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblroutes
                {
                    ROUTES_ID = Convert.ToInt32(dt.Rows[i]["ROUTES_ID"]),
                    ROUTES_NAME = dt.Rows[i]["ROUTES_NAME"].ToString(),
                    ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString(),
                    ROUTES_STATUS = Convert.ToBoolean(dt.Rows[i]["ROUTES_STATUS"])
                });
            }
            ViewBag.routes00 = obj;
            return View();
        }
        public JsonResult student_insert(string student_id, string student_name, string student_dob, string student_class, string parent1_name, string parent1_mobile, string parent2_name, string parent2_mobile, string parent1_email, string categName)
        {
            bool status = false;
            try
            {


                int categid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
                student obj = new student();
                obj.STUDENT_ID = student_id;
                obj.STUDENT_NAME = student_name;
                obj.STUDENT_DOB = Convert.ToDateTime(student_dob);
                obj.STUDENT_CLASS = student_class;
                obj.PARENT1_NAME = parent1_name;
                obj.PARENT1_MOBILE = parent1_mobile;
                obj.PARENT1_EMAIL = parent1_email;
                obj.PARENT2_NAME = parent2_name;
                obj.PARENT2_MOBILE = parent2_mobile;
                status = BLL.insert_student(obj);

                //create parent user
                SMVTS_USERS obj_user = new SMVTS_USERS();
                obj_user.OPERATION = operation.Insert;
                obj_user.USERS_ROLE_ID = 4;
                obj_user.USERS_USERNAME = parent1_email;
                obj_user.USERS_PASSWORD = BLL.Encrypt(parent1_mobile);
                obj_user.USERS_FULLNAME = parent1_name;
                obj_user.USERS_STATUS = "1";
                obj_user.USERS_DEVICE_IDS = "0";
                obj_user.CREATEDBY = categid;
                obj_user.USERS_CATEGORY_ID = categid;
                status = BLL.set_Users(obj_user, "", categName, categid, 4, "");
            }
            catch (Exception ex)
            {
                status = false;
            }
            return Json(new { data = status });
        }
        public JsonResult get_students()
        {
            int categid = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt = BLL.get_students();
            List<student> obj = new List<student>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new student
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                        STUDENT_ID = dt.Rows[i]["STUDENT_ID"].ToString(),
                        STUDENT_NAME = dt.Rows[i]["STUDENT_NAME"].ToString(),
                        PARENT1_NAME = dt.Rows[i]["PARENT1_NAME"].ToString(),
                        PARENT1_MOBILE = dt.Rows[i]["PARENT1_MOBILE"].ToString(),
                        PARENT1_EMAIL = dt.Rows[i]["PARENT1_EMAIL"].ToString(),
                        PARENT2_MOBILE = dt.Rows[i]["PARENT2_MOBILE"].ToString(),
                        PARENT2_NAME = dt.Rows[i]["PARENT2_NAME"].ToString()
                    });
                }
            }
            return Json(new { data = obj });
        }


        public JsonResult get_students1()
        {
            string categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_students1(categid);
            List<student> obj = new List<student>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new student
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                        STUDENT_ID = dt.Rows[i]["STUDENT_ID"].ToString(),
                        STUDENT_NAME = dt.Rows[i]["STUDENT_NAME"].ToString(),
                        PARENT1_NAME = dt.Rows[i]["PARENT1_NAME"].ToString(),
                        PARENT1_MOBILE = dt.Rows[i]["PARENT1_MOBILE"].ToString(),
                        PARENT1_EMAIL = dt.Rows[i]["PARENT1_EMAIL"].ToString(),
                        PARENT2_MOBILE = dt.Rows[i]["PARENT2_MOBILE"].ToString(),
                        PARENT2_NAME = dt.Rows[i]["PARENT2_NAME"].ToString(),                                            
                        ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString()
                    });
                }
            }
            return Json(new { data = obj});
        }


        public JsonResult get_students_by_id(int id)
        {
            string categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_students_by_id(id,categid);
            List<student> obj = new List<student>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new student
                    {
                        //ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                        //STUDENT_ID = dt.Rows[i]["STUDENT_ID"].ToString(),
                        //dob = dt.Rows[i]["STUDENT_DOB"].ToString(),
                        //STUDENT_CLASS = dt.Rows[i]["STUDENT_CLASS"].ToString(),
                        //STUDENT_NAME = dt.Rows[i]["STUDENT_NAME"].ToString(),
                        //PARENT1_NAME = dt.Rows[i]["PARENT1_NAME"].ToString(),
                        //PARENT1_MOBILE = dt.Rows[i]["PARENT1_MOBILE"].ToString(),
                        //PARENT2_MOBILE = dt.Rows[i]["PARENT2_MOBILE"].ToString(),
                        //PARENT2_NAME = dt.Rows[i]["PARENT2_NAME"].ToString(),
                        //ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString()


                        ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                        STUDENT_ID = dt.Rows[i]["STUDENT_ID"].ToString(),
                        STUDENT_NAME = dt.Rows[i]["STUDENT_NAME"].ToString(),
                        PARENT1_NAME = dt.Rows[i]["PARENT1_NAME"].ToString(),
                        PARENT1_MOBILE = dt.Rows[i]["PARENT1_MOBILE"].ToString(),
                        PARENT1_EMAIL = dt.Rows[i]["PARENT1_EMAIL"].ToString(),
                        PARENT2_MOBILE = dt.Rows[i]["PARENT2_MOBILE"].ToString(),
                        PARENT2_NAME = dt.Rows[i]["PARENT2_NAME"].ToString(),
                        //STUDENT_DOB = (DateTime)(dt.Rows[i]["STUDENT_DOB"]),
                        ROUTES_CODE = dt.Rows[i]["ROUTES_CODE"].ToString()
                    });
                }
            }
            return Json(new { data = obj });
        }
        public JsonResult student_update(string student_id, string student_name, string student_dob, string student_class, string parent1_name, string parent1_mobile, string parent2_name, string parent2_mobile, int id)
        {
            student obj = new student();
            obj.ID = id;
            obj.STUDENT_ID = student_id;
            obj.STUDENT_NAME = student_name;           
            obj.STUDENT_DOB = Convert.ToDateTime(student_dob);            
            obj.STUDENT_CLASS = student_class;
            obj.PARENT1_NAME = parent1_name;
            obj.PARENT1_MOBILE = parent1_mobile;
            obj.PARENT2_NAME = parent2_name;
            obj.PARENT2_MOBILE = parent2_mobile;
            bool status = BLL.update_student(obj);
            return Json(new { data = status });
        }
        public ActionResult RenewalReport()
        {

            return View();
        }
        public JsonResult getRenewalData(string StartDate)
        {
            List<Expiry_data> obj_Expiry = new List<Expiry_data>();
            try
            {


                DataTable dt = BLL.get_renewal_data(StartDate);

                for (int j = 0; j < dt.Rows.Count; j++)

                {
                    obj_Expiry.Add(new Expiry_data
                    {

                        Distributorname = dt.Rows[j]["PARENT_NAME"].ToString(),
                        DealerName = dt.Rows[j]["DEALER_NAME"].ToString(),
                        CustomerName = dt.Rows[j]["CUSTOMER_NAME"].ToString(),
                        DeviceName = dt.Rows[j]["DEVICE_NAME"].ToString(),
                        VehicleRegNumebr = dt.Rows[j]["VEHICLES_REGNUMBER"].ToString(),
                        CustomerMobile = dt.Rows[j]["CATEG_MOBILENUMBER"].ToString(),
                        IMEI = dt.Rows[j]["DEVICE_SERIALNUMBER"].ToString(),
                        SimNumber = dt.Rows[j]["SIM_NUMBER"].ToString(),

                        StartDate = dt.Rows[j]["STARTDATE"].ToString(),
                        EndDate = dt.Rows[j]["EXPDATE"].ToString()
                    });

                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();

            }
            var jsonData = new
            {
                data = obj_Expiry
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //By Praveen
        public ActionResult Renew_Rpt()
        {
            return View();
        }
        public ActionResult AlertsConfigaration()
        {
            return View();
        }


        //BY Praveen

        public JsonResult Get_Vehicle_images(string Vehicle_model)
        {
            List<SMVTS_VEHICLE_MODELS> obj = new List<SMVTS_VEHICLE_MODELS>();


            DataTable dt = BLL.getvehicleimages(Vehicle_model);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_VEHICLE_MODELS
                {

                    VEHMODELIMAGES_ID = Convert.ToInt32(dt.Rows[i]["VEHMODELIMAGES_ID"]),
                    VEHMODEL_IMAGENAME = dt.Rows[i]["VEHMODELIMAGES_NAME"].ToString(),



                });
            }

            return Json(new { data = obj });
        }


        public ActionResult Search_Vehicle()
        {
            return View();
        }


        public ActionResult Certificates()
        {
            ViewBag.Icat = "/content/ais-140-protocol.pdf";
            return View();
        }




        public JsonResult Load_SchoolsCateg(string ClientId = null)
        {
            List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
            List<SMVTS_VEHICLES> obj1 = new List<SMVTS_VEHICLES>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            try
            {
                string parClientID = "";
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    parClientID = ClientId;
                }
                else
                {
                    parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                }

                //  string parClientID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);

                _obj_Smvts_Vehicles.OPERATION = operation.Empty;
                _obj_Smvts_Vehicles.CREATEDBY = 0;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(parClientID);


                DataTable dt;
                if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "4")
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = -1;
                }
                else
                {
                    _obj_Smvts_Vehicles.LASTMDFBY = ((SMVTS_USERS)Session["USERINFO"]).USERS_ID;
                }

                dt = BLL.get_Schools(_obj_Smvts_Vehicles);
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj.Add(new SMVTS_VEHICLES
                        {
                            VEHICLES_REGNUMBER = dt.Rows[i]["USERS_FULLNAME"].ToString(),
                            VEHICLES_DEVICE_ID = Convert.ToInt32(dt.Rows[i]["USERS_CATEGORY_ID"]),
                        });
                    }
                }



            }

            catch
            {

            }
            //ViewBag["ddlvehicles"] = obj;
            return Json(new { data = obj });
        }

        public ActionResult RFID_Reader()
        {

            List<Models.SMVTS_RFID_READER> obj = new List<Models.SMVTS_RFID_READER>();

            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.get_RfidReaders(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new Models.SMVTS_RFID_READER
                {
                    READER_ID = Convert.ToInt32(dt.Rows[i]["READER_ID"]),
                    READER_IMEI = dt.Rows[i]["READER_IMEI"].ToString(),
                    READER_VEHICLENO = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),

                });
            }
            ViewBag.SMVTS_RFID_READER = obj;
            return View();

        }
        public JsonResult RFID_Reader_insert(string ReaderIMEI, string VEHICLE_REGNUMBER)
        {
            string User_ID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int CATEG_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            int b = BLL.RFID_Reader_insert(ReaderIMEI, CATEG_ID, Convert.ToInt32(User_ID), VEHICLE_REGNUMBER);
            Session["reader_id"] = b;
            return Json(new { data = b });
        }
        public JsonResult insert_RFID_Reader_details(string SCHEDULERDETAILS_STATIONID, string SCHEDULERDETAILS_Type, string SCHEDULERDETAILS_Time, string SCHEDULERDETAILS_PassengerIDs, int Device_ID)
        {
            int ID = Convert.ToInt32(Session["SCHEDULER_ID"]);
            bool b = false; bool v = false;
            if (ID != 0)
            {
                b = BLL.sheduler_details_insert(Convert.ToInt32(ID), Convert.ToInt32(SCHEDULERDETAILS_STATIONID), SCHEDULERDETAILS_Type, SCHEDULERDETAILS_Time, SCHEDULERDETAILS_PassengerIDs);
                v = BLL.update_user_vehicle(Device_ID, SCHEDULERDETAILS_PassengerIDs);
            }
            return Json(new { data = b });
        }
        //string VNO, int ROUTES_ID, string SCHEDULER_STARTDATE, string SCHEDULER_ENDDATE, int SCHEDULER_ModifiedBY, string SHIFT_TIME, int SCHEDULER_ID 

        public JsonResult RFID_RollBackAllocation(int READER_ID, string password)
        {
            //if(password=="Ctpl@1216")
            //{
            //    bool status = BLL.rollback_allocation(sim_id);
            //}
            bool status = BLL.RFID_RollBackAllocation(READER_ID);
            return Json(new { data = status });
        }


      

    }

}
 








