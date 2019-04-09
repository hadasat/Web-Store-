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
            bool fail = false;
            double totalPrice = 0;
            ShoppingBasket basket = user.shoppingBasket;
            //check the basket is empty
            if (basket.isEmpty())
                return -1;

            Dictionary<Store, ShoppingCart> carts = basket.carts;
            Dictionary<Product, Store> purchasedProducts = new Dictionary<Product, Store>();
            List<Store.callback> callbacks = new List<Store.callback>(); 
            //calc toal price
            foreach (KeyValuePair<Store, ShoppingCart> c in carts)
            {
                Store currStore = c.Key;
                ShoppingCart currShoppingCart = c.Value;

                //check if the item in the stock and add the price
                Dictionary<Product, int> itemsPerStore = currShoppingCart.getProducts();
                foreach (KeyValuePair<Product, int> item in itemsPerStore)
                {
                    Product currProduct = item.Key;
                    int currAmount = item.Value;
                    
                    //buy from store
                    Store.callback currCallBack = currStore.buyProduct(currProduct, currAmount);
                    callbacks.Add(currCallBack);
                    //store disconfirm the purchase
                    if (currCallBack == null)
                        return -1;

                    //list of product to remove from basket
                    purchasedProducts.Add(currProduct,currStore);
                    totalPrice += calcPrice(currProduct, currAmount);
                }
            }
            //parches 
            //send products

            if (fail)
            {
                foreach (Store.callback call in callbacks)
                    call();
                return -1;
            }
            //clean purches product from basket
            else
            {
                foreach (KeyValuePair<Product, Store> p in purchasedProducts)
                    basket.setProductAmount(p.Value, p.Key, 0);
                return ++transactionCounter;
            }
            
        }

        private static double calcPrice(Product p, int amount)
        {
            return amount * (p.getPrice());
        }
    }
}
