using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tansactions;
using Users;
using WorkshopProject.Examples;
using WorkshopProject.System_Service;

namespace WorkshopProject
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Store store1 = new Store(1, "store1", 1, true);
            Store store2 = new Store(2, "store2", 2, true);
            Product p1, p2, p3, p4;
            User user = new User();

            p1 = new Product("first", 10, "h", "g", 10, 10, 10);
            p2 = new Product("second", 20, "l", "g", 20, 20, 20);
            p3 = new Product("third", 30, "k", "g", 30, 30, 30);
            p4 = new Product("five", 40, "j", "g", 40, 40, 40);
            user.shoppingBasket.addProduct(store1, p1, 10);
            user.shoppingBasket.addProduct(store2, p2, 20);
            user.shoppingBasket.addProduct(store1, p3, 20);
            user.shoppingBasket.addProduct(store2, p4, 20);

            //Console.WriteLine(Transaction.purchase(user));
            Dictionary<int, int> ret = new Dictionary<int, int>();
            ShoppingCart sc = user.shoppingBasket.carts[store1];
            Console.WriteLine(sc.setProductAmount(p1, -1));
            JsonShoppingCart jsc = new JsonShoppingCart(user.shoppingBasket.carts[store1]);
            string msg = JsonConvert.SerializeObject(jsc,Formatting.Indented);
            JObject json = JObject.Parse(msg);
            JArray productsAndAmounts = (JArray)json["products"];

            SystemServiceImpl f = new SystemServiceImpl();
           
            //f.AddProductToBasket(p1, 20);



            foreach (JObject pair in productsAndAmounts)
            {
                int productId = (int)pair["product"]["id"];
                int amount = (int)pair["amount"];
                Console.WriteLine("pId: " + productId + " amount: " +amount + "\n");
                ret.Add(productId, amount);
            }
            

            Console.ReadLine();
        }


        static void Examples()
        {
            LogExample.RunMe();
        }
        
    }
}
