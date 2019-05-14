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
        int idStore;
        Store store;
        Member storeOwner = new Member("hadas", 12, DateTime.Today.AddYears(-25), "A");

        [TestInitialize]
        public void Init()
        {
            idStore = WorkShop.createNewStore("TestPolicyStore", 10, true, storeOwner);
            store = WorkShop.getStore(idStore);
            PolicyService.addDiscountPolicy(storeOwner,idStore,)
        }

        [TestCleanup]
        public void Cealup()
        {   
        }

        //discount
        [TestMethod]
        public void addDiscountPolicyTestTest()
        {
            IOutcome outcome = new Percentage(20);
            IBooleanExpression exp = new FalseCondition();
            Discount discount = new Discount(exp, outcome);

            
        }

        [TestMethod]
        public void removeDiscountPolicyTest()
        {
            
        }

        //purchasing
        [TestMethod]
        public void addPurchasingPolicyTest()
        {
            
        }

        [TestMethod]
        public void removePurchasingPolicyTest()
        {
            
        }

        //store
        [TestMethod]
        public void addStorePolicyTest()
        {
                    }

        [TestMethod]
        public void removeStorePolicyTest()
        {
           
        }

    }
}
