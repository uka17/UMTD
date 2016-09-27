using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Security;
using UMTD.Models;

namespace UMTD.Controllers
{
    public class UserController : ApiController
    {
        private UMTDEntities dbContext = new UMTDEntities();
        #region User
        /// <summary>
        /// Check if email-password pair is valid
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password hash</param>
        /// <returns>HttpStatusCode.OK and User model in JSON format in case of success, 
        /// HttpStatusCode.Forbidden and error message in case if login failed,
        /// HttpStatusCode.InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("Login")]
        public HttpResponseMessage Login(string email, string password, bool remember)
        {
            try
            {
                int NumberOfUsers = (from s in dbContext.prcUserCheck(email, password)
                                     select s.Value).FirstOrDefault();

                prcUserSelect_Result SelectedUser = null;

                if (NumberOfUsers == 0)
                {
                    //TODO: resolve translations
                    return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "Incorrect email or password");
                }
                else
                {
                    SelectedUser = (from s in dbContext.prcUserSelect(email)
                                    select s).FirstOrDefault();
                    HttpResponseMessage Response = Request.CreateResponse<prcUserSelect_Result>(HttpStatusCode.OK, SelectedUser);
                    Response.Headers.AddCookies(Authorize(email, password, remember));
                    return Response;
                }               
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }

        }
        private CookieHeaderValue[] Authorize(string email, string password, bool remember)
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


            var cookie = new CookieHeaderValue("UMTD", encTicket);
            cookie.Expires = DateTimeOffset.Now.AddDays(CookieExpiration); 
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/"; 
            return new CookieHeaderValue[] { cookie };
        }
        #endregion
    }
}
