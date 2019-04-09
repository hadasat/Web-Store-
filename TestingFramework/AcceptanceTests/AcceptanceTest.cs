using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject.System_Service;
using WorkshopProject.Log;

namespace TestingFramework.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTest
    {
        protected IServiceBridge bridge  = Driver.getBridge();
        protected IGodObject godObject = new GodObject();

        protected string adminUser = "Admin";
        protected string adminPass = "Admin";
 
        protected int userId;
        protected string user = "User";
        protected string password = "Password";
        protected string wrongPassword = "WrongPassword";
        protected string fakeUser = "FakeUser";
        protected string illegalUser = ";";

        protected int storeOwner1Id;
        protected string storeOwner1 = "StoreOwner1";

        protected int storeOwner2Id;
        protected string storeOwner2 = "StoreOwner2";

        protected int storeManager1Id;
        protected string storeManager1 = "StoreManager1";

        protected int storeManager2Id;
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
        protected int productAmount = 1;
        protected string fakeProductName = "TestFakeProduct";
        protected string illegalProductName = ";";


        [TestInitialize]
        virtual public void Init() { }

        [TestCleanup]
        virtual public void Cleanup() { }

        virtual protected void addTestMemberToSystem()
        {
            try { 
                userId = godObject.addMember(user, password);
            }
            catch(Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        virtual protected void removeTestMemberFromSystem()
        {
            try
            {
                godObject.removeMember(user);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        virtual protected void addTestStoreOwner1ToSystem()
        {
            try
            {
                storeOwner1Id = godObject.addMember(storeOwner1, password);
                
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                storeId = godObject.addStore(storeName, storeRank, storeOwner1Id);
            }
        }

        virtual protected void removeTestStoreOwner1FromSystem()
        {
            try
            {
                godObject.removeStore(storeId, storeOwner1Id);
                
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                godObject.removeMember(storeOwner1);
            }
        }

        virtual protected void addTestStoreOwner2ToSystem()
        {
            try
            {
                storeOwner2Id = godObject.addMember(storeOwner2, password);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                storeId = godObject.addStore(storeName, storeRank, storeOwner2Id);
            }
        }

        virtual protected void removeTestStoreOwner2FromSystem()
        {
            try
            {
                godObject.removeStore(storeId, storeOwner2Id);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                godObject.removeMember(storeOwner2);
            }
        }
        
        ///only adds user. does not give store management roles to that user
        virtual protected void addTestStoreManager1ToSystem()
        {
            try
            {
                storeManager1Id = godObject.addMember(storeManager1, password);
                //godObject.makeUserManager(storeId, storeOwner1Id, storeManager1Id, );
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        virtual protected void removeTestStoreManager1FromSystem()
        {
            try
            {
                godObject.removeMember(storeManager2);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        ///only adds user. does not give store management roles to that user
        virtual protected void addTestStoreManager2ToSystem()
        {
            try
            {
                storeManager1Id = godObject.addMember(storeManager1, password);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        virtual protected void removeTestStoreManager2FromSystem()
        {
            try
            {
                godObject.removeMember(storeManager2);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        virtual protected void addTestStoreToSystem()
        {
            try
            {
                addTestStoreOwner1ToSystem();
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        virtual protected void removeTestStoreFromSystem()
        {
            try
            {
                removeTestStoreOwner1FromSystem();
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
        }

        virtual protected void addTestProductToSystem()
        {
            try
            {
                addTestStoreToSystem();
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                productId = godObject.addProductToStore(storeId, productName, productPrice, productCategory, productDesc, productKeyword, productAmount, productRank);
            }
        }

        virtual protected void removeTestProductFromSystem()
        {
            try
            {
                godObject.removeProductFromStore(storeId, productId);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                removeTestStoreFromSystem();
            }
        }
    }
}
