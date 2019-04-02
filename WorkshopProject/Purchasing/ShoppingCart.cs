using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{

    class ShoppingCart
    {
        Dictionary<Product,int> products;

        public ShoppingCart()
        {
            products = new Dictionary<Product, int>();
        }

        public int setProductAmount(Product product,int amount)
        {
            if( amount >= 0)
            {
                products[product] = amount;
                return 0;
            }
            return -1;
        }

        public Dictionary<Product, int> getProducts()
        {
            return products;
        }


        public int addProduct(Product product)
        {
            if (products.ContainsKey(product))
                products[product]++;
            products[product] = 1;
            return 0;
        }

    }
}
