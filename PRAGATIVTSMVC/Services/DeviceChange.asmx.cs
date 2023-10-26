using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;

namespace PRAGATIVTSMVC.Services
{
    /// <summary>
    /// Summary description for DeviceChange
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeviceChange : System.Web.Services.WebService
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = null;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        response resp = null;

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = "For  Device Change")]
        public string DeviceInstallaton(int dist_id, int dealer_id, int cust_id,string old_imei_no, string new_imei_no,string device_type)
        {
           string status = BLL.updateDeviceAPI(dealer_id, cust_id, new_imei_no, old_imei_no, device_type);
            return status;
        }
    }
}
