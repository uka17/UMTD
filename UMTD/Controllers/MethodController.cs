﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UMTD.Models;

namespace UMTD.Controllers
{
    public class MethodController : ApiController
    {
        private TestEntities dbContext = new TestEntities();
        #region Method
        [HttpGet]
        [ActionName("List")]
        public HttpResponseMessage List()
        {
            try
            {
                List<prcMethodList_Result> MethodList = (from s in dbContext.prcMethodList(1)
                                                         select s).ToList();
                return Request.CreateResponse<IEnumerable<prcMethodList_Result>>(HttpStatusCode.OK, MethodList);
            }
            catch (Exception e)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public HttpResponseMessage Delete(int testId, int methodId)
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

        [HttpGet]
        [ActionName("Insert")]
        public HttpResponseMessage Insert(int testId, int methodId)
        {
            try
            {
                dbContext.prcTestMethodInsert(testId, methodId);
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
