using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Communication;
using WorkshopProject.System_Service;

namespace TestingFramework.AcceptanceTests.Requirement_10
{
    public class testObserverAcceptence : IObserver
    {
        public List<string> msg;

        public testObserverAcceptence()
        {
            msg = new List<string>();
        }

        public void update(List<string> messages)
        {
            this.msg = messages;
        }
    }

    [TestClass]

    public class req_10 : AcceptanceTest
    {
        testObserverAcceptence obs1;
        LoginProxy userA;

        private void myCleanUp()
        {
            userA.unSubscribeAsObserver(obs1);
            Cleanup();
        }

        private void myInit()
        {
            obs1 = new testObserverAcceptence();
            addTestStoreOwner1ToSystem();
            userA = new LoginProxy();
            string loginAns = userA.login(storeOwner1, password);
            bool error = loginAns == LoginProxy.successMsg;
            Assert.IsTrue(error, "can't log in");
        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void HappyConnectNoMessages()
        {
            myInit();
            //close store


            bool subscribe = userA.subscribeAsObserver(obs1);
            Assert.IsTrue(subscribe, "can't subscribe");
            //make sure no messages received on conncet
            bool noMessages = obs1.msg.Count == 0;
            Assert.IsTrue(noMessages, "have messages on recieve");

            myCleanUp();


        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void bad()
        {
            myInit();

            bool subscribe = userA.subscribeAsObserver(obs1);
            Assert.IsTrue(subscribe, "can't subscribe");
            bool subscribe2 = userA.subscribeAsObserver(obs1);
            Assert.IsFalse(subscribe2, "got true on second subscribe");

            myCleanUp();
        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void happyUnsubscribe()
        {
            myInit();

            bool subscribe = userA.subscribeAsObserver(obs1);
            Assert.IsTrue(subscribe, "can't subscribe");

            bool unSubscribe = userA.unSubscribeAsObserver(obs1);
            Assert.IsTrue(unSubscribe, "can't unsubscribe");

            myCleanUp();
        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void badUnsubscribe()
        {
            myInit();

            bool subscribe = userA.subscribeAsObserver(obs1);
            Assert.IsTrue(subscribe, "can't subscribe");

            bool unSubscribe = userA.unSubscribeAsObserver(obs1);
            Assert.IsTrue(unSubscribe, "can't unsubscribe");

            bool unSubscribe2 = userA.unSubscribeAsObserver(obs1);
            Assert.IsFalse(unSubscribe2, "eror in scecond unsubscribe");

            myCleanUp();
        }


        [TestMethod]
        [TestCategory("Req_10")]
        public void HappyConnectMessages()
        {
            closeStoreOnconnecectMessage();
            purchaseOncConnectMessage();
        }

        private void closeStoreOnconnecectMessage()
        {
            try
            {
                myInit();

                bool closeAns = godObject.removeStore(storeId, storeOwner1Id);
                Assert.IsTrue(closeAns, "can't close store");

                bool subscribe = userA.subscribeAsObserver(obs1);
                Assert.IsTrue(subscribe, "can't subscribe");
                //make sure no messages received on conncet
                bool noMessages = obs1.msg.Count != 0;
                Assert.IsTrue(noMessages, "don't have messages on recieve");
            }
            finally
            {
                myCleanUp();
            }

        }

        private void purchaseOncConnectMessage()
        {
            try
            {
                myInit();

                //purchase
                addTestProductToSystem();
                bridge.AddProductToBasket(storeId, productId, 1);
                bool purchase = bridge.BuyShoppingBasket();
                Assert.IsTrue(purchase, "couldn't purchase");
                //subscribe and chec observer
                bool subscribe = userA.subscribeAsObserver(obs1);
                Assert.IsTrue(subscribe, "can't subscribe");
                bool noMessages = obs1.msg.Count == 1;
                Assert.IsTrue(noMessages, "have messages on recieve");
            }
            finally
            {
                myCleanUp();
            }
        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void NewMessageEvent()
        {
            closStoreByOwnerLiveNotification();
            purchaseLiveNotification();
            closeStoreByAdminLiveNotification();
        }

        private void closStoreByOwnerLiveNotification()
        {
            try
            {
                myInit();

                bool subscribe = userA.subscribeAsObserver(obs1);
                Assert.IsTrue(subscribe, "can't subscribe");
                //make sure no messages received on conncet
                bool noMessages = obs1.msg.Count == 0;
                Assert.IsTrue(noMessages, "have messages on recieve");
                //close store
                bool closeAns = userA.closeStore(storeId);
                Assert.IsTrue(closeAns, "can't close store");
                //check observer
                bool hasMessages = obs1.msg.Count == 1;
                Assert.IsTrue(hasMessages, "didn't recieve message on close");
            }
            finally
            {
                myCleanUp();
            }
        }

        private void closeStoreByAdminLiveNotification()
        {
            testObserverAcceptence managerObserver = null;
            LoginProxy manager = null;
            try
            {
                myInit();

                bool subscribe1 = userA.subscribeAsObserver(obs1);
                Assert.IsTrue(subscribe1, "can't subscribe");
                //add manager
                addTestStoreManager1ToSystem();
                manager = new LoginProxy();
                //sign in and subscribe observer
                bool login = manager.login(storeManager1, password) == LoginProxy.successMsg;
                Assert.IsTrue(login, "can't login manager");
                managerObserver = new testObserverAcceptence();
                bool subscribe2 = userA.subscribeAsObserver(managerObserver);
                Assert.IsTrue(subscribe2, "can't subscribe manger as observer");
                //close store as admin
                LoginProxy admin = new LoginProxy();
                bool adminLogin = admin.login("Admin", "Admin") == LoginProxy.successMsg;
                Assert.IsTrue(adminLogin, "can't login Admin");
                admin.closeStore(storeId); //TODO fix fails because admin can't close sotres

                bool hasMessages1 = obs1.msg.Count == 1;
                Assert.IsTrue(hasMessages1, "didn't recieve message on close");

                bool hasMessages2 = managerObserver.msg.Count == 1;
                Assert.IsTrue(hasMessages2, "didn't recieve message on close");
            }
            finally
            {
                if (managerObserver != null & manager !=null)
                {
                    manager.unSubscribeAsObserver(managerObserver);
                }
                myCleanUp();
            }

        }

        private void purchaseLiveNotification()
        {
            try
            {
                myInit();

                bool subscribe = userA.subscribeAsObserver(obs1);
                Assert.IsTrue(subscribe, "can't subscribe");
                //make sure no messages received on conncet
                bool noMessages = obs1.msg.Count == 0;
                Assert.IsTrue(noMessages, "have messages on recieve");
                //purchase
                addTestProductToSystem();
                bridge.AddProductToBasket(storeId, productId, 1);
                bool purchase = bridge.BuyShoppingBasket();
                Assert.IsTrue(purchase, "couldn't purchase");
                //check observer
                bool hasMessages = obs1.msg.Count == 1;
                Assert.IsTrue(hasMessages, "didn't recieve message on close");
            }
            finally
            {
                removeTestProductFromSystem();
                myCleanUp();
            }
        }

    }
}
