using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer.Context;

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
        public void AddObjectTest()
        {

                ctx.Members.Add(new Member());
                ctx.Members.Add(new Member());
                ctx.Members.Add(new Member());

                ctx.SaveChanges();
            using (var ctx = new WorkshopProductionDBContext())
            {
                ctx.Members.Add(new Member());
                ctx.Members.Add(new Member());
                ctx.Members.Add(new Member());

                ctx.SaveChanges();
            }
        }

    }
}
