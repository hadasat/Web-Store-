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

        public TransactionService(User user)
        {
            this.user = user;
        }

        internal string AddProductToBasket(int productId, int amount)
        {
            ShoppingBasket userShoppingBasket = user.shoppingBasket;
            Dictionary<Store, Product> storeAndProuduct = WorkShop.findProduct(productId);
            if(storeAndProuduct != null)
            {
                Store store = storeAndProuduct.First().Key;
                Product product = storeAndProuduct.First().Value;
                userShoppingBasket.addProduct(store, product);
                return "{message: Success}";
            }
            return "{message: Illegal Product id}";
        }

        internal string BuyShoppingBasket()
        {
            int transId = Transaction.purchase(user);
            return "{message: Success, transactionId: " + transId + " }";
        }

        internal string GetShoppingCart(int storeId)
        {
            return JsonConvert.SerializeObject(user.shoppingBasket.Carts);
        }

        internal string SetProductAmountInCart(int productId, int amount)
        {
            return AddProductToBasket(productId, amount);
        }
    }
}
