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

        string userPass = "f";

        string register = "register", make_admin = "make-admin", add_store= "open-store",
            add_product = "add-store-product", add_manager = "add-store-manager";
        int amount = 10, price = 20;

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

        private void addUser(string userName)
        {
            string command = $"{register}({userName},{userPass},A,27/02/1994)";
            string[] splitedCommand = InitSystem.splitCommand(command);
            InitSystem.registerNewUser(splitedCommand);
            User user = InitSystem.getUser(userName, userPass);
            Assert.IsNotNull(user);
        }

        private void addStore(string storeName,string userName)
        {
            string command = $"{add_store}({userName},{storeName})";
            string[] splitedCommand = InitSystem.splitCommand(command);
            InitSystem.openStore(splitedCommand);
            Store store = InitSystem.getStore(storeName);
            Assert.IsNotNull(store);
        }

        private void addProduct(string userName, string storeName, string productName)
        {
            string command = $"{add_product}({userName},{storeName},{productName},A,funny bunny,{price},{amount})";
            string[] splitedCommand = InitSystem.splitCommand(command);
            InitSystem.addProductStore(splitedCommand);
            Store store = InitSystem.getStore(storeName);
            Product product = InitSystem.getProduct(store, productName);
            Assert.IsNotNull(product);
        }

        private void deleteUser(String userName)
        {
            User user = InitSystem.getUser(userName, userPass);
            ConnectionStubTemp.removeMember((Member)user);
        }

        private void deleteStore(string storeName)
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
            addUser("A");
            deleteUser("A");
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void makeAdminTest()
        {
            string userName = "B";
            addUser(userName);

            string command = $"{make_admin}({userName})";
            string[] splitedCommand = InitSystem.splitCommand(command);
            InitSystem.makeAdmin(splitedCommand);
            User user = InitSystem.getUser(userName, userPass);
            Assert.IsTrue(user is SystemAdmin);

            ConnectionStubTemp.removeAdmin((SystemAdmin)user);
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void openStoreTest()
        {
            addUser("C");
            addStore("S1", "C");
            deleteUser("C");
            deleteStore("S1");
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
            addUser("D");
            addStore("S2","D");
            addProduct("D", "S2", "P1");
            deleteUser("D");
            deleteStore("S2");
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public void addStoreOwnerTest()
        {
            string userName = "E", userName2 = "F", storeName = "S4";
            addUser(userName);
            addUser(userName2);
            addStore(storeName,userName);

            string command = $"{add_manager}({userName},{storeName},{userName2})";
            string[] splitedCommand = InitSystem.splitCommand(command);
            InitSystem.addStoreOwner(splitedCommand);
            Store store = InitSystem.getStore(storeName);
            List<Member> managers =  StoreService.getAllManagers(store.id);
            User user = InitSystem.getUser(userName2,userPass);
            Assert.AreEqual(2, managers.Count);
            Assert.IsTrue(managers.Contains((Member)user));

            deleteUser(userName);
            deleteUser(userName2);
            deleteStore(storeName);
        }

    }
}
