using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRAGATIVTSMVC.Controllers
{
    public class REPORTSController : BaseController
    {
        //
        // GET: /REPORTS/
        public ActionResult Index()
        {
            return View();
        }
        [SessionAuthorize]
        public ActionResult Reports_Vts()
        {
            SMVTS_ASSOCIATED _obj_smvts_associated = new SMVTS_ASSOCIATED();
            List<SMVTS_ASSOCIATED> obj = new List<SMVTS_ASSOCIATED>();

            _obj_smvts_associated.associated = ((SMVTS_USERS)(Session["USERINFO"])).USERS_ID.ToString();
            _obj_smvts_associated.OPERATION = operation.Select;

            DataTable dt1 = BLL.get_associated(_obj_smvts_associated);

            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                obj.Add(new SMVTS_ASSOCIATED
                {
                    USERS_USERNAME = dt1.Rows[j]["USERS_USERNAME"].ToString(),
                    USERS_ID = dt1.Rows[j]["USERS_ID"].ToString(),
                });
            }
            ViewBag.Ass_grid = obj;


            return View();
        }
        //get_associated


        public JsonResult Ass_grid()
        {
            SMVTS_ASSOCIATED _obj_smvts_associated = new SMVTS_ASSOCIATED();
            List<SMVTS_ASSOCIATED> obj = new List<SMVTS_ASSOCIATED>();

            _obj_smvts_associated.associated = ((SMVTS_USERS)(Session["USERINFO"])).USERS_ID.ToString();
            _obj_smvts_associated.OPERATION = operation.Select;

            DataTable dt1 = BLL.get_associated(_obj_smvts_associated);

            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                obj.Add(new SMVTS_ASSOCIATED
                {
                    USERS_USERNAME = dt1.Rows[j]["USERS_USERNAME"].ToString(),
                    USERS_ID = dt1.Rows[j]["USERS_ID"].ToString(),
                });
            }

            var jsonData = new
            {
                data = obj,

                //data2 = obj01
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [SessionAuthorize]
        public JsonResult frmVehicles_ALL()
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

            var jsonData = new
            {
                data = obj,
            };

            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



        [SessionAuthorize]
        public JsonResult _frm_ViewReports(string USERID = null)
        {
            //get_associated
            //rcmb_associated.DataTextField = "USERS_USERNAME";
            //rcmb_associated.DataValueField = "USERS_ID";
            //rcmb_associated.DataBind();

            DataTable dt = new DataTable();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _objGridTrack = new SMVTS_GRIDTRACK();
            if (USERID != null && USERID != "" && USERID != "0")
            {
                dt = BLL.get_GridTrack(_objGridTrack, USERID);

            }
            else
            {
                dt = BLL.get_GridTrack(_objGridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
            }

            //  dt = BLL.get_GridTrack(_objGridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
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
            // return Json(new { data = obj });

            var jsonData = new
            {
                data = obj,

                //data2 = obj01
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult SendAutoSMS()
        {
            return View();
        }
        

            public ActionResult live_tracking(string apikey)
        {


            // DataTable dt = BLL.ExecuteQuery1("exec USP_GET_BUS_TRACKING @mobileNo='" + apikey + "'");


            //if (dt.Rows.Count > 0)  
            {
                //string DEVID = dt.Rows[0]["DEVICEID"].ToString();
                //string CATEGID = dt.Rows[0]["CATEGID"].ToString();

                //string USERID = dt.Rows[0]["USERID"].ToString();
                //string DBNAME = dt.Rows[0]["dbname"].ToString();

                //string username = dt.Rows[0]["CATEG_NAME"].ToString();

                //Session["USERID"] = USERID;
                //Session["DBNAME"] = DBNAME;
                //Session["CATEGID"] = CATEGID;
                //Session["DEVID"] = DEVID;
                //Session["username"] = username;

                ViewBag.apikey = apikey;
            }
            //  else
            {
                // return RedirectToAction("Error", "Home");
            }

            return View();
        }

        // public ActionResult _parentsms()
        public JsonResult _parentsms()
        {

            DataTable dt01 = BLL.ExecuteQuery1("select distinct parent1_mobile from  student ");

            for (int i = 0; i < dt01.Rows.Count; i++)
            {
                string mobileno = dt01.Rows[i]["parent1_mobile"].ToString();
               // mobileno = "+919142907408";//remove
                bool b = sentsms_parents(mobileno);
              //  return Json(b, JsonRequestBehavior.AllowGet); //remove

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private static bool sentsms_parents(string mno)
        {

            try
            {
                System.Text.StringBuilder linesSB2 = new System.Text.StringBuilder();
                linesSB2.Append("Dear Parent, Please click here to track bus of your child, URL :http://tranopro.com/Reports/Live_Tracking?apiKey="+mno+". Thanks and Regards");
                linesSB2.AppendFormat("{0}", Environment.NewLine);
                linesSB2.Append("Containe Technologies Limited.");
                string msg = linesSB2.ToString();
                //string udh = "";
                string message_text = msg;
                string mobileNumbers = mno;

                string url = "http://173.45.76.227/send.aspx?username=contdemo&pass= &route=trans1&senderid=eTRANO&numbers=" + mobileNumbers + "&message=" + message_text + "";
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
                        //status = "sent";
                    }
                    _Answer.Close();
                    response.Close();

                }
                catch (Exception ex)
                {

                }
            }
            catch
            {

            }

            return true;
        }



        public JsonResult VEHICLECHANGE_URL(string apikey)
        {
           
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
            DataTable dt;
            string strConn = (ConfigurationManager.ConnectionStrings["connection_prod"].ToString());

            //if(dt001.Rows.Count>0)
            //{
            //    int DEVICE_ID = Convert.ToInt32(dt001.Rows[0]["DEVICE_ID"]);
            //}



            //if (username == "BAJAJ" || username == "JLPL" || username == "OMKAR IB" || username == "Satish" || username == "Bafna" || username == "shreenathji ib" || username == "AUTOMOBILE ENGINEERS")
            //{
            //    distance = 1;
            //    dt = BLL.get_Tracking_url("GETLOC_BAJAJ", User_ID, Category_ID, Device_ID, dbname);
            //}
            //else
            //{ 
            // dt = BLL.get_Tracking_url("GETLOC_TEST", User_ID, Category_ID, Device_ID, dbname);
            dt = BLL.ExecuteQueryDB1("exec USP_GET_BUS_TRACKING @mobileNo='" + apikey + "'", strConn);
            //  }
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string TEMP = "";
                DataColumnCollection columns = dt.Columns;

                try
                {
                    obj.Add(new SMVTS_GRIDTRACK
                    {

                        SPEED = Convert.ToInt32(dt.Rows[i]["speed"]),
                        IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                        DRIVER_NAME = "",
                        VEHICLENUMBER = dt.Rows[i]["vno"].ToString(),
                        TRIPDATA_TRIPDATE = dt.Rows[i]["TRIPDATE"].ToString(),
                        PLACE = dt.Rows[i]["PLACE"].ToString(),
                        DISTANCE = "0",
                        DKM = "0",
                        LATITUDE = dt.Rows[i]["TRIPDATA_LATITUDE"].ToString(),
                        LONGITUDE = dt.Rows[i]["TRIPDATA_LONGITUDE"].ToString(),
                        COLOR = dt.Rows[i]["MoveStatus"].ToString(),
                        TRIPDATA_DIRECTION = dt.Rows[i]["TRIPDATA_DIRECTION"].ToString(),

                        ODOMETER = 0,

                    });
                }
                catch
                {

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
        public JsonResult frmVehicles(string UserID = null)
        {

            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            DataTable dt = new DataTable();
            if (UserID != null && UserID != "0" && UserID != "")
            {
                dt = BLL.get_ReportDevices("DEVICES", UserID);
            }
            else
            {
                dt = BLL.get_ReportDevices("DEVICES", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {

                    DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"]),
                    DEVICE_NAME = dt.Rows[i]["DEVICE_NAME"].ToString(),

                });
            }
            //   ViewBag.vehicles = obj;
            var jsondata = new
            {
                data = obj
            };

            var jsonResult = Json(jsondata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [SessionAuthorize]
        public JsonResult _view_Violation(string strOperation, string UserID = null, string VNO = null, string FRMDATE = null, string TODATE = null)
        {
            DataTable dt = new DataTable();
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            SMVTS_GRIDTRACK _objGridTrack = new SMVTS_GRIDTRACK();

            if (UserID != null && UserID != "" && UserID != "0")
            {
                dt = BLL.get_ReportsData(strOperation, UserID,
                 (VNO == "0" ? string.Empty : VNO),
                 (FRMDATE == null ? string.Empty : FRMDATE),
                 (TODATE == null ? string.Empty : TODATE), "", false, "", "");
            }
            else
            {
                dt = BLL.get_ReportsData(strOperation, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                 (VNO == "0" ? string.Empty : VNO),
                 (FRMDATE == null ? string.Empty : FRMDATE),
                 (TODATE == null ? string.Empty : TODATE), "", false, "", "");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strOperation == "11")
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





        public JsonResult MatrixReport(string Date, string USERID = null)
        {
            string UserId = "";
            if (USERID != null && USERID != "0" && USERID != "")
            {
                UserId = USERID;
            }
            else
            {
                UserId = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            }

            string CategId = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = new DataTable();
            if (Convert.ToInt32(UserId) == 1104)
            {
                string Query = "EXEC USP_SMVTS_24HOURS_LOHIYA @OPERATION='GETMATRIX',@present_date='" + Date + "',@USERID='" + UserId + "',@CATEGID='" + CategId + "'";
                dt = BLL.ExecuteQuery(Query);
                //   RadGrid1.DataSource = DT_MATRIX;
            }
            else
            {
                string Query2 = "EXEC USP_SMVTS_24HOURS_MATRIX @OPERATION='GETMATRIX',@present_date='" + Date + "',@USERID='" + UserId + "',@CATEGID='" + CategId + "'";
                dt = BLL.ExecuteQuery(Query2);
                // RadGrid1.DataSource = DT_MATRIX2;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            if (dt != null)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
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
        public ActionResult Certificate()
        {
            return View();
        }

        [SessionAuthorize]
        public JsonResult _VERIFY_REQUEST(string VNO)
        {

            string CATEGID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            string CATEGNAME = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_FULLNAME);

            DataTable dt = new DataTable();
            string Query1 = "EXEC SP_CERTFICATE_REQUEST @OPERTAION='CERTIFICATE_VALIDITY',@VNO='" + VNO + "'";
            string MSG = "FALSE";
            dt = BLL.ExecuteQuery1(Query1);
            if (dt.Rows[0]["VALID"].ToString() == "1")
            {
                MSG = "TRUE";
            }
            else
            {
                string Query = "EXEC SP_CERTFICATE_REQUEST @OPERTAION='CERTIFICATE_REQUEST',@VNO='" + VNO + "',@TRANSPORTERNAME='" + CATEGNAME + "',@CATEGID='" + CATEGID + "'";
                bool B = BLL.ExecuteNonQuery1(Query);
                if (B == true)
                {
                    MSG = "REQ";
                    DataTable dt_ = Dal.ExecuteQuery1("select * from smvts_certificate where sendemail=1 and categid='" + CATEGID + "' and VNO='" + VNO + "' ");

                    if (dt_.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        certificate_email(CATEGID, VNO);
                        bool c = BLL.ExecuteNonQuery1("update smvts_certificate set SENDEMAIL=1  where categid='" + CATEGID + "' and VNO='" + VNO + "'");
                    }

                }
                else
                {
                    MSG = "TRY AGAIN";
                }
            }

            return Json(new { data = MSG });
        }

        public bool certificate_email(string CATEGID, string VNO)
        {

            DataTable dt_ = Dal.ExecuteQuery1("select CATEG_ID,CATEG_NAME,CATEG_EMAILID from SMVTS_CATEGORIES(nolock) where CATEG_ID=" + CATEGID + " and CATEG_STATUS=1");
            string tomail = Convert.ToString(dt_.Rows[0]["CATEG_EMAILID"]);
            StringBuilder sb = new StringBuilder();

            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">").Append("<head>").Append("<title>RAMKI TECHNOLOGIES PVT LTD (VTS)</title>");
            sb.Append("</head>");
            sb.Append("<body>");

            sb.Append("Dear Sir ,<br />");
            sb.Append("<br />");
            sb.Append("VEHICLE NUMBER " + VNO + "<br />");
            sb.Append("<br />");
            sb.Append("We have received your request to provide certificates for RTA approval.<br />  You would shortly receive update on the certificate. <br /> Kindly note that Rs.500 per year per vehicle would be charged in the invoice for this request, as we have to pay to GOVT for getting TRADE CERTIFICATE. <br/> For further queries kindly send an email to below ID's provided: <br /> saikiran@ramkigroup.com<br />   gskreddy@ramkigroup.com");



            Sendmailsave_Certificate(string.Format(sb.ToString()), tomail, "info@smartvts.com");
            bool b = false;
            return b;
        }


        public bool Sendmailsave_Certificate(string str, string toomail, string cc)
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
                mailMessage.From = new System.Net.Mail.MailAddress("info@ramkigroup.com");
                mailMessage.To.Add(new System.Net.Mail.MailAddress(toomail));
                mailMessage.CC.Add(new System.Net.Mail.MailAddress(cc));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress("makarand_w77@yahoo.com"));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress("info@massenterprises.in"));
                mailMessage.Subject = "TRADE CERTIFICATE" + DateTime.Now.ToLongTimeString();
                // mailMessage.Subject = "Hi Pragatipadh - " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
                mailMessage.Priority = System.Net.Mail.MailPriority.High;

                mailMessage.Body = str;



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

                }

            }
            catch (Exception ex)
            {
                // BLL.ShowMessage(this, ex.Message);
            }
            return true;
        }

        public AlternateView InlineAttachment(string str)
        {
            string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();
            paths = paths.Remove(paths.LastIndexOf("\\"), paths.Length - paths.LastIndexOf("\\"));
            paths = paths.Remove(paths.LastIndexOf("\\"), paths.Length - paths.LastIndexOf("\\"));
            paths = paths.Remove(paths.LastIndexOf("\\"), paths.Length - paths.LastIndexOf("\\"));


            string path = Server.MapPath("~/IMAGES/paynowemail_btn.jpg");

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

        public JsonResult CERTIFICATE_APPROVE(string VNO)
        {
            string Query = "EXEC SP_CERTFICATE_REQUEST @OPERTAION='CERTIFICATE_APPROVE',@VNO='" + VNO + "'";
            bool B = BLL.ExecuteNonQuery1(Query);
            return Json(new { data = B });
        }
        [SessionAuthorize]
        public ActionResult Verification_certificate()
        {
            return View();
        }
        public JsonResult _GETALL_CERTIFICATE()
        {
            List<tblcertificate_request> obj = new List<tblcertificate_request>();
            DataTable dt = new DataTable();
            string Query1 = "EXEC SP_CERTFICATE_REQUEST @OPERTAION='GETALL_CERIFICATES'";
            dt = BLL.ExecuteQuery1(Query1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new tblcertificate_request
                {
                    ID = Convert.ToInt32(dt.Rows[i]["ID"]),
                    VNO = dt.Rows[i]["VNO"].ToString(),
                    FLAGSTATUS = Convert.ToInt32(dt.Rows[i]["FLAGSTATUS"]),
                    VALIDUPTO = dt.Rows[i]["VALIDUPTO"].ToString(),
                    REQUESTEDDATE = dt.Rows[i]["REQUESTEDDATE"].ToString(),
                    TRANSPORTERNAME = dt.Rows[i]["TRANSPORTERNAME"].ToString(),
                    TRANSPORTERPAYMENT = dt.Rows[i]["TRANSPORTERPAYMENT"].ToString(),
                    DEALERPAYMENT = dt.Rows[i]["DEALERPAYMENT"].ToString(),
                });
            }
            return Json(new { data = obj });
        }


        public JsonResult CERTIFICATE_PAYMENT(string TPAY, string DEAPAY, string ID)
        {
            string Query = "EXEC SP_CERTFICATE_REQUEST @OPERTAION='UPDATE_PAYMENTS',@TRANSPORTERPAYMENT='" + TPAY + "',@DEALERPAYMENT='" + DEAPAY + "',@ID='" + ID + "'";
            bool B = BLL.ExecuteNonQuery1(Query);
            return Json(new { data = B });
        }




        public class tblcertificate_request
        {
            public string TRANSPORTERPAYMENT;
            public string VNO { get; set; }
            public int FLAGSTATUS { get; set; }
            public string VALIDUPTO { get; set; }
            public string REQUESTEDDATE { get; set; }
            public string TRANSPORTERNAME { get; set; }

            public string DEALERPAYMENT { get; set; }

            public int ID { get; set; }
        }


        public ActionResult Payments_invoice()
        {
            return View();
        }















        public JsonResult DISATNCETRAVELLED_WEEK()
        {
            string USERID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);

            DataTable dt = new DataTable();
            string Query = "exec SP_WEEKLY_REPORT @OPERAION='DISATNCETRAVELLED_WEEK',@USERID='" + USERID + "'";
            dt = BLL.ExecuteQuery(Query);


            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            if (dt != null)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
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


        public ActionResult download_generated_vehicles()
        {
            string CATEGID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            GridView gv = new GridView();

            DataTable dt = new DataTable();
            string Query = "exec SP_CERTFICATE_REQUEST @OPERTAION='GETALL_CERIFICATES_TRANS',@CATEGID='" + CATEGID + "'";
            dt = BLL.ExecuteQuery1(Query);
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Excelreport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
            return View();

        }


        public ActionResult DISATNCETRAVELLED_DETAILS()
        {

            string USERID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            GridView gv = new GridView();

            DataTable dt = new DataTable();
            string Query = "exec SP_WEEKLY_REPORT @OPERAION='DISATNCETRAVELLED_WEEK_DETAILS',@USERID='" + USERID + "'";
            dt = BLL.ExecuteQuery(Query);
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Excelreport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
            return View();
        }

        public ActionResult DOWNLOAD_STOPPAGE_WEEK()
        {

            string USERID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            GridView gv = new GridView();

            DataTable dt = new DataTable();
            string Query = "exec [SP_STOPPAGEREPORT_WEEK] @OPERATION='GET_STOPPAGE_WEEK_DETAILS',@DURATION=180,@USERID='" + USERID + "'";
            dt = BLL.ExecuteQuery(Query);
            gv.DataSource = dt;
            gv.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Excelreport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
            return View();
        }


        //EXEC SP_STOPPAGEREPORT_WEEK @USERID=332,@DURATION=180

        public JsonResult STOPPAGE_WEEK()
        {
            string USERID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);

            DataTable dt = new DataTable();
            string Query = "exec SP_STOPPAGEREPORT_WEEK @USERID='" + USERID + "',@DURATION=180,@OPERATION ='GET_STOPPAGE_WEEK'";
            dt = BLL.ExecuteQuery(Query);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            if (dt != null)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
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


        public JsonResult _certificate(string VNO)
        {
            List<tblcertificate> obj = new List<tblcertificate>();
            DataTable dt = new DataTable();
            string Query = "EXEC SP_CERTIFICATE @vno='" + VNO + "'";
            dt = BLL.ExecuteQuery(Query);
            obj.Add(new tblcertificate
            {
                Vehicle_regnumer = dt.Rows[0]["VEHICLES_REGNUMBER"].ToString(),
                sno = dt.Rows[0]["Sno"].ToString(),
                engineno = dt.Rows[0]["VEHICLES_ENGINENO"].ToString(),
                chassisno = dt.Rows[0]["VEHICLES_CHASSIS_NO"].ToString(),
                imei = dt.Rows[0]["imei"].ToString(),
                vehiclemake = dt.Rows[0]["VEHLEMM_MAKE"].ToString(),
                vehiclemodel = dt.Rows[0]["VEHLEMM_MODEL"].ToString(),
                categ_name = dt.Rows[0]["categ_name"].ToString(),
                categ_address = dt.Rows[0]["categ_address"].ToString(),
                certifcateno = dt.Rows[0]["certifcateno"].ToString(),
                vehicles_yom = dt.Rows[0]["vehicles_yom"].ToString(),
            });
            return Json(new { data = obj });
        }

        public class tblcertificate
        {
            public string Vehicle_regnumer { get; set; }
            public string sno { get; set; }
            public string chassisno { get; set; }
            public string engineno { get; set; }
            public string imei { get; set; }

            public string vehiclemake { get; set; }
            public string vehiclemodel { get; set; }

            public string categ_name { get; set; }
            public string categ_address { get; set; }
            public string certifcateno { get; set; }
            public string vehicles_yom { get; set; }
        }

        public JsonResult DEVICE_PERFOMANCE_WEEK()
        {
            string CATEGID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            string USERID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = new DataTable();
            string Query = "EXEC SP_DEVICEPERFORMANCE @OPERATION='GET_DEVICE_PERFORMANCE',@CATEGID='" + CATEGID + "',@USERID=" + USERID + "";
            dt = BLL.ExecuteQuery(Query);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            if (dt != null)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
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
        public JsonResult _fasttag(string VNO, string fromdate, string enddate)
        {
            //KA01AF6080  //1/30/2019 // 2/6/2019
            DataTable dt = new DataTable();
            string Query = "EXEC SP_FASTTAG_HISTORY @vehicleNo ='" + VNO + "',@startdate ='" + fromdate + "',@enddate ='" + enddate + "'";
            dt = BLL.ExecuteQuery(Query);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row1;
            if (dt != null)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
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
        public ActionResult frm_fuelgraphs()
        {
            List<SMVTS_FUELTYPE> obj = new List<SMVTS_FUELTYPE>();
            List<SMVTS_DEVICES> obj1 = new List<SMVTS_DEVICES>();
            SMVTS_FUELTYPE _obj_Smvts_fuelvalue = new SMVTS_FUELTYPE();
            _obj_Smvts_fuelvalue.OPERATION = operation.select;

            DataTable dt = BLL.getfuelvalue(_obj_Smvts_fuelvalue);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_FUELTYPE
                {
                    TYPENAME = dt.Rows[i]["TYPENAME"].ToString(),
                    TYPEID = Convert.ToInt32(dt.Rows[i]["TYPEID"]),
                });
            }


            DataTable dt1 = BLL.get_ReportDevices("DEVICES", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));

            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                obj1.Add(new SMVTS_DEVICES
                {
                    DEVICE_NAME = dt1.Rows[j]["DEVICE_NAME"].ToString(),
                    DEVICE_ID = Convert.ToInt32(dt1.Rows[j]["DEVICE_ID"]),

                });
            }

            ViewBag.fueltype = obj;
            ViewBag.fuelvehicles = obj1;

            return View();
        }


        public JsonResult _fuelgraphs(string VNO, string Formdate, string Todate, string Value)
        {
            List<SMVTS_FUELTYPE> obj = new List<SMVTS_FUELTYPE>();
            DataTable dt = new DataTable();
            if (Value == "1")
            {
                dt = BLL.get_fuelrefil(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
               (VNO == "0" ? string.Empty : VNO),
               (Formdate == null ? string.Empty : Convert.ToDateTime(Formdate).ToString("MM/dd/yyyy hh:mm tt")),
               (Todate == null ? string.Empty : Convert.ToDateTime(Todate).ToString("MM/dd/yyyy hh:mm tt")), 0, "GET_FUELREFILLING");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_FUELTYPE
                    {
                        Location = dt.Rows[i]["location"].ToString(),
                        Refildate = dt.Rows[i]["refill_date"].ToString(),
                        Vehicleno = dt.Rows[i]["vehicleno"].ToString(),
                        Refil_value = dt.Rows[i]["refill_val"].ToString(),
                        Odometer = dt.Rows[i]["odometer"].ToString(),
                    });
                }
            }
            else if (Value == "2")
            {
                string Operation = string.Empty, strDS = string.Empty, strRptpath = string.Empty, strRptDisplayName = string.Empty;
                DataTable dt_Report = new DataTable();
                strDS = "VTS_RPT_GETDATA_TRIP";
                strRptpath = @"Reports_rdlc\rpt_FuelTheft.rdlc";
                strRptDisplayName = "Fuel Theft Report";

                dt_Report = BLL.get_fuelmillage(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                    (VNO == "0" ? string.Empty : VNO),
                    (Formdate == null ? string.Empty : Convert.ToDateTime(Formdate).ToString("MM/dd/yyyy hh:mm tt")),
                    (Todate == null ? string.Empty : Convert.ToDateTime(Todate).ToString("MM/dd/yyyy hh:mm tt")), 0, "GET_FUELTHEFT");
                for (int i = 0; i < dt_Report.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_FUELTYPE
                    {
                        Location = dt_Report.Rows[i]["final_loc"].ToString(),
                        Refildate = dt_Report.Rows[i]["final_time"].ToString(),
                        Vehicleno = dt_Report.Rows[i]["final_vno"].ToString(),
                        STARTLEVEL = dt_Report.Rows[i]["final_startlevel"].ToString(),
                        ENDLEVEL = dt_Report.Rows[i]["final_endlevel"].ToString(),
                        THEFTLEVEL = dt_Report.Rows[i]["theft_value"].ToString(),
                    });
                }
            }
            else if (Value == "3")
            {
                string Operation = string.Empty, strDS = string.Empty, strRptpath = string.Empty, strRptDisplayName = string.Empty;
                DataTable dt_Report = new DataTable();
                strDS = "SMVTSReport_RPT_USP_CONSIGNMENT";
                strRptpath = @"Reports_rdlc\rpt_fuelconsumptionreport.rdlc";
                strRptDisplayName = "Fuel Consumption";

                dt_Report = BLL.get_fuelconsumption_report(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
                (VNO == "0" ? string.Empty : VNO),
                (Formdate == null ? string.Empty : Convert.ToDateTime(Formdate).ToString("MM/dd/yyyy hh:mm tt")),
                (Todate == null ? string.Empty : Convert.ToDateTime(Todate).ToString("MM/dd/yyyy hh:mm tt")), 0);
                for (int i = 0; i < dt_Report.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_FUELTYPE
                    {
                        TRIPDATE = dt_Report.Rows[i]["TRIPDATE"].ToString(),
                        FUEL = Convert.ToInt32(dt_Report.Rows[i]["fuel"]),
                        DISTANCE = Convert.ToInt32(dt_Report.Rows[i]["DISTANCE"]),
                    });
                }

            }
            else if (Value == "4")
            {
                string Operation = string.Empty, strDS = string.Empty, strRptpath = string.Empty, strRptDisplayName = string.Empty;
                DataTable dt_Report = new DataTable();
                strDS = "SMVTSReport_RPT_USP_CONSIGNMENT";
                strRptpath = @"Reports_rdlc\rpt_fuelconsumptionreport.rdlc";
                strRptDisplayName = "Fuel Consumption";
                dt_Report = BLL.get_fuelmillage(Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID),
              (VNO == "0" ? string.Empty : VNO),
              (Formdate == null ? string.Empty : Convert.ToDateTime(Formdate).ToString("MM/dd/yyyy hh:mm tt")),
              (Todate == null ? string.Empty : Convert.ToDateTime(Todate).ToString("MM/dd/yyyy hh:mm tt")), 0, "GET_VEHICLEMILEAGE");

                for (int i = 0; i < dt_Report.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_FUELTYPE
                    {
                        Location = dt_Report.Rows[i]["location"].ToString(),
                        Refildate = dt_Report.Rows[i]["refill_date"].ToString(),
                        Vehicleno = dt_Report.Rows[i]["vehicleno"].ToString(),
                        Refil_value = dt_Report.Rows[i]["refill_val"].ToString(),
                        Refil_fuel = dt_Report.Rows[i]["fuel_consumed"].ToString(),
                        DISTANCE = Convert.ToInt32(dt_Report.Rows[i]["DISTANCE"]),
                        MILLEAGE = dt_Report.Rows[i]["mileage"].ToString(),
                    });
                }

            }

            else
            {

            }
            var jsonData = new
            {
                Report = obj
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        [SessionAuthorize]
        public ActionResult TOLLGATE()
        {
            return View();
        }
        

        public JsonResult _Stoppage()
        {
            int USERID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            DataTable dt = BLL.GET_STOPPAGE(Convert.ToString(USERID));
            List<stickynotepad> obj = new List<stickynotepad>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new stickynotepad
                {
                    VNO = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                    DURATION = dt.Rows[i]["DURATION(DAYS:HRS:MINS)"].ToString(),
                    DATE = dt.Rows[i]["Last Record Date"].ToString(),
                    PLACE = dt.Rows[i]["PLACE"].ToString(),
                });
            }

            var jsonData = new
            {
                Report = obj
            };
            return Json(jsonData);
        }


        public class stickynotepad
        {
            public string VNO { get; set; }
            public string DURATION { get; set; }
            public string DATE { get; set; }
            public string PLACE { get; set; }
        }





        public ActionResult Testing()
        {
            return View();
        }

        [SessionAuthorize]
        public ActionResult KMS_Monthly()
        {
            return View();
        }
      
        public string Get_KMS_Monthly(string startdate)
        {
            DataTable dt = new DataTable();
            int userid=Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            int categid= Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
           // DateTime date = DateTime.Now;
            //string year = date.Year.ToString();
            // string startdate= date.ToString("MM-dd-yyyy");
          //  string startdate = month + "-" + year;
            dt = BLL.Get_Monthly_Report(userid, categid, startdate);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
            //var list = dt.AsEnumerable().ToList();
            //var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            //return jsonResult;
        }
       
        public ActionResult IgnitionReport()

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
            ViewBag.vehicles = obj;
            return View();
        }
        public JsonResult _ignition(string VNO, string startdate, string Enddate)
        {
            int USERID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
            DataTable dt = BLL.GET_IGNITION(USERID, VNO, startdate, Enddate);
            List<GET_IGNITION> obj = new List<GET_IGNITION>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new GET_IGNITION
                {
                    VNO = dt.Rows[i]["VEH_NUM"].ToString(),
                    startdate = dt.Rows[i]["STARTDT"].ToString(),
                    Enddate = dt.Rows[i]["ENDDT"].ToString(),
                    Timediff = dt.Rows[i]["TIMEDIFF"].ToString(),
                    status = dt.Rows[i]["STATUS"].ToString(),
                    Lndmkadd = dt.Rows[i]["LNDMKADD"].ToString(),
                    Noofmins = dt.Rows[i]["NOOFMINS"].ToString(),
                    KMS = dt.Rows[i]["KMS"].ToString()
                });
            }

            var jsonData = new
            {
                Report = obj
            };

            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult AcReport()
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
            ViewBag.vehicles = obj;
            return View();
        }
        public JsonResult _acreport(string VNO, string startdate, string Enddate)
        {
            try
            {
                int USERID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
                DataTable dt = BLL.getAcReport(USERID, VNO, startdate, Enddate);
                List<acreport> obj = new List<acreport>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new acreport
                    {
                        VNO = dt.Rows[i]["VEH_NUM"].ToString(),
                        startdate = dt.Rows[i]["STARTDATE"].ToString(),
                        Enddate = dt.Rows[i]["ENDDATE"].ToString(),
                        Timediff = dt.Rows[i]["TIME"].ToString(),
                        status = dt.Rows[i]["STATUS"].ToString(),
                        Lndmkadd = dt.Rows[i]["ADDERESS"].ToString(),
                        // Noofmins = dt.Rows[i]["NOOFMINS"].ToString()
                    });
                }

                var jsonData = new
                {
                    Report = obj
                };
                var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch(Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
           
        }
    }

}