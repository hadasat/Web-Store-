using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingFramework.IntegrationTests
{
    [TestClass()]
    public class InitSystemTest
    {

        public static string file = "../../forTests.txt";
        string storeName = "test store", userName = "funny user",
            userPass = "funny password", product = "funny product";


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
        public static void splitCommand(string sentence)
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void initSystem()
        {

        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void execute(string line)
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void getUser(string username, string password)
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void getStore(string name)
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void registerNewUser(string[] fullcommand)
        {
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void makeAdmin(string[] fullcommand)
        {

        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void openStore(string[] fullcommand)
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void addProductBasket(string[] fullcommand)
        {

        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void addProductStore(string[] fullcommand)
        {
            
        }

        [TestMethod]
        [TestCategory("init System Test")]
        public static void addStoreOwner(string[] fullcommand)
        {
            
        }

    }
}
