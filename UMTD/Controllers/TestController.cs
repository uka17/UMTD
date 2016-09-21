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
        private UMTDEntities dbContext = new UMTDEntities();
        #region Test
        /// <summary>
        /// Returns list of Test entities for filter and page number
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <returns>HttpStatusCode.OK and Test list in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("Get")]
        [Authorize]
        public HttpResponseMessage Get(string userKey, int testId)
        {
            try
           {
                //TODO: error translations and maybe separate function
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.AbsolutePath).FirstOrDefault().Value)
                    throw new Exception("UserKey is incorrect or used with wrong IP address");
                if (!dbContext.prcPrivilegeCheck(userKey, "test.get").FirstOrDefault().Value)
                    throw new Exception("Forbidden");

                prcTestSelect_Result Test = (from s in dbContext.prcTestSelect(userKey, testId)
                                             select s).FirstOrDefault();
                return Request.CreateResponse<prcTestSelect_Result>(HttpStatusCode.OK, Test);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Returns list of Test entities for filter and page number (obsolete)
        /// </summary>
        /// <param name="filter">Part of Test translation, method, material or uom name</param>
        /// <param name="pageNumber">Which page to show</param>
        /// <returns>HttpStatusCode.OK and Test list in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("List")]
        private HttpResponseMessage List(string userKey, string filter, int pageNumber = 0)
        {
            try
            {
                List<prcTestSelectAll_Result> TestList = (from s in dbContext.prcTestSelectAll(1, filter, pageNumber)
                                                          select s).ToList();
                return Request.CreateResponse<IEnumerable<prcTestSelectAll_Result>>(HttpStatusCode.OK, TestList);
            }
            catch(Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Returns number of pages for filter value (obsolete)
        /// </summary>
        /// <param name="filter">Part of Test translation, method, material or uom name</param>
        /// <returns>HttpStatusCode.OK and number of records in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("PageCount")]
        private HttpResponseMessage PageCount(string filter)
        {
            try
            {
                int PageCount = (from s in dbContext.prcTestSelectAllPageCount(filter, 1)
                                 select s.Value).FirstOrDefault();
                return Request.CreateResponse<int>(HttpStatusCode.OK, PageCount);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Returns list of Test entities for filter
        /// </summary>
        /// <param name="userKey">Authorization user key</param>
        /// <param name="filter">Part of Test translation, method, material or uom name</param>
        /// <returns>HttpStatusCode.OK and Test list in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("Summary")]
        public HttpResponseMessage Summary(string userKey, string filter)
        {
            try
            {
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.AbsolutePath).FirstOrDefault().Value)
                    throw new Exception("userKey is incorrect or used with wrong IP address");

                List<prcTestSelectAllSummary_Result> TestList = (from s in dbContext.prcTestSelectAllSummary(userKey, filter)
                                                          select s).ToList();
                return Request.CreateResponse<IEnumerable<prcTestSelectAllSummary_Result>>(HttpStatusCode.OK, TestList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Deletes Test from database
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("Delete")]
        public HttpResponseMessage Delete(string userKey, int testId)
        {
            try
            {
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.AbsolutePath).FirstOrDefault().Value)
                    throw new Exception("userKey is incorrect or used with wrong IP address");

                dbContext.prcTestDelete(userKey, testId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Set Confirmed attribute of test to True
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("Confirm")]
        public HttpResponseMessage Confirm(string userKey, int testId)
        {
            try
            {
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.AbsolutePath).FirstOrDefault().Value)
                    throw new Exception("userKey is incorrect or used with wrong IP address");

                dbContext.prcTestConfirm(userKey, testId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Add new translation to test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="languageId">Id of language for translation</param>
        /// <param name="translation">Translation text</param>
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("TranslationInsert")]
        public HttpResponseMessage TranslationInsert(string userKey, int testId, int languageId, string translation)
        {
            try
            {
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.AbsolutePath).FirstOrDefault().Value)
                    throw new Exception("userKey is incorrect or used with wrong IP address");

                int Result = (from s in dbContext.prcTestTranslationInsert(userKey, testId, languageId, translation)
                              select s.Value).FirstOrDefault();
                return Request.CreateResponse<int>(HttpStatusCode.OK, Result);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Modify existing translation
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="translationId">Id of Translation</param>
        /// <param name="languageId">Id of language for translation</param>
        /// <param name="translation">Translation text</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("TranslationUpdate")]
        public HttpResponseMessage TranslationUpdate(string userKey, int translationId, int languageId, string translation)
        {
            try
            {
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.AbsolutePath).FirstOrDefault().Value)
                    throw new Exception("userKey is incorrect or used with wrong IP address");

                dbContext.prcTestTranslationUpdate(userKey, translationId, translation, languageId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Delete translation 
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="translationId">Id of translation</param>
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("TranslationDelete")]
        public HttpResponseMessage TranslationDelete(string userKey, int translationId)
        {
            try
            {
                if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.AbsolutePath).FirstOrDefault().Value)
                    throw new Exception("userKey is incorrect or used with wrong IP address");

                dbContext.prcTestTranslationDelete(translationId);
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
