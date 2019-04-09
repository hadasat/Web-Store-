using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_8 : AcceptanceTest
    {

        //[TestInitialize]
        public override void Init()
        {
            addTestProductToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            removeTestProductFromSystem();
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyCartSuccess()
        {
            try
            {
                Init();
                bridge.AddProductToCart(productId, 1);
                bool result = bridge.BuyShoppingBasket();

                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyEmptyCart()
        {
            try
            {
                Init();
                bool result = bridge.BuyShoppingBasket();

                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyCartUnavailableProducts()
        {
            try
            {
                Init();
                BuyCartSuccess();

                bridge.AddProductToCart(productId, 200);
                bool result = bridge.BuyShoppingBasket();

                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyCartIllegalProducts()
        {
            try
            {
                Init();
                BuyCartSuccess();

                bridge.AddProductToCart(-1, 1);
                bool result = bridge.BuyShoppingBasket();

                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
