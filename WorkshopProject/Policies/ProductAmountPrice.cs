using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WorkshopProject.Policies
{
    public class ProductAmountPrice
    {
        public Product product { get; set; }
        public int amount{ get; set; }
        public double price { get; set; }

        public ProductAmountPrice(Product product, int amount, double price)
        {
            this.product = product;
            this.amount = amount;
            this.price = price;
        }

        //public static Product FromJson(string json)
        //{
        //    return JsonConvert.DeserializeObject<Product>(json);
        //}
    }
}
