using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password.Tests
{
    [TestSystemAdmin()]
    public class TestPasswordHandler
    {
        [TestInitialize]
        public void Init()
        {
            int ID = "1111";
            string password;
        }

        [TestCleanup]
        public void Cealup()
        {
            //this method is called AFTER each test
        }

        [TestMethod()]
        [TestCategory("Users_Password")]
        public void hashPassword_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }

        [TestMethod()]
        [TestCategory("Users_Password")]
        public void IdentifyPassword_Test()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }
    }
}