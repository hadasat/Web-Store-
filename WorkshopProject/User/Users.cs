using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password;
using Managment;
using WorkshopProject;
using Shopping;
using WorkshopProject.Log;

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
            try
            {
                members.Remove(m.ID);
                mapIDUsermane.Remove(m.username);
            }
            catch (Exception ex)
            {
                throw new Exception("this should noy happen, member doesn't exist");
            }
        }

        public static Member getMember(int id)
        {
            try
            {
                return members[id];
            }
            catch (Exception ex)
            {
                throw new Exception("this should not happen, member doesn't exist");
            }
        }

        public static Member getMember(string username)
        {
            try
            {
                return members[(mapIDUsermane[username])];
            }
            catch (Exception ex)
            {
                throw new Exception("this should noy happen, member doesn't exist");
            }
        }

        /*** START - USER FUNCTIONS ***/

        private static int getID()
        {
            return memberIDGenerator++;
        }
        //sign in
        public static int identifyUser(string username, string password)
        {
            //very tmp until database! TODO: change
            if (username == "Admin")
                registerNewUser(username, password, "all", 120);
            try
            {
                int ID = mapIDUsermane[username];
                if (pHandler.IdentifyPassword(password, ID))
                    return ID;
            } catch(Exception ex)
            {
                return -1;
            }
            return -1;
        }
        //sign up
        public static void registerNewUser(string username, string password, string country, int age)
        {
            //Member m = null;
            //int ID = getID();
            //try
            //{
            //    m = members[ID];
            //} catch(Exception ex) { }
            //if(m != null)
            //{
            //    throw new Exception("this username is already taken. try somthing else");
            //}
            //pHandler.hashPassword(password, ID);
            //Member newMember = new Member(username,ID);
            //members[ID] = newMember;
            //mapIDUsermane[username]=ID;


            //jonathan rewrite
            sanitizeInput(username, password);
            int id;
            if(mapIDUsermane.TryGetValue(username, out id)) {
                Logger.Log("file", logLevel.INFO, "user try to register with taken username:" + username);
                throw new Exception("this username is already taken. try somthing else");
            }
            id = getID();
            pHandler.hashPassword(password, id);
            Member newMember;
            if (age < 0 )
                newMember = new Member(username, id);
            else
                newMember = new Member(username, id, country, age);
            if (username == "Admin" && password == "Admin")
            {
                newMember = new SystemAdmin(username, id);
                Logger.Log("file", logLevel.INFO, "Admin has logged in");
            }
            members[id] = newMember;
            mapIDUsermane[username] = id;
            Logger.Log("file", logLevel.INFO, "user:" + username + " succesfully registered");
        }




        private static bool sanitizeInput(string username, string password)
        {
            return sanitizeUsername(username) &&
                sanitizePassword(password);
        }

        private static bool sanitizeUsername(string username)
        {
            if (username.Equals(""))
            {
                throw new Exception("username contains illegal charachters");
            }
            string[] illegalChars = { ";" };
            foreach (string c in illegalChars)
            {
                if (username.Contains(c))
                {
                    throw new Exception("username contains illegal charachters");
                    //return false;
                }
            }
            return true;
        }

        private static bool sanitizePassword(string password)
        {
            if (password.Equals(""))
            {
                throw new Exception("password can't be empty");
            }
            return true;
        }


        /*** END - USER FUNCTIONS ***/

        /****************************************************************/

        /*** START - MEMBER FUNCTIONS ***/

        public static void logout(string username)
        {
            
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


        public static bool isAnAdmin(string username)
        {
            return true;
        }


        /*** END - SYSTEM ADMIN FUNCTIONS ***/



    }



    public class User : Permissions
    {
        public ShoppingBasket shoppingBasket;


        public User()
        {
            this.shoppingBasket = new ShoppingBasket();
            Logger.Log("file", logLevel.INFO, "New user been created");
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
            {
                //don't log password!
                Logger.Log("file", logLevel.INFO, "user: " + username + "tried to log in and failed");
                throw new Exception("username or password does not correct");
            }
            Logger.Log("file", logLevel.INFO, "user: " + username + "log in and succses");
            return ConnectionStubTemp.getMember(ID);
           
        }

        public void registerNewUser(string username, string password)
        {
            ConnectionStubTemp.registerNewUser(username, password, "", -1);
        }

        public void registerNewUser(string username, string password, string countery, int age)
        {
            ConnectionStubTemp.registerNewUser(username, password, countery, age);
        }

        /****************************************************************/

    }




    public class Member : User
    {
        public int ID; //why do we need id?
        public string username;
        public LinkedList<StoreManager> storeManaging;
        private string country;
        private int age;
        
        
        public Member(string username, int ID) : base()//Register
        {
            this.ID = ID;
            this.username = username;
            this.storeManaging = new LinkedList<StoreManager>();
            this.country = "none";
            this.age = -1;
        }

        public Member(string username, int ID, string country, int age) : base()//Register
        {
            this.ID = ID;
            this.username = username;
            this.storeManaging = new LinkedList<StoreManager>();
            this.country = country;
            this.age = age;
        }

        /*** SERVICE LAYER FUNCTIONS***/
        public void logOut()
        {
            Logger.Log("file", logLevel.INFO, "user: " + this.username + "log out and succses");
            ConnectionStubTemp.logout(username);
        }

        /************************/

        /*** This function is the function that create Store Owner - STORE USE THIS IN THE CONSTRUCTOR ***/
        public void addStore(Store store)
        {
            Roles storeOwner = new Roles(true, true, true, true, true, true, true, true);
            StoreManager storeOwnerManager = new StoreManager(store, storeOwner);
            storeManaging.AddFirst(storeOwnerManager);
            Logger.Log("file", logLevel.INFO, "user: " + this.username + "created succesfully new store: " + store.Id);
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
            } else if (!store.isActive)
            {
                throw new Exception("you cant close this store, it closed already");
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
            Logger.Log("file", logLevel.INFO, "user: " + this.username + "closed succesfully store: " + store.Id);
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

            throw new Exception("you dont own or manage this store");
        }

        private Store GetStore(int StoreID)
        {
            if (isStoresManagers())
            {
                foreach (StoreManager sm in storeManaging)
                {
                    if (sm.GetStore().Id == StoreID)
                    {
                        return sm.GetStore();
                    }
                }
            }

            throw new Exception("you dont own or manage this store");
        }

        public bool addManager(string username, Roles role, int StoreID)
        {
            Store store = GetStore(StoreID);
            StoreManager myStoreRoles = getStoreManagerOb(store);
            return myStoreRoles.CreateNewManager(ConnectionStubTemp.getMember(username), role);
        }


        public bool addManager(string username,Roles role,Store store)
        {

            StoreManager myStoreRoles = getStoreManagerOb(store);
            return myStoreRoles.CreateNewManager(ConnectionStubTemp.getMember(username), role);
        }

        //TODO : is there  need for remove manager option?
        public bool removeManager(string username, Store store)
        {
            Logger.Log("file", logLevel.INFO, "user: " + this.username + "try to remove user: "+ username);
            StoreManager myStoreRoles = getStoreManagerOb(store);
            Member memberToRemove = ConnectionStubTemp.getMember(username);
            bool res = myStoreRoles.removeManager(memberToRemove.getStoreManagerOb(store));
            memberToRemove.RemoveStoreFromMe(memberToRemove.getStoreManagerOb(store));
            return res;
        }

        public bool removeManager(string username, int StoreID)
        {
            Logger.Log("file", logLevel.INFO, "user: " + this.username + "try to remove user: " + username);
            Store store = GetStore(StoreID);
            StoreManager myStoreRoles = getStoreManagerOb(store);
            Member memberToRemove = ConnectionStubTemp.getMember(username);
            bool res = myStoreRoles.removeManager(memberToRemove.getStoreManagerOb(store));
            memberToRemove.RemoveStoreFromMe(memberToRemove.getStoreManagerOb(store));
            return res;
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

        public string getCountry ()
        {
            return this.country;
        }

        public int getAge()
        {
            return this.age;
        }
        
    }




    public class SystemAdmin : Member
    {
        public SystemAdmin(string username, int ID) : base(username, ID) { }

        public SystemAdmin(string username, int ID, string country, int age) : base(username, ID, country, age) { }

        public bool RemoveUser(string userName)
        {
            Member member;
            try
            {
                member = ConnectionStubTemp.getMember(userName);
            }
            catch (Exception ex)
            {
                Logger.Log("file", logLevel.INFO, "Admin fail removed user: " + userName + " user doen't exist");
                return false;
            }
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
                Logger.Log("file", logLevel.INFO, "Admin succesfully removed user: " + userName);
                return ConnectionStubTemp.removeUser(userName, this);
            }
            else
            {
                Logger.Log("file", logLevel.INFO, "Admin succesfully removed user: " + userName);
                ConnectionStubTemp.removeMember(member);
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
