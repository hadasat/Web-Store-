using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.System_Service;

namespace WorkshopProject.Tests
{
    [TestClass()]
    public class WorkShopUnitTest
    {

        Member member;
        GodObject god = new GodObject();
        int memberId;
        //int storeId;
       // Store store;

        //[TestInitialize]
        public void Init()
        {
            memberId = god.addMember("username", "password");
            member = ConnectionStubTemp.getMember(memberId);
           
        }

        //[TestCleanup]
        public void CleanUp()
        {
            god.cleanUpAllData();
        }

        [TestMethod()]
        [TestCategory("WorkShop")]
        public void createNewStore_test()
        {
            try
            {
                Init();
               
                string store1Name = "store1 test";
                string store2Name = "store2 test";
                int store1Id = WorkShop.createNewStore(store1Name, 1, true, member);
                int store2Id = WorkShop.createNewStore(store2Name, 1, true, member);
                Store store1 = WorkShop.getStore(store1Id);
                Assert.IsTrue(store1 != null && store1.name == store1Name, "creating new store failed");
                Assert.IsTrue(store2Id != store1Id, "two stores with the same id");


            }
            finally
            {
                CleanUp();
            }

        }


    }
}
