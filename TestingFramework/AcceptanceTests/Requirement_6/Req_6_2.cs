using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests.Requirement_1
{
    [TestClass]
    public class Req_6_2 : AcceptanceTest
    {
        [TestInitialize]
        public override void Init()
        {
            addTestMemberToSystem();
            addTestStoreOwner1ToSystem();
            bridge.Login(adminUser, adminPass);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            bridge.Logout();
            removeTestStoreOwner1FromSystem();
            removeTestMemberFromSystem();
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberSuccess()
        {
            bool result = bridge.RemoveUser(user);
            Assert.IsTrue(result);

            result = bridge.Login(user, password);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberThatWasAlreadyRemoved()
        {
            RemoveNormalMemberSuccess();

            bool result = bridge.RemoveUser(user);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberIllegal()
        {
            bool result = bridge.RemoveUser(";");
            Assert.IsFalse(result);
        }


        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveSoleStoreOwnerSuccess()
        {
            bool result = bridge.RemoveUser(storeOwner1);
            Assert.IsTrue(result);

            result = bridge.Login(storeOwner1, password);
            Assert.IsFalse(result);

            //TODO: verify that store doesnt exist when this feature is added
        }
    }
}
