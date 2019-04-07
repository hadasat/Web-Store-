using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Tests
{
    [TestClass()]
    public class TestUser
    {
        [TestInitialize]
        public void Init()
        {
            //this method is called BEFORE each test
        }

        [TestCleanup]
        public void Cealup()
        {
            //this method is called AFTER each test
        }

        [TestMethod()]
        [TestCategory("TestUser")]
        public void loginMember_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }


        [TestMethod()]
        [TestCategory("TestUser")]
        public void registerNewUser_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }
    }



    [TestClass()]
    public class TestMember
    {
        [TestInitialize]
        public void Init()
        {
            //this method is called BEFORE each test
        }

        [TestCleanup]
        public void Cealup()
        {
            //this method is called AFTER each test
        }

        [TestMethod()]
        [TestCategory("TestMember")]
        public void logOut_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }


        [TestMethod()]
        [TestCategory("TestMember")]
        public void addStore_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }

        [TestMethod()]
        [TestCategory("TestMember")]
        public void closeStore_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }


        [TestMethod()]
        [TestCategory("TestMember")]
        public void addManager_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }


        [TestMethod()]
        [TestCategory("TestMember")]
        public void removeManager_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }

        

    }


    [TestClass()]
    public class TestSystemAdmin
    {
        [TestInitialize]
        public void Init()
        {
            //this method is called BEFORE each test
        }

        [TestCleanup]
        public void Cealup()
        {
            //this method is called AFTER each test
        }

        [TestMethod()]
        [TestCategory("TestSystemAdmin")]
        public void RemoveUser_test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }
    }
}