using SnacksShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SnacksShop
{
    public class UserAuthentication: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (MyAuthentication.IsLoging())
            {
                if (MyAuthentication.GetRights() != "user")
                {
                    HttpContext.Current.Response.Redirect("~/Home/Login", true);
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Home/Login", true);
            }
        }
    }
}