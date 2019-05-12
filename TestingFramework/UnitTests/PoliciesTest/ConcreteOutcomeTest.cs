using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    [TestClass]
    public class ConcreteOutcomeTest
    {
        int[] amount = { 10, 20, 30, 40 };
        Store store;
        Product product;
        int storeId, productId;
        Member member = new Member();

        [TestInitialize]
        public void Init()
        {
            storeId = WorkShop.createNewStore("testStore", 10, true, member);
            store = WorkShop.getStore(storeId);
            productId = store.addProduct(member, "testproduct", "A", 10, "B");
            product = store.findProduct(productId);

        }

        [TestCleanup]
        public void Cleanup()
        {
            WorkShop.stores.Remove(storeId);
        }

        [TestMethod]
        public void FreeProductApplyTest()
        {
            List<ProductAmountPrice> list = Factory.getList();
            User user = Factory.getUser();
            int oldCapacity = ProductAmountPrice.sumProduct(list);

            FreeProduct free = new FreeProduct(productId, 2);
            List<ProductAmountPrice> withFreeProduct = free.Apply(list, user);
            int newCapacity = ProductAmountPrice.sumProduct(withFreeProduct);

            Assert.AreEqual(oldCapacity + 2, newCapacity);
            ProductAmountPrice ppa = new ProductAmountPrice(product, 1, 1);
            Assert.IsTrue(withFreeProduct.Contains(ppa));
        }

        [TestMethod]
        public void PercentageApplyTest() {
            List<ProductAmountPrice> list = Factory.getList();
            User user = Factory.getUser();
            double oldTotalPrice = calcPrice(list) ; 

            Percentage percentage = new Percentage(50);
            list = percentage.Apply(list, user);
            double newTotalPrice = calcPrice(list);

            Assert.AreEqual(oldTotalPrice, newTotalPrice*2);

        }

        private double calcPrice(List<ProductAmountPrice> list)
        {
            double sum = 0;
            foreach (ProductAmountPrice p in list)
                sum += p.price;
            return sum;
        }   

    }


}