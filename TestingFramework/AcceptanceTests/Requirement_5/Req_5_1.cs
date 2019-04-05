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

        private string createRolesJson(bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore)
        {
            return JsonConvert.SerializeObject(
                new ManagerRolesContainer(
                    addRemovePurchasing, 
                    addRemoveDiscountPolicy, 
                    addRemoveStoreManger, 
                    closeStore)
                , Formatting.Indented);
        }

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


    class ManagerRolesContainer
    {
        public bool addRemovePurchasing;
        public bool addRemoveDiscountPolicy;
        public bool addRemoveStoreManger;
        public bool closeStore;

        public ManagerRolesContainer(bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore)
        {
            this.addRemovePurchasing = addRemovePurchasing;
            this.addRemoveDiscountPolicy = addRemoveDiscountPolicy;
            this.addRemoveStoreManger = addRemoveStoreManger;
            this.closeStore = closeStore;
        }
    }
}
