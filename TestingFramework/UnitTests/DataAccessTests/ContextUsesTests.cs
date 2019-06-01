using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;

namespace TestingFramework.UnitTests.DataAccessTests
{
    [TestClass]
    public class ContextUsesTests
    {



        public void StoreTest()
        {

            Repo repo = new Repo();


            Store store = new Store();

            repo.Add(store);

            Product product1 = new Product();
            Product product2 = new Product();
            Product product3 = new Product();

            repo.Add(product1); repo.Add(product2); repo.Add(product3);

        }
    }
}
