using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using PRAGATIVTS_MVC.Models;
namespace PRAGATIVTSMVC.Controllers
{
    public class ConsignmentAPIController : Controller
    {
        //
        // GET: /ConsignmentAPI/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Trip(string from, string vno, string despatchdate)
        {
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            DataTable dt = BLL.GetVEHICLE_PUSHPAK(from, vno, despatchdate);
            if (dt.Rows.Count > 0)
            {
                ViewBag.fromplace = dt.Rows[0]["frm"].ToString();
                ViewBag.toplace = dt.Rows[0]["too"].ToString();

                if (dt.Rows[0]["Gps_Status"] != "EMPTY" && dt.Rows[0]["Gps_Status"] != "empty")
                {
                    DataTable dt1 = BLL.GETVEHICLE_DETAILS_PUSHPAK(vno);
                    if(dt1.Rows.Count>0)
                    {
                        for (int i = 0; i < dt1.Rows.Count;i++)
                        {
                            obj.Add(new SMVTS_GRIDTRACK
                            {
                                VEHICLENUMBER = dt1.Rows[i]["VNO"].ToString(),
                                DATE = Convert.ToDateTime(dt1.Rows[i]["TRIPDATE"]),
                                PLACE = dt1.Rows[i]["PLACE"].ToString(),
                                LATITUDE = dt1.Rows[i]["tripdata_latitude"].ToString(),
                                LONGITUDE = dt1.Rows[i]["tripdata_longitude"].ToString(),
                            });
                        }
                            
                    }
                }
                else
                {
                    ViewBag.message = "TRIP COMPLETE";
                }
            }
            ViewBag.vehiclesdetails = obj;


            return View();
        }

        //API_LIVEDATA
      

     

    }
}