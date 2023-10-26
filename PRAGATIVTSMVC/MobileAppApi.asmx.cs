using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Xml;

namespace PRAGATIVTSMVC
{
    /// <summary>
    /// Summary description for MobileAppApi
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class MobileAppApi : System.Web.Services.WebService
    {
        int id;

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ClientDashboard(string Org, string Username, string Password)
        {
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
            // return serializer.Serialize(rows);
            this.Context.Response.Write(serializer.Serialize(rows));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DriverNumber(string drivernumber)
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
            return serializer.Serialize(rows);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Driverstatus(string drivernumber)
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
            return serializer.Serialize(rows);
        }
        private static bool sendRequest_new(string msg, string mno, string type)
        {
            string from_mobileno = "PRAGAT";
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



        [WebMethod]
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


        [WebMethod]
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DriverNumber_CurrentLocation(string drivernumber)
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
            return serializer.Serialize(rows);
        }

        [WebMethod]
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
        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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




        [WebMethod]
        public string SAVEIMAGESTARTTRIP(byte[] f, string mobileno)
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


            return status;
        }


        //[WebMethod]
        //public bool SAVEIMAGEDISPATCH(byte[] f, string fileName, string url, string mobileno)
        //{
        //    bool status = false;
        //    try
        //    {

        //        MemoryStream ms = new MemoryStream(f);
        //        //FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
        //        FileStream fs = new FileStream(Server.MapPath("~/MOBILEAPP_IMGS/") + fileName, FileMode.Create);
        //        SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUc7ZFy6tCuvB4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0="));
        //        con.Open();
        //        string query = "Exec usp_SMVTS_MOBILEAPP_ERTRIP @OPERATION='INSERT',@TRIP_BTNTYPE='DISPATCH',@TRIP_URL='" + url + "',@ERTRIP_MOBILENO='8453777220'";
        //        SqlCommand comm = new SqlCommand(query, con);

        //        int r = comm.ExecuteNonQuery();
        //        if (r > 0)
        //        {
        //            ms.WriteTo(fs);
        //            // clean up 
        //            ms.Close();
        //            fs.Close();
        //            fs.Dispose();
        //            // return OK if we made it this far 
        //            status = true;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // return the error message if the operation fails 
        //        throw (ex);
        //    }
        //    return status;
        //}

        [WebMethod]
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
        [WebMethod]
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

        [WebMethod]
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


        [WebMethod]
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

        [WebMethod]
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




        [WebMethod]
        public bool Omkar_UserReg(string mnobno)
        {
            bool status = false;
            Random ran = new Random();
            string sa = ran.Next(10000, 99999).ToString();
            bool r = BLL.insert_mob_info_ws("UnKnown", mnobno, sa);

            if (r == true)
            {

                string type = "OTP";
                //send varification code(sa) to mobile
                string msg = "Dear User, The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app https://play.google.com/store/apps/details?id=org.pragatipadh.omkar";
                //string msg = "Dear " + clname + ", The OTP to complete PRAGATIVTS activation is " + sa + ".Click Here to download the app http://localhost:1503//mobiledowloadapk.aspx";
                // f = sendRequest(masg, mno, type1);
                status = sendRequest1(msg, mnobno, type);


            }
            return status;
        }

        [WebMethod]
        public bool Omkar_UpdateUserInfo(string Name, string mobileno)
        {
            bool status = false;
            Random ran = new Random();
            string sa = ran.Next(10000, 99999).ToString();
            bool r = BLL.insert_mob_info_ws("UnKnown", mobileno, sa);


            return r;
        }


        private static bool sendRequest1(string msg, string mno, string type)
        {

            string from_mobileno = "VTSSMS";
            //string udh = "";
            string message_text = msg;
            //valuefirst
            string URL = "http://api.myvaluefirst.com/psms/servlet/psms.Eservice2?data=<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE MESSAGE SYSTEM \"http://127.0.0.1:80/psms/dtd/messagev12.dtd\"><MESSAGE VER=\"1.2\"><USER USERNAME=\"dhanushinfotrns\" PASSWORD=\"dhanush617\"/>";

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
            //if (result_value.ToUpper().Trim() == "SENT.")
            // {


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

                        SqlConnection con1 = new SqlConnection("Data Source=192.168.50.141;Initial Catalog=VTS_Config;User ID=smvts;Password=ZCBAVTSDHANUSH2010INDIA");
                        con1.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO SMVTS_SMSSERVICEPROVIDERS_RESPONSE VALUES('" + result_value + "',GETDATE(),'VALUEFIRST','0')", con1);
                        int s = cmd.ExecuteNonQuery();

                        // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, response_query);
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + mno + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')", con1);
                        int s1 = cmd1.ExecuteNonQuery();


                        // string insrt_query = "INSERT INTO SMVTS_VALUEFIRSTRESPONSE VALUES('25','" + GUID + "','" + to_mobileno + "','" + msg + "','0','" + SUBMITDATE + "',null,'" + type + "')";
                        // SqlHelper.ExecuteNonQuery(strConnect, CommandType.Text, insrt_query);

                    }
                    //string author = bookElement.GetElementsByTagName("author")[0].InnerText;
                }



            }





            return true;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Omkar_MOBILEAPPVERIFICAIION(string mno, string otp, string imeino)
        {
            //connected to vts 141 server prod2 database.

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            DataTable dt = BLL.install_Activation2(imeino, mno, otp);

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

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Omkar_ClientDashboard(string VehicleNo)
        {

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            string Query = "EXEC GET_DASHBOARD_DETAILS @VEHICLENO='" + VehicleNo + "'";
            string strConn = (ConfigurationManager.ConnectionStrings["connectionP2"].ToString());
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

            return serializer.Serialize(rows);
        }



        [WebMethod]
        public bool Omkar_MOBILEVERIFY(string mnobno, string imeino)
        {
            bool status = false;
            DataTable dt = BLL.getverify2(mnobno, imeino);
            if (dt.Rows.Count > 0)
            {
                status = true;
            }
            return status;
        }


        [WebMethod]
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

        [WebMethod]
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




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Dealer_MOBILEAPPVERIFICAIION(string mno, string otp, string imeino, string CategId)
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



            return serializer.Serialize(rows);

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Dealer_ClientDashboard(string VehicleNo, string CategId)
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

            return serializer.Serialize(rows);
        }



        [WebMethod]
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


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetReports(string Vno, DateTime StartDate, DateTime EndDate, int ReportType, int InterVal_Mnts)
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

            return serializer.Serialize(rows);

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


        [WebMethod(Description = "Get comma Separated Form Ids on the basis of Category Name")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string FormIds(string CategName)
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
            return serializer.Serialize(rows);
        }



        //Venkatesh : 31st MARCH 2017

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Listofvehicles(string mobile_no, string imei, string branchname)
        {
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
                                    return serializer.Serialize(rows);
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
            return serializer.Serialize(rows);
        }

        [WebMethod]
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

        //Venkatesh : 31st MARCH 2017


        // BY VENKATESH 23RD MAY 2017

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Client_Dashboard2(string Org, string Username, string Password)
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
            return serializer.Serialize(_obj);
        }


        // by venkatesh 26th may 2017
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Client_Dashboard_By_VehicleNo(string Org, string Username, string Password, string VehicleNo)
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
            return serializer.Serialize(_obj);
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
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string Dashboard_basedon_vehicleno(string Vehiclenumber)
        //{
        //    List<Client_Dashboard_vehicles> _obj = new List<Client_Dashboard_vehicles>();
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    if (Vehiclenumber != string.Empty)
        //    {

        //        try
        //        {
        //           // string dbname = "Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg==";
        //            SqlConnection con = new SqlConnection(BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjthAI++qzMZOMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rfW1Z+Pe4ImmMiLUmwm8Bng6qht1Tr44G1pWVuP1Ndgg=="));
        //            SqlCommand cmd = new SqlCommand();
        //            con.Open();
        //            //cmd.CommandText = "select VEHICLE NUMBER ,PLACE, DATE, TRIPDATA_LATITUDE, TRIPDATA_LONGITUDE, VEHICLE_COLOR,SPEED,DRIVER_NAME,DRIVER_NUMBER from SMVTS_DASHBOARD(nolock) where VNO='" + Vehiclenumber + "' ";
        //             cmd.CommandText = "select * from SMVTS_DASHBOARD(nolock) where VNO='" + Vehiclenumber + "' ";
        //            DataSet ds = new DataSet();
        //            DataTable dt = new DataTable();
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.SelectCommand.Connection = con;
        //            da.Fill(dt);

        //            // DataTable dt = BLL.GETDRIVERDETAILS(drivernumber);
        //            if (dt != null)
        //            {
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    row = new Dictionary<string, object>();
        //                    _obj.Add(new Client_Dashboard_vehicles
        //                    {
        //                        //SlNO = Convert.ToInt32(dr["SlNO"]),
        //                        VEHICLE_NUMBER = Convert.ToString(dr["VNO"]),
        //                        PLACE = Convert.ToString(dr["PLACE"]),
        //                        DATE = Convert.ToString(dr["TRIPDATE"]),
        //                        TRIPDATA_LATITUDE = Convert.ToString(dr["TRIPDATA_LATITUDE"]),
        //                        TRIPDATA_LONGITUDE = Convert.ToString(dr["TRIPDATA_LONGITUDE"]),
        //                        VEHICLE_COLOR = Convert.ToString(dr["VEHICLE_COLOR"]),
        //                         SPEED = Convert.ToString(dr["SPEED"]),
        //                         DRIVERNAME = Convert.ToString(dr["DRIVERNAME"]),
        //                         DRIVER_NUMBER = Convert.ToString(dr["DRIVER_NUMBER"])
        //                    });
        //                }
        //                }

        //            }
        //        catch (Exception ex)
        //        {
        //            throw (ex);
        //        }

        //    }
        //    return serializer.Serialize(_obj);
        //}
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

    }
}
