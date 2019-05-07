using Newtonsoft.Json;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tansactions;
using Users;


namespace WorkshopProject.System_Service
{

    //Message Format: {message: String}
    //Search Format: {List<Product> products}

public static class TransactionService
    {

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
                    ret = true;
                else
                    ret = false;
            }
            else
                throw new Exception("Illegal Product id");
            return ret;
        }

        public static int BuyShoppingBasket(User user)
        {
            int transId = Transaction.purchase(user);
            return transId;

            //return JsonConvert.SerializeObject(new IdMessage(transId));
        }
        
        public static JsonShoppingCart GetShoppingCart(User user, int storeId)
        {
            //find the product store;
            Store store = WorkShop.getStore(storeId);
            if (store == null) { 
                throw new Exception("Illegal store id");
            }
            ShoppingCart shoppingCart;
            user.shoppingBasket.carts.TryGetValue(store,out shoppingCart);
            if(shoppingCart == null)
            {
                throw new Exception("Illegal store id for this user");
            }
            JsonShoppingCart jsc = new JsonShoppingCart(shoppingCart);
            return jsc;
        }

        public static JsonShoppingBasket GetShoppingBasket(User user)
        {
            JsonShoppingBasket jsb = new JsonShoppingBasket(user.shoppingBasket);
            return jsb;
        }

        public static bool SetProductAmountInBasket(User user, int storeId,int productId, int amount)
        {
            bool ret;
            ShoppingBasket userShoppingBasket = user.shoppingBasket;
            Store store = WorkShop.getStore(storeId);
            Product product;
            if (store != null && (product=store.findProduct(productId)) !=null)
            {
                bool sucss;
                //jonathan - this flow makes no sense
                //if (product.amount <= amount)
                    sucss = userShoppingBasket.setProductAmount(store, product, amount);
               // else
               //     sucss = false;
                if (sucss)
                    ret = true;
                else
                    ret = false;
            }
            else
                throw new Exception("Illegal Product id");
            return ret;
        }
    }
}
