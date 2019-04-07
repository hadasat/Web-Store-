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
    public class ShoppingBasketTest
    {

        ShoppingBasket shoppingBasket = new ShoppingBasket();
        Store store1 = new Store(1, "store1", 1, true);
        Product p1;
        int amount1 = 10;

        [TestInitialize]
        public void Init()
        {
            p1 = new Product("first", 10, "h", "g", 10, 10, 10);
            shoppingBasket.addProduct(store1, p1, amount1);


        }

        [TestCleanup]
        public void Cealup()
        {
            shoppingBasket = new ShoppingBasket();
        }

        [TestMethod()]
        [TestCategory("Unit test - ShoppingBasketTest")]
        public void setProductAmountTest()
        {
            int newAmount = 50;

            Product p5 = new Product("five", 50, "j", "g", 50, 50, 50);
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
        [TestCategory("Unit test - ShoppingBasketTest")]
        public void addProductTest()
        {
            int newAmount = 50;
            Product p5 = new Product("five", 50, "Category.Categories.category1", "g", 50, 50, 50);
            //positive amount
            shoppingBasket.addProduct(store1, p5, newAmount);
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));

            //negative amount
            Assert.IsFalse(shoppingBasket.addProduct(store1, p5, -newAmount));
            Assert.AreEqual(newAmount, shoppingBasket.getProductAmount(p5));
        }

        [TestMethod()]
        [TestCategory("Unit test - ShoppingBasketTest")]
        public void getProductAmountTest()
        {
            //existing product
            Assert.AreEqual(shoppingBasket.getProductAmount(p1), amount1);

            //new product
            int newAmount = 50;
            Product p5 = new Product("five", 50, "j", "g", 50, 50, 50);
            shoppingBasket.addProduct(store1, p5, newAmount);
            Assert.AreEqual(shoppingBasket.getProductAmount(p5), newAmount);
        }

        [TestMethod()]
        [TestCategory("Unit test - ShoppingBasketTest")]
        public void isEmptyTest()
        {
            Assert.IsFalse(shoppingBasket.isEmpty());
            shoppingBasket.setProductAmount(store1, p1, 0);
            Assert.IsTrue(shoppingBasket.isEmpty());


        }
    }
}