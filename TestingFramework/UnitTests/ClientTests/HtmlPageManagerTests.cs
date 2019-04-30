using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Client;

namespace TestingFramework.UnitTests.ClientTests
{
    [TestClass()]
    public class HtmlPageManagerTests
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
        [TestCategory("HtmlPageManagerTests")]
        public void getEnumByNameTest()
        {
            try
            {
                InitTestMethod();
                HtmlPageManager.PagesNames ans = HtmlPageManager.getEnumByName("/wot/main");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.Main, "error finding main enum");
                ans = HtmlPageManager.getEnumByName("/wot/");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.Main, "error finding main enum");
                ans = HtmlPageManager.getEnumByName("/wot");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.Main, "error finding main enum");

                ans = HtmlPageManager.getEnumByName("fdsfd");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.Error, "error finding ERROR enum");
            }
            finally
            {
                CleanupTestMethod();
            }

        }

        [TestMethod()]
        public void findPageByName()
        {
            try
            {
                InitTestMethod();
                string ans = HtmlPageManager.findPageByName(false, "/wot");
                Assert.IsTrue(ans == Properties.Resources.WSClientExample, "error getting unsecured example");

                ans = HtmlPageManager.findPageByName(true, "/wot");
                Assert.IsTrue(ans == Properties.Resources.WSSclientExample, "error getting unsecured example");
            }
            finally
            {
                CleanupTestMethod();
            }

        }
    }
}
