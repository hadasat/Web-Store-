using Shopping;
using System.Collections.Generic;
using Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TansactionsNameSpace;
using WorkshopProject.External_Services;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.Tests
{
    [TestClass()]
    public class TransactionTest
    {

        Store store1, store2;
        int store1Id, store2Id;
        Product[] pro = new Product[4];
        User user = new User();
        Member menager = new Member("TestMember");

        [TestInitialize]
        public void Init()
        {
            DataAccessDriver.UseStub = true;
            store1Id = WorkShop.createNewStore("TestStore1", 1, true, menager);
            store2Id = WorkShop.createNewStore("TestStore2", 2, true, menager);
            store1 = WorkShop.getStore(store1Id);
            store2 = WorkShop.getStore(store2Id);

            pro[0] = new Product("first", 10, "h", "g", 10, 10, store1Id);
            pro[1] = new Product("second", 20, "l", "g", 20, 20, store2Id);
            pro[2] = new Product("third", 30, "k", "g", 30, 30, store1Id);
            pro[3] = new Product("five", 40, "j", "g", 40, 40, store2Id);
            user.shoppingBasket.addProduct(store1, pro[0], 10);
            user.shoppingBasket.addProduct(store2, pro[1], 20);
            user.shoppingBasket.addProduct(store1, pro[2], 20);
            user.shoppingBasket.addProduct(store2, pro[3], 20);

            //adding products to store
            store1.GetStock().Add(pro[0].id, pro[0]);
            store2.GetStock().Add(pro[1].id, pro[1]);
            store1.GetStock().Add(pro[2].id, pro[2]);
            store2.GetStock().Add(pro[3].id, pro[3]);

        }


        [TestCleanup]
        public void Cleanup()
        {
            WorkShop.Remove(store1.id);
            WorkShop.Remove(store2.id);
            DataAccessDriver.UseStub = false;
        }

        [TestMethod]
        [TestCategory("Unit test - TransactionTest")]
        [TestCategory("Regression")]
        public async void purchaseTest()
         {
            try
            {
                Init();
                //chack if the purchase sucesess
                int  ccv = 0, month =10,  year = 2050, id=123456789 ;
                string holder = "mosh moshe",city = "shit",country="shit",zip="12345",address = "",cardNumber = "0";
                Transaction transaction = new Transaction();
                await transaction.doTransaction(user, cardNumber,month,year, holder, ccv,id, holder, address,city,country,zip,new PaymentStub (true),new SupplyStub(true));
                int transactionId = transaction.id;
                Assert.IsTrue(transactionId > 0, "fail to purchase legal transaction trans id:" + transactionId);

                //check if the product remove from user
                int productAmount = user.shoppingBasket.getProductAmount(pro[0]);
                Assert.AreEqual( 0, productAmount, "products stay in user basket");

                //check purchase fail becouse basket empty
                try
                {
                    transaction = new Transaction();
                    await transaction.doTransaction(user, cardNumber, month, year, holder, ccv, id, holder, address, city, country, zip, new PaymentStub(true), new SupplyStub(true));
                    Assert.IsTrue(false, "didn't fail on empty basket");
                }catch
                {   
                }


                //Product p5 = new Product("five", 40, "Category.Categories.category1", "g", 40, 40, 40);
                //user.shoppingBasket.addProduct(store1, p5, 10);
                //store1.GetStock().Add(p5.id, p5);
                ////check transction number grow
                //transaction = new Transaction(user, cardNumber, month, year, holder, ccv, id, holder, address, city, country, zip, new PaymentStub(true), new SupplyStub(true));
                //Assert.IsTrue(transaction.id > transactionId, "fail to update transaction id");
            }
            finally
            {
                Cleanup();
            }
        }

    }
}
