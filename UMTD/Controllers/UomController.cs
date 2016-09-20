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
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List()
        {
            try
            {
                List<prcUomList_Result> UomList = (from s in dbContext.prcUomList(1)
                                                   select s).ToList();
                return Request.CreateResponse<IEnumerable<prcUomList_Result>>(HttpStatusCode.OK, UomList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public HttpResponseMessage Delete(int testId, int uomId)
        {
            try
            {
                dbContext.prcTestUomDelete(testId, uomId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ActionName("Insert")]
        public HttpResponseMessage UomInsert(int testId, int uomId)
        {
            try
            {
                dbContext.prcTestUomInsert(testId, uomId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        #endregion 
    }
}
