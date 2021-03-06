﻿using System;
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
