using PRAGATIVTS_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PRAGATIVTSMVC.Services
{
    /// <summary>
    /// Summary description for DeviceConfig
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeviceConfig : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod]
        public void SetDevice(string Packet)
        {
            try
            {
                string OrgPacket = Packet;
                // $DeviceName - DL07AU0005$IMEI - 868997035856200$Firmware - 1.0.0$Protocol - 1.0.0$LastValidLocation - 28.359518N076.928078E$,C,CTPL,1.0.0,NR,01,L,868997035856200,DL07AU0005,0,07052019,134502,28.359478,N,076.928099,E,0.0,0.0,0,0.0,0.0,0.0,IDEA P,1,1,12.9,2.68,0,C,14,404,4,0138,0927,4ECD,0138,10,E60C,0138,10,1768,0138,9,1C2B,0138,9,0000,00,1069,69,*
                string[] tempPacket = Packet.Split('$');


                for (int i = 0; i < tempPacket.Length; i++)
                {

                    if (tempPacket[i].StartsWith(",C,"))
                    {

                        //Packet = "$,C,CTPL,1.0.0,NR,01,L,868997035856200,DL07AU0005,1,07052019,111919,28.360134,N,076.927375,E,0.7,64.3,5,225.19,13.4,8.0,BSNL F,1,1,12.7,2.68,0,C,18,404,34,0D5D,87C1,87C0,0D5D,12,87BF,0D5D,0,0,0,0,0,0,0,0000,00,194,19,*";
                        // Packet = "$,HD,MH001ENT,0.0.7,NR,L,862462038774138,MH04AC7088,1,11042019,104716,17.44272,N,78.47954,E,0,180.2,04,521.89,0.0,,IDEA CELLULAR LTD,0,1,5.01,4.27,0,1,31,404,007,270f,98,00bf,723a,-51,00bf,7239,-56,00bf,723b,-66,270f,f1f1,,0000,00,1172,2AD6,*";
                        Packet = tempPacket[i];



                        //string insert_query = "EXEC USP_SMVTS_RAWTRIPDATA @RAWTRIPDATA_DEVICE_ID='',@RAWTRIPDATA_DATE='" + DateTime.Now.AddMinutes(-330).ToString()
                        //                                                  + "',@RAWDATA='" + Packet + "'";
                        //bool result = DAL.ExecuteNonQuery(insert_query);


                        string[] data = Packet.Split(',');
                        string IMEINo = data[7];

                        //string IMEINo = Packet.Substring(1, 15);

                        //  byte[] keepAlivePacket = Encoding.ASCII.GetBytes("@" + IMEINo+ ",OK!");
                        //  io.Write(keepAlivePacket, 0, keepAlivePacket.Length);

                        // An incoming connection needs to be processed.



                        Console.WriteLine("Sending message.. " + DateTime.Now.ToString());
                        //io.Write(message, 0, message.Length);



                        string DeviceIdQuery = "SELECT * FROM SMVTS_TT_DEVID(NOLOCK) WHERE IMEI='" + IMEINo + "'";
                        DataTable dt_devId = Dal.ExecuteQuery(DeviceIdQuery);
                        string deviceId = "0";
                        if (dt_devId.Rows.Count > 0)
                        {
                            deviceId = dt_devId.Rows[0][1] != null ? dt_devId.Rows[0][1].ToString() : "0";
                        }

                        string str = Packet;
                        if (str != null)
                        {

                            string insert_query = "EXEC USP_SMVTS_RAWTRIPDATA @RAWTRIPDATA_DEVICE_ID='" + deviceId + "',@RAWTRIPDATA_DATE='" + DateTime.Now.ToString() + "',@RAWDATA='" + OrgPacket + "'";
                            bool result = Dal.ExecuteNonQuery(insert_query);

                            if (true)
                            {
                                // string str = "@351099811222089,701,FGT,1,20.1,31,0,0,2012/01/23,09:57:46,17.443678,78.473946,542.2,8.073,263.0,6,1.8,0.129!";

                                string device_id = deviceId;
                                string[] packet = str.Split(',');
                                string imei = packet[7];
                                string reason = "";
                                string profile_name = packet[2];
                                string gps_status = packet[9];
                                string battery_voltage = packet[26];
                                string gsm_strength = packet[27];
                                string starter_status = "";
                                string ignition = packet[23] != string.Empty ? packet[23] : "0";
                                string datetime = "";
                                //string date = packet[8];
                                //string time = packet[9];




                                if (packet[10] != string.Empty)
                                {
                                    if (packet[11] != string.Empty)
                                    {
                                        int date = 0, year = 0, Month = 0;
                                        // string[] dates = packet[9].Split('/');
                                        //year = dates[0];
                                        //if (year.Length == 2)
                                        //{
                                        //    year = "20" + dates[0];
                                        //}
                                        //else
                                        //{
                                        //    year = dates[0];
                                        //}
                                        year = Convert.ToInt32(packet[10].Substring(4, 4));
                                        Month = Convert.ToInt32(packet[10].Substring(2, 2));
                                        date = Convert.ToInt32(packet[10].Substring(0, 2));
                                        int hour = Convert.ToInt32(packet[11].Substring(0, 2));
                                        int min = Convert.ToInt32(packet[11].Substring(2, 2));
                                        int sec = Convert.ToInt32(packet[11].Substring(4, 2));

                                        //packet[9] = year + "/" + Month + "/" + date;
                                        packet[9] = Month + "/" + date + "/" + year + " " + hour + ":" + min + ":" + sec;
                                        datetime = Convert.ToDateTime(packet[9]).ToString();

                                        //DateTime dt = new DateTime(Month, date, year, hour, min, sec);
                                        //datetime = dt.ToString();
                                    }
                                }
                                else
                                {
                                    datetime = DateTime.Now.AddHours(-5).AddMinutes(-30).ToString();
                                }


                                string latitude = packet[12] != string.Empty ? packet[12] : "0.000";
                                string longtitude = packet[14] != string.Empty ? packet[14] : "0.000";
                                string altitude = packet[18] != string.Empty ? (Convert.ToDouble(packet[18]) / 1000).ToString() : "0";
                                string speed = packet[16] != string.Empty ? (Convert.ToDouble(packet[16]) * 1.609).ToString() : "0";
                                string direction = packet[13] != string.Empty ? packet[13] : "0";
                                string number_of_satellites = packet[18] != string.Empty ? packet[18] : "0";
                                string gps_accuracy = packet[17] != string.Empty ? packet[17] : "0";
                                //packet[17] = packet[17] != string.Empty ? packet[17] : "0";
                                /*  if (ignition == "0")
                                  {
                                      speed = "0";
                                  }*/

                                // string distance = (Convert.ToDouble(packet[17].Substring(0, packet[17].Length - 1)) * 1.609).ToString();
                                // string geofence_id = packet[18];
                                string distance = 0.ToString();

                                string Query2 = "EXEC [USP_SMVTS_TRIPDATA]    @TRIPDATA_DEVICE_ID=" + device_id + ",@TRIPDATA_LATITUDE='" + latitude + "',@TRIPDATA_LONGITUDE='" + longtitude + "',@TRIPDATA_SPEED=" + speed + ",";
                                Query2 = Query2 + " @TRIPDATA_SOS='NO',     @TRIPDATA_TRIPDATE='" + datetime + "',@RAWDATA='" + str + "',@TRIPDATA_MAINBATTERYCONN=NULL,   @TRIPDATA_IGNITIONSTATUS='" + ignition + "',  @TRIPDATA_IMMOBILIZERSTATUS='" + starter_status + "',";
                                Query2 = Query2 + " @TRIPDATA_DIGITALINPUT1STATUS=NULL,@TRIPDATA_DIGITALINPUT2STATUS=NULL,  @TRIPDATA_DISTANCE='" + distance + "',@TRIPDATA_DEGREEINDIRECTION='0',@TRIPDATA_ADDRESS=NULL,";
                                Query2 = Query2 + " @TRIPDATA_KIND=NULL,@TRIPDATA_RESPONSE=NULL,@TRIPDATA_ALTITUDE='" + altitude + "', @TRIPDATA_GPS_VALID='" + gps_status + "', @TRIPDATA_GPS_CONNECTED='" + gps_accuracy + "',";
                                Query2 = Query2 + " @TRIPDATA_SATELLITES='" + number_of_satellites + "',@TRIPDATA_HEADING=NULL, @TRIPDATA_EMERGENCY=NULL, @TRIPDATA_DOOR=NULL, @TRIPDATA_LOCK=NULL, @TRIPDATA_SIREN=NULL,";
                                Query2 = Query2 + " @TRIPDATA_UNITTYPE='SAPL13', @TRIPDATA_PENDING=NULL, @TRIPDATA_EVENTID=NULL,";
                                Query2 = Query2 + " @TRIPDATA_MAIN_VOLTAGE='" + battery_voltage + "', @TRIPDATA_BACKUP_VOLTAGE='" + battery_voltage + "',";
                                Query2 = Query2 + " @TRIPDATA_ANALOG1=NULL, @TRIPDATA_ANALOG2=NULL, @TRIPDATA_DATETIME_ACTUAL=NULL, @TRIPDATA_NETWORK='" + gsm_strength + "',";
                                Query2 = Query2 + " @TRIPDATA_REASON='" + reason + "'";




                                try
                                {

                                    string sqlConStr = (ConfigurationSettings.AppSettings["Connect"].ToString());
                                    string connectionString = "";
                                    string DbQuery = "EXEC USP_SMVTS_MULTIDBPORTLISTENER @DEVICEID='" + device_id + "',@OPERATION='MULTIDB_DEVICEID'";
                                    DataTable dt_ConnectionString = SqlHelper.ExecuteDataset(sqlConStr, CommandType.Text, DbQuery).Tables[0];

                                    if (dt_ConnectionString != null)
                                    {
                                        if (dt_ConnectionString.Rows.Count > 0)
                                        {
                                            // connectionString = ConfigurationSettings.AppSettings[dt_ConnectionString.Rows[0][0].ToString()].ToString();
                                            connectionString = BLL.Decrypt(dt_ConnectionString.Rows[0][0].ToString());
                                        }
                                        else
                                        {
                                            connectionString = sqlConStr;
                                        }
                                    }
                                    else
                                    {
                                        connectionString = sqlConStr;
                                    }
                                    //  ret_val = SqlHelper.ExecuteNonQuery(sqlConStr, System.Data.CommandType.Text, Query2);
                                    SqlConnection s = new SqlConnection(connectionString);
                                    s.Open();
                                    SqlCommand cmd = new SqlCommand(Query2, s);
                                    cmd.CommandTimeout = 40000000;
                                    cmd.ExecuteNonQuery();
                                    s.Close();

                                    /*  if (DAL.ExecuteNonQuery(Query2))
                                      {
                                          Console.WriteLine("Packet Inserted into RaData 39998::" + DateTime.Now.ToString());
                                      }*/
                                }
                                catch (Exception ex)
                                {

                                }


                                /*  if (DAL.ExecuteNonQuery(Query2))
                                  {
                                      Console.WriteLine("Packet Inserted into RaData 39998::" + DateTime.Now.ToString());
                                  }*/


                            }
                            else
                            {
                                Console.WriteLine("Insertion Failed RaData 39998::" + DateTime.Now.ToString());
                            }
                        }
                    }
                }
            }
            catch
            {

                Console.WriteLine("Exception message.. " + DateTime.Now.ToString() + " " + Packet);

            }
            
        }
    }
}
