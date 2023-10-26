using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Web.Script.Serialization;
using System.Threading;

namespace PRAGATIVTSMVC
{
    public partial class PushData : System.Web.UI.Page
    {
        public class OutPut
        {
            public string data { get; set; }
            public List<list> data1 { get; set; }
        }
        public class list
        {
            public string Id { get; set; }
        }
        WebClient client = new WebClient();
        protected void Page_Load(object sender, EventArgs e)



        {
           // if (!IsPostBack)

            //{
                //while (true)
                //{ 
                string output = "";
                lbl_message.Text = "";
                try
                {
                    mahamining.Service obj = new mahamining.Service();
                    //get all vehicles
                    DataTable dt_vehicles = new DataTable();
                    dt_vehicles = BLL.get_All_MiningVehicles();
                    if (dt_vehicles.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt_vehicles.Rows.Count; i++)
                        {
                            DataTable dt = new DataTable();
                            dt = BLL.get_Tripdata_for_Mining(Convert.ToInt32(dt_vehicles.Rows[i]["DEVICE_ID"]));
                            if (dt.Rows.Count > 0)
                            {
                                string Unit = (dt.Rows[0]["DEVICE_SERIALNUMBER"]).ToString();
                                string Reason = "A";
                                string CommandKey = "0";
                                string Commandkeyvalue = "0";
                                int Ignition = Convert.ToInt32(dt.Rows[0]["TRIPDATA_IGNITIONSTATUS"]);
                                int PowerCut = 1;
                                int BoxOpen = 0;
                                string MSGKey = "0";
                                long Odometer = Convert.ToInt32(dt.Rows[0]["TRIPDATA_C_ODOMETER"]);
                                int Speed = Convert.ToInt32(dt.Rows[0]["TRIPDATA_SPEED"]);
                                int SatVisible = Convert.ToInt32(dt.Rows[0]["TRIPDATA_GPS_CONNECTED"]);
                                string GPSfixed = "";
                                if (Convert.ToInt32(dt.Rows[0]["TRIPDATA_GPS_CONNECTED"]) == 1)
                                {
                                    GPSfixed = "A";
                                }
                                else
                                {
                                    GPSfixed = "V";
                                }
                                string Latitude = dt.Rows[0]["TRIPDATA_LATITUDE"].ToString();
                                string Longitude = dt.Rows[0]["TRIPDATA_LONGITUDE"].ToString();
                                int Altitude = Convert.ToInt32(dt.Rows[0]["TRIPDATA_ALTITUDE"]);
                                int Direction = Convert.ToInt32(dt.Rows[0]["TRIPDATA_DEGREEINDIRECTION"]);
                                DateTime date = Convert.ToDateTime(dt.Rows[0]["TRIPDATA_TRIPDATE"]);
                                string Time = date.ToLongTimeString();
                                string DATE = date.ToString("dd-MM-yyyy");
                                int GSMStrength = 0;
                                int DeviceCompanyId = 15;

                                //obj.InsertGPRSdata(Unit, Reason, CommandKey, Commandkeyvalue, Ignition, PowerCut, BoxOpen, MSGKey, Odometer, Speed, SatVisible, GPSfixed, Latitude, Longitude, Altitude, Direction, Time, DATE, GSMStrength, DeviceCompanyId);

                                string url = "http://gps.mahamining.com/Service.asmx/InsertGPRSdata?Unit=" + Unit + "&Reason=" + Reason + "&CommandKey=" + CommandKey + "&Commandkeyvalue=" + Commandkeyvalue + "&Ignition=" + Ignition + "&PowerCut=" + PowerCut + "&BoxOpen=" + BoxOpen + "&MSGKey=" + MSGKey + "&Odometer=" + Odometer + "&Speed=" + Speed + "&SatVisible=" + SatVisible + "&GPSfixed=" + GPSfixed + "&Latitude=" + Latitude + "&Longitude=" + Longitude + "&Altitude=" + Altitude + "&Direction=" + Direction + "&Time=" + Time + "&DATE=" + DATE + "&GSMStrength=" + GSMStrength + "&DeviceCompanyId=" + DeviceCompanyId + "";
                                //using (WebClient client = new WebClient())
                                //{
                                    string json = client.DownloadString(url);

                                    OutPut Info = (new JavaScriptSerializer()).Deserialize<OutPut>(json);
                                    string value1 = (Info.data); string value2 = "";
                                    var data2 = Info.data1.ToList();
                                    foreach (var d in data2)
                                    {
                                        value2 = (d.Id).ToString();
                                    }
                                    bool status = false;

                                    status = BLL.insert_mining_log(Convert.ToInt32(dt_vehicles.Rows[i]["DEVICE_ID"]), value1, value2);
                                    output = output+ "<br/>" + dt_vehicles.Rows[i]["DEVICE_ID"].ToString() + " last Synced: " + DateTime.Now.ToString();
                                //}
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                output = output + ex.ToString();

                }
                    //  Response.Write("last Synced : " + DateTime.Now.ToString());
                    lbl_message.Text = output;
                    //Thread.Sleep(60000*5);

            // }
           // }
        }
    }
}