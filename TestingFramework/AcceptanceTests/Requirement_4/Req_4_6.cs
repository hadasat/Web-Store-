using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_6 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            base.Init();
            addTestStoreOwner1ToSystem();
            addTestStoreManager1ToSystem();
            addTestStoreManager2ToSystem();
        }

        //[TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager2FromSystem();
            removeTestStoreManager1FromSystem();
            removeTestStoreOwner1FromSystem();
            godObject.cleanUpAllData();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreManagerSuccess()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                bridge.Login(getStoreOwner1(), password);
                bridge.AddStoreManager(storeId, getStoreManager1());

                bool result = bridge.RemoveStoreManager(storeId, getStoreManager1());
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreManagerWhoIsntChild()
        {
            try
            {
                Init();
                bridge.Login(getStoreOwner1(), password);
                bridge.AddStoreManager(storeId, getStoreManager1());
                bridge.AddStoreManager(storeId, getStoreManager2());
                bridge.Logout();
                bridge.Login(getStoreManager1(), password);
                bool result = bridge.RemoveStoreManager(storeId, storeOwner1);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreManagerIllegal()
        {
            try
            {
                Init();
                bridge.Login(getStoreOwner1(), password);
                bridge.AddStoreManager(storeId, getStoreManager1());

                bool result = bridge.RemoveStoreManager(storeId, ";");
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

    }
}
