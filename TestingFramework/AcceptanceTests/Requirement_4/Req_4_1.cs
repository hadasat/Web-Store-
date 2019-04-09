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

        //[TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreManager1FromSystem();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductSuccess()
        {
            Init();
            int result = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);
            Assert.AreNotEqual(result, -1);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductIllegalName()
        {
            Init();
            int result = bridge.AddProductToStore(storeId, ";", productDesc, productPrice, productCategory);
            Assert.AreEqual(result, -1);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductIllegalPrice()
        {
            Init();
            int result = bridge.AddProductToStore(storeId, productName, productDesc, -1, productCategory);
            Assert.AreEqual(result, -1);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveNewProductSuccess()
        {
            Init();
            AddNewProductSuccess();
            bool result = bridge.RemoveProductFromStore(storeId, productId);
            Assert.IsTrue(result);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveNewProductThatWasAlreadyRemoved()
        {
            Init();
            RemoveNewProductSuccess();
            bool result = bridge.RemoveProductFromStore(storeId, productId);
            Assert.IsFalse(result);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void RemoveIllegalProduct()
        {
            Init();
            bool result = bridge.RemoveProductFromStore(storeId, -1);
            Assert.IsFalse(result);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void UpdateProductSuccess()
        {
            Init();
            AddNewProductSuccess();
            bool result = bridge.ChangeProductInfo(storeId, productId, "", "", 50.0, "", -1);
            Assert.AreNotEqual(result, -1);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void UpdateProductThatWasRemoved()
        {
            Init();
            RemoveNewProductSuccess();
            bool result = bridge.ChangeProductInfo(storeId, productId, "", "", 50.0, "", -1);
            Assert.AreNotEqual(result, -1);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void UpdateIllegalProduct()
        {
            Init();
            bool result = bridge.ChangeProductInfo(storeId, -1, "", "", 50.0, "", -1);
            Assert.AreNotEqual(result, -1);
            Cleanup();
        }

    }
}
