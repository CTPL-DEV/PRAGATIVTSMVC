using PRAGATIVTS_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRAGATIVTSMVC.Controllers
{
    public class PVTSController : Controller
    {
     
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult ALLVEHICLES_DASHBOARD()
        //{
        //    int CATEG_ID = 3;
        //    BLL obj = new BLL();
        //   var data = obj.GETALL_VEHICLES_DASHBOARD(CATEG_ID);
        //   return Json(new { data = data });
       
        //}
	}
}