using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UMTD.Models;

namespace UMTD.Controllers
{
    public class MethodController : ApiController
    {
        private UMTDEntities dbContext = new UMTDEntities();
        #region Method
        /// <summary>
        /// Return list of all available research Methods
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <returns>HttpStatusCode.OK and Method list in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List(string userKey)
        {
            try
            {
                List<prcMethodList_Result> MethodList = (from s in dbContext.prcMethodList(userKey)
                                                         select s).ToList();
                return Request.CreateResponse<IEnumerable<prcMethodList_Result>>(HttpStatusCode.OK, MethodList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        #endregion
    }
}
