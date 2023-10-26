using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
namespace PRAGATIVTSMVC.Controllers
{
    public class GetMonthlyKMSReportController : ApiController
    {
        [Route("api/GetMonthlyReports/{UserId}/{CategId}")]
        public DataTable getMonthlyReports(int UserId,int CategId)
        {
            DataTable dt = new DataTable();
          //  int userid = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_ID);
           // int categid = Convert.ToInt32(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
            DateTime date = DateTime.Now;
            string startdate = date.ToString("MM-dd-yyyy");
            dt = BLL.Get_Monthly_Report(UserId, CategId, startdate);
            return dt;
        }
    }
}
