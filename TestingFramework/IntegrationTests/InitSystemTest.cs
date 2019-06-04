using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.System_Service;

namespace TestingFramework.IntegrationTests
{
    [TestClass()]
    public class InitSystemTest
    {

        public string file = "../../forTests.txt";
        string storeName = "test store", userName = "funny user",
            userPass = "funny password", product = "funny product";
        string register = "register", admin = "make-admin", add_store= "open-store",
            add_product = "add-store-product", add_manager = "add-store-manager";

        [TestInitialize]
        public void Init()
        {  
        }

        [TestCleanup]
        public void Cealup()
        {

        }

        //discount
        [TestMethod]
        [TestCategory("init System Test")]
        public void splitCommandTest()
        {
            string command = "one(two,three,a-1)";
            string[] splited = InitSystem.splitCommand(command);
            Assert.AreEqual(4, splited.Length);
            Assert.AreEqual("one", splited[0]);
            Assert.AreEqual("two", splited[1]);
            Assert.AreEqual("three", splited[2]);
            Assert.AreEqual("a-1", splited[3]);
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void initSystem()
        {

        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void executeTest()
        {
            string command = "";
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void getUserTest()
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void getStoreTest()
        {
            
        }

        private void addUser()
        {
            string command = $"{register}({userName},{userPass},A,27/02/1994)";
            string[] splitedCommand = InitSystem.splitCommand(command);
            InitSystem.registerNewUser(splitedCommand);
            User user = InitSystem.getUser(userName, userPass);
            Assert.IsNotNull(user);
        }

        private void deleteUser()
        {
            User user = InitSystem.getUser(userName, userPass);
            ConnectionStubTemp.removeMember((Member)user);
        }

        private void deleteStore()
        {
            Store store = InitSystem.getStore(storeName);
            WorkShop.deleteStore(store.id);
            store = InitSystem.getStore(storeName);
            Assert.IsNull(store);
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void registerNewUserTest()
        {
            addUser();
            deleteUser();
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void makeAdminTest()
        {

        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void openStoreTest()
        {
            addUser();
            string command = $"{add_store}({userName},{storeName})";
            string[] splitedCommand = InitSystem.splitCommand(command);
            InitSystem.openStore(splitedCommand);
            Store store = InitSystem.getStore(storeName);
            Assert.IsNotNull(store);
            deleteUser();
            deleteStore();
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void addProductBasketTest()
        {

        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void addProductStoreTest()
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void addStoreOwnerTest()
        {
            
        }

    }
}
