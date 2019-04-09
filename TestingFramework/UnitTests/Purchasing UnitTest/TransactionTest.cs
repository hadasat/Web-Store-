using Shopping;
using System.Collections.Generic;
using Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tansactions;

namespace WorkshopProject.Tests
{
    [TestClass()]
    public class TransactionTest
    {

        Store store1 = new Store(1, "store1", 1, true);
        Store store2 = new Store(2, "store2", 2, true);
        Product[] p = new Product[4];
        User user = new User();

        [TestInitialize]
        public void Init()
        {
            p[0] = new Product("first", 10, "h", "g", 10, 10, 10);
            p[1] = new Product("second", 20, "l", "g", 20, 20, 20);
            p[2] = new Product("third", 30, "k", "g", 30, 30, 30);
            p[3] = new Product("five", 40, "j", "g", 40, 40, 40);
            user.shoppingBasket.addProduct(store1, p[0], 10);
            user.shoppingBasket.addProduct(store2, p[1], 20);
            user.shoppingBasket.addProduct(store1, p[2], 20);
            user.shoppingBasket.addProduct(store2, p[3], 20);
        }


        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod()]
        [TestCategory("Unit test - TransactionTest")]
        public void purchaseTest()
         {
            try
            {
                Init();
                //chack if the purchase sucesess
                int transactionId = Transaction.purchase(user);
                Assert.IsTrue(transactionId > 0, "fail to purchase legal transaction");

                //check if the product remove from user
                int productAmount = user.shoppingBasket.getProductAmount(p[0]);
                Assert.AreEqual(productAmount, 0, "products stay in user basket");

                //check purchase fail becouse basket empty
                transactionId = Transaction.purchase(user);
                Assert.IsTrue(transactionId == -1, "fail to unpurchase ilegal transaction");


                Product p5 = new Product("five", 40, "Category.Categories.category1", "g", 40, 40, 40);
                user.shoppingBasket.addProduct(store1, p5, 10);
                //check transction number grow
                Assert.IsTrue(Transaction.purchase(user) > transactionId, "fail to update transaction id");
            }
            finally
            {
                Cleanup();
            }
        }

    }
}
