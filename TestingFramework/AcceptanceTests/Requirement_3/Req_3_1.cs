using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_3
{

    [TestClass]
    public class Req_3_1 : AcceptanceTest
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
        [TestCategory("Req_3")]
        public void LogoutAfterLogin()
        {
            Init();
            bool result = bridge.Login(user, password);
            Assert.IsTrue(result);

            result = bridge.Logout();
            Assert.IsTrue(result);
            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_3")]
        public void LogoutWithNoLogin()
        {
            Init();
            bool result = bridge.Logout();
            Assert.IsFalse(result);
            Cleanup();
        }
    }
}
