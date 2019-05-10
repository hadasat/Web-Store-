using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.UsesrsN
;
using WorkshopProject.System_Service;

namespace WorkshopProject.Managment.Tests
{
    [TestClass()]
    public class TestStoreManager
    {
        Member storeOwner, storeOwner2, storeManager, storeManager2;
        GodObject god = new GodObject();
        int ownerId;
        int ownerId2;
        int managerId;
        int managerId2;
        int storeId;

        Store store;
        Roles ownerRoles;
        Roles managerRoles;


        //[TestInitialize]
        public void Init()
        {
            ownerId = god.addMember("username1", "password1");
            ownerId2 = god.addMember("username2", "password2");
            managerId = god.addMember("username3", "password3");
            managerId2 = god.addMember("username4", "password4");
            storeOwner = ConnectionStubTemp.getMember(ownerId);
            storeOwner2 = ConnectionStubTemp.getMember(ownerId2);
            storeManager = ConnectionStubTemp.getMember(managerId);
            storeManager2 = ConnectionStubTemp.getMember(managerId2);

            ownerRoles = new Roles(true, true, true, true, true, true, true, true);
            managerRoles = new Roles(true, true, true, true, true, false, false, false);

            storeId = WorkShop.createNewStore("best shop", 1, true, storeOwner);
            store = WorkShop.getStore(storeId);
        }

        //[TestCleanup]
        public void Cealup()
        {
            god.removeStore(storeId, ownerId);
            god.removeMember(ownerId);
            god.removeMember(ownerId2);
            god.removeMember(managerId);
            god.removeMember(managerId2);
            try
            {
                ConnectionStubTemp.removeMember(storeOwner);
                ConnectionStubTemp.removeMember(storeOwner2);
                ConnectionStubTemp.removeMember(storeManager);
                ConnectionStubTemp.removeMember(storeManager2);
            } catch (Exception ex)
            {

            }
        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void CreateNewManager_test()
        {
            try
            {
                Init();
                StoreManager storeManagerFirstOwner = storeOwner.getStoreManagerOb(store);
                storeManagerFirstOwner.CreateNewManager(storeOwner2, ownerRoles);

                Assert.IsTrue(storeOwner2.getStoreManagerRoles(store).isStoreOwner());
                Assert.IsTrue(storeOwner2.getStoreManagerOb(store).GetFather().GetStore().id == storeId);
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void removeManager_test()
        {
            try
            {
                Init();
                //create one owner
                StoreManager storeManagerFirstOwner = storeOwner.getStoreManagerOb(store);
                storeManagerFirstOwner.CreateNewManager(storeOwner2, ownerRoles);
                //create one manager sub the new owner
                StoreManager storeManagerSecondOwner = storeOwner2.getStoreManagerOb(store);
                storeManagerSecondOwner.CreateNewManager(storeManager, ownerRoles);
                //delete the owner see if he and is sub are removed
                bool res = storeManagerFirstOwner.removeManager(storeManagerSecondOwner);

                Assert.IsTrue(res);
                Assert.IsTrue(storeManagerSecondOwner.SubManagers.Count == 0);
                Assert.IsTrue(storeManagerFirstOwner.SubManagers.Count == 0);

                StoreManager notExist = new StoreManager(null, null);
                try
                {
                    storeManagerFirstOwner.removeManager(notExist);
                    Assert.IsTrue(false);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(true);
                }
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void removeAllManagers_test()
        {
            try
            {
                Init();
                //create one owner
                StoreManager storeManagerFirstOwner = storeOwner.getStoreManagerOb(store);
                storeManagerFirstOwner.CreateNewManager(storeOwner2, ownerRoles);
                storeManagerFirstOwner.CreateNewManager(storeManager, ownerRoles);
                //create one manager sub the new owner
                StoreManager storeManagerSecondOwner = storeOwner2.getStoreManagerOb(store);
                storeManagerSecondOwner.CreateNewManager(storeManager2, ownerRoles);
                //delete the owner see if he and is sub are removed

                storeManagerFirstOwner.removeAllManagers();


                Assert.IsTrue(storeManagerSecondOwner.SubManagers.Count == 0);
                Assert.IsTrue(storeManagerFirstOwner.SubManagers.Count == 0);
            }
            finally
            {
                Cealup();
            }


        }

    }


    [TestClass()]
    public class TestRoles
    {

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void CompareRoles_test()
        {
            Roles a = new Roles(true, true, true, true, true, true, true, true);
            Roles b = new Roles(true, true, true, true, true, true, true, false);
            Roles c = new Roles(true, true, true, true, true, true, false, false);
            Roles d = new Roles(true, true, true, true, true, false, true, true);
            Roles e = new Roles(true, true, true, true, false, false, true, true);
            Roles f = new Roles(true, true, true, false, false, true, true, true);
            Roles g = new Roles(true, true, false, false, false, true, true, true);
            Roles h = new Roles(true, false, false, false, false, true, true, true);
            Roles i = new Roles(false, false, false, false, false, false, false, false);

            Assert.IsFalse(c.CompareRoles(b));
            Assert.IsFalse(d.CompareRoles(c));
            Assert.IsFalse(e.CompareRoles(d));
            Assert.IsFalse(f.CompareRoles(e));
            Assert.IsFalse(g.CompareRoles(f));
            Assert.IsFalse(h.CompareRoles(g));
            Assert.IsFalse(i.CompareRoles(a));
            Assert.IsFalse(c.CompareRoles(d));

            Assert.IsTrue(a.CompareRoles(b));
            Assert.IsTrue(b.CompareRoles(c));
            Assert.IsTrue(d.CompareRoles(e));
            Assert.IsTrue(g.CompareRoles(i));
            Assert.IsTrue(f.CompareRoles(g));
        }

        [TestMethod()]
        [TestCategory("Users_managment")]
        public void isStoreOwner_test()
        {
            Roles a = new Roles(true, true, true, true, true, true, true, true);
            Roles b = new Roles(false, false, false, false, false, false, false, false);
            Roles c = new Roles(true, true, true, true, true, false, true, true);

            Assert.IsTrue(a.isStoreOwner());
            Assert.IsFalse(b.isStoreOwner());
            Assert.IsFalse(c.isStoreOwner());
        }


    }
}