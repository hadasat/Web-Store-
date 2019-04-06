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
        Product p1, p2, p3, p4;
        User user = new User();

        [TestInitialize]
        public void Init()
        {
            p1 = new Product(10, "first", 10, Category.Categories.category1, 10, 10);
            p2 = new Product(20, "second", 20, Category.Categories.category2, 20, 20);
            p3 = new Product(30, "third", 30, Category.Categories.category3, 30, 30);
            p4 = new Product(40, "five", 40, Category.Categories.category1, 40, 40);
            user.shoppingBasket.addProduct(store1, p1, 10);
            user.shoppingBasket.addProduct(store2, p2, 20);
            user.shoppingBasket.addProduct(store1, p3, 20);
            user.shoppingBasket.addProduct(store2, p4, 20);
        }


        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void purchaseTest()
        {
            //chack if the purchase sucesess
            int transactionId = Transaction.purchase(user);
            Assert.IsTrue(transactionId > 0);

            //check if the product remove from user
            int productAmount = user.shoppingBasket.getProductAmount(p1);
            Assert.Equals(productAmount, 0);

            //check purchase fail becouse basket empty
            transactionId = Transaction.purchase(user);
            Assert.IsTrue(transactionId == -1);


            Product p5 = new Product(40, "five", 40, Category.Categories.category1, 40, 40);
            user.shoppingBasket.addProduct(store1,p5, 10);
            //check transction number grow
            Assert.IsTrue(Transaction.purchase(user) > transactionId);
        }

    }
}
