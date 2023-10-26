using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PRAGATIVTSMVC.Controllers
{
    public class FETCHDATAAPIController : ApiController
    {
        [HttpGet]
        public dynamic FetchLive_Data(string Token, string VehicleNo)
        {
            List<Fetchlivedate> obj = new List<Fetchlivedate>();
            //ignition = dt.Rows[i]["IgnitionStatus"].ToString("dd MMM yyyy HH:mm:ss"),
            DataTable dt = BLL.API_LIVEDATA(Token, VehicleNo);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime dt1 = Convert.ToDateTime(dt.Rows[i]["DateTime"]);

             
                string fulldate=   dt1.ToString("dd MMMM yyyy HH:mm:ss");
                    obj.Add(new Fetchlivedate
                  {
                      longitude = dt.Rows[i]["Long"].ToString(),
                      latitude = dt.Rows[i]["Lat"].ToString(),
                      location = dt.Rows[i]["Location"].ToString(),
                      speed = dt.Rows[i]["speed"].ToString(),
                      dttime = fulldate,
                      ignition = dt.Rows[i]["IgnitionStatus"].ToString(),
                      vehicle_name = dt.Rows[i]["VehicleNo"].ToString(),
                  });
                }

            }
            else
            {
                // ViewBag.message = "TRIP COMPLETE";
            }

            return new
            {
                Vehicle = obj
            };
        }

        //public class ReturnedJson
        //{
        //    public IList<Fetchlivedate> People { get; set; }
        //}
        public class Fetchlivedate
        {
            //  public IEnumerable<Fetchlivedate> People { get; set; }
            public string longitude { get; set; }
            public string latitude { get; set; }
            public string location { get; set; }
            public string speed { get; set; }
            public string dttime { get; set; }
            public string ignition { get; set; }
            public string vehicle_name { get; set; }

        }


    }
}
