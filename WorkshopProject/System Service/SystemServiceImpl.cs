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
        User user;

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
            userS.user = member;
            this.user = member;
        }

        public string addDiscountPolicy(int storeId)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.addDiscountPolicy(storeId);
        }

        public string AddProductToBasket(int productId, int amount)
        {
            return transactionS.AddProductToBasket(productId, amount);
        }

        public string AddProductToStock(int storeId, int productId, int amount)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.AddProductToStock(storeId,productId,amount);
        }

        public string AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.AddProductToStore(storeId, name, desc, price, category);
        }

        public string addPurchasingPolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        public string AddStore(string storeName)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.AddStore(storeName);
        }

        public string AddStoreManager(int storeId, string user, string roles)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return userS.AddStoreManager(storeId, user, roles);    ///TODO:::::::::::::::::;///////////// :))))))
        }

        public string AddStoreOwner(int storeId, string user)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return userS.AddStoreOwner(storeId, user);
        }

        public string BuyShoppingBasket(int id)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return transactionS.BuyShoppingBasket();
        }

        public string ChangeProductInfo(int productId, string name, string desc, double price, string category, int amount)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.ChangeProductInfo(productId, name, desc, price, category, amount);
        }

        public string closeStore(int storeID)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.CloseStore(storeID);
        }

        public string GetProductInfo(int id)
        {
            return  storeS.GetProductInfo(id);
        }

        public string GetShoppingCart(int storeId)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return transactionS.GetShoppingCart(storeId);
        }

        public string login(string username, string password)
        {
            loggedIn = true;
            String toReturn =  userS.login(username, password);
            updateMember(userS.user);
            return toReturn;
        }

        public string logout()
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            loggedIn = false;
            String toReturn =  userS.logout();
            updateMember(new User());
            return toReturn;
        }

        public string Register(string user, string password)
        {
            return userS.Register(user, password);
        }

        public string removeDiscountPolicy(int storeId)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.removeDiscountPolicy(storeId);
        }

        public string RemoveProductFromStore(int storeId, int productId)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.RemoveProductFromStore(storeId, productId);
        }

        public string removePurchasingPolicy(int storeId)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return storeS.removeProductFromStore(storeId);
        }

        public string RemoveStoreManager(int storeId, string user)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return userS.RemoveStoreManager(storeId, user);
        }

        public string RemoveUser(string user)
        {
            if (!loggedIn)
                throw new Exception("Not logged in");
            return userS.RemoveUser(user);
        }

        public string SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int storeRank)
        {
            return storeS.SearchProducts(name, category, keyword, startPrice, endPrice, storeRank);
        }

        public string SetProductAmountInCart(int productId, int amount)
        {

            return transactionS.SetProductAmountInCart(productId, amount);
        }
    }
}
