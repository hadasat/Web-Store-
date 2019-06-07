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
        public static Dictionary<int, OwnershipRequest> ownershipsRequestList = new Dictionary<int, OwnershipRequest>();
        // <ID, ownershipRequest>

        public static int memberIDGenerator = 0;
        public static int ownerShipRequestsIDGenerator = 0;

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
            if (m is SystemAdmin)
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

        public static void removeAdmin(SystemAdmin m)
        {
            
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

        public static void addMemberJustForExternalUsage(Member m)
        {

            try
            {
                members.Add(m.id, m);
                mapIDUsermane.Add(m.username, m.id);
            }
            catch (Exception ignore)
            {
                throw new Exception("this should not happen, member couldn't be added");
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
                Logger.Log("event", logLevel.INFO, "user try to register with taken username:" + username);
                throw new Exception("this username is already taken. try somthing else");
            }
            id = getID();
            pHandler.hashPassword(password, id);
            Member newMember;
            if (DateTime.Today < birthdate)
                newMember = new Member(username, id);
            else
                newMember = new Member(username, id, birthdate, country);
            /*
            if (username == "Admin" && password == "Admin")
            {
                newMember = new SystemAdmin(username, id, birthdate, country);
                Logger.Log("event", logLevel.INFO, "Admin has logged in");
            }*/

            if (password == "Admin")
            {
                newMember = new SystemAdmin(username, id, birthdate, country);
                Logger.Log("event", logLevel.INFO, "Admin has logged in");
            }
            members[id] = newMember;
            mapIDUsermane[username] = id;
            Logger.Log("event", logLevel.INFO, "user:" + username + " succesfully registered");
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



        public static int createOwnershipRequest(Store store, Member memberThatOpenRequest, Member candidate)
        {

            int requestID = ownerShipRequestsIDGenerator++;
            OwnershipRequest newOwnership = new OwnershipRequest(requestID, store, candidate, memberThatOpenRequest);
            foreach (KeyValuePair<int, Member> entry in members)
            {
                Member m = entry.Value;
                if (m.isStoresOwner(store.id))
                {
                    newOwnership.addOwner(m);
                }
            }
            newOwnership.sendRequestsToOwners(store, memberThatOpenRequest.id, candidate.username, requestID);//should handle notifications
            newOwnership.approveOrDisapprovedOwnership(1, memberThatOpenRequest);//first approval of asker
            ownershipsRequestList[requestID] = (newOwnership);//add ownership request to list  
            return requestID;
        }

        public static void deleteOwnershipRequest(OwnershipRequest ownership)
        {
            ownershipsRequestList.Remove(ownership.getID());
        }

        public static OwnershipRequest getOwnershipRequest(int id)
        {
            try
            {
                return ownershipsRequestList[id];
            } catch (Exception ex)
            {
                ///somtihng went wrong with id's
                throw ex;
            }
        }

        public static int getNumOfOwners(Store store)
        {
            int ret = 0;
            foreach (KeyValuePair<int, Member> entry in members)
            {
                Member m = entry.Value;
                if (m.isStoresOwner(store.id))
                {
                    ret++;
                }
            }
            return ret;
        }


    }



    public class OwnershipRequest
    {
        private int ID;
        private Store store;
        private Member initiate;
        private Member candidate;
        public static Dictionary<String, int> owners = new Dictionary<String, int>();
        //<ownerNames, approved>
        private readonly object OwnersLock = new object();
        private int counter = 0;
        private readonly object CounterLock = new object();
        private bool done;
        private readonly object doneLock = new object();

        public OwnershipRequest(int id, Store store, Member candidate, Member initiate)
        {
            this.ID = id;
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
        public void approveOrDisapprovedOwnership(int decition, Member member)
        {
            lock(OwnersLock)
            {
                owners[member.username] = decition;
            }
            lock(CounterLock)
            {
                counter--;
            }
            if (isFullfielld())
            {
                lock (doneLock)
                {
                    if (!done && checkIfApproved())
                    {
                        done = true;
                        makeOwner();
                    } else
                    {
                        //needs to decide if somthing happens in case of
                        //unapproval
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

        private bool checkIfApproved()
        {
            bool ans = true;
            foreach (KeyValuePair<String, int> entry in owners)
            {
                ans = ans & (entry.Value == 1);
            }
            return ans;
        }

        public void sendRequestsToOwners(Store store,int creatorId, string candidateName,int requestId)
        {

            lock (OwnersLock)
            {
                foreach (KeyValuePair<String, int> entry in owners)
                {
                    Member currMember = ConnectionStubTemp.getMember(entry.Value);
                    if (currMember.id != creatorId && currMember.username!=candidateName)
                    {
                        currMember.addMessage("Do you agree adding " + candidateName + " as a co - owner to the store " + store.name + "?", Notification.NotificationType.CREATE_OWNER, requestId);
                    }
                }
            }

            /*
            List<Member> members = ConnectionStubTemp.members.Values.ToList();
            foreach (Member currMember in members)
            {
                if (currMember.isStoresOwner(store.id) && currMember.id != creatorId)
                {
                    currMember.addMessage("Do you agree adding " + candidateName+ " as a co-owner to the store "+store.name ,
                        Notification.NotificationType.CREATE_OWNER,requestId);
                }
            }*/

            //if (owners.Count != 0)
            //{
            //    // message:
            //    //store <name / id>
            //    //owner that made the qequest <username>
            //    //member candidate <username>



            //    //send to all members in owners.
            //    //I SAVED USERNAMES - U CAN GET THE MEMBER ITSELF WITH THIS LINE
            //    ////ConnectionStubTemp.getMember(username);

            //}
        }

        public void makeOwner()
        {
            Roles roles = initiate.getStoreManagerRoles(store.id);
            initiate.makeStoreOwner(candidate.username, roles, store.id);
            ConnectionStubTemp.deleteOwnershipRequest(this);
        }

        public int getID()
        {
            return ID;
        }

    }
}
