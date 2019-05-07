using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.System_Service
{
    public class SystemServiceImpl : UserInterface
    {
        StoreService storeS;
        TransactionService transactionS;
        UserService userS;

        //Those fileds are temporary
        Boolean loggedIn;
        public User user { get; set; }

        private static string successMsg = "Success";
        private static string failMsg = "Fail";

        //private string adminUsername = "Admin";
        //private string adminPassword = "Admin";

        public SystemServiceImpl()
        {
            user = new User();
            storeS = new StoreService(user);
            transactionS = new TransactionService(user);
            userS = new UserService(user);
            loggedIn = false;
        }

        public void updateMember(User member)
        {
            storeS.user = member;
            transactionS.user = member;
            user = member;
            //userS.user = member;
            this.user = member;
        }

        public string addDiscountPolicy(int storeId)
        {
            if (!loggedIn)
                return notLoggedInError();

            bool ret;
            try
            {
                ret = storeS.addDiscountPolicy(storeId);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string AddProductToBasket(int storeId,int productId, int amount)
        {
            return transactionS.AddProductToBasket(storeId,productId, amount);
        }

        public string AddProductToStock(int storeId, int productId, int amount)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = storeS.AddProductToStock(storeId, productId, amount);
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
                ret = storeS.AddProductToStore(storeId, name, desc, price, category);
                return intJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string addPurchasingPolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        public string AddStore(string storeName)
        {
            if (!loggedIn)
                return notLoggedInError();
            int ret;
            try
            {
                ret = storeS.AddStore(storeName);
                return intJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string AddStoreManager(int storeId, string user, string roles)
        {
            if (!loggedIn)
                return notLoggedInError();
            return userS.AddStoreManager(storeId, user, roles);    ///TODO:::::::::::::::::;///////////// :))))))
        }

        public string AddStoreOwner(int storeId, string user)
        {
            if (!loggedIn)
                return notLoggedInError();
            return userS.AddStoreOwner(storeId, user);
        }

        public string BuyShoppingBasket()
        {
            //if (!loggedIn)
            //    throw new Exception("Not logged in");
            return transactionS.BuyShoppingBasket();
        }

        public string ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = storeS.ChangeProductInfo(storeId, productId, name, desc, price, category, amount);
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
                ret = storeS.CloseStore(storeID);
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
                ret = storeS.GetProductInfo(productId);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string GetShoppingCart(int storeId)
        {
            return transactionS.GetShoppingCart(storeId);
        }

        public string GetShoppingBasket()
        {
            return transactionS.GetShoppingBasket();
        }

        public string login(string username, string password)
        {
            //loggedIn = true; //TODO: why does the field turn to TRUE even if we fail ?!?!
            String toReturn =  userS.login(username, password);
            updateMember(userS.user);
            if (user is Member)
            {
                loggedIn = true;
            }
            else
                loggedIn = false;
            return toReturn;
        }

        public string logout()
        {
            if (!loggedIn)
                return notLoggedInError();
            loggedIn = false;
            String toReturn =  userS.logout();
            updateMember(new User());
            return toReturn;
        }

        public string Register(string user, string password)
        {
            return userS.Register(user, password);
        }
        public string Register(string user, string password, string country, int age)
        {
            return userS.Register(user, password, country, age);
        }

        public string removeDiscountPolicy(int storeId)
        {
            if (!loggedIn)
                return notLoggedInError();
            return storeS.removeDiscountPolicy(storeId);
        }

        public string RemoveProductFromStore(int storeId, int productId)
        {
            if (!loggedIn)
                return notLoggedInError();
            bool ret;
            try
            {
                ret = storeS.RemoveProductFromStore(storeId, productId);
                return resultJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string removePurchasingPolicy(int storeId)
        {
            if (!loggedIn)
                return notLoggedInError();
            return storeS.removePurchasingPolicy(storeId);
        }

        public string RemoveStoreManager(int storeId, string user)
        {
            if (!loggedIn)
                return notLoggedInError();
            return userS.RemoveStoreManager(storeId, user);
        }

        public string RemoveUser(string user)
        {
            if (!loggedIn)
                return notLoggedInError();
            return userS.RemoveUser(user);
        }

        public string SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank)
        {
            List<Product> ret;
            try
            {
                ret = storeS.SearchProducts(name, category, keyword, startPrice, endPrice, productRank, storeRank);
                return objDynamicJson(ret);
            }
            catch (Exception e)
            {
                return generateMessageFormatJason(e.Message);
            }
        }

        public string SetProductAmountInBasket(int storeId,int productId, int amount)
        {
            return transactionS.SetProductAmountInBasket(storeId,productId, amount);
        }


        private string notLoggedInError()
        {
            Message msg = new Message("User not logged in");
            return JsonConvert.SerializeObject(msg);
        }

        private string notActiveStoreError()
        {
            Message msg = new Message("Store not Active");
            return JsonConvert.SerializeObject(msg);
        }

        private string resultJson(bool ret)
        {
            string msg = ret ? successMsg : failMsg;
            return generateMessageFormatJason(msg);
        }

        private string intJson(int ret)
        {
            IdMessage idMsg = new IdMessage(ret);
            return JsonConvert.SerializeObject(idMsg);
        }

        private string successJson()
        {
            return JsonConvert.SerializeObject(new Message("Success"));
        }

        private string failJson()
        {
            return JsonConvert.SerializeObject(new Message("Fail"));
        }

        private string objDynamicJson(Object obj)
        {
            return JsonHandler.SerializeObjectDynamic(obj);
        }

        private string generateMessageFormatJason(string message)
        {
            return JsonConvert.SerializeObject(new Message(message));
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