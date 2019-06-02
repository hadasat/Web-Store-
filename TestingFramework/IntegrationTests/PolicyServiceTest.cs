using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.Policies;
using WorkshopProject.System_Service;

namespace IntegrationTests
{
    [TestClass()]
    public class PolicyServiceTest
    {
        int idStore, idDiscount, idpurchasingExp, idstoreExp;
        Store store;
        Discount discount;
        IBooleanExpression purchasingExp, storeExp;
        Member storeOwner = new Member("hadas", 12, DateTime.Today.AddYears(-25), "A");

        [TestInitialize]
        public void Init()
        {
            idStore = WorkShop.createNewStore("TestPolicyStore", 10, true, storeOwner);
            store = WorkShop.getStore(idStore);            
        }

        [TestCleanup]
        public void Cealup()
        {
            WorkShop.stores.Remove(idStore);
        }

        //discount
        [TestMethod]
        [TestCategory("Policy Test")]
        [TestCategory("RegretionTest")]
        public void addDiscountPolicyTestTest()
        {
            //new Discount
            IOutcome outcome = new Percentage(20);
            IBooleanExpression exp = new FalseCondition();
            discount = new Discount(exp, outcome);
            int oldNumPolicies = store.discountPolicies.Count;

            Policystatus status = PolicyService.addDiscountPolicy(storeOwner, idStore, discount);
            Assert.IsTrue(status == Policystatus.Success);
            int newNumPolicies = store.discountPolicies.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
            Discount theNewDiscount = store.getDiscountPolicy(-1);
            Assert.IsNotNull(theNewDiscount);
            idDiscount = theNewDiscount.id;
            Assert.IsTrue(idDiscount > 0);
        }

        [TestMethod]
        [TestCategory("Policy Test")]
        [TestCategory("RegretionTest")]
        public void removeDiscountPolicyTest()
        {
            IOutcome outcome = new Percentage(20);
            IBooleanExpression exp = new FalseCondition();
            discount = new Discount(exp, outcome);
            Policystatus status = PolicyService.addDiscountPolicy(storeOwner, idStore, discount);
            Assert.IsTrue(status == Policystatus.Success);
            Discount theNewDiscount = store.getDiscountPolicy(-1);
            idDiscount = theNewDiscount.id;

            int oldNumPolicies = store.discountPolicies.Count;
            status = PolicyService.removeDiscountPolicy(storeOwner, idStore, idDiscount);
            Assert.IsTrue(status == Policystatus.Success);
            int newNumPolicies = store.discountPolicies.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        //purchasing
        [TestMethod]
        [TestCategory("Policy Test")]
        [TestCategory("RegretionTest")]
        public void addPurchasingPolicyTest()
        {
            purchasingExp = new FalseCondition();
            int oldNumPolicies = store.purchasePolicies.Count;

            Policystatus status = PolicyService.addPurchasingPolicy(storeOwner, idStore, purchasingExp);
            Assert.IsTrue(status == Policystatus.Success);
            IBooleanExpression policy = store.getPolicy(-2);
            Assert.IsNotNull(policy);
            idpurchasingExp = policy.id;
            Assert.IsTrue(idpurchasingExp > 0);
            int newNumPolicies = store.purchasePolicies.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        [TestMethod]
        [TestCategory("Policy Test")]
        [TestCategory("RegretionTest")]
        public void removePurchasingPolicyTest()
        {
            purchasingExp = new FalseCondition();
            Policystatus status = PolicyService.addPurchasingPolicy(storeOwner, idStore, purchasingExp);
            Assert.IsTrue(status == Policystatus.Success);
            int oldNumPolicies = store.purchasePolicies.Count;

            IBooleanExpression policy = store.getPolicy(-2);
            idpurchasingExp = policy.id;
            status = PolicyService.removePurchasingPolicy(storeOwner, idStore, idpurchasingExp);
            Assert.IsTrue(status == Policystatus.Success);
            
            int newNumPolicies = store.purchasePolicies.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        //store
        [TestMethod]
        [TestCategory("Policy Test")]
        [TestCategory("RegretionTest")]
        public void addStorePolicyTest()
        {
            storeExp = new FalseCondition();
            int oldNumPolicies = store.storePolicies.Count;

            Policystatus status = PolicyService.addStorePolicy(storeOwner, idStore, storeExp);
            Assert.IsTrue(status == Policystatus.Success);

            IBooleanExpression policy = store.getPolicy(-1);
            idstoreExp = policy.id;
            Assert.IsTrue(idstoreExp >= 0,idstoreExp+"");
            int newNumPolicies = store.storePolicies.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        [TestMethod]
        public void removeStorePolicyTest()
        {
            storeExp = new FalseCondition();
            Policystatus status = PolicyService.addStorePolicy(storeOwner, idStore, storeExp);
            Assert.IsTrue(status == Policystatus.Success);

            IBooleanExpression policy = store.getPolicy(-1);
            idstoreExp = policy.id;

            int oldNumPolicies = store.storePolicies.Count;
            status = PolicyService.removeStorePolicy(storeOwner, idStore, idstoreExp);
            Assert.IsTrue(status == Policystatus.Success);
            int newNumPolicies = store.storePolicies.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

    }
}
