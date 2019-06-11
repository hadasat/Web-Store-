using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_6 : AcceptanceTest
    {

        //[TestInitialize]
        public override void Init()
        {
            base.Init();
            addTestMemberToSystem();
            addTestProductToSystem();
            bridge.Login(getUserName(), password);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestProductFromSystem();
            removeTestMemberFromSystem();
            godObject.cleanUpAllData();
        }

        //TODO dependant test
        [TestMethod]
        [TestCategory("Req_2")]
        public void AddProductToCart()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                bool result = bridge.AddProductToBasket(storeId,productId, 1);
                Assert.IsTrue(result);

                int tmp_amount;
                int cart = bridge.GetShoppingCart(storeId);
                Dictionary<int, int> products = bridge.GetProductsInShoppingCart(storeId);
                Assert.IsTrue(products.TryGetValue(productId, out tmp_amount));
                Assert.AreEqual(tmp_amount, 1);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void AddNonExistentProductToCart()
        {
            try
            {
                Init();
                bool result = bridge.AddProductToBasket(storeId,-1, 1);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void AddIllegalAmountToCart()
        {
            try
            {
                Init();
                bool result = bridge.AddProductToBasket(storeId,productId, -1);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
