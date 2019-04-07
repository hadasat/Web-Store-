using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.System_Service
{
    public class Message
    {
        public string message;
        public Message(string message)
        {
            this.message = message;
        }
    }

    public class StoreService
    {
        internal User user;

        public StoreService(User user)
        {
            this.user = user;
        }

        private string successJason()
        {
            return JsonConvert.SerializeObject(new Message("Success"));
        }

        private string generateMessageFormatJason(string message)
        {
            return JsonConvert.SerializeObject(new Message(message));
        }

        internal string addDiscountPolicy(int storeId)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                return generateMessageFormatJason("Store does not exist");


            //TODO

            return successJason(); //All Valid

        }

        internal string AddProductToStock(int storeId, int productId, int amount)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                return generateMessageFormatJason("Store does not exist");

            Product product = store.getProduct(productId);
            if (product == null)
                return generateMessageFormatJason("Product does not exist in store id" + storeId);


            if (!store.addProductTostock(user, product, amount))
                return generateMessageFormatJason("User does not have permission");

            return successJason(); //All Valid
        }

        internal string AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                return generateMessageFormatJason("Store does not exist");

            if (store.addProduct(user, name, desc, price, category) == -1)
                return generateMessageFormatJason("User does not have permission");

            return successJason(); //All Valid

        }

        internal string AddStore(string storeName)
        {
            WorkShop.createNewStore(storeName, 0, true, (Member)user);
            return successJason(); //All Valid

        }

        internal string ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                return generateMessageFormatJason("Store does not exist");

            if (!store.changeProductInfo(user, productId, name, desc, price, category, amount))
                return generateMessageFormatJason("Error: User does not have permission Or Product does not exist");

            return successJason(); //All Valid
        }

        internal string CloseStore(int storeID)
        {
            if (!WorkShop.closeStore(storeID, (Member)user))
                return generateMessageFormatJason("Error: User does not have permission");

            return successJason(); //All Valid
        }

        internal string GetProductInfo(int productId)
        {
            Product product = WorkShop.getProduct(productId);
            if (product == null)
                return generateMessageFormatJason("Product does not exist in store id");

            return JsonConvert.SerializeObject(product);


        }

        internal string removeDiscountPolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        internal string RemoveProductFromStore(int storeId, int productId)
        {
            Store store = WorkShop.getStore(storeId);
            if (store == null)
                return generateMessageFormatJason("Store does not exist");

            Product product = store.getProduct(productId);
            if (product == null)
                return generateMessageFormatJason("Product does not exist in store id" + storeId);

            if (!store.removeProductFromStore(user, product))
                return generateMessageFormatJason("Error: User does not have permission");

            return successJason(); //All Valid
        }


        internal string SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank)
        {
            return JsonConvert.SerializeObject(WorkShop.search(name, category, startPrice, endPrice, productRank, storeRank));
        }


        internal string removePurchasingPolicy(int storeId)
        {
            throw new NotImplementedException();
        }


    }
}