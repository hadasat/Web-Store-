using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{

    public class ShoppingCart
    {
        Dictionary<Product, int> products;

        public ShoppingCart()
        {
            products = new Dictionary<Product, int>();
        }

        public int setProductAmount(Product product, int amount)
        {
            if (amount >= 0)
            {
                products[product] = amount;
                return 0;
            }
            return -1;
        }

        public int getProductAmount(Product product)
        {
            return products[product];
        }

        public Dictionary<Product, int> getProducts()
        {
            return products;
        }

    }
}
