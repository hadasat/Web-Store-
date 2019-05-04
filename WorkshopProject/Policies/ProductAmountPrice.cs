using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
