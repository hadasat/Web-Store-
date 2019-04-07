using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.System_Service;

namespace Managment.Tests
{
    [TestClass()]
    public class TestStoreManager
    {
        Member storeOwner, someMember;
        GodObject god = new GodObject();
        int ownerId;
        int storeId;
        int ownerId2;
        Store store;
        Roles ownerRoles;


        [TestInitialize]
        public void Init()
        {
            ownerId = god.addMember("username", "password");
            ownerId2 = god.addMember("some member", "some password");
            storeOwner = ConnectionStubTemp.getMember(ownerId);
            someMember = ConnectionStubTemp.getMember(ownerId2);

            ownerRoles = new Roles(true, true, true, true, true, true, true, true);

            storeId = WorkShop.createNewStore("best shop", 1, true, storeOwner);
            store = WorkShop.getStore(storeId);
        }

        [TestCleanup]
        public void Cealup()
        {
            god.removeMember(ownerId);
            god.removeMember(ownerId2);
            god.removeStore(storeId, ownerId);
        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void CreateNewManager_test()
        {

            StoreManager storeManagerFirstOwner = storeOwner.getStoreManagerOb(store);
            storeManagerFirstOwner.CreateNewManager(someMember, ownerRoles);

            Assert.IsTrue(someMember.getStoreManagerRoles(store).isStoreOwner());
            Assert.IsTrue(someMember.getStoreManagerOb(store).GetFather().GetStore().Id == storeId);
        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void removeManager_test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void removeAllManagers_test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }

    }


    [TestClass()]
    public class TestRoles
    {
        [TestInitialize]
        public void Init()
        {
            //this method is called BEFORE each test
        }

        [TestCleanup]
        public void Cealup()
        {
            //this method is called AFTER each test
        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void CompareRoles_test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void isStoreOwner_test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }


    }
}