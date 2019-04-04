using Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.System_Service
{
    public interface IGodObject
    {
       
        /// <returns>member id</returns>
        int addMember(string username, string password);

        /// <returns>product id</returns>
        int addProductToStore(int storeId, string name, double price, string category, string desc, string keyword, int amount, int rank);
        
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
            ConnectionStubTemp.registerNewUser(username, password);
            return ConnectionStubTemp.memberIDGenerator - 1;

        }

        public int addProductToStore(int storeId, string name, double price, string category, string desc, string keyword, int amount, int rank)
        {
            Store store = WorkShop.getStore(storeId);
            Product p = new Product(name, price, category, rank, amount);
            store.GetStock().Add(p.getId(), p);
            return p.getId();
        }

        public int addStore(string name, int rank, int ownerId)
        {
            Member owner = ConnectionStubTemp.getMember(ownerId);
            WorkShop.createNewStore(name, rank, true, owner);
            return WorkShop.id - 1;
        }

        public bool appointUserToStoreOwnership(int storeId, int newOwnerId, int storeOwnerId)
        {
            Store store = WorkShop.getStore(storeId);
            Member owner = ConnectionStubTemp.getMember(storeOwnerId);
            Member newowner = ConnectionStubTemp.getMember(storeOwnerId);
            Roles role = new Roles(true, true, true, true, true, true, true, true);
            owner.addManager(newowner.username,role,store);
            return true;
           
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
            Store store = WorkShop.getStore(storeId);
            return store.GetStock().Remove(productID);
        }

        public bool removeStore(int storeId , int ownerId)
        {
            Member owner = ConnectionStubTemp.getMember(ownerId);
            WorkShop.closeStore(storeId, owner);
            return true;
        }
    }
}
