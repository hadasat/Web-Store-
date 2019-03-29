using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;

namespace Managment
{


    public class Roles
    {
        private bool addRemoveProducts;
        private bool addRemovePurchasing;
        private bool addRemoveDiscountPolicy;
        private bool addRemoveStoreManger;
        private bool closeStore;
        private bool customerCommunication;

        public Roles(bool addRemoveProducts, 
                     bool addRemovePurchasing, 
                     bool addRemoveDiscountPolicy, 
                     bool addRemoveStoreManger, 
                     bool closeStore, 
                     bool customerCommunication)
        {
            this.AddRemoveProducts = addRemoveProducts;
            this.addRemovePurchasing = addRemovePurchasing;
            this.addRemoveDiscountPolicy = addRemoveDiscountPolicy;
            this.addRemoveStoreManger = addRemoveStoreManger;
            this.closeStore = closeStore;
            this.customerCommunication = customerCommunication;
        }

        public bool CompareRoles(Roles otherRoles)
        {
            return true;
        }

        public bool AddRemoveProducts { get => addRemoveProducts; set => addRemoveProducts = value; }
        public bool AddRemovePurchasing { get => addRemovePurchasing; set => addRemovePurchasing = value; }
        public bool AddRemoveDiscountPolicy { get => addRemoveDiscountPolicy; set => addRemoveDiscountPolicy = value; }
        public bool AddRemoveStoreManger { get => addRemoveStoreManger; set => addRemoveStoreManger = value; }
        public bool CloseStore { get => closeStore; set => closeStore = value; }
        public bool CustomerCommunication { get => customerCommunication; set => customerCommunication = value; }
    }

    public class StoreManager
    {
        Store store;
        Roles myRoles;
        public LinkedList<StoreManager> subManagers;

        public StoreManager(Store store, Roles storeRoles)
        {
            this.store = store;
            this.myRoles = storeRoles;
            this.subManagers = new LinkedList<StoreManager>();
        }



        /*about roles: the client will choose what roles he wants to give the new
          manager (needs to be like hes and below) */
        public StoreManager createNewManager(Member member, Roles roles)
        {
            if (myRoles.CompareRoles(roles))
            {
                StoreManager newSubStoreManager = new StoreManager(this.store, roles);
                subManagers.AddFirst(newSubStoreManager);
                return newSubStoreManager;
            }
            else
            {
                throw new Exception("this manager try to give more roles than he can");
            }
        }





        public bool removeManager(StoreManager managerToRemove)
        {
            if (subManagers.Contains(managerToRemove))
            {
                recursiveCleanManager(managerToRemove);
                return subManagers.Remove(managerToRemove);
            } else
            {
                throw new Exception("The manager to remove is not below to this manager");
            }
        }

        private void recursiveCleanManager(StoreManager managerToRemove)
        {
            LinkedList<StoreManager> subManagers = managerToRemove.subManagers;
            if (subManagers.Count != 0)
            {
                foreach (StoreManager sm in subManagers)
                {
                    recursiveCleanManager(sm);
                    subManagers.Remove(sm);
                }
            }
        }
    }
}
