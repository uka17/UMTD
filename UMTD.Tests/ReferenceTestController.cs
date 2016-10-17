using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMTD.Models;
using System.Linq;
using System.Collections.Generic;

namespace UMTD.Tests
{
    [TestClass]
    public class ReferenceTestController
    {
        private UMTDEntities dbContext = new UMTDEntities();

        [TestMethod]
        public void LanguageList()
        {
            List<prcLanguageList_Result> LanguageList = (from s in dbContext.prcLanguageList()
                                                         select s).ToList();
            Assert.AreEqual(1, (from s in LanguageList where s.Name == "Русский" select s.Id).FirstOrDefault());
            Assert.AreEqual(2, (from s in LanguageList where s.Name == "English" select s.Id).FirstOrDefault());
        }
    }
}
