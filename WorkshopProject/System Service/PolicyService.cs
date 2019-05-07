using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.Policies;

namespace WorkshopProject.System_Service
{
    public class PolicyService
    {
        internal User user;

        public PolicyService(User user)
        {
            this.user = user;
        }

        private String storeValidation(int storeId)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                return JMessage.generateMessageFormatJason("Store does not exist");
            if (!store.isActive)
                return JMessage.notActiveStoreError();
            return null;
        }

        private String removePolicyfailes(int faultId)
        {
            switch (faultId)
            {
                case (-1):
                    return JMessage.generateMessageFormatJason("Store is not active");
                case (-2):
                    return JMessage.generateMessageFormatJason("user premission do not allow this action");
                case (-3):
                    return JMessage.generateMessageFormatJason("policies are empty");
            }
            return null;
        }

        //discount
        internal string addDiscountPolicy(int storeId,String policies)
        {
            String output = storeValidation(storeId);
            if (output == null)
            {
                Store store = WorkShop.getStore(storeId);
                IBooleanExpression dicountPolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policies);
                int policyId = store.AddDiscountPolicy(user, dicountPolicy);
                return JMessage.generateIDMessageFormatJason(policyId);
            }
            return output;

        }

        internal string removeDiscountPolicy(int storeId,int policyId)
        {
            String output = storeValidation(storeId);
            if (output == null)
            {
                Store store = WorkShop.getStore(storeId);
                int response = store.RemoveDiscountPolicy(user, policyId);
                if (response < 0)
                    return removePolicyfailes(response);
                return JMessage.generateIDMessageFormatJason(policyId);
            }
            return output;
        }

        //purchasing
        internal string addPurchasingPolicy(int storeId, String policies)
        {
            String output = storeValidation(storeId);
            if (output == null)
            {
                Store store = WorkShop.getStore(storeId);
                IBooleanExpression purchasingPolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policies);
                int policyId = store.AddPurchasPolicy(user, purchasingPolicy);
                return JMessage.generateIDMessageFormatJason(policyId);
            }
            return output;
        }

        internal string removePurchasingPolicy(int storeId, int policyId)
        {
            String output = storeValidation(storeId);
            if (output == null)
            {
                Store store = WorkShop.getStore(storeId);
                int response = store.RemovePurchasPolicy(user, policyId);
                if (response < 0)
                    return removePolicyfailes(response);
                return JMessage.generateIDMessageFormatJason(policyId);
            }
            return output;
        }

        //store
        internal string addStorePolicy(int storeId, String policies)
        {
            String output = storeValidation(storeId);
            if (output == null)
            {
                Store store = WorkShop.getStore(storeId);
                IBooleanExpression storePolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policies);
                int policyId = store.AddStorePolicy(user, storePolicy);
                return JMessage.generateIDMessageFormatJason(policyId);
            }
            return output;
        }

        internal string removeStorePolicy(int storeId, int policyId)
        {
            String output = storeValidation(storeId);
            if (output == null)
            {
                Store store = WorkShop.getStore(storeId);
                int response = store.RemoveStorePolicy(user, policyId);
                if (response < 0)
                    return removePolicyfailes(response);
                return JMessage.generateIDMessageFormatJason(policyId);
            }
            return output;
        }

    }
}
