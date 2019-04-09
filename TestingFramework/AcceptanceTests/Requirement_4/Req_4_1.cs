using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_1 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            bridge.Login(storeOwner1, password);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager1FromSystem();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductSuccess()
        {
            try
            {
                Init();
                int result = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);
                Assert.AreNotEqual(result, -1);
            }
            finally
            {
                Cleanup();
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
                Init();
                int result = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);
                bool result2 = bridge.RemoveProductFromStore(storeId, productId);
                Assert.IsTrue(result2);
            }
            finally
            {
                Cleanup();
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
        public void UpdateProductSuccess()
        {
            try
            {
                Init();

                int result = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);

                bool result2 = bridge.ChangeProductInfo(storeId, productId, "", "", 50.0, "", -1);
                Assert.IsTrue(result2);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void UpdateProductThatWasRemoved()
        {
            try
            {
                Init();
                RemoveNewProductSuccess();
                bool result = bridge.ChangeProductInfo(storeId, productId, "", "", 50.0, "", -1);
                Assert.AreNotEqual(result, -1);
            }
            finally
            {
                Cleanup();
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

    }
}
