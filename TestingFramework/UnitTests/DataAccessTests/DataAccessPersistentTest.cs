using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.UnitTests.DataAccessTests
{
    [TestClass]
    public class DataAccessPersistentTest
    {
        DataAccessPersistent dal;

        [TestInitialize]
        public void Init()
        {
            dal = new DataAccessPersistent();
        }

        [TestCleanup]
        public void Cleanup()
        {
            //this method is called ONCE AFTER the test run
        }

        public void GetMemberTest(int id)
        {

        }

        public void GetModeTest()
        {

        }

        public void GetProductTest()
        {

        }

        public void GetStoreTest()
        {

        }

        public void SaveMemberTest()
        {

        }

        public void SaveProductTest()
        {

        }
        public void SaveStoreTest()
        {

        }

        public void SetModeTest()
        {

        }
    }
}
