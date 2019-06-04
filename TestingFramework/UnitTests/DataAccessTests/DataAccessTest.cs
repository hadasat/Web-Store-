using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;
/*
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
            member = dal.GetMember(id);
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

            member = dal.GetMember(member.id);
            Assert.AreEqual("Test2", member.country);
        }

        [TestMethod]
        [TestCategory("DAL")]
        public void RemoveExistingMemberTest()
        {
            int memberId = SaveNewMember();
            dal.RemoveEntity<Member>(memberId);
            Member result = dal.GetMember(memberId);
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

        [TestMethod]
        [TestCategory("DAL")]
        public void AddStoreWithStock()
        {
            //Store store = new Store("store", 0, true);
            ////dal.SaveStore(store);

            //dal.SaveEntity(store, store.id);

            //store = dal.GetStore(store.id);

            //Product prod = new Product("product", 10, "desc", "cat", 0, 10, store.id);

            ////store.AddToStock(20, prod);
            //Stock neStock = new Stock(20, prod);
            //store.GetStock().Add(neStock);
            ////((DataAccessNonPersistent) dal).SaveStore(store);
            //dal.SaveEntity(store, store.id);

            //int count = 0;
            //Store store2 = DataAccessDriver.GetDataAccess().GetStore(store.id);
            //Assert.AreEqual(store2.GetStock().Count, 1);
            //foreach(Stock st in store.Stocks)
            //{
            //    count++;
            //}

            //Assert.AreNotEqual(count, 0);
            ////DataAccessDriver.GetDataAccess().GetStore(store.id);


        }

        //[TestMethod]
        //[TestCategory("DAL")]
        //public void AddDiscount()
        //{
        //    IBooleanExpression cond = new TrueCondition();
        //    IOutcome outcome = new Percentage();
        //    Discount discount = new Discount(cond, outcome);

        //    dal.SaveEntity(discount, discount.id);

        //    Discount replicated = null;
        //    using (WorkshopDBContext ctx = dal.getContext()){
        //        replicated = ctx.Discounts.Find(discount.id);
        //    }
        //    Assert.IsNotNull(replicated.outcome);
        //}
    }
}
*/