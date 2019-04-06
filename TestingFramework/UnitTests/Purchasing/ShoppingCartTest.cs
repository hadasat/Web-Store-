using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using Shopping;

namespace WorkshopProject.Tests
{
    [TestClass()]
    public class ShoppingCartTest
    {
        ShoppingCart shoppingCart = new ShoppingCart();
        int[] amount = { 10, 20, 30, 40 };
        Product p1, p2, p3, p4;

        [TestInitialize]
        public void Init()
        {
            p1 = new Product(10, "first", 10, Category.Categories.category1, 10, 10);
            p2 = new Product(20, "second", 20, Category.Categories.category2, 20, 20);
            p3 = new Product(30, "third", 30, Category.Categories.category3, 30, 30);
            p4 = new Product(40, "five", 40, Category.Categories.category1, 40, 40);
            shoppingCart.addProducts(p1, amount[0]);
            shoppingCart.addProducts(p2, amount[1]);
            shoppingCart.addProducts(p3, amount[2]);
            shoppingCart.addProducts(p4, amount[3]);
        }

        [TestCleanup]
        public void Cleanup()
        {
            shoppingCart = new ShoppingCart();
        }
        
        [TestMethod()]
        [TestCategory("UnitTest")]
        public void SetProductAmountTest()
        {
            //adding new item
            int newAmount = 20;
            Product testp = new Product(10, "testp", 10, Category.Categories.category1, 10, 10);
            shoppingCart.setProductAmount(testp, newAmount);
            int inCartAmount = shoppingCart.getProducts()[testp];
            Assert.AreEqual(newAmount, inCartAmount);

            //mul amount 
            shoppingCart.setProductAmount(testp, 2*newAmount);
            Assert.AreEqual(newAmount*2, 2*inCartAmount);

            //bad amount
            Assert.IsFalse(shoppingCart.setProductAmount(p1, -20));
            Assert.AreEqual(shoppingCart.getProductAmount(p1), amount[0]);

            //zero amount - remove product from list
            Assert.IsTrue(shoppingCart.setProductAmount(testp, 0));
            Assert.Equals(shoppingCart.getProductAmount(testp), 0);
            Assert.IsFalse(shoppingCart.products.ContainsKey(testp));
        }

        

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void addProductsTest()
        {
            //adding new item
            int newAmount = 20;
            Product testp = new Product(10, "testp", 10, Category.Categories.category1, 10, 10);
            shoppingCart.addProducts(testp, newAmount);
            int inCartAmount =shoppingCart.getProductAmount(testp);
            Assert.AreEqual(newAmount, inCartAmount);

            //adding for old item
            shoppingCart.addProducts(p1, newAmount);
            inCartAmount = shoppingCart.getProductAmount(p1);
            Assert.AreEqual(amount[0] + newAmount, inCartAmount);

            shoppingCart.addProducts(p2, newAmount);
            inCartAmount = shoppingCart.getProductAmount(p2);
            Assert.AreEqual(amount[1] + newAmount, inCartAmount);

            //adding negativ number
            Assert.IsFalse(shoppingCart.addProducts(p4, -20));
            //item old amount 
            Assert.AreEqual(shoppingCart.getProductAmount(p4), amount[3]);
        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void getProductAmountTest()
        {
            //check matching amount
            int pAmount = shoppingCart.getProductAmount(p1);
            Assert.AreEqual(pAmount,amount[0]);
            pAmount = shoppingCart.getProductAmount(p2);
            Assert.AreEqual(pAmount, amount[1]);
            pAmount = shoppingCart.getProductAmount(p3);
            Assert.AreEqual(pAmount, amount[2]);
            pAmount = shoppingCart.getProductAmount(p4);
            Assert.AreEqual(pAmount, amount[3]);

            //adding amount
            int newAmount = 20;
            Product testp = new Product(10, "testp", 10, Category.Categories.category1, 10, 10);
            shoppingCart.setProductAmount(testp, newAmount);
            pAmount = shoppingCart.getProductAmount(testp);
            Assert.AreEqual(pAmount, newAmount);

        }
        
        [TestMethod()]
        [TestCategory("UnitTest")]
        public void getTotalAmountTest()
        {
            //defult amount
            int currAmount = shoppingCart.getTotalAmount();
            int goalAmmount = amount.Sum();
            Assert.Equals(currAmount, goalAmmount);

            //adding new product 
            Product newp = new Product(10, "first", 10, Category.Categories.category1, 10, 10);
            shoppingCart.addProducts(newp, 20);
            goalAmmount += 20;
            currAmount = shoppingCart.getTotalAmount();
            Assert.Equals(currAmount, goalAmmount);
            

        }
    }
}