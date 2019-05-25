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
using WorkshopProject.Communication;

namespace Users
{
    public static class ConnectionStubTemp
    {

        public static PasswordHandler pHandler = new PasswordHandler();
        public static Dictionary<int, Member> members = new Dictionary<int, Member>();
        // <ID, MEMBER>
        public static Dictionary<string, int> mapIDUsermane = new Dictionary<string, int>();
        // <username, ID>

        public static LinkedList<OwnershipRequest> ownershipsRequestList = new LinkedList<OwnershipRequest>();


        public static int memberIDGenerator = 0;

        public static void init()
        {
            registerNewUser("Admin", "Admin", DateTime.Today.AddYears(-120), "all");

        }

        static ConnectionStubTemp()
        {
            init();
        }

        public static void removeMember(Member m)
        {
            if (m.id == 0)
                throw new Exception("don't remove Admin!");
            try
            {
                members.Remove(m.id);
                mapIDUsermane.Remove(m.username);
            }
            catch (Exception ignore)
            {
                throw new Exception("this should not happen, member doesn't exist");
            }
        }

        public static Member getMember(int id)
        {
            try
            {
                return members[id];
            }
            catch (Exception ignore)
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
            catch (Exception ignore)
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
            /*
            if (username == "Admin")
                registerNewUser(username, password, "all", 120);*/
            try
            {
                int ID = mapIDUsermane[username];
                if (pHandler.IdentifyPassword(password, ID))
                    return ID;
            }
            catch (Exception ignore)
            {
                return -1;
            }
            return -1;
        }
        //sign up
        public static void registerNewUser(string username, string password, DateTime birthdate, string country)
        {



            //jonathan rewrite
            sanitizeInput(username, password);
            int id;
            if (mapIDUsermane.TryGetValue(username, out id))
            {
                Logger.Log("file", logLevel.INFO, "user try to register with taken username:" + username);
                throw new Exception("this username is already taken. try somthing else");
            }
            id = getID();
            pHandler.hashPassword(password, id);
            Member newMember;
            if (DateTime.Today < birthdate)
                newMember = new Member(username, id);
            else
                newMember = new Member(username, id, birthdate, country);
            if (username == "Admin" && password == "Admin")
            {
                newMember = new SystemAdmin(username, id, birthdate, country);
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



        public static void createOwnershipRequest(Store store, Member memberThatOpenRequest, Member candidate)
        {
            OwnershipRequest newOwnership = new OwnershipRequest(store,candidate,memberThatOpenRequest);

            foreach(KeyValuePair<int, Member> entry in members)
            {
                Member m = entry.Value;
                if (m.isStoresOwner(store.id))
                {
                    newOwnership.addOwner(m);
                }
            }
            newOwnership.approveOrDisapprovedOwnership(1, memberThatOpenRequest);//first approval of asker
            ownershipsRequestList.AddFirst(newOwnership);//add ownership request to list
            newOwnership.sendRequestsToOwners();//should handle notifications
        }

        public static void deleteOwnershipRequest(OwnershipRequest ownership)
        {
            ownershipsRequestList.Remove(ownership);
        }

    }



    public class OwnershipRequest
    {
        private Store store;
        private Member initiate;
        private Member candidate;
        public static Dictionary<String, int> owners = new Dictionary<String, int>();
        //approved, ownerNames
        private readonly object OwnersLock = new object();
        private int counter = 0;
        private readonly object CounterLock = new object();
        private bool done;
        private readonly object doneLock = new object();

        public OwnershipRequest(Store store, Member candidate, Member initiate)
        {
            this.store = store;
            this.candidate = candidate;
            this.initiate = initiate;
            this.counter = 0;
            this.done = false;
        }

        public void addOwner(Member member)
        {
            owners[member.username] = 0;
            lock (CounterLock)
            {
                counter++;
            }
        }


        // 1 opproved -1 disapproved
        public void approveOrDisapprovedOwnership(int desition, Member member)
        {
            lock(OwnersLock)
            {
                owners[member.username] = desition;
            }
            lock(CounterLock)
            {
                counter--;
                if(isFullfielld())
                {

                    lock (doneLock)
                    {
                        if (!done)
                        {
                            done = true;
                            makeOwner();
                        }
                    }
                }
            }
        }

        private bool isFullfielld()
        {
            lock(CounterLock)
            {
                return counter == 0;
            }
        }

        public void sendRequestsToOwners()
        {

        }

        public void makeOwner()
        {
            Roles roles = initiate.getStoreManagerRoles(store.id);
            initiate.addStoreOwner(candidate.username, roles, store.id);
            ConnectionStubTemp.deleteOwnershipRequest(this);
        }


    }
}
