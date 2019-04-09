using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_7 : Req_2_6
    {

        protected int cartId;

        //[TestInitialize]
        //public override void Init()
        //{
        //    addTestMemberToSystem();
        //    addTestProductToSystem();
        //    bridge.Login(user, password);
        //}

        //[TestCleanup]
        //public override void Cleanup()
        //{
        //    bridge.Logout();
        //    removeTestProductFromSystem();
        //    removeTestMemberFromSystem();
        //}

        //TODO dependant test
        [TestMethod]
        [TestCategory("Req_2")]
        public void GetShoppingCart()
        {
            try
            {
                Init();
                bridge.AddProductToCart(productId, 1);
                int result = bridge.GetShoppingCart(storeId);
                Assert.AreNotEqual(result, -1);
                cartId = result;
            }
            finally
            {
                Cleanup();
            }
        }

        //TODO: dependant test
        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveProductFromCart()
        {
            try
            {

                //GetShoppingCart();
                Init();
                bool result = bridge.AddProductToCart(productId, 1);
                Assert.IsTrue(result);

                result = bridge.SetProductAmountInCart(cartId, productId, 0);
                Assert.IsTrue(result);

                int cart = bridge.GetShoppingCart(storeId);
                Assert.AreEqual(cart, -1);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveNonExistentProductFromCart()
        {
            try
            {
                Init();
                RemoveProductFromCart();

                bool result = bridge.SetProductAmountInCart(cartId, productId, 0);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveIllegalProductFromCart()
        {
            try
            {
                Init();
                bool result = bridge.SetProductAmountInCart(cartId, -1, 0);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        //TODO: dependant test
        [TestMethod]
        [TestCategory("Req_2")]
        public void ChangeProductAmountInCart()
        {
            try
            {
                
                //GetShoppingCart();
                Init();
                bool result = bridge.AddProductToCart(productId, 5);
                Assert.IsTrue(result);

                result = bridge.SetProductAmountInCart(cartId, productId, 2);
                Assert.IsTrue(result);


                int tmp_amount;
                int cart = bridge.GetShoppingCart(storeId);
                Dictionary<int, int> products = bridge.GetProductsInShoppingCart(storeId);
                Assert.IsTrue(products.TryGetValue(productId, out tmp_amount));
                Assert.AreEqual(tmp_amount, 2);
            }
            finally
            {
                Cleanup();
            }
        }

        //TODO: dependant test
        [TestMethod]
        [TestCategory("Req_2")]
        public void ChangeNonExistingProductAmountInCart()
        {
            try
            {
                Init();
                //bridge.AddProductToCart(productId, 1);
                //RemoveProductFromCart();

                bool result = bridge.SetProductAmountInCart(cartId, productId, 2);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        //TODO: dependant test
        [TestMethod]
        [TestCategory("Req_2")]
        public void ChangeProductIllegalAmountInCart()
        {
            try
            {
                Init();
                bridge.AddProductToCart(productId, 1);
                //ChangeProductAmountInCart();
                bool result = bridge.SetProductAmountInCart(cartId, productId, -1);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
