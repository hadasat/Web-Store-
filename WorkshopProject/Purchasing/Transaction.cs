using Shopping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;

namespace Tansactions
{
    public static class Transaction
    {
        static int transactionCounter = 0;


        public static int purchase(User user)
        {
            double totalPrice = 0;
            ShoppingBasket basket = user.shoppingBasket;
            Dictionary<Store, ShoppingCart> carts = basket.carts;
            //calc toal price
            foreach (KeyValuePair<Store, ShoppingCart> c in carts)
            {
                Store currStore = c.Key;
                ShoppingCart currShoppingCart = c.Value;

                //check if the item in the stock and add the price
                Dictionary<Product, int> itemsPerStore = currShoppingCart.getProducts();
                foreach (KeyValuePair<Product, int> item in itemsPerStore)
                {
                    // TODO buy from store
                    Product currProduct = item.Key;
                    int currAmount = item.Value;
                    //remove product from basket
                    basket.setProductAmount(currStore, currProduct, 0);
                    totalPrice += calcPrice(currProduct, currAmount);
                }
            }
            //parches 
            //send products
            return transactionCounter++;
        }

        private static double calcPrice(Product p, int amount)
        {
            return amount * (p.getPrice());
        }
    }
}
