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
        /// <summary>
        /// Returns list of Test entities for filter and page number
        /// </summary>
        /// <param name="filter">Part of Test translation, method, material or uom name</param>
        /// <param name="pageNumber">Which page to show</param>
        /// <returns>HttpStatusCode and Test list in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("Get")]
        public HttpResponseMessage Get(string userKey, int testId)
        {
            try
            {
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
        /// Returns list of Test entities for filter and page number
        /// </summary>
        /// <param name="filter">Part of Test translation, method, material or uom name</param>
        /// <param name="pageNumber">Which page to show</param>
        /// <returns>HttpStatusCode and Test list in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List(string filter, int pageNumber = 0)
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
        /// Returns number of pages for filter value
        /// </summary>
        /// <param name="filter">Part of Test translation, method, material or uom name</param>
        /// <returns>HttpStatusCode and number of records in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("PageCount")]
        public HttpResponseMessage PageCount(string filter)
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
        /// <returns>HttpStatusCode and Test list in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("Summary")]
        public HttpResponseMessage Summary(string userKey, string filter)
        {
            try
            {
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
        /// <param name="testId">Id of Test</param>
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description i ncase of error</returns>
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
        /// <summary>
        /// Set Confirmed attribute of test to True
        /// </summary>
        /// <param name="testId">Id of Test</param>
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("Confirm")]
        public HttpResponseMessage Confirm(int testId)
        {
            try
            {
                dbContext.prcTestConfirm(testId);
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
        /// <param name="testId">Id of Test</param>
        /// <param name="languageId">Id of language for translation</param>
        /// <param name="translation">Translation text</param>
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("TranslationInsert")]
        public HttpResponseMessage TranslationInsert(int testId, int languageId, string translation)
        {
            try
            {
                int Result = (from s in dbContext.prcTestTranslationInsert(testId, languageId, translation, "system")
                              select s.Value).FirstOrDefault();
                return Request.CreateResponse<int>(HttpStatusCode.OK, Result);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Delete translation 
        /// </summary>
        /// <param name="translationId">Id of translation</param>
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description i ncase of error</returns>
        [HttpGet]
        [ActionName("TranslationDelete")]
        public HttpResponseMessage TranslationDelete(int translationId)
        {
            try
            {
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
