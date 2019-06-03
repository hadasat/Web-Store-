using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;

namespace WorkshopProject.Policies
{
    public static class Factory
    {
    
        public static List<ProductAmountPrice> getList()
        {
            int[] amount = { 10, 20, 30, 40 };
            List<ProductAmountPrice> products = new List<ProductAmountPrice>();
            Product[] p = new Product[4];
            ProductAmountPrice[] pam = new ProductAmountPrice[4];

            p[0] = new Product("first", 10, "h", "A", 10, 10, 10);
            p[1] = new Product("second", 20, "l", "B", 20, 20, 20);
            p[2] = new Product("third", 30, "k", "B", 30, 30, 30);
            p[3] = new Product("five", 40, "j", "A", 40, 40, 40);

            pam[0] = new ProductAmountPrice(p[0], amount[0], 10);
            pam[1] = new ProductAmountPrice(p[1], amount[1], 20);
            pam[2] = new ProductAmountPrice(p[2], amount[2], 30);
            pam[3] = new ProductAmountPrice(p[3], amount[3], 40);

            products.Add(pam[0]);
            products.Add(pam[1]);
            products.Add(pam[2]);
            products.Add(pam[3]);

            return products;
        }

        public static User getUser()
        {
            return new Member("hadas", new DateTime(1994, 2, 27), "Isreal");
        }
    }

        [TestClass]
    public class AllProductsFilterTest
    {
        [TestMethod]
        [TestCategory("POLICIES")]
        public void AllProductsgetFilteredItemsTest() {
            AllProductsFilter filter = new AllProductsFilter();
            List<ProductAmountPrice> list = Factory.getList();

            Assert.AreEqual(list.Count, filter.getFilteredItems(list).Count);
        }
    }

    [TestClass]
    public class ProductListFilterTest
    {
        [TestMethod]
        [TestCategory("POLICIES")]
        public void ProductListgetFilteredItemsTest() {
            List<ProductAmountPrice> list = Factory.getList();

            List<int> idList = new List<int>();
            idList.Add(list.ElementAt(0).product.id);
            idList.Add(list.ElementAt(1).product.id);

            ProductListFilter filter = new ProductListFilter(idList);
            List<ProductAmountPrice> filterdList = filter.getFilteredItems(list);

            Assert.AreEqual(idList.Count, filterdList.Count);
            Assert.IsTrue(filterdList.Contains(list.ElementAt(0)));
            Assert.IsTrue(filterdList.Contains(list.ElementAt(1)));
            Assert.IsFalse(filterdList.Contains(list.ElementAt(2)));
            Assert.IsFalse(filterdList.Contains(list.ElementAt(3)));

        }
    }

    [TestClass]
    public class CategoryFilterTest
    {
        [TestMethod]
        [TestCategory("POLICIES")]
        public void CategorygetFilteredItemsTest() {
            List<ProductAmountPrice> list = Factory.getList();

            CategoryFilter filter = new CategoryFilter("B");
            List<ProductAmountPrice> filterdList = filter.getFilteredItems(list);

            Assert.AreEqual(2, filterdList.Count);
            foreach(ProductAmountPrice p in filterdList)
            {
                Assert.AreEqual("B",p.product.category);
            }


        }
    }

}