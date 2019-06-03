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
            base.Init();
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
            godObject.cleanUpAllData();
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberSuccess()
        {
            try
            {
                Init();
                bool result = bridge.RemoveUser(getUserName());
                Assert.IsTrue(result);

                result = bridge.Login(getUserName(), password);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberThatWasAlreadyRemoved()
        {
            try
            {
                Init();
                RemoveNormalMemberSuccess();

                bool result = bridge.RemoveUser(getUserName());
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveNormalMemberIllegal()
        {
            try
            {
                Init();
                bool result = bridge.RemoveUser(";");
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
        }


        [TestMethod]
        [TestCategory("Req_6")]
        public void RemoveSoleStoreOwnerSuccess()
        {
            try
            {
                Init();
                bool result = bridge.RemoveUser(getStoreOwner1());
                Assert.IsTrue(result);

                result = bridge.Login(getStoreOwner1(), password);
                Assert.IsFalse(result);
            }
            finally
            {
                Cleanup();
            }
            //TODO: verify that store doesnt exist when this feature is added
        }

        [TestMethod]
        [TestCategory("Req_6")]
        public void CloseStoreSuccess()
        {
            try
            {
                Init();
                bool result = bridge.CloseStore(storeId);
                Assert.IsTrue(result);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}
