using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UMTD.Models;

namespace UMTD.Controllers
{
    public class LanguageController : ApiController
    {
        private UMTDEntities dbContext = new UMTDEntities();
        /// <summary>
        /// Return list of all available Languages
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <returns>HttpStatusCode.OK and Material list in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List(string userKey)
        {
            try
            {
                List<prcLanguageList_Result> LanguageList = (from s in dbContext.prcLanguageList()
                                                         select s).ToList();
                return Request.CreateResponse<IEnumerable<prcLanguageList_Result>>(HttpStatusCode.OK, LanguageList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }        

    }
}
