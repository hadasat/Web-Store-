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
        private Dictionary<Product, int> products;

        public ShoppingCart()
        {
            products = new Dictionary<Product, int>();
        }

        public ShoppingCart(ShoppingCart s)
        {
            this.products = new Dictionary<Product, int>();
            Dictionary<Product, int> products = s.getProducts();
            foreach (KeyValuePair<Product, int> c in products)
            {
                Product product = c.Key ;
                int amount = c.Value;
                this.products[product] = amount;
            }
        }

        public bool setProductAmount(Product product, int amount)
        {
            if (amount >= 0)
            {
                products[product] = amount;
                return true;
            }
            return false;
        }

        public int getProductAmount(Product product)
        {
            return products[product];
        }

        public Dictionary<Product, int> getProducts()
        {
            return products;
        }


        public bool addProduct(Product product)
        {
            if (products.ContainsKey(product))
                products[product]++;
            products[product] = 1;
            return true;
        }

        public bool addProduct(Product product,int amount)
        {
            if (products.ContainsKey(product))
                products[product] += amount;
            else
                products[product] = amount;
            return true;
        }
    }
}
