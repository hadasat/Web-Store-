using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;


namespace WorkshopProject.System_Service
{
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
                return "good";
            }
            return "bad";
        }

        internal string BuyShoppingBasket()
        {
            int transId = Tansaction.purchase(user);

            return "good" + transId;
        }

        internal string GetShoppingCart(int storeId)
        {
            //return JsonConvert.SerializeObject(user.shoppingBasket);
            throw new NotImplementedException();
        }

        internal string SetProductAmountInCart(int productId, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
