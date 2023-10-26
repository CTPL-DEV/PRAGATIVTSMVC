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

namespace PRAGATIVTSMVC.Controllers
{
    public class AlertsController : Controller
    {
        // GET: Alerts
        public ActionResult Index()
        {
            return View();
        }

        //Ajith 13-09-2020
        public JsonResult getEvents()
        {
            SMVTS_CATEGORIES obj = new SMVTS_CATEGORIES();

            DataTable dt = new DataTable();
            List<ctpl_events> evt = new List<ctpl_events>();

            dt = BLL.get_events();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    evt.Add(new ctpl_events
                    {
                        EVENT_ID = Convert.ToInt32(dt.Rows[i]["EVENT_ID"]),
                        EVENT_NAME = dt.Rows[i]["EVENT_NAME"].ToString()
                    });
                }
            }


            return Json(new { data = evt });
        }
        //[Route("/SaveAlerts")]
        [HttpPost]
        public JsonResult SaveAlertConfiguration(int EventId = 0,bool AppAlert=false,bool SmsAlert=false,bool EmailAlert=false)
        {
            ctpl_events_configuration obj = new ctpl_events_configuration();
            obj.CONFIG_APPALERT = AppAlert;
            obj.CONFIG_EMAILALERT = EmailAlert;
            obj.CONFIG_EVENTID = EventId;
            obj.CONFIG_SMSALERT = SmsAlert;
            obj.CONFIG_CATEG_ID= ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            DataTable dt=BLL.getEventById(EventId, ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            if(dt.Rows.Count>0)
            {
                return Json(new { data = false, message = "Event Already Exists" });
            }
            else
            {
                bool status = BLL.insert_events(obj);
                return Json(new { data = status });
            }
           
           
        }

        public JsonResult getEventConfiguration()
        { 
            
            List<ctpl_events_configuration> obj = new List<ctpl_events_configuration>();
            DataTable dt = BLL.getEventConfiguration(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            if(dt.Rows.Count>0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    obj.Add(new ctpl_events_configuration
                    {
                        CONFIG_ID= Convert.ToInt32(dt.Rows[i]["CONFIG_ID"]),
                        CONFIG_EVENTID = Convert.ToInt32(dt.Rows[i]["CONFIG_EVENTID"]),
                        EventName = dt.Rows[i]["EventName"].ToString(),
                        AppAlert= dt.Rows[i]["AppAlert"].ToString(),
                        SmsAlert= dt.Rows[i]["SmsAlert"].ToString(),
                        EmailAlert = dt.Rows[i]["EmailAlert"].ToString(),
                        CONFIG_CATEG_ID= Convert.ToInt32(dt.Rows[i]["CONFIG_CATEG_ID"]),
                        CONFIG_CREATED_DATE =Convert.ToDateTime(dt.Rows[i]["CONFIG_CREATED_DATE"].ToString())
                    }); 
                    
                }
                
            }
            return Json(new { data = obj });
        }
        public JsonResult EditEvent(int evnet_id,int CATEG_ID)
        {
            List<ctpl_events_configuration> obj = new List<ctpl_events_configuration>();
            DataTable dt = BLL.getEventById(evnet_id, CATEG_ID);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new ctpl_events_configuration
                    {
                        CONFIG_EVENTID = Convert.ToInt32(dt.Rows[i]["CONFIG_EVENTID"]),
                        CONFIG_APPALERT =Convert.ToBoolean(dt.Rows[i]["CONFIG_APPALERT"]),
                        CONFIG_EMAILALERT =Convert.ToBoolean(dt.Rows[i]["CONFIG_EMAILALERT"]),
                        CONFIG_SMSALERT =Convert.ToBoolean(dt.Rows[i]["CONFIG_SMSALERT"]),
                       
                    });

                }

            }
            return Json(new { data = obj });
        }
        public JsonResult UpdateAlertConfiguration(int EventId = 0, bool AppAlert = false, bool SmsAlert = false, bool EmailAlert = false)
        {
            ctpl_events_configuration obj = new ctpl_events_configuration();
            obj.CONFIG_APPALERT = AppAlert;
            obj.CONFIG_EMAILALERT = EmailAlert;
            obj.CONFIG_EVENTID = EventId;
            obj.CONFIG_SMSALERT = SmsAlert;

            bool status = BLL.update_events(obj);
            return Json(new { data = status });
        }
        public JsonResult LoadVehicles()
        {
            List<SMVTS_DEVICES> obj = new List<SMVTS_DEVICES>();
            string CATEG_ID= (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID).ToString();
            DataTable dt = BLL.getVehicles(CATEG_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new SMVTS_DEVICES
                {

                    DEVICE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLES_DEVICE_ID"]),
                    DEVICE_NAME = dt.Rows[i]["VEHICLES_REGNUMBER"].ToString(),
                    DEVICE_SERIALNUMBER= dt.Rows[i]["DEVICE_SERIALNUMBER"].ToString()

                });
            }
            return Json(new { data = obj });
        }
        public JsonResult AssignAlertVehicles(int configId,string VehicleIds,string MobileNumber,string ToEmail,string CCMail,string BccEmail,int LandmarkId)
        {
            string vehcles_ids = "", device_ids = "";bool status = false;
            string[] arrayVehicles = JsonConvert.DeserializeObject<string[]>(VehicleIds);
            for(int i=0;i< arrayVehicles.Length;i++)
            {
                if (vehcles_ids=="")
                {
                    vehcles_ids = arrayVehicles[i];
                }
                else
                {
                    vehcles_ids += "," + arrayVehicles[i];
                }
            }  
                  
            ctpl_eventconfig_details obj = new ctpl_eventconfig_details();
            obj.DETAILS_BCCMAIL = BccEmail;
            obj.DETAILS_CATEGID = (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            obj.DETAILS_VEHICLEID = vehcles_ids;
            obj.DETAILS_CCMAIL = CCMail;
            obj.DETAILS_CONFIGID = configId;
            obj.DETAILS_MOBILENO = MobileNumber;
            obj.DETAILS_TOMAIL = ToEmail;
            obj.DETAILS_LANDMARKID = LandmarkId;
            DataTable dt_config = BLL.getConfigDetails(configId, (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID));
            if(dt_config.Rows.Count > 0)
            {
                if(Convert.ToInt32(dt_config.Rows[0]["CONFIG_EVENTID"]) == 4)
                {
                    DataTable dt_geofence = BLL.get_geofence_landmark((((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID), configId, LandmarkId);
                    if (dt_geofence.Rows.Count > 0)
                    {
                        status = BLL.update_geofence_detials(obj);
                    }
                    else
                    {
                        status = BLL.insert_config_detials(obj);
                    }
                }
                else
                {
                    status = BLL.update_config_detials(obj);
                }
               
            }
            else
            { 
               
                    status = BLL.insert_config_detials(obj);
                
            }
            
            

            return Json(new { data = status ,data2= device_ids });
        }
        public JsonResult getConfigDetails(int config_id,int event_Id)
        {
            int categ_id = (((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DataTable dt = BLL.getConfigDetails(config_id, categ_id);
            List<ctpl_eventconfig_details> obj = new List<ctpl_eventconfig_details>();
            List<SMVTS_LANDMARKS> obj_landmarks = new List<SMVTS_LANDMARKS>();
            if (dt.Rows.Count>0)
            {

                for(int i=0;i<dt.Rows.Count;i++)
                {
                    obj.Add(new ctpl_eventconfig_details
                    {
                        DETAILS_BCCMAIL = dt.Rows[i]["DETAILS_BCCMAIL"].ToString(),
                        DETAILS_CCMAIL = dt.Rows[i]["DETAILS_CCMAIL"].ToString(),
                        DETAILS_CONFIGID = Convert.ToInt32(dt.Rows[i]["DETAILS_CONFIGID"]),
                        DETAILS_LANDMARKID=Convert.ToInt32(dt.Rows[i]["DETAILS_LANDMARKID"]),
                        DETAILS_TOMAIL = dt.Rows[i]["DETAILS_TOMAIL"].ToString(),
                        DETAILS_MOBILENO= dt.Rows[i]["DETAILS_MOBILENO"].ToString(),
                        DETAILS_VEHICLEID= dt.Rows[i]["DETAILS_VEHICLEID"].ToString(),
                        LANDMARK_ADDRESS= dt.Rows[i]["LANDMARKS_ADDRESS"].ToString(),
                        DETAILS_ID=Convert.ToInt32(dt.Rows[i]["DETAILS_ID"])
                    }); 
                }
                
            }
            if(event_Id==4)
            {
                 DataTable dt_geofence = BLL.getGeofenceByCateg(categ_id);

                    if (dt_geofence.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt_geofence.Rows.Count; j++)
                        {
                            obj_landmarks.Add(new SMVTS_LANDMARKS
                            {
                                LANDMARKS_ADDRESS = dt_geofence.Rows[j]["LANDMARKS_ADDRESS"].ToString(),
                                LANDMARK_GEOFENCE_ID =Convert.ToInt32(dt_geofence.Rows[j]["LANDMARK_GEOFENCE_ID"]),
                                LANDMARKS_ID = Convert.ToInt32(dt_geofence.Rows[j]["LANDMARKS_ID"])
                            });
                        }
                    }
               
            }
            return Json(new { data = obj, data2= obj_landmarks });
        }
        public JsonResult getGeofecneData(int DetailsId)
        {
            DataTable dt = BLL.getGeofenceDetails(DetailsId);
            List<ctpl_eventconfig_details> obj = new List<ctpl_eventconfig_details>();
            if(dt.Rows.Count>0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    obj.Add(new ctpl_eventconfig_details
                    {
                        DETAILS_BCCMAIL = dt.Rows[i]["DETAILS_BCCMAIL"].ToString(),
                        DETAILS_CCMAIL = dt.Rows[i]["DETAILS_CCMAIL"].ToString(),
                        DETAILS_CONFIGID = Convert.ToInt32(dt.Rows[i]["DETAILS_CONFIGID"]),
                        DETAILS_LANDMARKID = Convert.ToInt32(dt.Rows[i]["DETAILS_LANDMARKID"]),
                        GEOFENCE_ID = Convert.ToInt32(dt.Rows[i]["LANDMARK_GEOFENCE_ID"]),
                        DETAILS_TOMAIL = dt.Rows[i]["DETAILS_TOMAIL"].ToString(),
                        DETAILS_MOBILENO = dt.Rows[i]["DETAILS_MOBILENO"].ToString(),
                        DETAILS_VEHICLEID = dt.Rows[i]["DETAILS_VEHICLEID"].ToString(),
                        DETAILS_ID = Convert.ToInt32(dt.Rows[i]["DETAILS_ID"])
                    });
                }
            }
            return Json(new { data = obj });
        }
    }
}