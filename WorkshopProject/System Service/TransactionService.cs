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

        internal string AddProductToBasket(int storeId, int productId, int amount)
        {
            Message msg;
            ShoppingBasket userShoppingBasket = user.shoppingBasket;
            Store store = WorkShop.getStore(storeId);
            Product product;
            if (store != null && (product = store.findProduct(productId)) != null)
            {
                    bool sucss;
                sucss = userShoppingBasket.addProduct(store, product, amount);
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
            int transId = Transaction.purchase(user);
            return JsonConvert.SerializeObject(new IdMessage(transId));
            
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

        internal string SetProductAmountInBasket(int storeId,int productId, int amount)
        {
            Message msg;
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
