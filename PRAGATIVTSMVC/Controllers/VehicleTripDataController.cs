using Newtonsoft.Json;
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
    public class VehicleTripDataController : ApiController
    {
        // GET: VehicleTripData

        [HttpGet]
        [Route("api/ParentRouteTracking")]
        public HttpResponseMessage GetParentMobileNo(string MobileNo,string VehicleNo)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<FetchVehicleTripDataDetails> obj = new List<FetchVehicleTripDataDetails>();
            DataTable dt = BLL.FetchVehicleTripDataDetails(MobileNo, VehicleNo);
            if (dt.Rows.Count > 0)
            {
                obj.Add(new FetchVehicleTripDataDetails
                {
                    
                    VehicelNo = dt.Rows[0]["VehicelNo"].ToString(),
                    Lattitude = dt.Rows[0]["Lattitude"].ToString(),
                    Langitude = dt.Rows[0]["Langitude"].ToString(),
                    Place = dt.Rows[0]["Place"].ToString(),
                    Speed = dt.Rows[0]["Speed"].ToString(),
                    //Direction = dt.Rows[0]["Direction"].ToString(),
                });
            }
            else
            {

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(obj), Encoding.UTF8, "application/json");
            return response;


        }

        public class FetchVehicleTripDataDetails
        {

            public string VehicelNo { get; set; }
           
            public string Lattitude { get; set; }
            public string Langitude { get; set; }
            public string Place { get; set; }
            public string Datetime { get; set; }
            public string Speed { get; set; }

            public string Direction { get; set; }

          
        }
    }

}
