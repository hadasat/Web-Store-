using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    class Tansaction
    {
        static int idCounter = 0;
        int id; 

        public Tansaction()
        {
            id = idCounter++; 
        }

        public int purchase(String user,ShoppingBasket basket)
        {
            int totalPrice = 0;
            Dictionary<Store, ShoppingCart> carts = basket.getCarts();
            //calc toal price
            foreach (KeyValuePair<Store, ShoppingCart> c in carts)
            {
                //check if the item in the stock and add the price
                Dictionary<Product, int> itemsPerStore = c.Value.getProducts();
                foreach (KeyValuePair<Product, int> item in itemsPerStore) {
                    totalPrice += calcPrice(item.Key, item.Value);
                        }
            }
            
            //parches 

            //send products
            return 0;
        }

        private int calcPrice(Product p, int amount)
        {
            return 1;
        }
    }
}
