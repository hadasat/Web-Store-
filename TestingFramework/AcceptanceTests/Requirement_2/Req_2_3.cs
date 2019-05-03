using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_2
{

    [TestClass]
    public class Req_2_3 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestMemberToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestMemberFromSystem();
            godObject.cleanUpAllData();
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginExistingUserCorrectly()
        {
            try
            {
            Init();
            bool result = bridge.Login(user, password);

            Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }


        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginExistingUserInCorrectly()
        {
            try
            {

            
            Init();
            bool result = bridge.Login(user, wrongPassword);

            Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginNonExistingUser()
        {
            try
            {

            
            Init();
            bool result = bridge.Login(fakeUser, password);

            Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        public void LoginIllegalUser()
        {
            try {

            Init();
            bool result = bridge.Login(illegalUser, password);

            Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
