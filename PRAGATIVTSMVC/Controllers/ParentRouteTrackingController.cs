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
    public class ParentRouteTrackingController : ApiController
    {
        // GET: ParentRouteTracking    


        [HttpGet]
        [Route("api/ParentRouteTracking/GetParentMobileNo/{MobileNo}")]
        public HttpResponseMessage GetParentMobileNo(string MobileNo)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<FetchVehicleDetails> obj = new List<FetchVehicleDetails>();
            DataTable dt = BLL.FetchVehicleDetailsByPhone(MobileNo);
            if (dt.Rows.Count > 0)
            {
                obj.Add(new FetchVehicleDetails
                {
                    StudentName = dt.Rows[0]["StudentName"].ToString(),
                    VehicelNo = dt.Rows[0]["VehicelNo"].ToString(),
                    Shift_type = dt.Rows[0]["Shift_type"].ToString(),
                    Route_Name = dt.Rows[0]["Route_Name"].ToString(),
                    RouteNo = dt.Rows[0]["RouteNo"].ToString(),
                    //Lattitude = dt.Rows[0]["Lattitude"].ToString(),
                    //Langitude = dt.Rows[0]["Langitude"].ToString(),
                    Place = dt.Rows[0]["Place"].ToString(),
                    Speed = dt.Rows[0]["Speed"].ToString(),
                });
            }
            else
            {

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(obj), Encoding.UTF8, "application/json");
            return response;


        }

        public class FetchVehicleDetails
        {

            public string VehicelNo { get; set; }
            public string RouteNo { get; set; }
            public string Lattitude { get; set; }
            public string Langitude { get; set; }
            public string Place { get; set; }
            public string Datetime { get; set; }
            public string Speed { get; set; }

            public string StudentName { get; set; }

            public string Shift_type { get; set; }

            public string Route_Name { get; set; }
        }
    }
}


    
