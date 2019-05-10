using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.Policies;

namespace WorkshopProject.System_Service
{
    public static class PolicyService
    {
        private static void storeValidation(int storeId)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                throw new Exception("Store does not exist");
            if (!store.isActive)
                throw new Exception("inactive store");
        }

        private static void removePolicyfailes(int faultId)
        {
            switch (faultId)
            {
                case (-1):
                    throw new Exception ("Store is not active");
                case (-2):
                    throw new Exception("user premission do not allow this action");
                case (-3):
                    throw new Exception("policies are empty");
            }
        }

        //discount
        public static int addDiscountPolicy(User user, int storeId, String policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            IBooleanExpression dicountPolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policies);
            int policyId = store.AddDiscountPolicy(user, dicountPolicy);
            return policyId;

        }

        public static bool removeDiscountPolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            int response = store.RemoveDiscountPolicy(user, policyId);
            if (response < 0)
                removePolicyfailes(response);
            return true;

        }

        //purchasing
        public static int addPurchasingPolicy(User user, int storeId, String policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            IBooleanExpression purchasingPolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policies);
            int policyId = store.AddPurchasPolicy(user, purchasingPolicy);
            return policyId;
        }

        public static bool removePurchasingPolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            int response = store.RemovePurchasPolicy(user, policyId);
            if (response < 0)
                removePolicyfailes(response);
            return true;
        }

        //store
        public static int addStorePolicy(User user, int storeId, String policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            IBooleanExpression purchasingPolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policies);
            int policyId = store.AddStorePolicy(user, purchasingPolicy);
            return policyId;
        }

        public static bool removeStorePolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            int response = store.RemoveStorePolicy(user, policyId);
            if (response < 0)
                removePolicyfailes(response);
            return true;
        }

    }
}
