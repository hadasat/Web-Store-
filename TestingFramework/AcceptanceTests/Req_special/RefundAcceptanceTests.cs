using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject.External_Services;


namespace TestingFramework.AcceptanceTests.Req_special
{
    
    [TestClass]
    public class RefundAcceptanceTests : AcceptanceTest
    {

        public override void Init()
        {

        }

        public override void Cleanup()
        {
            
        }


        [TestMethod]
        [TestCategory("Req_special")]
        public void RefundInCaseOfPaymentFailure()
        {
            try
            {
                Init();
                RefundInCaseOfPaymentFailureInner();
            }
            finally
            {
                Cleanup();
            }
        }

        public void RefundInCaseOfPaymentFailureInner()
        {
            //TODO: wait for external systems
        }


        [TestMethod]
        [TestCategory("Req_special")]
        public void RefundInCaseOfSupplyFailure()
        {
            try
            {
                Init();
                RefundInCaseOfSupplyFailureInner();
            }
            finally
            {
                Cleanup();
            }
        }

        public void RefundInCaseOfSupplyFailureInner()
        {
            //TODO: wait for external systems
        }
    }
}
