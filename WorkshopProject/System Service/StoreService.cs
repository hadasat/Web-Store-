using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.System_Service
{
    //TODO: jonathan added this just to return store IDs when using AddStore
    public class IdMessage
    {
        public int id;
        public IdMessage(int id)
        {
            this.id = id;
        }
    }
   
    public class StoreService
    {
        internal User user;

        public StoreService(User user)
        {
            this.user = user;
        }
        private void notActiveStoreError()
        {
            throw new Exception("Store not Active");

            //Message msg = new Message("Store not Active");
            //return JsonConvert.SerializeObject(msg);
        }

        internal bool addDiscountPolicy(int storeId)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                throw new Exception("Store does not exist");
            if (!store.isActive)
                throw new Exception("Store does not exist");

            return true; //All Valid
        }

        internal bool AddProductToStock(int storeId, int productId, int amount)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
               throw new Exception("Store does not exist");
            if (!store.isActive)
                notActiveStoreError();

            Product product = store.getProduct(productId);
            if (product == null)
                throw new Exception("Product does not exist in store id" + storeId);


            if (!store.addProductTostock(user, product, amount))
                throw new Exception("User does not have permission");

            return true; //All Valid
        }

        internal int AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            sanitizeName(name);
            checkPrice(price);
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                throw new Exception("Store does not exist");
            if (!store.isActive)
                notActiveStoreError();
            int id = store.addProduct(user, name, desc, price, category);
            if (id == -1)
                throw new Exception("User does not have permission");

            //return successJason(); //All Valid
            //jonathan - we need the id of the new store, not a message
            //IdMessage idMsg = new IdMessage(id);
            //return JsonConvert.SerializeObject(idMsg);

            return id;
        }

        internal int AddStore(string storeName)
        {
            sanitizeName(storeName);
            int id = WorkShop.createNewStore(storeName, 0, true, (Member)user);
            //return successJason(); //All Valid
            //jonathan - we need the id of the new store, not a message
            //IdMessage idMsg = new IdMessage(id);
            //return JsonConvert.SerializeObject(idMsg);

            return id;
        }

        internal bool ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                throw new Exception("Store does not exist");
            if (!store.isActive)
                notActiveStoreError();

            if (!store.changeProductInfo(user, productId, name, desc, price, category, amount))
                throw new Exception("Error: User does not have permission Or Product does not exist");

            return true; //All Valid
        }

        internal bool CloseStore(int storeID)
        {
            if (!WorkShop.closeStore(storeID, (Member)user))
                throw new Exception("Error: User does not have permission");

            return true; //All Valid
        }

        internal Product GetProductInfo(int productId)
        {
            Product product = WorkShop.getProduct(productId);
            if (product == null)
                throw new Exception("Product does not exist in store id");
            
            return product;
        }

        internal string removeDiscountPolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        internal bool RemoveProductFromStore(int storeId, int productId)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                throw new Exception("Store does not exist");
            if (!store.isActive)
                notActiveStoreError();

            Product product = store.getProduct(productId);
            if (product == null)
                throw new Exception("Product does not exist in store id" + storeId);

            if (!store.removeProductFromStore(user, product))
                throw new Exception("Error: User does not have permission");

            return true; //All Valid
        }


        internal List<Product> SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank)
        {
            List<Product> products = WorkShop.search(name, category, startPrice, endPrice, productRank, storeRank);
            return products;
            //return JsonConvert.SerializeObject(products);
        }


        internal string removePurchasingPolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        //jonathan
        private bool checkPrice(double price)
        {
            if (price <= 0)
            {
                throw new Exception("product price must be positive");
            }
            return true;
        }

        private bool sanitizeName(string storeName)
        {
        string[] illegalChars = { ";" };
            foreach (string c in illegalChars)
            {
                if (storeName.Contains(c))
                {
                    throw new Exception("name contains illegal charachters");
                    //return false;
                }
            }
            return true;
        }

    }
}