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
            p1 = new Product("first", 10, "h", 10, 10, 10);
            p2 = new Product("second", 20, "l", 20, 20, 20);
            p3 = new Product("third", 30, "k", 30, 30, 30);
            p4 = new Product("five", 40, "j", 40, 40, 40);
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
        [TestCategory("Unit Test")]
        public void setProductAmount()
        {
            int newAmount = 50;

            Product p5 = new Product("five", 50, "j", 50, 50,50);
            //positive amount
            Assert.IsTrue(shoppingBasket.setProductAmount(store1, p5, newAmount));
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));

            //stay the previous price
            Assert.IsFalse(shoppingBasket.setProductAmount(store1, p5, -newAmount));
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));

            //zero amount
            Assert.IsTrue(shoppingBasket.setProductAmount(store1, p5, 0));
            Assert.AreEqual(0, shoppingBasket.getProductAmount(p5));
            //check 

        }

        [TestMethod()]
        [TestCategory("Unit Test")]
        public void addProduct()
        {
            int newAmount = 50;
            Product p5 = new Product("five", 50, "Category.Categories.category1", 50, 50,50);
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
}