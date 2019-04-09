using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_6 : AcceptanceTest
    {

        [TestInitialize]
        public override void Init()
        {
            addTestMemberToSystem();
            addTestProductToSystem();
            bridge.Login(user, password);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestProductFromSystem();
            removeTestMemberFromSystem();
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void AddProductToCart()
        {
            bool result = bridge.AddProductToCart(productId, 1);
            Assert.IsTrue(result);

            int tmp_amount;
            int cart = bridge.GetShoppingCart(storeId);
            Dictionary<int, int> products = bridge.GetProductsInShoppingCart(cart);
            Assert.IsTrue(products.TryGetValue(productId, out tmp_amount));
            Assert.AreEqual(tmp_amount, 1);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void AddNonExistentProductToCart()
        {
            bool result = bridge.AddProductToCart(-1, 1);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void AddIllegalAmountToCart()
        {
            bool result = bridge.AddProductToCart(productId, -1);
            Assert.IsFalse(result);
        }
    }
}
