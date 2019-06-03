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
            WorkShop.Remove(idStore);
        }

        //discount
        [TestMethod]
        public void addDiscountPolicyTestTest()
        {
            IOutcome outcome = new Percentage(20);
            IBooleanExpression exp = new FalseCondition();
            discount = new Discount(exp, outcome);
            int oldNumPolicies = store.discountPolicy.Count;

            idDiscount = PolicyService.addDiscountPolicy(storeOwner, idStore, discount);
            Assert.IsTrue(idDiscount > 0);
            int newNumPolicies = store.discountPolicy.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        [TestMethod]
        public void removeDiscountPolicyTest()
        {
            IOutcome outcome = new Percentage(20);
            IBooleanExpression exp = new FalseCondition();
            discount = new Discount(exp, outcome);
            idDiscount = PolicyService.addDiscountPolicy(storeOwner, idStore, discount);

            int oldNumPolicies = store.discountPolicy.Count;
            bool sucsess = PolicyService.removeDiscountPolicy(storeOwner, idStore, idDiscount);
            Assert.IsTrue(sucsess);
            int newNumPolicies = store.discountPolicy.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        //purchasing
        [TestMethod]
        public void addPurchasingPolicyTest()
        {
            purchasingExp = new FalseCondition();
            int oldNumPolicies = store.purchasePolicy.Count;

            idpurchasingExp = PolicyService.addPurchasingPolicy(storeOwner, idStore, purchasingExp);
            Assert.IsTrue(idpurchasingExp > 0);
            int newNumPolicies = store.purchasePolicy.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        [TestMethod]
        public void removePurchasingPolicyTest()
        {
            purchasingExp = new FalseCondition();
            idpurchasingExp = PolicyService.addPurchasingPolicy(storeOwner, idStore, purchasingExp);

            int oldNumPolicies = store.purchasePolicy.Count;
            bool sucsess = PolicyService.removePurchasingPolicy(storeOwner, idStore, idpurchasingExp);
            Assert.IsTrue(sucsess);
            int newNumPolicies = store.purchasePolicy.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        //store
        [TestMethod]
        public void addStorePolicyTest()
        {
            storeExp = new FalseCondition();
            int oldNumPolicies = store.storePolicy.Count;

            idstoreExp = PolicyService.addStorePolicy(storeOwner, idStore, storeExp);
            Assert.IsTrue(idstoreExp >= 0,idstoreExp+"");
            int newNumPolicies = store.storePolicy.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

        [TestMethod]
        public void removeStorePolicyTest()
        {
            storeExp = new FalseCondition();
            idstoreExp = PolicyService.addStorePolicy(storeOwner, idStore, storeExp);

            int oldNumPolicies = store.storePolicy.Count;
            bool sucsess = PolicyService.removeStorePolicy(storeOwner, idStore, idstoreExp);
            Assert.IsTrue(sucsess);
            int newNumPolicies = store.storePolicy.Count;
            Assert.AreNotEqual(oldNumPolicies, newNumPolicies);
        }

    }
}
