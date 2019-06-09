using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_5 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            base.Init();
            addTestStoreOwner1ToSystem();
            addTestStoreManager2ToSystem();
            bridge.Login(getStoreOwner1(), password);
        }

        //[TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager2FromSystem();
            removeTestStoreOwner1FromSystem();
            godObject.cleanUpAllData();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreManagerSuccess()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                AddStoreManagerSuccessInner();
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        private void AddStoreManagerSuccessInner()
        {
            bool result = bridge.AddStoreManager(storeId, storeManager2);
            Assert.IsTrue(result);
        }


        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreManagerDuplicate()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                AddStoreManagerSuccessInner();
                bool result = bridge.AddStoreManager(storeId, storeManager2);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreManagerIllegal()
        {
            try
            {
                Init();
                bool result = bridge.AddStoreManager(storeId, ";");
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
