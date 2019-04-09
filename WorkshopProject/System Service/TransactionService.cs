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

public class TransactionService
    {
        internal User user;
        public string successMsg = "success";

        public TransactionService(User user)
        {
            this.user = user;
        }

        internal string AddProductToBasket(int productId, int amount)
        {
            Message msg;
            ShoppingBasket userShoppingBasket = user.shoppingBasket;
            Dictionary<Store, Product> storeAndProuduct = WorkShop.findProduct(productId);
            if(storeAndProuduct != null)
            {
                Store store = storeAndProuduct.First().Key;
                Product product = storeAndProuduct.First().Value;
                bool sucss;
                if (product.amount <= amount)
                    sucss = userShoppingBasket.addProduct(store, product, amount);
                else
                    sucss = false;
                if (sucss)
                    msg = new Message(successMsg);
                else
                    msg = new Message("request Fail");
            }
            else
                msg = new Message("Illegal Product id");
            return JsonConvert.SerializeObject(msg);
        }

        internal string BuyShoppingBasket()
        {
            Message msg;
            int transId = Transaction.purchase(user);
            if(transId > 0)
                return "{\"message\": \"Success\", \"transactionId\": " + transId + " }";
            else
                msg = new Message("purchase failed");
            return JsonConvert.SerializeObject(msg);
        }

        internal string GetShoppingCart(int storeId)
        {
            Message msg;
            //find the product store;
            Store store = WorkShop.getStore(storeId);
            if (store == null) { 
                msg = new Message("Illegal store id");
                return JsonConvert.SerializeObject(msg);
            }
            ShoppingCart shoppingCart;
            user.shoppingBasket.carts.TryGetValue(store,out shoppingCart);
            if(shoppingCart == null)
            {
                msg = new Message("Illegal store id for this user");
                return JsonConvert.SerializeObject(msg);
            }
            JsonShoppingCart jsc = new JsonShoppingCart(shoppingCart);
            return JsonConvert.SerializeObject(jsc);
        }

        internal string GetShoppingBasket()
        {
            JsonShoppingBasket jsb = new JsonShoppingBasket(user.shoppingBasket);
            return JsonConvert.SerializeObject(jsb);
        }

        internal string SetProductAmountInCart(int productId, int amount)
        {
            Message msg;
            ShoppingBasket userShoppingBasket = user.shoppingBasket;
            Dictionary<Store, Product> storeAndProuduct = WorkShop.findProduct(productId);
            if (storeAndProuduct != null)
            {
                Store store = storeAndProuduct.First().Key;
                Product product = storeAndProuduct.First().Value;
                bool sucss;
                //jonathan - this flow makes no sense
                //if (product.amount <= amount)
                    sucss = userShoppingBasket.setProductAmount(store, product, amount);
               // else
               //     sucss = false;
                if (sucss)
                    msg = new Message(successMsg);
                else
                    msg = new Message("request Fail");
            }
            else
                msg = new Message("Illegal Product id");
            return JsonConvert.SerializeObject(msg);
        }
    }
}
