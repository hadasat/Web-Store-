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
        public static Policystatus addDiscountPolicy(User user, int storeId, Discount policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            return store.AddDiscountPolicy(user, policies);

        }

        public static Policystatus removeDiscountPolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            return store.RemoveDiscountPolicy(user, policyId);

        }

        //purchasing
        public static Policystatus addPurchasingPolicy(User user, int storeId, IBooleanExpression policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            return store.AddPurchasPolicy(user, policies);
        }

        public static Policystatus removePurchasingPolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            return store.RemovePurchasPolicy(user, policyId);
          
        }
        
        //store
        public static Policystatus addStorePolicy(User user, int storeId, IBooleanExpression policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
           return store.AddStorePolicy(user, policies);
        }
        
        public static Policystatus removeStorePolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            return store.RemoveStorePolicy(user, policyId);

        }
        //System
        public static Policystatus addSystemPolicy(User user, int storeId, IBooleanExpression policies)
        {
            return WorkShop.addSystemPolicy(policies);
        }

        public static Policystatus removeSystemPolicy(User user, int storeId, int policyId)
        {
            return WorkShop.removeSystemPolicy(policyId);

        }

    }
}
