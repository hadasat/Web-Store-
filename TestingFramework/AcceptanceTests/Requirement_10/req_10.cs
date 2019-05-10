using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Communication;

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
    class req_10 : AcceptanceTest
    {
        testObserverAcceptence obs1;

        [TestInitialize]
        public override void Init()
        {
            obs1 = new testObserverAcceptence();
        }

        [TestCleanup]
        public override void Cleanup()
        {

        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void HappyConnectNoMessages()
        {

        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void sad()
        {

        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void HappyConnectMessages()
        {

        }

        [TestMethod]
        [TestCategory("Req_10")]
        public void NewMessageEvent()
        {

        }

    }
}
