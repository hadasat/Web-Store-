using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_3
{

    [TestClass]
    public class Req_3_2 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            bool result = bridge.Login(storeOwner1, password);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bool result = bridge.Logout();
            removeTestStoreOwner1FromSystem();
        }

        [TestMethod]
        [TestCategory("Req_3")]
        public void AddStoreSuccess()
        {
            try
            {
                Init();

                int result = bridge.AddStore(storeName);
                Assert.AreNotEqual(result, -1);
            }
            finally
            {
                Cleanup();
            }
            //TODO: add query for store to see that it was added (when this requirment is in the version)
        }

        [TestMethod]
        [TestCategory("Req_3")]
        public void AddIllegalStore()
        {
            try
            {
                Init();

                int result = bridge.AddStore(";");
                Assert.AreEqual(result, -1);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_3")]
        public void AddDuplicateStore()
        {
            try
            {
                Init();

                AddStoreSuccess();
                int result = bridge.AddStore(storeName);
                Assert.AreEqual(result, -1);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
