using Managment;
using Newtonsoft.Json;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.Policies;

namespace WorkshopProject.System_Service
{
    public class SystemServiceImpl : UserInterface
    {
        //Those fields are temporary
        bool loggedIn;
        public User user { get; set; }

        private static string successMsg = "Success";
        private static string failMsg = "Fail";

        //private string adminUsername = "Admin";
        //private string adminPassword = "Admin";
        //StoreService storeS;
        //TransactionService transactionS;
        //UserService userS;

        public SystemServiceImpl()
        {
            user = new User();
            loggedIn = false;


            //storeS = new StoreService(user);
            //transactionS = new TransactionService(user);
            //userS = new UserService(user);
        }

        public void updateMember(User member)
        {
            //storeS.user = member;
            //transactionS.user = member;
            //user = member;
            //userS.user = member;
            this.user = member;
        }

        public string AddProductToBasket(int storeId,int productId, int amount)
        {
            bool ret;
            try
            {
                ret = TransactionService.AddProductToBasket(user, storeId, productId, amount);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string AddProductToStock(int storeId, int productId, int amount)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = StoreService.AddProductToStock(user, storeId, productId, amount);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            if (!loggedIn)
                return notLoggedInError();
            int ret;
            try
            {
                ret = StoreService.AddProductToStore(user, storeId, name, desc, price, category);
                return intJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string AddStore(string storeName)
        {
            if (!loggedIn)
                return notLoggedInError();
            int ret;
            try
            {
                ret = StoreService.AddStore(user, storeName);
                return intJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string AddStoreManager(int storeId, string userToAdd, string roles)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = UserService.AddStoreManager(user, storeId, userToAdd, roles);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string AddStoreOwner(int storeId, string userToAdd)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = UserService.AddStoreOwner(user, storeId, userToAdd);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string BuyShoppingBasket()
        {
            //if (!loggedIn)
            //    throw new Exception("Not logged in");
            int ret;
            try
            {
                ret = TransactionService.BuyShoppingBasket(user);
                return intJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = StoreService.ChangeProductInfo(user, storeId, productId, name, desc, price, category, amount);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string closeStore(int storeID)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = StoreService.CloseStore(user, storeID);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetProductInfo(int productId)
        {
            Product ret;
            try
            {
                ret = StoreService.GetProductInfo(productId);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetShoppingCart(int storeId)
        {
            JsonShoppingCart ret;
            try
            {
                ret = TransactionService.GetShoppingCart(user, storeId);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetShoppingBasket()
        {
            JsonShoppingBasket ret;
            try
            {
                ret = TransactionService.GetShoppingBasket(user);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string login(string username, string password)
        {
            Member ret;
            try
            {
                ret = UserService.login(username, password,user);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }

            if (ret != null)
            {
                updateMember(ret);
                if (user is Member)
                {
                    loggedIn = true;
                }
                else
                    loggedIn = false;
            }
            return resultJson(ret != null);
        }

        public string logout()
        {
            if (!loggedIn)
                return notLoggedInError();
            loggedIn = false;
            bool ret;
            try
            {
                ret = UserService.logout(user);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
            updateMember(new User());
            return resultJson(ret);
        }

        public string Register(string username, string password)
        {
            bool ret;
            try
            {
                ret = UserService.Register(username, password);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }
        public string Register(string username, string password, DateTime birthdate,string country)
        {
            bool ret;
            try
            {
                ret = UserService.Register(username, password,birthdate ,country);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

     
        public string RemoveProductFromStore(int storeId, int productId)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = StoreService.RemoveProductFromStore(user, storeId, productId);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string RemoveStoreManager(int storeId, string managerName)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = UserService.RemoveStoreManager(user, storeId, managerName);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string RemoveUser(string usernameToRemove)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = UserService.RemoveUser(user, usernameToRemove);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank)
        {
            List<Product> ret;
            try
            {
                ret = StoreService.SearchProducts(name, category, keyword, startPrice, endPrice, productRank, storeRank);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string SetProductAmountInBasket(int storeId,int productId, int amount)
        {
            bool ret;
            try
            {
                ret = TransactionService.SetProductAmountInBasket(user, storeId, productId, amount);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetStore(int storeId)
        {
            Store ret;
            try
            {
                ret = StoreService.GetStore(storeId);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetAllStores()
        {
            List<Store> ret;
            try
            {
                ret = StoreService.GetAllStores();
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetAllManagers(int storeId)
        {
            List<Member> ret;
            try
            {
                ret = StoreService.getAllManagers(storeId);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetAllOwners(int storeId)
        {
            List<Member> ret;
            try
            {
                ret = StoreService.getAllManagers(storeId);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetRoles()
        {
            List<StoreManager> ret;
            try
            {
                ret = UserService.GetRoles(user);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetAllMembers()
        {
            List<Member> ret;
            try
            {
                ret = UserService.GetAllMembers();
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string SendMessage(int memberId, string message)
        {
            try
            {
                UserService.SendMessage(memberId, message);
                return resultJson(true);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        //public string GetMessages(int memberId)
        //{
        //    List<string> ret;
        //    try
        //    {
        //        ret = UserService.GetMessages(memberId);
        //        return objDynamicJson(ret);
        //    }
        //    catch (Exception e)
        //    {
        //        return generateMessageFormatJason(e.Message);
        //    }
        //}
        //**********POLICIES*********************

        //policies
        public string addDiscountPolicy(int storeId, string policy)
        {
            if (!loggedIn)
                return notLoggedInError();
            try
            {
                Discount dicountPolicy = JsonHandler.DeserializeObject<Discount>(policy);
                int policyId = PolicyService.addDiscountPolicy(user, storeId, dicountPolicy);
                return intJson(policyId);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }

        }

        public string removeDiscountPolicy(int storeId, int policyId)
        {
            if (!loggedIn)
                return notLoggedInError();
            try
            {
                bool res = PolicyService.removeDiscountPolicy(user, storeId, policyId);
                return resultJson(res);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string addPurchasingPolicy(int storeId, string policy)
        {
            if (!loggedIn)
                return notLoggedInError();

            try
            {
                IBooleanExpression purchasingPolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policy);
                int policyId = PolicyService.addPurchasingPolicy(user, storeId, purchasingPolicy);
                return intJson(policyId);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string removePurchasingPolicy(int storeId, int policyId)
        {
            if (!loggedIn)
                return notLoggedInError();
            try
            {
                bool res = PolicyService.removePurchasingPolicy(user, storeId, policyId);
                return resultJson(res);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string addStorePolicy(int storeId, string policy)
        {
            if (!loggedIn)
                return notLoggedInError();
            try
            {
                IBooleanExpression storePolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policy);
                int policyId = PolicyService.addStorePolicy(user, storeId, storePolicy);
                return intJson(policyId);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string removeStorePolicy(int storeId, int policyId)
        {
            if (!loggedIn)
                return notLoggedInError();
            try
            {
                bool res = PolicyService.removeStorePolicy(user, storeId, policyId);
                return resultJson(res);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }


        //***********HELPER METHODS**************

        private string notLoggedInError()
        {
            Message msg = new Message("User not logged in");
            return JsonHandler.SerializeObjectDynamic(msg);
        }

        private string notActiveStoreError()
        {
            Message msg = new Message("Store not Active");
            return JsonHandler.SerializeObjectDynamic(msg);
        }

        private string resultJson(bool ret)
        {
            string msg = ret ? successMsg : failMsg;
            return generateMessageFormatJason(msg);
        }

        private string intJson(int ret)
        {
            IdMessage idMsg = new IdMessage(ret);
            return JsonHandler.SerializeObjectDynamic(idMsg);
        }

        private string successJson()
        {
            return JsonHandler.SerializeObjectDynamic(new Message("Success"));
        }

        private string failJson()
        {
            return JsonHandler.SerializeObjectDynamic(new Message("Fail"));
        }

        private string objDynamicJson(Object obj)
        {
            return JsonHandler.SerializeObjectDynamic(obj);
        }

        private string generateMessageFormatJason(string message)
        {
            return JsonHandler.SerializeObjectDynamic(new Message(message));
        }


        //jonathan - no idea how SystemAdmin object can be added
        //private string addAdmin()
        //{
        //    return Register(adminUsername, adminPassword);
        //}
    }

    public class Message
    {
        public string message;
        public Message(string message)
        {
            this.message = message;
        }
    }
}