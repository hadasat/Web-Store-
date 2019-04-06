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
            ShoppingCart c = new ShoppingCart();
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


            c.addProducts(p, 1);
            c.addProducts(p1, 2);
            c.addProducts(p2, 3);
            c.addProducts(p3, 4);


            JsonShoppingCart shp = new JsonShoppingCart(c);

            String f = JsonConvert.SerializeObject(shp, Formatting.Indented);
            Console.WriteLine(f);
            //Object[] array1 = JsonConvert.DeserializeObject< Object[]>(f);

            // ShoppingBasket g = JsonConvert.DeserializeObject<ShoppingBasket>(f);
            Console.ReadLine();

        }


        static void Examples()
        {
            LogExample.RunMe();
        }
        
    }
}
