using PRAGATIVTS_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace PRAGATIVTSMVC.Services
{
    /// <summary>
    /// Summary description for FetchLiveData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FetchLiveData : System.Web.Services.WebService
    {

        [WebMethod]
        public XmlDocument FetchLive_Data(string TOKEN)
        {
            XmlDocument doc = new XmlDocument();
            DataTable dt = new DataTable();
            string API_Livedata = "EXEC SMVTS_GENERIC_API @TOKEN='" + TOKEN + "'";


            dt = ExecuteQueryLiveData(API_Livedata);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    //  dt_Livedate = ExecuteQueryLiveData(API_Livedata);

                    string str2 = "";
                    string str = "";

                    if (dt != null)
                    {
                        str = "<VehicleLatestInfo>";

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            str = str + "<Node>";
                            // this is in case if the client wants REC1, REC2...str  =  str + "<REC" + (i+1) +"><VEHICLENO>" + dt.Rows[i][2] + "</VEHICLENO><SPEED>" + dt.Rows[i][1] + "</SPEED><STATUS>" + dt.Rows[i][4] + "</STATUS><DATEOFLASTREC>" + dt.Rows[i][5] + "</DATEOFLASTREC><COLORSTATUS>" + dt.Rows[i][11] + "</COLORSTATUS><ODOMETER>" + dt.Rows[i][6] + "</ODOMETER></REC" + (i+1) + ">";

                            str = str + "<VehicleNo>" + dt.Rows[i][0] + "</VehicleNo><Location>" + dt.Rows[i][1] + "</Location><DateTime>" + Convert.ToDateTime(dt.Rows[i][2]).ToString() + "</DateTime><Temperature>" + dt.Rows[i][3] + "</Temperature><IgnitionStatus>" + dt.Rows[i][4] + "</IgnitionStatus><Lat>" + dt.Rows[i][5] + "</Lat><Long>" + dt.Rows[i][6] + "</Long><Distance>" + dt.Rows[i][7] + "</Distance>";
                            // str ="<REC" + (i + 1) + "><VEHICLENO>" + dt.Rows[i][2] + "</VEHICLENO><SPEED>" + dt.Rows[i][1] + "</SPEED><STATUS>" + dt.Rows[i][4] + "</STATUS><DATEOFLASTREC>" + dt.Rows[i][5] + "</DATEOFLASTREC><COLORSTATUS>" + dt.Rows[i][11] + "</COLORSTATUS><ODOMETER>" + dt.Rows[i][6] + "</ODOMETER></REC" + (i + 1) + ">";
                            str = str + "</Node>";
                        }
                        str = str + "</VehicleLatestInfo>";
                        str2 = str.Replace("&", "&amp;");
                        doc.LoadXml(str2);
                    }

                }
            }
            return doc;
        }

        public static DataTable ExecuteQueryLiveData(string Query)
        {
            DataTable dt = new DataTable();
            try
            {
                string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());

                dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }
    }
}
