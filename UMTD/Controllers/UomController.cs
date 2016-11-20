using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UMTD.Models;

namespace UMTD.Controllers
{
    public class UomController : ApiController
    {
        private UMTDEntities dbContext = new UMTDEntities();
        #region Uom
        /// <summary>
        /// Return list of all available unit of measurement
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <returns>HttpStatusCode.OK and Uom list in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List(string userKey)
        {
            try
            {
                //TODO: error translations and maybe separate function
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.Host).FirstOrDefault().Value)
                    throw new Exception("UserKey is incorrect or used with wrong IP address");
                List<prcUomList_Result> UomList = (from s in dbContext.prcUomList(userKey)
                                                   select s).ToList();
                return Request.CreateResponse<IEnumerable<prcUomList_Result>>(HttpStatusCode.OK, UomList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }        
        #endregion 
    }
}
