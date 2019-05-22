using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
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
            dal.SetMode(false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //this method is called ONCE AFTER the test run
        }

        [TestMethod]
        [TestCategory("DAL - persistent")]
        public void GetMemberTest()
        {
            int id = SaveNewMember();
            Member member;
            member = dal.GetMember(id);
            Assert.IsNotNull(member);
            Assert.AreEqual(member.id, id);
        }


        public int SaveNewMember()
        {
            Member member = new Member("user", 0, new DateTime(), "country");

            bool result = dal.SaveMember(member);
            Assert.IsTrue(result);
            return member.id;
        }

        [TestMethod]
        [TestCategory("DAL - persistent")]
        public void SaveNewMemberTest()
        {
            SaveNewMember();
        }



        [TestMethod]
        [TestCategory("DAL - persistent")]
        public void SaveExisitingMemberTest()
        {

        }

        [TestMethod]
        [TestCategory("DAL - persistent")]
        public void RemoveExistingMemberTest()
        {

        }
    }
}
