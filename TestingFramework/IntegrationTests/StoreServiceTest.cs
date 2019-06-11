using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.System_Service;

namespace IntegrationTests
{
    public class Response
    {
        public int id;
        public string message;
    }


    [TestClass()]
    public class StoreServiceTest
    {
        SystemServiceImpl service;
        GodObject god;
        public string successMsg = "Success";
        string name = "p", desc = "d", cat = "c", keyword = "key";
        int price = 1, amount = 2, rank = 1, amountToAdd = 3;
        int store_id, owner_id;
        int pid;
        //Member member;


        //[TestInitialize]
        public void init()
        {
            god = new GodObject();
            god.clearDb();
            DataAccessDriver.UseStub = true;
            service = new SystemServiceImpl();
            //service.Register("user", "123");
            try { owner_id = god.addMember("owner", "123"); } catch { };
            service.login("owner", "123");
            try { store_id = god.addStore("store_test", 1, owner_id); } catch { };
            try { pid = god.addProductToStore(store_id, name, price, cat, desc, keyword, amount, rank); } catch { };
        }

        //[TestCleanup]
        public void Cealup()
        {
            god.cleanUpAllData();
            DataAccessDriver.UseStub = false;
        }

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        private void successJasonTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        private void generateMessageFormatJasonTest()
        {
            throw new NotImplementedException();
        }

        //[TestMethod()]
        //[TestCategory("StoreServiceTest")]
        //private void addDiscountPolicyTest()
        //{
        //    throw new NotImplementedException();
        //}

            private string getMsg(string msg)
        {
            return JsonConvert.DeserializeObject<Response>(msg).message;
        }

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        public void AddProductToStockTest()
        {

            try
            {
                init();
                string res = service.AddProductToStock(store_id, pid, amountToAdd);
                Assert.IsTrue(getMsg(res) == successMsg);
                Product p = WorkShop.getStore(store_id).getProduct(pid);
                Assert.IsTrue(p.amount == amount + amountToAdd);
            }
            finally
            {
                Cealup();
            }



        }
            
        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        public void AddProductToStoreTest()
        {
            try
            {
                init();
                string name = "p", desc = "d", cat = "c";
                int price = 1;
                string res = service.AddProductToStore(store_id, name, desc, price, cat);
                Response mes = JsonConvert.DeserializeObject<Response>(res);
                Assert.IsTrue(mes.id >= 0);


                Product p = WorkShop.getStore(store_id).getProduct(mes.id);
                Assert.IsTrue(p != null);
                Assert.IsTrue(p.name == name && p.description == desc && p.price == price && p.category == cat);
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        public void AddStoreTest()
        {
            try
            {
                init();
                string name = "name";
                string res = service.AddStore(name);
                Response mes = JsonConvert.DeserializeObject<Response>(res);
                Assert.IsTrue(mes.id >= 0);
                Assert.IsTrue(WorkShop.getStore(mes.id) != null);
            }
            finally
            {
                Cealup();
            }


        }

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        public void ChangeProductInfoTest()
        {
            try
            {
                init();
                string nName = "new name", nDesc = "new D", nCat = "nCat";
                int nPrice = 11, nAmount = 100;
                string res = service.ChangeProductInfo(store_id, pid, nName, nDesc, nPrice, nCat, nAmount);
                Assert.IsTrue(getMsg(res) == successMsg);

                Product p = WorkShop.getStore(store_id).getProduct(pid);
                Assert.IsTrue(p != null);
                Assert.IsTrue(p.name == nName && p.description == nDesc && p.price == nPrice && p.category == nCat && p.amount == nAmount, "updtae  didnt happend");

            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        public void CloseStoreTest()
        {
            try
            {
                init();
                string res = service.closeStore(store_id);
                Assert.IsTrue(getMsg(res) == successMsg);

                Assert.IsTrue(WorkShop.getStore(store_id).isActive == false, "store still exist");
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        public void GetProductInfoTest()
        {
            try
            {
                init();
                string res = service.GetProductInfo(pid);
                Product p = JsonConvert.DeserializeObject<Product>(res);

                Assert.IsTrue(p != null);
                Assert.IsTrue(p.name == name && p.description == desc && p.price == price && p.category == cat);
            }
            finally
            {
                Cealup();
            }

        }

        //[TestMethod()]
        //[TestCategory("StoreServiceTest")]
        //public void removeDiscountPolicyTest()
        //{
        //    throw new NotImplementedException();
        //}

        [TestMethod()]
        [TestCategory("StoreServiceTest")]
        public void RemoveProductFromStoreTest()
        {
            try
            {
                init();
                string res = service.RemoveProductFromStore(store_id, pid);
                Assert.IsTrue(getMsg(res) == successMsg);
                Assert.IsTrue(WorkShop.getStore(store_id).getProduct(pid) == null,
                    "product didnt removed");
            }
            finally
            {
                Cealup();
            }

        }

  

        //[TestMethod()]
        //[TestCategory("StoreServiceTest")]
        //private void removePurchasingPolicyTest()
        //{
        //    throw new NotImplementedException();
        //}


    }
}