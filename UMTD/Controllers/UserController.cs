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
        private string userData = "bite_me";
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
        /// <summary>
        /// Return user profiel by email
        /// </summary>
        /// <param name="email">User email which will be used as ID</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Profile")]
        public HttpResponseMessage Profile(string email)
        {
            try
            {
                prcUserSelect_Result SelectedUser = (from s in dbContext.prcUserSelect(email)
                                                     select s).FirstOrDefault();

                if (SelectedUser == null)
                {
                    //TODO: resolve translations
                    return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "Incorrect email or password");
                }
                else
                {                    
                    return Request.CreateResponse<prcUserSelect_Result>(HttpStatusCode.OK, SelectedUser); 
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }

        }
        /// <summary>
        /// Refresh API key
        /// </summary>
        /// <param name="apiKey">Current key value</param>
        /// <returns>New API key value or NULL in case if apiKey was not found</returns>
        [HttpGet]
        [ActionName("RefreshApiKey")]
        public HttpResponseMessage RefreshApiKey(string apiKey)
        {
            try
            {
                if(!CheckKeyOwner(apiKey))
                {
                    //TODO: resolve translations
                    throw new Exception("ApiKey ownership check failed");
                }

                string NewKey = (from s in dbContext.prcKeyRefresh(apiKey) select s).FirstOrDefault();
                
                if (NewKey == null)
                {
                    //TODO: resolve translations
                    throw new Exception ("ApiKey was not updated");
                }
                else
                {
                    return Request.CreateResponse<string>(HttpStatusCode.OK, NewKey);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }

        }
        /// <summary>
        /// Authorize user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="remember">In case if true user cookie will be saved</param>
        /// <returns></returns>
        private CookieHeaderValue[] Authorize(string email, string password, bool remember)
        {
            try
            {
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
            catch (Exception exc)
            {
                //TODO Logging
                //uLog.PutException(exc, "uController.OnActionExecuting");
                throw exc;
            }
}
        /// <summary>
        /// Check if key belongs to currently authorized user
        /// </summary>
        /// <param name="userKey">Key values</param>
        /// <returns></returns>
        private bool CheckKeyOwner(string userKey)
        {
            try
            {
                bool Result = false;
                CookieHeaderValue cookie = Request.Headers.GetCookies("UMTD").FirstOrDefault();

                if (cookie["UMTD"] != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie["UMTD"].Value);

                    if (ticket.Name != null)
                    {
                        Result = (from s in dbContext.prcKeyCheckOwner(userKey, ticket.Name, Request.RequestUri.Host)
                                      select s.Value).FirstOrDefault();
                    }
                }
                    
                return Result;
            }
            catch (Exception exc)
            {
                //TODO Logging
                //uLog.PutException(exc, "uController.OnActionExecuting");
                throw exc;
            }
        }
        #endregion
    }
}
