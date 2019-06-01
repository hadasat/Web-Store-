using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.Policies;

namespace TestingFramework.UnitTests.DataAccessTests
{
    [TestClass]
    public class ContextUsesTests
    {
        [TestMethod()]
        [TestCategory("DAL - Repo")]
        public void StoreTest()
        {
            Repo repo = new Repo();

            Store store = new Store();

            repo.Add(store);

            Product product1 = new Product();
            Product product2 = new Product();
            Product product3 = new Product();

            repo.Add(product1); repo.Add(product2); repo.Add(product3);

            store.Stocks.Add(new Stock(10, product1));
            store.Stocks.Add(new Stock(10, product2));
            store.Stocks.Add(new Stock(10, product3));

            store.purchasePolicy.Add(new TrueCondition());


            repo.Update(store);

            Store getStore = (Store) repo.Get<Store>(store.GetKey());

            getStore.name = "idk";
            repo.Update<Store>(getStore);

            Assert.AreSame(getStore, store);
            Assert.IsNotNull(getStore.Stocks);

            Assert.IsNotNull(getStore.Stocks);



            getStore = (Store)repo.Get<Store>(store.GetKey());
            Assert.IsNotNull(getStore.Stocks[0].product);
            Assert.AreEqual(getStore.name, "idk");

            //--------------------



            repo = new Repo();

            Store store2 = new Store();



            getStore = (Store)repo.Get<Store>(store.GetKey());

            getStore.name = "idk";
            repo.Update<Store>(getStore);

            Assert.AreSame(getStore, store);
            Assert.IsNotNull(getStore.Stocks);

            Assert.IsNotNull(getStore.Stocks);



            getStore = (Store)repo.Get<Store>(store.GetKey());
            Assert.IsNotNull(getStore.Stocks[0].product);
            Assert.AreEqual(getStore.name, "idk");



            //repo.Clear<Store>();
            // Assert.IsNull(repo.Get<Store>(store.GetKey()));
        }
    }
}
