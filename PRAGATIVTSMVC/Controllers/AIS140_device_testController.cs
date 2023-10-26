using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
namespace PRAGATIVTSMVC.Controllers
{
    public class AIS140_device_testController : ApiController
    {


        [HttpGet]
        [Route("api/AIS140_Device_Test/{result}/{userid}")]
        public dynamic AIS140_Device_Test(string result,string userid)
        {
            bool status = BLL.insert_device_test(result);
            return status;
        }
        [HttpGet]
        [Route("api/AIS140_Device_Test_user_login/{username}/{password}")]
        public dynamic AIS140_Device_Test_Login(string username,string password)
        {
            DataTable dt = BLL.get_UserLogin_for_DeviceTest(username, password);

            return dt;
        }
    }
}
