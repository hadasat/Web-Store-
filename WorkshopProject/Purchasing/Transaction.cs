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
using WorkshopProject.Log;
using WorkshopProject.DataAccessLayer;
using System.Data.Entity.Infrastructure;

namespace TansactionsNameSpace
{
    public enum status {Sucess ,empty , StokesShortage, Consistency ,Policies ,Payment ,Supply,ContactStoreForRefound} 

    public class Transaction : IEntity
    {
        static int transactionCounter = 1;

        [Key]
        public int id { get; set; }
        [NotMapped]
        public User user { get; set; }
        [Include]
        public virtual Member member { get { return (user is Member) ? (Member)user : null; } set => member = value; }
        [Include]
        public virtual List<ShoppingCartDeal> sucess { get; set; }
        [Include]
        public virtual List<ShoppingCartDeal> fail { get; set; }
        public double total { get; set; }
        [Include]
        public virtual status transactionSatus { get; set; }
        [NotMapped]
        private IPayment paymentSystem;
        [NotMapped]
        private ISupply supplySystem;

        public Transaction() { } //added for DB

        public Transaction(User user, string cardNumber, int month, int year, string holder, int ccv, int id, string name, string address, string city, string country, string zip, IPayment payService,ISupply supplyService)
        {
            constructorCommon(user, cardNumber, month, year, holder, ccv, id, name, address, city, country, zip, payService, supplyService);
        }

        public Transaction(User user, string cardNumber, int month, int year, string holder, int ccv, int id,string name, string address, string city, string country, string zip)
        {
            ExternalSystemConnection extSytem = new ExternalSystemConnection();
            constructorCommon(user, cardNumber, month, year, holder, ccv, id, name, address, city, country, zip, (IPayment)extSytem, (ISupply)extSytem);
        }

        public override int GetKey()
        {
            return id;
        }

        private async void constructorCommon(User user, string cardNumber, int month, int year, string holder, int ccv, int userId, string name, string address, string city, string country, string zip, IPayment payService, ISupply supplyService)
        {
            try
            {
                this.user = user;
                this.total = 0;
                sucess = new List<ShoppingCartDeal>();
                fail = new List<ShoppingCartDeal>();
                paymentSystem = payService;
                supplySystem = supplyService;

                int transactionId = await purchase(cardNumber, month, year, holder, ccv, userId, name, address, city, country, zip);
                if (transactionSatus == status.empty || sucess.Count == 0)
                {
                    id = -1;
                    Logger.Log("error", logLevel.ERROR, "finished creating new transaction but the id is -1");
                    throw new Exception("error completing transaction");
                }
                else
                    id = transactionId;
            }
            finally
            {
                paymentSystem.Dispose();
            }
        }

        public void returnProducts(List<Store.callback> callbacks)
        {
            foreach (Store.callback call in callbacks)
                call();
        }

        public async Task<int> purchase(string cardNumber, int month, int year, string holder, int ccv, int id, string name, string address, string city, string country, string zip)
        {
            int transactionId = -1;
            ShoppingBasket basket = user.shoppingBasket;
            //check the basket is empty
            if (basket.isEmpty())
            {
                transactionSatus = status.empty;
                throw new Exception("can't buy an empty shopping basket");
            }

            List<ShoppingCartAndStore> carts = basket.getCarts();
            List<ProductAmountPrice> purchasedProducts = new List<ProductAmountPrice>();
            //calc toal price
            foreach (ShoppingCartAndStore c in carts)
            {
                List<Store.callback> callbacks = new List<Store.callback>();
                Store currStore = c.store;
                ShoppingCart currShoppingCart = c.cart;
                List<ProductAmountPrice> currStoreProducts = ProductAmountPrice.translateCart(currShoppingCart);
                //check consistency
                if (!checkConsistency(user, currStore, currShoppingCart))
                {
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Consistency);
                    fail.Add(failcartDeal);
                    throw new Exception("The shopping basket has consistency error");
                }
                //check store policies  
                if (!currStore.checkPolicies(currStoreProducts, user))
                {
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Policies);
                    fail.Add(failcartDeal);
                    throw new Exception("You don't pass all purchsing polices, please remove any problematic items");
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
                    throw new Exception("not all itmes are availabe");
                }

                //return products to store if transaction fails
                int storeBankNum = currStore.storeBankNum;
                int storeAccountNum = currStore.storeAccountNum;
                string sourceAddress = currStore.storeAddress;

                transactionId = await pay(cardNumber, month, year, holder, ccv, id);

                if (transactionId == -1)
                {
                    returnProducts(callbacks);
                    ShoppingCartDeal failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Payment);
                    fail.Add(failcartDeal);
                    throw new Exception("Payment was rejected by the payment service");
                }
                else if ((await supplySystem.supply(name,address,city,country,zip)) == -1) // supply system fails
                {
                    returnProducts(callbacks);
                    ShoppingCartDeal failcartDeal;
                    bool refudnAns = await paymentSystem.cancelPayment(transactionId);
                    //double refound = PaymentStub.Refund(totalCart, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate);
                    if(!refudnAns)
                        failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.ContactStoreForRefound);
                    else
                        failcartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, 0, currStore.id, status.Supply);
                    fail.Add(failcartDeal);
                    throw new Exception("Supply adress was rejected");
                }
                else // purches all cart products
                {
                    purchasedProducts.Concat(currStoreProducts);
                    this.total += totalCart;
                    ShoppingCartDeal sucessCartDeal = new ShoppingCartDeal(currStoreProducts, currStore.name, totalCart, currStore.id, status.Sucess);
                    WorkshopProject.Log.Logger.Log("event", logLevel.INFO, $"user {user.id} buy cart {currShoppingCart.id} sucessfully");
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
            return transactionId;
        }

        public double calcPrice(double product, int amount)
        {
            return product * amount;
        }

        private async Task<int> pay(string cardNumber, int month, int year, string holder, int ccv, int id)
        {
            return await paymentSystem.payment(cardNumber, month, year, holder, ccv, id);

        }

        public static bool checkConsistency(User user, Store store, ShoppingCart cart)
        {
            List<Discount> discount = store.discountPolicies;
            List<IBooleanExpression> purchase = store.purchasePolicies;
            List<IBooleanExpression> storePolicy = store.storePolicies;
            return ConsistencyStub.checkConsistency(user, discount, purchase, storePolicy, cart);
        }

        

        public static bool updateUser(User user)
        {
            bool sucess = false,tryAgain = true;
            int maxTries = 10,counter =0;
            if(user is Member)
            {
                Member member = (Member)user;
                //IDataAccess dal = DataAccessDriver.GetDataAccess(); //TODO: old dal
                while(tryAgain && counter < maxTries)
                try
                {
                        //dal.SaveEntity(member, member.id); //TODO: old dal
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



        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is Transaction)
            {
                Transaction _other = ((Transaction)other);
                member = _other.member;
                sucess = _other.sucess;
                fail = _other.fail;
            }
        }

        public override void LoadMe()
        {
            foreach (IEntity obj in sucess)
            {
                obj.LoadMe();
            }
            foreach (IEntity obj in fail)
            {
                obj.LoadMe();
            }
            member.LoadMe();
        }



    }

    public class ShoppingCartDeal : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public virtual List<ProductAmountPrice> products { get; set; }
        public String storeName { get; set; }
        public int storId { get; set; }
        public double totalPrice { get; set; }
        [Include]
        public virtual status transStatus { get; set; }

        public ShoppingCartDeal(List<ProductAmountPrice> products, String storeName, double totalPrice,int storId, status transStatus)
        {
            this.products = products;
            this.storeName = storeName;
            this.totalPrice = totalPrice;
            this.storId = storId;
            this.transStatus = transStatus;
        }

        public override int GetKey()
        {
            return id;
        }
        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is ShoppingCartDeal)
            {
                ShoppingCartDeal _other = ((ShoppingCartDeal)other);
                products = _other.products;
            }
        }

        public override void LoadMe()
        {
            foreach (IEntity obj in products)
            {
                obj.LoadMe();
            }
        }
    }
}
