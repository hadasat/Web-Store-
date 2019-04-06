using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingFramework.AcceptanceTests
{
    public interface IServiceBridge
    {
        //Req 1_1
        //bool Initialize(string admin, string password);

        //Reg 2_2
        bool Register(string user, string password);

        //Reg 2_3
        bool Login(string user, string password);

        //List<int> GetStores();
        //List<int> GetProductsFromStore(string store);

        //Req 2_5
        List<int> SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank);
        bool GetProductInfo(int id, out string name, out string productDesc, out double price, out string category, out int rank);

        //Req 2_6
        bool AddProductToCart(int productId, int amount);

        //Req 2_7
        int GetShoppingCart(int storeId);

        //returns IDs and Amounts
        Dictionary<int, int> GetProductsInShoppingCart(int cartId);
        bool SetProductAmountInCart(int cartId, int productId, int amount);

        //Req 2_8
        bool BuyShoppingBasket();

        //Req 3_1
        bool Logout();

        //Req 3_2
        int AddStore(string storeName);

        //Req 4_1
        int AddProductToStore(int storeId, string name, string desc, double price, string category);
        bool RemoveProductFromStore(int storeId, int productId);
        bool ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount);

        //Req 4_3
        bool AddStoreOwner(int storeId, string user);

        //Req 4_4
        bool RemoveStoreOwner(int storeId, string user);

        //Req 4_5
        bool AddStoreManager(int storeId, string user);
        bool AddStoreManager(int storeId, string user, bool addRemovePurchasing, bool addRemoveDiscountPolicy, bool addRemoveStoreManger, bool closeStore);

        //Req 4_6
        bool RemoveStoreManager(int storeId, string user);

        //Req 5_1
        //TODO - what are the current permissions available?
        
        //Req 6_2
        bool RemoveUser(string user);
    }
}
