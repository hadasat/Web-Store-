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
            base.Init();
            addTestStoreOwner1ToSystem();
            addTestStoreOwner2ToSystem();
            bridge.Login(getStoreOwner1(), password);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreOwner2FromSystem();
            removeTestStoreOwner1FromSystem();
            godObject.cleanUpAllData();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreOwnerSuccess()
        {
            try
            {
                Init();
                AddStoreOwnerSuccessInner();
            }
            finally
            {
                Cleanup();
            }
        }

        private void AddStoreOwnerSuccessInner()
        {
            bool result = bridge.AddStoreOwner(storeId, getStoreOwner2());
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreOwnerDuplicate()
        {
            try
            {
                Init();
                AddStoreOwnerSuccessInner();
                bool result = bridge.AddStoreOwner(storeId, getStoreOwner2());
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddStoreOwnerIllegal()
        {
            try
            {
                Init();
                bool result = bridge.AddStoreOwner(storeId, ";");
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
