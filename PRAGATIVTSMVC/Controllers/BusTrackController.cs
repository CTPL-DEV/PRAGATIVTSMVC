using PRAGATIVTSMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for BusTrackController
/// </summary>
public class BusTrackController : BaseController
{
    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Live()
    {
        return View();
    }
}