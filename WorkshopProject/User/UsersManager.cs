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
using WorkshopProject.DataAccessLayer;

namespace Users
{
    public static class ConnectionStubTemp
    {
        public static Repo repo = new Repo();
        public static PasswordHandler pHandler = new PasswordHandler();
        //public static Dictionary<int, Member> members = new Dictionary<int, Member>();
        // <ID, MEMBER>
        //public static Dictionary<string, int> mapIDUsermane = new Dictionary<string, int>();
        // <username, ID>
        public static Dictionary<int, OwnershipRequest> ownershipsRequestList = new Dictionary<int, OwnershipRequest>();
        // <ID, ownershipRequest>

        //public static int memberIDGenerator = 0;
        public static int ownerShipRequestsIDGenerator = 0;

        public static void init()
        {
            try
            {
                registerNewUser("Admin", "Admin", DateTime.Today.AddYears(-120), "all");
            }
            catch(Exception e)
            {
                //jonathan: i guess there already is an Admin
            }
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
                Remove(m.id);
                //members.Remove(m.id);
                //mapIDUsermane.Remove(m.username);
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
                AddMember(m);
                //members.Add(m.id, m);
                //mapIDUsermane.Add(m.username, m.id);
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
                //return members[id];
                return GetMemberById(id);
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
                return GetMemberByName(username);
                //return members[(mapIDUsermane[username])];
            }
            catch (Exception ignore)
            {
                throw new Exception("this should noy happen, member doesn't exist");
            }
        }

        /*** START - USER FUNCTIONS ***/

        //private static int getID()
        //{
        //    return memberIDGenerator++;
        //}
        //sign in
        public static int identifyUser(string username, string password)
        {
            //very tmp until database! TODO: change
            /*
            if (username == "Admin")
                registerNewUser(username, password, "all", 120);*/
            try
            {
                //int ID = mapIDUsermane[username];
                int ID = GetMemberByName(username).id;
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
        public static Member registerNewUser(string username, string password, DateTime birthdate, string country)
        {
            //jonathan rewrite
            sanitizeInput(username, password);
            if (GetMemberByName(username) != null)
            {
                Logger.Log("event", logLevel.INFO, "user try to register with taken username:" + username);
                throw new Exception("this username is already taken. try somthing else");
            }

            //int id;
            //if (mapIDUsermane.TryGetValue(username, out id))
            //{
            //    Logger.Log("event", logLevel.INFO, "user try to register with taken username:" + username);
            //    throw new Exception("this username is already taken. try somthing else");
            //}
            //id = getID();
            
            Member newMember;
            if (DateTime.Today < birthdate)
                newMember = new Member(username);
            else
                newMember = new Member(username, birthdate, country);
            /*
            if (username == "Admin" && password == "Admin")
            {
                newMember = new SystemAdmin(username, birthdate, country);
                Logger.Log("event", logLevel.INFO, "Admin has logged in");
            }*/

            if (password == "Admin")
            {
                newMember = new SystemAdmin(username, birthdate, country);
                Logger.Log("event", logLevel.INFO, "Admin has logged in");
            }


            //members[id] = newMember;
            //mapIDUsermane[username] = id;
            AddMember(newMember);
            pHandler.hashPassword(password, newMember.id);
            Logger.Log("event", logLevel.INFO, "user:" + username + " succesfully registered");
            return newMember;
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
            //foreach (KeyValuePair<int, Member> entry in members)
            foreach (Member m in GetMembers())
            {
                //Member m = entry.Value;
                if (m.isStoresOwner(store.id))
                {
                    newOwnership.addOwner(m);
                }
            }
            newOwnership.approveOrDisapprovedOwnership(1, memberThatOpenRequest);//first approval of asker
            ownershipsRequestList[requestID] = (newOwnership);//add ownership request to list
            newOwnership.sendRequestsToOwners(store,memberThatOpenRequest.id,candidate.username,requestID);//should handle notifications
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
            //foreach (KeyValuePair<int, Member> entry in members)
            foreach(Member m in GetMembers())
            {
                //Member m = entry.Value;
                if (m.isStoresOwner(store.id))
                {
                    ret++;
                }
            }
            return ret;
        }


        public static List<Member> GetMembers()
        {
            return repo.GetList<Member>();
        }

        public static Member GetMemberByName(string username)
        {
            foreach (Member m in GetMembers())
            {
                if (m.username != null && m.username.Equals(username))
                {
                    return m;
                }
            }
            return null;
        }

        public static void AddMember(Member member)
        {
            repo.Add<Member>(member);
        }

        public static Member GetMemberById(int id)
        {
            return (Member) repo.Get<Member>(id);
        }

        public static void Remove(int id)
        {
            repo.Remove<Member>(GetMemberById(id));
        }

        public static void Update(Member member)
        {
            repo.Update<Member>(member);
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
                    //currMember.addMessage("addManagerConfirmation-Do you agree adding " + candidateName + " as a co-owner to the store " + store.name);
                    currMember.addMessage("Do you agree adding " + candidateName + " as a co - owner to the store " + store.name + "?", Notification.NotificationType.CREATE_OWNER, requestId);
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
