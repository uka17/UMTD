using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UMTD.Models;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          

namespace UMTD.Controllers
{
    public class TestController : ApiController
    {
        private TestEntities dbContext = new TestEntities();                
        #region Test
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List()
        {
            try
            {
                List<prcTestSelectAll_Result> TestList = (from s in dbContext.prcTestSelectAll(1)
                                                          select s).ToList();
                return Request.CreateResponse<IEnumerable<prcTestSelectAll_Result>>(HttpStatusCode.OK, TestList);
            }
            catch(Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        [ActionName("Delete")]
        public HttpResponseMessage Delete(int testId)
        {
            try
            {
                dbContext.prcTestDelete(testId);
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
