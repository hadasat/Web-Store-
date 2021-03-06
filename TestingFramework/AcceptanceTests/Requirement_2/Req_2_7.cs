﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_7 : Req_2_6
    {

        protected int cartId;

        //[TestInitialize]
        public override void Init()
        {
            addTestMemberToSystem();
            addTestProductToSystem();
            bridge.Login(getUserName(), password);
        }

        //[TestCleanup]
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
        public void GetShoppingCart()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                bridge.AddProductToBasket(storeId,productId, 1);
                int result = bridge.GetShoppingCart(storeId);
                Assert.AreNotEqual(result, -1);
                cartId = result;
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
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
                DataAccessDriver.UseStub = true;
                Init();
                bool result = bridge.AddProductToBasket(storeId,productId, 1);
                Assert.IsTrue(result);

                result = bridge.SetProductAmountInBasket(storeId, productId, 0);
                Assert.IsTrue(result);

                int cart = bridge.GetShoppingCart(storeId);
                Assert.AreEqual(cart, -1);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveNonExistentProductFromCart()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                RemoveProductFromCart();

                bool result = bridge.SetProductAmountInBasket(storeId, productId, 0);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RemoveIllegalProductFromCart()
        {
            try
            {
                Init();
                bool result = bridge.SetProductAmountInBasket(storeId, -1, 0);
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
                DataAccessDriver.UseStub = true;
                //GetShoppingCart();
                Init();
                bool result = bridge.SetProductAmountInBasket(storeId,productId, 5);
                Assert.IsFalse(result);

                result = bridge.AddProductToBasket(storeId, productId, 2);
                Assert.IsTrue(result);


                int tmp_amount;
                Dictionary<int, int> products = bridge.GetProductsInShoppingCart(storeId);
                Assert.IsTrue(products.TryGetValue(productId, out tmp_amount));
                Assert.AreEqual(2,tmp_amount);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
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

                bool result = bridge.SetProductAmountInBasket(cartId, productId, 2);
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
                bridge.AddProductToBasket(storeId,productId, 1);
                //ChangeProductAmountInCart();
                bool result = bridge.SetProductAmountInBasket(cartId, productId, -1);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
