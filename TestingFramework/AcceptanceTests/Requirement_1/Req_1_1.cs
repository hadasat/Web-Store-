using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_1
{
    [TestClass]
    public class Req_1_1 : AcceptanceTest
    {

        [TestMethod]
        [TestCategory("Req_1")]
        public void InitializeWithAdmin()
        {
            string admin = "Admin";
            string password = "Admin";

            bool result = bridge.Initialize(admin, password);

            Assert.IsTrue(result);
        }

        //TODO: test with existing admin?
    }

}
