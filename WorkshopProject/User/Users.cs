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

        /*** START - USER FUNCTIONS ***/

        public static bool identifyUser(string username, string password)
        {
            pHandler.hashPassword(password);
            return true;
        }

        public static bool registerNewUser(string username, string password)
        {
            pHandler.hashPassword(password);
            return true;
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
                return ConnectionStubTemp.getMember(username);
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
            return this.storeManaging.Count == 0;
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
                //talk with ofir!!!
                return ConnectionStubTemp.removeUser(userName, this);
            }
            else
            {
                return ConnectionStubTemp.removeUser(userName, this);
            }
        }
    }
}
