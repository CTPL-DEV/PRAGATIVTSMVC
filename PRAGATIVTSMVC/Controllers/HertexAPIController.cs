using PRAGATIVTS_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PRAGATIVTSMVC.Controllers
{
    public class HertexAPIController : ApiController
    {

        [HttpGet]
        [Route("api/FetchLiveData")]
        public HttpResponseMessage Fetch_LiveData(string TOKEN,string imei_list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            Dictionary<string, object> location;
            Dictionary<string, object> parameters;

            Dictionary<string, Dictionary<string, object>> collection;
            List<Dictionary<string, Dictionary<string, object>>> list_collection = new List<Dictionary<string, Dictionary<string, object>>>();
                try
                {
                string list_devices = "";
                string[] List_imei = imei_list.Split(',');
                if(List_imei.Length>0)
                {
                   
                    for(int i=0; i<List_imei.Length; i++)
                    {
                        DataTable dt_devices = new DataTable();
                        dt_devices = Dal.ExecuteQuery_Prod("select DEVICE_ID from smvts_devices where DEVICE_SERIALNUMBER2='" + List_imei[i] + "'");
                        if(dt_devices.Rows.Count>0)
                        {
                            if(list_devices=="")
                            {
                                list_devices = dt_devices.Rows[0][0].ToString();
                            }
                            else
                            {
                                list_devices = list_devices + "," + dt_devices.Rows[0][0].ToString();
                            }
                        }
                    }
                }
                DataTable dt = new DataTable();
                string API_Livedata = "EXEC [SMVTS_HERTEX_API] @TOKEN='" + TOKEN + "',@imei_list='" + list_devices + "'";
                dt = ExecuteQueryLiveData(API_Livedata);

                if (dt.Rows.Count>0)
                {
                    
                    for (int i=0; i<dt.Rows.Count; i++)
                    {
                        collection = new Dictionary<string, Dictionary<string, object>>();
                        row = new Dictionary<string, object>();
                        location = new Dictionary<string, object>();
                        parameters = new Dictionary<string, object>();

                        row.Add("imei", dt.Rows[i]["imei"].ToString());
                        row.Add("dt_server", dt.Rows[i]["dt_server"].ToString());
                        row.Add("dt_tracker", dt.Rows[i]["dt_tracker"].ToString());
                        row.Add("lat", dt.Rows[i]["lat"].ToString());
                        row.Add("lng", dt.Rows[i]["lng"].ToString());
                        row.Add("altitude", dt.Rows[i]["altitude"].ToString());
                        row.Add("angle", dt.Rows[i]["angle"].ToString());
                        row.Add("speed", dt.Rows[i]["speed"].ToString());
                        parameters.Add("ignition_status", dt.Rows[i]["ignition_status"].ToString());
                        row.Add("params", parameters);
                        row.Add("loc_valid", dt.Rows[i]["loc_valid"].ToString());
                      
                        collection.Add(dt.Rows[i]["imei"].ToString(), row);
                        list_collection.Add(collection);
                    }

                }
               }
                catch (Exception ex)
                {
                    throw (ex);
                }

            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(list_collection), Encoding.UTF8, "application/json");
            return response;
        }

        public static DataTable ExecuteQueryLiveData(string Query)
        {
            DataTable dt = new DataTable();
            try
            {
                string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());

                dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }
    }
}
