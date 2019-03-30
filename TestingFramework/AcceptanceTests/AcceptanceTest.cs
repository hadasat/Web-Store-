using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTest
    {
        protected IServiceBridge bridge;


        protected string user = "User";
        protected string password = "Password";
        protected string wrongPassword = "WrongPassword";
        protected string fakeUser = "FakeUser";
        protected string illegalUser = ";";


        protected int productId;
        protected string productName = "TestProduct";
        protected string productCategory = "TestCategory";
        protected string productKeyword = "TestProduct";
        protected double startPrice = 10.0;
        protected double endPrice = 20.0;
        protected int storeRank = 3;
        protected string fakeProductName = "TestFakeProduct";
        protected string illegalProductName = ";";




        [TestInitialize]
        virtual public void Init()
        {
            bridge = Driver.getBridge();
        }


        [TestCleanup]
        virtual public void Cleanup()
        {
            //TODO
        }


        virtual protected void addTestMemberToSystem()
        {
            //TODO
        }

        virtual protected void removeTestMemberFromSystem()
        {
            //TODO
        }


        virtual protected void addTestStoreToSystem()
        {
            //TODO
        }

        virtual protected void removeTestStoreFromSystem()
        {
            //TODO
        }


        virtual protected void addTestProductToSystem()
        {
            //TODO
            productId = -1; //TODO: change to return id from actually adding to system
        }

        virtual protected void removeTestProductFromSystem()
        {
            //TODO
        }
    }
}
