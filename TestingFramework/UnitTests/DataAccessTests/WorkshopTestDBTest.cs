using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer.Context;

/*
namespace TestingFramework.UnitTests.DataAccessTests
{
    [TestClass]
    public class WorkshopTestDBTest
    {
        WorkshopDBContext ctx;

        [TestInitialize]
        public void Init()
        {
            ctx = new WorkshopTestDBContext();
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        [TestCategory("DAL - Context")]
        public void AddObjectNonPersistentTest()
        {
            Member Member1 = new Member();
            Member Member2 = new Member();
            Member Member3 = new Member();

            using (var ctx = new WorkshopTestDBContext())
            {
                ctx.Members.Add(Member1);
                ctx.Members.Add(Member2);
                ctx.Members.Add(Member3);

                ctx.SaveChanges();
            }
            using (var ctx = new WorkshopTestDBContext())
            {
                Member M1 = ctx.Members.Find(Member1.id);
                Member M2 = ctx.Members.Find(Member2.id);
                Member M3 = ctx.Members.Find(Member3.id);
                Assert.IsNotNull(M1);
                Assert.IsNotNull(M2);
                Assert.IsNotNull(M3);

                Assert.AreNotSame(M1, M2);
                Assert.AreNotSame(M2, M3);
                Assert.AreNotSame(M1, M3);

                Assert.AreNotSame(Member1, M1);
            }
        }

        [TestMethod]
        [TestCategory("DAL - Context")]
        public void AddObjectPersistentTest()
        {
            Member Member1 = new Member();
            ctx.Members.Add(Member1);
            ctx.SaveChanges();
            Member M1 = ctx.Members.Find(Member1.id);
            Assert.AreSame(Member1, M1);
        }
    }
}
*/