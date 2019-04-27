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
            //this method is called ONCE before the test run
        }

        [TestCleanup]
        public void Cleanup()
        {
            //this method is called ONCE AFTER the test run
        }

        public void InitTestMethod()
        {
            //this method is called before each test method call
        }

        public void CleanupTestMethod()
        {
            //this method is called AFTER each test method call
        }


        [TestMethod()]
        [TestCategory("Examples")]
        public void TestMeTest()
        {
            try
            {
                InitTestMethod();
                TestMeTestInner();
            }
            finally
            {
                CleanupTestMethod();
            }

        }

        public void TestMeTestInner()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }
    }
}