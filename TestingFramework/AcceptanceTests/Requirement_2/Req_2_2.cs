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
            godObject.cleanUpAllData();
        }


        [TestMethod]
        [TestCategory("Req_2")]
        [TestCategory("Regression")]
        public void RegisterNewUser()
        {
            try
            {
                bool result = bridge.Register(getUserName(), password, DateTime.Now, "shit");

                Assert.IsTrue(result);

                result = bridge.Login(getUserName(), password);
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
        [TestCategory("Regression")]
        public void RegisterExistingUser()
        {
            try
            {
                bridge.Register(getUserName(), password,DateTime.Now,"shit");

                bool result = bridge.Register(getUserName(), password, DateTime.Now, "shit");

                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_2")]
        [TestCategory("Regression")]
        public void RegisterIllegalUser()
        {
            try
            {
                //illegal user name
                string user = ";";
                string password = "123456";

                bool result = bridge.Register(user, password, DateTime.Now, "shit");

                Assert.IsFalse(result);

                //illegal password
                user = "User";
                password = "";

                result = bridge.Register(user, password, DateTime.Now, "shit");

                Assert.IsFalse(result);

            }
            finally
            {
                Cleanup();
            }
        }
    }
}
