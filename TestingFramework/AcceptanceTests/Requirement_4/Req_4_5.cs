using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_5 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            addTestStoreManager2ToSystem();
            bridge.Login(storeOwner1, password);
        }

        //[TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager2FromSystem();
            removeTestStoreOwner1FromSystem();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreManagerSuccess()
        {
            try
            {
                Init();
                bool result = bridge.AddStoreManager(storeId, storeManager2);
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreManagerDuplicate()
        {
            try
            {
                Init();
                AddStoreManagerSuccess();
                bool result = bridge.AddStoreManager(storeId, storeManager2);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
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
