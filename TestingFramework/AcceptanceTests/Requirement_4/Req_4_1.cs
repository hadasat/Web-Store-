using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_4
{

    [TestClass]
    public class Req_4_1 : AcceptanceTest
    {
        [TestInitialize]
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
            int result = bridge.AddProductToStore(storeId, productName, productDesc, productPrice, productCategory);
            Assert.AreNotEqual(result, -1);
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductIllegalName()
        {
            int result = bridge.AddProductToStore(storeId, ";", productDesc, productPrice, productCategory);
            Assert.AreEqual(result, -1);
        }

        [TestMethod]
        [TestCategory("Req_4")]
        public void AddNewProductIllegalPrice()
        {
            int result = bridge.AddProductToStore(storeId, productName, productDesc, -1, productCategory);
            Assert.AreEqual(result, -1);
        }



    }
}
