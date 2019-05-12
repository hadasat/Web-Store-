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
                ans = HtmlPageManager.getEnumByName("/wot/index");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.Main, "error finding main enum");

                ans = HtmlPageManager.getEnumByName("fdsfd");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.Error, "error finding ERROR enum");

                ans = HtmlPageManager.getEnumByName("/wot/signin");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.signIn, "error finding main enum");
                ans = HtmlPageManager.getEnumByName("/wot/signIn");
                Assert.IsTrue(ans == HtmlPageManager.PagesNames.signIn, "error finding main enum");
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
                string ans = HtmlPageManager.findPageByName( "/wot");
                Assert.IsTrue(ans == Properties.Resources.index, "cant get /wot");
                ans = HtmlPageManager.findPageByName("/wot/main");
                Assert.IsTrue(ans == Properties.Resources.index, "cant get /wot/main");
                ans = HtmlPageManager.findPageByName("/wot/index");
                Assert.IsTrue(ans == Properties.Resources.index, "cant get /wot/index");

                ans = HtmlPageManager.findPageByName("/wot/signIn");
                Assert.IsTrue(ans == Properties.Resources.SignIn, "cant get /wot/singIn");
                ans = HtmlPageManager.findPageByName("/wot/signin");
                Assert.IsTrue(ans == Properties.Resources.SignIn, "cant get /wot/signin");
            }
            finally
            {
                CleanupTestMethod();
            }

        }
    }
}
