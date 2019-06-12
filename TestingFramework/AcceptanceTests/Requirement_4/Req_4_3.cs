using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Users;
using WorkshopProject.DataAccessLayer;

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
                DataAccessDriver.UseStub = true;
                Init();
                AddStoreOwnerSuccessInner();
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        private void AddStoreOwnerSuccessInner()
        {
            try
            {
                bool result = bridge.AddStoreOwner(storeId, getStoreOwner2());
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
                DataAccessDriver.UseStub = true;
                Init();

                AddStoreOwnerSuccessInner();
                try
                {
                    bool result = bridge.AddStoreOwner(storeId, getStoreOwner2());
                    Assert.IsFalse(true);

                } catch(Exception ex)
                {
                    Assert.IsFalse(false);
                }
                
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
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
