using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Users;

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
            try
            {
                bool result = bridge.AddStoreOwner(storeId, storeOwner2);
                Assert.IsTrue(result);
            } catch(Exception e)
            {

            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        [TestCategory("Regression")]
        public void AddStoreOwnerDuplicate()
        {
            try
            {
                Init();

                AddStoreOwnerSuccessInner();
                try
                {
                    bool result = bridge.AddStoreOwner(storeId, storeOwner2);
                    Assert.IsFalse(true);

                } catch(Exception ex)
                {
                    Assert.IsFalse(false);
                }
                
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        [TestCategory("Regression")]
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
