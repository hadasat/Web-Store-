using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject
{
    public static class Tansaction
    {
        static int idCounter = 0;
        

        public static int purchase(User user)
        {
            int totalPrice = 0;
            ShoppingBasket basket = user.shoppingBasket;
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
            return idCounter++;
        }

        private static int calcPrice(Product p, int amount)
        {
            return amount;
        }
    }
}
