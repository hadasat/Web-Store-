using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTest
    {
        protected IServiceBridge bridge  = Driver.getBridge();

        protected string user = "User";
        protected string password = "Password";
        protected string wrongPassword = "WrongPassword";
        protected string fakeUser = "FakeUser";
        protected string illegalUser = ";";

        protected string storeOwner1 = "StoreOwner1";
        protected string storeOwner2 = "StoreOwner1";
        protected string storeManager1 = "StoreManager1";
        protected string storeManager2 = "StoreManager2";

        protected int storeId;
        protected string storeName = "TestStore";
        protected int storeRank = 3;

        protected int productId;
        protected string productName = "TestProduct";
        protected string productDesc = "TestDescription";
        protected string productCategory = "TestCategory";
        protected string productKeyword = "TestProduct";
        protected double productPrice = 10.0;
        protected double startPrice = 10.0;
        protected double endPrice = 20.0;
        protected int productRank = 1;
        protected int amount = 1;
        protected string fakeProductName = "TestFakeProduct";
        protected string illegalProductName = ";";


        [TestInitialize]
        virtual public void Init()
        { 

        }


        [TestCleanup]
        virtual public void Cleanup()
        {
            //TODO: AcceptanceTest method
        }


        virtual protected void addTestMemberToSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void removeTestMemberFromSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void addTestStoreOwner1ToSystem()
        {
            //TODO: AcceptanceTest method
            //also adds store
        }

        virtual protected void removeTestStoreOwner1FromSystem()
        {
            //TODO: AcceptanceTest method
            //also removes store
        }

        virtual protected void addTestStoreOwner2ToSystem()
        {
            //TODO: AcceptanceTest method

        }

        virtual protected void removeTestStoreOwner2FromSystem()
        {
            //TODO: AcceptanceTest method

        }

        virtual protected void addTestStoreOwner3ToSystem()
        {
            //TODO: AcceptanceTest method

        }

        virtual protected void removeTestStoreOwner3FromSystem()
        {
            //TODO: AcceptanceTest method

        }

        virtual protected void addTestStoreManager1ToSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void removeTestStoreManager1FromSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void addTestStoreManager2ToSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void removeTestStoreManager2FromSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void addTestStoreManager3ToSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void removeTestStoreManager3FromSystem()
        {
            //TODO: AcceptanceTest method
        }

        virtual protected void addTestStoreToSystem()
        {
            //TODO: AcceptanceTest method
            storeId = -1; //TODO: change to return id from actually adding to system
        }

        virtual protected void removeTestStoreFromSystem()
        {
            //TODO: AcceptanceTest method
        }


        virtual protected void addTestProductToSystem()
        {
            addTestStoreToSystem();
            //TODO: AcceptanceTest method
            productId = -1; //TODO: change to return id from actually adding to system
        }

        virtual protected void removeTestProductFromSystem()
        {
            //TODO: AcceptanceTest method

            removeTestStoreFromSystem();
        }
    }
}
