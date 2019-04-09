using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_3 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            addTestStoreOwner2ToSystem();
            bridge.Login(storeOwner1, password);
        }

        //[TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreOwner2FromSystem();
            removeTestStoreOwner1FromSystem();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreOwnerSuccess()
        {
            Init();
            bool result = bridge.AddStoreOwner(storeId, storeOwner2);
            Assert.IsTrue(result);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreOwnerDuplicate()
        {
            Init();
            AddStoreOwnerSuccess();
            bool result = bridge.AddStoreOwner(storeId, storeOwner2);
            Assert.IsFalse(result);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreOwnerIllegal()
        {
            Init();
            bool result = bridge.AddStoreOwner(storeId, ";");
            Assert.IsFalse(result);
            Cleanup();
        }
    }
}
