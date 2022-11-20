using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SnacksShop.App_Start
{
    public class MyAuthentication
    {

        public static void SetCookie(string UserName,string UserID, string Rights)
        {
            String UserDate = UserID + "#" + Rights;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, UserName, DateTime.Now, DateTime.Now.AddMinutes(60), false, UserDate);
            string enyTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, enyTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);

        }


        public static bool IsLoging()
        {
            return HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null;
        }


        public static void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public static string GetUserName()
        {
            if (IsLoging())
            {
                HttpCookie authCooke = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCooke.Value);
                return authTicket.Name;
            }
            else
            {
                return "";
            }
        }


        public static string GetUserId()
        {
            if (IsLoging())
            {
                HttpCookie authCooke = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCooke.Value);
                string[] UserData = authTicket.UserData.Split('#');
                if (UserData.Length > 0)
                {
                    return UserData[0].ToString();
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        public static string GetRights()
        {
            if (IsLoging())
            {
                HttpCookie authCook = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCook.Value);
                string[] UserData = authTicket.UserData.Split('#');
                if (UserData.Length > 1)
                {
                    return UserData[1].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}