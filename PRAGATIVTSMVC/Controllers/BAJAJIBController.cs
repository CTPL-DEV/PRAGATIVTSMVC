using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRAGATIVTS_MVC.Models;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using Newtonsoft.Json;
//using Kendo.Mvc.UI;
//using Kendo.Mvc.Extensions;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;
using System.Web.Mvc.Html;
using System.Net;
using PRAGATIVTSMVC.LRRECEIPTPRODUCTION;
using System.Web.UI;
using System.Web.UI.WebControls;
//using PRAGATIVTSMVC.WebReference1;
//using PRAGATIVTSMVC.LRRECEIPTDEVELOPMENT;
//using PRAGATIVTSMVC.Controllers.BaseController;
namespace PRAGATIVTSMVC.Controllers
{
    public class BAJAJIBController : BaseController
    {
        //
        // GET: /BAJAJIB/
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Test()
        {
            int[] intArray2 = new int[6] { 2001, 2002, 2002, 2003, 2004, 2005 };
            int j = 0;
            ArrayList a1 = new ArrayList();
            foreach (int obj in intArray2)
            {


                if (j == 0)
                {
                    a1.Add("000");
                }
                if (a1.Contains(obj))
                {

                }
                else
                {
                    if (j == 0)
                    {
                        a1.Add(obj);
                        // write your code here 
                    }
                    else
                    {
                        // write your code here 
                        a1.Add(obj);
                    }

                }
                j++;
            }


            return View();
        }






        [SessionAuthorize]
        public ActionResult frm_BajajIBTrips()
        {
            //LoadWorkorders();
            //ViewBag.workorders = TempData["workorders"];
            //return View();
            DataTable dt = BLL.GetWorkOrders_NotAssign((((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID).ToString());
            List<workorders> mylist = new List<workorders>();

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                mylist.Add(new workorders
                {
                    Workordertext = dt.Rows[k]["WORkORDER_NO"].ToString(),
                    Workordervalue = dt.Rows[k]["WORkORDER_NO"].ToString(),
                });
            }
            ViewBag.workorders = mylist;
            return View();
        }


        public JsonResult workordersbasedonclients(string CLIENTID = null)
        {

            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                CLIENTID = CLIENTID;
            }
            else
            {
                CLIENTID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }
            DataTable dt = BLL.GetWorkOrders_NotAssign(CLIENTID);
            List<workorders> mylist = new List<workorders>();
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                mylist.Add(new workorders
                {
                    Workordertext = dt.Rows[k]["WORkORDER_NO"].ToString(),
                    Workordervalue = dt.Rows[k]["WORkORDER_NO"].ToString(),
                });
            }
            return Json(new { data = mylist });
        }

        public JsonResult workordersbasedonclients_DOISNULL(string CLIENTID = null)
        {
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                CLIENTID = CLIENTID;
            }
            else
            {
                CLIENTID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }
            DataTable dt = BLL.GetWorkOrders_NotAssign_DOISNULL(CLIENTID);
            List<workorders> mylist = new List<workorders>();
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                mylist.Add(new workorders
                {
                    Workordertext = dt.Rows[k]["WORkORDER_NO"].ToString(),
                    Workordervalue = dt.Rows[k]["WORkORDER_NO"].ToString(),
                });
            }
            return Json(new { data = mylist });
        }



        public JsonResult dostatusupdate(string WORKORDERNO)
        {

            bool b = BLL.UPDATEDO_STATUS(WORKORDERNO);
            return Json(new { data = b });
        }
        public JsonResult pickuptimedate(string date, string tripid)
        {

            bool b = BLL.UPDATE_PICKUPTIME(date, Convert.ToInt32(tripid));
            return Json(new { data = b });
        }


        [SessionAuthorize]
        public JsonResult frm_getall_workorders(string ClientID = null)
        {
            //LoadWorkorders();
            //ViewBag.workorders = TempData["workorders"];
            //return View();
            string categid = "";
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                categid = ClientID;
            }
            else
            {
                categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }


            DataTable dt = BLL.GetWorkOrdersALL(categid);
            List<workorders> mylist = new List<workorders>();

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                mylist.Add(new workorders
                {
                    Workordertext = dt.Rows[k]["TRIP_WORKORDER_ID"].ToString(),
                    Workordervalue = dt.Rows[k]["TRIP_WORKORDER_ID"].ToString(),
                });
            }
            var data = mylist;
            return Json(new { data = data });
        }


        [SessionAuthorize]
        public JsonResult frm_all_workorders()
        {
            //LoadWorkorders();
            //ViewBag.workorders = TempData["workorders"];
            //return View();



            DataTable dt = BLL.GetALLWORK_ORDERS();
            List<workorders> mylist = new List<workorders>();

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                mylist.Add(new workorders
                {
                    Workordertext = dt.Rows[k]["TRIP_WORKORDER_ID"].ToString(),
                    Workordervalue = dt.Rows[k]["TRIP_WORKORDER_ID"].ToString(),
                });
            }
            var data = mylist;
            return Json(new { data = data });
        }











        [SessionAuthorize]
        public JsonResult Load_Clients()
        {
            //(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID).ToString()
            //rcmb_Clients.Items.Clear();


            SMVTS_CATEGORIES _obj_Smvts_Categories = new SMVTS_CATEGORIES();
            _obj_Smvts_Categories.OPERATION = operation.Insert;
            _obj_Smvts_Categories.CATEG_CATETYPE_ID = 3;
            _obj_Smvts_Categories.CATEG_ID = Convert.ToInt32((((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID).ToString());
            DataTable dt = BLL.get_Categories(_obj_Smvts_Categories);
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_CATEGORIES
                {
                    CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                    CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                });
            }
            return Json(new { data = obj });
        }



        [SessionAuthorize]
        public JsonResult PartiallWorkorders(string ClientID = null)
        {
            // LoadWorkorders(ClientID);
            // var data = TempData["workorders"];
            // var data = TempData["workorders"];
            List<workorders> obj = new List<workorders>();
            DataTable DT01;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                DT01 = BLL.GetPARTIALWORKORDERS(ClientID);
            }
            else
            {
                DT01 = BLL.GetPARTIALWORKORDERS((((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID).ToString());
            }

            for (int i = 0; i < DT01.Rows.Count; i++)
            {
                obj.Add(new workorders
                {
                    Workordertext = DT01.Rows[i]["WORKORDER_NO"].ToString(),
                    Workordervalue = DT01.Rows[i]["WORKORDER_NO"].ToString(),
                });
            }
            return Json(new { data = obj });
        }
        public JsonResult TRIPUPDATE(string VNO, string VID, string TRIPID, string WORKORDERNO, string ClientId = null)
        {
            bool b = BLL.UPDATETRIP(VID, VNO, Convert.ToInt32(TRIPID));
            if (b)
            {
                DataTable DT01;
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    DT01 = BLL.GetALLExisting_WorkOrdersInTrip(Convert.ToInt32(WORKORDERNO), ClientId);
                }
                else
                {
                    DT01 = BLL.GetALLExisting_WorkOrdersInTrip(Convert.ToInt32(WORKORDERNO), (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID).ToString());
                }



                VTSLRReceiptRecords[] _obj = new VTSLRReceiptRecords[DT01.Rows.Count];
                for (int j = 0; j < DT01.Rows.Count; j++)
                {
                    _obj[j] = new VTSLRReceiptRecords();
                    _obj[j].COSIGNMENT_ID = Convert.ToString(DT01.Rows[j]["TRIP_WORKORDER_ID"]);
                    _obj[j].LR_NUMBER = Convert.ToString(DT01.Rows[j]["TRIP_lr_NO"]);
                    _obj[j].LR_DATE = Convert.ToString(DT01.Rows[j]["TRIP_LR_DATE"]);//_obj_smvts_workorderTrips1.TRIP_LR_DATE;//.ToString("MM/dd/yy");
                    _obj[j].TRUCK_NO = Convert.ToString(DT01.Rows[j]["TRIP_VEHICLENO"]);
                    _obj[j].TRANSPORTER_ID = GET_TRANSPORTERCODE(Convert.ToInt32(DT01.Rows[j]["TRIP_WORKORDER_ID"]));
                }
                //VTSLRReceipt_OutService _outobj = new VTSLRReceipt_OutService();
                //NetworkCredential _n = new NetworkCredential();
                //_n.UserName = System.Configuration.ConfigurationManager.AppSettings["bajajusername"];
                //_n.Password = System.Configuration.ConfigurationManager.AppSettings["bajajpswd"];
                //_outobj.Credentials = _n;
                //_outobj.VTSLRReceipt_Out(_obj);
            }

            return Json(new { data = b });
        }



        public JsonResult Trip_Close(string WORKORDERNO, string ClientID = null)
        {
            bool b = BLL.CLOSE_TRIP(WORKORDERNO);
            if (b)
            {
                string categid = "";
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    categid = ClientID;
                }
                else
                {
                    categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                }

                DataTable DT01 = BLL.GetALLExisting_WorkOrdersInTrip(Convert.ToInt32(WORKORDERNO), categid);



                VTSLRReceiptRecords[] _obj = new VTSLRReceiptRecords[DT01.Rows.Count];
                for (int j = 0; j < DT01.Rows.Count; j++)
                {
                    _obj[j] = new VTSLRReceiptRecords();
                    _obj[j].COSIGNMENT_ID = Convert.ToString(DT01.Rows[j]["TRIP_WORKORDER_ID"]);
                    _obj[j].LR_NUMBER = Convert.ToString(DT01.Rows[j]["TRIP_lr_NO"]);
                    _obj[j].LR_DATE = Convert.ToString(DT01.Rows[j]["TRIP_LR_DATE"]);//_obj_smvts_workorderTrips1.TRIP_LR_DATE;//.ToString("MM/dd/yy");
                    _obj[j].TRUCK_NO = Convert.ToString(DT01.Rows[j]["TRIP_VEHICLENO"]);
                    _obj[j].TRANSPORTER_ID = GET_TRANSPORTERCODE(Convert.ToInt32(DT01.Rows[j]["TRIP_WORKORDER_ID"]));
                }
                //VTSLRReceipt_OutService _outobj = new VTSLRReceipt_OutService();
                //NetworkCredential _n = new NetworkCredential();
                //_n.UserName = System.Configuration.ConfigurationManager.AppSettings["bajajusername"];
                //_n.Password = System.Configuration.ConfigurationManager.AppSettings["bajajpswd"];
                //_outobj.Credentials = _n;
                //_outobj.VTSLRReceipt_Out(_obj);
            }
            bool c = BLL.TRIP_CLOSE_WORKORDER_TRIPS(WORKORDERNO);
            if (c)
            {

            }
            return Json(new { data = b });
        }
        [SessionAuthorize]
        public JsonResult _Bajajtrips(string WORKSTATUS, string ClientID = null)
        {
            //PENDING WORK ORDER GRID
            List<WORKORDERS> obj = new List<WORKORDERS>();
            if (WORKSTATUS == "OPEN")
            {
                //   First_Empty();

            }
            else if (WORKSTATUS == "In Progress")  // COMPLETED WORK ORDER GRID
            {


                obj = Load_CompletedWorkOrdersGrd(ClientID);



            }
            else
            {


                obj = Load_CompletedWorkOrdersGrd(ClientID);
                //  WorkOrderCompletedGrd.DataBind();


            }
            return Json(new { data = obj });
        }




        [SessionAuthorize]
        public void LoadWorkorders(string ClientID = null)
        {
            System.Data.DataTable dt_rcmb = new System.Data.DataTable();
            DateTime dt = DateTime.Now;
            System.Data.DataTable dtNu = new System.Data.DataTable();
            dtNu.Columns.Add("SrNo");
            dtNu.Columns.Add("SrNo1");
            dtNu.Columns.Add("WORKORDER_NO");
            dtNu.Columns.Add("VEHICLE_NO");
            dtNu.Columns.Add("LORRY_REG_NO");
            dtNu.Columns.Add("STATUS");
            dtNu.Columns.Add("LR_DATE");
            dtNu.Columns.Add("WORKORDER_COUNTRY");
            dt_rcmb.Columns.Add("WORkORDER_Text");
            dt_rcmb.Columns.Add("WORKORDER_Value");
            var categid = string.Empty;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                categid = ClientID;
            }
            else
            {
                categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }
            System.Data.DataTable dt_Wrd = BLL.GetPendingWorkOrders(categid);
            //new 7 FEB 2017
            int count = 1;
            if (dt_Wrd != null)
            {
                if (dt_Wrd.Rows.Count > 0)
                {
                    foreach (DataRow r in dt_Wrd.Rows)
                    {
                        int WorkOrderCount = Convert.ToInt32(r["WORKORDER_NO_OF_TRUCKS"]);
                        //Check already exisiting trips of the same workorder
                        System.Data.DataTable WOE = new System.Data.DataTable();
                        int WOECount = 0;
                        WOE = BLL.GetExisting_WorkOrdersInTrip(Convert.ToInt32(r["WORKORDER_NO"]));
                        if (WOE != null)
                        {
                            if (WOE.Rows.Count > 0)
                            {
                                WOECount = Convert.ToInt32(WOE.Rows[0][0]);
                            }
                            WorkOrderCount = WorkOrderCount - WOECount;
                        }
                        for (int i = 1; i <= WorkOrderCount; i++)
                        {
                            DataRow rnu = dtNu.NewRow();
                            rnu["SrNo"] = count;
                            rnu["SrNo1"] = count;
                            rnu["WORKORDER_NO"] = r["WORKORDER_NO"];
                            rnu["STATUS"] = "Pending";
                            rnu["VEHICLE_NO"] = "";
                            rnu["LORRY_REG_NO"] = "";
                            rnu["LR_DATE"] = dt;
                            rnu["WORKORDER_COUNTRY"] = r["WORKORDER_COUNTRY"];
                            count++;
                            dtNu.Rows.Add(rnu);
                        }
                    }
                }
            }
            DataView view = new DataView(dtNu);
            System.Data.DataTable distinctValues = view.ToTable(true, "WORKORDER_NO");
            //new 7 FEB 2017
            //if (dt_Wrd != null)
            if (distinctValues != null)
            {
                if (distinctValues.Rows.Count > 0)
                {
                    foreach (DataRow dr in distinctValues.Rows)
                    {
                        DataRow r1 = dt_rcmb.NewRow();
                        r1["WORkORDER_Text"] = dr["WORkORDER_NO"];
                        r1["WORKORDER_Value"] = dr["WORkORDER_NO"];
                        dt_rcmb.Rows.Add(r1);
                    }


                    List<workorders> mylist = new List<workorders>();

                    for (int k = 0; k < dt_rcmb.Rows.Count; k++)
                    {
                        mylist.Add(new workorders
                        {
                            Workordertext = dt_rcmb.Rows[k]["WORkORDER_Text"].ToString(),
                            Workordervalue = dt_rcmb.Rows[k]["WORKORDER_Value"].ToString(),

                        });
                    }
                    TempData["workorders"] = mylist;
                }
            }
        }


        public class workorders
        {
            public string Workordertext { get; set; }
            public string Workordervalue { get; set; }
        }







        [SessionAuthorize]
        public List<WORKORDERS> First_Empty()
        {
            List<WORKORDERS> obj = new List<WORKORDERS>();
            DateTime dt = DateTime.Now;
            System.Data.DataTable dt1 = new System.Data.DataTable();
            System.Data.DataTable dtNu = new System.Data.DataTable();
            dtNu.Columns.Add("SrNo");
            dtNu.Columns.Add("SrNo1");
            dtNu.Columns.Add("WORKORDER_NO");
            dtNu.Columns.Add("VEHICLE_NO");
            dtNu.Columns.Add("LORRY_REG_NO");
            dtNu.Columns.Add("STATUS");
            dtNu.Columns.Add("LR_DATE");
            dtNu.Columns.Add("WORKORDER_COUNTRY");
            var categid = string.Empty;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                categid = "";
            }
            else
            {
                categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }
            dt1 = BLL.GetPendingWorkOrders(categid);
            int count = 1;
            if (dt1 != null)
            {
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow r in dt1.Rows)
                    {
                        int WorkOrderCount = Convert.ToInt32(r["WORKORDER_NO_OF_TRUCKS"]);
                        //Check already exisiting trips of the same workorder
                        System.Data.DataTable WOE = new System.Data.DataTable();
                        int WOECount = 0;
                        WOE = BLL.GetExisting_WorkOrdersInTrip(Convert.ToInt32(r["WORKORDER_NO"]));
                        if (WOE != null)
                        {
                            if (WOE.Rows.Count > 0)
                            {
                                WOECount = Convert.ToInt32(WOE.Rows[0][0]);
                            }
                            WorkOrderCount = WorkOrderCount - WOECount;
                        }
                        for (int i = 1; i <= WorkOrderCount; i++)
                        {
                            DataRow rnu = dtNu.NewRow();
                            rnu["SrNo"] = count;
                            rnu["SrNo1"] = count;
                            rnu["WORKORDER_NO"] = r["WORKORDER_NO"];
                            rnu["STATUS"] = "Pending";
                            rnu["VEHICLE_NO"] = "";
                            rnu["LORRY_REG_NO"] = "";
                            rnu["LR_DATE"] = dt;
                            rnu["WORKORDER_COUNTRY"] = r["WORKORDER_COUNTRY"];
                            count++;
                            dtNu.Rows.Add(rnu);
                        }
                    }
                }
            }
            //   dtNu.Clear();
            //  WorkOrdersGrd.DataSource = dtNu;
            //  WorkOrdersGrd.DataBind();

            //foreach (GridDataItem gridrow in WorkOrdersGrd.Items)
            //{
            //    System.Data.DataTable dtvno = new System.Data.DataTable();

            //    if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            //    {
            //        dtvno = BLL.get_ReportDevices("DEVICES2", Convert.ToString(rcmb_Clients.SelectedValue));
            //    }
            //    else
            //    {
            //        dtvno = BLL.get_ReportDevices("DEVICES1", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
            //    }



            //    ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataSource = dtvno;

            //    ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataTextField = "DEVICE_NAME";
            //    //((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataValueField = "DEVICE_ID";
            //    ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataValueField = "DEVICE_NAME";
            //    ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataBind();

            //    if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "3")
            //    {
            //        if (((DropDownList)gridrow.FindControl("txt_vehicleinfo")).Items.Count > 15)
            //            ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).Height = 250;
            //        else
            //            ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).Attributes.Add("style", "height:auto;");
            //    }

            //    ((RadDateTimePicker)gridrow.FindControl("rdtp_FromDate")).SelectedDate = DateTime.Now;
            //}

            //for (int j = 0; j < dtNu.Rows.Count; j++)
            //{
            //    ((DropDownList)WorkOrdersGrd.Items[j].FindControl("txt_vehicleinfo")).Items.Insert(0, new ListItem("Select Vehicle", "-1"));
            //    ((DropDownList)WorkOrdersGrd.Items[j].FindControl("txt_vehicleinfo")).SelectedValue = "-1";

            //}
            //WorkOrdersGrd.Visible = true;
            //btn_Save.Visible = true;
            //WorkOrderCompletedGrd.Visible = false;
            //Editdiv.Visible = false;
            //rcmbWOdiv.Visible = true;
            return obj;
        }


        [SessionAuthorize]
        public JsonResult Getworkorders_working(string ClientID)
        {
            List<WORKORDERS> obj = new List<WORKORDERS>();
            try
            {
                string categid = "";
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    categid = ClientID;
                }
                else
                {
                    categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                }
                DataTable DT = BLL.GetPendingWorkOrders_IB(categid);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    obj.Add(new WORKORDERS
                    {
                        WORKORDERNO = DT.Rows[i]["TRIP_WORKORDER_ID"].ToString(),
                    });
                }
            }
            catch
            {

            }
            return Json(new { data = obj });
        }


        [SessionAuthorize]
        public List<WORKORDERS> Load_CompletedWorkOrdersGrd(string ClientID = null)
        {
            List<WORKORDERS> obj = new List<WORKORDERS>();

            DataTable dt1 = new DataTable();
            var categid = string.Empty;
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                categid = ClientID;
            }
            else
            {
                categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }
            dt1 = BLL.GetWorkOrdersCompleted(categid);


            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                obj.Add(new WORKORDERS
                {
                    WORKORDERNO = dt1.Rows[i]["TRIP_WORKORDER_ID"].ToString(),
                    VEHICLENO = dt1.Rows[i]["TRIP_VEHICLENO"].ToString(),
                    LRNO = dt1.Rows[i]["TRIP_LR_NO"].ToString(),
                    INDENTCREATEION = dt1.Rows[i]["WORKORDER_CREATED_DATETIME"].ToString(),

                    REPORTOUT = dt1.Rows[i]["TRIP_REPORT_OUTTIME_PORT"].ToString(),
                    PLANTIN = dt1.Rows[i]["TRIP_PLANT_INTIME"].ToString(),
                    PLANTOUT = dt1.Rows[i]["TRIP_PLANT_OUTTIME"].ToString(),
                    UNLOADIN = dt1.Rows[i]["TRIP_UNLOAD_INTIME"].ToString(),
                    UNLOADOUT = dt1.Rows[i]["TRIP_UNLOAD_OUTTIME"].ToString(),
                    TRIPID = dt1.Rows[i]["TRIP_ID"].ToString(),
                    WORKORDER_CO = dt1.Rows[i]["WORKORDER_COUNTRY"].ToString(),
                    TRIP_DEVICEID = dt1.Rows[i]["TRIP_DEVICEID"].ToString(),
                    PICKUPTIME = dt1.Rows[i]["PICKUPTIME"].ToString(),
                    DOSTATUS = dt1.Rows[i]["DOSTATUS"].ToString(),
                });
            }

            //  WorkOrderCompletedGrd.DataSource = dt1;




            return obj;
        }


        public class WORKORDERS
        {
            public string WORKORDERNO { get; set; }
            public string VEHICLENO { get; set; }

            public string LRNO { get; set; }

            public string LRDATE { get; set; }

            public string REPORTOUT { get; set; }

            public string PLANTIN { get; set; }
            public string PLANTOUT { get; set; }

            public string UNLOADIN { get; set; }

            public string UNLOADOUT { get; set; }

            public int SrNo { get; set; }

            public string STATUS { get; set; }

            public string WORKORDER_CO { get; set; }

            public string TRIP_VEHICLENO { get; set; }

            public string TRIPID { get; set; }

            public string TRIP_DEVICEID { get; set; }

            public string PICKUPTIME { get; set; }

            public string DOSTATUS { get; set; }

            public string INDENTCREATEION { get; set; }
        }





        public JsonResult Loadvehicles(string ClientID = null)
        {
            List<SMVTS_DEVICES> obj1 = new List<SMVTS_DEVICES>();
            System.Data.DataTable dtvno = new System.Data.DataTable();
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                dtvno = BLL.get_ReportDevices("DEVICES2", Convert.ToString(ClientID));
                // dtvno = BLL.get_ReportDevices("DEVICES3", Convert.ToString(ClientID));
            }
            else
            {
                dtvno = BLL.get_ReportDevices("DEVICES1", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
                // dtvno = BLL.get_ReportDevices("DEVICES3", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
            }


            for (int j = 0; j < dtvno.Rows.Count; j++)
            {

                obj1.Add(new SMVTS_DEVICES
                {
                    DEVICE_NAME = dtvno.Rows[j]["DEVICE_NAME"].ToString(),
                    DEVICE_ID = Convert.ToInt32(dtvno.Rows[j]["DEVICE_ID"]),
                });
            }
            return Json(new { data = obj1 });
        }






        [SessionAuthorize]
        public JsonResult rcmb_WorkOrders_SelectedIndexChanged(string WorkStatus, string Workorder, string ClientID = null)
        {
            List<WORKORDERS> obj = new List<WORKORDERS>();
            List<SMVTS_DEVICES> obj1 = new List<SMVTS_DEVICES>();
            List<WORKORDERS> obj01 = new List<WORKORDERS>();
            if (WorkStatus == "OPEN" || WorkStatus == "Partially Open")
            {
                //if (rcmb_WorkOrders.SelectedItem.Value != "-1")
                if (Workorder != "0" && Workorder != "")
                {
                    var SelectedWorkOrder = Workorder;
                    DateTime dt = DateTime.Now;
                    System.Data.DataTable dt1 = new System.Data.DataTable();
                    System.Data.DataTable dtNu = new System.Data.DataTable();
                    dtNu.Columns.Add("SrNo");
                    dtNu.Columns.Add("SrNo1");
                    dtNu.Columns.Add("WORKORDER_NO");
                    dtNu.Columns.Add("VEHICLE_NO");
                    dtNu.Columns.Add("LORRY_REG_NO");
                    dtNu.Columns.Add("STATUS");
                    dtNu.Columns.Add("LR_DATE");
                    dtNu.Columns.Add("WORKORDER_COUNTRY");
                    var categid = string.Empty;
                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                    {
                        categid = ClientID;
                    }
                    else
                    {
                        categid = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                    }
                    dt1 = BLL.GetPendingWorkOrders(categid);
                    int count = 1;
                    int WorkOrderCount = 0;
                    string WorkOrderCounty = string.Empty;
                    if (dt1 != null)
                    {
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt1.Rows)
                            {
                                if (dr["WORKORDER_NO"].ToString() == SelectedWorkOrder)
                                {
                                    WorkOrderCount = Convert.ToInt32(dr["WORKORDER_NO_OF_TRUCKS"]);
                                    WorkOrderCounty = Convert.ToString(dr["WORKORDER_COUNTRY"]);
                                }
                            }
                            //Check already exisiting trips of the same workorder
                            System.Data.DataTable WOE = new System.Data.DataTable();
                            int WOECount = 0;
                            WOE = BLL.GetExisting_WorkOrdersInTrip(Convert.ToInt32(SelectedWorkOrder));



                            if (WOE != null)
                            {
                                if (WOE.Rows.Count > 0)
                                {
                                    WOECount = Convert.ToInt32(WOE.Rows[0][0]);
                                }
                                //pradeep  
                                WorkOrderCount = WorkOrderCount - WOECount;
                            }



                            for (int i = 1; i <= WorkOrderCount; i++)
                            {
                                DataRow rnu = dtNu.NewRow();
                                rnu["SrNo"] = count;
                                rnu["SrNo1"] = count;
                                rnu["WORKORDER_NO"] = SelectedWorkOrder;
                                rnu["STATUS"] = "Pending";
                                rnu["VEHICLE_NO"] = "";
                                rnu["LORRY_REG_NO"] = "";
                                rnu["LR_DATE"] = dt;
                                rnu["WORKORDER_COUNTRY"] = WorkOrderCounty;
                                count++;
                                dtNu.Rows.Add(rnu);
                            }



                            if (WOECount > 0)
                            {
                                if (WOECount < WorkOrderCount)
                                {
                                    DataTable dt01;
                                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                                    {
                                        dt01 = BLL.GetALLExisting_WorkOrdersInTrip(Convert.ToInt32(Workorder), ClientID);
                                    }
                                    else
                                    {
                                        dt01 = BLL.GetALLExisting_WorkOrdersInTrip(Convert.ToInt32(Workorder), Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
                                    }


                                    for (int m = 0; m < dt01.Rows.Count; m++)
                                    {
                                        obj01.Add(new WORKORDERS
                                        {
                                            TRIPID = dt01.Rows[m]["TRIP_ID"].ToString(),
                                            TRIP_VEHICLENO = dt01.Rows[m]["TRIP_VEHICLENO"].ToString(),
                                            TRIP_DEVICEID = dt01.Rows[m]["TRIP_DEVICEID"].ToString(),
                                        });
                                    }
                                }
                            }
                        }

                    }





                    for (int i = 0; i < dtNu.Rows.Count; i++)
                    {
                        obj.Add(new WORKORDERS
                        {

                            SrNo = Convert.ToInt32(dtNu.Rows[i]["SrNo"]),
                            WORKORDERNO = dtNu.Rows[i]["WORKORDER_NO"].ToString(),
                            STATUS = dtNu.Rows[i]["STATUS"].ToString(),
                            LRDATE = dtNu.Rows[i]["LR_DATE"].ToString(),
                            WORKORDER_CO = dtNu.Rows[i]["WORKORDER_COUNTRY"].ToString(),
                        });
                    }





                    //   WorkOrdersGrd.DataSource = dtNu;
                    //  WorkOrdersGrd.DataBind();

                    //  foreach (GridDataItem gridrow in dtNu.Rows)
                    // {
                    System.Data.DataTable dtvno = new System.Data.DataTable();
                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                    {
                        //PRADEEP
                        //  dtvno = BLL.get_ReportDevices("DEVICES2", Convert.ToString(ClientID));
                        dtvno = BLL.get_ReportDevices("DEVICES3", Convert.ToString(ClientID));

                    }
                    else
                    {
                        //  dtvno = BLL.get_ReportDevices("DEVICES1", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
                        dtvno = BLL.get_ReportDevices("DEVICES3", Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
                    }


                    for (int j = 0; j < dtvno.Rows.Count; j++)
                    {

                        obj1.Add(new SMVTS_DEVICES
                        {
                            DEVICE_NAME = dtvno.Rows[j]["DEVICE_NAME"].ToString(),
                            DEVICE_ID = Convert.ToInt32(dtvno.Rows[j]["DEVICE_ID"]),
                        });

                    }



                    //((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataSource = dtvno;
                    //((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataTextField = "DEVICE_NAME";
                    //((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataValueField = "DEVICE_NAME";
                    //((DropDownList)gridrow.FindControl("txt_vehicleinfo")).DataBind();




                    if ((((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString()) == "3")
                    {
                        //if (((DropDownList)gridrow.FindControl("txt_vehicleinfo")).Items.Count > 15)
                        //    ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).Height = 250;
                        //else
                        //    ((DropDownList)gridrow.FindControl("txt_vehicleinfo")).Attributes.Add("style", "height:auto;");
                    }

                    //  ((RadDateTimePicker)gridrow.FindControl("rdtp_FromDate")).SelectedDate = DateTime.Now;
                    // }

                    for (int j = 0; j < dtNu.Rows.Count; j++)
                    {
                        //  ((DropDownList)WorkOrdersGrd.Items[j].FindControl("txt_vehicleinfo")).Items.Insert(0, new ListItem("Select Vehicle", "-1"));
                        //  ((DropDownList)WorkOrdersGrd.Items[j].FindControl("txt_vehicleinfo")).SelectedValue = "-1";

                    }
                    //WorkOrdersGrd.Visible = true;
                    //btn_Save.Visible = true;
                    //WorkOrderCompletedGrd.Visible = false;
                    //Editdiv.Visible = false;

                }
                else
                {
                    if (Workorder == "All Order")
                    {
                        // First();
                    }
                    else if (Workorder == "Select Order")
                    {
                        //  First_Empty();
                    }
                    else
                    {

                    }
                }
            }


            var jsonData = new
            {
                data = obj,
                data1 = obj1,
                data2 = obj01
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }



        public JsonResult btn_Save_Click(string btn, string TRIP_WORKORDER_ID, string VNO, string DEVICEID, string status, string ClientId = null)
        {
            string LRDate1 = string.Empty;
            string MSG = "false";
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("COSIGNMENT_ID");
            dt.Columns.Add("LR_NUMBER");
            dt.Columns.Add("LR_DATE");
            dt.Columns.Add("TRUCK_NO");
            dt.Columns.Add("TRANSPORTER_CODE");
            //DataColumn COSIGNMENT_ID=new DataColumn("COSIGNMENT_ID");
            //DataColumn LR_NUMBER=new DataColumn("LR_NUMBER");
            // DataColumn LR_DATE=new DataColumn("LR_DATE");
            //  DataColumn TRUCK_NO=new DataColumn("TRUCK_NO");


            //FOR INSERTION INTO TABLE SMVTS_WORKORDER_TRIPS
            int TI = 0;

            DataTable dt01 = BLL.GetlastTripId(Convert.ToInt32(TRIP_WORKORDER_ID));
            if (dt01.Rows.Count > 0)
            {
                TI = Convert.ToInt32(dt01.Rows[0]["TRIP_LR_NO"]);
                TI = TI + 1;
            }
            else
            {
                TI = 1;
            }







            if (btn == "BTN_SAVE")
            {
                bool s = false;
                SMVTS_WORKORDER_TRIPS _obj_smvts_workorderTrips1 = new SMVTS_WORKORDER_TRIPS();
                string sp = VNO;
                string sp1 = DEVICEID;

                string[] values = sp.Split(',');
                string[] deviceid = sp1.Split(',');

                for (int n = 0; n < values.Length; n++)
                {
                    values[n] = values[n].Trim();
                    //   if (((System.Web.UI.WebControls.CheckBox)gridrow.FindControl("RowCheck")).Checked == true)
                    //  {

                    _obj_smvts_workorderTrips1.TRIP_WORKORDER_ID = Convert.ToInt32(TRIP_WORKORDER_ID);
                    _obj_smvts_workorderTrips1.TRIP_VEHICLENO = values[n]; //.SelectedValue;
                    _obj_smvts_workorderTrips1.TRIP_DEVICEID = deviceid[n];
                    _obj_smvts_workorderTrips1.TRIP_DEVICEID = values[n];
                    _obj_smvts_workorderTrips1.TRIP_LR_NO = TI.ToString();
                    _obj_smvts_workorderTrips1.TRIP_LR_DATE = Convert.ToString(DateTime.Now);
                    _obj_smvts_workorderTrips1.TRIP_CREATED_BY = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
                    _obj_smvts_workorderTrips1.TRIP_CREATED_DATETIME = DateTime.Now;
                    _obj_smvts_workorderTrips1.TRIP_MODIFIED_BY = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
                    _obj_smvts_workorderTrips1.TIRP_MODIFIED_DATETIME = DateTime.Now;
                    _obj_smvts_workorderTrips1.TRIP_STATUS = 2; //Vehicle assigned

                    var categid = string.Empty;
                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                    {
                        _obj_smvts_workorderTrips1.TRIP_CATEGORY_ID = Convert.ToInt32(ClientId);
                    }
                    else
                    {
                        _obj_smvts_workorderTrips1.TRIP_CATEGORY_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                    }
                    //DateTime dt1 = Convert.ToDateTime(_obj_smvts_workorderTrips1.TRIP_LR_DATE);
                    //LRDate1 = dt1.Day.ToString() + "/" + dt1.Month.ToString() + "/" + dt1.Year.ToString();
                    s = BLL.set_WorkOrderTrip(_obj_smvts_workorderTrips1);
                    if (s)
                    {
                        MSG = "true";
                        DataRow dr = dt.NewRow();
                        dr["COSIGNMENT_ID"] = Convert.ToString(_obj_smvts_workorderTrips1.TRIP_WORKORDER_ID);
                        dr["LR_NUMBER"] = _obj_smvts_workorderTrips1.TRIP_LR_NO;
                        dr["LR_DATE"] = _obj_smvts_workorderTrips1.TRIP_LR_DATE;
                        dr["TRUCK_NO"] = _obj_smvts_workorderTrips1.TRIP_VEHICLENO;
                        dr["TRANSPORTER_CODE"] = GET_TRANSPORTERCODE(_obj_smvts_workorderTrips1.TRIP_WORKORDER_ID);
                        dt.Rows.Add(dr);
                    }
                    TI++;
                }


                // }





                VTSLRReceiptRecords[] _obj = new VTSLRReceiptRecords[dt.Rows.Count];
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    _obj[j] = new VTSLRReceiptRecords();
                    _obj[j].COSIGNMENT_ID = Convert.ToString(dt.Rows[j]["COSIGNMENT_ID"]);
                    _obj[j].LR_NUMBER = Convert.ToString(dt.Rows[j]["LR_NUMBER"]);
                    _obj[j].LR_DATE = Convert.ToString(dt.Rows[j]["LR_DATE"]);//_obj_smvts_workorderTrips1.TRIP_LR_DATE;//.ToString("MM/dd/yy");
                    _obj[j].TRUCK_NO = Convert.ToString(dt.Rows[j]["TRUCK_NO"]);
                    _obj[j].TRANSPORTER_ID = Convert.ToString(dt.Rows[j]["TRANSPORTER_CODE"]);
                }
                //VTSLRReceipt_OutService _outobj = new VTSLRReceipt_OutService();

                //NetworkCredential _n = new NetworkCredential();
                //_n.UserName = System.Configuration.ConfigurationManager.AppSettings["bajajusername"];
                //_n.Password = System.Configuration.ConfigurationManager.AppSettings["bajajpswd"];


                //_outobj.Credentials = _n;

                //_outobj.VTSLRReceipt_Out(_obj);
                //if (s)
                //{
                //   // MSG = "true";
                //}
                //else
                //{
                   
                //}

                MSG = "true";
            }

            //WorkOrdersGrd.Visible = false;
            //btn_Save.Visible = false;
            //Editdiv.Visible = false;
            //rcmbWOdiv.Visible = false;
            //rcmb_VehicleStatus.SelectedIndex = 0;
            //btn_DeleteWorkOrder.Visible = false;
            return Json(new { data = MSG });
        }



        public JsonResult BTN_TRIP_UPDATE_CLICK(string TRIP_WORKORDER_ID, string VNO, string DEVICEID, string TRIPID, string status, string ClientId = null)
        {
            string LRDate1 = string.Empty;
            string MSG01 = "false";

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("COSIGNMENT_ID");
            dt.Columns.Add("LR_NUMBER");
            dt.Columns.Add("LR_DATE");
            dt.Columns.Add("TRUCK_NO");
            dt.Columns.Add("TRANSPORTER_CODE");

            bool s = false;
            SMVTS_WORKORDER_TRIPS _obj_smvts_workorderTrips1 = new SMVTS_WORKORDER_TRIPS();
            string sp = VNO;
            string sp1 = DEVICEID;

            string trip = TRIPID;

            string[] values = sp.Split(',');
            string[] deviceid = sp1.Split(',');
            string[] TRIP_ID = trip.Split(',');



            int TI = 0;

            DataTable dt01 = BLL.GetlastTripId(Convert.ToInt32(TRIP_WORKORDER_ID));
            if (dt01.Rows.Count > 0)
            {
                TI = Convert.ToInt32(dt01.Rows[0]["TRIP_LR_NO"]);
                TI = TI + 1;
            }
            else
            {
                TI = 1;
            }







            for (int n = 0; n < values.Length; n++)
            {
                values[n] = values[n].Trim();


                _obj_smvts_workorderTrips1.TRIP_WORKORDER_ID = Convert.ToInt32(TRIP_WORKORDER_ID);
                _obj_smvts_workorderTrips1.TRIP_VEHICLENO = values[n]; //.SelectedValue;
                _obj_smvts_workorderTrips1.TRIP_DEVICEID = deviceid[n];
                //_obj_smvts_workorderTrips1.TRIP_DEVICEID = values[n];
                _obj_smvts_workorderTrips1.TRIP_LR_NO = TI.ToString();
                _obj_smvts_workorderTrips1.TRIP_LR_DATE = Convert.ToString(DateTime.Now);
                _obj_smvts_workorderTrips1.TRIP_CREATED_BY = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
                _obj_smvts_workorderTrips1.TRIP_CREATED_DATETIME = DateTime.Now;
                _obj_smvts_workorderTrips1.TRIP_MODIFIED_BY = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
                _obj_smvts_workorderTrips1.TIRP_MODIFIED_DATETIME = DateTime.Now;
                _obj_smvts_workorderTrips1.TRIP_STATUS = 2; //Vehicle assigned

                var categid = string.Empty;
                if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                {
                    _obj_smvts_workorderTrips1.TRIP_CATEGORY_ID = Convert.ToInt32(ClientId);
                }
                else
                {
                    _obj_smvts_workorderTrips1.TRIP_CATEGORY_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                }
                int z = n;
                if (Convert.ToInt32(TRIP_ID.Length) > z)
                {
                    s = BLL.UPDATE_WORKORDERTRIP(values[n], TRIP_ID[n]);
                }
                else
                {
                    s = BLL.set_WorkOrderTrip(_obj_smvts_workorderTrips1);
                    if (s)
                    {
                        MSG01 = "true";
                        DataRow dr = dt.NewRow();
                        dr["COSIGNMENT_ID"] = Convert.ToString(_obj_smvts_workorderTrips1.TRIP_WORKORDER_ID);
                        dr["LR_NUMBER"] = "";
                        dr["LR_DATE"] = "";//_obj_smvts_workorderTrips1.TRIP_LR_DATE;
                        dr["TRUCK_NO"] = _obj_smvts_workorderTrips1.TRIP_VEHICLENO;
                        dr["TRANSPORTER_CODE"] = GET_TRANSPORTERCODE(_obj_smvts_workorderTrips1.TRIP_WORKORDER_ID);
                        dt.Rows.Add(dr);
                    }

                    DataTable DT01;
                    if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
                    {
                        DT01 = BLL.GetALLExisting_WorkOrdersInTrip(Convert.ToInt32(TRIP_WORKORDER_ID), ClientId);
                    }
                    else
                    {
                        DT01 = BLL.GetALLExisting_WorkOrdersInTrip(Convert.ToInt32(TRIP_WORKORDER_ID), (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID).ToString());
                    }





                    VTSLRReceiptRecords[] _obj = new VTSLRReceiptRecords[DT01.Rows.Count];
                    for (int j = 0; j < DT01.Rows.Count; j++)
                    {

                        _obj[j] = new VTSLRReceiptRecords();
                        _obj[j].COSIGNMENT_ID = Convert.ToString(DT01.Rows[j]["TRIP_WORKORDER_ID"]);
                        _obj[j].LR_NUMBER = Convert.ToString(DT01.Rows[j]["TRIP_lr_NO"]);
                        _obj[j].LR_DATE = Convert.ToString(DT01.Rows[j]["TRIP_LR_DATE"]);//_obj_smvts_workorderTrips1.TRIP_LR_DATE;//.ToString("MM/dd/yy");
                        _obj[j].TRUCK_NO = Convert.ToString(DT01.Rows[j]["TRIP_VEHICLENO"]);
                        _obj[j].TRANSPORTER_ID = GET_TRANSPORTERCODE(Convert.ToInt32(DT01.Rows[j]["TRIP_WORKORDER_ID"]));
                    }

                    //VTSLRReceipt_OutService _outobj = new VTSLRReceipt_OutService();
                    //NetworkCredential _n = new NetworkCredential();
                    //_n.UserName = System.Configuration.ConfigurationManager.AppSettings["bajajusername"];
                    //_n.Password = System.Configuration.ConfigurationManager.AppSettings["bajajpswd"];
                    //_outobj.Credentials = _n;
                    //_outobj.VTSLRReceipt_Out(_obj);
                    //if (s)
                    //{
                    //    MSG01 = "true";
                    //}
                    //else
                    //{
                    //    MSG01 = "false";
                    //}
                    MSG01 = "true";
                }
            }
            return Json(new { data1 = MSG01 });
        }

        private string GET_TRANSPORTERCODE(int p)
        {
            string TransporterCode = string.Empty;
            System.Data.DataTable dt = BLL.GetTranasporterCode(p);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    TransporterCode = dt.Rows[0][0].ToString();
                }
            }
            return TransporterCode;
        }


        [SessionAuthorize]
        public void GridTrack_CAB()
        {
            checkcategoryid();
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
            DataTable dt = BLL.get_GridTrackDistance_IB(_obj_Smvts_GridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
            List<SMVTS_GRIDTRACK> obj = new List<SMVTS_GRIDTRACK>();
            if (dt.Rows.Count > 0 && dt != null)
            {
                for (int i = 0;
              i < dt.Rows.Count; i++)
                {
                    int spe = 0;
                    if (dt.Rows[i]["SPEED"] == DBNull.Value)
                    {
                        spe = 0;
                    }
                    else
                    {
                        spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                    }
                    obj.Add(new SMVTS_GRIDTRACK
                    {
                        SPEED = spe,
                        IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                        DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                        VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                        DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                        PLACE = dt.Rows[i]["PLACE"].ToString(),
                        DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                        DURATION = dt.Rows[i]["duration"].ToString(),
                        DKM = dt.Rows[i]["distance_day"].ToString(),
                        TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                        LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                        LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                        COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                        DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                        DISTANCE = dt.Rows[i]["DISTANCE_DAY"].ToString(),
                        VEHSTATUS = dt.Rows[i]["IBSTATUS"].ToString(),
                        WORKORDERID = dt.Rows[i]["TRIP_WORKORDER_ID"].ToString(),
                    });
                }
                TempData["GridTrack"] = obj;
            }


        }

        [SessionAuthorize]
        public ActionResult frm_GridTrack_CAB()
        {
            GridTrack_CAB();
            ViewBag.DASHBOARDDATA = TempData["GridTrack"];
            return View();
        }
        [SessionAuthorize]
        public JsonResult _frm_GridTrack_CAB()
        {
            GridTrack_CAB();
            var data = TempData["GridTrack"];
            return Json(new { data = data });
        }



        public JsonResult Get_TRIPRECORDS(string tripid)
        {
            DataTable dt = new DataTable();
            dt = Dal.ExecuteQuery("SELECT TRIP_REPORT_OUTTIME_PORT,TRIP_PLANT_INTIME,TRIP_PLANT_OUTTIME,TRIP_UNLOAD_INTIME,TRIP_UNLOAD_OUTTIME from SMVTS_WORKORDER_TRIPS where TRIP_ID=" + tripid + "");



            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row1;
            List<WORKORDERS> obj = new List<WORKORDERS>();
            string REPORTOUT = "", PLANTIN = "", PLANTOUT = "", UNLOADIN = "", UNLOADOUT = "";
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[0]["TRIP_REPORT_OUTTIME_PORT"].ToString() != "")
                    {
                        REPORTOUT = Convert.ToDateTime(dt.Rows[0]["TRIP_REPORT_OUTTIME_PORT"]).ToString("MM/dd/yyyy HH:mm");
                    }
                    if (dt.Rows[0]["TRIP_PLANT_INTIME"].ToString() != "")
                    {
                        PLANTIN = Convert.ToDateTime(dt.Rows[0]["TRIP_PLANT_INTIME"]).ToString("MM/dd/yyyy HH:mm");
                    }
                    if (dt.Rows[0]["TRIP_PLANT_OUTTIME"].ToString() != "")
                    {
                        PLANTOUT = Convert.ToDateTime(dt.Rows[0]["TRIP_PLANT_OUTTIME"]).ToString("MM/dd/yyyy HH:mm");
                    }
                    if (dt.Rows[0]["TRIP_UNLOAD_INTIME"].ToString() != "")
                    {
                        UNLOADIN = Convert.ToDateTime(dt.Rows[0]["TRIP_UNLOAD_INTIME"]).ToString("MM/dd/yyyy HH:mm");
                    }
                    if (dt.Rows[0]["TRIP_UNLOAD_OUTTIME"].ToString() != "")
                    {
                        UNLOADOUT = Convert.ToDateTime(dt.Rows[0]["TRIP_UNLOAD_OUTTIME"]).ToString("MM/dd/yyyy HH:mm");
                    }
                    obj.Add(new WORKORDERS
                    {

                        REPORTOUT = REPORTOUT,
                        PLANTIN = PLANTIN,
                        PLANTOUT = PLANTOUT,
                        UNLOADIN = UNLOADIN,
                        UNLOADOUT = UNLOADOUT,
                    });

                }
                //foreach (DataRow dr1 in dt.Rows)
                //{
                //    row1 = new Dictionary<string, object>();
                //    foreach (DataColumn col in dt.Columns)
                //    {

                //        row1.Add(col.ColumnName, dr1[col]);
                //    }
                //    rows.Add(row1);
                //}

            }
            //return Json(rows);

            var jsonData = new
            {
                Report = obj
            };
            return Json(jsonData);
        }




        //public JsonResult Trip_Edit(string TRIPID, string ReportOut, string PlantIn, string PlantOut, string UnloadIn, string UnloadOut, string ClientID = null)
        public JsonResult Trip_Edit(string TRIPID, string UnloadIn, string UnloadOut, string ClientID = null)
        {
            string MSG = "false";
            SMVTS_WORKORDER_TRIPS SWT = new SMVTS_WORKORDER_TRIPS();
            SWT.TRIP_ID = Convert.ToInt32(TRIPID);
            if (((SMVTS_USERS)Session["USERINFO"]).USERS_ROLE_ID.ToString() == "2")
            {
                SWT.TRIP_CATEGORY_ID = Convert.ToInt32(ClientID);
            }
            else
            {
                SWT.TRIP_CATEGORY_ID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            }
            SWT.TRIP_REPORT_INTIME_PORT = DateTime.Now;
            //SWT.TRIP_REPORT_OUTTIME_PORT = Convert.ToDateTime(ReportOut);
            //SWT.TRIP_PLANT_INTIME = Convert.ToDateTime(PlantIn);
            //SWT.TRIP_PLANT_OUTTIME = Convert.ToDateTime(PlantOut);
            SWT.TRIP_UNLOAD_INTIME = Convert.ToDateTime(UnloadIn);
            SWT.TRIP_UNLOAD_OUTTIME = Convert.ToDateTime(UnloadOut);
            SWT.TRIP_CALCULATION_TYPE = -1;



            //if (SWT.TRIP_PLANT_OUTTIME < SWT.TRIP_PLANT_INTIME)
            //{
            //    MSG = "Plant Out is before Plant In";

            //}
            if (SWT.TRIP_UNLOAD_OUTTIME < SWT.TRIP_UNLOAD_INTIME)
            {
                MSG = "Trip  end is before Trip Start";

            }
            //if (SWT.TRIP_PLANT_INTIME < SWT.TRIP_REPORT_OUTTIME_PORT)
            //{
            //    MSG = "Plant In is before Report Out";

            //}
            //if (SWT.TRIP_UNLOAD_INTIME < SWT.TRIP_PLANT_OUTTIME)
            //{
            //    MSG = "Trip start is before Plant Out";

            //}
            //if (ReportOut != null
            //    && PlantIn != null && PlantOut != null
            //    && UnloadIn != null && UnloadOut != null)
            //{
            //    SWT.TRIP_STATUS = 3;
            //}
            //else
            //{
            //    SWT.TRIP_STATUS = 2;
            //}

            if (UnloadIn != null && UnloadOut != null)
            {
                SWT.TRIP_STATUS = 3;
            }
            else
            {
                SWT.TRIP_STATUS = 2;
            }


            bool st = false;
            if (MSG == "false")
            {
                st = BLL.UpdateWorkOrderTrip_close(SWT);
                if (st)
                {
                    MSG = "true";
                    Load_CompletedWorkOrdersGrd();
                }
                else
                {

                }
            }



            return Json(new { data = MSG });
        }



        public JsonResult WORKORDERS_VEHICLES(string WORKORDER)
        {
            List<WORKORDERS> obj01 = new List<WORKORDERS>();
            SMVTS_VEHICLES _obj_Smvts_Vehicles = new SMVTS_VEHICLES();
            try
            {
                SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
                DataTable dt01 = BLL.GETVEHILCES_WORKORDER(WORKORDER);

                for (int m = 0; m < dt01.Rows.Count; m++)
                {
                    obj01.Add(new WORKORDERS
                    {

                        TRIP_VEHICLENO = dt01.Rows[m]["TRIP_VEHICLENO"].ToString(),
                        TRIP_DEVICEID = dt01.Rows[m]["TRIP_DEVICEID"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { data = obj01 });
        }
        [SessionAuthorize]
        public ActionResult IB_REPORTS()
        {
            List<TRAILERREPORTS> obj = new List<TRAILERREPORTS>();
            string Report = "TRAILER_REPORT";
            int CATEGID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.GetTrailer_summaryreport(Report, CATEGID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new TRAILERREPORTS
                {
                    CATEGNAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                    TOTALVEHICLES = Convert.ToInt32(dt.Rows[i]["TOTALVEHICLES"]),
                    GPS = Convert.ToInt32(dt.Rows[i]["GPS"]),
                    ATPORT = Convert.ToInt32(dt.Rows[i]["AT PORT"]),
                    WAITINGFORLOAD = Convert.ToInt32(dt.Rows[i]["WAITING FOR LOAD"]),
                    INTRANSIPLANT = Convert.ToInt32(dt.Rows[i]["IN TRANSIT TOWARDS PLANT"]),
                    ATPLANT = Convert.ToInt32(dt.Rows[i]["AT PLANT"]),
                    INTRANSIPORT = Convert.ToInt32(dt.Rows[i]["IN TRANSIT TOWARDS PORT"]),
                    OTHERTRIP = Convert.ToInt32(dt.Rows[i]["OTHER TRIP"]),
                    NODATA = Convert.ToInt32(dt.Rows[i]["NO DATA"]),
                    NOGPSDEVICE = Convert.ToInt32(dt.Rows[i]["NO GPS DEVICE"]),
                });
            }
            ViewBag.TRAILERTRPORT = obj;
            return View();
        }
        public ActionResult Trackingtest()
        {
            return View();
        }
        public class TRAILERREPORTS
        {
            public string CATEGNAME { get; set; }
            public int TOTALVEHICLES { get; set; }

            public int GPS { get; set; }
            public int ATPORT { get; set; }
            public int WAITINGFORLOAD { get; set; }
            public int INTRANSIPLANT { get; set; }
            public int ATPLANT { get; set; }
            public int INTRANSIPORT { get; set; }

            public int OTHERTRIP { get; set; }

            public int NODATA { get; set; }

            public int NOGPSDEVICE { get; set; }
        }
        [SessionAuthorize]
        public ActionResult IB_SUMMARYREPORTS()
        {
            int categid = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            List<SUMMARYREPORT> obj = new List<SUMMARYREPORT>();
            string Report = "SUMMARY_REPORT";
            DataTable dt = BLL.GetTrailer_summaryreport(Report, categid);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SUMMARYREPORT
                {
                    TRIP_WORKORDER_ID = dt.Rows[i]["TRIP_WORKORDER_ID"].ToString(),
                    CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                    TRIPSTART = dt.Rows[i]["TRIP START"].ToString(),
                    GATEIN = dt.Rows[i]["GATE IN"].ToString(),
                    GATEOUT = dt.Rows[i]["GATE OUT"].ToString(),
                    PORTIN = dt.Rows[i]["PORT IN"].ToString(),
                    TRIPEND = dt.Rows[i]["TRIP END"].ToString(),
                    PLANT = dt.Rows[i]["PLANT"].ToString(),
                    COUNTRY = dt.Rows[i]["COUNTRY"].ToString(),
                    CONTAINERS = dt.Rows[i]["CONTAINERS"].ToString(),
                    QUANTITY = dt.Rows[i]["QUANTITY"].ToString(),
                    INDENTCREATION = dt.Rows[i]["INDENT CREATION"].ToString(),
                    LRNO = dt.Rows[i]["LRNO"].ToString(),
                    DOSTATUS = dt.Rows[i]["DOSTATUS"].ToString(),
                    PICKUPTIME = dt.Rows[i]["PICKUPTIME"].ToString(),
                });
            }
            ViewBag.TRAILERTRPORT = obj;
            return View();
        }

        [SessionAuthorize]
        public JsonResult SUMMARYREPORTS_FILTER(string WORKORDERIDS)
        {
            int categid = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            List<SUMMARYREPORT> obj = new List<SUMMARYREPORT>();
            string Report = "SUMMARY_REPORT_FILTER";
            DataTable dt = BLL.summaryreport_filter(Report, WORKORDERIDS, categid);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SUMMARYREPORT
                {
                    TRIP_WORKORDER_ID = dt.Rows[i]["TRIP_WORKORDER_ID"].ToString(),
                    CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                    TRIPSTART = dt.Rows[i]["TRIP START"].ToString(),
                    GATEIN = dt.Rows[i]["GATE IN"].ToString(),
                    GATEOUT = dt.Rows[i]["GATE OUT"].ToString(),
                    PORTIN = dt.Rows[i]["PORT IN"].ToString(),
                    TRIPEND = dt.Rows[i]["TRIP END"].ToString(),
                    PLANT = dt.Rows[i]["PLANT"].ToString(),
                    COUNTRY = dt.Rows[i]["COUNTRY"].ToString(),
                    CONTAINERS = dt.Rows[i]["CONTAINERS"].ToString(),
                    QUANTITY = dt.Rows[i]["QUANTITY"].ToString(),
                    INDENTCREATION = dt.Rows[i]["INDENT CREATION"].ToString(),
                    LRNO = dt.Rows[i]["LRNO"].ToString(),
                    DOSTATUS = dt.Rows[i]["DOSTATUS"].ToString(),
                    PICKUPTIME = dt.Rows[i]["PICKUPTIME"].ToString(),
                });
            }

            return Json(new { data = obj });
        }


        public class SUMMARYREPORT
        {

            public string TRIP_WORKORDER_ID { get; set; }
            public string CATEG_NAME { get; set; }
            public string TRIP_VEHICLENO { get; set; }
            public string COUNTRY { get; set; }
            public string QUANTITY { get; set; }
            public string CONTAINERS { get; set; }
            public string PLANT { get; set; }
            public string INDENTCREATION { get; set; }
            public string DOACKNOWLEDGEMENT { get; set; }
            public string TRIPSTART { get; set; }
            public string PLANTIN { get; set; }
            public string PLANTOUT { get; set; }
            public string PORTIN { get; set; }
            public string TRIPEND { get; set; }
            public string DOSTATUS { get; set; }
            public string LRNO { get; set; }
            public string GATEIN { get; set; }
            public string GATEOUT { get; set; }


            public string PICKUPTIME { get; set; }
        }


        public class SMVTS_GRIDTRACKONE
        {
            public string TRANSPORTER { get; set; }
            public int SPEED { get; set; }
            public DateTime? DATE { get; set; }
            public int DEVICEID { get; set; }
            public string IGNITION { get; set; }
            public string DRIVER_NAME { get; set; }
            public string VEHICLENUMBER { get; set; }
            public string PLACE { get; set; }
            public string DRIVERNUMBER { get; set; }
            public string KILOMETERS { get; set; }
            public string TRIPPLAN { get; set; }
            public string LATITUDE { get; set; }
            public string LONGITUDE { get; set; }
            public string DURATION { get; set; }
            public string COLOR { get; set; }
            public string VEHSTATUS { get; set; }
            public string WORKORDERID { get; set; }
        }

        public class SUMMARYREPORT_EXCEL
        {
            public string TRANSPORTER { get; set; }
            public string TRIP_WORKORDER_ID { get; set; }
            public string TRIP_VEHICLENO { get; set; }
            public string COUNTRY { get; set; }
            public string QUANTITY { get; set; }
            public string CONTAINERS { get; set; }
            public string PLANT { get; set; }
            public string INDENTCREATION { get; set; }
            public string DOACKNOWLEDGEMENT { get; set; }
            public string PICKUPTIME { get; set; }
            public string TRIPSTART { get; set; }
            public string PLANTIN { get; set; }
            public string PLANTOUT { get; set; }
            public string PORTIN { get; set; }
            public string TRIPEND { get; set; }
            public string TAT { get; set; }




        }

        public ActionResult ExcelDownload()
        {
            SMVTS_GRIDTRACK _obj_Smvts_GridTrack = new SMVTS_GRIDTRACK();
            DataTable dt = BLL.get_GridTrackDistance_IB(_obj_Smvts_GridTrack, Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_ID));
            GridView gv = new GridView();

            List<SMVTS_GRIDTRACKONE> obj = new List<SMVTS_GRIDTRACKONE>();
            if (dt.Rows.Count > 0 && dt != null)
            {
                for (int i = 0;
              i < dt.Rows.Count; i++)
                {
                    int spe = 0;
                    if (dt.Rows[i]["SPEED"] == DBNull.Value)
                    {
                        spe = 0;
                    }
                    else
                    {
                        spe = Convert.ToInt32(dt.Rows[i]["SPEED"]);// o = value;
                    }
                    obj.Add(new SMVTS_GRIDTRACKONE
                    {
                        TRANSPORTER = dt.Rows[i]["CATEG_NAME"].ToString(),
                        SPEED = spe,
                        IGNITION = dt.Rows[i]["IGNITION"].ToString(),
                        DRIVER_NAME = dt.Rows[i]["DRIVER_NAME"].ToString(),
                        VEHICLENUMBER = dt.Rows[i]["VEHICLE_NUMBER"].ToString(),
                        DATE = Convert.ToDateTime(dt.Rows[i]["DATE"]),
                        PLACE = dt.Rows[i]["PLACE"].ToString(),
                        DRIVERNUMBER = dt.Rows[i]["driver_number"].ToString(),
                        DURATION = dt.Rows[i]["duration"].ToString(),
                        //  KILOMETERS = dt.Rows[i]["distance_day"].ToString(),
                        TRIPPLAN = dt.Rows[i]["tripplan"].ToString(),
                        LATITUDE = dt.Rows[i]["tripdata_latitude"].ToString(),
                        LONGITUDE = dt.Rows[i]["tripdata_longitude"].ToString(),
                        COLOR = dt.Rows[i]["vehicle_color"].ToString(),
                        DEVICEID = Convert.ToInt32(dt.Rows[i]["DEVICEID"].ToString()),
                        KILOMETERS = dt.Rows[i]["DISTANCE_DAY"].ToString(),
                        VEHSTATUS = dt.Rows[i]["IBSTATUS"].ToString(),
                        WORKORDERID = dt.Rows[i]["TRIP_WORKORDER_ID"].ToString(),
                    });
                }

            }

            gv.DataSource = obj;

            gv.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Excelreport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
            return View();
        }




        public ActionResult ExcelDownloadsummary()
        {

            GridView gv = new GridView();
            List<SUMMARYREPORT_EXCEL> obj = new List<SUMMARYREPORT_EXCEL>();
            string Report = "SUMMARY_REPORT_DETAILS";
            int CATEGID = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.GetTrailer_summaryreport(Report, CATEGID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                obj.Add(new SUMMARYREPORT_EXCEL
                {
                    TRANSPORTER = dt.Rows[i]["CATEG_NAME"].ToString(),
                    TRIP_WORKORDER_ID = dt.Rows[i]["TRIP_WORKORDER_ID"].ToString(),
                    TRIP_VEHICLENO = dt.Rows[i]["TRIP_VEHICLENO"].ToString(),
                    TRIPSTART = dt.Rows[i]["TRIP START"].ToString(),
                    PLANTIN = dt.Rows[i]["GATE IN"].ToString(),
                    PLANTOUT = dt.Rows[i]["GATE OUT"].ToString(),
                    PORTIN = dt.Rows[i]["PORT IN"].ToString(),
                    TRIPEND = dt.Rows[i]["TRIP END"].ToString(),
                    TAT = dt.Rows[i]["hours"].ToString(),
                    PLANT = dt.Rows[i]["PLANT"].ToString(),
                    COUNTRY = dt.Rows[i]["COUNTRY"].ToString(),
                    CONTAINERS = dt.Rows[i]["CONTAINERS"].ToString(),
                    QUANTITY = dt.Rows[i]["QUANTITY"].ToString(),
                    INDENTCREATION = dt.Rows[i]["INDENT CREATION"].ToString(),
                    // LRNO = dt.Rows[i]["LRNO"].ToString(),
                    DOACKNOWLEDGEMENT = dt.Rows[i]["DOSTATUS"].ToString(),
                    PICKUPTIME = dt.Rows[i]["PICKUPTIME"].ToString(),
                });
            }

            gv.DataSource = obj;

            gv.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Excelreport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
            return View();
        }

        [SessionAuthorize]
        public ActionResult DO_ACKNOWLEDGEMENT()
        {
            return View();
        }




        public ActionResult Smarttriplogin()
        {
            string UserName = Session["R_username"].ToString();
            string Password = Session["R_Password"].ToString();
            string Companyname = Session["R_CompanyName"].ToString();

            return RedirectToAction("http://localhost:52832/Login/Index?Username=" + UserName + "&Password=" + Password + "&Organization=" + Companyname + "");
        }

    }
}