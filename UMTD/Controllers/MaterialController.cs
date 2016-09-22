using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UMTD.Models;

namespace UMTD.Controllers
{
    public class MaterialController : ApiController
    {
        private UMTDEntities dbContext = new UMTDEntities();
        #region Material
        /// <summary>
        /// Return list of all available Materials
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <returns>HttpStatusCode.OK and Material list in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List(string userKey)
        {
            try
            {
                List<prcMaterialList_Result> MaterialList = (from s in dbContext.prcMaterialList(userKey)
                                                             select s).ToList();
                return Request.CreateResponse<IEnumerable<prcMaterialList_Result>>(HttpStatusCode.OK, MaterialList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        #endregion  
    }
}
