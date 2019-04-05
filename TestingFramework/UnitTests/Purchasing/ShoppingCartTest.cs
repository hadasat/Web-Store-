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
    public class ShoppingCartTest
    {

        ShoppingCart shoppingCart = new ShoppingCart();
        int amount1 = 10, amount2 = 20, amount3 = 30, amount4 = 40;
        Product p1, p2, p3, p4;

        [TestInitialize]
        public void Init()
        {
            p1 = new Product(10, "first", 10, Category.Categories.category1, 10, 10);
            p2 = new Product(20, "second", 20, Category.Categories.category2, 20, 20);
            p3 = new Product(30, "third", 30, Category.Categories.category3, 30, 30);
            p4 = new Product(40, "five", 40, Category.Categories.category1, 40, 40);
            shoppingCart.addProducts(p1, amount1);
            shoppingCart.addProducts(p2, amount2);
            shoppingCart.addProducts(p3, amount3);
            shoppingCart.addProducts(p4, amount4);
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
            int newAmount = 20;
            Product testp = new Product(10, "testp", 10, Category.Categories.category1, 10, 10);
            shoppingCart.setProductAmount(testp, newAmount);
            int inCartAmount = shoppingCart.getProducts()[testp];

            Assert.AreEqual(newAmount, inCartAmount);

            shoppingCart.setProductAmount(testp, 2*newAmount);
            Assert.AreEqual(newAmount*2, 2*inCartAmount);

            Assert.IsFalse(shoppingCart.setProductAmount(p1, -20));
            Assert.AreEqual(shoppingCart.getProductAmount(p1), amount1);

        }

        

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void addProductsTest()
        {
            int newAmount = 20;
            Product testp = new Product(10, "testp", 10, Category.Categories.category1, 10, 10);
            shoppingCart.addProducts(testp, newAmount);
            int inCartAmount =shoppingCart.getProductAmount(testp);

            Assert.AreEqual(newAmount, inCartAmount);

            shoppingCart.addProducts(p1, newAmount);
            inCartAmount = shoppingCart.getProductAmount(p1);
            Assert.AreEqual(amount1 + newAmount, inCartAmount);

            shoppingCart.addProducts(p2, newAmount);
            inCartAmount = shoppingCart.getProductAmount(p2);
            Assert.AreEqual(amount2 + newAmount, inCartAmount);

            Assert.IsFalse(shoppingCart.addProducts(p4, -20));
            Assert.AreEqual(shoppingCart.getProductAmount(p4), amount4);
        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void getProductAmountTest()
        {
            int pAmount = shoppingCart.getProductAmount(p1);
            Assert.AreEqual(pAmount,amount1);
            pAmount = shoppingCart.getProductAmount(p2);
            Assert.AreEqual(pAmount, amount2);
            pAmount = shoppingCart.getProductAmount(p3);
            Assert.AreEqual(pAmount, amount3);
            pAmount = shoppingCart.getProductAmount(p4);
            Assert.AreEqual(pAmount, amount4);

            int newAmount = 20;
            Product testp = new Product(10, "testp", 10, Category.Categories.category1, 10, 10);
            shoppingCart.setProductAmount(testp, newAmount);
            pAmount = shoppingCart.getProductAmount(testp);
            Assert.AreEqual(pAmount, newAmount);

        }

    }
}