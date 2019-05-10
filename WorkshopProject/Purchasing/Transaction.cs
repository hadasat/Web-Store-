using WorkshopProject.External_Services;
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
        static int transactionCounter = 0;

        static int storeBankNum = 12;
        static int storeAccountNum = 12;
        static int userCredit = 27;
        static int userCsv = 388;
        static string userExpiryDate = "never";

        static string sourceAddress = "beer sheva";
        static string targetAddress = "tel aviv";

        public static int purchase(User user)
        {
            int transactionFail = 0;
            int numSucessTransaction = 0;
            double totalPrice = 0;

            ShoppingBasket basket = user.shoppingBasket;
            //check the basket is empty
            if (basket.isEmpty())
                return -1;

            Dictionary<Store, ShoppingCart> carts = basket.carts;
            List<ProductAmountPrice> purchasedProducts = new List<ProductAmountPrice>();
            List<Store.callback> callbacks = new List<Store.callback>();
            //calc toal price
            foreach (KeyValuePair<Store, ShoppingCart> c in carts)
            {
                Store currStore = c.Key;
                ShoppingCart currShoppingCart = c.Value;
                //check consistency
                if (!checkConsistency(user, currStore, currShoppingCart))
                    break;
                List<ProductAmountPrice> currStoreProducts = ProductAmountPrice.translateCart(currShoppingCart);
                //check store policies  
                if (!currStore.checkPolicies(currStoreProducts, user))
                    break;

                double totalCart = 0;
                currStoreProducts = currStore.afterDiscount(currStoreProducts, user);
                foreach (ProductAmountPrice p in currStoreProducts)
                {
                    Store.callback currCallBack = currStore.buyProduct(p.product, p.amount);
                    if (currCallBack != null)
                    {
                        callbacks.Add(currCallBack);
                        purchasedProducts.Add(p);
                        totalCart += calcPrice(p.product, p.amount);
                    }
                }

                //return products to store if transaction fails
                if (!PaymentStub.Pay(totalCart, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate))
                {
                    foreach (Store.callback call in callbacks)
                        call();
                    transactionFail = -2;
                }
                else if (!SupplyStub.supply(sourceAddress, targetAddress))
                {
                    foreach (Store.callback call in callbacks)
                        call();
                    transactionFail = -3;
                }
                else
                {
                    purchasedProducts.Concat(currStoreProducts);
                    numSucessTransaction++;
                    totalPrice += totalCart;
                }
            }
            //parches 
            //send products

            if (transactionFail < 0)
                return transactionFail;
            foreach (ProductAmountPrice p in purchasedProducts)
            {
                Store store;
                if((store = WorkShop.getStore(p.product.storeId)) != null)
                    basket.setProductAmount(store, p.product, 0);
            }
            if(numSucessTransaction >0)
                return ++transactionCounter;
            return -5;

        }

        private static double calcPrice(Product p, int amount)
        {
            return amount * (p.getPrice());
        }

        public static bool checkConsistency(User user,Store store,ShoppingCart cart)
        {
            List<Discount>discount = store.discountPolicy;
            List<IBooleanExpression> purchase = store.purchasePolicy;
            List<IBooleanExpression> storePolicy = store.storePolicy;
            return ConsistencyStub.checkConsistency(user,discount, purchase, storePolicy, cart);
        }
        

    }
}
