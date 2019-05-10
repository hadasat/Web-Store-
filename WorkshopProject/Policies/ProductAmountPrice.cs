using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shopping;

namespace WorkshopProject.Policies
{
    public class ProductAmountPrice
    {
        public Product product;
        public int amount;
        public double price;

        public ProductAmountPrice(Product product, int amount, double price)
        {
            this.product = product;
            this.amount = amount;
            this.price = price;
        }

        public ProductAmountPrice(){ }
        //public static Product FromJson(string json)
        //{
        //    return JsonConvert.DeserializeObject<Product>(json);
        //}

        public static List<ProductAmountPrice> translateCart(ShoppingCart cart)
        {
            List<ProductAmountPrice> output = new List<ProductAmountPrice>();
            Dictionary<Product, int> products = cart.products;
            foreach(KeyValuePair<Product,int> p in products)
            {
                ProductAmountPrice newProduct = new ProductAmountPrice(p.Key, p.Value, p.Key.price);
                output.Add(newProduct);
            }
            return output;
        }
    }
}
