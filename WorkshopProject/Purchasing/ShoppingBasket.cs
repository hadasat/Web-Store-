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
        private Dictionary<Store, ShoppingCart> carts { get; }

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
            return ((ShoppingCart)carts[store]).setProductAmount(product, amount);
        }

        public bool addProduct(Store store, Product product)
        {
            int curr_amount = carts[store].getProductAmount(product);
            return carts[store].setProductAmount(product, curr_amount + 1);
        }

        public Dictionary<Store, ShoppingCart> Carts
        {
            get { return carts; }

        }
    }
}
