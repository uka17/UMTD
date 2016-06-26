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
        private TestEntities dbContext = new TestEntities();
        #region Material
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List()
        {
            try
            {
                List<prcMaterialList_Result> MaterialList = (from s in dbContext.prcMaterialList()
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
        public HttpResponseMessage Delete(int testId, int MaterialId)
        {
            try
            {
                dbContext.prcTestMaterialRemove(testId, MaterialId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ActionName("Insert")]
        public HttpResponseMessage MaterialInsert(int testId, int MaterialId)
        {
            try
            {
                dbContext.prcTestMaterialInsert(testId, MaterialId);
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
