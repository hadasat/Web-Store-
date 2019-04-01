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
        bool Initialize(string admin, string password);

        //Reg 2_2
        bool Register(string user, string password);

        //Reg 2_3
        bool Login(string user, string password);

        //List<int> GetStores();
        //List<int> GetProductsFromStore(string store);

        //Req 2_5
        List<int> SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int storeRank);
        bool GetProductInfo(int id, ref string name, ref string productDesc, ref double price, ref string category, ref int rank);

        //Req 2_6
        bool AddProductToCart(int productId, int amount);

        //Req 2_7
        int GetShoppingCart(int storeId);
        Dictionary<int, int> GetProductsInShoppingCart(int cartId);
        bool SetProductAmountInCart(int cartId, int productId, int amount);

        //Req 2_8
        int GetShoppingBasket();
        bool BuyShoppingBasket(int id);

        //Req 3_1
        bool Logout();

        //Req 3_2
        int AddStore(string storeName);

        //Req 4_1
        int AddProductToStore(int storeId, string name, string desc, double price, string category);
        bool RemoveProductFromStore(int storeId, int productId);
        bool ChangeProductInfo(int productId, string name, string desc, double price, string category, int amount);

        //Req 4_3
        bool AddStoreOwner(int storeId, string user);

        //Req 4_4
        bool RemoveStoreOwner(int storeId, string user);

        //Req 4_5
        bool AddStoreManager(int storeId, string user);

        //Req 4_6
        bool RemoveStoreManager(int storeId, string user);

        //Req 5_1
        //TODO - what are the current permissions available?
        
        //Req 6_2
        bool RemoveUser(string user);
    }
}
