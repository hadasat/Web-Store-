using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingFramework.AcceptanceTests
{
    public interface IGodObject
    {

        /// <returns>member id</returns>
        int addMember(string username, string password);

        /// <returns>product id</returns>
        int addProductToStore(int storeId, string username, int price, string category, string desc, string keyword, int amount, int rank);

        /// <returns>store id</returns>
        int addStore(string name, int rank, int ownerId);

        bool appointUserToStoreOwnership(int storeId, int newOwnerId, int storeOwnerId);


        /// <param name="roles">[addRemoveProducts,
        /// addRemovePurchasing,addRemoveDiscountPolicy, addRemoveStoreManger,
        /// closeStore,customerCommunication,appointOwner,appointManager]</param>
        bool makeUserManager(int storeId, int storeOwnerId, int newManagerId, bool[] roles);

        bool removeMember(int memberId);

        bool removeStore(int storeId);

        bool removeManagerFromStore(int storeId, int ManagerId);    //manager or owner 

        bool removeProductFromStore(int storeId, int productID);

        bool removeProductFromStock(int storeId, int ProductId, int amountToRemove);

    }

    public class GodObject : IGodObject
    {
        public int addMember(string username, string password)
        {
            throw new NotImplementedException();
        }

        public int addProductToStore(int storeId, string username, int price, string category, string desc, string keyword, int amount, int rank)
        {
            throw new NotImplementedException();
        }

        public int addStore(string name, int rank, int ownerId)
        {
            throw new NotImplementedException();
        }

        public bool appointUserToStoreOwnership(int storeId, int newOwnerId, int storeOwnerId)
        {
            throw new NotImplementedException();
        }

        public bool makeUserManager(int storeId, int storeOwnerId, int newManagerId, bool[] roles)
        {
            throw new NotImplementedException();
        }

        public bool removeManagerFromStore(int storeId, int ManagerId)
        {
            throw new NotImplementedException();
        }

        public bool removeMember(int memberId)
        {
            throw new NotImplementedException();
        }

        public bool removeProductFromStock(int storeId, int ProductId, int amountToRemove)
        {
            throw new NotImplementedException();
        }

        public bool removeProductFromStore(int storeId, int productID)
        {
            throw new NotImplementedException();
        }

        public bool removeStore(int storeId)
        {
            throw new NotImplementedException();
        }
    }
}
