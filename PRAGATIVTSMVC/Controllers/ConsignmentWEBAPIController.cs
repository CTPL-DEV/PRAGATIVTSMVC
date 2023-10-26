using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;

namespace PRAGATIVTSMVC.Controllers
{
    public class ConsignmentWEBAPIController : ApiController
    {
        [AcceptVerbs("GET","POST")]
        public bool frm_omkartripinformation(string categid, string vehicleno, string despatchdate, 
            string from, string to, string partyname, string expecteddeliverydate, string drivername, 
            string driverphone, string reporting_next = null, string EwayExpiry = null,
            string invoiceno = null, string dealercode = null, string unloadingdate = null)
        {

            bool b=false;
            ///////////////
            if (categid != null && vehicleno != null && despatchdate != null && from != null && to != null && partyname != null && expecteddeliverydate != null && drivername != null && driverphone != null)
            {
                bool status;
                status = true;
               // string categid = Request.QueryString["categid"].ToString();
                string query = "SELECT DISTINCT CATEG_DBNAME AS dbname,CATEG_NAME FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_ID='" + categid + "'";
                //string query = "SELECT DISTINCT CATEG_NAME FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_ID='" + categid + "'";
                //DataTable dt_categ = Dal.ExecuteQueryDB(query);
                string dbname = "", Prod6_Cat_Name = "";
                DataTable dt_categ1 = Dal.ExecuteQueryDB(query);
                if (dt_categ1.Rows.Count > 0)
                {
                    dbname = dt_categ1.Rows[0]["dbname"].ToString();
                    Prod6_Cat_Name = dt_categ1.Rows[0]["CATEG_NAME"].ToString();
                }
                string queryy = "SELECT DISTINCT CATEG_ID   FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + Prod6_Cat_Name + "'";
                DataTable dt_categ = Dal.ExecuteQueryDBg(queryy, dbname);


                
                if (dt_categ.Rows.Count > 0)
                {
                 
                    categid = dt_categ.Rows[0]["CATEG_ID"].ToString();
              
                  //  string vehicleno = Request.QueryString["vehicleno"].ToString();
                  //  string despatchdate = Request.QueryString["despatchdate"].ToString();
                
                  //  string from = Request.QueryString["from"].ToString();
                  //  string to = Request.QueryString["to"].ToString();
                  //  string partyname = Request.QueryString["partyname"].ToString();
                 //   string expecteddeliverydate = Request.QueryString["expecteddeliverydate"].ToString();
                
                 //   string drivername = Request.QueryString["drivername"].ToString();
                 //   string driverphone = Request.QueryString["driverphone"].ToString();
                  
                     reporting_next = "";  EwayExpiry = "";
                    if (reporting_next == null || reporting_next == "null" || reporting_next == string.Empty)
                    {
                        reporting_next = "";
                    }
                    else
                    {
                      //  reporting_next = reporting_next;
                    }
                    if (EwayExpiry == null || EwayExpiry == "null" || EwayExpiry == string.Empty)
                    {
                        EwayExpiry = "";
                    }
                    else
                    {
                       // EwayExpiry = EwayExpiry;
                    }



                    var RETVAL = "";
                    status = true;
                    try
                    {
                        string Reportingdate = "", forman = "";
                        string Startdate = despatchdate, Enddate = expecteddeliverydate, loadingdate = despatchdate, Reacheddate = despatchdate, ConsignmentRemarks = "";
                        unloadingdate = expecteddeliverydate;
                        //DateTime Startdate = despatchdate, Enddate = expecteddeliverydate, loadingdate = despatchdate, unloadingdate = expecteddeliverydate, Reacheddate = despatchdate, ConsignmentRemarks = "";

                        string ER_VEHICLENO = vehicleno;
                        string ER_VEHICLETYPE = "";
                        string ER_BOOKBRANCH = "";
                        string ER_BOOKZONE = "";
                        string ER_DISPATCHDATE = despatchdate;
                        //DateTime  ER_DISPATCHDATE = despatchdate;
                        string ER_FROM = from;
                        string ER_TO = to; //session value from the page.
                        string ER_PARTYNAME = partyname;
                        string ER_DELIVERYBRANCH = "";
                        string ER_TO_ZONE = "";
                        string ER_EXPECTED_DATE = expecteddeliverydate;
                        //DateTime ER_EXPECTED_DATE = expecteddeliverydate;
                        string ER_REPORTING_DATE = Reportingdate;
                        string ER_CURRENT_DATE = "";
                        string ER_CURRENT_STATUS = "";
                        string ER_LOCATION = "";
                        string ER_ACK_STATUS = "";
                        string ER_DRIVER_NAME = drivername;
                        string ER_DRIVER_PHONE = driverphone;
                        string ER_CNTR_BRANCH = "";
                        string ER_FORMAN_DETAILS = forman;
                        string ER_DEST_CONTROL_BRANCH = "";
                        string ER_FORMAN_BRANCH = "";
                        //string ER_CATEGID = "1101";
                        string ER_CATEGID = categid;
                        string ER_UPLOADEDDATE = DateTime.Now.ToString();
                        string start_date = Startdate;
                        string end_date = Enddate;
                        string loading_date = loadingdate;
                        string unloading_date = unloadingdate;
                        string consgRemarks = ConsignmentRemarks;
                        string reachdate = Reacheddate;
                        if (invoiceno != null)
                        {
                            ER_ACK_STATUS = invoiceno;
                        }
                        if (dealercode != null)
                        {
                            ER_BOOKZONE = dealercode;
                        }
                        string er_values = "EXEC SMVTS_API_ER_TRIPINFO @Operation ='Insert', @ER_VEHICLENO='" + vehicleno + "', @ER_VEHICLETYPE='" + ER_VEHICLETYPE;
                        er_values = er_values + "', @ER_BOOKBRANCH='" + ER_BOOKBRANCH + "', @ER_BOOKZONE='" + ER_BOOKZONE + "', @ER_DISPATCHDATE='" + ER_DISPATCHDATE;
                        er_values = er_values + "', @ER_FROM='" + from + "', @ER_TO='" + to + "', @ER_PARTYNAME='" + partyname;
                        er_values = er_values + "', @ER_DELIVERYBRANCH='" + ER_DELIVERYBRANCH + "', @ER_TO_ZONE='" + ER_TO_ZONE + "', @ER_EXPECTED_DATE='" + expecteddeliverydate;
                        er_values = er_values + "', @ER_REPORTING_DATE='" + Reportingdate + "', @ER_CURRENT_DATE='" + ER_CURRENT_DATE + "', @ER_CURRENT_STATUS='" + ER_CURRENT_STATUS + "', @ER_LOCATION='" + ER_LOCATION;
                        er_values = er_values + "', @ER_ACK_STATUS='" + ER_ACK_STATUS + "', @ER_DRIVER_NAME='" + drivername + "', @ER_DRIVER_PHONE='" + driverphone + "',@ER_CNTR_BRANCH='" + ER_CNTR_BRANCH + "',@ER_FORMAN_DETAILS='" + forman + "',@ER_DEST_CONTROL_BRANCH='" + ER_DEST_CONTROL_BRANCH + "',@ER_FORMAN_BRANCH='" + ER_FORMAN_BRANCH + "',@ER_CATEGID='" + categid + "',@ER_UPLOADEDDATE='" + ER_UPLOADEDDATE + "'";
                        status = Dal.ExecuteNonQueryDB1(er_values, dbname);
                        if (status == true)
                        {
                            //insert into 149 server db ..

                            if (Convert.ToInt32(categid) == 1101)
                            {
                                try
                                {

                                    string con1 = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCrKZQ4DmwFwjGhTWCzxCZGLpNVvatXgQFewBTrTyQHLEGM8bIBvXogfXjgi5nzYlmveKC9JvHHrUfYMuhNJkaMQ==");



                                    status = BLL.Insertomkartripinfo(ER_DISPATCHDATE, from, to, partyname, expecteddeliverydate, Startdate, drivername, driverphone, vehicleno, con1);
                                    if (status == true)
                                    {
                                       // BLL.ShowMessage(this, "Omkartrip informaion inserted Successfully.");
                                    }
                                    else
                                    {
                                      //  BLL.ShowMessage(this, "Omkartrip information Not inserted. ");
                                    }

                                }
                                catch (Exception ex)
                                {
                                    throw (ex);
                                }
                            }


                            //149 pushpak
                            else if (Convert.ToInt32(categid) == 84)
                            {
                                try
                                {

                                    string con1 = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCrKZQ4DmwFwjGhTWCzxCZGJ4AN7R3hw0FVHbaexUAfsTwuxjFO6a55KS5SoFb8jrcYU6ffBAShnMcX+xSOG0v2A==");



                                    status = BLL.Insertomkartripinfo(ER_DISPATCHDATE, from, to, partyname, expecteddeliverydate, Startdate, drivername, driverphone, vehicleno, con1);
                                    if (status == true)
                                    {
                                      //  BLL.ShowMessage(this, "Omkartrip informaion inserted Successfully.");
                                    }
                                    else
                                    {
                                       // BLL.ShowMessage(this, "Omkartrip information Not inserted. ");
                                    }

                                }
                                catch (Exception ex)
                                {
                                    throw (ex);
                                }
                            }


                         //categid==3

                            else if (Convert.ToInt32(categid) == 3)
                            {
                                try
                                {

                                    string con2 = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCrKZQ4DmwFwjGhTWCzxCZGGuj0TmkkkaLewBTrTyQHLEGM8bIBvXogfXjgi5nzYlmveKC9JvHHrUfYMuhNJkaMQ==");



                                    status = BLL.Insertomkartripinfo_new(ER_DISPATCHDATE, from, to, partyname, expecteddeliverydate, drivername, driverphone, vehicleno, reporting_next, EwayExpiry, con2);
                                    if (status == true)
                                    {
                                      //  BLL.ShowMessage(this, "Omkartrip informaion inserted Successfully.");
                                    }
                                    else
                                    {
                                       // BLL.ShowMessage(this, "Omkartrip information Not inserted. ");
                                    }

                                }
                                catch (Exception ex)
                                {
                                    throw (ex);
                                }
                            }
                            else
                            {
                                try
                                {
                                    //omkar_smvts

                                    string con1 = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCrKZQ4DmwFwjGhTWCzxCZGGuj0TmkkkaLewBTrTyQHLEGM8bIBvXogfXjgi5nzYlmveKC9JvHHrUfYMuhNJkaMQ==");


                                    status = BLL.Insertomkartripinfo(ER_DISPATCHDATE, from, to, partyname, expecteddeliverydate, Startdate, drivername, driverphone, vehicleno, con1);
                                    if (status == true)
                                    {
                                      //  BLL.ShowMessage(this, "Omkartrip informaion inserted Successfully.");
                                    }
                                    else
                                    {
                                      //  BLL.ShowMessage(this, "Omkartrip information Not inserted. ");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw (ex);
                                }



                                int createdby = 54;
                                int modifiedby =54;
                                string operation = "Update";
                                string query1 = "EXEC USP_SMVTS_OMKAR_DRIVER_UPDATE @OPERATION='" + operation + "',@DRIVER_MOBILE='" + driverphone + "',@DRIVER_NAME='" + drivername + "',@VEHICLE_NO='" + vehicleno + "',@CREATED_BY='" + createdby + "',@MODIFIED_BY='" + modifiedby + "',@CATEG_ID='" + categid + "'";
                                bool status1 = Dal.ExecuteNonQueryDB1(query1, dbname);
                                if (status1 == true)
                                {
                                   // BLL.ShowMessage(this, "Driver Updated Successfully");
                                }
                                else
                                {
                                  //  BLL.ShowMessage(this, "Driver Details Not Upated");
                                }
                            }
                        }
                        else
                        {

                        }


                        JavaScriptSerializer JS = new JavaScriptSerializer();

                        JS.MaxJsonLength = Int32.MaxValue;
                        RETVAL = JS.Serialize(status);
                      

                       //  Response.Buffer = true;
                        // Response.Clear();

                      //  Response.ContentType = "application/json";

                        //Response.AddHeader("content-length", strResponse.Length.ToString()
                        //Response.Flush();
                     //   Response.Write(RETVAL);
                        //    CreateRoute1(from, to, categid, vehicleno, Startdate, Enddate, driverphone, forman, loadingdate, Reportingdate, unloadingdate, consgRemarks, reachdate, expecteddeliverydate, dbname);


                        //insert into 149 server db ..




                    }


                    catch
                    {
                        status = false;
                    }
                    status = true;
                }
                status = false;
            }
            if (vehicleno != null && despatchdate != null && from != null && partyname != null && unloadingdate != null)
            {
                bool status1;
                status1 = true;
                try
                {

                   // string vehicleno = Request.QueryString["vehicleno"].ToString();
                  //  string despatchdate = Request.QueryString["despatchdate"].ToString();
                  //  string from = Request.QueryString["from"].ToString();
                  //  string partyname = Request.QueryString["partyname"].ToString();
                 //   string unloadingdate = Request.QueryString["unloadingdate"].ToString();
                  
                    string con1 = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCrKZQ4DmwFwjGhTWCzxCZGJ4AN7R3hw0FVHbaexUAfsTwuxjFO6a55KS5SoFb8jrcYU6ffBAShnMcX+xSOG0v2A==");

                    status1 = BLL.closedtripinfo(vehicleno, from, partyname, despatchdate, unloadingdate, con1);


                    var RETVAL = "";

                    JavaScriptSerializer JS = new JavaScriptSerializer();

                    JS.MaxJsonLength = Int32.MaxValue;
                    RETVAL = JS.Serialize(status1);
                    //RETVAL.replace(/"(\w+)"\s*:/g, '$1:');
                    //  JSON.stringify(RETVAL);
                  //  Response.Buffer = true;
                 //   Response.Clear();

                 //   Response.ContentType = "application/json";

                    //Response.AddHeader("content-length", strResponse.Length.ToString()
                    //Response.Flush();
                 //   Response.Write(RETVAL);
                }
                catch
                {
                    status1 = false;
                    b = false;
                }
                status1 = true;
                b = true;
            }
            // status = true;

            ///////////////
            return b;
        }

         [AcceptVerbs("GET", "POST")]
        public string pushdata(string vehicleno, DateTime date, string clientname, string NoOfCns, int triplogid, string origin, string destination)
        {

            SMVTS_ROUTEPLAN _obj = new SMVTS_ROUTEPLAN();
            ////getting dbname
            //string query = "select CATEG_DBNAME from SMVTS_VEHICLES inner join SMVTS_CATEGORIES on  CATEG_ID=VEHICLES_CATEGORY_ID and VEHICLES_REGNUMBER='" + vehicleno + "'";
            //DataTable dt_categ = Dal.ExecuteQueryDB(query);
            //string dbname = dt_categ.Rows[0]["CATEG_DBNAME"].ToString();
            string query = "Exec USP_SMVTS_DBVEHICLES @Vehicleno='" + vehicleno + "'";
            DataTable dt_categ = Dal.ExecuteQueryDB(query);
            if (dt_categ != null)
            {
                if (dt_categ.Rows.Count > 0)
                {

                    string categid3 = dt_categ.Rows[0]["CATEGID"].ToString();
                    string dbname = dt_categ.Rows[0]["DBNAME"].ToString();
                    if (BLL.get_ServiceBasedLoginAPI(Convert.ToInt32(categid3), 33, System.DateTime.Now, dbname) == true || BLL.CHECKMRL(886, dbname, vehicleno) == true)
                    {


                        string devidiQ = "SELECT VEHICLES_DEVICE_ID,VEHICLES_CATEGORY_ID FROM SMVTS_VEHICLES(NOLOCK) WHERE VEHICLES_REGNUMBER='" + vehicleno.Replace(" ", "") + "'";
                        DataTable DEVID = Dal.ExecuteQueryDB1(devidiQ, dbname);
                        string Deviceid = DEVID.Rows[0][0].ToString();
                        string CategId = DEVID.Rows[0][1].ToString();

                        string DistanceQuery1 = "SELECT ROUTES_ID,ROUTES_DISTANCE FROM SMVTS_ROUTES(NOLOCK) WHERE ROUTES_FROM='" + origin + "' AND ROUTES_TO='" + destination + "'AND ROUTES_CATEGORY_ID='" + CategId + "'";
                        DataTable dt_dist1 = Dal.ExecuteQueryDB1(DistanceQuery1, dbname);
                        string Route_id = "0";
                        float dist = 0;
                        if (dt_dist1.Rows.Count < 1)
                        {
                            Route_id = CreateRoute(origin, destination, CategId, dbname);
                            string DistanceQuery2 = "SELECT ROUTES_ID,ROUTES_DISTANCE FROM SMVTS_ROUTES(NOLOCK) WHERE ROUTES_ID=" + Route_id;
                            DataTable dt_dist2 = Dal.ExecuteQueryDB1(DistanceQuery2, dbname);
                            dist = (float)Convert.ToDecimal(dt_dist2.Rows[0][0].ToString());
                        }
                        else
                        {
                            Route_id = dt_dist1.Rows[0][0].ToString();
                            dist = (float)Convert.ToDecimal(dt_dist1.Rows[0][1].ToString());
                        }

                        int TIME = Convert.ToInt32(dist / 300);

                        if (TIME > 1)
                        {

                            _obj.VEHROUTE_ENDDATE = date.AddDays(TIME);
                        }
                        else
                        {
                            _obj.VEHROUTE_ENDDATE = date.AddDays(1);
                        }
                        _obj.VEHROUTE_CATEGORY_ID = Convert.ToInt32(CategId);
                        _obj.VEHROUTE_ROUTE_ID = Convert.ToInt32(Route_id);
                        _obj.VEHROUTE_AVGSPEED = 40;
                        _obj.VEHROUTE_TRAVELDISTINDAY = 300;
                        _obj.VEHROUTE_DEVICE_ID = Convert.ToInt32(Deviceid);
                        _obj.VEHROUTE_DISTANCEROUTE = dist;
                        _obj.VEHROUTE_TRIPNO = triplogid.ToString();
                        _obj.CONSIGNMENT_DETAILS = NoOfCns;
                        _obj.VEHROUTE_STARTDATE = date;
                        _obj.CREATEDDATE = DateTime.Now;
                        _obj.LASTMDFDATE = DateTime.Now;
                        _obj.VEHROUTE_STATUS = 1;
                        _obj.OPERATION = operation.Insert;


                        if (BLL.set_RoutePlan_clientDB(_obj, dbname))
                        {
                            return "true";
                        }
                        else
                        {
                            return "false";
                        }

                    }
                }
            }
            return "false";

        }


        public string CreateRoute(string StartPoint, string Endpnt, string categid, string dbname)
        {

            string ROUTES_NAME = StartPoint.Trim() + "_" + Endpnt.Trim();
            string ROUTES_STATUS = "True";
            string ROUTES_CREATEDDATE = DateTime.Now.ToShortDateString().Replace("-", "/");
            string ROUTES_MODIFIEDDATE = DateTime.Now.ToShortDateString().Replace("-", "/");
            string ROUTES_FROM = StartPoint;
            string ROUTES_TO = Endpnt;
            string ROUTES_VIA = "";
            string ROUTES_CATEGORY_ID = categid; //session value from the page.
            string ROUTES_CREATEDBY = "332";
            string ROUTES_MODIFIEDBY = "332";
            string ROUTES_ST_LAT = "", ROUTES_ST_LNG = "", ROUTES_END_LAT = "", ROUTES_END_LNG = "", ROUTES_DISTANCE = "0.00";
            string ROUTES_POINTS = "", ROUTES_INTRMED_POINTSDATA = "";
            double TotDist = 0, TotDistCnt = 10;

            string ROUTE_ID;

            List<DirectionSteps> _obj = GMapUtil.GetDirections(StartPoint, Endpnt, "");
            //totalPoints = totalPoints + "(" + directionStep.StLat + "," + directionStep.StLong+"),";
            if (_obj.Count > 0)
            {
                ROUTES_ST_LAT = _obj[0].Steps[0].StLat;
                ROUTES_ST_LNG = _obj[0].Steps[0].StLong;
                ROUTES_END_LAT = _obj[_obj.Count - 1].Steps[_obj[_obj.Count - 1].Steps.Count - 1].EndLat;
                ROUTES_END_LNG = _obj[_obj.Count - 1].Steps[_obj[_obj.Count - 1].Steps.Count - 1].EndLong;

                for (int i = 0; i < _obj.Count; i++)
                {
                    ROUTES_DISTANCE = System.Convert.ToString(System.Convert.ToDouble(ROUTES_DISTANCE) + System.Convert.ToDouble(_obj[i].TotalDistance.Replace("km", "").Trim()));
                    string s = _obj[i].Steps.Count.ToString();
                    for (int j = 0; j < _obj[i].Steps.Count; j++)
                    {
                        ROUTES_POINTS = ROUTES_POINTS + "(" + _obj[i].Steps[j].StLat + "," + _obj[i].Steps[j].StLong + "),";
                        if (_obj[i].Steps[j].Distance.ToUpper().Contains("KM"))
                            TotDist = TotDist + System.Convert.ToDouble(_obj[i].Steps[j].Distance.ToUpper().Replace("KM", "").Trim());
                        else if (_obj[i].Steps[j].Distance.ToUpper().Contains("M"))
                            TotDist = TotDist + System.Convert.ToDouble(_obj[i].Steps[j].Distance.ToUpper().Replace("M", "").Trim()) / 1000;


                        if (TotDistCnt <= TotDist)
                        {
                            ROUTES_INTRMED_POINTSDATA = ROUTES_INTRMED_POINTSDATA + _obj[i].Steps[j].StLat + "," + _obj[i].Steps[j].StLong + "," + (TotDist * 1000).ToString() + ";";
                            TotDistCnt = TotDist + 10;
                        }
                    }
                }
            }

            string StrQuery = "EXEC USP_SMVTS_ROUTES @Operation ='Insert_Rec', @ROUTES_CATEGORY_ID='" + ROUTES_CATEGORY_ID + "', @ROUTES_NAME='" + ROUTES_NAME;
            StrQuery = StrQuery + "', @ROUTES_STARTLAT='" + ROUTES_ST_LAT + "', @ROUTES_STARTLONG='" + ROUTES_ST_LNG + "', @ROUTES_ENDLAT='" + ROUTES_END_LAT;
            StrQuery = StrQuery + "', @ROUTES_ENDLONG='" + ROUTES_END_LNG + "', @ROUTES_POINTS='" + ROUTES_POINTS.Substring(0, ROUTES_POINTS.Length - 1) + "', @ROUTES_STATUS='" + ROUTES_STATUS;
            StrQuery = StrQuery + "', @ROUTES_CREATEDBY='" + ROUTES_CREATEDBY + "', @ROUTES_CREATEDDATE='" + ROUTES_CREATEDDATE + "', @ROUTES_MODIFIEDBY='" + ROUTES_MODIFIEDBY;
            StrQuery = StrQuery + "', @ROUTES_MODIFIEDDATE='" + ROUTES_MODIFIEDDATE + "', @ROUTES_FROM='" + ROUTES_FROM + "', @ROUTES_TO='" + ROUTES_TO + "', @ROUTES_VIA='" + ROUTES_VIA;
            StrQuery = StrQuery + "', @ROUTES_DEVIATION_LIMIT='4', @ROUTES_DISTANCE='" + ROUTES_DISTANCE + "', @ROUTES_INTRMED_POINTSDATA='" + ROUTES_INTRMED_POINTSDATA.Substring(0, ROUTES_INTRMED_POINTSDATA.Length - 1) + "'";
            // DataTable dt_route = BLL.ExecuteQuery(StrQuery);
            DataTable dt_route = Dal.ExecuteQueryDB1(StrQuery, dbname);
            ROUTE_ID = dt_route.Rows[0][0].ToString();

            return ROUTE_ID;
        }

        public class DirectionSteps
        {

            public string TotalDuration { get; set; }

            public string TotalDistance { get; set; }

            public string OriginAddress { get; set; }

            public string DestinationAddress { get; set; }

            public List<DirectionStep> Steps { get; set; }

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
    }

}
