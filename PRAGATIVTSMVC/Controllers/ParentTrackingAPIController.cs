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
    public class ParentTrackingAPIController : ApiController
    {
        // GET: ParentTrackingAPI


        [System.Web.Http.HttpGet]

        public FetchMobileNlo GetParentMobileNo(string MobileNo)
        {

            FetchMobileNlo obj = new FetchMobileNlo();
            DataTable dt = BLL.GetStudentParentsMobile(MobileNo);
            if (dt.Rows[0][0].ToString() != "0")
            {
                obj.MobileNo = true;

            }

            else
            {
                obj.MobileNo = false;
            }
            return obj;


        }


        // GET: ParentRouteTracking    


        [HttpGet]
        [Route("api/ParentTrackingAPI/GetParentMobileNoForVehicle/{MobileNo}")]
        public HttpResponseMessage GetParentMobileNoForVehicle(string MobileNo)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<FetchVehicleDetails> obj = new List<FetchVehicleDetails>();
            DataTable dt = BLL.FetchVehicleDetailsByPhone(MobileNo);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new FetchVehicleDetails
                    {
                        StudentName = dt.Rows[i]["Student_Name"].ToString(),
                        VehicelNo = dt.Rows[i]["Scheduler_VehicleNo"].ToString(),
                        Shift_type = dt.Rows[i]["Shift_type"].ToString(),
                        Route_Name = dt.Rows[i]["Routes_Name"].ToString(),
                        RouteNo = dt.Rows[i]["Routes_Code"].ToString(),
                        Latitude = dt.Rows[i]["Latitude"].ToString(),
                        Longitude = dt.Rows[i]["Longitude"].ToString(),
                        Place = dt.Rows[i]["Place"].ToString(),
                        Speed = dt.Rows[i]["speed"].ToString(),
                        Datetime = dt.Rows[i]["Scheduler_StartDate"].ToString(),
                        Start_Date = dt.Rows[i]["Scheduler_StartDate"].ToString(),
                        End_Date = dt.Rows[i]["Scheduler_EndDate"].ToString(),
                        DriverMobile = dt.Rows[i]["DriverMobile"].ToString(),
                        ManagerMobile = dt.Rows[i]["ManagerMobile"].ToString(),
                    });
                }
            }
            else
            {

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(obj), Encoding.UTF8, "application/json");
            return response;

        }

        [HttpGet]
        [Route("api/ParentTrackingAPI/GetParentMobileNoTripData/{MobileNo}/{VehicleNo}")]
        public HttpResponseMessage GetParentMobileNoTripData(string MobileNo, string VehicleNo)
        {
             
            string res1 = null;
            HttpResponseMessage response = new HttpResponseMessage();
            List<FetchVehicleTripDataDetails> obj = new List<FetchVehicleTripDataDetails>();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();


            DataTable dt = BLL.FetchVehicleTripDataDetails(MobileNo, VehicleNo);
            if (dt.Rows.Count > 0)
            {
                obj.Add(new FetchVehicleTripDataDetails
                {

                    VehicelNo = dt.Rows[0]["Scheduler_VehicleNo"].ToString(),
                    Latitude = dt.Rows[0]["Latitude"].ToString(),
                    Longitude = dt.Rows[0]["Longitude"].ToString(),
                    Datetime= dt.Rows[0]["TRIPDATE"].ToString(),
                    Place = dt.Rows[0]["Place"].ToString(),
                    Speed = dt.Rows[0]["Speed"].ToString(),
                    Direction = dt.Rows[0]["Direction"].ToString(),
                });
            }
            else
            {

            }

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(obj), Encoding.UTF8, "application/json");

            return response;
        }



        [HttpGet]

        [Route("api/ParentTrackingAPI/GetParentTicketingStatus/{MobileNo}")]
        public HttpResponseMessage GetParentTicketingStatus(string MobileNo)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<PRAGATIVTSMVC.Models.ParentTicketing1> obj = new List<PRAGATIVTSMVC.Models.ParentTicketing1>();
            DataTable dt = BLL.GetParentTicketStatus(MobileNo);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PRAGATIVTSMVC.Models.ParentTicketing1
                    {
                        Ticket_Id = dt.Rows[i]["TicketID"].ToString(),
                        TicketMobileNo = dt.Rows[i]["TicketMobileNo"].ToString(),
                        Ticket_StudentId = dt.Rows[i]["Ticket_StudentId"].ToString(),
                        Ticket_Desc = dt.Rows[i]["Ticket_Desc"].ToString(),
                        Ticket_Status = dt.Rows[i]["Ticket_Status"].ToString(),
                        Ticket_CreatedBy = dt.Rows[i]["Ticket_CreatedBy"].ToString(),
                        Ticket_CreatedDate = dt.Rows[i]["Ticket_CreatedDate"].ToString(),
                        Ticket_ClosedDate = dt.Rows[i]["Ticket_ClosedDate"].ToString(),
                        Ticket_Remarks = dt.Rows[i]["Ticket_Remarks"].ToString(),

                    });
                }
            }
            else
            {

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(obj), Encoding.UTF8, "application/json");
            return response;

        }

        public class FetchMobileNlo
        {

            public bool MobileNo { get; set; }


        }

        public class FetchVehicleDetails
        {

            public string VehicelNo { get; set; }
            public string RouteNo { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Place { get; set; }
            public string Datetime { get; set; }
            public string Speed { get; set; }

            public string StudentName { get; set; }

            public string Shift_type { get; set; }

            public string Route_Name { get; set; }

            public string Start_Date { get; set; }

            public string End_Date { get; set; }

            public string DriverMobile { get; set; }

            public string ManagerMobile { get; set; }
        }


        public class FetchVehicleTripDataDetails
        {

            public string VehicelNo { get; set; }

            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Place { get; set; }
            public string Datetime { get; set; }
            public string Speed { get; set; }

            public string Direction { get; set; }


        }
    }

}





