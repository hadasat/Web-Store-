using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Req_special
{
    
    [TestClass]
    public class LegalStocks : AcceptanceTest
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
    }
}
