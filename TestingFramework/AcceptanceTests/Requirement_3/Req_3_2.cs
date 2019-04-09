using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_3
{

    [TestClass]
    public class Req_3_2 : AcceptanceTest
    {
        [TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            bridge.Login(storeOwner1, password);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreOwner1FromSystem();
        }

        [TestMethod]
        [TestCategory("Req_3")]
        public void AddStoreSuccess()
        {
            int result = bridge.AddStore(storeName);
            Assert.AreNotEqual(result, -1);

            //TODO: add query for store to see that it was added (when this requirment is in the version)
        }

        [TestMethod]
        [TestCategory("Req_3")]
        public void AddIllegalStore()
        {
            int result = bridge.AddStore(";");
            Assert.AreEqual(result, -1);
        }

        [TestMethod]
        [TestCategory("Req_3")]
        public void AddDuplicateStore()
        {
            AddStoreSuccess();
            int result = bridge.AddStore(storeName);
            Assert.AreEqual(result, -1);
        }
    }
}
