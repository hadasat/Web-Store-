using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
