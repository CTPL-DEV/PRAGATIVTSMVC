using System;
using System.Data;
using System.Configuration;
using System.Web;
using PRAGATIVTS_MVC.Models;

public class Dal
{
    //static string MasterDB = ConfigurationManager.AppSettings["MasterDB"].ToString();

    //static string strConn_PROD = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_PROD"].ToString());
    //static string strConn_TEST = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_TEST"].ToString());
    //static string strConn_DEVDB = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_DEVDB"].ToString());
    //static string strConn_LIVETEST = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_LIVETEST"].ToString());

    //static string strConn;


    public static DataTable ExecuteQuery(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
        try
        {
            if (HttpContext.Current.Session["USERINFO"] != null)
            {
                //for sairamschool
                // string DBName = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUJxaM2lG1Dw54Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0=");
                //for school sms connection string

                //string DBName = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUhcs0nXyC5SR4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0=");     
                string DBName = BLL.Decrypt(((SMVTS_USERS)(HttpContext.Current.Session["USERINFO"])).USERS_DBNAME);
                strConn = (DBName);
            }
            //_obj.STRSQL = Convert.ToString(Query).Replace("'","''");
            //_obj.STARTDATE = System.DateTime.Now;
            //return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            DataTable dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            //_obj.ENDDATE = System.DateTime.Now;
            //_obj.USERID = "0";
            //_obj.OPERATION = operation.Insert;
            //BLL.set_TraceLog_ErrorLog(_obj);


            return dt;
        }
        catch (Exception ex)
        {
            //_obj.ERR_DESC = Convert.ToString(Query).Replace("'", "''");
            //_obj.STARTDATE = System.DateTime.Now;
            //_obj.ERR_DESC = ex.Message;
            //  DataTable dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            //_obj.ENDDATE = System.DateTime.Now;
            //_obj.USERID = "0";
            //_obj.OPERATION = operation.Update;
            //BLL.set_TraceLog_ErrorLog(_obj);
            return null;
        }
    }

    //by venkatesh for siritecon report check on 27-03-2018
    public static DataTable ExecuteQuery_reportcheck(string Query)
    {
        string strConn = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUJxaM2lG1Dw54Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0=");
        try
        {
            if (HttpContext.Current.Session["USERINFO"] != null)
            {
                //for sairamschool
                // string DBName = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUJxaM2lG1Dw54Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0=");
                //for school sms connection string

                //string DBName = BLL.Decrypt("Aor2T0SveXPKBbMcC7tSjnV2biNo1UUCLQOZCWJCgdbGhTWCzxCZGIf5D1n4CCQUhcs0nXyC5SR4Ew7+Lt094Kzb8IV1HY40efjsVQ77zv/jAT15afI3tIplCvGTG6YErrb1ZxuqpT0=");     
                string DBName = BLL.Decrypt(((SMVTS_USERS)(HttpContext.Current.Session["USERINFO"])).USERS_DBNAME);
                strConn = (DBName);
            }
            //_obj.STRSQL = Convert.ToString(Query).Replace("'","''");
            //_obj.STARTDATE = System.DateTime.Now;
            //return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            DataTable dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            //_obj.ENDDATE = System.DateTime.Now;
            //_obj.USERID = "0";
            //_obj.OPERATION = operation.Insert;
            //BLL.set_TraceLog_ErrorLog(_obj);


            return dt;
        }
        catch (Exception)
        {
            //_obj.ERR_DESC = Convert.ToString(Query).Replace("'", "''");
            //_obj.STARTDATE = System.DateTime.Now;
            //_obj.ERR_DESC = ex.Message;
            //  DataTable dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            //_obj.ENDDATE = System.DateTime.Now;
            //_obj.USERID = "0";
            //_obj.OPERATION = operation.Update;
            //BLL.set_TraceLog_ErrorLog(_obj);
            return null;
        }
    }

    public static bool ExecuteNonQuery(string Query)
    {
        // SMVTS_TRACE_ERROR_LOG _obj1 = new SMVTS_TRACE_ERROR_LOG();
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
        // SMVTS_TRACE_ERROR_LOG _obj1 = new SMVTS_TRACE_ERROR_LOG();
        try
        {
            if (HttpContext.Current.Session["USERINFO"] != null)
            {
                string DBName = BLL.Decrypt(((SMVTS_USERS)(HttpContext.Current.Session["USERINFO"])).USERS_DBNAME);
                strConn = (DBName);
            }
            //    _obj1.STRSQL = Convert.ToString(Query).Replace("'", "''");
            //   _obj1.STARTDATE = System.DateTime.Now;
            //return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            bool strAns = Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
            //  _obj1.ENDDATE = System.DateTime.Now;
            //   _obj1.USERID = "0";
            //   _obj1.OPERATION = operation.Insert;
            //   BLL.set_TraceLog_ErrorLog(_obj1);


            return strAns;
        }
        catch (Exception EX)
        {
            //    _obj1.STRSQL = Convert.ToString(Query).Replace("'", "''");
            //   _obj1.ERR_DESC = ex.Message;
            //   _obj1.STARTDATE = System.DateTime.Now;
            //  DataTable dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            //    _obj1.ENDDATE = System.DateTime.Now;
            //    _obj1.USERID = "0";
            //    _obj1.OPERATION = operation.Update;
            //   BLL.set_TraceLog_ErrorLog(_obj1);
            return false;
        }

    }
    // Production database
    public static DataTable ExecutePRODDB(string Query)
    {
      string strConn=BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
      // string strConn = "Data Source=192.168.1.29;Initial Catalog=SMVTS_PROD01;User ID=ctpl;Password=CTPL@vts2019";
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    public static bool ExecuteNonQueryPROD(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
        //  string strConn = "Data Source=192.168.1.29;Initial Catalog=SMVTS_PROD01;User ID=ctpl;Password=CTPL@vts2019";
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }
    //Config Database


    public static bool ExecuteNonQueryConfig(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
       // string strConn = "Data Source=192.168.1.29;Initial Catalog=VTS_CONFIG;User ID=ctpl;Password=CTPL@vts2019";
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }
    public static DataTable ExecuteConfig(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
        //  string strConn = "Data Source=192.168.1.29;Initial Catalog=VTS_CONFIG;User ID=vts;Password=CTPL@vts2019";
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    //PRADEEP 149SERVER PUSHPAK
    public static DataTable ExecuteQuery_PUSHPAK(string Query)
    {
        string strConn = "Data Source=192.168.1.29;Initial Catalog=PUSHPAK_SMVTS;User ID=smvts;Password=ZCBAVTSDHANUSH2010INDIA";
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    public static DataTable ExecuteQuery_Prod(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
       // string strConn = "Data Source=192.168.1.29;Initial Catalog=SMVTS_PROD01;User ID=vts;Password=CTPL@vts2019";
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    
    
    // these methods are for executing the trace_log & errro_log insert query
    public static DataTable ExecuteQuery1(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    public static DataTable ExecuteQuery_Prod_for_Test(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod_test"].ToString());
        // string strConn = "Data Source=192.168.1.29;Initial Catalog=SMVTS_PROD01;User ID=vts;Password=CTPL@vts2019";
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    public static DataTable ExecuteQueryTest(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_test"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    public static bool ExecuteNonQuery_prod(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }
    public static bool ExecuteNonQueryTest(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_test"].ToString());
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }
    public static bool ExecuteNonQuery_prod_test(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod_test"].ToString());
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }
    public static bool ExecuteNonQuery1(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }
    public static DataTable ExecuteQuery2(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["EXPRESSCONN"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    public static bool ExecuteNonQuery2(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["EXPRESSCONN"].ToString());
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }

    public static bool ExecuteNonQuery3(string Query, string dbname)
    {
        string strConn = BLL.Decrypt(dbname);
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }

    public static DataTable ExecuteQuery3(string Query, string dbname)
    {
        string strConn = BLL.Decrypt(dbname);
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }

    internal static bool ExecuteNonQueryDB(string Query, string DBName)
    {
        string strConn = (DBName);

        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }

    internal static int executescalar(string Query, string DBName)
    {
        string strConn = (DBName);

        return Convert.ToInt32(SqlHelper.ExecuteScalar(strConn, CommandType.Text, Query));
    }


    //public static DataTable ExecuteQuery_4all(string connstr, string Query)
    //{
    //    try
    //    {
    //        DataTable dt = SqlHelper.ExecuteDataset(connstr, CommandType.Text, Query).Tables[0];
    //        strConn = connstr;
    //        return dt;
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}

    //public static bool ExecuteNonQuery_4all(string connstr, string Query)
    //{
    //    try
    //    {
    //        bool strAns = Convert.ToBoolean(SqlHelper.ExecuteNonQuery(connstr, CommandType.Text, Query));
    //        strConn = connstr;
    //        return strAns;
    //    }
    //    catch (Exception)
    //    {
    //        return false;
    //    }
    //}

    public static DataTable ExecuteQueryDB(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());

        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }

    public static DataTable ExecuteQueryDBg(string Query, string DBname)
    {
        string strConn = BLL.Decrypt(DBname);

        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }




    public static bool ExecuteNonQueryDB1(string Query, string DBName)
    {
        //string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
        string strConn = BLL.Decrypt(DBName);
        // strConn = strConn.Replace("Initial Catalog=" + MasterDB + "", "Initial Catalog=" +BLL.Decrypt(DBName));
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }


    public static bool ExecuteNonQueryDB2(string Query, string DBName)
    {
        string strConn = BLL.Decrypt(DBName);
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }

    public static DataTable ExecuteQueryDB1(string Query, string DBName)
    {
        string strConn = BLL.Decrypt(DBName);


        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    public static DataTable ExecuteQueryDBNEW(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
      //  string strConn = BLL.Decrypt(DBName);


        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }

    public static bool ExecuteNonQueryDB(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());

        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }


    public static DataSet ExecuteQueryDataset(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString());
        try
        {
            if (HttpContext.Current.Session["USERINFO"] != null)
            {
                string DBName = BLL.Decrypt(((SMVTS_USERS)(HttpContext.Current.Session["USERINFO"])).USERS_DBNAME);
                strConn = (DBName);
            }
            //_obj.STRSQL = Convert.ToString(Query).Replace("'","''");
            //_obj.STARTDATE = System.DateTime.Now;
            //return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            DataSet dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query);
            //_obj.ENDDATE = System.DateTime.Now;
            //_obj.USERID = "0";
            //_obj.OPERATION = operation.Insert;
            //BLL.set_TraceLog_ErrorLog(_obj);


            return dt;
        }
        catch (Exception)
        {
            //_obj.ERR_DESC = Convert.ToString(Query).Replace("'", "''");
            //_obj.STARTDATE = System.DateTime.Now;
            //_obj.ERR_DESC = ex.Message;
            //  DataTable dt = SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
            //_obj.ENDDATE = System.DateTime.Now;
            //_obj.USERID = "0";
            //_obj.OPERATION = operation.Update;
            //BLL.set_TraceLog_ErrorLog(_obj);
            return null;
        }
    }

    internal static bool ExecuteNonQuery_MTS(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_mts"].ToString());
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }
    //created by Ajith for ttraccar
    public static DataTable ExecuteQueryFortc(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_tc"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }
    internal static bool ExecuteNonQuerytc(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_tc"].ToString());
        return Convert.ToBoolean(SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, Query));
    }

    public static DataTable ExecuteQuery_ParentsMobileDetails(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }

    public static DataTable FetchVehicleDetailsByPhone(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }

    public static DataTable FetchVehicleTripDataDetails(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }

    public static DataTable FetchParentTicketStatus1(string Query)
    {
        string strConn = BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection_prod"].ToString());
        return SqlHelper.ExecuteDataset(strConn, CommandType.Text, Query).Tables[0];
    }


}