using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
namespace PRAGATIVTSMVC.xml
{
    /// <summary>
    /// Summary description for XSER
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class XSER : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string HelloWorld()
        {
           

          return "HELLO WORLD";
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string categories()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            DataTable dt = BLL.get_Clients();
            List<categories> OBJ = new List<categories>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OBJ.Add(new categories
                {
                    CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString()

                });
            }

            return serializer.Serialize(OBJ);
        }
    }

    public class categories
    {
        public string CATEG_NAME { get; set; }
    }
}
