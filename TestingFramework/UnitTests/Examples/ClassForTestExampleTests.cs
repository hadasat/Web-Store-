using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.Tests
{
    [TestClass()]
    public class ClassForTestExampleTests
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
        [TestCategory("Examples")]
        public void TestMeTest()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }
    }
}