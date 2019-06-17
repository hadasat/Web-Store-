using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.Policies;

namespace WorkshopProject.System_Service
{
    public static class PolicyService
    {
        private static Repo DB = new Repo();

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

        public static void Update(Store store)
        {
            if (useStub())
            {
                //do nothing
                return;
            }
           DB.Update(store);
        }

        private static bool useStub()
        {
            return DataAccessDriver.UseStub;
        }

        private static void updateStore(Store store)
        {
            if (useStub())
            {
                //do nothing
                return;
            }
            DB.Update<Store>(store);
        }

        //discount
        public static Policystatus addDiscountPolicy(User user, int storeId, Discount policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            Policystatus status = store.AddDiscountPolicy(user, policies);
            if (status == Policystatus.Success)
                updateStore(store);
            return status;

        }

        public static Policystatus removeDiscountPolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            Policystatus status = store.RemoveDiscountPolicy(user, policyId);
            if (status == Policystatus.Success)
                updateStore(store);
            return status;

        }

        //purchasing
        public static Policystatus addPurchasingPolicy(User user, int storeId, IBooleanExpression policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            Policystatus status = store.AddPurchasPolicy(user, policies);
            if (status == Policystatus.Success)
                updateStore(store);
            return status;
        }

        public static Policystatus removePurchasingPolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            Policystatus status = store.RemovePurchasPolicy(user, policyId);
            if (status == Policystatus.Success)
                updateStore(store);
            return status;

        }
        
        //store
        public static Policystatus addStorePolicy(User user, int storeId, IBooleanExpression policies)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
           Policystatus status = store.AddStorePolicy(user, policies);
            if (status == Policystatus.Success)
                updateStore(store);
            return status;
        }
        
        public static Policystatus removeStorePolicy(User user, int storeId, int policyId)
        {
            storeValidation(storeId);
            Store store = WorkShop.getStore(storeId);
            Policystatus status = store.RemoveStorePolicy(user, policyId);
            if (status == Policystatus.Success)
                updateStore(store);
            return status;

        }
        //System
        public static Policystatus addSystemPolicy(User user, IBooleanExpression policies)
        {
            return WorkShop.addSystemPolicy(user,policies);
        }

        public static Policystatus removeSystemPolicy(User user, int policyId)
        {
            return WorkShop.removeSystemPolicy(user,policyId);

        }

    }
}
