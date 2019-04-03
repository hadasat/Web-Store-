using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_6 : AcceptanceTest
    {
        [TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            addTestStoreManager1ToSystem();
            addTestStoreManager2ToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager2FromSystem();
            removeTestStoreManager1FromSystem();
            removeTestStoreOwner1FromSystem();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreManagerSuccess()
        {
            bridge.Login(storeOwner1, password);
            bridge.AddStoreManager(storeId, storeManager1);

            bool result = bridge.RemoveStoreManager(storeId, storeManager1);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreManagerWhoIsntChild()
        {
            bridge.Login(storeOwner1, password);
            bridge.AddStoreManager(storeId, storeManager1);
            bridge.AddStoreManager(storeId, storeManager2);
            bridge.Logout();
            bridge.Login(storeManager1, password);
            bool result = bridge.RemoveStoreManager(storeId, storeOwner1);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreManagerIllegal()
        {
            bridge.Login(storeOwner1, password);
            bridge.AddStoreManager(storeId, storeManager1);

            bool result = bridge.RemoveStoreManager(storeId, ";");
            Assert.IsFalse(result);
        }

    }
}
