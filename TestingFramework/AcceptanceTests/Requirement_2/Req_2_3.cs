using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_3 : AcceptanceTest
    {
        [TestInitialize]
        public override void Init()
        {
            addTestMemberToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            removeTestMemberFromSystem();
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
