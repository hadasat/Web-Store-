using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestingFramework.AcceptanceTests.Requirement_2
{
    
    [TestClass]
    public class Req_2_5 : AcceptanceTest
    {
        private string tmp_name;
        private string tmp_desc;
        private double tmp_price;
        private string tmp_category;
        private int tmp_rank;
        private int tmp_amount;

        //[TestInitialize]
        public override void Init()
        {
            //addTestStoreToSystem();
            addTestProductToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            removeTestProductFromSystem();
            //removeTestStoreFromSystem();
            godObject.cleanUpAllData();
        }


        

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByCategory()
        {
            try
            {
                Init();
                List<int> products = bridge.SearchProducts("", productCategory, "", -1, -1, -1, -1);
                Assert.IsTrue(products.Count > 0);

                bool result = bridge.GetProductInfo(products[0], out tmp_name, out tmp_desc, out tmp_price, out tmp_category, out tmp_rank, out tmp_amount);
                Assert.IsTrue(result);
                Assert.IsNotNull(tmp_name);
                Assert.AreNotEqual(tmp_name, "");
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByName()
        {
            try
            {
                Init();
                List<int> products = bridge.SearchProducts(productName, "", "", -1, -1, -1, -1);
                Assert.IsTrue(products.Count == 1);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByKeyword()
        {
            try
            {
                Init();
                List<int> products = bridge.SearchProducts("", "", productKeyword, -1, -1, -1, -1);
                Assert.IsTrue(products.Count > 0);

                bool result = bridge.GetProductInfo(products[0], out tmp_name, out tmp_desc, out tmp_price, out tmp_category, out tmp_rank, out tmp_amount);
                Assert.IsTrue(result);
                Assert.IsNotNull(tmp_name);
                Assert.AreNotEqual(tmp_name, "");
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByPriceRange()
        {
            try
            {
                Init();
                List<int> products = bridge.SearchProducts("", "", "", startPrice, endPrice, -1, -1);
                Assert.IsTrue(products.Count > 0);

                bool result = bridge.GetProductInfo(products[0], out tmp_name, out tmp_desc, out tmp_price, out tmp_category, out tmp_rank, out tmp_amount);
                Assert.IsTrue(result);
                Assert.IsNotNull(tmp_name);
                Assert.AreNotEqual(tmp_name, "");
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForExistingProductByStoreRank()
        {
            try
            {
                Init();
                List<int> products = bridge.SearchProducts("", "", "", -1, -1, -1, storeRank);
                Assert.IsTrue(products.Count > 0);

                bool result = bridge.GetProductInfo(products[0], out tmp_name, out tmp_desc, out tmp_price, out tmp_category, out tmp_rank, out tmp_amount);
                Assert.IsTrue(result);
                Assert.IsNotNull(tmp_name);
                Assert.AreNotEqual(tmp_name, "");
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForNonExistingProduct()
        {
            try
            {
                Init();
                List<int> products = bridge.SearchProducts(fakeProductName, "", "", -1, -1, -1, -1);
                Assert.IsTrue(products.Count == 0);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void SearchForIllegalProduct()
        {
            try
            {
                Init();
                List<int> products = bridge.SearchProducts(illegalProductName, "", "", -1, -1, -1, -1);
                Assert.IsTrue(products.Count == 0);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}


