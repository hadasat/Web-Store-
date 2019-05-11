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
        public List<string> msg { get; }

        public testObserverAcceptence()
        {
            msg = new List<string>();
        }

        public void update(List<string> messages)
        {
            throw new NotImplementedException();
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
            addTestMemberToSystem();
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
            Assert.IsTrue(subscribe, "got true on second subscribe");

            myCleanUp();
        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void HappyConnectMessages()
        {

            //close store
            myInit();
            bool closeAns = godObject.removeStore(storeId, storeOwner1Id);
            Assert.IsTrue(closeAns, "can't close store");

            bool subscribe = userA.subscribeAsObserver(obs1);
            Assert.IsTrue(subscribe, "can't subscribe");
            //make sure no messages received on conncet
            bool noMessages = obs1.msg.Count != 0;
            Assert.IsTrue(noMessages, "don't have messages on recieve");

            myCleanUp();
        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void NewMessageEvent()
        {
            myInit();
            //close store


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
            Assert.IsTrue(noMessages, "didn't recieve message on close");

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
    }
}
