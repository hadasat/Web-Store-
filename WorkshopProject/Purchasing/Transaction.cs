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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkshopProject.DataAccessLayer;
using System.Data.Entity.Infrastructure;

namespace TansactionsNameSpace
{
    public enum status {Sucess ,empty , StokesShortage, Consistency ,Policies ,Payment ,Supply,ContactStoreForRefound} 

    public class Transaction
    {
        static int transactionCounter = 1;

        [Key]
        public int id { get; set; }
        [NotMapped]
        public User user { get; set; }
        [Include]
        public Member member { get { return (user is Member) ? (Member)user : null; } set => member = value; }
        [Include]
        public List<ShoppingCartDeal> sucess { get; set; }
        [Include]
        public List<ShoppingCartDeal> fail { get; set; }
        public double total { get; set; }
        [Include]
        public status transactionSatus { get; set; }


        public Transaction() { } //added for DB

        public Transaction(User user, int userCredit, int userCsv, string userExpiryDate, string targetAddress)
        {
            this.user = user;
            this.total = 0;
            sucess = new List<ShoppingCartDeal>();
            fail = new List<ShoppingCartDeal>();
            purchase(userCredit, userCsv, userExpiryDate, targetAddress);
            if (transactionSatus == status.empty || sucess.Count == 0)
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

            Dictionary<Store, ShoppingCart> carts = basket.getCarts();
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
                        totalCart += calcPrice(p.price, p.amount);
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

                if (!pay(totalCart, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate))
                {
                    returnProducts(callbacks);
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Payment);
                    fail.Add(failcartDeal);
                    continue;
                }
                else if (!SupplyStub.supply(sourceAddress, targetAddress))
                {
                    returnProducts(callbacks);
                    ShoppingCartDeal failcartDeal;
                    double refound = PaymentStub.Refund(totalCart, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate);
                    if(refound < 0)
                        failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.ContactStoreForRefound);
                    else
                        failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Supply);
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

        public double calcPrice(double product, int amount)
        {
            return product * amount;
        }

        private bool pay(double totalCart,int storeBankNum,int storeAccountNum,int userCredit,int userCsv,string userExpiryDate)
        {
            double sum = PaymentStub.Pay(totalCart, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate);
            return (sum > 0);

        }

        public static bool checkConsistency(User user, Store store, ShoppingCart cart)
        {
            List<Discount> discount = store.discountPolicy;
            List<IBooleanExpression> purchase = store.purchasePolicy;
            List<IBooleanExpression> storePolicy = store.storePolicy;
            return ConsistencyStub.checkConsistency(user, discount, purchase, storePolicy, cart);
        }

        

        public static bool updateUser(User user)
        {
            bool sucess = false,tryAgain = true;
            int maxTries = 10,counter =0;
            if(user is Member)
            {
                Member member = (Member)user;
                IDataAccess dal = DataAccessDriver.GetDataAccess();
                while(tryAgain && counter < maxTries)
                try
                {
                    dal.SaveEntity(member, member.id);
                        sucess = true;
                        tryAgain = false;
                    }
                catch (DbUpdateConcurrencyException concurrencyException)
                {
                        tryAgain = true;
                        counter++;
                }
                catch (Exception e)
                {
                        sucess = false;
                        tryAgain = false;
                    }

            }
            return sucess;
        }
    }

    public class ShoppingCartDeal
    {
        [Key]
        public int id { get; set; }
        [Include]
        public List<ProductAmountPrice> products { get; set; }
        public String storeName { get; set; }
        public int storId { get; set; }
        public double totalPrice { get; set; }
        [NotMapped]
        public status transStatus { get; set; }

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
