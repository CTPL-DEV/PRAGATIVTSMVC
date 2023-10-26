using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.Data;
using PRAGATIVTSMVC.ExcelExtensions;
using System.Configuration;

namespace PRAGATIVTSMVC
{
    /// <summary>
    /// Summary description for ImportSIMS
    /// </summary>
    public class ImportSIMS : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //var categ_id = context.Request.Form["CATEG_ID"];

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
                  .GroupBy(r => r["SIM_NUMBER"])//Using Column Name
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
                        status = "File Contains Duplicate SIM No.s " + plotnos + "";
                    }
                    else
                    {



                        DataColumn created_date = new DataColumn();
                        created_date.ColumnName = "CREATEDDATE";
                        created_date.DefaultValue = DateTime.Now;
                        dt.Columns.Add(created_date);

                        DataColumn created_by = new DataColumn();
                        created_by.ColumnName = "CREATEDBY";
                        created_by.DefaultValue = 1;
                        dt.Columns.Add(created_by);

                        DataColumn sim_status = new DataColumn();
                        sim_status.ColumnName = "SIM_STATUS";
                        sim_status.DefaultValue = 0;
                        dt.Columns.Add(sim_status);

                        DataColumn sim_lock = new DataColumn();
                        sim_lock.ColumnName = "SIM_LOCK";
                        sim_lock.DefaultValue = 0;
                        dt.Columns.Add(sim_lock);

                        //int maxid = 0;
                        //DataTable dt_last = BLL.getLastDeviceID();
                        //if (dt_last.Rows.Count > 0)
                        //{
                        //    if (dt_last.Rows[0][0].ToString() != "")
                        //    {
                        //        maxid = Convert.ToInt32(dt_last.Rows[0][0]);
                        //    }
                        //}
                        //int length = dt.Rows.Count;
                        //for (int m = 0; m < dt.Rows.Count; m++)
                        //{
                        //    dt.Rows[m]["DEVICEID"] = maxid + 1;
                        //    maxid = maxid + 1;
                        //}


                        var table = "SMVTS_ALL_SIMS";
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