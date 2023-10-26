using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using System.Web.Services.Protocols;

namespace PRAGATIVTSMVC.Services
{
    /// <summary>
    /// Summary description for DeviceInstallation140
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeviceInstallation140 : System.Web.Services.WebService
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = null;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        response resp = null;

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod(Description = "For Install Device")]
        public void  DeviceInstallaton(string DistributorName, string DealerName, string CustomerName, string Customer_Email, string Customer_MobileNumber, int CountryId, int StateId, int CityId, string pincode, string Address, int PackageId, string Vehicle_No, string Device_IMEI_NO, string sim1,string operator1,string sim2,string operator2, int DistributorId, int DealerId,int Customer_mis_id,string device_type)
        {

            try
            {
                serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                resp = new response();


                SMVTS_ORDERS obj_orders = new SMVTS_ORDERS();
                int packageid = Convert.ToInt32(PackageId);
                DataTable dt_package = BLL.Get_PackagesBy_Id_For_API(packageid);
                DateTime date = DateTime.Now;
                obj_orders.ORDER_NAME = CustomerName + DateTime.Now.ToShortDateString();
                obj_orders.ORDER_PACKAGE = dt_package.Rows[0]["PACKAGE_NAME"].ToString();
                obj_orders.ORDER_PRICE = Convert.ToDecimal(dt_package.Rows[0]["PACKAGE_PRICE"]) + Convert.ToDecimal(dt_package.Rows[0]["GST"]);
                obj_orders.ORDER_STARTDATE = DateTime.Now;
                obj_orders.ORDER_EXPDATE = DateTime.Now.AddDays(Convert.ToInt32(dt_package.Rows[0]["NUM_OF_DAYS"]));
                obj_orders.ORDER_CREATEDATE = DateTime.Now;

                // bool status = BLL.SaveOrder(obj);

                string db_name = "Aor2T0SveXPKBbMcC7tSjrv+vNImP9nNMtkDjeimINYnX23VBtrCYEG2tZkQKsyW/89syeJQD0rznBz47OI45T+I00H+mKDZtnMOdeZlpCzckukpOKqMbXjlktkV49jJ";
                int Cust_Id = 0;
                // int dealerId = 0;

                //check for Dealer
                string abc = "";
                abc = "SELECT * FROM SMVTS_CATEGORIES(NOLOCK) WHERE MIS_ID=" + DealerId + " AND CATEG_CATETYPE_ID=6";
                DataTable dt_dealer = new DataTable();
                dt_dealer = Dal.ExecuteQuery_Prod(abc);
                if (dt_dealer.Rows.Count > 0)
                {
                    string dealer_Name = dt_dealer.Rows[0]["CATEG_NAME"].ToString();
                    int dealer_id = Convert.ToInt32(dt_dealer.Rows[0]["CATEG_ID"]);
                    //check customer 
                    DataTable dt_customer = BLL.Get_Customer_Api(CustomerName, dealer_id,Customer_mis_id);
                    if (dt_customer.Rows.Count > 0)
                    {
                        Cust_Id = Convert.ToInt32(dt_customer.Rows[0]["CATEG_ID"].ToString());

                    }

                    else
                    {


                        //Insert new Client
                        SMVTS_CATEGORIES obj_cust = new SMVTS_CATEGORIES();
                        obj_cust.CATEG_CATETYPE_ID = 3;
                        obj_cust.CATEG_ADDRESS = Address;
                        obj_cust.CATEG_CITY_ID = CityId;
                        obj_cust.CATEG_CONTACTPERSON = "Prashanth";
                        obj_cust.CATEG_COUNTRY_ID = CountryId;
                        obj_cust.CATEG_DBNAME = db_name;
                        obj_cust.CATEG_DESC = CustomerName;
                        obj_cust.CATEG_EMAILID = Customer_Email;
                        obj_cust.CATEG_MOBILENUMBER = Customer_MobileNumber;
                        obj_cust.CATEG_NAME = CustomerName;
                        obj_cust.CATEG_NOOFUSERS = 10;

                        obj_cust.CATEG_PARENT_ID = dealer_id;
                        obj_cust.CATEG_PRODNAME = "SMVTS_PROD01";
                        obj_cust.CATEG_STATE_ID = StateId;
                        obj_cust.CATEG_STATUS = "1";
                        obj_cust.CATEG_UOMSPEED = "Km/Hr";
                        obj_cust.CATEG_UOMVOLUME = "Litre";
                        obj_cust.CATEG_WEBSITENAME = "www.tranopro.com";
                        obj_cust.CATEG_ZIPCODE = pincode;
                        obj_cust.MIS_ID = Customer_mis_id;
                        //  DataTable cust_status = BLL.insert_new_customer(obj_cust, db_name, PackageId, obj_orders.ORDER_EXPDATE);
                        //  Cust_Id = Convert.ToInt32(cust_status.Rows[0]["CustID"]);
                        string password = BLL.Encrypt(Customer_MobileNumber.ToString());

                        Cust_Id = BLL.UploadCustomers(obj_cust, "", dealer_Name, "", "", 3, password);
                        //Insert Client Role
                        // DataTable dt_package = BLL.Get_FormIDsByCategory(Cust_Id);


                        //SMVTS_ROLES obj_role = new SMVTS_ROLES();
                        //obj_role.ROLES_CATEGORY_ID = Cust_Id;
                        //obj_role.ROLES_COLUMNSID = dt_package.Rows[0][1].ToString();
                        //obj_role.ROLES_FORMSID = dt_package.Rows[0][0].ToString();
                        //obj_role.ROLES_ROLETYPE = 3;
                        //bool role_status = BLL.InsertUserRolesForWebservice(obj_role, CustomerName, db_name);

                        ////Insert new User
                        //string username = Customer_Email;
                        //SMVTS_USERS obj_user = new SMVTS_USERS();
                        //obj_user.USERS_CATEGORY_ID = Cust_Id;
                        //obj_user.USERS_FULLNAME = CustomerName;
                        //obj_user.USERS_PASSWORD = BLL.Encrypt(Customer_MobileNumber);

                        ////  obj_user.USERS_USERNAME = customername+ CreateRandomUsername(4);
                        //obj_user.USERS_ROLE_ID = 3;

                        //obj_user.USERS_USERNAME = Customer_Email;
                        //bool userstatus = BLL.InsertNewUser(obj_user, db_name);

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


                        //check in SMVTS_TT_DEVIDS

                        // bool status1 = BLL.Insert_Into_tt_devids(device_ID, dt.Rows[j]["IMEI"].ToString());

                        ////check in Sims Table
                        //bool status3 = BLL.Insert_NEW_Sims(dt.Rows[j]["SimNumber"].ToString(), Cust_Id, db_name, dt.Rows[j]["CustomerName"].ToString());

                        ////check in Devices table
                        //bool status2 = BLL.Insert_Into_Devices(device_ID, dt.Rows[j]["IMEI"].ToString(), Cust_Id, dt.Rows[j]["SimNumber"].ToString(), db_name, dt.Rows[j]["CustomerName"].ToString(), orderid, dt.Rows[j]["DeviceType"].ToString());

                        ////Insert Vehicles 
                        //bool veh_status = BLL.Insert_Vehicles(dt.Rows[j]["VehRegNumber"].ToString(), Cust_Id, device_ID, db_name, dt.Rows[j]["CustomerName"].ToString());


                        bool status_device = BLL.DeviceInstallation140(Cust_Id.ToString(), device_ID, Device_IMEI_NO, device_type, sim1,sim2, Vehicle_No, "3", "Cargo", DealerId.ToString(), orderid.ToString(), CustomerName, operator1,operator2);
                        //update wallet
                            bool status2 = BLL.UpdateDeviceWallet(Cust_Id.ToString(), DealerId.ToString());
                        //update Orders
                         bool status = Dal.ExecuteNonQueryPROD("update SMVTS_ORDERS set DEVICE_ID=" + device_ID + " where ORDER_ID=" + orderid + "");
                        if (status_device == true)
                        {
                            resp.status = "Success";
                            this.Context.Response.Write(serializer.Serialize(resp));
                        }
                        else
                        {
                            resp.status = "Failed";
                            this.Context.Response.Write(serializer.Serialize(resp));
                        }
                    }
                }
                else
                {
                    resp.status = "No Dealer Found";
                    this.Context.Response.Write(serializer.Serialize(resp));
                }
            }
            catch (Exception ex)
            {
                resp.status = ex.Message;
                this.Context.Response.Write(serializer.Serialize(resp));
            }

        }
    }
}
