using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PRAGATIVTSMVC.Controllers
{
    public class SavePackagesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Packages(string PackageName, string Price, string gst, string Num_Days, string Code)
        {
            var resp = new HttpResponseMessage();
            var response = Request.CreateResponse(HttpStatusCode.OK);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            try
            {
                bool status = BLL.SaveMisPackages(PackageName, Price, gst, Num_Days, Code);
                response.Content = new StringContent(serializer.Serialize(status), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }
    }
}
