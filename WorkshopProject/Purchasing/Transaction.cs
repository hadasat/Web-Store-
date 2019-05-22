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

namespace TansactionsNameSpace
{
    public enum status {Sucess ,empty , StokesShortage, Consistency ,Policies ,Payment ,Supply} 

    public class Transaction
    {
        static int transactionCounter = 0;

        public User user;
        public List<ShoppingCartDeal> sucess;
        public List<ShoppingCartDeal> fail;
        public double total;
        public status transactionSatus;
        public int id;

        public Transaction(User user, int userCredit, int userCsv, string userExpiryDate, string targetAddress)
        {
            this.user = user;
            this.total = 0;
            List<ShoppingCartDeal> sucess = new List<ShoppingCartDeal>();
            List<ShoppingCartDeal> fail = new List<ShoppingCartDeal>();
            purchase(userCredit, userCsv, userExpiryDate, targetAddress);
            if (transactionSatus == status.empty)
                id = -1;
            else
                id = transactionCounter++;
        }

        public void returnProducts(List<Store.callback> callbacks)
        {
            foreach (Store.callback call in callbacks)
                call();
        }

        public void purchase(int userCredit, int userCsv, string userExpiryDate, string targetAddress)
        {
            ShoppingBasket basket = user.shoppingBasket;
            //check the basket is empty
            if (basket.isEmpty())
            {
                transactionSatus = status.empty;
                return;
            }

            Dictionary<Store, ShoppingCart> carts = basket.carts;
            List<ProductAmountPrice> purchasedProducts = new List<ProductAmountPrice>();
            //calc toal price
            foreach (KeyValuePair<Store, ShoppingCart> c in carts)
            {
                List<Store.callback> callbacks = new List<Store.callback>();
                Store currStore = c.Key;
                ShoppingCart currShoppingCart = c.Value;
                List<ProductAmountPrice> currStoreProducts = ProductAmountPrice.translateCart(currShoppingCart);
                //check consistency
                if (!checkConsistency(user, currStore, currShoppingCart))
                {
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Consistency);
                    fail.Add(failcartDeal);
                    break;
                }
                //check store policies  
                if (!currStore.checkPolicies(currStoreProducts, user))
                {
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Policies);
                    fail.Add(failcartDeal);
                    break;
                }

                double totalCart = 0;
                //applay discount
                currStoreProducts = currStore.afterDiscount(currStoreProducts, user);
                bool allProductsAreAvailable = true;
                foreach (ProductAmountPrice p in currStoreProducts)
                {
                    Store.callback currCallBack = currStore.buyProduct(p.product, p.amount);
                    if (currCallBack != null)
                    {
                        callbacks.Add(currCallBack);
                        purchasedProducts.Add(p);
                        totalCart += calcPrice(p.product, p.amount);
                    }
                    else
                    {
                        allProductsAreAvailable = false;
                        break;
                    }
                }
                //cancel this store transaction
                if (!allProductsAreAvailable)
                {
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, totalCart, currStore.id, status.StokesShortage);
                    fail.Add(failcartDeal);
                    returnProducts(callbacks);
                    break;
                }

                //return products to store if transaction fails
                int storeBankNum = currStore.storeBankNum;
                int storeAccountNum = currStore.storeAccountNum;
                string sourceAddress = currStore.storeAddress;

                if (!PaymentStub.Pay(totalCart, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate))
                {
                    returnProducts(callbacks);
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Payment);
                    fail.Add(failcartDeal);
                    continue;
                }
                else if (!SupplyStub.supply(sourceAddress, targetAddress))
                {
                    returnProducts(callbacks);
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Supply);
                    fail.Add(failcartDeal);
                    continue;
                }
                else // purches all cart products
                {
                    purchasedProducts.Concat(currStoreProducts);
                    this.total += totalCart;
                    ShoppingCartDeal sucessCartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, totalCart, currStore.id, status.Sucess);
                    sucess.Add(sucessCartDeal);
                }
            }
            //empty all bought products
            foreach (ProductAmountPrice p in purchasedProducts)
            {
                Store store;
                if ((store = WorkShop.getStore(p.product.storeId)) != null)
                    basket.setProductAmount(store, p.product, 0);
            }
        }

        private static double calcPrice(Product p, int amount)
        {
            return amount * (p.getPrice());
        }

        public static bool checkConsistency(User user, Store store, ShoppingCart cart)
        {
            List<Discount> discount = store.discountPolicy;
            List<IBooleanExpression> purchase = store.purchasePolicy;
            List<IBooleanExpression> storePolicy = store.storePolicy;
            return ConsistencyStub.checkConsistency(user, discount, purchase, storePolicy, cart);
        }

    }

    public class ShoppingCartDeal
    {
        public List<ProductAmountPrice> products;
        public String storeName;
        public int storId;
        public double totalPrice;
        public status transStatus;

        public ShoppingCartDeal(List<ProductAmountPrice> products, String storeName, double totalPrice,int storId, status transStatus)
        {
            this.products = products;
            this.storeName = storeName;
            this.totalPrice = totalPrice;
            this.storId = storId;
            this.transStatus = transStatus;
        }

    }

}
