using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.System_Service;

namespace WorkshopProject.Tests
{
    [TestClass()]
    public class StoreUnitTest
    {

        User storeOwner, someMember;
        GodObject god = new GodObject();
        int ownerId;
        int storeId;
        Store store;

        //[TestInitialize]
        public void Init()
        {
            ownerId = god.addMember("username", "password");
            storeId = god.addStore("store name", 1, ownerId);
            store = WorkShop.getStore(storeId);
            storeOwner = ConnectionStubTemp.getMember(ownerId);
            int someMemberId = god.addMember("some member", "some password");
            someMember = ConnectionStubTemp.getMember(someMemberId);


        }

        //[TestCleanup]
        public void Cealup()
        {
            god.removeStore(storeId, ownerId);
            god.removeMember("username");
            god.removeMember("some member");
        }

        [TestMethod()]
        [TestCategory("Store")]
        public void addProduct_test()
        {
            try
            {
                Init();
                int id = store.addProduct(storeOwner, "product name 1", "desc1", 10, "category 1");
                Assert.IsTrue(id != -1, "add product fail");

                id = store.addProduct(someMember, "product name 2", "desc2", 10, "category 1");
                Assert.IsTrue(id == -1, "user with wrong permission can adding products");
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("Store")]
        public void getProduct_test()
        {
            try
            {
                Init();
                int id = store.addProduct(storeOwner, "product name 1", "desc1", 10, "category 1");
                Product p = store.getProduct(id);
                Assert.IsTrue(id != -1, "add product fail");

                Assert.IsTrue(p.name == "product name 1");
                Assert.IsTrue(p.description == "desc1");
                Assert.IsTrue(p.getPrice() == 10);
                Assert.IsTrue(p.category == "category 1");
                Assert.IsTrue(p.getId() == id);
                Assert.IsTrue(p.amount == 0);
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("Store")]
        public void removeProductFromStore_test()
        {
            try
            {
                Init();
                int id = store.addProduct(storeOwner, "product name 1", "desc1", 10, "category 1");
                Product p = store.getProduct(id);
                Assert.IsTrue(id != -1, "add product fail");

                bool result = store.removeProductFromStore(storeOwner, p);
                Assert.IsTrue(result, "remove product fail");

                p = store.getProduct(id);
                Assert.IsTrue(p == null, "remove product fail");
            }
            finally
            {
                Cealup();
            }


        }

        [TestMethod()]
        [TestCategory("Store")]
        public void buyProduct_test()
        {
            try
            {
                Init();
                int amountToBuy = 6, originalAmount = 12;
                int id = store.addProduct(storeOwner, "product name 1", "desc1", 10, "category 1");
                Product p = store.getProduct(id);
                Assert.IsTrue(id != -1, "add product fail");

                bool addToStock = store.addProductTostock(storeOwner, p, originalAmount);
                Assert.IsTrue(addToStock, "add product to stock fail");


                Store.callback callback1 = store.buyProduct(p, originalAmount + 1);
                Assert.IsTrue(callback1 == null && p.amount == originalAmount, "Error: Test amount bigger than the current amount in the stock ");

                Store.callback callback2 = store.buyProduct(p, amountToBuy);
                Assert.IsTrue(p.amount == originalAmount - amountToBuy, "buy product fail");

                callback2();
                Assert.IsTrue(p.amount == originalAmount, "Error: callback to return the products to the shelf fail");

            }
            finally
            {
                Cealup();
            }

        }


        [TestMethod()]
        [TestCategory("Store")]
        public void addProductToStock_test()
        {
            try
            {
                Init();
                int amountToBuy = 12;
                int id = store.addProduct(storeOwner, "product name 1", "desc1", 10, "category 1");
                Product p = store.getProduct(id);
                Assert.IsTrue(id != -1, "add product fail");
                Assert.IsTrue(p.amount == 0);

                bool addToStock = store.addProductTostock(storeOwner, p, amountToBuy);
                Assert.IsTrue(addToStock, "add product to stock fail");
                Assert.IsTrue(p.amount == amountToBuy);
            }
            finally
            {
                Cealup();
            }

        }

        [TestMethod()]
        [TestCategory("Store")]
        public void checkAvailability_test()
        {
            try
            {
                Init();
                int amountToBuy = 12;
                int id = store.addProduct(storeOwner, "product name 1", "desc1", 10, "category 1");
                Product p = store.getProduct(id);
                store.addProductTostock(storeOwner, p, amountToBuy);

                Assert.IsTrue(store.checkAvailability(p, 11), "Error: amount available and the system doesnt recognize it");
                Assert.IsTrue(store.checkAvailability(p, 0), "Error: amount available and the system doesnt recognize it");
                Assert.IsTrue(store.checkAvailability(p, 12), "Error: amount available and the system doesnt recognize it");
                Assert.IsTrue(!store.checkAvailability(p, 13), "Error: amount  not available and the system doesnt recognize it");

            }
            finally
            {
                Cealup();
            }
        }

        [TestMethod()]
        [TestCategory("Store")]
        public void changeProductInfo_test()
        {
            try
            {
                Init();
                int amountToBuy = 12;
                int id = store.addProduct(storeOwner, "product name 1", "desc1", 10, "category 1");
                Product p = store.getProduct(id);
                store.addProductTostock(storeOwner, p, amountToBuy);

                store.changeProductInfo(storeOwner, id, "new name", "new desc", 33.3, "new category", 20);
                Assert.IsTrue(p.name == "new name");
                Assert.IsTrue(p.description == "new desc");
                Assert.IsTrue(p.getPrice() == 33.3);
                Assert.IsTrue(p.category == "new category");
                Assert.IsTrue(p.getId() == id);
                Assert.IsTrue(p.amount == 20);
            }
            finally
            {
                Cealup();
            }


        }
    }
}
