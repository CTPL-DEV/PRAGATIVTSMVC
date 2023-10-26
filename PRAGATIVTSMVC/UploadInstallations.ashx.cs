using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using System.Data;
using PRAGATIVTSMVC.ExcelExtensions;
using System.Configuration;
namespace PRAGATIVTSMVC
{
    /// <summary>
    /// Summary description for UploadInstallations
    /// </summary>
    public class UploadInstallations : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string result = "";
            HttpFileCollection files = context.Request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                try
                {


                    var excel = new ExcelPackage(file.InputStream);
                    DataTable dt = new DataTable();
                    dt = excel.ToDataTable();

                    if (dt.Rows.Count > 0)
                    {
                        int j = 0;
                        for (; j < dt.Rows.Count; j++)
                        {
                            SMVTS_ORDERS obj_orders = new SMVTS_ORDERS();
                            // int packageid = Convert.ToInt32(PackageId);
                            // DataTable dt_package = BLL.Get_PackagesBy_Id(packageid);
                            DateTime date = DateTime.Now;
                            obj_orders.ORDER_NAME = dt.Rows[j]["CustomerName"].ToString() + "_" + date.Day + date.Month + date.Year + "_" + (j + 1);
                            obj_orders.ORDER_PACKAGE = "Basic-Anual";
                            obj_orders.ORDER_PRICE = 1000;
                            obj_orders.ORDER_STARTDATE = Convert.ToDateTime(dt.Rows[j]["StartDate"]);
                            obj_orders.ORDER_EXPDATE = Convert.ToDateTime(dt.Rows[j]["ExpDate"]);
                            obj_orders.ORDER_CREATEDATE = DateTime.Now;

                            //bool status = BLL.SaveOrder(obj);

                            string db_name = "Aor2T0SveXN3Qx99ow3pJdxI6ruR88McJyyA+m2RzQ7GhTWCzxCZGIf5D1n4CCQUJxaM2lG1Dw54Ew7+Lt094KFUqE9WKmftI15Bs/dDX74J5+Uak0lGlaJEX9NVCHgn";
                            int Cust_Id = 0;
                            int dealerId = Convert.ToInt32(dt.Rows[j]["Dealerid"]);

                            //check for Dealer
                            //string abc = "";
                            //abc = "SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + DealerName + "' ";
                            //DataTable dt_categ = new DataTable();
                            //dt_categ = Dal.ExecuteQuery_Prod_for_Test(abc);
                            // dealerId = Convert.ToInt32(dt.Rows[j]["DealerId"].ToString());

                            //check customer 
                            DataTable dt_customer = BLL.Get_Customer(dt.Rows[j]["CustomerName"].ToString(), dealerId);
                            if (dt_customer.Rows.Count > 0)
                            {
                                Cust_Id = Convert.ToInt32(dt_customer.Rows[0]["CATEG_ID"].ToString());

                            }

                            else
                            {


                                //Insert new Client
                                SMVTS_CATEGORIES obj_cust = new SMVTS_CATEGORIES();
                                obj_cust.CATEG_CATETYPE_ID = 3;
                                obj_cust.CATEG_ADDRESS = "";
                                obj_cust.CATEG_CITY_ID = 642;
                                obj_cust.CATEG_CONTACTPERSON = "Prashanth";
                                obj_cust.CATEG_COUNTRY_ID = 1;
                                obj_cust.CATEG_DBNAME = db_name;
                                obj_cust.CATEG_DESC = dt.Rows[j]["CustomerName"].ToString();
                                obj_cust.CATEG_EMAILID = dt.Rows[j]["UserName"].ToString();
                                obj_cust.CATEG_MOBILENUMBER = dt.Rows[j]["CustomerMobileNumber"].ToString();
                                obj_cust.CATEG_NAME = dt.Rows[j]["CustomerName"].ToString();
                                obj_cust.CATEG_NOOFUSERS = 10;

                                //string abc = "";
                                //abc = "SELECT CATEG_ID FROM SMVTS_CATEGORIES(NOLOCK) WHERE CATEG_NAME='" + DealerName + "' ";
                                //DataTable dt_categ = new DataTable();
                                //dt_categ = Dal.ExecuteQuery_Prod(abc);

                                obj_cust.CATEG_PARENT_ID = Convert.ToInt32(dt.Rows[j]["Dealerid"]);
                                obj_cust.CATEG_PRODNAME = "SMVTS_PROD01";
                                obj_cust.CATEG_STATE_ID = 34;
                                obj_cust.CATEG_STATUS = "1";
                                //    obj_cust.CATEG_CITY_ID = 368;
                                obj_cust.CATEG_UOMSPEED = "Km/Hr";
                                obj_cust.CATEG_UOMVOLUME = "Litre";
                                obj_cust.CATEG_WEBSITENAME = "live.eagletracks.com";
                                obj_cust.CATEG_ZIPCODE = "500094";
                                obj_cust.CATEG_VALID_TO = DateTime.Now;
                                obj_cust.Logo_Url = "/IMAGES/Logos/Tranopro.png";
                                // obj_cust.MIS_ID= Convert.ToInt32(dt.Rows[j]["MISDealerID"]);

                                string password = BLL.Encrypt(dt.Rows[j]["Password"].ToString());

                                Cust_Id = BLL.UploadCustomers(obj_cust, "", dt.Rows[j]["DealerName"].ToString(), "", "", 3, password);

                                //if(dt_cust.Rows.Count>0)
                                //{
                                //    Cust_Id = Convert.ToInt32(dt_cust.Rows[0][0].ToString());
                                //}
                            }

                            obj_orders.ORDER_CATEGORYID = Convert.ToInt32(Cust_Id);
                            DataTable dt_order = BLL.RenewOrder(obj_orders);
                            int orderid = Convert.ToInt32(dt_order.Rows[0][0]);
                            int device_ID = 0;
                            if (dt_order.Rows.Count > 0)
                            {
                                SMVTS_DEVICES _obj_Smvts_Devices = new SMVTS_DEVICES();
                                DataTable dt_lastrec = new DataTable();
                                _obj_Smvts_Devices.OPERATION = operation.Insert;
                                dt_lastrec = BLL.get_Devices(_obj_Smvts_Devices);

                                if (dt_lastrec.Rows.Count > 0)
                                {
                                    if (dt_lastrec.Rows[0][0].ToString() != "")
                                    {
                                        device_ID = Convert.ToInt32(dt_lastrec.Rows[0][0]) + 1;
                                    }
                                    else
                                    {
                                        device_ID = 1;
                                    }
                                }


                                // check in SMVTS_TT_DEVIDS

                                //bool status1 = BLL.Insert_Into_tt_devids(device_ID, dt.Rows[j]["IMEI"].ToString());

                                //   //check in Sims Table
                                //   bool status3 = BLL.Insert_NEW_Sims(dt.Rows[j]["SimNumber"].ToString(), Cust_Id, db_name, dt.Rows[j]["CustomerName"].ToString());

                                //   //check in Devices table
                                //   bool status2 = BLL.Insert_Into_Devices(device_ID, dt.Rows[j]["IMEI"].ToString(), Cust_Id, dt.Rows[j]["SimNumber"].ToString(), db_name, dt.Rows[j]["CustomerName"].ToString(), orderid, dt.Rows[j]["DeviceType"].ToString());

                                //   //Insert Vehicles 
                                //   bool veh_status = BLL.Insert_Vehicles(dt.Rows[j]["VehRegNumber"].ToString(), Cust_Id, device_ID, db_name, dt.Rows[j]["CustomerName"].ToString());


                                bool status_device = BLL.DeviceInstallation(Cust_Id.ToString(), device_ID, dt.Rows[j]["IMEI"].ToString(), dt.Rows[j]["DeviceType"].ToString(), dt.Rows[j]["SimNumber"].ToString(), dt.Rows[j]["VehRegNumber"].ToString(), "3", "Cargo", "2710", orderid.ToString(), dt.Rows[j]["CustomerName"].ToString(), dt.Rows[j]["Network"].ToString(), dt.Rows[j]["Vehimagemodel"].ToString());
                                //update wallet
                                bool status2 = BLL.UpdateDeviceWallet(Cust_Id.ToString(), dt.Rows[j]["Dealerid"].ToString());
                                //update Orders
                                bool status = Dal.ExecuteNonQueryPROD("update SMVTS_ORDERS set DEVICE_ID=" + device_ID + " where ORDER_ID=" + orderid + "");


                            }
                        }
                        if (j == dt.Rows.Count)
                        {
                            result = "Success";
                        }
                        else
                        {
                            result = "Failed_at" + j;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}