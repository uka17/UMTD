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

        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List()
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
