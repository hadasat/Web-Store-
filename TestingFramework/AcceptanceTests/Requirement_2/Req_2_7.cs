using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_7 : AcceptanceTest
    {

        protected int cartId;

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
        public void GetShoppingCart()
        {
            int result = bridge.GetShoppingCart(storeId);
            Assert.AreNotEqual(result, -1);
            cartId = result;
        }


        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveProductFromCart()
        {
            GetShoppingCart();

            bool result = bridge.AddProductToCart(productId, 1);
            Assert.IsTrue(result);

            result = bridge.SetProductAmountInCart(cartId, productId, 0);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveNonExistentProductFromCart()
        {
            RemoveProductFromCart();

            bool result = bridge.SetProductAmountInCart(cartId, productId, 0);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveIllegalProductFromCart()
        {
            bool result = bridge.SetProductAmountInCart(cartId, -1, 0);
            Assert.IsFalse(result);
        }


        [TestMethod]
        [TestCategory("Req_2")]
        public void ChangeProductAmountInCart()
        {
            GetShoppingCart();

            bool result = bridge.AddProductToCart(productId, 5);
            Assert.IsTrue(result);

            result = bridge.SetProductAmountInCart(cartId, productId, 2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void ChangeNonExistingProductAmountInCart()
        {
            RemoveProductFromCart();

            bool result = bridge.SetProductAmountInCart(cartId, productId, 2);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void ChangeProductIllegalAmountInCart()
        {
            ChangeProductAmountInCart();
            bool result = bridge.SetProductAmountInCart(cartId, productId, -1);
            Assert.IsFalse(result);
        }
    }
}
