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
            int fail = 0;
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
                //check if the item in the stock and the policies consistency

                Dictionary<Product, int> itemsPerStore = currShoppingCart.getProducts();
                //make <product,amount,price>list
                foreach (KeyValuePair<Product, int> item in itemsPerStore)
                {
                    Product currProduct = item.Key;
                    int currAmount = item.Value;

                    //buy from store
                    Store.callback currCallBack = currStore.buyProduct(currProduct, currAmount);
                    //store disconfirm the purchase
                    if (currCallBack == null)
                        break;
                    callbacks.Add(currCallBack);
                    //list of product to remove from basket
                    purchasedProducts.Add(new ProductAmountPrice(currProduct, currAmount, currProduct.price));
                }
                double totalCart = currStore.calcProductsPrice(purchasedProducts, user);

                //buy
                if (!PaymentStub.Pay(totalCart, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate))
                {
                    foreach (Store.callback call in callbacks)
                        call();
                    fail = -2;
                }
                else if (!SupplyStub.supply(sourceAddress, targetAddress))
                {
                    foreach (Store.callback call in callbacks)
                        call();
                    fail = -3;
                }

            }
            //parches 
            //send products

            if (fail < 0)

                return fail;
            foreach (ProductAmountPrice p in purchasedProducts)
            {
                Store store;
                if((store = WorkShop.getStore(p.product.storeId)) != null)
                    basket.setProductAmount(store, p.product, 0);
            }
            return ++transactionCounter;

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
