using PRAGATIVTS_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PRAGATIVTSMVC.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
         
        }

        public void PAGE_PERMISSIONS()
        {

            try
            {
             
             //   string CATEGID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
              //  string USERID = Convert.ToString(((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID);
                string CATEGID = "29";
                BLL OBJ = new BLL();

             
                ViewBag.panels1 = OBJ.GETALL_PANELS();
                //    var a = OBJ.PERMISSIONS_PAGENAME(Convert.ToInt32(CATEGID), PAGE_NAME);
              //  ViewBag.panels1 = OBJ.GETALL_PANELS();
                ViewBag.permissions = OBJ.GETALL_PAGE_PERMISSIONS(Convert.ToInt32(CATEGID));
                //if (a.Count > 0)
                //{

                //    if (a[0].PP_VIEW == 1)
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                //else
                //{

                //    }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
         

           
        }



        public void checkcategoryid()
        {
            try
            {
                ViewBag.laycateg_id = ((SMVTS_USERS)Session["USERINFO"]).USERS_CATEGORY_ID;
            }
            catch
            {

            }
            
        }

        public class SessionAuthorizeAttribute : AuthorizeAttribute
        {
            
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                //  return httpContext.Session["USERID"] != null; 
                return httpContext.Session["USERINFO"] != null;
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }





     

    }
}