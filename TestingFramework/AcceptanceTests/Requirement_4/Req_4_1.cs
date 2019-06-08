using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_1 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()  
        {
            base.Init();
            addTestStoreOwner1ToSystem();
            bridge.Login(getStoreOwner1(), password);
        }


        //[TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreOwner1FromSystem();
            godObject.clearDb();
        }


        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductSuccess()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                int result = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);
                Assert.AreNotEqual(result, -1);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductIllegalName()
        {
            try
            {
                Init();
                int result = bridge.AddProductToStore(storeId, ";", productDesc, productPrice, productCategory);
                Assert.AreEqual(result, -1);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductIllegalPrice()
        {
            try
            {
                Init();
                int result = bridge.AddProductToStore(storeId, productName, productDesc, -1, productCategory);
                Assert.AreEqual(result, -1);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveNewProductSuccess()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                int productId = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);
                bool result2 = bridge.RemoveProductFromStore(storeId, productId);
                Assert.IsTrue(result2);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveNewProductThatWasAlreadyRemoved()
        {
            try
            {
                Init();
                int result = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);
                bool result2 = bridge.RemoveProductFromStore(storeId, productId);
                bool result3 = bridge.RemoveProductFromStore(storeId, productId);
                Assert.IsFalse(result3);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveIllegalProduct()
        {
            try
            {
                Init();
                bool result = bridge.RemoveProductFromStore(storeId, -1);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        //TODO: dependent test
        [TestMethod]
        [TestCategory("Req_4")]
        [TestCategory("Regression")]
        public void UpdateProductSuccess()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();

                int productId = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);

                bool result2 = bridge.ChangeProductInfo(storeId, productId, "", "", 50.0, "", -1);
                Assert.IsTrue(result2);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void UpdateProductThatWasRemoved()
        {
            try
            {
                DataAccessDriver.UseStub = true;
                Init();
                RemoveNewProductSuccess();
                bool result = bridge.ChangeProductInfo(storeId, productId, "", "", 50.0, "", -1);
                Assert.AreNotEqual(result, -1);
            }
            finally
            {
                Cleanup();
                DataAccessDriver.UseStub = false;
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void UpdateIllegalProduct()
        {
            try
            {
                Init();
                bool result = bridge.ChangeProductInfo(storeId, -1, "", "", 50.0, "", -1);
                Assert.AreNotEqual(result, -1);
            }
            finally
            {
                Cleanup();
            }
        }


        [TestMethod]
        [TestCategory("Req_4")]
        public void AddProductToStockNegativeAmount()
        {
            try
            {
                Init();
                string tmp_name;
                string tmp_desc;
                double tmp_price;
                string tmp_category;
                int tmp_rank;
                int tmp_amount;

                int productId = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);

                bool result = bridge.AddProductToStock(storeId, productId, -10);
                Assert.IsFalse(result);

                bridge.GetProductInfo(productId, out tmp_name, out tmp_desc, out tmp_price, out tmp_category, out tmp_rank, out tmp_amount);
                Assert.IsTrue(tmp_amount >= 0);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
