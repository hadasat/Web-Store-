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
        Product p1;

        [TestInitialize]
        public void Init()
        {
            p1 = new Product("first", 10, "h", "g", 10, 10, 10);
            shoppingBasket.addProduct(store1, p1, 10);


        }

        [TestCleanup]
        public void Cealup()
        {
            shoppingBasket = new ShoppingBasket();
        }

        [TestMethod()]
        [TestCategory("Unit Test")]
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
        [TestCategory("Unit Test")]
        public void addProductTest()
        {
            int newAmount = 50;
            Product p5 = new Product("five", 50, "Category.Categories.category1", "g", 50, 50, 50);
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
        

        public void getProductAmountTest()
        {
        }
    }
}