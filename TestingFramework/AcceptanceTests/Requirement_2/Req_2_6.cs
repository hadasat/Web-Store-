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
            addTestProductToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            removeTestProductFromSystem();
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void AddProductToCart()
        {
            bool result = bridge.AddProductToCart(productId, 1);
            Assert.IsTrue(result);

            //TODO: maybe check that product was added?
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
