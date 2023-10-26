using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRAGATIVTSMVC.Models
{
    public class ScheduleRoutes
    {
        public string RouteName { get; set; }
        public string RouteCode { get; set; }
        public string RouteStartDate { get; set; }
        public string RouteEndDate { get; set; }
        public string RouteCategoryId { get; set; }
        public string RouteStatus { get; set; }
        public string RouteCreatedBy { get; set; }
        public string RouteModifiedBy { get; set; }

    }
}