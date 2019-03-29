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
        [TestMethod()]
        [TestCategory("Examples")]
        public void TestMeTest()
        {
            Assert.IsTrue(ClassForTestExample.TestMe());
        }
    }
}