using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shopping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WorkshopProject.Policies
{
    [TestClass]
    public class ProductAmountPriceTest
    {
        ShoppingCart shoppingCart = new ShoppingCart();
        int[] amount = { 10, 20, 30, 40 };
        List<ProductAmountPrice> products = new List<ProductAmountPrice>();
        Product[] p = new Product[4];
        ProductAmountPrice[] pam = new ProductAmountPrice[4];

        [TestInitialize]
        public void Init()
        {
            p[0] = new Product("first", 10, "h", "g", 10, 10, 10);
            p[1] = new Product("second", 20, "l", "g", 20, 20, 20);
            p[2] = new Product("third", 30, "k", "g", 30, 30, 30);
            p[3] = new Product("five", 40, "j", "g", 40, 40, 40);

            shoppingCart.addProducts(p[1], amount[0]);
            shoppingCart.addProducts(p[2], amount[1]);
            shoppingCart.addProducts(p[3], amount[2]);
            shoppingCart.addProducts(p[3], amount[3]);

            pam[0] = new ProductAmountPrice(p[0], amount[0], 10);
            pam[1] = new ProductAmountPrice(p[1], amount[1], 20);
            pam[2] = new ProductAmountPrice(p[2], amount[2], 30);
            pam[3] = new ProductAmountPrice(p[3], amount[3], 40);

            products.Add(pam[0]);
            products.Add(pam[1]);
            products.Add(pam[2]);
            products.Add(pam[3]);
        }

        [TestCleanup]
        public void Cleanup()
        {
            shoppingCart = new ShoppingCart();
            products = new List<ProductAmountPrice>();
        }

        [TestMethod]
        [TestCategory("POLICIES")]
        public void translateCartTest() {
            List<ProductAmountPrice> list = ProductAmountPrice.translateCart(shoppingCart);
            Assert.AreEqual(list.Count, shoppingCart.products.Count);

            foreach(ProductAmountPrice item in list)
            {                
                Assert.IsNotNull(shoppingCart.products[item.product]);
                Assert.AreEqual(item.amount, shoppingCart.getProductAmount(item.product));
            }

        }

        [TestMethod]
        [TestCategory("POLICIES")]
        public void sumProduct()
        {
            int listSum = ProductAmountPrice.sumProduct(products);
            Assert.AreEqual(100, listSum);

        }
    }
}
