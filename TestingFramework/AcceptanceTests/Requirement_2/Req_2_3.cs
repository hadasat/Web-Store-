using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_3 : AcceptanceTest
    {
        private string user = "User";
        private string password = "Password";
        private string wrongPassword = "WrongPassword";
        private string fakeUser = "FakeUser";
        private string illegalUser = ";";


        [TestInitialize]
        public override void Init()
        {
            bridge.Register(user, password);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            //TODO: remove added users
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginExistingUserCorrectly()
        {
            bool result = bridge.Login(user, password);

            Assert.IsTrue(result);
        }


        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginExistingUserInCorrectly()
        {
            bool result = bridge.Login(user, wrongPassword);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginNonExistingUser()
        {
            bool result = bridge.Login(fakeUser, password);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginIllegalUser()
        {
            bool result = bridge.Login(illegalUser, password);

            Assert.IsFalse(result);
        }
    }
}
