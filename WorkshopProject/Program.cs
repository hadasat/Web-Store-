using Newtonsoft.Json;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Examples;

namespace WorkshopProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ShoppingBasket s = new ShoppingBasket();
            Product p = new Product(1, "p", 10, Category.Categories.category1, 100, 1000);
            Product p1 = new Product(1, "p", 10, Category.Categories.category1, 100, 1000);
            Product p2 = new Product(1, "p", 10, Category.Categories.category1, 100, 1000);
            Product p3 = new Product(1, "p", 10, Category.Categories.category1, 100, 1000);
            Store store = new Store(22, "hadas", 220, true);
            Store store2 = new Store(222, "hadas", 2220, true);
            s.addProduct(store, p, 30);
            s.addProduct(store2, p1, 10);
            s.addProduct(store2, p2, 10);
            s.addProduct(store, p3, 10);
            Dictionary<Product, int> products = new Dictionary<Product, int>();
            products.Add(p, 1);
            products.Add(p1, 2);
            products.Add(p2, 3);
            products.Add(p3, 4);

            //foreach () { }
            String f = JsonConvert.SerializeObject(s);
            Console.WriteLine(f);
            List<Product> pr = new List<Product>();

            ShoppingBasket g = JsonConvert.DeserializeObject<ShoppingBasket>(f);
            Console.ReadLine();

        }


        static void Examples()
        {
            LogExample.RunMe();
        }
        
    }
}
