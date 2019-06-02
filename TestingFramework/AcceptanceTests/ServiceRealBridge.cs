using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.System_Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;
using WorkshopProject;
using Managment;

namespace TestingFramework.AcceptanceTests
{
    public class ServiceRealBridge : IServiceBridge
    {
        private UserInterface service;
        private string successMsg = "success";

        public ServiceRealBridge()
        {
            service = new SystemServiceImpl();
        }

        private bool wasSuccessful(JObject msg)
        {
            if (msg["message"] == null)
            {
                return false;
            }
            bool ret = ((string)msg["message"]).ToLower() == successMsg;
            return ret;
        }

        private int getId(JObject msg)
        {
            if (msg["id"] == null)
            {
                return -1;
            }
            int ret = (int)msg["id"];
            return ret;
        }

        private int getAmount(JObject msg)
        {
            if (msg["id"] == null)
            {
                return -1;
            }
            int ret = (int)msg["amount"];
            return ret;
        }

        private string createRolesJson(bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore, bool addRemoveStorePolicy)
        {
            Roles roles = new Roles(
                true,
                addRemovePurchasing,
                addRemoveDiscountPolicy,
                addRemoveStoreManger,
                closeStore,
                false,
                true,
                addRemoveStoreManger,
                addRemoveStorePolicy
                );
            return JsonHandler.SerializeObjectDynamic(roles);
        }

        //{id : int , name :string , price : int , rank : int  , category : string  }
        private void retrieveProductInfo(JObject product, out string name, out string productDesc, out double price, out string category, out int rank, out int amount)
        {
            name = (string)product["name"];
            productDesc = (string)product["name"]; //TODO: must be fixed to description
            price = (double)product["price"];
            category = (string)product["category"];
            rank = (int)product["rank"];
            amount = (int)product["amount"];
        }

        public bool AddProductToBasket(int storeId,int productId, int amount)
        {
            string msg = service.AddProductToBasket(storeId,productId, amount);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        //TODO: what about keywords?
        public int AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            string msg = service.AddProductToStore(storeId, name, desc, price, category);
            JObject json = JObject.Parse(msg);
            return getId(json);
        }

        public int AddStore(string storeName)
        {
            string msg = service.AddStore(storeName);
            JObject json = JObject.Parse(msg);
            return getId(json);
        }

        public bool AddStoreManager(int storeId, string user)
        {
            return AddStoreManager(storeId, user, false, false, false, false,false);
        }

        public bool AddStoreManager(int storeId, string user, bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore,bool addRemoveStorePolicy)
        {
            string roles = createRolesJson(addRemovePurchasing, addRemoveDiscountPolicy, addRemoveStoreManger, closeStore, addRemoveStorePolicy);
            string msg = service.AddStoreManager(storeId, user, roles);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool AddStoreOwner(int storeId, string user)
        {
            string msg = service.AddStoreOwner(storeId, user);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool BuyShoppingBasket()
        {
            //TODO amsel fix this shit
            string msg = service.BuyShoppingBasket();
            JObject json = JObject.Parse(msg);
            return ((int)json["id"] > 0);
        }

        public bool CloseStore(int storeID)
        {
            string msg = service.closeStore(storeID);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount)
        {
            string msg = service.ChangeProductInfo(storeId, productId, name, desc, price, category, amount);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool GetProductInfo(int id, out string name, out string productDesc, out double price, out string category, out int rank, out int amount)
        {
            string msg = service.GetProductInfo(id);
            JObject json = JObject.Parse(msg);
            if (getId(json) != id)
            {
                name = "";
                productDesc = "";
                price = 0.0;
                category = "";
                rank = 0;
                amount = 0;
                return false;
            }
            retrieveProductInfo(json, out name, out productDesc, out price, out category, out rank, out amount);
            return true;
        }

        public Dictionary<int, int> GetProductsInShoppingCart(int storeId)
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();

            string msg = service.GetShoppingCart(storeId);
            JObject json = JObject.Parse(msg);
            JArray productsAndAmounts = (JArray)json["products"];

            foreach (JObject pair in productsAndAmounts)
            {
                int productId = (int)pair["product"]["id"];
                int amount = (int)pair["amount"];
                ret.Add(productId, amount);
            }
            return ret;
        }

        public int GetShoppingCart(int storeId)
        {
            string msg = service.GetShoppingCart(storeId);
            JObject json = JObject.Parse(msg);
            return getId(json);
        }

        public int GetShoppingBasket()
        {
            string msg = service.GetShoppingBasket();
            JObject json = JObject.Parse(msg);
            return getId(json);
        }

        public bool Login(string user, string password)
        {
            string msg = service.login(user, password);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool Logout()
        {
            string msg = service.logout();
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool Register(string user, string password,DateTime birthdate, string country)
        {
            string msg = service.Register(user, password,birthdate,country);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool RemoveProductFromStore(int storeId, int productId)
        {
            string msg = service.RemoveProductFromStore(storeId, productId);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool RemoveStoreManager(int storeId, string user)
        {
            string msg = service.RemoveStoreManager(storeId, user);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool RemoveStoreOwner(int storeId, string user)
        {
            string msg = service.RemoveStoreManager(storeId, user);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool RemoveUser(string user)
        {
            string msg = service.RemoveUser(user);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public List<int> SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank)
        {
            List<int> ret = new List<int>();
            string msg = service.SearchProducts(name, category, keyword, startPrice, endPrice, productRank, storeRank);
            //JArray json = JArray.Parse(msg);
            List<Product> json = JsonHandler.DeserializeObject<List<Product>>(msg);
            foreach (Product product in json)
            {
                ret.Add(product.getId());
            }
            return ret;
        }

        public bool SetProductAmountInBasket(int storeId, int productId, int amount)
        {
            string msg = service.SetProductAmountInBasket(storeId,productId, amount);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool AddProductToStock(int storeId, int productId, int amount)
        {
            string msg = service.AddProductToStock(storeId, productId, amount);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public int addDiscountPolicy(int storeId, string policy)
        {
            string msg = service.addDiscountPolicy(storeId, policy);
            JObject json = JObject.Parse(msg);
            return getId(json);
        }

        public bool removeDiscountPolicy(int storeId, int policyId)
        {
            string msg = service.removeDiscountPolicy(storeId, policyId);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public int addPurchasingPolicy(int storeId, string policy)
        {
            string msg = service.addPurchasingPolicy(storeId, policy);
            JObject json = JObject.Parse(msg);
            return getId(json);
        }

        public bool removePurchasingPolicy(int storeId, int policyId)
        {
            string msg = service.removePurchasingPolicy(storeId, policyId);
            JObject json = JObject.Parse(msg);
            return wasSuccessful(json);
        }
    }

    class ManagerRolesContainer
    {
        public bool addRemovePurchasing;
        public bool addRemoveDiscountPolicy;
        public bool addRemoveStoreManger;
        public bool closeStore;
        public bool addRemoveStorePolicy;

        public ManagerRolesContainer(bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore, bool addRemoveStorePolicy)
        {
            this.addRemovePurchasing = addRemovePurchasing;
            this.addRemoveDiscountPolicy = addRemoveDiscountPolicy;
            this.addRemoveStoreManger = addRemoveStoreManger;
            this.closeStore = closeStore;
            this.addRemoveStorePolicy = addRemoveStorePolicy;
        }
    }
}
