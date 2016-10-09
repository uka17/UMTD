using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UMTD.Models;

namespace UMTD
{
    public class uController : Controller
    {
        private UMTDEntities dbContext = new UMTDEntities();
        private prcUserSelect_Result  user = null;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {

                HttpCookie cookie = HttpContext.Request.Cookies.Get("UMTD");
                //cookie reading
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    if (ticket.Name != null)
                    {
                        user = (from s in dbContext.prcUserSelect(ticket.Name)
                                select s).FirstOrDefault();
                        System.Web.HttpContext.Current.Session["user"] = user;
                    }
                }
                else
                    System.Web.HttpContext.Current.Session["user"] = null;


            }
            catch (Exception exc)
            {
                //uLog.PutException(exc, "uController.OnActionExecuting");
                throw exc;
            }
        }
        /*
        /// <summary>
        /// Authorize user and creates cookie for authorization in case if remember option is true
        /// </summary>
        /// <param name="email">Email of authorized user</param>
        /// <param name="remember">Authorization should be saved in case if true</param>
        public void Authorize(string email, Boolean remember)
        {
            try
            {
                string userData = "bite_me";
                int CookieExpiration = Convert.ToInt32((from s in dbContext.prcSettingSelect("CookieExpiration")
                                                        select s).FirstOrDefault());
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    email,
                    DateTime.Now,
                    DateTime.Now.AddDays(CookieExpiration),
                    remember,
                    userData);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                HttpCookie cookie = new HttpCookie("UMTD", encTicket);
                cookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cookie);
            }
            catch (Exception exc)
            {
                //uLog.PutException(exc, "uController.Authorize");
                throw exc;
            }
        }
        */
    }
}