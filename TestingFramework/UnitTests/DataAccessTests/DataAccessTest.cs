using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.UnitTests.DataAccessTests
{
    [TestClass]
    public class DataAccessTest
    {
        IDataAccess dal;

        [TestInitialize]
        public void Init()
        {
            dal = DataAccessDriver.GetDataAccess();
            dal.SetMode(false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //this method is called ONCE AFTER the test run
        }

        [TestMethod]
        [TestCategory("DAL")]
        public void GetMemberTest()
        {
            int id = SaveNewMember();
            Member member;
            member = dal.GetEntity<Member>(id);
            Assert.IsNotNull(member);
            Assert.AreEqual(member.id, id);
        }


        public int SaveNewMember()
        {
            Member member = new Member();
            member.country = "Test";
            member.username = "Test";

            bool result = dal.SaveEntity(member, member.id);
            Assert.IsTrue(result);
            return member.id;
        }

        [TestMethod]
        [TestCategory("DAL")]
        public void SaveNewMemberTest()
        {
            SaveNewMember();
        }



        [TestMethod]
        [TestCategory("DAL")]
        public void SaveExisitingMemberTest()
        {
            Member member = new Member();
            member.country = "Test";
            member.username = "Test";

            bool result = dal.SaveEntity(member, member.id);
            Assert.IsTrue(result);

            member.country = "Test2";
            result = dal.SaveEntity(member, member.id);
            Assert.IsTrue(result);

            member = dal.GetEntity<Member>(member.id);
            Assert.AreEqual("Test2", member.country);
        }

        [TestMethod]
        [TestCategory("DAL")]
        public void RemoveExistingMemberTest()
        {
            int memberId = SaveNewMember();
            dal.RemoveEntity<Member>(memberId);
            Member result = dal.GetEntity<Member>(memberId);
            Assert.IsNull(result);
        }


        [TestMethod]
        [TestCategory("DAL")]
        public void SqlTest()
        {
            string name = "FindMe";
            Member member = new Member();
            member.username = name;

            bool result = dal.SaveEntity(member, member.id);

            string sql = "select * from Members where username = @name";
            SqlParameter sqlparam = new SqlParameter("@name", name);
            DbRawSqlQuery<Member> query = dal.SqlQuery<Member>(sql, sqlparam);
            Member memberExtracted = query.FirstOrDefault();
            Assert.IsNotNull(memberExtracted);
            Assert.AreEqual(name, memberExtracted.username);
        }
    }
}
