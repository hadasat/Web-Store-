using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_2 : AcceptanceTest
    {

        [TestCleanup]
        public override void Cleanup()
        {
            removeTestMemberFromSystem();
        }


        [TestMethod]
        [TestCategory("Req_2")]
        public void RegisterNewUser()
        {
            try
            {
                bool result = bridge.Register(user, password);

                Assert.IsTrue(result);

                result = bridge.Login(user, password);
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }


        //TODO: dependant test
        [TestMethod]
        [TestCategory("Req_2")]
        public void RegisterExistingUser()
        {
            try
            {
                bridge.Register(user, password);

                bool result = bridge.Register(user, password);

                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RegisterIllegalUser()
        {
            try
            {
                //illegal user name
                string user = ";";
                string password = "123456";

                bool result = bridge.Register(user, password);

                Assert.IsFalse(result);

                //illegal password
                user = "User";
                password = "";

                result = bridge.Register(user, password);

                Assert.IsFalse(result);

            }
            finally
            {
                Cleanup();
            }
        }
    }
}
