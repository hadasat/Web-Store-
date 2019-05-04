using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_4 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            addTestStoreOwner2ToSystem();
            //addTestStoreOwner3ToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            //removeTestStoreOwner3FromSystem();
            removeTestStoreOwner2FromSystem();
            removeTestStoreOwner1FromSystem();
            godObject.cleanUpAllData();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreOwnerSuccess()
        {
            try
            {
                Init();
                bridge.Login(storeOwner1, password);
                bridge.AddStoreOwner(storeId, storeOwner2);

                bool result = bridge.RemoveStoreOwner(storeId, storeOwner2);
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreOwnerWhoIsntChild()
        {
            try
            {
                Init();
                bridge.Login(storeOwner1, password);
                bridge.AddStoreOwner(storeId, storeOwner2);
                bridge.Logout();
                bridge.Login(storeOwner2, password);
                bool result = bridge.RemoveStoreOwner(storeId, storeOwner1);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveStoreOwnerIllegal()
        {
            try
            {
                Init();
                bridge.Login(storeOwner1, password);
                bridge.AddStoreOwner(storeId, storeOwner2);

                bool result = bridge.RemoveStoreOwner(storeId, ";");
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

    }
}
