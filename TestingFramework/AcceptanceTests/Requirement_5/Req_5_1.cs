using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace TestingFramework.AcceptanceTests.Requirement_5
{
    // ROLES:
    // AddRemovePurchasing
    // AddRemoveDiscountPolicy
    // AddRemoveStoreManger
    // CloseStore


    //TODO: AccTest 5.1 - add test for policies and discounts when they are supported
    [TestClass]
    public class Req_5_1 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            addTestStoreManager1ToSystem();
        }

        //[TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager1FromSystem();
            removeTestStoreOwner1FromSystem();
        }

        private void createManagerWithRoles(bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore)
        {
            bridge.Login(storeOwner1, password);
            bridge.AddStoreManager(storeId, storeOwner1, addRemovePurchasing, addRemoveDiscountPolicy, addRemoveStoreManger, closeStore);  
        }

        private void createOwner()
        {
            bridge.Login(storeOwner1, password);
            bridge.AddStoreManager(storeId, storeOwner1, true, true, true, true);

        }

        [TestMethod]
        [TestCategory("Req_5")]
        public void TestManagerWithNoPermissions()
        {
            try
            {
                Init();
                createManagerWithRoles(false, false, false, false);

                bridge.Login(storeManager1, password);
                bool result = bridge.AddStoreManager(storeId, storeManager1, false, false, false, false);
                Assert.IsFalse(result);

                result = bridge.CloseStore(storeId);
                Assert.IsFalse(result);

            }
            finally
            {
                Cleanup();
            }
        }


        [TestMethod]
        [TestCategory("Req_5")]
        public void TestAddAnotherManagerSuccess()
        {
            try
            {
                Init();
                createManagerWithRoles(false, false, true, false);

                bridge.Login(storeManager1, password);
                bool result = bridge.AddStoreManager(storeId, storeManager1, false, false, true, false);
                bridge.Logout();

                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_5")]
        public void TestAddAnotherManagerWithWrongPermissions()
        {
            try
            {
                Init();
                createManagerWithRoles(false, false, true, false);
                bridge.Login(storeManager1, password);

                for (int i = 0; i < 2; i++)
                {
                    bool p1 = (i == 0) ? false : true;
                    for (int j = 0; j < 2; j++)
                    {
                        bool p2 = (j == 0) ? false : true;
                        for (int k = 0; k < 2; k++)
                        {
                            bool p3 = (k == 0) ? false : true;
                            if (k == 0 && j == 0 && i == 0)
                            {
                                continue;
                            }

                            bool result = bridge.AddStoreManager(storeId, storeManager1, false, false, true, true);
                            Assert.IsFalse(result);
                        }
                    }
                }
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_5")]
        public void TestCloseStoreSuccess()
        {
            try
            {
                Init();
                createOwner();

                bridge.Login(storeOwner1, password);
                bool result = bridge.CloseStore(storeId);
                bridge.Logout();

                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
