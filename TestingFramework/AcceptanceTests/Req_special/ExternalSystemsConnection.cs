using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            //TODO: wait for external systems
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
            //TODO: wait for external systems
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
            //TODO: wait for external systems
        }
    }
}
