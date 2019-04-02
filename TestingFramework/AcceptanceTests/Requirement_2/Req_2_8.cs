using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_8 : AcceptanceTest
    {

        [TestInitialize]
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
            bridge.AddProductToCart(productId, 1);
            int basket = bridge.GetShoppingBasket();
            bool result = bridge.BuyShoppingBasket(basket);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyEmptyCart()
        {
            int basket = bridge.GetShoppingBasket();
            bool result = bridge.BuyShoppingBasket(basket);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyCartUnavailableProducts()
        {
            BuyCartSuccess();

            bridge.AddProductToCart(productId, 1);
            int basket = bridge.GetShoppingBasket();
            bool result = bridge.BuyShoppingBasket(basket);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyCartIllegalProducts()
        {
            BuyCartSuccess();

            bridge.AddProductToCart(-1, 1);
            int basket = bridge.GetShoppingBasket();
            bool result = bridge.BuyShoppingBasket(basket);

            Assert.IsFalse(result);
        }
    }
}
