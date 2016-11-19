using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Security;
using UMTD.Models;
using UMTD.Classes;

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
                prcUserSelect_Result SelectedUser = null;

                if (!Verify(email, password))
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
        /// Remove cookie from response
        /// </summary>
        /// <returns>HttpStatusCode.OK in case if logout sucesfull, 
        /// HttpStatusCode.InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("Logout")]
        public HttpResponseMessage Logout()
        {
            try
            {
                var cookie = new CookieHeaderValue("UMTD", "Logout");
                cookie.Expires = DateTimeOffset.Now.AddDays(-1);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";                
                HttpResponseMessage Response = Request.CreateResponse<string>(HttpStatusCode.OK, "Logout complet");
                Response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                return Response;
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
                if (GetCurrentUserID() != email)
                    //TODO Translation
                    throw new Exception("Something wrong with user profile");

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
        [HttpGet]
        [ActionName("ProfileUpdate")]
        public HttpResponseMessage ProfileUpdate(
                int id,
                string name,
                string email,
	            int languageId,
                bool isLinked,
	            string domain,
                string newPassword,
                string oldPassword
            )
        {
            try
            {
                string UserCurrentEmail = (from s in dbContext.prcUserSelectEmail(id)
                                           select s).FirstOrDefault();
                if (GetCurrentUserID() != UserCurrentEmail)
                    //TODO Translation
                    throw new Exception("Something wrong with user profile update");

                prcUserSelect_Result SelectedUser = (from s in dbContext.prcUserSelect(UserCurrentEmail)
                                                     select s).FirstOrDefault();
                if (newPassword != null)
                {
                    
                    if (!Verify(email, oldPassword))
                        //TODO Translation
                        throw new Exception("Incorrect old password");
                    uMD5 newPassworduMD5 = new uMD5(newPassword);
                    newPassword = newPassworduMD5.GetMd5Hash();
                }

                dbContext.prcUserUpdate(
                    id,
                    name,
                    email,
                    newPassword,
                    languageId,
                    isLinked,
                    domain,
                    email
                    );
                
                if(UserCurrentEmail != email)
                {
                    //User has changed email
                    Logout();
                }
                return Request.CreateResponse<string>(HttpStatusCode.OK, "Profile was updated");


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

                string UserId = GetCurrentUserID();

                if (UserId != null)
                {

                    Result = (from s in dbContext.prcKeyCheckOwner(userKey, UserId, Request.RequestUri.Host)
                                    select s.Value).FirstOrDefault();
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
        /// <summary>
        /// Obtain email of signed in user
        /// </summary>
        /// <returns>User id (email)</returns>
        private string GetCurrentUserID()
        {
            string Result = null;

            CookieHeaderValue cookie = Request.Headers.GetCookies("UMTD").FirstOrDefault();

            if (cookie["UMTD"] != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie["UMTD"].Value);

                Result = ticket.Name;
            }

            return Result;
        }
        /// <summary>
        /// Verify if email-password pair is correct
        /// </summary>
        /// <param name="email">Email value</param>
        /// <param name="password">Password values</param>
        /// <returns>True in case if pair is correct and false in case if incorrect</returns>
        private bool Verify(string email, string password)
        {
            //TODO delay for query due to avoid brootforce
            try
            {
                uMD5 pass = new uMD5(password);
                bool Result = (from s in dbContext.prcUserCheck(email, pass.GetMd5Hash())
                            select s.Value).FirstOrDefault();

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
