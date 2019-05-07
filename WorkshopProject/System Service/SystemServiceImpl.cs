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
        PolicyService policyS;

        //Those fileds are temporary
        Boolean loggedIn;
        public User user { get; set; }

        private string adminUsername = "Admin";
        private string adminPassword = "Admin";

        public SystemServiceImpl()
        {
            user = new User();
            storeS = new StoreService(user);
            transactionS = new TransactionService(user);
            userS = new UserService(user);
            policyS = new PolicyService(user);
            loggedIn = false;
        }

        public void updateMember(User member)
        {
            storeS.user = member;
            transactionS.user = member;
            user = member;
            policyS.user = member;
            this.user = member;
        }

        public string AddProductToBasket(int storeId,int productId, int amount)
        {
            return transactionS.AddProductToBasket(storeId,productId, amount);
        }

        public string AddProductToStock(int storeId, int productId, int amount)
        {
            if (!loggedIn)
                return notLoggedInError();
            return storeS.AddProductToStock(storeId, productId, amount);
        }

        public string AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            if (!loggedIn)
                return notLoggedInError();
            return storeS.AddProductToStore(storeId, name, desc, price, category);
        }

        public string AddStore(string storeName)
        {
            if (!loggedIn)
                return notLoggedInError();
            return storeS.AddStore(storeName);
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
            return storeS.ChangeProductInfo(storeId, productId, name, desc, price, category, amount);
        }

        public string closeStore(int storeID)
        {
            if (!loggedIn)
                return notLoggedInError();
            return storeS.CloseStore(storeID);
        }

        public string GetProductInfo(int productId)
        {
            return  storeS.GetProductInfo(productId);
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

       

        public string RemoveProductFromStore(int storeId, int productId)
        {
            if (!loggedIn)
                return notLoggedInError();
            return storeS.RemoveProductFromStore(storeId, productId);
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
            return storeS.SearchProducts(name, category, keyword, startPrice, endPrice, productRank, storeRank);
        }

        public string SetProductAmountInBasket(int storeId,int productId, int amount)
        {
            return transactionS.SetProductAmountInBasket(storeId,productId, amount);
        }

        //policies

        internal string addDiscountPolicy(int storeId,string policy)
        {
            if (!loggedIn)
                return notLoggedInError();
            return policyS.addDiscountPolicy(storeId, policy);

        }

        internal string removeDiscountPolicy(int storeId,int policyId)
        {
            if (!loggedIn)
                return notLoggedInError();
            return policyS.removeDiscountPolicy(storeId, policyId);
        }

        //purchasing
        internal string addPurchasingPolicy(int storeId,String policy)
        {
            if (!loggedIn)
                return notLoggedInError();
            return policyS.addPurchasingPolicy(storeId, policy);
        }

        internal string removePurchasingPolicy(int storeId,int policyId)
        {
            if (!loggedIn)
                return notLoggedInError();
            return policyS.removePurchasingPolicy(storeId, policyId);
        }

        //store
        internal string addStorePolicy(int storeId, String policy)
        {
            if (!loggedIn)
                return notLoggedInError();
            return policyS.addStorePolicy(storeId,policy);

        }

        internal string removeStorePolicy(int storeId, int policyId)
        {
            if (!loggedIn)
                return notLoggedInError();
            return policyS.removeStorePolicy(storeId, policyId);
        }

        //jonathan
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

        //jonathan - no idea how SystemAdmin object can be added
        //private string addAdmin()
        //{
        //    return Register(adminUsername, adminPassword);
        //}
    }

    //messages:

    public class Message
    {
        public string message;
        public Message(string message)
        {
            this.message = message;
        }
    }

    //TODO: jonathan added this just to return store IDs when using AddStore
    public class IdMessage
    {
        public int id;
        public IdMessage(int id)
        {
            this.id = id;
        }
    }

    public static class JMessage
    {
        public static string notActiveStoreError()
        {
            Message msg = new Message("Store not Active");
            return JsonConvert.SerializeObject(msg);
        }

        public static string successJason()
        {
            return JsonConvert.SerializeObject(new Message("Success"));
        }

        public static string generateMessageFormatJason(string message)
        {
            return JsonConvert.SerializeObject(new Message(message));
        }

        public static string generateIDMessageFormatJason(int id)
        {
            return JsonConvert.SerializeObject(new IdMessage(id));
        }
    }
}