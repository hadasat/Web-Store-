using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.System_Service
{
    public interface UserInterface
    {
        /*
         * Message Format: {message: String}
         * If the 'message' field equal to "Success" so the method succeed.
         * otherwise the content contain information about the error
         * 
         * 
         * 
         * Search Format: { products : List<Product>}
         * Product Format: {id : int , name :string , price : int , rank : int  , category : string ,storeId : int }
         * 
         * Shopping basket Jason Object: {products : Dictionary<store,Shopping cart>}
         * Dictionary info: <Product Object, Amount>
         * 
         */


        //Return: Message Format
        String logout();

        //Return: Message Format
        String login(string username, string password);

        //Return: Message Format
        String Register(string user, string password);

        //Return: Search Format
        String SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank);

        //Return Product Jason Object
        String GetProductInfo(int productId);

        //Return: Shopping basket Jason Object
        String GetShoppingCart(int storeId);

        String GetShoppingBasket();

        //Return: Message Format
        String SetProductAmountInBasket(int storeId,int productId, int amount);

        //Return: Message Format
        String AddProductToBasket(int storeId, int productId, int amount);

        //Return: Message Format
        String BuyShoppingBasket();

        //TODO jonathan: please choose format that includes ID
        //Return: ID
        String AddStore(string storeName);

        //TODO jonathan: please choose format that includes ID
        //Return: ID
        String AddProductToStore(int storeId, string name, string desc, double price, string category);

        //Return: Message Format
        String AddProductToStock(int storeId, int productId, int amount);

        //Return: Message Format
        String ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount);

        //Return: Message Format
        String RemoveProductFromStore(int storeId, int productId);

        //Return: Message Format
        /// <summary>
        /// adding create store manager with full rules permissions and add it to user. Only store owner can use this function. 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        String AddStoreOwner(int storeId, string username);

        //Return: Message Format
        String RemoveStoreManager(int storeId, string username);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="user"></param>
        /// <param name="roles">Json Format: {Bool: addRemoveProducts,
        /// addRemovePurchasing,
        /// addRemoveDiscountPolicy,
        //addRemoveStoreManger,
        //closeStore,
        //customerCommunication}
        /// <returns>//Return: Message Format</returns>
        String AddStoreManager(int storeId, string user, String roles);

        //Return: Message Format
        String RemoveUser(string user);

        //Return: Message Format
        String addPurchasingPolicy(int storeId);

        //Return: Message Format
        String removePurchasingPolicy(int storeId);

        //Return: Message Format
        String addDiscountPolicy(int storeId);

        //Return: Message Format
        String removeDiscountPolicy(int storeId);

        //Return: Message Format
        String closeStore(int storeID);



    }
}