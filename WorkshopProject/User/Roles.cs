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
        bool addRemoveProducts;
        bool addRemovePurchasing;
        bool addRemoveDiscountPolicy;
        bool addRemoveStoreManger;
        bool closeStore;
        bool customerCommunication;

        public Roles(bool addRemoveProducts, 
                     bool addRemovePurchasing, 
                     bool addRemoveDiscountPolicy, 
                     bool addRemoveStoreManger, 
                     bool closeStore, 
                     bool customerCommunication)
        {
            this.addRemoveProducts = addRemoveProducts;
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
    }

    public class StoreManager
    {
        Store store;
        Roles myRoles;
        public LinkedList<StoreManager> subManagers;

        public StoreManager(Store store)
        {
            this.store = store;
            this.subManagers = new LinkedList<StoreManager>();
        }

        public StoreManager createNewManager(Member member, Roles roles)
        {
            if (myRoles.CompareRoles(roles))
            {
                StoreManager newSubStoreManager = new StoreManager(this.store);
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
            return subManagers.Remove(managerToRemove);
        }
    }
}
