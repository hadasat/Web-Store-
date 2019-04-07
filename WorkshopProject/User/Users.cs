using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password;
using Managment;
using WorkshopProject;



namespace Users
{

    public static class ConnectionStubTemp
    {

        public static PasswordHandler pHandler = new PasswordHandler();
        public static Dictionary<int, Member> members = new Dictionary<int, Member>();
        // <ID, MEMBER>
        public static Dictionary<string, int> mapIDUsermane = new Dictionary<string, int>();
        // <username, ID>

        public static int memberIDGenerator = 0;

       

        public static void removeMember(Member m)
        {

            members.Remove(m.ID);
            mapIDUsermane.Remove(m.username);
        }

        /*** START - USER FUNCTIONS ***/

        private static int getID()
        {
            return memberIDGenerator++;
        }
        //sign in
        public static int identifyUser(string username, string password)
        {
            int ID = mapIDUsermane[username];
            if(pHandler.IdentifyPassword(password, ID))
                return ID;
            return -1;
        }
        //sign up
        public static void registerNewUser(string username, string password)
        {
            int ID = getID();
            pHandler.hashPassword(password, ID);
            Member newMember = new Member(username,ID);
            members[ID] = newMember;
            mapIDUsermane[username]=ID;
        }

        
        public static Member getMember(int id)
        {
            try
            {
                return members[id];
            }
            catch (Exception ex)
            {
                throw new Exception("this should noy happen, user doesn't exist");
            }
        }

        public static Member getMember(string username)
        {
            return members[(mapIDUsermane[username])];
        }

        public static bool isAnAdmin(string username)
        {
            return true;
        }


        /*** END - USER FUNCTIONS ***/

        /****************************************************************/

        /*** START - MEMBER FUNCTIONS ***/

        public static void logout(string username)
        {
            
        }

        public static ShoppingBasket loadShoppingBasket(string ID)
        {
            return new ShoppingBasket();
        }

        public static LinkedList<StoreManager> loadStoreManaging(string ID)
        {
            if (ID != "0")
            {
                Roles firstStoreRoles = new Roles(false, false, false, false, false, false, false,false);
                Store store = new Store(0,"stubStore",3,true);
                StoreManager st = new StoreManager(store, firstStoreRoles);
                LinkedList<StoreManager> storesManaging = new LinkedList<StoreManager>();
                storesManaging.AddFirst(st);
                return storesManaging;
            }
            else
            {
                return null;
            }
        }

        /*** END - MEMBER FUNCTIONS ***/


        /****************************************************************/

        /*** START - SYSTEM ADMIN FUNCTIONS ***/

        public static bool removeUser(string username, Member sa)
        {
            if (sa is SystemAdmin)
                return true;
            return false;
        }


        /*** END - SYSTEM ADMIN FUNCTIONS ***/



    }



    public class User : Permissions
    {
        public ShoppingBasket shoppingBasket;//is it this way or the opposite?


        public User()
        {
            this.shoppingBasket = new ShoppingBasket();
        }

        public virtual bool hasAddRemoveDiscountPermission(Store store)
        {
            return false;
        }

        public virtual bool hasAddRemoveProductsPermission(Store store)
        {
            return false;
        }

        public virtual bool hasAddRemovePurchasingPermission(Store store)
        {
            return false;
        }

        public virtual bool hasAddRemoveStoreManagerPermission(Store store)
        {
            return false;
        }



        /*** SERVICE LAYER FUNCTIONS***/

        public Member loginMember(string username, string password)
        {
            //TODO:: load shopping basket


            int ID = ConnectionStubTemp.identifyUser(username, password);
            if (ID == -1)
                throw new Exception("username or password does not correct");
            
            return  ConnectionStubTemp.getMember(ID);
            
           
        }

        public void registerNewUser(string username, string password)
        {
            ConnectionStubTemp.registerNewUser(username, password);
            //need to deside whats happen in this senario
        }

        /****************************************************************/

    }




    public class Member : User
    {
        public int ID; //why do we need id?
        public string username;
        public LinkedList<StoreManager> storeManaging;
        
        
        public Member(string username, int ID) : base()//Register
        {
            this.ID = ID;
            this.username = username;
            this.storeManaging = new LinkedList<StoreManager>();
        }

        /*** SERVICE LAYER FUNCTIONS***/
        public void logOut()
        {
            ConnectionStubTemp.logout(username);
        }

        /************************/

        /*** This function is the function that create Store Owner - STORE USE THIS IN THE CONSTRUCTOR ***/
        public void addStore(Store store)
        {
            Roles storeOwner = new Roles(true, true, true, true, true, true, true, true);
            StoreManager storeOwnerManager = new StoreManager(store, storeOwner);
            storeManaging.AddFirst(storeOwnerManager);
        }

        public void addStoreToMe(StoreManager storeManager)
        {
            storeManaging.AddFirst(storeManager);
        }

        public void RemoveStoreFromMe(StoreManager storeManager)
        {
            storeManaging.Remove(storeManager);
        }

        public void closeStore(Store store)
        {
            Roles myRoles = getStoreManagerRoles(store);
            if (!myRoles.isStoreOwner())
            {
                throw new Exception("you cant close this store");
            }
            StoreManager thisStoreManager;
            foreach (StoreManager sm in storeManaging)
            {
                if (sm.GetStore() == store)
                {
                    thisStoreManager = sm;
                    thisStoreManager.removeAllManagers();
                    storeManaging.Remove(sm);
                    break;
                }
            }
        }

        public bool isStoresManagers()
        {
            return this.storeManaging.Count != 0;
        }
        /*input: store
         output: if this user is a store manager/ owner of the store return hes roles
                   else return null.
                 note = for store owner there is a function isStoreOwner to use */
        public Roles getStoreManagerRoles(Store store)
        {
            if (isStoresManagers())
            {
                foreach(StoreManager sm in storeManaging)
                {
                    if (sm.GetStore().Id == store.Id)
                    {
                        return sm.GetRoles();
                    }
                }
            }
            
            return null;
        }

        public StoreManager getStoreManagerOb(Store store)
        {
            if (isStoresManagers())
            {
                foreach (StoreManager sm in storeManaging)
                {
                    if (sm.GetStore().Id == store.Id)
                    {
                        return sm;
                    }
                }
            }

            return null;
        }

        public bool addManager(string username,Roles role,Store store)
        {
            StoreManager myStoreRoles = getStoreManagerOb(store);
            return myStoreRoles.CreateNewManager(ConnectionStubTemp.getMember(username), role);
        }

        //TODO : is there  need for remove manager option?
        public bool removeManager(string username, Store store)
        {
            StoreManager myStoreRoles = getStoreManagerOb(store);
            Member memberToRemove = ConnectionStubTemp.getMember(username);
            memberToRemove.RemoveStoreFromMe(memberToRemove.getStoreManagerOb(store));
            return myStoreRoles.removeManager(memberToRemove.getStoreManagerOb(store));
        }

        public override bool hasAddRemoveDiscountPermission(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemoveProducts;
        }

        public override bool hasAddRemoveProductsPermission(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemoveDiscountPolicy;
        }

        public override bool hasAddRemovePurchasingPermission(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemovePurchasing;
        }

        public override bool hasAddRemoveStoreManagerPermission(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemoveStoreManger;
        }

        
    }




    public class SystemAdmin : Member
    {
        public SystemAdmin(string username, int ID) : base(username, ID) { }

        public bool RemoveUser(string userName)
        {

            Member member = ConnectionStubTemp.getMember(userName);
            if (member.isStoresManagers())
            {
                foreach (StoreManager st in storeManaging)
                {
                    if (st.GetFather() == null)///change to super father
                    {
                        Store store = st.GetStore();
                        WorkShop.closeStore(store.Id, member);
                    }
                    else
                    {
                        StoreManager father = st.GetFather();
                        father.removeManager(st);
                    }
                }
                ConnectionStubTemp.removeMember(member);
                return ConnectionStubTemp.removeUser(userName, this);
            }
            else
            {
                return ConnectionStubTemp.removeUser(userName, this);
            }
        }
    }


    interface Permissions
    {
        bool hasAddRemoveProductsPermission(Store store);

        bool hasAddRemoveDiscountPermission(Store store);

        bool hasAddRemovePurchasingPermission(Store store);

        bool hasAddRemoveStoreManagerPermission(Store store);
    }
}
