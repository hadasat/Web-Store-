using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.Log;

namespace Managment
{


    public class Roles
    {
        [Key]
        public int id { get; set; }

        public bool AddRemoveProducts { get; set; }
        public bool AddRemovePurchasing { get; set; }
        public bool AddRemoveDiscountPolicy { get; set; }
        public bool AddRemoveStorePolicy { get; set; }
        public bool AddRemoveStoreManger { get; set; }
        public bool CloseStore { get; set; }
        public bool CustomerCommunication { get; set; }
        public bool AppointOwner { get; set; }
        public bool AppointManager { get; set; }

        public Roles(bool addRemoveProducts, 
                     bool addRemovePurchasing, 
                     bool addRemoveDiscountPolicy, 
                     bool addRemoveStoreManger, 
                     bool closeStore, 
                     bool customerCommunication,
                     bool appointOwner,
                     bool appointManager,
                     bool addRemoveStorePolicy)
        {
            this.AddRemoveProducts = addRemoveProducts;
            this.AddRemovePurchasing = addRemovePurchasing;
            this.AddRemoveDiscountPolicy = addRemoveDiscountPolicy;
            this.AddRemoveStoreManger = addRemoveStoreManger;
            this.CloseStore = closeStore;
            this.CustomerCommunication = customerCommunication;
            this.AppointManager = appointManager;
            this.AppointOwner = appointOwner;
            this.AddRemoveStorePolicy = addRemoveStorePolicy;
        }

        public bool CompareRoles(Roles otherRoles)
        {
            if (!this.AddRemoveProducts && otherRoles.AddRemoveProducts)
                return false;
            if (!this.AddRemovePurchasing && otherRoles.AddRemovePurchasing)
                return false;
            if (!this.AddRemoveDiscountPolicy && otherRoles.AddRemoveDiscountPolicy)
                return false;
            if (!this.AddRemoveStorePolicy && otherRoles.AddRemoveStorePolicy)
                return false;
            if (!this.AddRemoveStoreManger && otherRoles.AddRemoveStoreManger)
                return false;
            if (!this.CloseStore && otherRoles.CloseStore)
                return false;
            if (!this.CustomerCommunication && otherRoles.CustomerCommunication)
                return false;
            if (!this.AppointManager && otherRoles.AppointManager)
                return false;
            if (!this.AppointOwner && otherRoles.AppointOwner)
                return false;
            return true;
        }


        public bool isStoreOwner()
        {
            return  this.AddRemoveProducts &&
                    this.AddRemovePurchasing &&
                    this.AddRemoveDiscountPolicy &&
                    this.AddRemoveStoreManger &&
                    this.CloseStore &&
                    this.CustomerCommunication &&
                    this.AppointManager &&
                    this.AppointOwner;
        }
    }

    public class StoreManager
    {
        private bool storeOwner;
        [Key]
        public int id { get; set; }
        public Store store { get; set; }
        public Roles myRoles { get; set; }
        public LinkedList<StoreManager> subManagers { get; set; }
        public StoreManager father { get; set; }

        public StoreManager(Store store, Roles storeRoles)
        {
            this.store = store;
            this.myRoles = storeRoles;
            this.subManagers = new LinkedList<StoreManager>();
            this.father = null; //change to super father
            this.storeOwner = false;
        }

        /*about roles: the client will choose what roles he wants to give the new
          manager (needs to be like hes and below) */
        public bool CreateNewManager(Member member, Roles roles)
        {
            if (myRoles.isStoreOwner() && myRoles.CompareRoles(roles) && checkNotAManager(member))
            {
                StoreManager newSubStoreManager = new StoreManager(this.store, roles);
                newSubStoreManager.setFather(this);
                subManagers.AddFirst(newSubStoreManager);
                member.addStoreToMe(newSubStoreManager);
                Logger.Log("file", logLevel.INFO, "store:" + store.id + " succesfully add new manager: "+ member.username);
                return true;
            }
            else
            {
                Logger.Log("file", logLevel.INFO, "store:" + store.id + " failed add new manager: " + member.username);
                throw new Exception("this manager try to give more roles than he can");
            }
        }




        private bool checkNotAManager(Member member)
        {
            foreach (StoreManager sm in member.storeManaging)
            {
                if (sm.GetStore().id == this.store.id)
                {
                    return false;
                }
            }

            return true;
        }

        public void setFather(StoreManager father)
        {
            this.father = father;
        }



        public bool removeManager(StoreManager managerToRemove)
        {
            if (managerToRemove.storeOwner)
                throw new Exception("Sorry, you can't remove a partner! this user is a an owner of your store");
            if (subManagers.Contains(managerToRemove))
            {
                recursiveCleanManager(managerToRemove);
                Logger.Log("file", logLevel.INFO, "success remove");
                return subManagers.Remove(managerToRemove);
            } else
            {
                Logger.Log("file", logLevel.INFO, "fail remove");
                throw new Exception("The manager to remove is not below to this manager");
            }
        }

        public void removeAllManagers()
        {
            while(subManagers.Count > 0)
            {
                StoreManager subStoreManager = subManagers.ElementAt(0);
                removeManager(subStoreManager);
                //if (subManagers.Count == 0) break;
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
                    if (subManagers.Count == 0) break;
                }
            }
        }

        public Store GetStore()
        {
            return store;
        }
        public Roles GetRoles()
        {
            return myRoles;
        }

        public StoreManager GetFather()
        {
            return father;
        }

        public void SetStoreOwnerTrue()
        {
            this.storeOwner = true;
        }


        public void SetStoreOwnerFalse()
        {
            this.storeOwner = false;
        }
    }
}
