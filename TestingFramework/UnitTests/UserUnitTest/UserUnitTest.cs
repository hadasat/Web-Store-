using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managment;
using WorkshopProject.Communication;
using WorkshopProject.System_Service;
using WorkshopProject.DataAccessLayer;

namespace Users.Tests
{
    [TestClass()]
    public class TestUser
    {

        protected IGodObject godObject = new GodObject();
        string username;
        string password;
        User user;
        Member m;


        public void Init()
        {
            DataAccessDriver.UseStub = true;
            godObject.clearDb();
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
            DataAccessDriver.UseStub = false;
        }

        [TestMethod()]
        [TestCategory("TestUser")]
        [TestCategory("Regression")]
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

                user.registerNewUser(username, password, DateTime.Now, "shit");
                try
                {
                    user.loginMember(username, password);
                    Assert.IsTrue(false);
                }
                catch (Exception ex)//registerd
                {
                    Assert.IsTrue(true);
                }
            }
            finally
            {
                Cleanup();
            }
        }


        [TestMethod()]
        [TestCategory("TestUser")]
        [TestCategory("Regression")]
        public void registerNewUser_Test()
        {
            try
            {
                Init();
                user.registerNewUser(username, password, DateTime.Now, "shit");
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


    public class testObserver : IObserver
    {
        public List<string> msg { get; set;}

        public testObserver()
        {
            msg = new List<string>();
        }
        public void update(List<Notification> notifications)
        {
            foreach (Notification curr in notifications)
            {
                msg.Add(curr.msg);
            }

        }
    }


    [TestClass()]
    public class TestMember
    {
        protected IGodObject godObject = new GodObject();
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
            DataAccessDriver.UseStub = true;
            godObject.clearDb();
            username = "username";
            password = "password";
            user1 = new User();
            user2 = new User();
            user1.registerNewUser(username + "1", password + "1",DateTime.Now, "shit");
            member1 = user1.loginMember(username + "1", password + "1");
            user2.registerNewUser(username + "2", password + "2", DateTime.Now, "shit");
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
            DataAccessDriver.UseStub = false;
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
                Assert.IsTrue(member1.getStoreManagerOb(s).GetStore().id == 1);
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

                Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true,true);
                member1.addManager("username2", ownerRoles, s);

                Assert.IsTrue(member2.isStoresManagers());
                Assert.IsTrue(member2.getStoreManagerOb(s).GetStore().id == 1);
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

                Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true,true);
                member1.addManager("username2", ownerRoles, 1);

                Assert.IsTrue(member2.isStoresManagers());
                Assert.IsTrue(member2.getStoreManagerOb(s).GetStore().id == 1);
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
                Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true,true);
                member1.addManager("username2", ownerRoles, s);
                member1.removeManager("username2", s);
                Assert.IsFalse(member2.isStoresManagers());
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("TestMember")]
        public void subscribeAndUnsubscribeTest()
        {
            try
            {
                Init();
                IObserver observer1 = new testObserver();
                IObserver observer2 = new testObserver();
                bool subscribeAns = member1.subscribe(observer1);
                Assert.IsTrue(subscribeAns, "can't subscribe");

                subscribeAns = member1.subscribe(observer1);
                Assert.IsFalse(subscribeAns, "subscribed secondTime");

                subscribeAns = member1.subscribe(observer2);
                Assert.IsTrue(subscribeAns, "can't subscribe 2");

                bool unsubscribeAns = member1.unsbscribe(observer1);
                Assert.IsTrue(unsubscribeAns, "can't unsubscribe 1");

                unsubscribeAns = member1.unsbscribe(observer1);
                Assert.IsFalse(unsubscribeAns, "unsubscribe 1 second time");

                unsubscribeAns = member1.unsbscribe(observer2);
                Assert.IsTrue(unsubscribeAns, "can't unsubscribe 2");

                subscribeAns = member1.subscribe(null);
                Assert.IsFalse(subscribeAns, "subscribed null");

                unsubscribeAns = member1.unsbscribe(null);
                Assert.IsFalse(unsubscribeAns, "unsubscribe null");
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("TestMember")]
        [TestCategory("Regression")]
        public void NotificationTests()
        {
            try
            {
                Init();
                testObserver obs1 = new testObserver();
                testObserver obs2 = new testObserver();

                //test one observe subscrived
                member1.subscribe(obs1);
                member1.addMessage("test1");
                Assert.IsTrue (obs1.msg.Count == 1, "bad message count 1");
                Assert.IsTrue(obs1.msg[0] =="test1", "bad message");

                obs1.msg.Clear();
                obs2.msg.Clear();
                //test 2 subscribed observers
                member1.subscribe(obs2);
                member1.addMessage("test1");
                Assert.IsTrue(obs1.msg.Count == 1, "bad message count 2");
                Assert.IsTrue(obs1.msg[0] == "test1", "bad message 2");
                Assert.IsTrue(obs2.msg.Count == 1, "bad message count 1 for obs 2");
                Assert.IsTrue(obs2.msg[0] == "test1", "bad message for obs 2");

                obs1.msg.Clear();
                obs2.msg.Clear();

                member1.unsbscribe(obs1);
                member1.unsbscribe(obs2);

                //test unsubscribed observer
                member1.addMessage("test1");
                member1.addMessage("test2");

                member1.subscribe(obs1);
                Assert.IsTrue(obs1.msg.Count == 2, "bad message count 3");
                Assert.IsTrue(obs1.msg[0] == "test1", "bad message 3");
                Assert.IsTrue(obs1.msg[1] == "test2", "bad message 4");

            }
            finally{
                Cealup();
            }
        }




    }


    [TestClass()]
    public class TestSystemAdmin
    {
        protected IGodObject godObject = new GodObject();
        SystemAdmin admin = new SystemAdmin("admin");
        User user;
        Member member;

        //[TestInitialize]
        public void Init()
        {
            DataAccessDriver.UseStub = true;
            godObject.clearDb();
            admin = new SystemAdmin("admin");
            user = new User();
            user.registerNewUser("username","password", DateTime.Now, "shit");
            member = user.loginMember("username", "password");
        }

        //[TestCleanup]
        public void Cleanup()
        {

            //int id;
            //bool ret = ConnectionStubTemp.mapIDUsermane.TryGetValue("username", out id);
            //ConnectionStubTemp.mapIDUsermane.Remove("username");
            //ConnectionStubTemp.members.Remove(id);

            DataAccessDriver.UseStub = false;
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