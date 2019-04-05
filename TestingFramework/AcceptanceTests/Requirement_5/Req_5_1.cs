using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace TestingFramework.AcceptanceTests.Requirement_5
{
    // ROLES:
    // AddRemovePurchasing
    // AddRemoveDiscountPolicy
    // AddRemoveStoreManger
    // CloseStore

    [TestClass]
    public class Req_5_1 : AcceptanceTest
    {
        [TestInitialize]
        public override void Init()
        {
            addTestStoreOwner1ToSystem();
            addTestMemberToSystem();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            removeTestMemberFromSystem();
            removeTestStoreOwner1FromSystem();
        }


        //TODO: 5.1 tests
        [TestMethod]
        [TestCategory("Req_5")]
        public void TestAddPurchasingManagerSuccess()
        {
            bridge.Login(storeOwner1, password);
            //bridge.AddStoreManager();
            
            bridge.Logout();
        }
    }
}
