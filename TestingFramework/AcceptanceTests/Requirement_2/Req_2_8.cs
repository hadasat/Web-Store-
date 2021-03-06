﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_8 : AcceptanceTest
    {

        //[TestInitialize]
        public override void Init()
        {
            DataAccessDriver.UseStub = true;
            base.Init();
            godObject.cleanUpAllData();
            addTestProductToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            removeTestProductFromSystem();
            godObject.cleanUpAllData();
            DataAccessDriver.UseStub = true;
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void BuyCartSuccess()
        {
            try
            {
                Init();
                bridge.AddProductToBasket(storeId,productId, 1);
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

                bridge.AddProductToBasket(storeId,productId, 200);
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

                bridge.AddProductToBasket(storeId ,- 1, 1);
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
