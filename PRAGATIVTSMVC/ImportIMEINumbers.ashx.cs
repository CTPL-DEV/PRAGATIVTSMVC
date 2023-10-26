using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using OfficeOpenXml;
using System.Data;
using PRAGATIVTSMVC.ExcelExtensions;
using System.Configuration;

namespace PRAGATIVTSMVC
{
    /// <summary>
    /// Summary description for ImportIMEINumbers
    /// </summary>
    public class ImportIMEINumbers : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            var categ_id = context.Request.Form["CATEG_ID"];
           
            string status = "";


            HttpFileCollection files = context.Request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                try
                {


                    var excel = new ExcelPackage(file.InputStream);
                    DataTable dt = new DataTable();
                    dt = excel.ToDataTable();

                    var duplicates = dt.AsEnumerable()
                  .GroupBy(r => r["IMEI"])//Using Column Name
                  .Where(gr => gr.Count() > 1)
                  .Select(g => g.Key);
                    string[] arr = new string[1000];
                    int j = 0;
                    foreach (var d in duplicates)
                    {

                        arr[j++] = d.ToString();
                    }
                    if (arr[0] != null)
                    {
                        string plotnos = "";
                        for (int k = 0; k < arr.Length; k++)
                        {
                            if (arr[k] != null)
                            {
                                plotnos = plotnos + ',' + arr[k];
                            }
                        }
                        status = "File Contains Duplicate IMEI No.s " + plotnos + "";
                    }
                    else
                    {

                        DataColumn DEVICEID = new DataColumn();
                        DEVICEID.ColumnName = "DEVICEID";
                        DEVICEID.DefaultValue = "";
                        dt.Columns.Add(DEVICEID);

                        DataColumn CATEGID = new DataColumn();
                        CATEGID.ColumnName = "CATEG_ID";
                        CATEGID.DefaultValue = categ_id;
                        dt.Columns.Add(CATEGID);

                        int maxid = 0;
                        DataTable dt_last = BLL.getLastDeviceID();
                        if(dt_last.Rows.Count>0)
                        {
                           if(dt_last.Rows[0][0].ToString()!="")
                            {
                                maxid = Convert.ToInt32(dt_last.Rows[0][0]);
                            }
                        }
                        int length = dt.Rows.Count;
                        for(int m=0;m<dt.Rows.Count;m++)
                        {
                            dt.Rows[m]["DEVICEID"] = maxid + 1;
                            maxid= maxid + 1;
                        }

                       
                        var table = "smvts_tt_devid";
                        using (var conn = new SqlConnection(BLL.Decrypt(ConfigurationManager.ConnectionStrings["connection"].ToString())))
                        {
                            DataTable dt_old = new DataTable();
                            var bulkCopy = new SqlBulkCopy(conn);
                            bulkCopy.DestinationTableName = table;
                            conn.Open();
                            var schema = conn.GetSchema("Columns", new[] { null, null, table, null });
                            foreach (DataColumn sourceColumn in dt.Columns)
                            {
                                foreach (DataRow row in schema.Rows)
                                {
                                    if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                                    {
                                        bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                                        break;
                                    }
                                }
                            }
                            //bool status1 = BLL.deleteplots(projectcode);
                            bulkCopy.WriteToServer(dt);
                            status = "File Uploaded Successfully";
                        }


                    }
                }
                catch (Exception ex)
                {
                    status = "Failed to Upload File";
                }

            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(status);
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