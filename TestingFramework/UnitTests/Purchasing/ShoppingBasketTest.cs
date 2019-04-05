using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping;

namespace WorkshopProject.Tests
{
    [TestClass()]
    class ShoppingBasketTest
    {

    ShoppingBasket shoppingBasket = new ShoppingBasket();
    Store store1 = new Store(1, "store1", 1, true);
    Store store2 = new Store(2, "store2", 2, true);
    Product p1, p2, p3, p4;

        [TestInitialize]
        public void Init()
        {
            p1 = new Product(10, "first", 10, Category.Categories.category1, 10, 10);
            p2 = new Product(20, "second", 20, Category.Categories.category2, 20, 20);
            p3 = new Product(30, "third", 30, Category.Categories.category3, 30, 30);
            p4 = new Product(40, "five", 40, Category.Categories.category1, 40, 40);
            shoppingBasket.addProduct(store1, p1, 10);
            shoppingBasket.addProduct(store2, p2, 20);
            shoppingBasket.addProduct(store1, p3, 20);
            shoppingBasket.addProduct(store2, p4, 20);

        }

    [TestCleanup]
        public void Cealup()
        {
            shoppingBasket = new ShoppingBasket();
        }

        [TestMethod()]
        [TestCategory("Examples")]
        public void setProductAmount()
        {
            int newAmount = 50;
            Product p5 = new Product(50, "five", 50, Category.Categories.category1, 50, 50);
            //positive amount
            Assert.IsTrue(shoppingBasket.setProductAmount(store1,p5,newAmount));
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));

            //stay the previous price
            Assert.IsFalse(shoppingBasket.setProductAmount(store1, p5, -newAmount));
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));

            //zero amount
            Assert.IsTrue(shoppingBasket.setProductAmount(store1, p5, 0));
            Assert.AreEqual(0, shoppingBasket.getProductAmount(p5));

        }

        [TestMethod()]
        [TestCategory("Examples")]
        public void addProduct()
        {
            int newAmount = 50;
            Product p5 = new Product(50, "five", 50, Category.Categories.category1, 50, 50);
            //positive amount
            shoppingBasket.addProduct(store1, p5, newAmount);
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));

            //zeroamount
            shoppingBasket.addProduct(store1, p5, -newAmount);
            Assert.AreEqual(0, shoppingBasket.getProductAmount(p5));

            //negative amount
            Assert.IsFalse(shoppingBasket.addProduct(store1, p5, -newAmount));
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));
        }
}