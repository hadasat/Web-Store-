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
using System.ComponentModel.DataAnnotations;

namespace Users
{
    public static class ConnectionStubTemp
    {
        //public static bool useStub = false;
        public static Repo repo = new Repo();
        
        public static PasswordHandler pHandler = new PasswordHandler();




        //public static Dictionary<int, Member> members = new Dictionary<int, Member>();
        // <ID, MEMBER>
        //public static Dictionary<string, int> mapIDUsermane = new Dictionary<string, int>();
        // <username, ID>
        //public static Dictionary<int, OwnershipRequest> ownershipsRequestList = new Dictionary<int, OwnershipRequest>();
        // <ID, ownershipRequest>

        //public static int memberIDGenerator = 0;
        //public static int ownerShipRequestsIDGenerator = 0;

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
            if (m is SystemAdmin)
                throw new Exception("don't remove Admin!");
            try
            {
                pHandler.RemovePasswordFromMember(m.id);
                Remove(m.id);
                //members.Remove(m.id);
                //mapIDUsermane.Remove(m.username);
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
                Remove(m.id);
                //mapIDUsermane.Remove(m.username);
            }
            catch (Exception ignore)
            {
                throw new Exception("this should not happen, member doesn't exist");
            }
        }

        public static void addMemberJustForExternalUsage(Member m, Password.Password pass)
        {

            try
            {
                
                AddMember(m);
                pass.memberId = m.id;
                pHandler.Add(pass);
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
                throw new Exception("this should not happen, member doesn't exist");
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
            OwnershipRequest newOwnership = new OwnershipRequest(store, candidate, memberThatOpenRequest);
            //foreach (KeyValuePair<int, Member> entry in members)
            foreach (Member m in GetMembers())
            {
                //Member m = entry.Value;
                if (m.isStoresOwner(store.id))
                {
                    newOwnership.addOwner(m);
                }
            }
            AddOwnershipRequest(newOwnership);//add ownership request to list 
            newOwnership.sendRequestsToOwners(store, memberThatOpenRequest.id, candidate.username, newOwnership.getID());//should handle notifications
            newOwnership.approveOrDisapprovedOwnership(1, memberThatOpenRequest);//first approval of asker
            return newOwnership.getID();
        }

        public static void deleteOwnershipRequest(OwnershipRequest ownership)
        {
            RemoveOwnershipRequest(ownership.ID);
        }

        public static OwnershipRequest GetOwnershipRequest(int id)
        {
            if (useStub())
            {
                return getOwnershipRequestDbStub().Get(id);
            }

            return (OwnershipRequest)repo.Get<OwnershipRequest>(id);
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
            if (useStub())
            {
                return getMemberDbStub().GetList();
            }
            return repo.GetList<Member>();
        }

        public static Member GetMemberByName(string username)
        {
            foreach (Member m in GetMembers())
            {
                if (m.username != null && string.Compare(m.username, username) == 0)
                {
                    return m;
                }
            }
            return null;
        }

        public static void AddMember(Member member)
        {
            if (useStub())
            {
                getMemberDbStub().Add(member);
                return;
            }
            repo.Add<Member>(member);
        }

        public static Member GetMemberById(int id)
        {
            if (useStub())
            {
                return getMemberDbStub().Get(id);
            }
            return (Member) repo.Get<Member>(id);
        }

        public static void Remove(int id)
        {
            if (useStub())
            {
                getMemberDbStub().Remove(id);
                return;
            }
            repo.Remove<Member>(GetMemberById(id));
        }

        public static void Update(Member member)
        {
            if (useStub())
            {
                return;
            }
            repo.Update<Member>(member);
        }

        public static void Clear()
        {
            if (useStub())
            {
                getMemberDbStub().Delete();
                getOwnershipRequestDbStub().Delete();
            }
            //TODO: repo in the future
        }

        private static bool useStub()
        {
            return DataAccessDriver.UseStub;
        }

        private static DbListStub<Member> getMemberDbStub()
        {
            return DataAccessDriver.Members;
        }

        private static DbListStub<OwnershipRequest> getOwnershipRequestDbStub()
        {
            return DataAccessDriver.OwnershipRequests;
        }

        public static void AddOwnershipRequest(OwnershipRequest entity)
        {
            if (useStub())
            {
                getOwnershipRequestDbStub().Add(entity);
                return;
            }
            repo.Add<OwnershipRequest>(entity);
        }

        public static void RemoveOwnershipRequest(int id)
        {
            if (useStub())
            {
                getOwnershipRequestDbStub().Remove(id);
                return;
            }
            repo.Remove<OwnershipRequest>(GetOwnershipRequest(id));
        }

        public static void UpdateOwnershipRequest(OwnershipRequest entity)
        {
            if (useStub())
            {
                return;
            }
            repo.Update<OwnershipRequest>(entity);
        }

    }



    public class OwnershipRequest : IEntity
    {
        [Key]
        public int ID { get; set; }
        public virtual Store store { get; set; }
        public virtual Member initiate { get; set; }
        public virtual Member candidate { get; set; }
        //public static Dictionary<String, int> owners = new Dictionary<String, int>();
        //<ownerNames, approved>
        public LinkedList<Decision> owners = new LinkedList<Decision>();
        public readonly object OwnersLock;
        public int counter = 0;
        public readonly object CounterLock;
        public bool done;
        public readonly object doneLock;

        public OwnershipRequest()
        {
            OwnersLock = new object();
            CounterLock = new object();
            doneLock = new object();
        }

         public OwnershipRequest(Store store, Member candidate, Member initiate)
        {
            this.store = store;
            this.candidate = candidate;
            this.initiate = initiate;
            this.counter = 0;
            this.done = false;
            OwnersLock = new object();
            CounterLock = new object();
            doneLock = new object();
        }

        public void addOwner(Member member)
        {
            owners.AddFirst(new Decision(member.username));
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
                foreach(Decision d in owners)
                {
                    if (d.username == member.username)
                    {
                        d.setDecition(decition);
                        break;
                    }
                }
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
            foreach (Decision d in owners)
            {
                ans = ans & (d.getDecition() == 1);
            }
            return ans;
        }

        public void sendRequestsToOwners(Store store,int creatorId, string candidateName,int requestId)
        {

            lock (OwnersLock)
            {
                foreach (Decision d in owners)
                {
                    Member currMember = ConnectionStubTemp.getMember(d.username);
                    if (currMember.id != creatorId && currMember.username!=candidateName)
                    {
                        currMember.addMessage("Do you agree adding " + candidateName + " as a co - owner to the store " + store.name + "?", Notification.NotificationType.CREATE_OWNER, requestId);
                    }
                }
            }

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

        public override int GetKey()
        {
            return getID();
        }

        public override void SetKey(int key)
        {
            ID = key;
        }

        public override void LoadMe()
        {
            store.LoadMe();
            initiate.LoadMe();
            candidate.LoadMe();
        }
    }

    public class Decision : IEntity
    {
        [Key]
        public int id { get; set; }
        public String username { get; set; }
        public int desicion { get; set; }

        public Decision() {
        }

        public Decision(string username)
        {
            this.desicion = 0; ;
            this.username = username;
        }

        public void setDecition(int dec)
        {
            this.desicion = dec;
        }

        public int getDecition()
        {
            return this.desicion;
        }

        public override int GetKey()
        {
            return id;
        }

        public override void SetKey(int key)
        {
            id = key;
        }

        public override void LoadMe()
        {
            //do nothing
        }
    }
}
