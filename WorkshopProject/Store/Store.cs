using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    class Store
    {

         string name;
         int rank;
         Boolean isActive;

        Dictionary<int, Product> products;

        public Store()
        {
            products = new Dictionary<int, Product>();

        }

        Store createNewStore(User StoreOwner)
        {
            return this;
        }

        Boolean addProduct(User user,Product product)
        {
            return true;
        }

        Boolean removeProduct(User user, Product product)
        {
            return true;
        }

        Boolean addDiscount(User user, DiscountPolicy discount)
        {
            return true;
        }

        Boolean removeDiscount(User user, DiscountPolicy discount)
        {
            return true;
        }

        Boolean addPurchasingPolicy(PurchasePolicy pPolicy)
        {
            return true;
        }

        Boolean removePurchasingPolicy(PurchasePolicy pPolicy)
        {
            return true;
        }

        Boolean addStoremanager(User user, User storeManager)
        {
            return true;
        }




    }
}
