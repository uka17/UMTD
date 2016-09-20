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
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List()
        {
            try
            {
                List<prcMaterialList_Result> MaterialList = (from s in dbContext.prcMaterialList(1)
                                                             select s).ToList();
                return Request.CreateResponse<IEnumerable<prcMaterialList_Result>>(HttpStatusCode.OK, MaterialList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public HttpResponseMessage Delete(int testId, int materialId)
        {
            try
            {
                dbContext.prcTestMaterialDelete(testId, materialId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ActionName("Insert")]
        public HttpResponseMessage MaterialInsert(int testId, int materialId)
        {
            try
            {
                dbContext.prcTestMaterialInsert(testId, materialId);
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
