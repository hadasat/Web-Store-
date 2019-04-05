using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.System_Service
{
    public class StoreService
    {
        internal User user;
       
        public StoreService(User user)
        {
            this.user = user;
        }

        internal string addDiscountPolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        internal string AddProductToStock(int storeId, int productId, int amount)
        {
            throw new NotImplementedException();
        }

        internal string AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            throw new NotImplementedException();
        }

        internal string AddStore(string storeName)
        {
            throw new NotImplementedException();
        }

        internal string ChangeProductInfo(int productId, string name, string desc, double price, string category, int amount)
        {
            throw new NotImplementedException();
        }

        internal string CloseStore(int storeID)
        {
            throw new NotImplementedException();
        }

        internal string GetProductInfo(int id)

        {
            throw new NotImplementedException();
        }

        internal string removeDiscountPolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        internal string RemoveProductFromStore(int storeId, int productId)
        {
            throw new NotImplementedException();
        }

        internal string removeProductFromStore(int storeId)
        {
            throw new NotImplementedException();
        }

        internal string SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int storeRank)
        {
            throw new NotImplementedException();
        }
    }
}
