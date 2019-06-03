using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject.System_Service;
using WorkshopProject.Log;
using System.Diagnostics;

namespace TestingFramework.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTest
    {
        protected IServiceBridge bridge  = Driver.getBridge();
        protected IGodObject godObject = new GodObject();

        protected string testIdentifier = "";

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
        protected int productAmount = 20;
        protected string fakeProductName = "TestFakeProduct";
        protected string illegalProductName = ";";


        //[TestInitialize]
        public virtual void Init() {
            setTestIdentifer(3);
            //godObject.cleanUpAllData();
        }

        [TestCleanup]
        virtual public void Cleanup() {
            godObject.cleanUpAllData();
        }

        virtual protected void addTestMemberToSystem()
        {
            try { 
                userId = godObject.addMember(getUserName(), password);
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
                godObject.removeMember(getUserName());
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
                storeOwner1Id = godObject.addMember(getStoreOwner1(), password);
                
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                try
                {
                    storeId = godObject.addStore(storeName, storeRank, storeOwner1Id);
                }
                catch(Exception e)
                {

                }
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
                try
                {
                    godObject.removeMember(getStoreOwner1());
                }
                catch (Exception e)
                {

                }
            }
        }

        virtual protected void addTestStoreOwner2ToSystem()
        {
            try
            {
                storeOwner2Id = godObject.addMember(getStoreOwner2(), password);
            }
            catch (Exception e)
            {
                Logger.Log("testFile", logLevel.DEBUG, e.Message);
            }
            finally
            {
                //storeId = godObject.addStore(storeName, storeRank, storeOwner2Id);
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
                try
                {
                    godObject.removeMember(getStoreOwner2());
                }
                catch (Exception e)
                {

                }
            }
        }
        
        ///only adds user. does not give store management roles to that user
        virtual protected void addTestStoreManager1ToSystem()
        {
            try
            {
                storeManager1Id = godObject.addMember(getStoreManager1(), password);
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
                storeManager2Id = godObject.addMember(storeManager2, password);
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
                try
                {
                    productId = godObject.addProductToStore(storeId, productName, productPrice, productCategory, productDesc, productKeyword, productAmount, productRank);
                }
                catch (Exception e)
                {

                }
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
                try
                {
                    removeTestStoreFromSystem();
                }
                catch (Exception e)
                {

                }
            }
        }


        protected virtual string TestIdentifier()
        {
            return testIdentifier;
        }

        protected virtual void setTestIdentifer(int level)
        {
            StackTrace stackTrace = new StackTrace();
            testIdentifier = stackTrace.GetFrame(level).GetMethod().Name;
        }

        protected virtual string getUserName()
        {
            return String.Concat(user, TestIdentifier());
        }

        protected virtual string getStoreOwner1()
        {
            return String.Concat(storeOwner1, "_", TestIdentifier());
        }

        protected virtual string getStoreManager1()
        {
            return String.Concat(storeManager1, TestIdentifier());
        }

        protected virtual string getStoreOwner2()
        {
            return String.Concat(storeOwner2, TestIdentifier());
        }

        protected virtual string getStoreManager2()
        {
            return String.Concat(storeManager2, TestIdentifier());
        }

    }
}
