using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PRAGATIVTS_MVC.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
//using Kendo.Mvc.UI;
using System.Globalization;
using System.IO;
using PRAGATIVTS_MVC.Models;
namespace PRAGATIVTS_MVC.Models
{
    #region classes
    public class BLL : System.Web.UI.Page
    {
        string dbname = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
        public override BLL GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public override List<BLL> GetAll()
        {
            throw new NotImplementedException();
        }

        public override List<BLL> GetAllByCategoryID(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public override bool Set(BLL model)
        {
            throw new NotImplementedException();
        }

        public List<PVTS_LOGIN> get_User(string UserName, string Password)
        {
            List<PVTS_LOGIN> obj = new List<PVTS_LOGIN>();

            string strQry = "EXEC SP_PVTS_LOGIN @Operation='CHECK_LOGIN', @USER_USERNAME='" + UserName + "',@USER_PASSWORD='" + Password + "'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_LOGIN
                    {
                        USERID = Convert.ToInt32(dt.Rows[i]["USER_ID"]),
                        ROLEID = Convert.ToInt32(dt.Rows[i]["ROLE_ID"]),
                        ROLENAME = dt.Rows[i]["ROLE_NAME"].ToString(),
                        USERNAME = dt.Rows[i]["USER_NAME"].ToString(),
                        STATUS = Convert.ToInt32(dt.Rows[i]["USER_STATUS"]),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_LOGIN
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public List<PVTS_CATEGORIES> GETALL_CATEGORIES()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_CATEGORIES'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        CATEG_DESC = dt.Rows[i]["CATEG_DESC"].ToString(),
                        CATEG_MOBILENUMBER = dt.Rows[i]["CATEG_MOBILENUMBER"].ToString(),
                        CATEG_EMAILID = dt.Rows[i]["CATEG_EMAILID"].ToString(),
                        CATEG_WEBSITENAME = dt.Rows[i]["CATEG_WEBSITENAME"].ToString(),
                        CATEG_ZIPCODE = dt.Rows[i]["CATEG_ZIPCODE"].ToString(),
                        CATEG_CONTACTPERSON = dt.Rows[i]["CATEG_CONTACTPERSON"].ToString(),
                        CATEG_STATUS = Convert.ToInt32(dt.Rows[i]["CATEG_STATUS"]),
                        CATEG_TYPE = dt.Rows[i]["CATEG_TYPE"].ToString(),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool INSERT_CATEGORIES(string CATEG_NAME, string CATEG_EMAILID, string CATEG_DESC, string CATEG_CONTACTPERSON,
            string CATEG_MOBILENUMBER, string CATEG_TELEPHONE, string CATEG_WEBSITENAME, string CATEG_ZIPCODE, string CATEG_ADDRESS, int CATEG_CREATEDBY,string CATEG_TYPE)
        {
            //ExecuteNonQuery3
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_CATEGORIES', @CATEG_NAME='" + CATEG_NAME + "',@CATEG_EMAILID='" + CATEG_EMAILID + "',@CATEG_DESC='" + CATEG_DESC + "',@CATEG_CONTACTPERSON='" + CATEG_CONTACTPERSON + "',@CATEG_MOBILENUMBER='" + CATEG_MOBILENUMBER + "',@CATEG_TELEPHONE='" + CATEG_TELEPHONE + "',@CATEG_WEBSITENAME='" + CATEG_WEBSITENAME + "',@CATEG_ZIPCODE='" + CATEG_ZIPCODE + "',@CATEG_ADDRESS='" + CATEG_ADDRESS + "',@CATEG_CREATEDBY=" + CATEG_CREATEDBY + ",@CATEG_TYPE='" + CATEG_TYPE + "'";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public bool CATEGORIES_UPDATE(string CATEG_NAME, string CATEG_EMAILID, string CATEG_DESC, string CATEG_CONTACTPERSON,
           string CATEG_MOBILENUMBER, string CATEG_TELEPHONE, string CATEG_WEBSITENAME, string CATEG_ZIPCODE, string CATEG_ADDRESS, int CATEG_UPDATEDBY, string CATEG_TYPE,int CATEG_ID)
        {
            //ExecuteNonQuery3
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CATEGORIES_UPDATE', @CATEG_NAME='" + CATEG_NAME + "',@CATEG_EMAILID='" + CATEG_EMAILID + "',@CATEG_DESC='" + CATEG_DESC + "',@CATEG_CONTACTPERSON='" + CATEG_CONTACTPERSON + "',@CATEG_MOBILENUMBER='" + CATEG_MOBILENUMBER + "',@CATEG_TELEPHONE='" + CATEG_TELEPHONE + "',@CATEG_WEBSITENAME='" + CATEG_WEBSITENAME + "',@CATEG_ZIPCODE='" + CATEG_ZIPCODE + "',@CATEG_ADDRESS='" + CATEG_ADDRESS + "',@CATEG_UPDATEDBY=" + CATEG_UPDATEDBY + ",@CATEG_TYPE='" + CATEG_TYPE + "',@CATEG_ID=" + CATEG_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public bool UPDATE_CATEGORIES_STATUS(int CATEG_STATUS, int CATEG_ID, int CATEG_UPDATEDBY)
        {
            //ExecuteNonQuery3
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_CATEGORIES_STATUS', @CATEG_STATUS=" + CATEG_STATUS + ",@CATEG_UPDATEDBY=" + CATEG_UPDATEDBY + ",@CATEG_UPDATEDDATE='" + DateTime.Now + "',@CATEG_ID=" + CATEG_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_ROLES()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_ROLES'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        ROLE_ID = Convert.ToInt32(dt.Rows[i]["ROLE_ID"]),
                        ROLE_NAME = dt.Rows[i]["ROLE_NAME"].ToString(),
                        ROLE_STATUS = Convert.ToInt32(dt.Rows[i]["ROLE_STATUS"]),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool INSERT_ROLES(string ROLE_NAME)
        {

            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_ROLE', @ROLE_NAME='" + ROLE_NAME + "'";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        //UPDATE_ROLE_STATUS
        public bool UPDATE_ROLE_STATUS(int ROLE_STATUS, int ROLE_ID)
        {

            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_ROLE_STATUS', @ROLE_STATUS=" + ROLE_STATUS + ",@ROLE_ID=" + ROLE_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_USERS()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_USERS'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        USER_ID = Convert.ToInt32(dt.Rows[i]["USER_ID"]),
                        USER_NAME = dt.Rows[i]["USER_NAME"].ToString(),
                        ROLE_NAME = dt.Rows[i]["ROLE_NAME"].ToString(),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        USER_STATUS = Convert.ToInt32(dt.Rows[i]["USER_STATUS"]),
                        USER_PASSWORD = dt.Rows[i]["USER_PASSWORD"].ToString(),
                        USER_EMAILID = dt.Rows[i]["USER_EMAILID"].ToString(),
                        USER_IMAGE = dt.Rows[i]["USER_IMAGE"].ToString(),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool USER_UPDATE(string USER_EMAILID,string USER_PASSWORD,string  USER_IMAGE,int USER_UPDATEDBY, int USER_ID)
        {
            bool b = false;
            string Query = "";
            if(USER_IMAGE != "" && USER_IMAGE !=null)
            {
                 Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='USER_UPDATE', @USER_EMAILID='" + USER_EMAILID + "',@USER_PASSWORD='" + USER_PASSWORD + "',@USER_UPDATEDBY=" + USER_UPDATEDBY + ",@USER_IMAGE='" + USER_IMAGE + "',@USER_ID=" + USER_ID + "";
            }
            else
            {
                 Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='USER_UPDATE_NOIMAGE', @USER_EMAILID='" + USER_EMAILID + "',@USER_PASSWORD='" + USER_PASSWORD + "',@USER_UPDATEDBY=" + USER_UPDATEDBY + ",@USER_ID=" + USER_ID + "";
            }
            
            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public bool UPDATE_USER_STATUS(int USER_STATUS, int USER_ID)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_USER_STATUS', @USER_STATUS=" + USER_STATUS + ",@USER_ID=" + USER_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public bool CREATE_USER(int CATEG_ID, int ROLE_ID, string USER_NAME, string USER_EMAILID, string USER_PASSWORD, string USER_MOBILENUMBER, string USER_DEVICE_IDS, int USER_CREATEDBY)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_USER',@CATEG_ID = " + CATEG_ID + ",@ROLE_ID =" + ROLE_ID + ",@USER_USERNAME='" + USER_NAME + "',@USER_EMAILID='" + USER_EMAILID + "',@USER_PASSWORD='" + USER_PASSWORD + "',@USER_MOBILENUMBER='" + USER_MOBILENUMBER + "',@USER_DEVICE_IDS='" + USER_DEVICE_IDS + "',@USER_CREATEDBY=" + USER_CREATEDBY + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_SIMS()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_SIMS'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        SIM_ID = Convert.ToInt32(dt.Rows[i]["SIM_ID"]),
                        SIM_OPERATORNAME = dt.Rows[i]["SIM_OPERATORNAME"].ToString(),
                        SIM_SERIALNO = dt.Rows[i]["SIM_SERIALNO"].ToString(),
                        SIM_NUMBER = dt.Rows[i]["SIM_NUMBER"].ToString(),
                        SIM_STATUS = Convert.ToInt32(dt.Rows[i]["SIM_STATUS"]),
                        SIM_APNIP = dt.Rows[i]["SIM_APNIP"].ToString(),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        SIM_APNWEBSITE = dt.Rows[i]["SIM_APNWEBSITE"].ToString(),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool CREATE_SIM(string SIM_NUMBER, string SIM_SERIALNO, string SIM_OPERATORNAME, string SIM_APNWEBSITE, string SIM_APNIP, string CATEG_ID, int SIM_CREATEDBY)
        {

            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_SIMS',@SIM_NUMBER='" + SIM_NUMBER + "',@SIM_SERIALNO='" + SIM_SERIALNO + "',@SIM_OPERATORNAME='" + SIM_OPERATORNAME + "',@SIM_APNWEBSITE='" + SIM_APNWEBSITE + "',@SIM_APNIP='" + SIM_APNIP + "',@CATEG_ID=" + CATEG_ID + ",@SIM_CREATEDBY=" + SIM_CREATEDBY + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public bool UPDATE_SIM(string SIM_NUMBER, string SIM_SERIALNO, string SIM_OPERATORNAME, string SIM_APNWEBSITE, string SIM_APNIP, string CATEG_ID, int SIM_UPDATEDBY, int SIM_ID)
        {

            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_SIMS',@SIM_NUMBER='" + SIM_NUMBER + "',@SIM_SERIALNO='" + SIM_SERIALNO + "',@SIM_OPERATORNAME='" + SIM_OPERATORNAME + "',@SIM_APNWEBSITE='" + SIM_APNWEBSITE + "',@SIM_APNIP='" + SIM_APNIP + "',@CATEG_ID=" + CATEG_ID + ",@SIM_CREATEDBY=" + SIM_UPDATEDBY + ",@SIM_ID=" + SIM_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);

            return b;
        }
        public bool UPDATE_SIM_STATUS(int SIM_STATUS, int SIM_ID)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_SIM_STATUS',@SIM_STATUS='" + SIM_STATUS + "',@SIM_ID=" + SIM_ID + "";
            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_DEVICES()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_DEVICES'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        SIM_ID = Convert.ToInt32(dt.Rows[i]["SIM_ID"]),
                        DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"]),
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        DEVICE_NAME = dt.Rows[i]["DEVICE_NAME"].ToString(),
                        DEVICE_MFGDATE = dt.Rows[i]["DEVICE_MFGDATE"].ToString(),
                        DEVICE_SERIALNUMBER = dt.Rows[i]["DEVICE_SERIALNUMBER"].ToString(),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        SIM_SERIALNO = dt.Rows[i]["SIM_SERIALNO"].ToString(),
                        SIM_NUMBER = dt.Rows[i]["SIM_NUMBER"].ToString(),
                        DEVICE_STATUS = Convert.ToInt32(dt.Rows[i]["DEVICE_STATUS"]),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool CREATE_DEVICE(string DEVICE_NAME, string DEVICE_SERIALNUMBER, string DEVICE_MFGDATE, string SIM_ID, string CATEG_ID, int DEVICE_CREATEDBY)
        {

            bool b = false;
            //   string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_DEIVCES',@DEVICE_NAME='" + DEVICE_NAME + "',@DEVICE_SERIALNUMBER=" + DEVICE_SERIALNUMBER + ",@DEVICE_MFGDATE='" + DEVICE_MFGDATE + "',@SIM_ID=" + SIM_ID + ",@CATEG_ID=" + CATEG_ID + ",@DEVICE_CREATEDBY=" + DEVICE_CREATEDBY + "";

            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='NEW_CREATE_DEVICE',@DEVICE_NAME='" + DEVICE_NAME + "',@DEVICE_SERIALNUMBER='" + DEVICE_SERIALNUMBER + "',@DEVICE_MFGDATE='" + DEVICE_MFGDATE + "',@SIM_ID=" + SIM_ID + ",@CATEG_ID=" + CATEG_ID + ",@DEVICE_CREATEDBY=" + DEVICE_CREATEDBY + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool UPDATE_DEVICE(string DEVICE_NAME, string DEVICE_SERIALNUMBER, string DEVICE_MFGDATE, string SIM_ID, string CATEG_ID, int DEVICE_UPDATEDBY, int DEVICE_ID)
        {

            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_DEVICE',@DEVICE_NAME='" + DEVICE_NAME + "',@DEVICE_SERIALNUMBER='" + DEVICE_SERIALNUMBER + "',@DEVICE_MFGDATE='" + DEVICE_MFGDATE + "',@SIM_ID=" + SIM_ID + ",@CATEG_ID=" + CATEG_ID + ",@DEVICE_CREATEDBY=" + DEVICE_UPDATEDBY + ",@DEVICE_ID=" + DEVICE_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool UPDATE_DEVICES_STATUS(int DEVICE_STATUS, int DEVICE_ID)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_DEVICES_STATUS',@DEVICE_STATUS='" + DEVICE_STATUS + "',@DEVICE_ID=" + DEVICE_ID + "";
            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_VEHILCES()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_VEHICLES'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        VEHICLE_ID = Convert.ToInt32(dt.Rows[i]["VEHICLE_ID"]),
                        CATEG_ID = Convert.ToInt32(dt.Rows[i]["CATEG_ID"]),
                        DEVICE_ID = Convert.ToInt32(dt.Rows[i]["DEVICE_ID"]),
                        CATEG_NAME = dt.Rows[i]["CATEG_NAME"].ToString(),
                        VEHICLE_REGNUMBER = dt.Rows[i]["VEHICLE_REGNUMBER"].ToString(),
                        //DEVICE_SERIALNUMBER = dt.Rows[i]["DEVICE_SERIALNUMBER"].ToString(),
                        DEVICE_NAME = dt.Rows[i]["DEVICE_NAME"].ToString(),
                        SIM_SERIALNO = dt.Rows[i]["SIM_SERIALNO"].ToString(),
                        VEHICLE_STATUS = Convert.ToInt32(dt.Rows[i]["VEHICLE_STATUS"]),
                        VEHICLE_ZONE = dt.Rows[i]["VEHICLE_ZONE"].ToString(),
                        VEHICLE_MAXSPEED = dt.Rows[i]["VEHICLE_MAXSPEED"].ToString(),
                        VEHICLE_MODELNAME = dt.Rows[i]["VEHICLE_MODELNAME"].ToString(),
                        VEHICLE_CAPACITY = dt.Rows[i]["VEHICLE_CAPACITY"].ToString(),
                        VEHICLE_MILLAGE = dt.Rows[i]["VEHICLE_MILLAGE"].ToString(),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool CREATE_VEHICLES(string VEHICLE_REGNUMBER, int DEVICE_ID, int CATEG_ID, string VEHICLE_ZONE, string VEHICLE_MODELNAME, string VEHICLE_MAXSPEED, string VEHICLE_MILLAGE, int VEHICLE_CREATEDBY)
        {

            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_VEHICLES',@VEHICLE_REGNUMBER='" + VEHICLE_REGNUMBER + "',@DEVICE_ID=" + DEVICE_ID + ",@CATEG_ID=" + CATEG_ID + ",@VEHICLE_ZONE='" + VEHICLE_ZONE + "',@VEHICLE_MODELNAME='" + VEHICLE_MODELNAME + "',@VEHICLE_MAXSPEED='" + VEHICLE_MAXSPEED + "',@VEHICLE_MILLAGE='" + VEHICLE_MILLAGE + "',@VEHICLE_CREATEDBY=" + VEHICLE_CREATEDBY + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool UPDATE_VEHICLES(string VEHICLE_REGNUMBER, int DEVICE_ID, int CATEG_ID, string VEHICLE_ZONE, string VEHICLE_MODELNAME, string VEHICLE_MAXSPEED, string VEHICLE_MILLAGE, int VEHICLE_UPDATEDBY, int VEHICLE_ID)
        {

            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_VEHICLES',@VEHICLE_REGNUMBER='" + VEHICLE_REGNUMBER + "',@DEVICE_ID=" + DEVICE_ID + ",@CATEG_ID=" + CATEG_ID + ",@VEHICLE_ZONE='" + VEHICLE_ZONE + "',@VEHICLE_MODELNAME='" + VEHICLE_MODELNAME + "',@VEHICLE_MAXSPEED='" + VEHICLE_MAXSPEED + "',@VEHICLE_MILLAGE='" + VEHICLE_MILLAGE + "',@VEHICLE_UPDATEDBY=" + VEHICLE_UPDATEDBY + ",@VEHICLE_ID=" + VEHICLE_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool UPDATE_VEHICLE_STATUS(int VEHICLE_STATUS, int VEHICLE_ID)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_VEHICLE_STATUS',@VEHICLE_STATUS='" + VEHICLE_STATUS + "',@VEHICLE_ID=" + VEHICLE_ID + "";
            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_PAGES()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_PAGES'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        PAGE_ID = Convert.ToInt32(dt.Rows[i]["PAGE_ID"]),
                        PAGE_STATUS = Convert.ToInt32(dt.Rows[i]["PAGE_STATUS"]),
                        PAGE_UNDER = dt.Rows[i]["PAGE_UNDER"].ToString(),
                        PAGE_NAME = dt.Rows[i]["PAGE_NAME"].ToString(),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool CREATE_PAGES(string PAGE_NAME, int PAGE_CREATEDBY, string PAGE_UNDER)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_PAGES',@PAGE_NAME='" + PAGE_NAME + "',@PAGE_CREATEDBY=" + PAGE_CREATEDBY + ",@PAGE_UNDER='" + PAGE_UNDER + "'";

            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool UPDATE_PAGES_STATUS(int PAGE_STATUS, int PAGE_ID)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_PAGES_STATUS',@PAGE_STATUS='" + PAGE_STATUS + "',@PAGE_ID=" + PAGE_ID + "";
            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool PAGE_UPDATE(string PAGE_NAME,string PAGE_UNDER,int PAGE_UPDATEDBY,int PAGE_ID)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='PAGE_UPDATE',@PAGE_NAME='" + PAGE_NAME + "',@PAGE_UNDER='" + PAGE_UNDER + "',@PAGE_UPDATEDBY=" + PAGE_UPDATEDBY + ",@PAGE_ID="+PAGE_ID+"";
            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool CREATE_PERMISSIONS(int userid, string name, string view, string edit, string create)
        {

            string[] names = name.Split(',');
            string[] views = view.Split(',');
            string[] editd = edit.Split(',');
            string[] creates = create.Split(',');
            bool b = false;
            for (int i = 0; i < names.Length; i++)
            {
                string dash = names[i];
                string viewdata = views[i];
                string editdata = editd[i];
                string createdata = creates[i];
                string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_PERMISSIONS',@USER_ID=" + userid + ",@PAGE_NAME='" + dash + "',@PP_VIEW=" + viewdata + ",@PP_EDIT=" + editdata + ",@PP_CREATE=" + createdata + "";
                b = BLL.ExecuteNonQuery3(Query, dbname);
            }
            return b;
        }
        public bool UPDATE_PERMISSIONS(string name, string view, string edit, string create, int USER_ID)
        {

            string[] names = name.Split(',');
            string[] views = view.Split(',');
            string[] editd = edit.Split(',');
            string[] creates = create.Split(',');
            bool b = false;
            for (int i = 0; i < names.Length; i++)
            {
                string dash = names[i];
                string viewdata = views[i];
                string editdata = editd[i];
                string createdata = creates[i];

                string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CHECK_USER_PERMISSIONS',@USER_ID=" + USER_ID + "";
                DataTable dt = null;
                dt = BLL.ExecuteQuery3(strQry, dbname);
                string PAGE_NAME = "";
                try
                {
                    PAGE_NAME = dt.Rows[i]["PAGE_NAME"].ToString();
                    if (PAGE_NAME == dash)
                    {
                        string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_PERMISSIONS',@PAGE_NAME='" + dash + "',@PP_VIEW=" + viewdata + ",@PP_EDIT=" + editdata + ",@PP_CREATE=" + createdata + ",@USER_ID="+USER_ID+"";
                        b = BLL.ExecuteNonQuery3(Query, dbname);
                    }
                }
                catch (Exception ex)
                {
                    string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_PERMISSIONS',@USER_ID=" + USER_ID + ",@PAGE_NAME='" + dash + "',@PP_VIEW=" + viewdata + ",@PP_EDIT=" + editdata + ",@PP_CREATE=" + createdata + "";
                    b = BLL.ExecuteNonQuery3(Query, dbname);
                }





            }
            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_PAGE_PERMISSIONS(int USER_ID)
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CHECK_USER_PERMISSIONS',@USER_ID=" + USER_ID + "";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        PP_ID = Convert.ToInt32(dt.Rows[i]["PP_ID"]),
                        PAGE_NAME = dt.Rows[i]["PAGE_NAME"].ToString(),
                        PP_VIEW = Convert.ToInt32(dt.Rows[i]["PP_VIEW"]),
                        PP_EDIT = Convert.ToInt32(dt.Rows[i]["PP_EDIT"]),
                        PP_CREATE = Convert.ToInt32(dt.Rows[i]["PP_CREATE"]),
                        PAGE_UNDER = dt.Rows[i]["PAGE_UNDER"].ToString(),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public List<PVTS_CATEGORIES> PERMISSIONS_PAGENAME(int USER_ID,string PAGE_NAME)
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='PERMISSIONS_PAGENAME',@USER_ID=" + USER_ID + ",@PAGE_NAME='"+PAGE_NAME+"'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        PP_ID = Convert.ToInt32(dt.Rows[i]["PP_ID"]),
                        PAGE_NAME = dt.Rows[i]["PAGE_NAME"].ToString(),
                        PP_VIEW = Convert.ToInt32(dt.Rows[i]["PP_VIEW"]),
                        PP_EDIT = Convert.ToInt32(dt.Rows[i]["PP_EDIT"]),
                        PP_CREATE = Convert.ToInt32(dt.Rows[i]["PP_CREATE"]),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }
        public bool CREATE_PANEL(string PANEL_NAME, int PANEL_CREATEDBY)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='CREATE_PANEL',@PANEL_NAME='" + PANEL_NAME + "',@PANEL_CREATEDBY=" + PANEL_CREATEDBY + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public bool UPDATE_PANEL_STATUS(string PANEL_STATUS, int PANEL_UPDATEDBY, int PANEL_ID)
        {
            bool b = false;
            string Query = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='UPDATE_PANEL_STATUS',@PANEL_STATUS='" + PANEL_STATUS + "',@PANEL_UPDATEDBY=" + PANEL_UPDATEDBY + ",@PANEL_ID=" + PANEL_ID + "";

            b = BLL.ExecuteNonQuery3(Query, dbname);
            return b;
        }
        public List<PVTS_CATEGORIES> GETALL_PANELS()
        {
            List<PVTS_CATEGORIES> obj = new List<PVTS_CATEGORIES>();

            string strQry = "EXEC SP_PVTS_CREATEADMINPANEL @Operation='GETALL_PANEL'";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_CATEGORIES
                    {
                        PANEL_ID = Convert.ToInt32(dt.Rows[i]["PANEL_ID"]),
                        PANEL_NAME = dt.Rows[i]["PANEL_NAME"].ToString(),
                        PANEL_STATUS = Convert.ToInt32(dt.Rows[i]["PANEL_STATUS"]),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }

        public List<PVTS_DASHBOARD> GETALL_VEHICLES_DASHBOARD(int CATEG_ID)
        {
            List<PVTS_DASHBOARD> obj = new List<PVTS_DASHBOARD>();

            string strQry = "EXEC SP_PVTS_DASHBOARD @OPERATION='GETALL_DASHBOARD',@CATEGORY="+CATEG_ID+"";
            DataTable dt = null;

            dt = BLL.ExecuteQuery3(strQry, dbname);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new PVTS_DASHBOARD
                    {
                       LATITUDE=dt.Rows[i]["latitude"].ToString(),
                       LONGITUDE = dt.Rows[i]["longitude"].ToString(),
                       SERVERTIME = dt.Rows[i]["servertime"].ToString(),
                       CURRENT_SPEED = dt.Rows[i]["CURRENT_SPEED"].ToString(),
                       ATTRIBUTES = dt.Rows[i]["attributes"].ToString(),
                       COURSE = dt.Rows[i]["course"].ToString(),
                       COLOR = dt.Rows[i]["COLOR"].ToString(),
                       ADDRESS = dt.Rows[i]["address"].ToString(),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new PVTS_DASHBOARD
                {
                    MESSAGE = "FAILED",
                });

            }

            return obj;
        }








        internal static string getDatabase(string Orgname)
        {
            string Dbname = "";

            //string Query = "SELECT top 1 CATEG_DBNAME from smvts_categories WHERE CATEG_NAME='"+Orgname+"'";
            string Query = "";
            if (Orgname.Trim().Contains("(C)"))
            {
                Query = "SELECT TOP 1 CATEG_DBNAME FROM SMVTS_CATEGORIES WHERE CATEG_NAME='" + Orgname.Replace("(C)", "").Trim() + "'";
            }
            else
            {
                Query = "SELECT TOP 1 CATEG_DBNAME FROM SMVTS_CATEGORIES WHERE CATEG_NAME='" + Orgname.Replace("(P)", "").Trim() + "'";
            }
            DataTable dt_table = BLL.ExecuteQueryDB(Query);

            if (dt_table != null)
            {
                if (dt_table.Rows.Count > 0)
                {
                    Dbname = dt_table.Rows[0][0].ToString();
                }
            }
            return Dbname;

        }

        internal static List<SMVTS_CATEGORIES> get_User(string CompanyName, string UserName, string dbname)
        {
            List<SMVTS_CATEGORIES> obj = new List<SMVTS_CATEGORIES>();

            string strQry = " EXEC USP_SMVTS_USERS @Operation='CHECK_LOGIN', @USERS_USERNAME='" + ReplaceQuote(UserName) + "',@USERS_FULLNAME='" + ReplaceQuote(CompanyName) + "'";
            DataTable dt = null;

            dt = BLL.ExecuteQueryDB1(strQry, dbname);

             if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj.Add(new SMVTS_CATEGORIES
                    {
                        USERID = Convert.ToInt32(dt.Rows[i]["USERS_ID"]),
                        ROLEID = Convert.ToInt32(dt.Rows[i]["USERS_ROLE_ID"]),
                        CATEGORYID = Convert.ToInt32(dt.Rows[i]["USERS_CATEGORY_ID"]),
                        USERNAME=Convert.ToString(dt.Rows[0]["USERS_USERNAME"]),
                        PASSWORD= Convert.ToString(dt.Rows[0]["USERS_PASSWORD"]),
                         USER_FULLNAME=Convert.ToString(dt.Rows[0]["USERS_FULLNAME"]),
                        USER_STATUS=Convert.ToBoolean(dt.Rows[0]["USERS_STATUS"]),
                         USER_DEVICEID=Convert.ToString(dt.Rows[0]["USERS_DEVICE_IDS"]),
                           LOGONAMEHEADER=Convert.ToString(dt.Rows[0]["LOGOS_NAME"]),         //LOGOS Header
                            LOGOHEADERURL=Convert.ToString(dt.Rows[0]["LOGOS_URL"]),          //LOGOS Header URL
                              LOGONAME= Convert.ToString(dt.Rows[1]["LOGOS_NAME"]),         //LOGOS
                           LOGOURL= Convert.ToString(dt.Rows[1]["LOGOS_URL"]),          //LOGOS URL
                            FORMID= Convert.ToString(dt.Rows[0]["ROLES_FORMSID"]),
                            COLUMNID= Convert.ToString(dt.Rows[0]["ROLES_COLUMNIDS"]),       //Forms id
                            TIMEZONE= Convert.ToInt32(dt.Rows[0]["COUNTRY_TIMEZONE"]),
                            //""
                             CATEGORYNAME=   Convert.ToString(dt.Rows[0]["CATEG_NAME"]),
                       // USERS_MENUITEMS = Convert.ToString(dt.Rows[0]["USERS_MENUITEMS"]),
                        MESSAGE = "SUCCESS",
                    });
                }
            }
            else
            {
                obj.Add(new SMVTS_CATEGORIES
                {
                    MESSAGE = "FAILED",
                });

            }
             return obj;
        }


        public static DataTable get_Categories(SMVTS_CATEGORIES _obj_Smvts_Categories)
        {
            DataTable dtCateg = new DataTable();
            switch (_obj_Smvts_Categories.OPERATION)
            {
                case operation.Select:
                    if ((Convert.ToString(_obj_Smvts_Categories.CATEG_ID) == "0") && (Convert.ToString(_obj_Smvts_Categories.CATEG_CATETYPE_ID) == "3"))

                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Select_ClientID'");
                    else if (Convert.ToString(_obj_Smvts_Categories.CATEG_ID) == "0")
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'select'");
                    else
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'select', @CATEG_ID ='" + Convert.ToString(_obj_Smvts_Categories.CATEG_ID) + "'");

                    break;

                case operation.Delete:
                    if (Convert.ToString(_obj_Smvts_Categories.CATEG_ID) == "0")
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'selectByOrder'");
                    else
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Select_UOM', @CATEG_ID ='" + Convert.ToString(_obj_Smvts_Categories.CATEG_ID) + "'");

                    break;

                case operation.Check:
                    if (Convert.ToString(_obj_Smvts_Categories.CATEG_ID) == "0")
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Check', @CATEG_NAME ='" + Convert.ToString(_obj_Smvts_Categories.CATEG_NAME) + "'");
                    else
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Check', @CATEG_NAME ='" + Convert.ToString(_obj_Smvts_Categories.CATEG_NAME) + "', @CATEG_ID ='" + Convert.ToString(_obj_Smvts_Categories.CATEG_ID) + "'");

                    break;
                case operation.Empty:
                    dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Select_ParentID'");

                    break;

                case operation.Update:
                    if (Convert.ToString(_obj_Smvts_Categories.CATEG_CATETYPE_ID) != "0")
                        //Get all Partners
                        //changes done for bajaj removed 1 from query
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Update' , @CATEG_CATETYPE_ID = '" + Convert.ToString(_obj_Smvts_Categories.CATEG_CATETYPE_ID) + "'");
                    else
                        //Dhanush (Org)
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Select_Org'");

                    break;

                case operation.Insert:
                    if ((Convert.ToString(_obj_Smvts_Categories.CATEG_CATETYPE_ID) != "0") && (Convert.ToString(_obj_Smvts_Categories.CATEG_PARENT_ID) == "0"))
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'GetAllClient' , @CATEG_CATETYPE_ID = '" + Convert.ToString(_obj_Smvts_Categories.CATEG_CATETYPE_ID) + "', @CATEG_ID = '" + Convert.ToString(_obj_Smvts_Categories.CATEG_ID) + "'");
                    else if (Convert.ToString(_obj_Smvts_Categories.CATEG_PARENT_ID) != "0")
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'GetAllClient' , @CATEG_CATETYPE_ID = '" + Convert.ToString(_obj_Smvts_Categories.CATEG_CATETYPE_ID) + "', @CATEG_PARENT_ID = '" + Convert.ToString(_obj_Smvts_Categories.CATEG_PARENT_ID) + "'");
                    else
                        //Get all Clients
                        dtCateg = ExecuteQueryDB1("EXEC USP_SMVTS_CATEGORIES @Operation = 'Select_ClientID'");

                    break;

                default:
                    break;
            }
            return dtCateg;

        }




        internal static string ReplaceQuote(string str)
        {
            return str.Replace("'", "''");
        }

       
    }


    public class PVTS_LOGIN
    {
        public string USERNAME { get; set; }
        public string USER_FULLNAME { get; set; }
        public bool USER_STATUS { get; set; }
        public string PASSWORD { get; set; }
        public int CATEGORYID { get; set; }
        public string CATEGORYNAME { get; set; }
        public int USERID { get; set; }
        public int STATUS { get; set; }
        public string MESSAGE { get; set; }
        public int ROLEID { get; set; }
        public string ROLENAME { get; set; }
    }


    public class SMVTS_CATEGORIES
    {
        public string USERNAME { get; set; }
        public string USER_FULLNAME { get; set; }
        public string LOGONAMEHEADER { get; set; }
        public string MENUITEMS { get; set; }

        public string LOGOHEADERURL { get; set; }

        public string LOGONAME { get; set; }

        public string LOGOURL { get; set; }

        public string FORMID { get; set; }
        public string COLUMNID { get; set; }

        public int TIMEZONE { get; set; }

        public string USERS_MENUITEMS { get; set; }

        public bool USER_STATUS { get; set; }
        public string USER_DEVICEID { get; set; }
        public string PASSWORD { get; set; }
        public int CATEGORYID { get; set; }
        public string CATEGORYNAME { get; set; }
        public int USERID { get; set; }
        public int STATUS { get; set; }
        public string MESSAGE { get; set; }

      

        public string OPERATION { get; set; }

        public string CATEG_ID { get; set; }

        public string CATEG_CATETYPE_ID { get; set; }

        public int ROLEID { get; set; }
    }






    public class PVTS_CATEGORIES
    {
        public string CATEG_NAME { get; set; }
        public string CATEG_EMAILID { get; set; }
        public string CATEG_DESC { get; set; }
        public string CATEG_CONTACTPERSON { get; set; }
        public string CATEG_MOBILENUMBER { get; set; }
        public string CATEG_TELEPHONE { get; set; }
        public string CATEG_WEBSITENAME { get; set; }
        public string CATEG_ZIPCODE { get; set; }
        public string CATEG_ADDRESS { get; set; }
        public string MESSAGE { get; set; }
        public int CATEG_STATUS { get; set; }
        public int CATEG_ID { get; set; }

        public int ROLE_ID { get; set; }

        public string ROLE_NAME { get; set; }

        public int ROLE_STATUS { get; set; }

        public string USER_NAME { get; set; }

        public int USER_ID { get; set; }

        public int USER_STATUS { get; set; }

        public string USER_PASSWORD { get; set; }

        public string USER_EMAILID { get; set; }

        public int SIM_ID { get; set; }

        public string SIM_NUMBER { get; set; }

        public string SIM_APNIP { get; set; }

        public int SIM_STATUS { get; set; }

        public string SIM_SERIALNO { get; set; }

        public string SIM_OPERATORNAME { get; set; }

        public int DEVICE_ID { get; set; }

        public string DEVICE_NAME { get; set; }

        public string DEVICE_SERIALNUMBER { get; set; }

        public int DEVICE_STATUS { get; set; }

        public int VEHICLE_STATUS { get; set; }

        public string VEHICLE_ZONE { get; set; }

        public string VEHICLE_REGNUMBER { get; set; }

        public int VEHICLE_ID { get; set; }

        public int PAGE_ID { get; set; }

        public int PAGE_STATUS { get; set; }

        public string PAGE_NAME { get; set; }

        public int PP_EDIT { get; set; }

        public int PP_CREATE { get; set; }

        public int PP_VIEW { get; set; }

        public int PP_ID { get; set; }

        public string DEVICE_MFGDATE { get; set; }

        public string SIM_APNWEBSITE { get; set; }

        public string VEHICLE_MODELNAME { get; set; }

        public string VEHICLE_CAPACITY { get; set; }

        public string VEHICLE_MILLAGE { get; set; }

        public string VEHICLE_MAXSPEED { get; set; }

        public int PANEL_ID { get; set; }

        public int PANEL_STATUS { get; set; }

        public string PANEL_NAME { get; set; }

        public string PAGE_UNDER { get; set; }

        public string CATEG_TYPE { get; set; }

        public string USER_IMAGE { get; set; }
    }



























    public class PVTS_DASHBOARD
    {
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string ATTRIBUTES { get; set; }
        public string COURSE { get; set; }
        public string CURRENT_SPEED { get; set; }
        public string DEVICEID { get; set; }
        public string SERVERTIME { get; set; }
        public string COLOR { get; set; }
        public string ADDRESS { get; set; }
        public string MESSAGE { get; set; }
    }
}
    #endregion