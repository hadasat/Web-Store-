using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;

namespace Shopping
{
    public class ShoppingBasket
    {
        private Dictionary<Store, ShoppingCart> carts;

        public ShoppingBasket()
        {
            carts = new Dictionary<Store, ShoppingCart>();
        }

        public ShoppingBasket(ShoppingBasket s)
        {
            this.carts = new Dictionary<Store, ShoppingCart>();
            Dictionary<Store, ShoppingCart> carts = s.carts;
            foreach (KeyValuePair<Store, ShoppingCart> c in carts)
            {
                Store store = c.Key;
                ShoppingCart shopping = new ShoppingCart(c.Value);
                this.carts[store] = shopping;
            }
        }

        public bool addProduct(Store store, Product product, int amount)
        {
            if(carts.ContainsKey(store))
                return (carts[store].addProducts(product, amount));
            else
            {
                Carts.Add(store, new ShoppingCart());
                return carts[store].addProducts(product, amount);
            }
        }

        
        public Dictionary<Store, ShoppingCart> Carts
        {
            get { return carts; }

        }
    }
}
