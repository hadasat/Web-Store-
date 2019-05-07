using Managment;
using Password;
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

        bool removeMember(string member);

        bool removeStore(int storeId, int ownerId);

        bool removeManagerFromStore(int storeId, int ManagerId);    //manager or owner 

        bool removeProductFromStore(int storeId, int productID);

        bool removeProductFromStock(int storeId, int ProductId, int amountToRemove);
        void cleanUpAllData();

    }

    public class GodObject : IGodObject
    {


        public int addMember(string username, string password)
        {
            ConnectionStubTemp.registerNewUser(username, password, DateTime.Today.AddYears(-1), "");
            return ConnectionStubTemp.memberIDGenerator - 1;

        }

        public int addProductToStore(int storeId, string name, double price, string category, string desc, string keyword, int amount, int rank)
        {
            Store store = WorkShop.getStore(storeId);
            Product p = new Product(name, price,desc, category, rank, amount, storeId);
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
            Roles newRoles = new Roles(roles[0], roles[1], roles[2], roles[3], roles[4],roles[5], roles[6], roles[7]);
            Store store = WorkShop.getStore(storeId);
            Member storeOwner = ConnectionStubTemp.members[storeOwnerId];
            Member newManager = ConnectionStubTemp.members[newManagerId];
            LinkedList<StoreManager> nmstoreManagers = newManager.storeManaging;
            StoreManager newStoreManager = new StoreManager(store, newRoles);
            nmstoreManagers.AddFirst(newStoreManager);

            LinkedList<StoreManager> storeManagers = storeOwner.storeManaging;
            foreach (StoreManager sm in storeManagers)
            {
                if (sm.GetStore().Id == storeId)
                {
                    sm.SubManagers.AddFirst(newStoreManager);
                    return true;
                }
            }
            return false;
        }

        public bool removeManagerFromStore(int storeId, int ManagerId)
        {
            Member member = ConnectionStubTemp.members[ManagerId];
            LinkedList<StoreManager> storeManagers = member.storeManaging;
            foreach (StoreManager sm in storeManagers)
            {
                if (sm.GetStore().Id == storeId)
                {
                    if (sm.GetFather() == null)
                    {
                        WorkShop.closeStore(storeId, member);
                    }
                    else
                    {
                        sm.GetFather().removeManager(sm);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool removeMember(int memberId)
        {
            string username = ConnectionStubTemp.members[memberId].username;
            ConnectionStubTemp.members.Remove(memberId);
            ConnectionStubTemp.mapIDUsermane.Remove(username);
            return true;
        }

        public bool removeMember(string member)
        {
            int id;
            bool ret = ConnectionStubTemp.mapIDUsermane.TryGetValue(member, out id);
            ConnectionStubTemp.mapIDUsermane.Remove(member);
            ConnectionStubTemp.members.Remove(id);     
            return ret;
        }

        public bool removeProductFromStock(int storeId, int ProductId, int amountToRemove)
        {
            Store store = WorkShop.getStore(storeId);
            store.GetStock()[ProductId].amount -= amountToRemove;
            return true;

        }

        public bool removeProductFromStore(int storeId, int productID)
        {
            Store store = WorkShop.getStore(storeId);
            return store.GetStock().Remove(productID);
        }

        public bool removeStore(int storeId, int ownerId)
        {
            Member owner = ConnectionStubTemp.getMember(ownerId);
            WorkShop.closeStore(storeId, owner);
            WorkShop.stores.Remove(storeId);
            return true;
        }

        public void cleanUpAllData()
        {
            WorkShop.stores = new Dictionary<int, Store>();
            WorkShop.id = 0;
            ConnectionStubTemp.pHandler = new PasswordHandler();
            ConnectionStubTemp.members = new Dictionary<int, Member>();
            ConnectionStubTemp.mapIDUsermane = new Dictionary<string, int>();
            ConnectionStubTemp.memberIDGenerator = 0;
            ConnectionStubTemp.init();
        }
    }
}