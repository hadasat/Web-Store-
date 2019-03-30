using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTest
    {
        protected IServiceBridge bridge;

        [TestInitialize]
        virtual public void Init()
        {
            bridge = Driver.getBridge();
        }


        [TestCleanup]
        virtual public void Cleanup()
        {

        }

    }
}
