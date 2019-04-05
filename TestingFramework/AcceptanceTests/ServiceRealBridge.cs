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

        private bool wasSuccessful(dynamic msg)
        {
            return msg.message.ToLower() == successMsg;
        }

        private int getId(dynamic msg)
        {
            return msg.id;
        }

        public bool AddProductToCart(int productId, int amount)
        {
            string msg = service.AddProductToBasket(productId, amount);
            dynamic json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        //TODO: what about keywords?
        public int AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            string msg = service.AddProductToStore(storeId, name, desc, price, category);
            dynamic json = JObject.Parse(msg);
            return getId(json);
        }

        public int AddStore(string storeName)
        {
            string msg = service.AddStore(storeName);
            dynamic json = JObject.Parse(msg);
            return getId(json);
        }

        public bool AddStoreManager(int storeId, string user)
        {
            //TODO: add roles
            return false;
        }

        public bool AddStoreOwner(int storeId, string user)
        {
            string msg = service.AddStoreOwner(storeId, user);
            dynamic json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool BuyShoppingBasket()
        {
            //TODO: remove -1 when calling BuyShoppingBasket
            string msg = service.BuyShoppingBasket(-1);
            dynamic json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool ChangeProductInfo(int productId, string name, string desc, double price, string category, int amount)
        {
            string msg = service.ChangeProductInfo(productId, name, desc, price, category, amount);
            dynamic json = JObject.Parse(msg);
            return wasSuccessful(json);
        }

        public bool GetProductInfo(int id, out string name, out string productDesc, out double price, out string category, out int rank)
        {
            //TODO: JSON of product
            name = "";
            productDesc = "";
            price = 0.0;
            category = "";
            rank = 0;
            return false;
        }

        public Dictionary<int, int> GetProductsInShoppingCart(int cartId)
        {
            //TODO GetProductsInShoppingCart
            throw new NotImplementedException();
        }

        public int GetShoppingCart(int storeId)
        {
            throw new NotImplementedException();
        }

        public bool Initialize(string admin, string password)
        {
            throw new NotImplementedException();
        }

        public bool Login(string user, string password)
        {
            throw new NotImplementedException();
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }

        public bool Register(string user, string password)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProductFromStore(int storeId, int productId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveStoreManager(int storeId, string user)
        {
            throw new NotImplementedException();
        }

        public bool RemoveStoreOwner(int storeId, string user)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUser(string user)
        {
            throw new NotImplementedException();
        }

        public List<int> SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int storeRank)
        {
            throw new NotImplementedException();
        }

        public bool SetProductAmountInCart(int cartId, int productId, int amount)
        {
            throw new NotImplementedException();
        }
    }


    class boolMessage
    {
        public string message;
    }

    class idMessage
    {
        public int id;
    }
}
