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
        /// <returns>HttpStatusCode.OK and Test list in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("Get")]
        public HttpResponseMessage Get(string userKey, int testId)
        {
            try
           {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.get");

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
        /// Returns number of pages for test list
        /// </summary>
        /// <param name="userKey">Authorization user key</param>        
        /// <param name="filter">Part of Test translation, method, material or uom name</param>
        /// <returns>HttpStatusCode.OK and number of pages in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("SummaryPageCount")]
        public HttpResponseMessage SummaryPageCount(string userKey, string filter)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.summary_page_count");

                int PageCount = (from s in dbContext.prcTestSelectAllSummaryPageCount(userKey, filter)
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
        /// <param name="pageNumber">Number of current page</param>
        /// <returns>HttpStatusCode.OK and Test list in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("Summary")]
        public HttpResponseMessage Summary(string userKey, string filter, int pageNumber)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.summary");

                List<prcTestSelectAllSummary_Result> TestList = (from s in dbContext.prcTestSelectAllSummary(userKey, filter, pageNumber)
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
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("Delete")]
        public HttpResponseMessage Delete(string userKey, int testId)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.delete");

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
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("Confirm")]
        public HttpResponseMessage Confirm(string userKey, int testId)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.confirm");

                dbContext.prcTestConfirm(userKey, testId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }        
        #endregion
        #region Test translation
        /// <summary>
        /// Add new translation to test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="languageId">Id of language for translation</param>
        /// <param name="translation">Translation text</param>
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("TranslationInsert")]
        public HttpResponseMessage TranslationInsert(string userKey, int testId, int languageId, string translation)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.translation_insert");

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
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("TranslationUpdate")]
        public HttpResponseMessage TranslationUpdate(string userKey, int translationId, int languageId, string translation)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.translation_update");

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
        /// <returns>HttpStatusCode in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("TranslationDelete")]
        public HttpResponseMessage TranslationDelete(string userKey, int translationId)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.translation_delete");

                dbContext.prcTestTranslationDelete(translationId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        #endregion
        #region Test uom
        /// <summary>
        /// Remove Uom from Test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="uomId">Id of Uom</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("UomDelete")]
        public HttpResponseMessage UomDelete(string userKey, int testId, int uomId)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.uom_delete");

                dbContext.prcTestUomDelete(testId, uomId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Add Uom to Test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="uomId">Id of Uom</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("UomInsert")]
        public HttpResponseMessage UomInsert(string userKey, int testId, int uomId)
        {
            try
            {
                CheckKey(userKey);
                CheckPrivilege(userKey, "test.uom_insert");
                dbContext.prcTestUomInsert(userKey, testId, uomId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        #endregion
        #region Test material
        /// <summary>
        /// Remove Material from Test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="materialId">Id of Material</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("MaterialDelete")]
        public HttpResponseMessage MaterialDelete(string userKey, int testId, int materialId)
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
        /// <summary>
        /// Add Material to Test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="materialId">Id of Material</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("MaterialInsert")]
        public HttpResponseMessage MaterialInsert(string userKey, int testId, int materialId)
        {
            try
            {
                dbContext.prcTestMaterialInsert(userKey, testId, materialId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        #endregion
        #region Test method
        /// <summary>
        /// Remove Method from Test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="materialId">Id of Method</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("MethodDelete")]
        public HttpResponseMessage MethodDelete(string userKey, int testId, int methodId)
        {
            try
            {
                dbContext.prcTestMethodDelete(testId, methodId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Add Method to Test
        /// </summary>
        /// <param name="userKey">Requestor identifier</param>
        /// <param name="testId">Id of Test</param>
        /// <param name="materialId">Id of Method</param>
        /// <returns>HttpStatusCode.OK in case of success, InternalServerError and error description in case of error</returns>
        [HttpGet]
        [ActionName("MethodInsert")]
        public HttpResponseMessage MethodInsert(string userKey, int testId, int methodId)
        {
            try
            {
                dbContext.prcTestMethodInsert(userKey, testId, methodId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        #endregion
        #region Service
        /// <summary>
        /// Check if key is valid for IP where it is being used
        /// </summary>
        /// <param name="userKey">Api key of user</param>
        private void CheckKey(string userKey)
        {
            //TODO: error translations and maybe separate function
            if (!dbContext.prcKeyCheck(userKey, Request.RequestUri.Host).FirstOrDefault().Value)
                throw new Exception("UserKey is incorrect or used with wrong IP address");
        }
        /// <summary>
        /// Check if this api key can use this section
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="section"></param>
        private void CheckPrivilege(string userKey, string section)
        {
            //TODO: error translations and maybe separate function
            if (!dbContext.prcPrivilegeCheck(userKey, section).FirstOrDefault().Value)
                throw new Exception("Forbidden");
        }
        #endregion
    }
}
