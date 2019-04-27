using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject.External_Services;


namespace TestingFramework.AcceptanceTests.Req_special
{
    
    [TestClass]
    public class ExternalSystemsConnection : AcceptanceTest
    {

        public override void Init()
        {
        }

        public override void Cleanup()
        {
        }


        [TestMethod]
        [TestCategory("Req_special")]
        public void PaymentSystemConnectionTest()
        {
            try
            {
                Init();
                PaymentSystemConnectionTestInner();
            }
            finally
            {
                Cleanup();
            }
        }

        public void PaymentSystemConnectionTestInner()
        {
            bool result = PaymentStub.connectionTest();
            Assert.IsTrue(result);
        }


        [TestMethod]
        [TestCategory("Req_special")]
        public void SupplySystemConnectionTest()
        {
            try
            {
                Init();
                SupplySystemConnectionTestInner();
            }
            finally
            {
                Cleanup();
            }
        }

        public void SupplySystemConnectionTestInner()
        {
            bool result = SupplyStub.connectionTest();
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_special")]
        public void ConsistencySystemConnectionTest()
        {
            try
            {
                Init();
                SupplySystemConnectionTestInner();
            }
            finally
            {
                Cleanup();
            }
        }

        public void ConsistencySystemConnectionTestInner()
        {
            bool result = ConsistencyStub.connectionTest();
            Assert.IsTrue(result);
        }
    }
}
