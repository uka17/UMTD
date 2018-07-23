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
        private UserController User;
        private string CorrectApiKey = "4444";
        private string IncorrectApiKey = "3333";
        private string TestString = "билирубин";
        private int TestId = 340;

        private int UomId1 = 680;
        private int UomId2 = 681;

        private int MaterialId1 = 301;
        private int MaterialId2 = 305;

        private int MethodId1 = 300;
        private int MethodId2 = 303;

        private string Translation1 = "тестовый_перевод1";
        private string Translation2 = "тестовый_перевод2";

        private int CorrectUserId = 5;
        private int IncorrectUserId = 1;
        private string CorrectLogin = "1@1";
        private string IncorrectLogin = "9@9";
        private string InitialName = "Unit";
        private string NewName = "Test";
        private string Password1 = "ewq";
        private string Password2 = "qwe";

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
            User = new UserController();
            User.Request = CreateRequest();
        }
        #region Ref Controllers
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
        #endregion

        #region Test Controller
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
            Assert.AreEqual(true, ((prcTestSelect_Result)((ObjectContent)Test.Get(CorrectApiKey, TestId).Content).Value).Uom.Contains(UomId1.ToString()));
        }
        [TestMethod]
        public void TestUomDelete()
        {
            Assert.AreEqual(HttpStatusCode.OK, Test.UomDelete(CorrectApiKey, TestId, UomId1).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.UomDelete(IncorrectApiKey, TestId, UomId2).StatusCode);
        }
        [TestMethod]
        public void TestMaterialInsert()
        {
            Assert.AreEqual(HttpStatusCode.OK, Test.MaterialInsert(CorrectApiKey, TestId, MaterialId1).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.MaterialInsert(IncorrectApiKey, TestId, MaterialId2).StatusCode);
            Assert.AreEqual(true, ((prcTestSelect_Result)((ObjectContent)Test.Get(CorrectApiKey, TestId).Content).Value).Material.Contains(MaterialId1.ToString()));
        }
        [TestMethod]
        public void TestMaterialDelete()
        {
            Assert.AreEqual(HttpStatusCode.OK, Test.MaterialDelete(CorrectApiKey, TestId, MaterialId1).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.MaterialDelete(IncorrectApiKey, TestId, MaterialId2).StatusCode);
        }
        [TestMethod]
        public void TestMethodInsert()
        {
            Assert.AreEqual(HttpStatusCode.OK, Test.MethodInsert(CorrectApiKey, TestId, MethodId1).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.MethodInsert(IncorrectApiKey, TestId, MethodId2).StatusCode);
            Assert.AreEqual(true, ((prcTestSelect_Result)((ObjectContent)Test.Get(CorrectApiKey, TestId).Content).Value).Method.Contains(MethodId1.ToString()));
        }
        [TestMethod]
        public void TestMethodDelete()
        {
            Assert.AreEqual(HttpStatusCode.OK, Test.MethodDelete(CorrectApiKey, TestId, MethodId1).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.MethodDelete(IncorrectApiKey, TestId, MethodId2).StatusCode);
        }
        [TestMethod]
        public void TestTranslationInsertDelete()
        {
            HttpResponseMessage GoodResponse = Test.TranslationInsert(CorrectApiKey, TestId, 1, Translation1);
            int TranslationId = (int)((ObjectContent)GoodResponse.Content).Value;            
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.TranslationInsert(IncorrectApiKey, TestId, 1, Translation2).StatusCode);
            Assert.AreEqual(true, ((prcTestSelect_Result)((ObjectContent)Test.Get(CorrectApiKey, TestId).Content).Value).Translation.Contains(Translation1.ToString()));

            Assert.AreEqual(HttpStatusCode.OK, Test.TranslationDelete(CorrectApiKey, TranslationId).StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, Test.TranslationDelete(IncorrectApiKey, TranslationId).StatusCode);
        }
        #endregion

        #region User Controller
        [TestMethod]
        public void UserLogin()
        {
            Assert.AreEqual(HttpStatusCode.OK, User.Login(CorrectLogin, Password1, true).StatusCode);
            Assert.AreEqual(HttpStatusCode.Forbidden, User.Login(CorrectLogin, Password2, true).StatusCode);
        }
        [TestMethod]
        public void UserChangeName()
        {
            //Login
            HttpResponseMessage LoginResponse = User.Login(CorrectLogin, Password1, true);
            Assert.AreEqual(HttpStatusCode.OK, LoginResponse.StatusCode);
            User.Request.Headers.Add("Cookie", LoginResponse.Headers.FirstOrDefault().Value.FirstOrDefault());
            //Change name
            Assert.AreEqual(HttpStatusCode.OK, User.ProfileUpdate(CorrectUserId, NewName, CorrectLogin, 1, true, "localhost", null, Password1).StatusCode);
            //Check if name was changed
            HttpResponseMessage GoodResponse1 = User.Login(CorrectLogin, Password1, true);
            Assert.AreEqual(NewName, ((prcUserSelect_Result)((ObjectContent)GoodResponse1.Content).Value).Name);
            //Try update with incorrect Email
            Assert.AreEqual(HttpStatusCode.InternalServerError, User.ProfileUpdate(CorrectUserId, NewName, IncorrectLogin, 1, true, "localhost", null, Password1).StatusCode);
            //Try update with incorrect Id
            Assert.AreEqual(HttpStatusCode.InternalServerError, User.ProfileUpdate(IncorrectUserId, NewName, CorrectLogin, 1, true, "localhost", null, Password1).StatusCode);
            //Change name back
            Assert.AreEqual(HttpStatusCode.OK, User.ProfileUpdate(CorrectUserId, NewName, CorrectLogin, 1, true, "localhost", null, Password1).StatusCode);
            //Check if name was changed back
            HttpResponseMessage GoodResponse2 = User.Login(CorrectLogin, Password1, true);
            Assert.AreEqual(InitialName, ((prcUserSelect_Result)((ObjectContent)GoodResponse2.Content).Value).Name);
        }
        [TestMethod]
        public void UserChangePassword()
        {
                        
        }
        //Logout
        #endregion

    }
}
