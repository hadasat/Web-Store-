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
        public static List<Member> membersOnline = new List<Member>();

        public static void addMember(Member m)
        {
            membersOnline.Add(m);
        }



        /*** START - USER FUNCTIONS ***/

        private static string getID()
        {
            return "A123";
        }
        //sign in
        public static bool identifyUser(string username, string password)
        {
            string ID = getID();
            return pHandler.IdentifyPassword(password, ID);
        }
        //sign up
        public static void registerNewUser(string username, string password)
        {
            string ID = getID();
            pHandler.hashPassword(password, ID);
        }

        
        public static Member getMember(string username)
        {
            string ID = "Robot123";
            if (isAnAdmin(username))
                return new SystemAdmin(username, ID);
            else
                return new Member(username, ID);
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
                Roles firstStoreRoles = new Roles(false, false, false, false, false, false);
                Store store = new Store();
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



    public class User
    {
        public ShoppingBasket shoppingBasket;//is it this way or the opposite?


        public User()
        {
            this.shoppingBasket = new ShoppingBasket();
        }



        /*** SERVICE LAYER FUNCTIONS***/

        public Member loginMember(string username, string password)
        {
            bool tryToRegister = ConnectionStubTemp.identifyUser(username, password);
            if (tryToRegister)
            {
                Member m =  ConnectionStubTemp.getMember(username);
                ConnectionStubTemp.addMember(m);
                return m;
            }
            else
                return null;
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
        public string ID; //why do we need id?
        public string username;
        public LinkedList<StoreManager> storeManaging;
        

        public Member(string username, string ID)
        {
            this.ID = ID;
            this.username = username;
            this.storeManaging = ConnectionStubTemp.loadStoreManaging(ID);
            this.shoppingBasket = ConnectionStubTemp.loadShoppingBasket(ID);
            
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
            Roles storeOwner = new Roles(true, true, true, true, true, true);
            StoreManager storeOwnerManager = new StoreManager(store, storeOwner);
            storeManaging.AddFirst(storeOwnerManager);
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
                    if (sm.GetStore() == store)
                    {
                        return sm.GetRoles();
                    }
                }
            }
            
            return null;
        }
    }




    public class SystemAdmin : Member
    {
        public SystemAdmin(string username, string ID) : base(username, ID) { }

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
                        //store.close();    //OFIR
                    }
                }
                return ConnectionStubTemp.removeUser(userName, this);
            }
            else
            {
                return ConnectionStubTemp.removeUser(userName, this);
            }
        }
    }
}
