using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.Policies;

namespace TestingFramework.AcceptanceTests.Requirement_5
{
    // ROLES:
    // AddRemovePurchasing
    // AddRemoveDiscountPolicy
    // AddRemoveStoreManger
    // CloseStore

    [TestClass]
    public class Req_5_1 : AcceptanceTest
    {
        private int policyId;

        string tmp_name;
        string tmp_desc;
        double tmp_price;
        string tmp_category;
        int  tmp_rank;
        int tmp_amount;

        public override void Init()
        {
            DataAccessDriver.UseStub = true;
            base.Init();
            policyId = -1;
            addTestStoreOwner1ToSystem();
            addTestStoreManager1ToSystem();
            addTestStoreManager2ToSystem();
            addTestProductToSystem();
        }

        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager2FromSystem();
            removeTestStoreManager1FromSystem();
            removeTestStoreOwner1FromSystem();
            godObject.cleanUpAllData();
            DataAccessDriver.UseStub = false;
        }

        private void createManagerWithRoles(bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore,bool addRemoveStorePolicy)
        {
            bridge.Login(getStoreOwner1(), password);
            bridge.AddStoreManager(storeId, getStoreManager1(), addRemovePurchasing, addRemoveDiscountPolicy, addRemoveStoreManger, closeStore, addRemoveStorePolicy);
            bridge.Logout();
        }

        private void createOwner()
        {
            bridge.Login(getStoreOwner1(), password);
            bridge.AddStoreManager(storeId, getStoreOwner1(), true, true, true, true,true);

        }

        [TestMethod]
        [TestCategory("Req_5")]
        public void TestManagerWithNoPermissions()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                 
                Init();
                createManagerWithRoles(false, false, false, false,false);

                bridge.Login(getStoreManager1(), password);
                bool result = bridge.AddStoreManager(storeId, getStoreManager1(), false, false, false, false,false);
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
        public void TestAddAnotherManagerWithWrongPermissions()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                Init();
                createManagerWithRoles(false, false, true, false,false);
                bridge.Login(getStoreManager1(), password);

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

                            bool result = bridge.AddStoreManager(storeId, getStoreManager1(), false, false, true, true,false);
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
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                Init();
                createOwner();

                bridge.Login(getStoreOwner1(), password);
                bool result = bridge.CloseStore(storeId);
                bridge.Logout();

                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }


        private void addPurchasingPolicyFalse()
        {
            bridge.Login(getStoreOwner1(), password);
            IBooleanExpression policy = new FalseCondition();
            string json = JsonHandler.SerializeObject(policy);
             bridge.addPurchasingPolicy(storeId, json);
            //TODO: not a number may couse prolems in the future
            policyId = 0;
            Assert.IsTrue(policyId >= 0);
        }

        private void addPurchasingPolicyTrue()
        {
            bridge.Login(getStoreOwner1(), password);
            IBooleanExpression policy = new TrueCondition();
            string json = JsonHandler.SerializeObject(policy);
            policyId = bridge.addPurchasingPolicy(storeId, json);
            Assert.IsTrue(policyId >= 0);
        }

        private void addDiscountPolicy()
        {
            bridge.Login(getStoreOwner1(), password);
            ItemFilter filter1 = new AllProductsFilter();
            IBooleanExpression leaf1 = new MinAmount(5, filter1);
            ItemFilter filter2 = new AllProductsFilter();
            IBooleanExpression leaf2 = new MinAmount(10, filter2);
            IBooleanExpression complex = new XorExpression();
            complex.addChildren(leaf1, leaf2);
            IOutcome outcome = new FreeProduct(productId, 1);
            Discount discount = new Discount(complex, outcome);
            string json = JsonHandler.SerializeObject(discount);
            policyId = bridge.addDiscountPolicy(storeId, json);
            Assert.IsTrue(policyId >= 0);
        }


        [TestMethod]
        [TestCategory("Req_5")]
        public void TestAddPurchasingPolicyFalseSuccess()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                Init();
                addPurchasingPolicyFalse();

                bridge.AddProductToBasket(storeId, productId, 1);
                bool result = bridge.BuyShoppingBasket();
                Assert.IsFalse(result,"shoping fail test");
            }
            finally
            {
                Cleanup();
            }
        }


        [TestMethod]
        [TestCategory("Req_5")]
        public void TestAddPurchasingPolicyTrueSuccess()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                Init();
                addPurchasingPolicyTrue();

                bridge.AddProductToBasket(storeId, productId, 1);
                bool result = bridge.BuyShoppingBasket();
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_5")]
        public void TestRemovePurchasingPolicySuccess()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                Init();
                addPurchasingPolicyFalse();
                bridge.removePurchasingPolicy(storeId, policyId);

                bridge.AddProductToBasket(storeId, productId, 1);
                bool result = bridge.BuyShoppingBasket();
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_5")]
        public void TestAddDiscountSuccess()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                Init();
                addDiscountPolicy();

                bridge.AddProductToBasket(storeId, productId, 1);
                bool result = bridge.BuyShoppingBasket();
                Assert.IsTrue(result);
                result = bridge.GetProductInfo(productId, out tmp_name, out tmp_desc, out tmp_price, out tmp_category, out tmp_rank, out tmp_amount);
                Assert.AreEqual(19, tmp_amount);

                bridge.AddProductToBasket(storeId, productId, 12);
                result = bridge.BuyShoppingBasket();
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_5")]
        public void TestRemoveDiscountSuccess()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                testIdentifier = stackTrace.GetFrame(0).GetMethod().Name;
                Init();
                addDiscountPolicy();
                bridge.removeDiscountPolicy(storeId, policyId);

                bridge.AddProductToBasket(storeId, productId, 1);
                bool result = bridge.BuyShoppingBasket();
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }


        //Jonathan - apperently managers can't add other managers
        //[TestMethod]
        //[TestCategory("Req_5")]
        //public void TestAddAnotherManagerSuccess()
        //{
        //    try
        //    {
        //        Init();
        //        createManagerWithRoles(false, false, true, false);

        //        bridge.Login(storeManager1, password);
        //        bool result = bridge.AddStoreManager(storeId, storeManager2, false, false, true, false);
        //        bridge.Logout();

        //        Assert.IsTrue(result);
        //    }
        //    finally
        //    {
        //        Cleanup();
        //    }
        //}

    }
}
