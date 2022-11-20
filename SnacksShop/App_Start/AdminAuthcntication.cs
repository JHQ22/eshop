using SnacksShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SnacksShop
{
    public class AdminAuthcntication:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (MyAuthentication.IsLoging())
            {
                if (MyAuthentication.GetRights() != "admin")
                {
                    HttpContext.Current.Response.Redirect("~/Admin/Login", true);
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Admin/Login", true);
            }
        }
    }
}