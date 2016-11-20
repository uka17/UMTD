using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMTD.Models;
using System.Linq;
using System.Collections.Generic;
using UMTD.Controllers;
using System.Net;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;

namespace UMTD.Tests
{
    [TestClass]
    public class ReferenceTestController
    {
        private UMTDEntities dbContext = new UMTDEntities();
        private LanguageController Language;
        private MaterialController Material;
        private MethodController Method;
        private UomController Uom;
        private TestController Test;
        private string CorrectApiKey = "4444";
        private string IncorrectApiKey = "3333";
        private string TestString = "билирубин";
        private int TestId = 340;
        private int UomId1 = 680;
        private int UomId2 = 681;

        private HttpRequestMessage CreateRequest()
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            Request.RequestUri = new Uri("http://localhost");
            return Request;
        }

        [TestInitialize()]
        public void PrepareData()
        {
            Language = new LanguageController();
            Language.Request = CreateRequest();
            Material = new MaterialController();
            Material.Request = CreateRequest(); 
            Method = new MethodController();
            Method.Request = CreateRequest();
            Uom = new UomController();
            Uom.Request = CreateRequest();
            Test = new TestController();
            Test.Request = CreateRequest();
        }

        [TestMethod]
        public void LanguageList()
        {
            HttpResponseMessage GoodResponse = Language.List(CorrectApiKey);

            Assert.AreEqual(HttpStatusCode.OK, GoodResponse.StatusCode);
        }
        [TestMethod]
        public void MethodList()
        {
            HttpResponseMessage GoodResponse = Method.List(CorrectApiKey);
            HttpResponseMessage BadResponse = Method.List(IncorrectApiKey);
            Assert.AreEqual(HttpStatusCode.OK, GoodResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, BadResponse.StatusCode);
        }
        [TestMethod]
        public void MaterialList()
        {
            HttpResponseMessage GoodResponse = Material.List(CorrectApiKey);
            HttpResponseMessage BadResponse = Material.List(IncorrectApiKey);
            Assert.AreEqual(HttpStatusCode.OK, GoodResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, BadResponse.StatusCode);
        }
        [TestMethod]
        public void UomList()
        {
            HttpResponseMessage GoodResponse = Uom.List(CorrectApiKey);
            HttpResponseMessage BadResponse = Uom.List(IncorrectApiKey);
            Assert.AreEqual(HttpStatusCode.OK, GoodResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, BadResponse.StatusCode);
        }
        [TestMethod]
        public void TestGet()
        {
            HttpResponseMessage GoodResponse = Test.Get(CorrectApiKey, 1);

            Assert.AreEqual(1, ((prcTestSelect_Result)((ObjectContent)GoodResponse.Content).Value).Id);
            Assert.AreEqual(HttpStatusCode.OK, GoodResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.Get(IncorrectApiKey, 1).StatusCode);            
        }
        [TestMethod]
        public void SummaryPageCount()
        {
            HttpResponseMessage GoodResponse = Test.SummaryPageCount(CorrectApiKey, "");

            Assert.AreNotEqual(0, GoodResponse.Content);
            Assert.AreEqual(HttpStatusCode.OK, GoodResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.Get(IncorrectApiKey, 1).StatusCode);
        }
        [TestMethod]
        public void TestSummary()
        {
            HttpResponseMessage GoodResponse = Test.Summary(CorrectApiKey, TestString, 1);
            
            List<prcTestSelectAllSummary_Result> testList = new List<prcTestSelectAllSummary_Result>((IEnumerable<prcTestSelectAllSummary_Result>)((ObjectContent)GoodResponse.Content).Value);

            Assert.AreEqual(TestId, testList[0].Id);
            Assert.AreEqual(HttpStatusCode.OK, GoodResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.Get(IncorrectApiKey, 1).StatusCode);
        }
        [TestMethod]
        public void TestUomInsert()
        {
            Assert.AreEqual(HttpStatusCode.OK, Test.UomInsert(CorrectApiKey, TestId, UomId1).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.UomInsert(IncorrectApiKey, TestId, UomId2).StatusCode);
        }
        [TestMethod]
        public void TestUomDelete()
        {
            Assert.AreEqual(HttpStatusCode.OK, Test.UomDelete(CorrectApiKey, TestId, UomId1).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.UomInsert(IncorrectApiKey, TestId, UomId2).StatusCode);
        }
    }
}
