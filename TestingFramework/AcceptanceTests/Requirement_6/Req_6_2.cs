using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_1
{
    [TestClass]
    public class Req_6_2 : AcceptanceTest
    {
        //[TestInitialize]
        public override void Init()
        {
            addTestMemberToSystem();
            addTestStoreOwner1ToSystem();
            bool result = bridge.Login(adminUser, adminPass);
        }

        //[TestCleanup]
        public override void Cleanup()
        {
            bool result = bridge.Logout();
            removeTestStoreOwner1FromSystem();
            removeTestMemberFromSystem();
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberSuccess()
        {
            Init();
            bool result = bridge.RemoveUser(user);
            Assert.IsTrue(result);

            result = bridge.Login(user, password);
            Assert.IsFalse(result);

            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberThatWasAlreadyRemoved()
        {
            Init();
            RemoveNormalMemberSuccess();

            bool result = bridge.RemoveUser(user);
            Assert.IsFalse(result);

            Cleanup();
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberIllegal()
        {
            Init();
            bool result = bridge.RemoveUser(";");
            Assert.IsFalse(result);

            Cleanup();
        }


        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveSoleStoreOwnerSuccess()
        {
            Init();
            bool result = bridge.RemoveUser(storeOwner1);
            Assert.IsTrue(result);

            result = bridge.Login(storeOwner1, password);
            Assert.IsFalse(result);

            Cleanup();
            //TODO: verify that store doesnt exist when this feature is added
        }
    }
}
