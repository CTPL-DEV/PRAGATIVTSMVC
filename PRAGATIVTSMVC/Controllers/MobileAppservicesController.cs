using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Web;
using System.Web.Script.Serialization;
using System.Net.Mail;

namespace PRAGATIVTSMVC.Controllers
{
    public class MobileAppservicesController : ApiController
    {
        [HttpGet]
        [Route("api/ClientDashboard/{Username}/{Password}")]
        public HttpResponseMessage ClientDashboard(string Username, string Password)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string RETVAL = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt_check = new DataTable();
                dt_check = BLL.getDatabaseForUser(Username, Password);
                if (dt_check.Rows.Count>0)
                {
                   // string dbname = "Aor2T0SveXN3Qx99ow3pJdxI6ruR88McJyyA+m2RzQ7GhTWCzxCZGIf5D1n4CCQUJxaM2lG1Dw54Ew7+Lt094KFUqE9WKmftI15Bs/dDX74J5+Uak0lGlaJEX9NVCHgn";
                    string dbname = dt_check.Rows[0][1].ToString();
                    dt_check.Rows[0][1] = dbname;
                    SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();
                    _obj_Smvts_Users = BLL.get_User(dt_check.Rows[0][0].ToString(), Username, dt_check.Rows[0][1].ToString());
                    if (_obj_Smvts_Users != null)
                    {
                        _obj_Smvts_Users.USERS_DBNAME = dbname;
                        if (Username.ToUpper() == _obj_Smvts_Users.USERS_USERNAME.ToUpper() && Password == BLL.Decrypt(_obj_Smvts_Users.USERS_PASSWORD))
                        {
                            string varValue = _obj_Smvts_Users.USERS_MENUITEMS;
                            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
                            DataTable dt = BLL.get_GridTrack_MOBILEAPP_METHODS(_obj_Smvts_GridTrack, Convert.ToString(_obj_Smvts_Users.USERS_ID), dbname);

                            DataColumn UserID = new DataColumn();
                            UserID.ColumnName = "UserID";
                            UserID.DefaultValue = Convert.ToString(_obj_Smvts_Users.USERS_ID);
                            dt.Columns.Add(UserID);

                            foreach (DataRow dr in dt.Rows)
                            {
                                row = new Dictionary<string, object>();
                                foreach (DataColumn col in dt.Columns)
                                {
                                    row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                                }
                                rows.Add(row);
                            }
                            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
                        }
                        else
                        {

                            //BLL.ShowMessage(this, "UserName/Password Does not Match !!!");
                            response.Content = new StringContent("UserName/Password Does not Match", Encoding.UTF8, "application/json");
                        }
                    }
                    else
                    {
                        //BLL.ShowMessage(this, "UserName/Password Does not Match !!!");
                        response.Content = new StringContent("UserName/Password Does not Match", Encoding.UTF8, "application/json");
                    }
                }
                else
                {

                    //BLL.ShowMessage(this, "Company Name/UserName/Password Does not Match !!!");
                    response.Content = new StringContent("UserName/Password Does not Match", Encoding.UTF8, "application/json");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            
           
            return response;
           //return Ok(rows);
            
           
        }


        [HttpGet]
        [Route("api/DriverNumber/{drivernumber}")]
        public HttpResponseMessage DriverNumber(string drivernumber)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (drivernumber != string.Empty)
            {

                try
                {
                    string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.CommandText = "select ER_VEHICLENO,ER_DRIVER_NAME,ER_DRIVER_PHONE,ER_PARTYNAME,ER_FROM,ER_TO from SMVTS_ER_TRIPINFO(nolock) where ER_DRIVER_PHONE='" + drivernumber + "' ";
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.Connection = con;
                    da.Fill(dt);

                    // DataTable dt = BLL.GETDRIVERDETAILS(drivernumber);
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


                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/Driverstatus/{drivernumber}")]
        public HttpResponseMessage Driverstatus(string drivernumber)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            if (drivernumber != string.Empty)
            {

                try
                {
                    string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    SqlCommand cmd = new SqlCommand();
                    con.Open();

                    cmd.CommandText = "select top 1* from SMVTS_MOBILEAPP_ERTRIP where ERTRIP_MOBILENO='" + drivernumber + "' order by ERTRIP_ID desc  ";
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.Connection = con;
                    da.Fill(dt);

                    // DataTable dt = BLL.GETDRIVERDETAILS(drivernumber);
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                // JsonConvert.SerializeObject(DateTimeOffset.UtcNow);
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }

                    }


                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
            //  return JsonConvert.SerializeObject(this, Formatting.None, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;
        }

        private static bool sendRequest_new(string msg, string mno, string type)
        {
            string from_mobileno = "CTPL";
            //string udh = "";
            string message_text = msg;
            //valuefirst
            // string URL = "http://api.myvaluefirst.com/psms/servlet/psms.Eservice2?data=<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE MESSAGE SYSTEM \"http://127.0.0.1:80/psms/dtd/messagev12.dtd\"><MESSAGE VER=\"1.2\"><USER USERNAME=\"dhanushinfotrns\" PASSWORD=\"dhanush617\"/>";
            // string mobileNumbers = mno;
            //string URL = "http://tran.rocktwosms.com/api.php?username=pragatipadh&password=771851&to="+mno+"&from=PRAGAT&message="+msg;

            string URL = "http://173.45.76.227/send.aspx?username=contdemo&pass=cont1234&route=trans1&senderid=eTRANO&numbers=" + mno + "&message=" + msg + "&data=<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE MESSAGE SYSTEM \"http://127.0.0.1:80/psms/dtd/messagev12.dtd\"><MESSAGE VER=\"1.2\"><USER USERNAME=\"pragatipadh\" PASSWORD=\"771851\"/>";


            string mobileNumbers = mno;

            string query_URL = "<SMS  UDH=\"0\" CODING=\"1\" TEXT=\"" + message_text + "\" PROPERTY=\"0\" ID=\"0\">";




            //Console.WriteLine("Message sent to" + to_mobileno + " at " + DateTime.Now.ToString() + "");
            query_URL = query_URL + "<ADDRESS FROM=\"" + from_mobileno + "\" TO=\"" + mno.Trim() + "\" SEQ=\"1\" TAG=\"some clientside random data\"/>";

            query_URL = query_URL + "</SMS>";

            string Credentails_URL = URL + query_URL + "</MESSAGE>&action=send";

            HttpWebRequest request = (HttpWebRequest)
               WebRequest.Create(Credentails_URL);

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            // we will read data via the response stream
            Stream Answer = response.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string result_value = _Answer.ReadToEnd();

            if (result_value.ToUpper().Trim() == "SENT.")
            {


                if (result_value != "")
                {

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result_value);

                    XmlNodeList list = doc.SelectNodes("MESSAGEACK");

                    foreach (XmlNode xNode in list)
                    {
                        XmlElement bookElement = (XmlElement)xNode;

                        XmlNodeList GUID_XMLS = bookElement.SelectNodes("GUID");

                        foreach (XmlNode xNode2 in GUID_XMLS)
                        {
                            string GUID = xNode2.Attributes["GUID"].Value.ToString();
                            string SUBMITDATE = xNode2.Attributes["SUBMITDATE"].Value.ToString();
                            int ID = Convert.ToInt32(xNode2.Attributes["ID"].Value.ToString());



                            string response_query = "INSERT INTO SMVTS_SMSSERVICEPROVIDERS_RESPONSE VALUES('" + result_value + "',GETDATE(),'VALUEFIRST','0')";
                            //SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, response_query);
                            string insrt_query = "INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + mno + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')";
                            // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, insrt_query);

                        }
                        //string author = bookElement.GetElementsByTagName("author")[0].InnerText;
                    }

                }

            }



            return true;
        }

        [HttpGet]
        [Route("api/DriverNumberOTP/{drivernumber}")]
        public bool DriverNumberOTP(string drivernumber)
        {
            bool r = false;

            if (drivernumber != string.Empty)
            {

                try
                {
                    string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.CommandText = "select * from SMVTS_ER_TRIPINFO(nolock) where ER_DRIVER_PHONE='" + drivernumber + "' ";
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.Connection = con;
                    da.Fill(dt);

                    // DataTable dt = BLL.GETDRIVERDETAILS(drivernumber);
                    if (dt != null)
                    {
                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    row = new Dictionary<string, object>();
                        //    foreach (DataColumn col in dt.Columns)
                        //    {
                        //        row.Add(col.ColumnName, dr[col]);
                        //    }
                        //    rows.Add(row);
                        //}

                        //}

                        bool f = false;
                        Random ran = new Random();
                        string sa = ran.Next(10000, 99999).ToString();
                        string type = "OTP";
                        string mno = drivernumber;
                        //send varification code(sa) to mobile
                        //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://pragativts.com/mobiledownloadpk.aspx";
                        string msg = "Dear User, Use " + sa + " as one time password (OTP) to login to your ExpressDriverApp account. Valid for 15 Minutes";
                        // string msg = "Dear  customer, The OTP to complete FTS activation is " + sa + ".Click Here to download the app http://localhost:1503//mobiledowloadapk.aspx";
                        //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://localhost:1503//mobiledowloadapk.aspx";
                        f = sendRequest_new(msg, mno, type);
                        // string from_mobileno = "VTSSMS";
                        //string from_mobileno = "BAJICT";
                        ////string udh = "";
                        //string message_text = msg;
                        ////valuefirst
                        //string URL = "http://api.myvaluefirst.com/psms/servlet/psms.Eservice2?data=<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE MESSAGE SYSTEM \"http://127.0.0.1:80/psms/dtd/messagev12.dtd\"><MESSAGE VER=\"1.2\"><USER USERNAME=\"dhanushinfotrns\" PASSWORD=\"dhanush617\"/>";
                        //// drivernumber="9701640330";
                        //string mobileNumbers = drivernumber;

                        //string query_URL = "<SMS  UDH=\"0\" CODING=\"1\" TEXT=\"" + message_text + "\" PROPERTY=\"0\" ID=\"0\">";




                        ////Console.WriteLine("Message sent to" + to_mobileno + " at " + DateTime.Now.ToString() + "");
                        //query_URL = query_URL + "<ADDRESS FROM=\"" + from_mobileno + "\" TO=\"" + mobileNumbers.Trim() + "\" SEQ=\"1\" TAG=\"some clientside random data\"/>";

                        //query_URL = query_URL + "</SMS>";

                        //string Credentails_URL = URL + query_URL + "</MESSAGE>&action=send";

                        //HttpWebRequest request = (HttpWebRequest)
                        //   WebRequest.Create(Credentails_URL);

                        //// execute the request
                        //HttpWebResponse response = (HttpWebResponse)
                        //    request.GetResponse();

                        //// we will read data via the response stream
                        //Stream Answer = response.GetResponseStream();
                        //StreamReader _Answer = new StreamReader(Answer);
                        //string result_value = _Answer.ReadToEnd();
                        ////if (result_value.ToUpper().Trim() == "SENT.")
                        //// {


                        //if (result_value != "")
                        //{

                        //    XmlDocument doc = new XmlDocument();
                        //    doc.LoadXml(result_value);

                        //    XmlNodeList list = doc.SelectNodes("MESSAGEACK");

                        //    foreach (XmlNode xNode in list)
                        //    {
                        //        XmlElement bookElement = (XmlElement)xNode;

                        //        XmlNodeList GUID_XMLS = bookElement.SelectNodes("GUID");

                        //        foreach (XmlNode xNode2 in GUID_XMLS)
                        //        {
                        //            string GUID = xNode2.Attributes["GUID"].Value.ToString();
                        //            string SUBMITDATE = xNode2.Attributes["SUBMITDATE"].Value.ToString();
                        //            int ID = Convert.ToInt32(xNode2.Attributes["ID"].Value.ToString());

                        //            SqlConnection con1 = new SqlConnection("Data Source=192.168.50.141;Initial Catalog=VTS_Config;User ID=smvts;Password=ZCBAVTSDHANUSH2010INDIA");
                        //            con1.Open();
                        //            SqlCommand cmd1 = new SqlCommand("INSERT INTO SMVTS_SMSSERVICEPROVIDERS_RESPONSE VALUES('" + result_value + "',GETDATE(),'VALUEFIRST','0')", con1);
                        //            int s = cmd.ExecuteNonQuery();

                        //            // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, response_query);
                        //            SqlCommand cmd2 = new SqlCommand("INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + mobileNumbers + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')", con1);
                        //            int s1 = cmd2.ExecuteNonQuery();
                        //            f = true;

                        //            //    con.Close();
                        //            // string insrt_query = "INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + to_mobileno + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')";
                        //            // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, insrt_query);

                        //        }
                        //        //string author = bookElement.GetElementsByTagName("author")[0].InnerText;
                        //    }



                        //}
                        if (f == true)
                        {
                            r = BLL.insertotp(drivernumber, sa, dbname);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
            return r;
           
        }

        [HttpGet]
        [Route("api/OTP_VERIFICATION/{driverno}/{otp}")]
        public bool OTP_VERIFICATION(string driverno, string otp)
        {
            bool r = false;
            string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
            System.Data.DataTable dt = BLL.getotpverification(driverno, otp, dbname);
            if (dt.Rows.Count > 0)
            {
                r = true;
            }
            return r;


        }


        [HttpGet]
        [Route("api/DriverNumber_CurrentLocation/{drivernumber}")]
        public HttpResponseMessage DriverNumber_CurrentLocation(string drivernumber)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select ER_VEHICLENO,ER_DRIVER_NAME,ER_DRIVER_PHONE,ER_PARTYNAME,ER_FROM,ER_TO,TRIPDATA_LATITUDE,TRIPDATA_LONGITUDE,TRIPDATE from SMVTS_ER_TRIPINFO inner join SMVTS_DASHBOARD_VIEW on ER_VEHICLENO=VNO where ER_DRIVER_PHONE='" + drivernumber + "' order by TRIPDATE desc ";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.Connection = con;
                da.Fill(dt);
                con.Close();
                // DataTable dt = BLL.GETDRIVERDETAILS(drivernumber);
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
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/ResendOTP/{mobileno}/{imei}")]
        public bool ResendOTP(string mobileno, string imei)
        {
            bool f = false;
            Random ran = new Random();
            string sa = ran.Next(10000, 99999).ToString();
            string mno = mobileno;
            bool r = BLL.setresendOTP(mobileno, sa);
            if (r == true)
            {

                string type = "OTP";
                //send varification code(sa) to mobile
                //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://pragativts.com/mobiledownloadpk.aspx";
                string msg = "Dear  customer, The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app https://play.google.com/store/apps/details?id=org.pragatipadh.pragati_vts";
                //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://localhost:1503//mobiledowloadapk.aspx";
                f = sendRequest_new(msg, mno, type);
                //string from_mobileno = "VTSSMS";
                ////string udh = "";
                //string message_text = msg;
                ////valuefirst
                //string URL = "http://api.myvaluefirst.com/psms/servlet/psms.Eservice2?data=<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE MESSAGE SYSTEM \"http://127.0.0.1:80/psms/dtd/messagev12.dtd\"><MESSAGE VER=\"1.2\"><USER USERNAME=\"dhanushinfotrns\" PASSWORD=\"dhanush617\"/>";

                //string mobileNumbers = mobileno;

                //string query_URL = "<SMS  UDH=\"0\" CODING=\"1\" TEXT=\"" + message_text + "\" PROPERTY=\"0\" ID=\"0\">";




                ////Console.WriteLine("Message sent to" + to_mobileno + " at " + DateTime.Now.ToString() + "");
                //query_URL = query_URL + "<ADDRESS FROM=\"" + from_mobileno + "\" TO=\"" + mobileno.Trim() + "\" SEQ=\"1\" TAG=\"some clientside random data\"/>";

                //query_URL = query_URL + "</SMS>";

                //string Credentails_URL = URL + query_URL + "</MESSAGE>&action=send";

                //HttpWebRequest request = (HttpWebRequest)
                //   WebRequest.Create(Credentails_URL);

                //// execute the request
                //HttpWebResponse response = (HttpWebResponse)
                //    request.GetResponse();

                //// we will read data via the response stream
                //Stream Answer = response.GetResponseStream();
                //StreamReader _Answer = new StreamReader(Answer);
                //string result_value = _Answer.ReadToEnd();
                ////if (result_value.ToUpper().Trim() == "SENT.")
                //// {


                //if (result_value != "")
                //{

                //    XmlDocument doc = new XmlDocument();
                //    doc.LoadXml(result_value);

                //    XmlNodeList list = doc.SelectNodes("MESSAGEACK");

                //    foreach (XmlNode xNode in list)
                //    {
                //        XmlElement bookElement = (XmlElement)xNode;

                //        XmlNodeList GUID_XMLS = bookElement.SelectNodes("GUID");

                //        foreach (XmlNode xNode2 in GUID_XMLS)
                //        {
                //            string GUID = xNode2.Attributes["GUID"].Value.ToString();
                //            string SUBMITDATE = xNode2.Attributes["SUBMITDATE"].Value.ToString();
                //            int ID = Convert.ToInt32(xNode2.Attributes["ID"].Value.ToString());

                //            SqlConnection con1 = new SqlConnection("Data Source=192.168.50.141;Initial Catalog=VTS_Config;User ID=smvts;Password=ZCBAVTSDHANUSH2010INDIA");
                //            con1.Open();
                //            SqlCommand cmd = new SqlCommand("INSERT INTO SMVTS_SMSSERVICEPROVIDERS_RESPONSE VALUES('" + result_value + "',GETDATE(),'VALUEFIRST','0')", con1);
                //            int s = cmd.ExecuteNonQuery();

                //            // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, response_query);
                //            SqlCommand cmd1 = new SqlCommand("INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + mno + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')", con1);
                //            int s1 = cmd1.ExecuteNonQuery();
                //            f = true;

                //            // string insrt_query = "INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + to_mobileno + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')";
                //            // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, insrt_query);

                //        }
                //        //string author = bookElement.GetElementsByTagName("author")[0].InnerText;
                //    }



                //}

            }

            return f;
        }

        [HttpGet]
        [Route("api/ResendOTP/{drivernumber}/{vehno}/{drivername}/{party}/{fromloc}/{toloc}/{latitiude}/{longitude}")]
        public bool GetReportedate(string drivernumber, string vehno, string drivername, string party, string fromloc, string toloc, string latitiude, string longitude)
        {
            // List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            bool status = false;
            if (drivernumber != string.Empty && vehno != string.Empty && drivername != string.Empty && party != string.Empty && fromloc != string.Empty && toloc != string.Empty && latitiude != string.Empty && longitude != string.Empty)
            {
                try
                {

                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    con.Open();
                    var cmd = new SqlCommand("INSERT INTO SMVTS_MOBILEAPP_ERTRIP(ERTRIP_MOBILENO,ERTRIP_VEHNO,ERTRIP_DRIVERNAME,ERTRIP_PARTY,ERTRIP_FROMLOC,ERTRIP_TOLOC,ERTRIP_REPORTINGDATE,ERTRIP_LATITUDE,ERTRIP_LONGITUDE,ERTRIP_PREVIOUS_STATUS) VALUES('" + drivernumber + "','" + vehno + "','" + drivername + "','" + party + "','" + fromloc + "','" + toloc + "',GETDATE(),'" + latitiude + "','" + longitude + "','GetReportedate')", con);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {

                        status = true;

                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return status;
        }

        [HttpGet]
        [Route("api/GetDispatchdate/{drivernumber}/{latitiude}/{longitude}")]
        public bool GetDispatchdate(string drivernumber, string latitiude, string longitude)
        {
            bool status = false;
            if (drivernumber != string.Empty && latitiude != string.Empty && longitude != string.Empty)
            {
                try
                {
                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    con.Open();
                    var cmd = new SqlCommand("update SMVTS_MOBILEAPP_ERTRIP set ERTRIP_REACHEDDATE=getdate(),ERTRIP_LATITUDE='" + latitiude + "',ERTRIP_LONGITUDE='" + longitude + "',ERTRIP_PREVIOUS_STATUS='GetDispatchdate' where ERTRIP_MOBILENO='" + drivernumber + "' and ERTRIP_PREVIOUS_STATUS='GetReportedate'", con);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        status = true;
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);

                }
            }
            return status;
        }

        [HttpGet]
        [Route("api/GetReacheddate/{drivernumber}/{latitiude}/{longitude}")]
        public bool GetReacheddate(string drivernumber, string latitiude, string longitude)
        {
            bool status = false;
            if (drivernumber != string.Empty && latitiude != string.Empty && longitude != string.Empty)
            {
                try
                {
                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    con.Open();
                    var cmd = new SqlCommand("update SMVTS_MOBILEAPP_ERTRIP set ERTRIP_REACHEDDATE=getdate(),ERTRIP_LATITUDE='" + latitiude + "',ERTRIP_LONGITUDE='" + longitude + "',ERTRIP_PREVIOUS_STATUS='GetReacheddate' where ERTRIP_MOBILENO='" + drivernumber + "' and ERTRIP_PREVIOUS_STATUS='Getunloadeddate'", con);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        status = true;
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);

                }
            }
            return status;
        }

        [HttpGet]
        [Route("api/Getunloadeddate/{drivernumber}/{latitiude}/{longitude}")]
        public bool Getunloadeddate(string drivernumber, string latitiude, string longitude)
        {
            bool status = false;
            if (drivernumber != string.Empty && latitiude != string.Empty && longitude != string.Empty)
            {
                try
                {
                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    con.Open();
                    var cmd = new SqlCommand("update SMVTS_MOBILEAPP_ERTRIP set ERTRIP_REACHEDDATE=getdate(),ERTRIP_LATITUDE='" + latitiude + "',ERTRIP_LONGITUDE='" + longitude + "',ERTRIP_PREVIOUS_STATUS='Getunloadeddate' where ERTRIP_MOBILENO='" + drivernumber + "' and ERTRIP_PREVIOUS_STATUS='GetDispatchdate'", con);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        status = true;
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);

                }
            }
            return status;
        }

        [HttpPost]
        [Route("api/SAVEIMAGESTARTTRIP/{f}/{mobileno}")]
        public HttpResponseMessage SAVEIMAGESTARTTRIP(byte[] f, string mobileno)
        {
            string status = "false";

            try
            {
                SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                con.Open();
                //string output = "false";
                string fname = string.Empty;
                MemoryStream ms = new MemoryStream(f);
                fname = mobileno + DateTime.Now.ToString("MMM dd yyyy HH mm ss") + ".jpg";
                FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                            ("~/MOBILEAPP_IMGS/") + fname, FileMode.Create);
                ms.WriteTo(fs);
                // clean up 
                ms.Close();
                fs.Close();
                fs.Dispose();
                string url = "~/MOBILEAPP_IMGS/" + fname;
                string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='STARTTRIP',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='" + mobileno + "'";
                SqlCommand comm = new SqlCommand(query, con);

                int r = comm.ExecuteNonQuery();
                if (r > 0)
                {
                    status = "true";
                }
                //MemoryStream ms = new MemoryStream(f);
                ////FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                //FileStream fs = new FileStream(Server.MapPath("~/MOBILEAPP_IMGS/") + fileName, FileMode.Create);
                //SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUc7ZFy6tCuvB4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0="));
                //con.Open();
                //string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='DISPATCH',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='8453777220'";
                //SqlCommand comm = new SqlCommand(query, con);

                //int r = comm.ExecuteNonQuery();
                //if (r > 0)
                //{
                //    ms.WriteTo(fs);
                //    // clean up 
                //    ms.Close();
                //    fs.Close();
                //    fs.Dispose();
                //    // return OK if we made it this far 
                //    status = true;

                //}
            }
            catch (Exception ex)
            {
                // return the error message if the operation fails 
                throw (ex);
            }
            //try
            //{

            //    MemoryStream ms = new MemoryStream(f);
            //    //FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
            //    FileStream fs = new FileStream(Server.MapPath("~/MOBILEAPP_IMGS/") + fileName, FileMode.Create);
            //    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUc7ZFy6tCuvB4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0="));
            //    con.Open();
            //    string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='STARTTRIP',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='" + mobileno + "'";
            //    SqlCommand comm = new SqlCommand(query, con);

            //    int r=comm.ExecuteNonQuery();
            //    if(r>0)
            //    { 
            //    ms.WriteTo(fs);
            //    // clean up 
            //    ms.Close();
            //    fs.Close();
            //    fs.Dispose();
            //    // return OK if we made it this far 
            //    status = true;

            //    }
            //}
            //catch (Exception ex)
            //{
            //    // return the error message if the operation fails 
            //    throw (ex);
            //}

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(status, Encoding.UTF8, "application/json");
            return response;
            //return status;
        }


        [HttpPost]
        [Route("api/SAVEIMAGEDISPATCH/{f}/{mobileno}")]
        public string SAVEIMAGEDISPATCH(byte[] f, string mobileno)
        {
            string status = "false";
            try
            {
                SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                con.Open();
                //string output = "false";
                string fname = string.Empty;
                MemoryStream ms = new MemoryStream(f);
                fname = mobileno + DateTime.Now.ToString("MMM dd yyyy HH mm ss") + ".jpg";
                FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                            ("~/MOBILEAPP_IMGS/") + fname, FileMode.Create);
                ms.WriteTo(fs);
                // clean up 
                ms.Close();
                fs.Close();
                fs.Dispose();
                string url = "~/MOBILEAPP_IMGS/" + fname;
                string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='DISPATCH',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='" + mobileno + "'";
                SqlCommand comm = new SqlCommand(query, con);

                int r = comm.ExecuteNonQuery();
                if (r > 0)
                {
                    status = "true";
                }
                //MemoryStream ms = new MemoryStream(f);
                ////FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                //FileStream fs = new FileStream(Server.MapPath("~/MOBILEAPP_IMGS/") + fileName, FileMode.Create);
                //SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUc7ZFy6tCuvB4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0="));
                //con.Open();
                //string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='DISPATCH',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='8453777220'";
                //SqlCommand comm = new SqlCommand(query, con);

                //int r = comm.ExecuteNonQuery();
                //if (r > 0)
                //{
                //    ms.WriteTo(fs);
                //    // clean up 
                //    ms.Close();
                //    fs.Close();
                //    fs.Dispose();
                //    // return OK if we made it this far 
                //    status = true;

                //}
            }
            catch (Exception ex)
            {
                // return the error message if the operation fails 
                throw (ex);
            }
            return status;
        }

        [HttpPost]
        [Route("api/SAVEIMAGEENDTRIP/{f}/{mobileno}")]
        public string SAVEIMAGEENDTRIP(byte[] f, string mobileno)
        {
            //bool status = false;
            //try
            //{

            //    MemoryStream ms = new MemoryStream(f);
            //    //FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
            //    FileStream fs = new FileStream(Server.MapPath("~/MOBILEAPP_IMGS/") + fileName, FileMode.Create);
            //    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUc7ZFy6tCuvB4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0="));
            //    con.Open();
            //    string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='ENDTRIP',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='8453777220'";
            //    SqlCommand comm = new SqlCommand(query, con);

            //    int r = comm.ExecuteNonQuery();
            //    if (r > 0)
            //    {
            //        ms.WriteTo(fs);
            //        // clean up 
            //        ms.Close();
            //        fs.Close();
            //        fs.Dispose();
            //        // return OK if we made it this far 
            //        status = true;

            //    }
            //}
            //catch (Exception ex)
            //{
            //    // return the error message if the operation fails 
            //    throw (ex);
            //}
            //return status;

            string status = "false";
            try
            {
                SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                con.Open();
                //string output = "false";
                string fname = string.Empty;
                MemoryStream ms = new MemoryStream(f);
                fname = mobileno + DateTime.Now.ToString("MMM dd yyyy HH mm ss") + ".jpg";
                FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                            ("~/MOBILEAPP_IMGS/") + fname, FileMode.Create);
                ms.WriteTo(fs);
                // clean up 
                ms.Close();
                fs.Close();
                fs.Dispose();
                string url = "~/MOBILEAPP_IMGS/" + fname;
                string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='ENDTRIP',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='" + mobileno + "'";
                SqlCommand comm = new SqlCommand(query, con);

                int r = comm.ExecuteNonQuery();
                if (r > 0)
                {
                    status = "true";
                }


            }
            catch (Exception ex)
            {
                // return the error message if the operation fails 
                throw (ex);
            }
            return status;
        }

        [HttpPost]
        [Route("api/SAVEIMAGEUNLOADED/{f}/{mobileno}")]
        public string SAVEIMAGEUNLOADED(byte[] f, string mobileno)
        {
            string status = "false";

            try
            {
                SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                con.Open();
                //string output = "false";
                string fname = string.Empty;
                MemoryStream ms = new MemoryStream(f);
                fname = mobileno + DateTime.Now.ToString("MMM dd yyyy HH mm ss") + ".jpg";
                FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                            ("~/MOBILEAPP_IMGS/") + fname, FileMode.Create);
                ms.WriteTo(fs);
                // clean up 
                ms.Close();
                fs.Close();
                fs.Dispose();
                string url = "~/MOBILEAPP_IMGS/" + fname;
                string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='UNLOADED',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='" + mobileno + "'";
                SqlCommand comm = new SqlCommand(query, con);

                int r = comm.ExecuteNonQuery();
                if (r > 0)
                {
                    status = "true";
                }
                //MemoryStream ms = new MemoryStream(f);
                ////FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                //FileStream fs = new FileStream(Server.MapPath("~/MOBILEAPP_IMGS/") + fileName, FileMode.Create);
                //SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUc7ZFy6tCuvB4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0="));
                //con.Open();
                //string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='DISPATCH',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='8453777220'";
                //SqlCommand comm = new SqlCommand(query, con);

                //int r = comm.ExecuteNonQuery();
                //if (r > 0)
                //{
                //    ms.WriteTo(fs);
                //    // clean up 
                //    ms.Close();
                //    fs.Close();
                //    fs.Dispose();
                //    // return OK if we made it this far 
                //    status = true;

                //}
            }
            catch (Exception ex)
            {
                // return the error message if the operation fails 
                throw (ex);
            }
            //try
            //{

            //    MemoryStream ms = new MemoryStream(f);
            //    //FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
            //    FileStream fs = new FileStream(Server.MapPath("~/MOBILEAPP_IMGS/") + fileName, FileMode.Create);
            //    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUc7ZFy6tCuvB4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0="));
            //    con.Open();
            //    string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='UNLOADED',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='8453777220'";
            //    SqlCommand comm = new SqlCommand(query, con);

            //    int r = comm.ExecuteNonQuery();
            //    if (r > 0)
            //    {
            //        ms.WriteTo(fs);
            //        // clean up 
            //        ms.Close();
            //        fs.Close();
            //        fs.Dispose();
            //        // return OK if we made it this far 
            //        status = true;

            //    }
            //}
            //catch (Exception ex)
            //{
            //    // return the error message if the operation fails 
            //    throw (ex);
            //}
            return status;
        }


        [HttpGet]
        [Route("api/MOBILEAPPVERIFICAION/{mnobno}/{otp}/{imeino}")]
        public bool MOBILEAPPVERIFICAION(string mnobno, string otp, string imeino)
        {
            bool status = false;
            /* DataTable dt = BLL.getverificationdata(mnobno, otp);
             if(dt.Rows.Count>0)
             {
                 status = BLL.senddata(imeino, mnobno,otp);
             }

             return status;*/

            DataTable dt = BLL.install_Activation(imeino, mnobno, otp);
            if (dt.Rows.Count > 0)
            {
                status = Convert.ToBoolean(dt.Rows[0][0]);
            }


            return status;
        }

        [HttpPost]
        [Route("api/MOBILEVERIFY/{mnobno}/{imeino}")]
        public bool MOBILEVERIFY(string mnobno, string imeino)
        {
            bool status = false;
            DataTable dt = BLL.getverify(mnobno, imeino);
            if (dt.Rows.Count > 0)
            {
                status = true;
            }
            return status;
        }

        [HttpGet]
        [Route("api/Dealer_UserReg/{mnobno}/{CategId}")]
        public bool Dealer_UserReg(string mnobno, string CategId)
        {
            bool status = false;
            if (mnobno != "7013841002")
            {

                Random ran = new Random();
                string sa = ran.Next(10000, 99999).ToString();
                DataTable dt = Dal.ExecuteQuery1("exec usp_get_generic_dbname @categid='" + CategId + "'");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        bool r = BLL.dealer_insert_mob_info_ws("UnKnown", mnobno, sa, dt.Rows[0][0].ToString());

                        if (r == true)
                        {

                            string type = "OTP";
                            //send varification code(sa) to mobile
                            string msg = "Dear User, The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app " + dt.Rows[0][2].ToString();
                            //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://localhost:1503//mobiledowloadapk.aspx";
                            // f = sendRequest(masg, mno, type1);
                            // status = sendRequest1(msg, mnobno, type);
                            status = sendRequest_new(msg, mnobno, type);


                        }
                    }
                }

            }
            else
            {
                status = true;
            }
            return status;
        }

        [HttpGet]
        [Route("api/Dealer_UpdateUserInfo/{Name}/{mobileno}/{CategId}")]
        public bool Dealer_UpdateUserInfo(string Name, string mobileno, string CategId)
        {
            bool status = false;
            Random ran = new Random();
            string sa = ran.Next(10000, 99999).ToString();

            DataTable dt = Dal.ExecuteQuery1("exec usp_get_generic_dbname @categid='" + CategId + "'");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    status = BLL.dealer_insert_mob_info_ws("UnKnown", mobileno, sa, dt.Rows[0][0].ToString());
                }
            }

            return status;
        }


        [HttpGet]
        [Route("api/Dealer_MOBILEAPPVERIFICAIION/{mno}/{otp}/{imeino}/{CategId}")]
        public HttpResponseMessage Dealer_MOBILEAPPVERIFICAIION(string mno, string otp, string imeino, string CategId)
        {
            //connected to vts 141 server prod2 database.

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            DataTable dt_DbName = Dal.ExecuteQuery1("exec usp_get_generic_dbname @categid='" + CategId + "'");
            if (dt_DbName != null)
            {
                if (dt_DbName.Rows.Count > 0)
                {

                    DataTable dt = BLL.Dealer_install_Activation2(imeino, mno, otp, dt_DbName.Rows[0][0].ToString(), CategId, dt_DbName.Rows[0][3].ToString());
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
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;

            //return serializer.Serialize(rows);

        }

        [HttpGet]
        [Route("api/Dealer_ClientDashboard/{VehicleNo}/{CategId}")]
        public HttpResponseMessage Dealer_ClientDashboard(string VehicleNo, string CategId)
        {

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            DataTable dt_DbName = Dal.ExecuteQuery1("exec usp_get_generic_dbname @categid='" + CategId + "'");
            if (dt_DbName != null)
            {
                if (dt_DbName.Rows.Count > 0)
                {
                    string Query = "EXEC GET_DASHBOARD_DETAILS2 @VEHICLENO='" + VehicleNo + "',@USERID='" + dt_DbName.Rows[0][1].ToString() + "'";
                    string strConn = dt_DbName.Rows[0][0].ToString();
                    DataTable dat = Dal.ExecuteQueryDB1(Query, strConn);
                    string table = Convert.ToString(dat);


                    foreach (DataRow dr in dat.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dat.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                }
            }

            // return serializer.Serialize(rows);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/Dealer_MOBILEVERIFY/{mnobno}/{imeino}/{CategId}")]
        public bool Dealer_MOBILEVERIFY(string mnobno, string imeino, string CategId)
        {
            bool status = false;

            DataTable dt_DbName = Dal.ExecuteQuery1("exec usp_get_generic_dbname @categid='" + CategId + "'");
            if (dt_DbName != null)
            {
                if (dt_DbName.Rows.Count > 0)
                {

                    DataTable dt = BLL.dealer_getverify2(mnobno, imeino, dt_DbName.Rows[0][0].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        status = true;
                    }
                }

            }
            return status;
        }

        [HttpGet]
        [Route("api/GetReports/{Vno}/{StartDate}/{EndDate}/{ReportType}/{InterVal_Mnts}")]
        public HttpResponseMessage GetReports(string Vno, DateTime StartDate, DateTime EndDate, int ReportType, int InterVal_Mnts)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            DataTable dt_Report = new DataTable();
            //Get DeviceId, User Id, Categ Id based on Vehicle Number

            #region "Engine"
            if (Vno != string.Empty)
            {
                DataTable DT_DBNAME = Dal.ExecuteQuery1("EXEC USP_SMVTS_DBVEHICLES @Vehicleno ='" + Vno + "'");

                if (DT_DBNAME != null)
                {
                    string DeviceId = DT_DBNAME.Rows[0][4].ToString();
                    string Encrypt_DbName = DT_DBNAME.Rows[0][1].ToString();
                    string categID = DT_DBNAME.Rows[0][3].ToString();

                    DataTable DT_uSERID = Dal.ExecuteQueryDB1("USP_GETUSERID @CATEGID='" + categID + "'", Encrypt_DbName);

                    if (DT_uSERID != null)
                    {

                        if (DT_uSERID.Rows.Count > 0)
                        {
                            string Userid = (DT_uSERID.Rows[0][0].ToString());

                            if (ReportType == 1)
                            {
                                string operation_latest = "";

                                dt_Report = get_ReportsData("1", Userid, DeviceId,
                                StartDate.ToString(),
                                 EndDate.ToString(), InterVal_Mnts.ToString(), false, "", operation_latest, Encrypt_DbName);

                            }

                            if (ReportType == 2)
                            {
                                dt_Report = get_TripData(Userid,
                                DeviceId,
                                StartDate.ToString("MM/dd/yyyy hh:mm tt"),
                                 EndDate.ToString("MM/dd/yyyy hh:mm tt"), InterVal_Mnts.ToString(), Encrypt_DbName);
                            }

                            if (ReportType == 3)
                            {

                                dt_Report = get_TripHistory("GETLOC", Userid, categID, DeviceId,
                                                            Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt")
                                                           , Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt"), Encrypt_DbName);

                            }

                        }
                    }
                }
            }
            #endregion

            foreach (DataRow dr in dt_Report.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt_Report.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;

        }


        #region bll For Report

        private DataTable get_ReportsData(string Operation, string strUserID, string strDeviceID, string strStartDate, string strEndDate, string strDuration, bool strShowGeofenceOnly, string strROUTEID, string Category_id, string DBName)
        {


            DataTable dt_RptData = Dal.ExecuteQueryDB1("EXEC [RPT_USP_STOPPAGE] @USERID='" + strUserID + "', @DEVICEID =" +
                                 (strDeviceID == string.Empty ? "null" : "'" + strDeviceID + "'")
                                  + ", @STARTDATE =" + (strStartDate == string.Empty ? "null" : "'" + strStartDate + "'")
                                  + ", @ENDDATE =" + (strEndDate == string.Empty ? "null" : "'" + strEndDate + "'") + ", @DURATION = '" + strDuration + "', @GEOFENCE_ONLY = '" + strShowGeofenceOnly + "', @OPERATION =" +
                                 (Category_id == string.Empty ? "null" : "'" + Category_id + "'"), DBName

                                  );
            return dt_RptData;
        }


        private DataTable get_TripData(string strUserID, string strDeviceID, string strStartDate, string strEndDate, string interval, string DBName)
        {
            //return ExecuteQuery("EXEC RPT_USP_TRIPDATA  @USERID='" + strUserID + "', @DEVICEID =" + (strDeviceID == string.Empty ? "null" : "'" + strDeviceID + "'")
            //                   + ", @STARTDATE =" + (strStartDate == string.Empty ? "null" : "'" + strStartDate + "'") + ", @ENDDATE =" + (strEndDate == string.Empty ? "null" : "'" + strEndDate + "'") + "");
            return Dal.ExecuteQueryDB1("EXEC RPT_USP_TRIPDATA_INTERVAL  @USERID='" + strUserID + "', @DEVICEID =" + (strDeviceID == string.Empty ? "null" : "'" + strDeviceID + "'")
                                + ", @STARTDATE =" + (strStartDate == string.Empty ? "null" : "'" + strStartDate + "'") + ", @ENDDATE =" + (strEndDate == string.Empty ? "null" : "'" + strEndDate + "'") + ", @MININTERVAL=" + interval, DBName);//", @STARTDATE ="
        }


        internal static DataTable get_TripHistory(string Operation, string Users_ID, string Category_ID, string Device_ID, string StartDate, string EndDate, string DBName)
        {
            DataTable dt_TripHistory = new DataTable();
            dt_TripHistory = Dal.ExecuteQueryDB1(" EXEC USP_SMVTS_TRIPHISTORY @Operation ='lOCATION', @DEVICE_ID='" + Device_ID + "', @START_DATE='" + StartDate + "' , @END_DATE = '" + EndDate + "'", DBName);
            return dt_TripHistory;
        }


        #endregion

        [HttpGet]
        [Route("api/FormIds/{CategName}")]
        public HttpResponseMessage FormIds(string CategName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            DataTable dt_FormIds = new DataTable();

            if (CategName != string.Empty)
            {
                dt_FormIds = Dal.ExecuteQuery1("EXEC [USP_SMVTS_USERS] @Operation='GetFormIdsbyCategName', @categName='" + CategName + "'");
                if (dt_FormIds != null)
                {
                    if (dt_FormIds.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_FormIds.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt_FormIds.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                    }
                }
            }
            // return serializer.Serialize(rows);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/Listofvehicles/{mobile_no}/{imei}/{branchname}")]
        public HttpResponseMessage Listofvehicles(string mobile_no, string imei, string branchname)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            bool r = false;
            if (mobile_no != string.Empty)
            {
                try
                {

                    string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
                    SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.CommandText = "select * from SMVTS_ERPL_LOADINGSTAFF where LOADINGSTAFF_MOBILENUMBER='" + mobile_no + "' ";
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.Connection = con;
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        bool f = false;
                        Random ran = new Random();
                        string sa = ran.Next(10000, 99999).ToString();
                        string type = "OTP";
                        string mno = mobile_no;
                        //send varification code(sa) to mobile
                        //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://pragativts.com/mobiledownloadpk.aspx";
                        string msg = "Dear User, Use " + sa + " as one time password (OTP) to login to your ExpressDriverApp account. Valid for 15 Minutes";
                        // string msg = "Dear  customer, The OTP to complete FTS activation is " + sa + ".Click Here to download the app http://localhost:1503//mobiledowloadapk.aspx";
                        //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://localhost:1503//mobiledowloadapk.aspx";
                        f = sendRequest_new(msg, mno, type);
                        // string from_mobileno = "VTSSMS";
                        //string from_mobileno = "BAJICT";
                        ////string udh = "";
                        //string message_text = msg;
                        ////valuefirst
                        //string URL = "http://api.myvaluefirst.com/psms/servlet/psms.Eservice2?data=<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE MESSAGE SYSTEM \"http://127.0.0.1:80/psms/dtd/messagev12.dtd\"><MESSAGE VER=\"1.2\"><USER USERNAME=\"dhanushinfotrns\" PASSWORD=\"dhanush617\"/>";
                        //// drivernumber="9701640330";
                        //string mobileNumbers = mobile_no;

                        //string query_URL = "<SMS  UDH=\"0\" CODING=\"1\" TEXT=\"" + message_text + "\" PROPERTY=\"0\" ID=\"0\">";




                        ////Console.WriteLine("Message sent to" + to_mobileno + " at " + DateTime.Now.ToString() + "");
                        //query_URL = query_URL + "<ADDRESS FROM=\"" + from_mobileno + "\" TO=\"" + mobileNumbers.Trim() + "\" SEQ=\"1\" TAG=\"some clientside random data\"/>";

                        //query_URL = query_URL + "</SMS>";

                        //string Credentails_URL = URL + query_URL + "</MESSAGE>&action=send";

                        //HttpWebRequest request = (HttpWebRequest)
                        //   WebRequest.Create(Credentails_URL);

                        //// execute the request
                        //HttpWebResponse response = (HttpWebResponse)
                        //    request.GetResponse();

                        //// we will read data via the response stream
                        //Stream Answer = response.GetResponseStream();
                        //StreamReader _Answer = new StreamReader(Answer);
                        //string result_value = _Answer.ReadToEnd();
                        ////if (result_value.ToUpper().Trim() == "SENT.")
                        //// {


                        //if (result_value != "")
                        //{

                        //    XmlDocument doc = new XmlDocument();
                        //    doc.LoadXml(result_value);

                        //    XmlNodeList list = doc.SelectNodes("MESSAGEACK");

                        //    foreach (XmlNode xNode in list)
                        //    {
                        //        XmlElement bookElement = (XmlElement)xNode;

                        //        XmlNodeList GUID_XMLS = bookElement.SelectNodes("GUID");

                        //        foreach (XmlNode xNode2 in GUID_XMLS)
                        //        {
                        //            string GUID = xNode2.Attributes["GUID"].Value.ToString();
                        //            string SUBMITDATE = xNode2.Attributes["SUBMITDATE"].Value.ToString();
                        //            int ID = Convert.ToInt32(xNode2.Attributes["ID"].Value.ToString());

                        //            SqlConnection con1 = new SqlConnection("Data Source=192.168.50.141;Initial Catalog=VTS_Config;User ID=smvts;Password=ZCBAVTSDHANUSH2010INDIA");
                        //            con1.Open();
                        //            SqlCommand cmd1 = new SqlCommand("INSERT INTO SMVTS_SMSSERVICEPROVIDERS_RESPONSE VALUES('" + result_value + "',GETDATE(),'VALUEFIRST','0')", con1);
                        //            int s = cmd.ExecuteNonQuery();

                        //            // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, response_query);
                        //            SqlCommand cmd2 = new SqlCommand("INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + mobileNumbers + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')", con1);
                        //            int s1 = cmd2.ExecuteNonQuery();
                        //            r = true;

                        //            //    con.Close();
                        //            // string insrt_query = "INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + to_mobileno + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')";
                        //            // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, insrt_query);

                        //        }
                        //        //string author = bookElement.GetElementsByTagName("author")[0].InnerText;
                        //    }

                        if (f == true)
                        {
                            f = BLL.updateotp(mobile_no, sa, dbname);
                            try
                            {

                                //SqlCommand cmd3 = new SqlCommand();
                                //cmd3.CommandText = "Exec USP_SMVTS_LOADINGVEHICLES @bname='" + branchname + "'";

                                DataTable dt1 = new DataTable();
                                dt1 = Dal.ExecuteQuery3("Exec USP_SMVTS_LOADINGVEHICLES @bname='" + branchname + "'", dbname);
                                //SqlDataAdapter da1 = new SqlDataAdapter(cmd3);
                                //da.SelectCommand.Connection = con;
                                //da.Fill(dt1);
                                //con.Close();
                                if (dt1 != null)
                                {
                                    foreach (DataRow dr in dt1.Rows)
                                    {
                                        row = new Dictionary<string, object>();
                                        foreach (DataColumn col in dt1.Columns)
                                        {
                                            row.Add(col.ColumnName, dr[col]);
                                        }
                                        rows.Add(row);
                                    }
                                    //return serializer.Serialize(rows);
                                   
                                    response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
                                    return response;
                                }

                            }
                            catch (Exception ex)
                            {
                                throw (ex);
                            }

                        }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }


            }
            //return serializer.Serialize(rows);
          
            response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [Route("api/Imageupload/{vehiclenumber}/{mobileno}/{f}")]
        public string Imageupload(string vehiclenumber, string mobileno, byte[] f)
        {
            string status = "false";
            try
            {
                SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
                con.Open();
                string output = "false";
                string fname = string.Empty;
                MemoryStream ms = new MemoryStream(f);
                fname = mobileno + DateTime.Now.ToString("MMM dd yyyy HH mm ss") + ".jpg";
                FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                            ("~/MOBILEAPP_IMGS/") + fname, FileMode.Create);
                ms.WriteTo(fs);
                // clean up 
                ms.Close();
                fs.Close();
                fs.Dispose();
                string url = "~/MOBILEAPP_IMGS/" + fname;
                string query = "Exec USP_SMVTS_IMAGE_UPLOAD @OPERATION='INSERT',@VEHICLENUMBER='" + vehiclenumber + "',@MOBILENUMBER='" + mobileno + "', @IMG_PATH='" + url + "'";
                SqlCommand comm = new SqlCommand(query, con);

                int r = comm.ExecuteNonQuery();
                if (r > 0)
                {
                    status = "true";
                }
            }
            catch (Exception ex)
            {
                // return the error message if the operation fails 
                throw (ex);
            }
            return status;
        }

        [HttpGet]
        [Route("api/Client_Dashboard2/{Org}/{Username}/{Password}")]
        public HttpResponseMessage Client_Dashboard2(string Org, string Username, string Password)
        {
            List<Client_Dashboard> _obj = new List<Client_Dashboard>();
            string RETVAL = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                string dbname = BLL.getDatabase(Org.Trim());
                if (dbname != "")
                {
                    SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();
                    _obj_Smvts_Users = BLL.get_User(Org.Trim(), Username.Trim(), dbname);
                    if (_obj_Smvts_Users != null)
                    {
                        _obj_Smvts_Users.USERS_DBNAME = dbname;
                        if (Username.ToUpper() == _obj_Smvts_Users.USERS_USERNAME.ToUpper() && Password == BLL.Decrypt(_obj_Smvts_Users.USERS_PASSWORD))
                        {
                            string varValue = _obj_Smvts_Users.USERS_MENUITEMS;
                            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
                            DataTable dt = BLL.get_GridTrack_MOBILEAPP_METHODS(_obj_Smvts_GridTrack, Convert.ToString(_obj_Smvts_Users.USERS_ID), dbname);

                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    row = new Dictionary<string, object>();
                            //    foreach (DataColumn col in dt.Columns)
                            //    {
                            //        row.Add(col.ColumnName, dr[col]);
                            //    }
                            //    rows.Add(row);
                            //}
                            if (dt != null)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    row = new Dictionary<string, object>();
                                    _obj.Add(new Client_Dashboard
                                    {
                                        SlNO = Convert.ToInt32(dr["SlNO"]),
                                        VEHICLE_NUMBER = Convert.ToString(dr["VEHICLE_NUMBER"]),
                                        PLACE = Convert.ToString(dr["PLACE"]),
                                        DATE = Convert.ToString(dr["DATE"]),
                                        TRIPDATA_LATITUDE = Convert.ToString(dr["TRIPDATA_LATITUDE"]),
                                        TRIPDATA_LONGITUDE = Convert.ToString(dr["TRIPDATA_LONGITUDE"]),
                                        VEHICLE_COLOR = Convert.ToString(dr["VEHICLE_COLOR"]),
                                        COLOR_STATUS = Convert.ToString(dr["COLOR_STATUS"]),
                                        SPEED = Convert.ToString(dr["SPEED"])
                                    });
                                }

                            }
                        }
                        else
                        {

                            //BLL.ShowMessage(this, "UserName/Password Does not Match !!!");
                        }
                    }
                    else
                    {
                        //BLL.ShowMessage(this, "UserName/Password Does not Match !!!");
                    }
                }
                else
                {

                    //BLL.ShowMessage(this, "Company Name/UserName/Password Does not Match !!!");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(_obj), Encoding.UTF8, "application/json");
            return response;
        }

        public class Client_Dashboard_vehicles
        {
            //public int SlNO { get; set; }
            public string VEHICLE_NUMBER { get; set; }
            public string PLACE { get; set; }
            public string DATE { get; set; }
            public string TRIPDATA_LATITUDE { get; set; }
            public string TRIPDATA_LONGITUDE { get; set; }
            public string VEHICLE_COLOR { get; set; }
            public string SPEED { get; set; }
            public string DRIVERNAME { get; set; }
            public string DRIVER_NUMBER { get; set; }
        }
        public class Client_Dashboard
        {
            public int SlNO { get; set; }
            public string VEHICLE_NUMBER { get; set; }
            public string PLACE { get; set; }
            public string DATE { get; set; }
            public string TRIPDATA_LATITUDE { get; set; }
            public string TRIPDATA_LONGITUDE { get; set; }
            public string VEHICLE_COLOR { get; set; }
            public string SPEED { get; set; }
            public string COLOR_STATUS { get; set; }
        }
        [HttpGet]
        [Route("api/Client_Dashboard_By_VehicleNo/{Org}/{Username}/{Password}/{VehicleNo}")]
        public HttpResponseMessage Client_Dashboard_By_VehicleNo(string Org, string Username, string Password, string VehicleNo)
        {
            List<Client_Dashboard_vehicles> _obj = new List<Client_Dashboard_vehicles>();
            string RETVAL = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                string dbname = BLL.getDatabase(Org.Trim());
                if (dbname != "")
                {
                    SMVTS_USERS _obj_Smvts_Users = new SMVTS_USERS();
                    _obj_Smvts_Users = BLL.get_User(Org.Trim(), Username.Trim(), dbname);
                    if (_obj_Smvts_Users != null)
                    {
                        _obj_Smvts_Users.USERS_DBNAME = dbname;
                        if (Username.ToUpper() == _obj_Smvts_Users.USERS_USERNAME.ToUpper() && Password == BLL.Decrypt(_obj_Smvts_Users.USERS_PASSWORD))
                        {
                            string varValue = _obj_Smvts_Users.USERS_MENUITEMS;
                            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
                            DataTable dt = BLL.get_GridTrack_MOBILEAPP_METHODS_BY_VEHICLENO(_obj_Smvts_GridTrack, Convert.ToString(_obj_Smvts_Users.USERS_ID), dbname, VehicleNo);

                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    row = new Dictionary<string, object>();
                            //    foreach (DataColumn col in dt.Columns)
                            //    {
                            //        row.Add(col.ColumnName, dr[col]);
                            //    }
                            //    rows.Add(row);
                            //}
                            if (dt != null)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    row = new Dictionary<string, object>();
                                    _obj.Add(new Client_Dashboard_vehicles
                                    {
                                        VEHICLE_NUMBER = Convert.ToString(dr["VEHICLE_NUMBER"]),
                                        PLACE = Convert.ToString(dr["PLACE"]),
                                        DATE = Convert.ToString(dr["DATE"]),
                                        TRIPDATA_LATITUDE = Convert.ToString(dr["TRIPDATA_LATITUDE"]),
                                        TRIPDATA_LONGITUDE = Convert.ToString(dr["TRIPDATA_LONGITUDE"]),
                                        VEHICLE_COLOR = Convert.ToString(dr["VEHICLE_COLOR"]),
                                        SPEED = Convert.ToString(dr["SPEED"]),
                                        DRIVERNAME = Convert.ToString(dr["DRIVER_NAME"]),
                                        DRIVER_NUMBER = Convert.ToString(dr["DRIVER_NUMBER"])
                                    });
                                }

                            }
                        }
                        else
                        {

                            //BLL.ShowMessage(this, "UserName/Password Does not Match !!!");
                        }
                    }
                    else
                    {
                        //BLL.ShowMessage(this, "UserName/Password Does not Match !!!");
                    }
                }
                else
                {

                    //BLL.ShowMessage(this, "Company Name/UserName/Password Does not Match !!!");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            // return serializer.Serialize(_obj);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(_obj), Encoding.UTF8, "application/json");
            return response;

        }

        //Get Vehicles
        [HttpGet]
        [Route("api/GetVehicles/{UserID}")]
        public HttpResponseMessage GetVehicles(string UserID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
                DataTable dt = BLL.get_ReportDevices("DEVICES", UserID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        //Get Landmarks
        [HttpGet]
        [Route("api/GetLandmarks/{UserID}")]
        public HttpResponseMessage GetLandmarks(string UserID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getGeofenceLandmarks(UserID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.ToString().ToUpper() == "LANDMARKS_ID")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString().ToUpper() == "LANDMARKS_ADDRESS")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                       
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        //Live Tracking
        [HttpGet]
        [Route("api/LiveTracking/{DeviceID}/{UserID}")]
        public HttpResponseMessage LiveTracking(string DeviceID, string UserID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string RETVAL = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.get_Tracking("GETLOC_TEST", UserID, "", DeviceID);

                if(dt.Rows.Count>0)
                {
                    //Adding StartTrip status
                    DataColumn START_TRIP = new DataColumn();
                    START_TRIP.ColumnName = "START_TRIP";
                    DataTable dt_trip = Dal.ExecuteQuery_Prod("select * From SMVTS_TRIP_ENABLE  where DEVICE_ID=" + DeviceID + "  AND (START_LATTITUDE IS NOT NULL AND START_LONGITUDE IS NOT NULL) AND (END_LATTITUDE IS NULL AND END_LONGITUDE IS  NULL)");
                    if (dt_trip.Rows.Count > 0)
                    {
                        START_TRIP.DefaultValue = true;
                    }
                    else
                    {
                        START_TRIP.DefaultValue = false;
                    }
                    dt.Columns.Add(START_TRIP);

                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        rows.Add(row);
                    }

                    
                   
                    
                }
                
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        //Route Playback
        [HttpGet]
        [Route("api/RoutePlayBack/{DeviceID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage RoutePlayBack(string DeviceID, string StartDate,string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string RETVAL = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                 DataTable  dt = BLL.get_TripHistory("GETLOC", "", "", DeviceID, (StartDate == null ? string.Empty : Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt")), (EndDate == null ? string.Empty : Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt")));
               // DataTable dt = BLL.get_trip_history(DeviceID, Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt"), Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt"));
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        //Distance KM 24 Hours Daily,weekly,Monthly
        [HttpGet]
        [Route("api/MonthlyReport/{UserID}/{DeviceID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage MonthlyReport(int UserID, int DeviceID, string StartDate, string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
          
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                //DataTable  dt = BLL.get_ReportsData("25", UserID,"",(StartDate == null ? string.Empty : StartDate),(EndDate == null ? string.Empty : EndDate), "", false, "", "");
                // DataTable dt = Dal.ExecuteQuery_Prod("exec RPT_USP_VEHICLE_24HRANNALYSISREPORT_APP @USER_ID=" + UserID + ",@RPT_DATE='" + StartDate + "',@DEVID=" + DeviceID + "");

                DataTable dt = BLL.get_DailyreportForApp(UserID, DeviceID, Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt") , Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt"));
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }


        //Stoppage Report
        //[HttpGet]
        //[Route("api/StoppageReport/{UserID}/{DeviceID}/{StartDate}/{EndDate}")]
        //public HttpResponseMessage StoppageReport(string UserID, string DeviceID, string StartDate, string EndDate)
        //{
        //    var resp = new HttpResponseMessage();
        //    var response = Request.CreateResponse(HttpStatusCode.OK);

        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    try
        //    {
        //        DataTable dt = BLL.get_ReportsData("12", UserID, DeviceID, (StartDate == null ? string.Empty : StartDate), (EndDate == null ? string.Empty : EndDate), "", false, "", "");

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            row = new Dictionary<string, object>();
        //            foreach (DataColumn col in dt.Columns)
        //            {
        //                row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
        //            }
        //            rows.Add(row);
        //        }
        //        response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return response;
        //}

        //Geofence Report
        [HttpGet]
        [Route("api/GeofenceReport/{UserID}/{LandmarkID}/{interval}/{StartDate}/{EndDate}/{DeviceID}")]
        public HttpResponseMessage GeofenceReport(string UserID, string LandmarkID, int interval, string StartDate, string EndDate,int DeviceID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = new DataTable();
                //if (Checkgeofence == true)
                //{
                //    dt = Dal.ExecuteQuery_Prod("EXEC USP_CURRENTLYWITHIN_GEOFENCE @USERID='" + UserID + "',@LANDMARKSID='" + LandmarkID + "'");
                //}
                //else
                //{
                //    dt = BLL.get_ReportsData13(UserID,
                //                (StartDate == null ? string.Empty : StartDate),
                //                (EndDate == null ? string.Empty : EndDate),
                //                (LandmarkID == "-1" ? string.Empty : LandmarkID));


                //}
                dt = BLL.get_geofenceData_Mobile(UserID,
                           (StartDate == null ? string.Empty : Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt")),
                           (EndDate == null ? string.Empty : Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt")),
                           (LandmarkID == "-1" ? string.Empty : LandmarkID), interval, DeviceID);
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }


        //Live Status
        [HttpGet]
        [Route("api/LiveStatus/{UserID}")]
        public HttpResponseMessage LiveDashboars(string UserID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string RETVAL = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row= new Dictionary<string, object>();
            try
            {
                List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
                SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
                _obj_Smvts_GridTrack.OPERATION = operation.Select;
              
                DataTable dt = Dal.ExecuteQuery_Prod("EXEC USP_SMVTS_GRID_TRACK_DISTANCE @USER_ID = '" + UserID + "'");
                int T = 0;
                int M = 0;
                int S = 0;
                int Y = 0;

                string result = "";
                if(dt.Rows.Count>0)
                {
                    T = Convert.ToInt32(dt.Rows.Count);
                    for(int i=0;i<dt.Rows.Count;i++)
                    {
                        if (dt.Rows[i]["vehicle_color"].ToString()=="G")
                        {
                            M++;
                        }
                        else if(dt.Rows[i]["vehicle_color"].ToString() == "R")
                        {
                            S++;
                        }
                        else if(dt.Rows[i]["vehicle_color"].ToString() == "Y")
                        {
                            Y++;
                        }
                    }
                    result = "Moving:" + M + ",Stopped:" + S + ",DataNotReceiving:" + Y + ",Total:" + T;
                    
                    row.Add("Moving", M);
                 
                    row.Add("Stopped", S);
                
                    row.Add("DataNotReceiving", Y);
                    rows.Add(row);
                }
                else
                {
                    result = "NoData";
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        //Get Device Order Status
        [HttpGet]
        [Route("api/GetOrderStatus/{UserID}")]
        public HttpResponseMessage getDeviceOrders(string UserID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string RETVAL = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
                SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
                _obj_Smvts_GridTrack.OPERATION = operation.Select;

                DataTable dt = Dal.ExecuteQuery_Prod("EXEC USP_SMVTS_GRID_TRACK_DISTANCE @USER_ID = '" + UserID + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)        
                    {
                        if(col.ColumnName.ToString().ToUpper()== "VEHICLE_NUMBER")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }


                        else if(col.ColumnName.ToString().ToUpper() == "STARTDATE")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        else if(col.ColumnName.ToString().ToUpper() == "EXPDATE")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        else if (col.ColumnName.ToString().ToUpper() == "DEVICEID")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        //Save Device Tokens Dt:28-08-2019
        //[HttpPost]
        //[Route("api/SaveDeviceTokens/{UserID}/{Token}")]
        //public HttpResponseMessage SaveDeviceTokens(string UserID,string Token)
        //{
        //    var resp = new HttpResponseMessage();
        //    var response = Request.CreateResponse(HttpStatusCode.OK);
            
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row = new Dictionary<string, object>();
        //    try
        //    {
        //        DateTime date = DateTime.Now;
        //        DataTable dt = new DataTable();

        //        bool token = BLL.checkTokens(UserID, Token);
        //        if (token==true)
        //        {
        //            row.Add("Output", "Success");

        //        }
        //       else
        //        {
        //            row.Add("Output", "Failed");

        //        }
        //        rows.Add(row);
        //        response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
        //    }

        //    return response;
        //}

        [HttpPost]
        [Route("api/SaveDeviceTokens")]
        public HttpResponseMessage SaveDeviceTokens(DevieTokens obj)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            try
            {
                DateTime date = DateTime.Now;
                DataTable dt = new DataTable();

                bool token = BLL.checkTokens(obj.UserId, obj.Token);
                if (token == true)
                {
                    row.Add("Output", "Success");

                }
                else
                {
                    row.Add("Output", "Failed");

                }
                rows.Add(row);
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }


        //Push Notifications Dt:28-08-2019
        [HttpPost]
        [Route("api/PushNotifications")]
        public async System.Threading.Tasks.Task<bool> PushNotifications(SendNotifications obj)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            try
            {
                // Get the server key from FCM console
                var serverKey = string.Format("key={0}", "AAAAAQBw-q0:APA91bEWNOI3s9vx-rNcJJFZ-mTi5YCkCjHYxlWaj6CJBAlorwFmBYn8aqsv-SXS3IzWo405IftrszVJKYgR5fRRiEv3pkpfPo5iXAxMZYuvnPmegCVQ0yvPHxJGE1S8Pnzm3d_E0Wjb");

                // Get the sender id from FCM console
                var senderId = string.Format("id={0}", "4302371501");

                var data = new
                {
                    to = obj.Token, // Recipient device token
                   notification = new { title= "New Notification!", body=obj.Message }
                };

                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(data);


                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.Add("Authorization", serverKey);
                    httpRequest.Headers.Add("Sender", senderId);
                  
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            //_logger.LogError($"Error sending notification. Status Code: {result.StatusCode}");
                        }
                    }
                }


                rows.Add(row);
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return false;
        }

        [HttpGet]
        [Route("api/notification")]
        public string notification()
        {
            string status = "";
            DataTable dt_tokens = Dal.ExecuteQuery_Prod("select DEVICE_TOKEN  from SMVTS_DEVICE_TOKENS where USER_ID=6");
            if(dt_tokens.Rows.Count>0)
            {
                for(int i=0;i<dt_tokens.Rows.Count;i++)
                {
                    string[] tokens = dt_tokens.Rows[i]["DEVICE_TOKEN"].ToString().Split(',');
                    for (int j = 0; j < tokens.Length; j++)
                    {
                        SendNotifications obj_noti = new SendNotifications();
                        obj_noti.Token = tokens[j];
                        obj_noti.Message = "Hi all Please Update Tranopro App";
                        status = SendNotification(obj_noti);

                    }
                    
                }
            }
            return status;
        }
        [HttpPost]
        [Route("api/SendNotifications")]
        public string SendNotification(SendNotifications obj)
        {
            try
            {


                var result = "-1";
                string webAddr = "https://fcm.googleapis.com/fcm/send";
                // Get the server key from FCM console
              //  var serverKey = string.Format("key={0}", "AAAAAQBw-q0:APA91bEWNOI3s9vx-rNcJJFZ-mTi5YCkCjHYxlWaj6CJBAlorwFmBYn8aqsv-SXS3IzWo405IftrszVJKYgR5fRRiEv3pkpfPo5iXAxMZYuvnPmegCVQ0yvPHxJGE1S8Pnzm3d_E0Wjb");
                string serverKey = "AAAAAQBw-q0:APA91bEWNOI3s9vx-rNcJJFZ-mTi5YCkCjHYxlWaj6CJBAlorwFmBYn8aqsv-SXS3IzWo405IftrszVJKYgR5fRRiEv3pkpfPo5iXAxMZYuvnPmegCVQ0yvPHxJGE1S8Pnzm3d_E0Wjb";
                // Get the sender id from FCM console
                //var senderId = string.Format("id={0}", "4302371501");

                string senderId = "4302371501";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                httpWebRequest.Method = "POST";

                //var payload = new
                //{
                //    to = obj.Token,
                //    priority = "high",
                //    content_available = true,
                //    notification = new
                //    {
                //        body = obj.Message,
                //        title = "CTPL"
                //    },
                //};
                
                
                string jsonData = "{\"to\": \"" + obj.Token + "\",\"data\": {\"message\": \"" + obj.Message + "\",\"title\":\"CTPL\"}}";
                var serializer = new JavaScriptSerializer();
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = serializer.Serialize(jsonData);
                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }


        [HttpGet]
        [Route("api/MyVehicles/{UserID}")]
        public HttpResponseMessage MyVehicles(string UserID)
       {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getVehiclesForAPP(UserID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        //Distnce Report Monthly wise
        [HttpGet]
        [Route("api/MonthlySummaryReport/{UserID}/{DeviceID}/{Month}")]
        public HttpResponseMessage MonthlySummaryReport(int UserID,int DeviceID, string Month)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            Dictionary<string, object> location; Dictionary<string, object> parameters;
            List<Dictionary<string, object>> distanceKms = new List<Dictionary<string, object>>();
           
            try
            {
                DataTable dt = new DataTable();
                DateTime date = DateTime.Now;
                string year = date.Year.ToString();
                // string startdate= date.ToString("MM-dd-yyyy");
                string startdate = Month + "-" + year;
                DataTable dt_categ = Dal.ExecuteQuery_Prod("select users_category_id from smvts_users where users_id='" + UserID + "'");
                int categid = Convert.ToInt32(dt_categ.Rows[0][0]);

             //   dt = BLL.Get_Monthly_Report_APP(UserID, categid, startdate, DeviceID);
                dt = BLL.Get_Monthly_Report_APP_New(UserID, categid, startdate, DeviceID);
                row = new Dictionary<string, object>();
                location = new Dictionary<string, object>();
                //parameters = new Dictionary<string, object>();

                //foreach (DataRow dr in dt.Rows)
                //{
                //    row = new Dictionary<string, object>();
                //    foreach (DataColumn col in dt.Columns)
                //    {

                //        if(col.ColumnName.ToString().ToUpper().Contains("KMS"))
                //        {
                //            parameters = new Dictionary<string, object>();
                //            parameters.Add("Date", col.ColumnName.ToString().ToUpper().Replace("KMS",""));
                //            parameters.Add("DistanceTravelled", dr[col].ToString().ToUpper());
                //            distanceKms.Add(parameters);
                //        }
                //        else
                //        {
                //            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                //        }

                //    }

                //}
                string Vehicle_No = "", VehicleType = "";
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    parameters = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                       
                        if (col.ColumnName.ToString().ToUpper().Contains("VEHICLENO"))
                        {
                            Vehicle_No = dr[col].ToString().ToUpper();
                            
                        }
                     else if (col.ColumnName.ToString().ToUpper().Contains("VEHICLE_TYPE"))
                        {
                            VehicleType = dr[col].ToString().ToUpper();
                            
                        }
                        else
                        {

                            // parameters.Add("Date", col.ColumnName.ToString().ToUpper().Replace("KMS", ""));
                            // parameters.Add("DistanceTravelled", dr[col].ToString().ToUpper());
                            parameters.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                            
                        }
                        
                    }
                    if(parameters.Count()!=0)
                    {
                        distanceKms.Add(parameters);
                    }
                   
                }
                row.Add("VEHICLENO", Vehicle_No);
                row.Add("VEHICLE_TYPE", VehicleType);
                row.Add("DistanceData", distanceKms);
                rows.Add(row);
               
                
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        //Distnce Report Day wise
        [HttpGet]
        [Route("api/DistanceKMS/{UserID}/{DeviceID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage DistanceKMS(int UserID, int DeviceID, string StartDate,string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            Dictionary<string, object> location; Dictionary<string, object> parameters;
            List<Dictionary<string, object>> distanceKms = new List<Dictionary<string, object>>();

            try
            {
                DataTable dt = new DataTable();
                
                DataTable dt_categ = Dal.ExecuteQuery_Prod("select users_category_id from smvts_users where users_id='" + UserID + "'");
                int categid = Convert.ToInt32(dt_categ.Rows[0][0]);

                //   dt = BLL.Get_Monthly_Report_APP(UserID, categid, startdate, DeviceID);
                dt = BLL.Get_Distance_Report_APP_New(UserID, categid, StartDate, EndDate,DeviceID);
                row = new Dictionary<string, object>();
                location = new Dictionary<string, object>();
                //parameters = new Dictionary<string, object>();

                //foreach (DataRow dr in dt.Rows)
                //{
                //    row = new Dictionary<string, object>();
                //    foreach (DataColumn col in dt.Columns)
                //    {

                //        if(col.ColumnName.ToString().ToUpper().Contains("KMS"))
                //        {
                //            parameters = new Dictionary<string, object>();
                //            parameters.Add("Date", col.ColumnName.ToString().ToUpper().Replace("KMS",""));
                //            parameters.Add("DistanceTravelled", dr[col].ToString().ToUpper());
                //            distanceKms.Add(parameters);
                //        }
                //        else
                //        {
                //            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                //        }

                //    }

                //}
                string Vehicle_No = "", VehicleType = "";
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    parameters = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {

                        if (col.ColumnName.ToString().ToUpper().Contains("VEHICLENO"))
                        {
                            Vehicle_No = dr[col].ToString().ToUpper();

                        }
                        else if (col.ColumnName.ToString().ToUpper().Contains("VEHICLE_TYPE"))
                        {
                            VehicleType = dr[col].ToString().ToUpper();

                        }
                        else
                        {

                            // parameters.Add("Date", col.ColumnName.ToString().ToUpper().Replace("KMS", ""));
                            // parameters.Add("DistanceTravelled", dr[col].ToString().ToUpper());
                            parameters.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());

                        }

                    }
                    if (parameters.Count() != 0)
                    {
                        distanceKms.Add(parameters);
                    }

                }
                row.Add("VEHICLENO", Vehicle_No);
                row.Add("VEHICLE_TYPE", VehicleType);
                row.Add("DistanceData", distanceKms);
                rows.Add(row);


                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        [HttpGet]
        [Route("api/IgnitionReport/{UserID}/{DeviceID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage IgnitionReport(int UserID, int DeviceID,string StartDate,string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.Ignition_Report_APP(UserID, DeviceID, StartDate, EndDate);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/GetNotificationsByUser/{UserID}")]
        public HttpResponseMessage getNotifications(int UserID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getNotificationsByUser(UserID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/GetNotificationsByVehicle/{UserID}/{DeviceID}")]
        public HttpResponseMessage getNotificationsByVehicle(int UserID,int DeviceID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getNotificationsByVehicle(UserID, DeviceID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }


        [HttpGet]
        [Route("api/GetNotificationsByDate/{UserID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage getNotificationsByDate(int UserID, string StartDate,string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getNotificationsByDate(UserID, StartDate, EndDate);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpPost]
        [Route("api/LoginUser/{Key}/{Username}/{Password}")]
        public HttpResponseMessage getLoginUser(string Key,string Username,string Password)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt_key = BLL.getIOSkey(Key);
                row = new Dictionary<string, object>();
                if (dt_key.Rows.Count>0)
                {
                    List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
                    var data = BLL.getDatabaseForUser(Username, Password);

                    if(data.Rows.Count>0)
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
                                    HttpContext.Current.Response.Cookies.Set(cookie);
                                    //user name and password binded on textbox that purpose
                                    HttpCookie cookie1 = new HttpCookie("namepassword");
                                    cookie1.Values.Add("user_N", Username);
                                    cookie1.Values.Add("user_P", Password);
                                    cookie1.Values.Add("user_C", CompanyName);
                                    cookie1.Expires = DateTime.Now.AddMonths(6);
                                    HttpContext.Current.Response.Cookies.Set(cookie1);

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

                               // row.Add("OutPut", "http://www.tranopro.com/HOME/frm_GridTrack_Distance");
                                string url = "http://www.tranopro.com/HOME/frm_GridTrack_Distance";
                                response.Headers.Location = new Uri(url);
                            }
                        }
                        else
                        {
                            //row.Add("OutPut", "Invalid UserName/Password");
                            response.Content =new StringContent("Invalid UserName/Password");
                        }
                        
                    }
                    else
                    {
                        // row.Add("OutPut", "Invalid UserName/Password");
                        response.Content = new StringContent("Invalid UserName/Password");
                    }
                }
                else
                {
                    //row.Add("OutPut", "Invalid Key");
                    response.Content = new StringContent("Invalid UserName/Password");
                }
               // rows.Add(row);
               // response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");

            }

            return response;
        }


        [HttpGet]
        [Route("api/StartTrip/{UserID}/{DeviceID}")]
        public HttpResponseMessage starttrip(int UserID, int DeviceID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                int categid = 0;string result = "";
                DataTable dt = Dal.ExecuteQuery_Prod("select categ_id from SMVTS_CATEGORIES c inner join SMVTS_USERS u on c.categ_id=u.USERS_CATEGORY_ID  where USERS_ID="+ UserID + "");
                if(dt.Rows.Count>0)
                {
                    categid = Convert.ToInt32(dt.Rows[0][0]);
                }
                DataTable dt_starttrip = BLL.getStartTrip(categid, DeviceID);
                if(dt_starttrip.Rows.Count>0)
                {
                    result = "Trip is already Started please End the Trip First";
                }
                else
                {
                    bool status = BLL.StartTrip(UserID, categid, DeviceID);
                    if(status==true)
                    {
                        result = "true";
                    }
                    else
                    {
                        result = "false";
                    }
                }
                
                
                response.Content = new StringContent(serializer.Serialize(result), Encoding.UTF8, "application/json");
            }



            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/EndTrip/{UserID}/{DeviceID}")]
        public HttpResponseMessage EndTrip(int UserID, int DeviceID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                int categid = 0; string result = "";
                DataTable dt = Dal.ExecuteQuery_Prod("select categ_id from SMVTS_CATEGORIES c inner join SMVTS_USERS u on c.categ_id=u.USERS_CATEGORY_ID  where USERS_ID=" + UserID + "");
                if (dt.Rows.Count > 0)
                {
                    categid = Convert.ToInt32(dt.Rows[0][0]);
                }
                DataTable dt_starttrip = BLL.getStartTrip(categid, DeviceID);
                if (dt_starttrip.Rows.Count > 0)
                {
                    bool status = BLL.EndTrip(UserID, categid, DeviceID);
                    if(status==true)
                    {
                        result = "Success";
                    }
                    else
                    {
                        result = "Failed";
                    }
                }
                else
                {
                    result = "Trip is Not Strated";
                }


                response.Content = new StringContent(serializer.Serialize(result), Encoding.UTF8, "application/json");
            }



            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/GetInboxMessages/{UserID}")]
        public HttpResponseMessage getMessages(int UserID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getMessagesByCustomer(UserID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/GetTripReport/{UserID}/{DeviceID}")]
        public HttpResponseMessage gettripdata(int UserID,int DeviceID)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {

                int categid = 0; string result = "";
                DataTable dt = Dal.ExecuteQuery_Prod("select categ_id from SMVTS_CATEGORIES c inner join SMVTS_USERS u on c.categ_id=u.USERS_CATEGORY_ID  where USERS_ID=" + UserID + "");
                if (dt.Rows.Count > 0)
                {
                    categid = Convert.ToInt32(dt.Rows[0][0]);
                }
                DataTable dt_trip = BLL.getTripReport(categid, DeviceID);

                foreach (DataRow dr in dt_trip.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt_trip.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }


        [HttpGet]
        [Route("api/GetUserDetails/{UserName}/{Password}")]
        public HttpResponseMessage getUserData(string UserName,string Password)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            string pass_word = BLL.Encrypt(Password);
            try
            {
                DataTable dt = Dal.ExecuteQuery_Prod("select c.CATEG_NAME as NAME,c.CATEG_ADDRESS As ADDRESS,u.USERS_USERNAME as USERNAME,u.USERS_ID,(select count(*) from smvts_vehicles where VEHICLES_CATEGORY_ID=c.CATEG_ID) as NumOfVehicles from smvts_users u inner join SMVTS_CATEGORIES c on u.USERS_CATEGORY_ID=c.CATEG_ID  where u.USERS_USERNAME='" + UserName + "' AND u.USERS_PASSWORD='" + pass_word + "'");

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        //Save app version Dt:07-11-2019
        [HttpGet]
        [Route("api/SaveAppVersions/{CurrentVersion}")]
        public HttpResponseMessage SaveAppVersions(string CurrentVersion)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            try
            {

                string query = "update SMVTS_APP_VERSIONS set current_app_version='" + CurrentVersion + "',modifieddate='" + DateTime.Now.ToString() + "'";
                bool status = Dal.ExecuteNonQuery1(query);

                DataTable dt = BLL.ExecuteQuery1("select current_app_version,critical_app_version,convert(varchar,convert(datetime,modifieddate,100),0) as UpdatedDate from SMVTS_APP_VERSIONS");
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }

               
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }


        //For Save MIS Packages
        [HttpGet]
        [Route("api/SavePackages/{PackageName}/{Price}/{gst}/{Num_Days}/{Code}")]
        public HttpResponseMessage SavePackages(string PackageName,string Price,string gst,string Num_Days,string Code)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            try
            {

                  bool status = BLL.SaveMisPackages(PackageName, Price,gst, Num_Days, Code);

                response.Content = new StringContent(serializer.Serialize(status), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }
        [HttpGet]
        [Route("api/AcReport/{UserID}/{DeviceID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage AcReport(int UserID, int DeviceID, string StartDate, string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getAcReport(UserID, DeviceID.ToString(), Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt"), Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt"));

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/StoppageReport/{UserID}/{DeviceID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage StoppageReport(int UserID, int DeviceID, string StartDate, string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                //DataTable dt = BLL.getAcReport(UserID, DeviceID.ToString(), Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt"), Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt"));
                DataTable dt = Dal.ExecuteQuery_Prod("EXEC [RPT_USP_STOPPAGE] @USERID='" + UserID + "', @DEVICEID =" +
                    (DeviceID.ToString() == string.Empty ? "null" : "'" + DeviceID.ToString() + "'")
                     + ", @STARTDATE =" + (StartDate == string.Empty ? "null" : "'" + Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt") + "'")
                     + ", @ENDDATE =" + (EndDate == string.Empty ? "null" : "'" + Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt") + "'"));
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.ToString() == "vehiclenumber")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "STARTDATE")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "ENDDATE")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "timediff")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "LandmarksAddress")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "lattitude")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "longitude")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/OverSpeedReport/{UserID}/{DeviceID}/{StartDate}/{EndDate}")]
        public HttpResponseMessage OverSppedReport(int UserID, int DeviceID, string StartDate, string EndDate)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                //DataTable dt = BLL.getAcReport(UserID, DeviceID.ToString(), Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt"), Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt"));
                DataTable dt = Dal.ExecuteQuery_Prod("EXEC RPT_USP_OVERSPEED  @USERID='" + UserID + "', @DEVICEID =" + (DeviceID.ToString() == string.Empty ? "null" : "'" + DeviceID + "'")
                                                            + ", @STARTDATE =" + (StartDate == string.Empty ? "null" : "'" + Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy hh:mm tt") + "'") + ", @ENDDATE =" + (EndDate == string.Empty ? "null" : "'" + Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy hh:mm tt") + "'") + "");
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.ToString() == "REGNUMBER")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "LANDMARKS_ADDRESS")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        //if (col.ColumnName.ToString() == "AVGSPEED")
                        //{
                        //    row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        //}
                        if (col.ColumnName.ToString() == "MAXSPEED")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "OVERSPEED_TRIP_MAXSPEED")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "TRIPDATA_LATITUDE")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }
                        if (col.ColumnName.ToString() == "TRIPDATA_LONGITUDE")
                        {
                            row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        }

                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpPost]
        [Route("api/EditDriverDetails/{DeviceID}/{DriverName}/{DriverMobileNumber}")]
        public HttpResponseMessage EditDriver(int DeviceID, string DriverName, string DriverMobileNumber)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string result = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                bool status = BLL.UpdateDriverDetails(DeviceID, DriverName, DriverMobileNumber);
               
                    if (status == true)
                    {
                        result = "true";
                    }
                    else
                    {
                        result = "false";
                    }
                response.Content = new StringContent(serializer.Serialize(result), Encoding.UTF8, "application/json");
            }
            catch(Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }
        [HttpPost]
        [Route("api/RechargeRequest/{UserID}/{DeviceId}")]
        public HttpResponseMessage RechargeRequest(int UserID, int DeviceId)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string result = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            string to_address = "prashant.a@containe.in,care@containe.in";
            string[] multi = to_address.Split(',');
            foreach(string mailid in multi)
            {
                mail.To.Add(mailid);
            }
            DataTable dt_customer = BLL.getCustomerAndVehicle(UserID, DeviceId);
            if (dt_customer.Rows.Count > 0)
            {
                mail.From = new MailAddress("tranocare@gmail.com", "Email From Tranopro App", System.Text.Encoding.UTF8);
                mail.Subject = "Recharge Request from App";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "Hi Mr. " + dt_customer.Rows[0]["CATEG_NAME"].ToString() + " has requested a gps recharge for vehicle " + dt_customer.Rows[0]["VEHICLES_REGNUMBER"].ToString() + "";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("tranocare@gmail.com", "trano@1234");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                // client.Host = "smtp.rediffmailpro.com";
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);
                    result = "sent";
                    response.Content = new StringContent(serializer.Serialize(result), Encoding.UTF8, "application/json");
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    string errorMessage = string.Empty;
                    while (ex2 != null)
                    {
                        errorMessage += ex2.ToString();
                        ex2 = ex2.InnerException;
                    }
                    // result = "failed";
                    response.Content = new StringContent(serializer.Serialize(ex2), Encoding.UTF8, "application/json");
                }
            }
            else
            {
                result = "No Data Found";
                response.Content = new StringContent(serializer.Serialize(result), Encoding.UTF8, "application/json");
            }
            return response;
        }

       
        [HttpGet]
        [Route("api/SendAlerts")]
        public HttpResponseMessage SendAlerts()
        {
            var resp = new HttpResponseMessage();
            var response2 = Request.CreateResponse(HttpStatusCode.OK);
            string result = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            string responseString = "";
           

            int[] arr1 = { 1, 7, 15 };
            for(int i=0; i<arr1.Length; i++)
            {
                DataTable dt = BLL.get_order_exp(arr1[i]);
                if(dt.Rows.Count>0)
                {
                    for(int j=0; j<dt.Rows.Count; j++)
                    {
                        string message = "Dear " + dt.Rows[j]["CUSTOMER_NAME"].ToString() + " your vehicle " + dt.Rows[j]["VEHICLES_REGNUMBER"].ToString() + " GPS Device Renewal Date is Expiring on " + dt.Rows[j]["EXP_DATE"].ToString() + ".Renewal it for uninterrupted services,Please ignore if already done.Team-Tranopro Call:9100777438";
                        string url = "http://173.45.76.227/send.aspx?username=contdemo&pass=cont1234&route=trans1&senderid=eTRANO&numbers="+ dt.Rows[j]["CATEG_MOBILENUMBER"].ToString() + "&message=" + message + "";
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

                            if (result_value.ToUpper().Trim() == "4")
                            {
                                responseString = "Failed,Invalid Mobile Number";
                            }
                            else if(result_value.ToUpper().Trim() == "2")
                            {
                                responseString = "Failed,Invalid Username/Password";
                            }
                            else if(result_value.Length>5)
                            {
                                responseString = "Success";
                            }
                            _Answer.Close();
                            response.Close();
                            bool status = BLL.save_alerts_log(dt.Rows[j]["CATEG_MOBILENUMBER"].ToString(), dt.Rows[j]["CUSTOMER_NAME"].ToString(), dt.Rows[j]["VEHICLES_REGNUMBER"].ToString(), message, responseString);
                            response2.Content = new StringContent(serializer.Serialize(responseString), Encoding.UTF8, "application/json");
                        }
                        catch (Exception ex)
                        {
                            bool status = BLL.save_alerts_log(dt.Rows[j]["CATEG_MOBILENUMBER"].ToString(), dt.Rows[j]["CUSTOMER_NAME"].ToString(), dt.Rows[j]["VEHICLES_REGNUMBER"].ToString(), message, ex.ToString());
                        }
                    }
                }
            }
            

           
            return response2;
        }

        [HttpGet]
        [Route("api/GetAllLandmarks/{CategId}")]
        public List<SMVTS_LANDMARKS> GetAllLandmarks(int CategId)
        {
            var param ="";
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            SMVTS_LANDMARKS _obj_Smvts_LandMarks = new SMVTS_LANDMARKS();
            _obj_Smvts_LandMarks.OPERATION = operation.Empty;
            _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = CategId;

            HomeController hc = new HomeController();
            
            return hc.getLandmarksDataObj1(_obj_Smvts_LandMarks);
        }

        //[HttpPost]
        //[Route("api/SaveLandmarks/{Userid}/{Latitude}/{Logitude}/{Adress}/{}")]
        //public List<SMVTS_LANDMARKS> SaveLandmarks(int Userid, string Latitude)
        //{
        //    var resp = new HttpResponseMessage();
        //    var response = Request.CreateResponse(HttpStatusCode.Moved);
        //    SMVTS_LANDMARKS obj = new SMVTS_LANDMARKS();
        //    obj.LANDMARKS_ADDRESS = Latitude;
        //    string btype = "save";

        //    HomeController hc = new HomeController();

        //    return hc.SaveLandmarksapp(obj,btype);
        //}
        [HttpGet]
        [Route("api/Landmarkslocations")]
        public HttpResponseMessage Landmarkslocations()
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                SMVTS_LOCATIONTYPE obj_loctype = new SMVTS_LOCATIONTYPE();
                obj_loctype.OPERATION = operation.Select;
                DataTable dt = new DataTable();
                //Get Location Type
                dt = BLL.get_LocationTypeapp(obj_loctype);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }


        [HttpPost]
        [Route("api/SaveLandmarks/{USERS_ID}/{LANDMARKS_LATITUDE}/{LANDMARKS_LONGITUDE}/{LANDMARKS_POLYPOINTS}/{LANDMARKS_GEOFENCETYPE}/{LANDMARKS_ADDRESS}/{LANDMARKS_LOCATIONTYPE}/{LANDMARKS_STATUS}/{LANDMARKS_GEOSTATUS}/{RADIUS}/{LANDMARKS_STATE}/{LANDMARKS_ZONE}/{LANDMARKS_NEARCITY}")]
        public HttpResponseMessage SaveLandmarks14(int USERS_ID, string LANDMARKS_LATITUDE,string LANDMARKS_LONGITUDE, string LANDMARKS_POLYPOINTS,string LANDMARKS_GEOFENCETYPE,string LANDMARKS_ADDRESS,string LANDMARKS_LOCATIONTYPE,string LANDMARKS_STATUS,bool LANDMARKS_GEOSTATUS,double RADIUS,string LANDMARKS_STATE,string LANDMARKS_ZONE,string LANDMARKS_NEARCITY)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            SMVTS_LANDMARKS obj = new SMVTS_LANDMARKS();
            //obj.LANDMARKS_ADDRESS = Latitude;
            string status = "";

            DataTable DT = new DataTable();
            int count = 0;

            if (LANDMARKS_POLYPOINTS != null && LANDMARKS_POLYPOINTS != "")
            {
                LANDMARKS_POLYPOINTS = LANDMARKS_POLYPOINTS.Substring(0,LANDMARKS_POLYPOINTS.ToString().Length - 1);
            }
            SMVTS_LANDMARKS _obj_Smvts_LandMarks = new SMVTS_LANDMARKS();
            _obj_Smvts_LandMarks.LANDMARKS_LATITUDE = BLL.ReplaceQuote(LANDMARKS_LATITUDE.ToString().Trim());
            _obj_Smvts_LandMarks.LANDMARKS_LONGITUDE = BLL.ReplaceQuote(LANDMARKS_LONGITUDE.ToString().Trim());
            _obj_Smvts_LandMarks.OPERATION = operation.Update;
            DataTable dt = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
            _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = Convert.ToInt32(dt.Rows[0][0]);
            _obj_Smvts_LandMarks.LANDMARKS_GEOFENCETYPE = Convert.ToInt32(LANDMARKS_GEOFENCETYPE);
            _obj_Smvts_LandMarks.LANDMARKS_POLYPOINTS = LANDMARKS_POLYPOINTS.ToString();
            _obj_Smvts_LandMarks.LANDMARKS_TYPE = 2;
            _obj_Smvts_LandMarks.LANDMARKS_CATEGORY_ID = Convert.ToInt32(dt.Rows[0][0]);
            _obj_Smvts_LandMarks.LANDMARKS_ADDRESS = BLL.ReplaceQuote(LANDMARKS_ADDRESS.ToString().Trim());

            DT = BLL.get_LandMarks1(_obj_Smvts_LandMarks);

            if (DT.Rows.Count > 0)
            {

                count = Convert.ToInt32(DT.Rows[0][0]);

            }

            _obj_Smvts_LandMarks.LANDMARKS_LOCATIONTYPE = Convert.ToInt32(LANDMARKS_LOCATIONTYPE);
            _obj_Smvts_LandMarks.LANDMARKS_CONTPERSONS = null;
            _obj_Smvts_LandMarks.LANDMARKS_MOBILENUMBER = null;
            _obj_Smvts_LandMarks.LANDMARKS_TYPE = 2;

            _obj_Smvts_LandMarks.LANDMARKS_STATUS = Convert.ToBoolean(LANDMARKS_STATUS);
            _obj_Smvts_LandMarks.CREATEDBY = USERS_ID;
            _obj_Smvts_LandMarks.CREATEDDATE = DateTime.Now;
            _obj_Smvts_LandMarks.LASTMDFBY = USERS_ID;
            _obj_Smvts_LandMarks.LASTMDFDATE = DateTime.Now;
            _obj_Smvts_LandMarks.LANDMARKS_GEOSTATUS = LANDMARKS_GEOSTATUS;
            _obj_Smvts_LandMarks.RADIUS = RADIUS;

            _obj_Smvts_LandMarks.LANDMARKS_STATE = LANDMARKS_STATE.ToString();
            _obj_Smvts_LandMarks.LANDMARKS_ZONE = LANDMARKS_ZONE.ToString();
            _obj_Smvts_LandMarks.LANDMARKS_NEARCITY = LANDMARKS_NEARCITY.ToString();


            //var gepfenceid = SaveGeofenceInTraccar(LANDMARKS_LATITUDE, LANDMARKS_LONGITUDE, RADIUS, LANDMARKS_ADDRESS);
            //_obj_Smvts_LandMarks.LANDMARK_GEOFENCE_ID = gepfenceid;
            //var client = new RestClient("http://52.230.24.18:8082/api/geofences");
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("postman-token", "d6458d54-9942-1bf8-da65-488fae419e20");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("authorization", "Basic YWRtaW46YWRtaW4=");
            //request.AddHeader("content-type", "application/json");
            //string jSONSTRING = "{\"CommandKey\": \"" + CommandKey + "\",\"Imei\":" + Unit + ",\"GPSfixed\":\"" + GPSfixed + "\",\"DATE\":\"" + DATE + "\", \"Time\":\"" + Time + "\",\"Latitude\":\"" + Latitude + "\",\"Longitude\":\"" + Longitude + "\",\"Speed\":" + Speed + ",\"Odometer\":" + Odometer + ",\"Direction\":" + Direction + ",\"SatVisible\":" + SatVisible + ",\"BoxOpen\":" + BoxOpen + ",\"GSMStrength\":" + GSMStrength + ",\"Powercut\":" + PowerCut + ",\"Ignition\":" + Ignition + ",\"Altitude\":" + Altitude + ",\"DeviceCompanyId\":" + DeviceCompanyId + ",\"DeviceId\":\"" + Unit + "\"}";
            //request.AddParameter("application/json", "{ \"area\": \"'CIRCLE('17.473983'   '78.571228' ,  '1')'\", \"calendarId\": 0, \"description\": \"Ok\", \"name\": \"ECIL X Road\" }", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);




            if (count == 0)
            {
                _obj_Smvts_LandMarks.OPERATION = operation.Insert;
                if (BLL.set_LandMarks(_obj_Smvts_LandMarks))
                    status = "true";
                else
                    status = "false";
            }
            else
            {
                status = "Latitude and Longitude already Exist";
            }

            response.Content = new StringContent(serializer.Serialize(status), Encoding.UTF8, "application/json");
            return response;
        }



        //Geofence configuration
        [HttpGet]
        [Route("api/AssignedGeofence/{USERS_ID}")]
        public HttpResponseMessage AssignedGeofence(int USERS_ID)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {

                int clientID;
                //Load  Vehicles
                List<SMVTS_VEHICLES> obj = new List<SMVTS_VEHICLES>();
                SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
                DataTable DT = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
                clientID = Convert.ToInt32(DT.Rows[0][0]);

                _obj_Smvts_Vehicles.OPERATION = operation.Empty;
                _obj_Smvts_Vehicles.CREATEDBY = 0;
                _obj_Smvts_Vehicles.VEHICLES_CATEGORY_ID = Convert.ToInt32(clientID);
                DataTable dt = BLL.get_Vehicles1(_obj_Smvts_Vehicles);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }
        [HttpGet]
        [Route("api/GeofenceLandmarks/{USERS_ID}")]
        public HttpResponseMessage GeofenceLandmarks(int USERS_ID)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                var USERSID ="";
                USERSID = USERS_ID.ToString();
                DataTable dt = BLL.getGeofenceLandmarks(USERSID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/getAllGeofenceData/{USERS_ID}")]
        public HttpResponseMessage getAllGeofenceData(int USERS_ID)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.GetAllGeofences1(USERS_ID);
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
                                DataTable dt_vehicle = BLL.getVehicleRegNumber1(allvehicles[j]);
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

                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    row = new Dictionary<string, object>();
                        //    foreach (DataColumn col in dt.Columns)
                        //    {
                        //        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                        //    }
                        //    rows.Add(row);
                        //}

                    }
                    response.Content = new StringContent(serializer.Serialize(obj_all), Encoding.UTF8, "application/json");
                }
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/getEvents")]
        public HttpResponseMessage getEvents()
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.get_events1();

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpGet]
        [Route("api/getEventConfiguration/{USERS_ID}")]
        public HttpResponseMessage getEventConfiguration(int USERS_ID)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                int clientID;
                DataTable DT = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
                clientID = Convert.ToInt32(DT.Rows[0][0]);
                DataTable dt = BLL.getEventConfiguration1(clientID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpPost]
        [Route("api/Saveconfig/{USERS_ID}/{chkedvehicles}/{mobileNumbers}/{LandMark}")]
        public HttpResponseMessage Saveconfig(int USERS_ID, string chkedvehicles, string mobileNumbers, string LandMark)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                int clientID;
                string result = "";
                SMVTS_ASSIGNGEOFENCE_LANDMARKS obj = new SMVTS_ASSIGNGEOFENCE_LANDMARKS();
                DataTable DT = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
                clientID = Convert.ToInt32(DT.Rows[0][0]);
                obj.ASSIGNGEOFENCE_CATEGID = clientID;
                obj.USERS_ID = USERS_ID;
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
               
                response.Content = new StringContent(serializer.Serialize(result), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        [HttpPost]
        [Route("api/SaveAlertConfiguration/{USERS_ID}/{EventId}/{AppAlert}/{SmsAlert}/{EmailAlert}")]
        public HttpResponseMessage SaveAlertConfiguration(string USERS_ID, int EventId = 0, bool AppAlert = false, bool SmsAlert = false, bool EmailAlert = false)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                int clientID;
                var message = "";
                DataTable DT = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
                clientID = Convert.ToInt32(DT.Rows[0][0]);
                ctpl_events_configuration obj = new ctpl_events_configuration();
                obj.CONFIG_APPALERT = AppAlert;
                obj.CONFIG_EMAILALERT = EmailAlert;
                obj.CONFIG_EVENTID = EventId;
                obj.CONFIG_SMSALERT = SmsAlert;
                obj.CONFIG_CATEG_ID = clientID;
                DataTable dt = BLL.getEventById1(EventId, clientID);
                if (dt.Rows.Count > 0)
                {
                    message = "Event Already Exists";
                }
                else
                {
                    bool status = BLL.insert_events1(obj);
                    if(status == true) {
                        message = "true";
                    }
                    else { 
                        message = "false"; }
                }
                response.Content = new StringContent(serializer.Serialize(message), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;

        }
        [HttpGet]
        [Route("api/LoadVehicles/{USERS_ID}")]
        public HttpResponseMessage LoadVehicles(int USERS_ID)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                string clientID;
                DataTable DT = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
                clientID = DT.Rows[0][0].ToString();
                DataTable dt = BLL.getVehicles1(clientID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }


        [HttpGet]
        [Route("api/getConfigDetails/{USERS_ID}/{config_id}/{event_Id}")]
        public HttpResponseMessage getConfigDetails(int USERS_ID, int config_id, int event_Id)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                int clientID;
                DataTable DT = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
                clientID = Convert.ToInt32(DT.Rows[0][0]);
                DataTable dt = BLL.getConfigDetails(config_id, clientID);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
                if (event_Id == 4)
                {
                    DataTable dt_geofence = BLL.getGeofenceByCateg(clientID);

                    if (dt_geofence.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                            }
                            rows.Add(row);
                        }
                    }
                    response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
                }
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }



        [HttpGet]
        [Route("api/getGeofecneData/{DetailsId}")]
        public HttpResponseMessage getGeofecneData(int DetailsId)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.getGeofenceDetails1(DetailsId);

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }


        [HttpPost]
        [Route("api/AssignAlertVehicles/{USERS_ID}/{configId}/{VehicleIds}/{MobileNumber}/{ToEmail}/{CCMail}/{BccEmail}/{LandmarkId}")]
        public HttpResponseMessage AssignAlertVehicles(int USERS_ID,int configId, string VehicleIds, string MobileNumber, string ToEmail, string CCMail, string BccEmail, int LandmarkId)
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                string vehcles_ids = "", device_ids = ""; bool status = false;
                string[] arrayVehicles = JsonConvert.DeserializeObject<string[]>(VehicleIds);
                for (int i = 0; i < arrayVehicles.Length; i++)
                {
                    if (vehcles_ids == "")
                    {
                        vehcles_ids = arrayVehicles[i];
                    }
                    else
                    {
                        vehcles_ids += "," + arrayVehicles[i];
                    }
                }

                ctpl_eventconfig_details obj = new ctpl_eventconfig_details();
                int clientID;
                DataTable DT = BLL.ExecuteQuery_an("select USERS_CATEGORY_ID from smvts_users where users_id='" + USERS_ID + "'");
                clientID = Convert.ToInt32(DT.Rows[0][0]);
                obj.DETAILS_BCCMAIL = BccEmail;
                obj.DETAILS_CATEGID = clientID;
                obj.DETAILS_VEHICLEID = vehcles_ids;
                obj.DETAILS_CCMAIL = CCMail;
                obj.DETAILS_CONFIGID = configId;
                obj.DETAILS_MOBILENO = MobileNumber;
                obj.DETAILS_TOMAIL = ToEmail;
                obj.DETAILS_LANDMARKID = LandmarkId;
                DataTable dt_config = BLL.getConfigDetailsapp(configId, clientID);
                if (dt_config.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt_config.Rows[0]["CONFIG_EVENTID"]) == 4)
                    {
                        DataTable dt_geofence = BLL.get_geofence_landmarkapp(clientID, configId, LandmarkId);
                        if (dt_geofence.Rows.Count > 0)
                        {
                            status = BLL.update_geofence_detialsapp(obj);
                        }
                        else
                        {
                            status = BLL.insert_config_detialsapp(obj);
                        }
                    }
                    else
                    {
                        status = BLL.update_config_detials(obj);
                    }

                }
                else
                {

                    status = BLL.insert_config_detialsapp(obj);

                }
                response.Content = new StringContent(serializer.Serialize(status), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }
        [HttpGet]
        [Route("api/Get_Renew_Dashboard")]
        public HttpResponseMessage Get_Renew_Dashboard()
        {

            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                DataTable dt = BLL.GetRenewDashboard();

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.ToString().ToUpper(), dr[col].ToString().ToUpper());
                    }
                    rows.Add(row);
                }
                response.Content = new StringContent(serializer.Serialize(rows), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

    }


}
