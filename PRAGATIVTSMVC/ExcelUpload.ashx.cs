using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PRAGATIVTSMVC.ExcelToTable;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;

namespace PRAGATIVTSMVC
{
    /// <summary>
    /// Summary description for ExcelUpload
    /// </summary>
    public class ExcelUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

           
           // HttpContext.Current.Session.Remove("DTEXCEL");
            // string paths = AppDomain.CurrentDomain.BaseDirectory.ToString();
            //    System.IO.StreamWriter file = new System.IO.StreamWriter(paths + "\\dashboardLOG.txt", true);

            System.Data.DataTable dt_excel = new System.Data.DataTable();

            DataColumn col_vno = new DataColumn("VEHICLES_REGNUMBER", typeof(string));
            DataColumn col_start = new DataColumn("STARTDATE", typeof(string));
            DataColumn col_end = new DataColumn("ENDATE", typeof(string));
            DataColumn col_from = new DataColumn("From", typeof(string));
            DataColumn col_to = new DataColumn("To", typeof(string));
            DataColumn col_via = new DataColumn("Via", typeof(string));
            DataColumn col_dist = new DataColumn("Distance", typeof(string));
            DataColumn col_remark = new DataColumn("Remark", typeof(string));

            dt_excel.Columns.Add(col_vno);
            dt_excel.Columns.Add(col_start);
            dt_excel.Columns.Add(col_end);
            dt_excel.Columns.Add(col_from);
            dt_excel.Columns.Add(col_to);
            dt_excel.Columns.Add(col_via);
            dt_excel.Columns.Add(col_dist);
            dt_excel.Columns.Add(col_remark);
            // rgrd_ExcelUpload.DataSource = dt_excel;
            //  rgrd_ExcelUpload.DataBind();
            //context.Session["DTEXCEL"] = dt_excel;
            HttpFileCollection files = context.Request.Files;
            string status = "";
            for (int j = 0; j < files.Count; j++)
            {
                HttpPostedFile file = files[j];
                try
                {


                    string extension = Path.GetExtension(file.FileName);
                    //  string excelPath = Path.GetFileName(excelupload.PostedFile.FileName).Replace(" ", String.Empty);
                    //  excelupload.SaveAs(Server.MapPath("~/Master/ExcelUploads/") + excelPath);
                    string filename = Path.GetFileName(file.FileName);
                    string fileTemp = ((DateTime.Now.ToString()) + "_" + filename).Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", "");
                    string savefile= context.Server.MapPath(@"~/Master/ExcelUploads/" + fileTemp);
                    file.SaveAs(savefile);
                    // string excelPath = Server.MapPath("~/ExcelUploads/") + Path.GetFileName(excelupload.PostedFile.FileName);
                    // excelupload.SaveAs(excelPath);
                    OleDbConnection conString = null;
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        switch (extension)
                        {
                            case ".xls": //Excel 97-03
                                         //conString = ConfigurationManager.ConnectionStrings["Exel03ConString"].ConnectionString;
                                         //conString = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=0\"");
                                conString = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + context.Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=0\"");
                                break;
                            case ".xlsx": //Excel 07 or higher
                                          //conString = ConfigurationManager.ConnectionStrings["Exel07plusConString"].ConnectionString;
                                          // conString = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"");
                                conString = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + context.Server.MapPath("~\\Master\\ExcelUploads\\" + fileTemp) + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"");
                                break;

                        }
                        //conString = string.Format(conString, excelPath);
                        //using (OleDbConnection excel_con = new OleDbConnection(conString))
                        //{
                        //excel_con.Open();
                        //string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["SMVTS_ROUTE_BUDGET "].ToString();
                        System.Data.DataTable dtExcelData = new System.Data.DataTable();


                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [Current Status$]", conString))
                        {
                            oda.Fill(dtExcelData);
                        }
                        //excel_con.Close();
                        if (dtExcelData.Rows.Count > 0)
                        {
                            System.Data.DataTable dt_data = new System.Data.DataTable();
                            string conn = ((SMVTS_USERS)context.Session["USERINFO"]).USERS_DBNAME;
                            bool result = false;
                            //int count;
                            //count = BLL.insertexcel(dt);
                            //count = BLL.insertexcel(dtExcelData);
                            for (int i = 3; i < dtExcelData.Rows.Count; i++)
                            {
                                //pass all parameters to above procedures
                                try
                                {
                                    string querry = "EXEC USP_ER_EXCELUPLOADEXP @ER_CATEGID=" + ((SMVTS_USERS)context.Session["USERINFO"]).USERS_CATEGORY_ID + ",@OPERATION='EXCELUPLOAD',@ER_VEHICLENO='" + dtExcelData.Rows[i][1] + "',@ER_VEHICLETYPE='" + dtExcelData.Rows[i][2] + "',@ER_BOOKBRANCH='" + dtExcelData.Rows[i][3] + "',@ER_BOOKZONE='" + dtExcelData.Rows[i][4] + "',@ER_DISPATCHDATE='" + dtExcelData.Rows[i][5] + "',@ER_FROM='" + dtExcelData.Rows[i][6] + "',@ER_TO='" + dtExcelData.Rows[i][7] + "',@ER_PARTYNAME='" + dtExcelData.Rows[i][8] + "',@ER_DELIVERYBRANCH='" + dtExcelData.Rows[i][10] + "',@ER_TO_ZONE='" + dtExcelData.Rows[i][11] + "',@ER_EXPECTED_DATE='" + dtExcelData.Rows[i][12] + "',@ER_REPORTING_DATE='" + dtExcelData.Rows[i][13] + "',@ER_CURRENT_STATUS='" + dtExcelData.Rows[i][15] + "',@ER_LOCATION='" + dtExcelData.Rows[i][16] + "',@ER_ACK_STATUS='" + dtExcelData.Rows[i][17] + "',@ER_DRIVER_NAME='" + dtExcelData.Rows[i][18] + "',@ER_DRIVER_PHONE='" + dtExcelData.Rows[i][19] + "',@ER_CNTR_BRANCH='" + dtExcelData.Rows[i][20] + "',@ER_FORMAN_DETAILS='" + dtExcelData.Rows[i][21] + "',@ER_DEST_CONTROL_BRANCH='" + dtExcelData.Rows[i][22] + "',@ER_FORMAN_BRANCH='" + dtExcelData.Rows[i][23] + "'";
                                    // string querry = "EXEC USP_ER_EXCELUPLOAD1 @ER_CATEGID=" + ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID + ",@OPERATION='operation',@ER_ID=" + dt1.Rows[i][0] + ",@ER_VEHICLENO='" + dt1.Rows[i][1] + "',@ER_VEHICLETYPE='" + dt1.Rows[i][2] + "',@ER_BOOKBRANCH='" + dt1.Rows[i][3] + "',@ER_BOOKZONE='" + dt1.Rows[i][4] + "',@ER_DISPATCHDATE='" + dt1.Rows[i][5] + "',@ER_FROM='" + dt1.Rows[i][6] + "',@ER_TO='" + dt1.Rows[i][7] + "',@ER_PARTYNAME='" + dt1.Rows[i][8] + "',@ER_DELIVERYBRANCH='" + dt1.Rows[i][9] + "',@ER_TO_ZONE='" + dt1.Rows[i][10] + "',@ER_EXPECTED_DATE='" + dt1.Rows[i][11] + "',@ER_REPORTING_DATE='" + dt1.Rows[i][12] + "',@ER_CURRENT_DATE='" + dt1.Rows[i][12] + "',@ER_CURRENT_STATUS='" + dt1.Rows[i][13] + "',@ER_LOCATION='" + dt1.Rows[i][14] + "',@ER_ACK_STATUS='" + dt1.Rows[i][15] + "',@ER_DRIVER_NAME='" + dt1.Rows[i][16] + "',@ER_DRIVER_PHONE='" + dt1.Rows[i][17] + "',@ER_CNTR_BRANCH='" + dt1.Rows[i][18] + "',@ER_FORMAN_DETAILS='" + dt1.Rows[i][19] + "',@ER_DEST_CONTROL_BRANCH='" + dt1.Rows[i][20] + "',@ER_FORMAN_BRANCH='" + dt1.Rows[i][21] + "',@DATE='" + dt1.Rows[i][22] + "',@ER_CHALLON_NO='" + dt1.Rows[i][23] + "',@ER_REMARKS='" + dt1.Rows[i][24] + "',@ER3='" + dt1.Rows[i][25] + "',@ER4='" + dt1.Rows[i][26] + "',@ER5='" + dt1.Rows[i][27] + "'";
                                    result = Dal.ExecuteNonQueryDB2(querry, conn);
                                }
                                catch (Exception ex)
                                {
                                    //BLL.ShowMessage(this, "" + dt1.Rows[i][0] + "");
                                }

                            }
                            if (result == true)
                            {
                                System.Data.DataTable dt_RouteDetails = BLL.ExecuteQuery("SELECT * FROM SMVTS_ER_TRIPINFO(NOLOCK) WHERE ER_CATEGID='" + ((SMVTS_USERS)context.Session["USERINFO"]).USERS_CATEGORY_ID + "'");
                                foreach (DataRow dr in dt_RouteDetails.Rows)
                                {


                                    try
                                    {

                                       // ASSIGNDRIVER(dr[1].ToString(), dr[17].ToString(), dr[18].ToString(), dr[20].ToString());


                                        //checking Startdate and enddate.. if missing.. continue.. the loop
                                        if ((dr[5].ToString() == string.Empty))
                                        {
                                           System.Data.DataTable dt_NoStartEndDate = (System.Data.DataTable)context.Session["DTEXCEL"];
                                            DataRow dr_err = dt_NoStartEndDate.NewRow();
                                            dr_err[0] = dr[1].ToString();
                                            dr_err[1] = dr[5].ToString();
                                            dr_err[2] = dr[11].ToString();
                                            dr_err[3] = dr[6].ToString();
                                            dr_err[4] = dr[7].ToString();
                                            dr_err[5] = "";
                                            dr_err[6] = "";
                                            dr_err[7] = "Start/End Date missing or is in wrong format";

                                            dt_NoStartEndDate.Rows.Add(dr_err);
                                            //ViewState["DTEXCEL"] = dt_NoStartEndDate;

                                        }

                                        #region "Checking"

                                        else if (dr[11].ToString() != string.Empty)
                                        {
                                            if (Convert.ToDateTime(dr[5].ToString().Replace("-", "/")) < Convert.ToDateTime(dr[11].ToString().Replace("-", "/")))
                                            {
                                             //  Plan_VehicleRoute_WithConsignment(dr);
                                            }
                                            else
                                            {
                                                System.Data.DataTable dt_errors = (System.Data.DataTable)context.Session["DTEXCEL"];
                                                DataRow dr_err = dt_errors.NewRow();
                                                dr_err[0] = dr[1].ToString();
                                                dr_err[1] = dr[5].ToString();
                                                dr_err[2] = dr[11].ToString();
                                                dr_err[3] = dr[6].ToString();
                                                dr_err[4] = dr[7].ToString();
                                                dr_err[5] = "";
                                                dr_err[6] = "";
                                                dr_err[7] = "Start Date Greater than End date";

                                                dt_errors.Rows.Add(dr_err);
                                                HttpContext.Current.Session["DTEXCEL"] = dt_errors;
                                            }
                                        }
                                        #endregion
                                        #region "EnDDate"
                                        else
                                        {
                                            System.Data.DataTable dt_errors = (System.Data.DataTable)context.Session["DTEXCEL"];
                                            DataRow dr_err = dt_errors.NewRow();
                                            dr_err[0] = dr[1].ToString();
                                            dr_err[1] = dr[5].ToString();
                                            dr_err[2] = dr[11].ToString();
                                            dr_err[3] = dr[6].ToString();
                                            dr_err[4] = dr[7].ToString();
                                            dr_err[5] = "";
                                            dr_err[6] = "";
                                            dr_err[7] = "End Date is Empty";

                                            dt_errors.Rows.Add(dr_err);
                                            HttpContext.Current.Session["DTEXCEL"] = dt_errors;
                                        }
                                        #endregion


                                    }
                                    catch { }
                                }
                              //  BLL.ShowMessage(this, "Excel uploaded successfully");
                                status = "Excel uploaded successfully";
                            }
                            else
                            {
                                //BLL.ShowMessage(this, "Failed to upload Excel");
                                status = "Failed to upload Excel";
                            }


                        }
                        else
                        {
                            //BLL.ShowMessage(this, "Excel file is empty cannot be uploaded.");
                            status = "Excel file is empty cannot be uploaded.";
                        }
                        //}
                    }
                    else
                    {
                      //  BLL.ShowMessage(this, "Selected file is not an Excel file.Please select Excel File.");
                        status = "Selected file is not an Excel file.Please select Excel File.";
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