using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTest
    {
        private IServiceBridge bridge;

        [TestInitialize]
        virtual public void init()
        {
            bridge = Driver.getBridge();
        }


        [TestCleanup]
        virtual public void cleanup()
        {

        }

    }
}
