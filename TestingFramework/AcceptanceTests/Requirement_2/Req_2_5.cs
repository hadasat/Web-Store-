using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_5 : AcceptanceTest
    {



        [TestInitialize]
        public override void Init()
        {
            //TODO: add product to system
        }

        [TestCleanup]
        public override void Cleanup()
        {
            //TODO: remove product
        }


        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByName()
        {
            List<int> products = bridge.SearchProducts(productName, "", "", -1, -1, -1);
            Assert.IsFalse(products.Count == 1);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByCategory()
        {
            List<int> products = bridge.SearchProducts("", productCategory, "", -1, -1, -1);
            Assert.IsFalse(products.Count == 1);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByKeyword()
        {
            List<int> products = bridge.SearchProducts("", "", productKeyword, -1, -1, -1);
            Assert.IsFalse(products.Count == 1);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByPriceRange()
        {
            List<int> products = bridge.SearchProducts("", "", "", startPrice, endPrice, -1);
            Assert.IsFalse(products.Count == 1);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByStoreRank()
        {
            List<int> products = bridge.SearchProducts("", "", "", -1, -1, storeRank);
            Assert.IsFalse(products.Count == 1);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForNonExistingProduct()
        {
            List<int> products = bridge.SearchProducts(fakeProductName, "", "", -1, -1, -1);
            Assert.IsFalse(products.Count == 0);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForIllegalProduct()
        {
            List<int> products = bridge.SearchProducts(illegalProductName, "", "", -1, -1, -1);
            Assert.IsFalse(products.Count == 0);
        }


        //private bool compareProductInfo()
        //{
        //    //TODO:
        //    return false;
        //}
    }
}
