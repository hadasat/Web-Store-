using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managment;

namespace Users.Tests
{
    [TestClass()]
    public class TestUser
    {


        string username;
        string password;
        User user;
        Member m;


        public void Init()
        {
            username = "username";
            password = "password";
            user = new User();
        }

        public void Cleanup()
        {
            try
            {
                m = ConnectionStubTemp.getMember(username);
                ConnectionStubTemp.removeMember(m);
            }
            catch (Exception ex)//not registerd
            {

            }
        }

        [TestMethod()]
        [TestCategory("TestUser")]
        public void loginMember_Test()
        {
            try
            {
                Init();
                try
                {
                    Member m = user.loginMember(username, password);
                    Assert.IsTrue(false);
                }
                catch (Exception ex)//not registerd
                {
                    Assert.IsTrue(true);
                }

                user.registerNewUser(username, password);
                try
                {
                    user.loginMember(username, password);
                    Assert.IsTrue(true);
                }
                catch (Exception ex)//registerd
                {
                    Assert.IsTrue(false);
                }
            }
            finally
            {
                Cleanup();
            }
        }


        [TestMethod()]
        [TestCategory("TestUser")]
        public void registerNewUser_Test()
        {
            try
            {
                Init();
                user.registerNewUser(username, password);
                Assert.IsTrue(true);
                try
                {
                    m = ConnectionStubTemp.getMember(username);
                    Assert.IsTrue(true);
                }
                catch (Exception ex)//didnt worked
                {
                    Assert.IsTrue(false);
                }
            }
            finally
            {
                Cleanup();
            }
        }
    }



    [TestClass()]
    public class TestMember
    {
        string username;
        string password;
        User user1;
        User user2;
        Member member1;
        Member member2;
        int storeId;
        Store store;


        //[TestInitialize]
        public void Init()
        {
            username = "username";
            password = "password";
            user1 = new User();
            user2 = new User();
            user1.registerNewUser(username + "1", password + "1");
            member1 = user1.loginMember(username + "1", password + "1");
            user2.registerNewUser(username + "2", password + "2");
            member2 = user2.loginMember(username + "2", password + "2");

            //storeId = WorkShop.createNewStore("best shop", 1, true, member1);
            //store = WorkShop.getStore(storeId);
        }

        //[TestCleanup]
        public void Cealup()
        {
            try
            {
                ConnectionStubTemp.getMember(username + "1");
                ConnectionStubTemp.removeMember(member1);
                ConnectionStubTemp.getMember(username + "2");
                ConnectionStubTemp.removeMember(member2);
            }
            catch (Exception ex)//not registerd
            {

            }
        }

        [TestMethod()]
        [TestCategory("TestMember")]
        public void logOut_Test()
        {
            try
            {
                Init();
                //logout currently not yet implemented
                Assert.IsTrue(true);
            }
            finally
            {
                Cealup();
            }

        }


        [TestMethod()]
        [TestCategory("TestMember")]
        public void addStore_Test()
        {
            try
            {
                Init();
                Store s = new Store(1, "store", 1, true);
                member1.addStore(s);
                Assert.IsTrue(member1.isStoresManagers());
                Assert.IsTrue(member1.getStoreManagerOb(s).GetStore().Id == 1);
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("TestMember")]
        public void closeStore_Test()
        {
            try
            {
                Init();
                Store s = new Store(1, "store", 1, true);
                member1.addStore(s);
                member1.closeStore(s);
                Assert.IsFalse(member1.isStoresManagers());
                try
                {
                    member1.closeStore(s);
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
        [TestCategory("TestMember")]
        public void addManager_Test()
        {
            try
            {
                Init();
                Store s = new Store(1, "store", 1, true);
                member1.addStore(s);

                Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true);
                member1.addManager("username2", ownerRoles, s);

                Assert.IsTrue(member2.isStoresManagers());
                Assert.IsTrue(member2.getStoreManagerOb(s).GetStore().Id == 1);
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("TestMember")]
        public void addManager2_Test()
        {
            try
            {
                Init();
                Store s = new Store(1, "store", 1, true);
                member1.addStore(s);

                Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true);
                member1.addManager("username2", ownerRoles, 1);

                Assert.IsTrue(member2.isStoresManagers());
                Assert.IsTrue(member2.getStoreManagerOb(s).GetStore().Id == 1);
            }
            finally
            {
                Cealup();
            }

        }


        [TestMethod()]
        [TestCategory("TestMember")]
        public void removeManager_Test()
        {
            try
            {
                Init();
                Store s = new Store(1, "store", 1, true);
                member1.addStore(s);
                Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true);
                member1.addManager("username2", ownerRoles, s);
                member1.removeManager("username2", s);
                Assert.IsFalse(member2.isStoresManagers());
            }
            finally
            {
                Cealup();
            }

        }



        

    }


    [TestClass()]
    public class TestSystemAdmin
    {

        SystemAdmin admin = new SystemAdmin("admin", 1);
        User user;
        Member member;

        //[TestInitialize]
        public void Init()
        {
            admin = new SystemAdmin("admin", 1);
            user = new User();
            user.registerNewUser("username","password");
            member = user.loginMember("username", "password");
        }

        //[TestCleanup]
        public void Cleanup()
        {
            int id;
            bool ret = ConnectionStubTemp.mapIDUsermane.TryGetValue("username", out id);
            ConnectionStubTemp.mapIDUsermane.Remove("username");
            ConnectionStubTemp.members.Remove(id);
        }

        [TestMethod()]
        [TestCategory("TestSystemAdmin")]
        public void RemoveUser_test()
        {
            try
            {
                Init();
                Store s = new Store(1, "store", 1, true);
                member.addStore(s);

                admin.RemoveUser("username");

                try
                {
                    ConnectionStubTemp.getMember("username");
                    Assert.IsTrue(false);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(true);
                }
            }
            finally
            {
                Cleanup();
            }

        }
    }
}