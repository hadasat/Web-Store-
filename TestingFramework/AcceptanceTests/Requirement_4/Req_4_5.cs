using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_5 : AcceptanceTest
    {
        [TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            addTestStoreManager2ToSystem();
            bridge.Login(storeOwner1, password);
        }

        [TestCleanup]
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
            bool result = bridge.AddStoreManager(storeId, storeManager2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreManagerDuplicate()
        {
            AddStoreManagerSuccess();
            bool result = bridge.AddStoreManager(storeId, storeManager2);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreManagerIllegal()
        {
            bool result = bridge.AddStoreManager(storeId, ";");
            Assert.IsFalse(result);
        }
    }
}
