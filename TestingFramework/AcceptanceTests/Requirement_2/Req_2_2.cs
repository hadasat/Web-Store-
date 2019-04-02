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
            //TODO: remove added users
        }


        [TestMethod]
        [TestCategory("Req_2")]
        public void RegisterNewUser()
        {
            string user = "User";
            string password = "SecretPassword";

            bool result = bridge.Register(user, password);

            Assert.IsTrue(result);

            result = bridge.Login(user, password);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RegisterExistingUser()
        {
            RegisterNewUser();
            string user = "User";
            string password = "SecretPassword";

            bool result = bridge.Register(user, password);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void RegisterIllegalUser()
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
    }
}
