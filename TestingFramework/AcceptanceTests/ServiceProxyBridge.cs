﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingFramework.AcceptanceTests
{
    public class ServiceProxyBridge : IServiceBridge
    {
        public bool AddProductToCart(int productId, int amount)
        {
            throw new NotImplementedException();
        }

        public int AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            throw new NotImplementedException();
        }

        public bool RemoveProductFromStore(int storeId, int productId)
        {
            throw new NotImplementedException();
        }

        public int AddStore(string storeName)
        {
            throw new NotImplementedException();
        }

        public bool AddStoreManager(int storeId, string user)
        {
            throw new NotImplementedException();
        }

        public bool AddStoreOwner(int storeId, string user)
        {
            throw new NotImplementedException();
        }

        public bool BuyShoppingBasket(int id)
        {
            throw new NotImplementedException();
        }

        public bool GetProductInfo(int id, out string name, out string productDesc, out double price, out string category, out int rank)
        {
            throw new NotImplementedException();
        }
        
        public bool ChangeProductInfo(int productId, string name, string desc, double price, string category, int amount)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, int> GetProductsInShoppingCart(int cartId)
        {
            throw new NotImplementedException();
        }

        public int GetShoppingBasket()
        {
            throw new NotImplementedException();
        }

        public int GetShoppingCart(int storeId)
        {
            throw new NotImplementedException();
        }

        public List<int> getProductsInCart(int cartId)
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
}