﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UMTD.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class UMTDEntities : DbContext
    {
        public UMTDEntities()
            : base("name=UMTDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<prcLanguageList_Result> prcLanguageList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prcLanguageList_Result>("prcLanguageList");
        }
    
        public virtual int prcTestInsert(string name, string code)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestInsert", nameParameter, codeParameter);
        }
    
        public virtual int prcTestMaterialDelete(Nullable<int> testId, Nullable<int> materialId)
        {
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            var materialIdParameter = materialId.HasValue ?
                new ObjectParameter("MaterialId", materialId) :
                new ObjectParameter("MaterialId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestMaterialDelete", testIdParameter, materialIdParameter);
        }
    
        public virtual int prcTestMethodDelete(Nullable<int> testId, Nullable<int> methodId)
        {
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            var methodIdParameter = methodId.HasValue ?
                new ObjectParameter("MethodId", methodId) :
                new ObjectParameter("MethodId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestMethodDelete", testIdParameter, methodIdParameter);
        }
    
        public virtual ObjectResult<prcTestSelect_Result> prcTestSelect(string userKey, Nullable<int> testId)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prcTestSelect_Result>("prcTestSelect", userKeyParameter, testIdParameter);
        }
    
        public virtual ObjectResult<prcTestSelectAllSummary_Result> prcTestSelectAllSummary(string userKey, string filter)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var filterParameter = filter != null ?
                new ObjectParameter("Filter", filter) :
                new ObjectParameter("Filter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prcTestSelectAllSummary_Result>("prcTestSelectAllSummary", userKeyParameter, filterParameter);
        }
    
        public virtual int prcTestUomDelete(Nullable<int> testId, Nullable<int> uomId)
        {
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            var uomIdParameter = uomId.HasValue ?
                new ObjectParameter("UomId", uomId) :
                new ObjectParameter("UomId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestUomDelete", testIdParameter, uomIdParameter);
        }
    
        public virtual int prcTestConfirm(string userKey, Nullable<int> testId)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestConfirm", userKeyParameter, testIdParameter);
        }
    
        public virtual int prcTestDelete(string userKey, Nullable<int> testId)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestDelete", userKeyParameter, testIdParameter);
        }
    
        public virtual int prcTestTranslationDelete(Nullable<int> testTranslationId)
        {
            var testTranslationIdParameter = testTranslationId.HasValue ?
                new ObjectParameter("TestTranslationId", testTranslationId) :
                new ObjectParameter("TestTranslationId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestTranslationDelete", testTranslationIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> prcTestTranslationInsert(string userKey, Nullable<int> testId, Nullable<int> languageId, string translation)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("LanguageId", languageId) :
                new ObjectParameter("LanguageId", typeof(int));
    
            var translationParameter = translation != null ?
                new ObjectParameter("Translation", translation) :
                new ObjectParameter("Translation", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("prcTestTranslationInsert", userKeyParameter, testIdParameter, languageIdParameter, translationParameter);
        }
    
        public virtual int prcTestTranslationUpdate(string userKey, Nullable<int> translationId, string translation, Nullable<int> languageId)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var translationIdParameter = translationId.HasValue ?
                new ObjectParameter("TranslationId", translationId) :
                new ObjectParameter("TranslationId", typeof(int));
    
            var translationParameter = translation != null ?
                new ObjectParameter("Translation", translation) :
                new ObjectParameter("Translation", typeof(string));
    
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("LanguageId", languageId) :
                new ObjectParameter("LanguageId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestTranslationUpdate", userKeyParameter, translationIdParameter, translationParameter, languageIdParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> prcKeyCheck(string userKey, string iP)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var iPParameter = iP != null ?
                new ObjectParameter("IP", iP) :
                new ObjectParameter("IP", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("prcKeyCheck", userKeyParameter, iPParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> prcPrivilegeCheck(string userKey, string privilege)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var privilegeParameter = privilege != null ?
                new ObjectParameter("Privilege", privilege) :
                new ObjectParameter("Privilege", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("prcPrivilegeCheck", userKeyParameter, privilegeParameter);
        }
    
        public virtual ObjectResult<prcMaterialList_Result> prcMaterialList(string userKey)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prcMaterialList_Result>("prcMaterialList", userKeyParameter);
        }
    
        public virtual ObjectResult<prcMethodList_Result> prcMethodList(string userKey)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prcMethodList_Result>("prcMethodList", userKeyParameter);
        }
    
        public virtual int prcTestMaterialInsert(string userKey, Nullable<int> testId, Nullable<int> materialId)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            var materialIdParameter = materialId.HasValue ?
                new ObjectParameter("MaterialId", materialId) :
                new ObjectParameter("MaterialId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestMaterialInsert", userKeyParameter, testIdParameter, materialIdParameter);
        }
    
        public virtual int prcTestMethodInsert(string userKey, Nullable<int> testId, Nullable<int> methodId)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            var methodIdParameter = methodId.HasValue ?
                new ObjectParameter("MethodId", methodId) :
                new ObjectParameter("MethodId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestMethodInsert", userKeyParameter, testIdParameter, methodIdParameter);
        }
    
        public virtual ObjectResult<prcUomList_Result> prcUomList(string userKey)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prcUomList_Result>("prcUomList", userKeyParameter);
        }
    
        public virtual int prcTestUomInsert(string userKey, Nullable<int> testId, Nullable<int> uomId)
        {
            var userKeyParameter = userKey != null ?
                new ObjectParameter("UserKey", userKey) :
                new ObjectParameter("UserKey", typeof(string));
    
            var testIdParameter = testId.HasValue ?
                new ObjectParameter("TestId", testId) :
                new ObjectParameter("TestId", typeof(int));
    
            var uomIdParameter = uomId.HasValue ?
                new ObjectParameter("UomId", uomId) :
                new ObjectParameter("UomId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcTestUomInsert", userKeyParameter, testIdParameter, uomIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> prcUserCheck(string email, string password)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("prcUserCheck", emailParameter, passwordParameter);
        }
    
        public virtual int prcUserRegister(string name, string email, string password)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prcUserRegister", nameParameter, emailParameter, passwordParameter);
        }
    
        public virtual ObjectResult<string> prcSettingSelect(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("prcSettingSelect", nameParameter);
        }
    
        public virtual ObjectResult<prcUserSelect_Result> prcUserSelect(string email)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prcUserSelect_Result>("prcUserSelect", emailParameter);
        }
    }
}
