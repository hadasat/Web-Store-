using External_Services;
using Shopping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.Policies;

namespace Tansactions
{
    public static class Transaction
    {
        static int transactionCounter = 1;

        public static int purchase(User user)
        {
            int output = 0;
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
            
                Dictionary<Product, int> itemsPerStore = currShoppingCart.getProducts();
                foreach (KeyValuePair<Product, int> item in itemsPerStore)
                {
                    Product currProduct = item.Key;
                    int currAmount = item.Value;
                    //check consistency
                    if (!checkConsistency(currShoppingCart, currStore, currProduct))
                    {
                        output = -2;
                        break;
                    }
                     
                    //buy from store
                    Store.callback currCallBack = currStore.buyProduct(currProduct, currAmount);
                    //store disconfirm the purchase
                    if (currCallBack == null)
                    {
                        output = -3;
                        break;
                    }
                    callbacks.Add(currCallBack);
                    //list of product to remove from basket
                    purchasedProducts.Add(currProduct,currStore);
                    totalPrice += calcPrice(currProduct, currAmount);
                }
                
            }
            //parches 
            //send products

            if (output < 0)
            {
                foreach (Store.callback call in callbacks)
                    call();
                output = - 4;
            }
            //clean purches product from basket
            else
            {
                foreach (KeyValuePair<Product, Store> p in purchasedProducts)
                    basket.setProductAmount(p.Value, p.Key, 0);
                output = ++transactionCounter;
            }
            return output;
            
        }

        private static double calcPrice(Product p, int amount)
        {
            return amount * (p.getPrice());
        }

        private static bool checkConsistency(ShoppingCart cart,Store store, Product product)
        {
            ConsistencyStub.setRet(true);
            IBooleanExpression discount = store.discountPolcies;
            List<PurchasePolicy> purchase = new List<PurchasePolicy>();
            return ConsistencyStub.checkConsistency(purchase, discount,cart);
        }
    }
}
