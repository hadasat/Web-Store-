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
            throw new NotImplementedException();
        }

        internal string BuyShoppingBasket(int id)
        {
            throw new NotImplementedException();
        }

        internal string GetShoppingCart(int storeId)
        {
            throw new NotImplementedException();
        }

        internal string SetProductAmountInCart(int productId, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
