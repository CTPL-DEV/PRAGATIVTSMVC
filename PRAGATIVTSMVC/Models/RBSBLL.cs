using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
namespace  PRAGATIVTS_MVC.Models
{
    public class RBSBLL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CONN"].ConnectionString);
        List<Locations1> obj = new List<Locations1>();
        public IEnumerable<Locations1> SaveLocations(string District, string Revenuedivision, string Mandal, string Village, string InstituteName, string MobileNumber, string Latitude, string Longitude, string ImagePath, string Remarks, string CreatedBy)
        {
            try
            {
                string url = LoadImage(ImagePath);
                //insert into RBS_Villages(District,Revenuedivision,Mandal,Village,InstituteName,MobileNumber,Latitude,Longitude,ImagePath,Remarks,CreatedBy,CreatedDate)
                //values(@District, @Revenuedivision, @Mandal, @Village, @InstituteName, @MobileNumber, @Latitude, @Longitude, @ImagePath, @Remarks, @CreatedBy, getdate());
                SqlCommand cmd = new SqlCommand("insert into RBS_Villages(District,Revenuedivision,Mandal,Village,InstituteName,MobileNumber,Latitude,Longitude,ImagePath,Remarks,CreatedBy,CreatedDate) values('" + District + "','" + Revenuedivision + "','" + Mandal + "','" + Village + "','" + InstituteName + "','" + MobileNumber + "','" + Latitude + "','" + Longitude + "','" + ImagePath + "','" + Remarks + "','" + CreatedBy + "','getdate()'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                
                obj.Add(new Locations1
                {
                    Message = "True",
                });
            }
            catch (Exception ex)
            {
                obj.Add(new Locations1
                {
                    Message = "Fail",
                });
            }
            finally
            {
                con.Close();
            }

            return obj;
        }

        public string LoadImage(string Code)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            string url = "";
            try
            {

                byte[] bytes = Convert.FromBase64String(Code);

                url = convertbyte(bytes);
            }
            catch (Exception ex)
            {
                url = "Failed";
            }


            return url;
        }

        public string convertbyte(byte[] f)
        {

            string fname = string.Empty;
            MemoryStream ms = new MemoryStream(f);
            fname = DateTime.Now.ToString("MMMddyyyyHHmmss") + ".png";
            // fname = Tripdetailedid + ".jpg";
            FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
                        ("~/MOBILEAPP_IMGS/") + fname, FileMode.Create);
            ms.WriteTo(fs);
            // clean up 
            ms.Close();
            fs.Close();
            fs.Dispose();
            string url = "http://192.168.50.56:86/MOBILEAPP_IMGS/" + fname;

            return url;
        }

        public List<Locations1> GetDistricts()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct(Data_DistrictName) from RBS_STUDENT_DATA", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj.Add(new Locations1
                    {
                        DistrictName = dr["Data_DistrictName"].ToString(),
                        Message = "True",
                    });
                }
                dr.Close();
            }
            catch(Exception ex)
            {
                obj.Add(new Locations1
                {
                    Message = "Failed",
                });
            }
            finally
            {
                con.Close();
            }
            return obj;
          
        }

    }
    public class Locations1
    {
        public string  Message{ get; set; }
        public string  DistrictName { get; set; }
    }
}