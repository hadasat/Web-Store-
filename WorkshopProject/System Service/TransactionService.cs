using Newtonsoft.Json;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TansactionsNameSpace;
using Users;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.System_Service
{

    //Message Format: {message: String}
    //Search Format: {List<Product> products}

    public static class TransactionService
    {
        private static Repo DB = new Repo();

        public static bool AddProductToBasket(User user, int storeId, int productId, int amount)
        {
            bool ret;
            ShoppingBasket userShoppingBasket = user.shoppingBasket;
            Store store = WorkShop.getStore(storeId);
            Product product;
            if (store != null && (product = store.findProduct(productId)) != null)
            {
                bool sucss;
                sucss = userShoppingBasket.addProduct(store, product, amount);
                if (sucss)
                {
                    ret = true;
                    Update(user);
                }
                else
                    ret = false;
            }
            else
            {
                throw new Exception("Illegal Product id");
            }
            return ret;
        }

        public static async Task<Transaction> BuyShoppingBasket(User user, string cardNumber, int month, int year, string holder, int ccv, int id, string name, string address, string city, string country, string zip)
        {
            Transaction transaction = new Transaction();
            await transaction.doTransaction(user, cardNumber,month,year,holder,ccv,id,name,address,city,country,zip);
            return transaction;
        }
        
        public static ShoppingCart GetShoppingCart(User user, int storeId)
        {
            //find the product store;
            Store store = WorkShop.getStore(storeId);
            if (store == null) { 
                throw new Exception("Illegal store id");
            }
            ShoppingCart shoppingCart = user.shoppingBasket.getCart(store);
            if(shoppingCart == null)
            {
                throw new Exception("Illegal store id for this user");
            }
            return shoppingCart;
        }

        public static ShoppingBasket GetShoppingBasket(User user)
        {
            return user.shoppingBasket;
        }

        public static bool SetProductAmountInBasket(User user, int storeId,int productId, int amount)
        {
            bool ret;
            ShoppingBasket userShoppingBasket = user.shoppingBasket;
            Store store = WorkShop.getStore(storeId);
            Product product;
            if (store != null && (product = store.findProduct(productId)) != null)
            {
                ret = userShoppingBasket.setProductAmount(store, product, amount);
                Update(user);
            }
            else
                throw new Exception("Illegal Product id");
            return ret;
        }

        public static void Update(User user)
        {
            if (useStub())
            {
                //do nothing
                return;
            }
            if (user is Member)
                DB.Update(user);
        }

        private static bool useStub()
        {
            return DataAccessDriver.UseStub;
        }
    }
}
