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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TansactionsNameSpace;
using WorkshopProject.DataAccessLayer;

namespace Users
{
    public class Member : User, IObserverSubject
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        [Include]
        public virtual LinkedList<StoreManager> storeManaging { get; set; }
        [Column(TypeName = "DateTime2")]
        public virtual DateTime birthdate { get; set; }
        public string country { get; set; }
        [Include]
        public virtual List<Notification> notifications { get; set; }
        [NotMapped]
        public HashSet<IObserver> observers { get; set; }
        [NotMapped]
        public Object notificationLock { get; set; }
        //public List<Transaction> purchasesHistory { get; set; }


        public Member() : base()
        {/*added for json*/
            notificationLock = new Object();
            notifications = new List<Notification>();
            observers = new HashSet<IObserver>();
            
            if (storeManaging == null)
            {
                storeManaging = new LinkedList<StoreManager>();
            }
        }

        public Member(string username, int ID) : this()//Register
        {
            this.id = ID;
            this.username = username;
            this.country = "none";
            //purchasesHistory = new List<Transaction>();
        }

        public Member(string username, int ID, DateTime birthdate, string country) : this()//Register
        {
            this.id = ID;
            this.username = username;
            this.birthdate = birthdate;
            this.country = country;
           // purchasesHistory = new List<Transaction>();
        }

        /*** SERVICE LAYER FUNCTIONS***/
        public void logOut()
        {
            Logger.Log("event", logLevel.INFO, "user: " + this.username + "log out and succses");
            ConnectionStubTemp.logout(username);
        }


        /************************/

        /*** This function is the function that create Store Owner - STORE USE THIS IN THE CONSTRUCTOR ***/
        public void addStore(Store store)
        {
            Roles storeOwner = new Roles(true, true, true, true, true, true, true, true, true);
            StoreManager storeOwnerManager = new StoreManager(store, storeOwner);
            storeOwnerManager.SetStoreOwnerTrue();
            storeManaging.AddFirst(storeOwnerManager);
            Logger.Log("event", logLevel.INFO, "user: " + this.username + "created succesfully new store: " + store.id);
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
            StoreManager st = getStoreManagerOb(store);
            if (!st.isStoreOwner())
            {
                throw new Exception("you cant close this store");
            }
            else if (!store.isActive)
            {
                throw new Exception("you cant close this store, it closed already");
            }
            //notify before delete info
            string closeMessage = String.Format("the store {0} was closed", store.name);
            Member.sendMessageToAllMangersAndAdmin(store.id, closeMessage);

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
            Logger.Log("event", logLevel.INFO, "user: " + this.username + "closed succesfully store: " + store.id);
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
            return getStoreManagerRoles(store.id);
        }

        public Roles getStoreManagerRoles(int storeId)
        {
            if (isStoresManagers())
            {
                foreach (StoreManager sm in storeManaging)
                {
                    if (sm.GetStore().id == storeId)
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
                    if (sm.GetStore().id == store.id)
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
                    if (sm.GetStore().id == StoreID)
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


        /// <summary>
        ///
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <param name="StoreID"></param>
        /// <returns> the request nunmber if nessecary, else -1</returns>
        public int addStoreOwner(string username, Roles role, int StoreID)
        {
            Store store = GetStore(StoreID);
            int numberOfOwners = ConnectionStubTemp.getNumOfOwners(store);
            int requestId;
            if (numberOfOwners > 1)
            {
                requestId = ConnectionStubTemp.createOwnershipRequest(store, this, ConnectionStubTemp.getMember(username));
                return requestId;
            } else if(numberOfOwners <= 0)
            {
                throw new Exception("You dont own this store! should not happen!!");
            }
            requestId = ConnectionStubTemp.createOwnershipRequest(store, this, ConnectionStubTemp.getMember(username));
            ConnectionStubTemp.deleteOwnershipRequest(ConnectionStubTemp.getOwnershipRequest(requestId));
            return -1;
        }


        public bool makeStoreOwner(string username, Roles role, int StoreID)
        {
            Store store = GetStore(StoreID);
            StoreManager myStoreRoles = getStoreManagerOb(store);
            Member candidate = ConnectionStubTemp.getMember(username);
            if (candidate.isStoresManagers())
            {
                try
                {
                    candidate.GetStore(StoreID);
                }
                catch (Exception ex)
                {
                    myStoreRoles.CreateNewManager(candidate, role);
                }

            }
            else
            {
                myStoreRoles.CreateNewManager(candidate, role);
            }
            StoreManager candidateStoreManager = candidate.getStoreManagerOb(store);
            candidateStoreManager.SetStoreOwnerTrue();
            return true;

        }


        //true on succsess exception if somthing went wrong
        public bool approveOwnershipRequest(int requestId)
        {
            OwnershipRequest ownershipRequest = ConnectionStubTemp.getOwnershipRequest(requestId);
            ownershipRequest.approveOrDisapprovedOwnership(1, this);
            return true;
        }

        //true on succsess exception if somthing went wrong
        public bool disapproveOwnershipRequest(int requestId)
        {
            OwnershipRequest ownershipRequest = ConnectionStubTemp.getOwnershipRequest(requestId);
            ownershipRequest.approveOrDisapprovedOwnership(-1, this);
            return true;
        }



        public bool addManager(string username, Roles role, Store store)
        {

            StoreManager myStoreRoles = getStoreManagerOb(store);
            return myStoreRoles.CreateNewManager(ConnectionStubTemp.getMember(username), role);
        }

        //TODO : is there  need for remove manager option?
        public bool removeManager(string username, Store store)
        {
            Logger.Log("event", logLevel.INFO, "user: " + this.username + "try to remove user: " + username);
            StoreManager myStoreRoles = getStoreManagerOb(store);
            Member memberToRemove = ConnectionStubTemp.getMember(username);
            bool res = myStoreRoles.removeManager(memberToRemove.getStoreManagerOb(store));
            memberToRemove.RemoveStoreFromMe(memberToRemove.getStoreManagerOb(store));
            return res;
        }

        public bool removeManager(string username, int StoreID)
        {
            Logger.Log("event", logLevel.INFO, "user: " + this.username + "try to remove user: " + username);
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

        public override bool hasAddRemovePurchasingPolicies(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemovePurchasing;
        }

        public override bool hasAddRemoveStorePolicies(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemoveStorePolicy;
        }

        public override bool hasAddRemoveStoreManagerPermission(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemoveStoreManger;
        }

        public override bool hasAddRemoveDiscountPolicies(Store store)
        {
            Roles roles = getStoreManagerRoles(store);
            return roles != null && roles.AddRemoveDiscountPolicy;
        }

        public string getCountry()
        {
            return this.country;
        }

        public int getAge()
        {
            int age = DateTime.Today.Year - birthdate.Year;
            if ((DateTime.Today.Month < birthdate.Month) || (DateTime.Today.Month > birthdate.Month && DateTime.Today.Day < birthdate.Day))
                age--;
            return age;
        }

        public bool isStoresOwner(int storeId)
        {
            bool acc = false;
            if (storeManaging.Count == 0) { return false; }

            foreach (StoreManager currManger in storeManaging)
            {
                if (currManger.GetStore().id == storeId)
                {
                    StoreManager st = getStoreManagerOb(GetStore(storeId));
                    acc = acc | st.isStoreOwner();
                }
            }

            return acc;
        }

        public bool isManageStore(int storeId)
        {
            
            if (storeManaging.Count == 0) { return false; }

            foreach (StoreManager currManger in storeManaging)
            {
                if (currManger.GetStore().id == storeId)
                {
                    return true;
                }
            }

            return false;
            
        }





        #region notificactions
        public void addMessage(string msg)
        {
            lock (notificationLock)
            {
                notifications.Add(new Notification(msg));
            }
            notifyAllObservers();
        }

        public void addMessage(string msg,Notification.NotificationType type, int requestId)
        {
            lock (notificationLock)
            {
                notifications.Add(new Notification(msg,type,requestId));
            }
            notifyAllObservers();
        }

        private List<Notification> getAllMessages()
        {
            lock (notificationLock)
            {
                if (notifications.Count != 0)
                {
                    List<Notification> res = notifications;
                    notifications = new List<Notification>();
                    return res;
                }
            }
            return null;
        }

        //private List<string> notificationListToStringList(List<Notification> notifications)
        //{
        //    List<string> ret = new List<string>();
        //    foreach(Notification n in notifications)
        //    {
        //        ret.Add(n.msg);
        //    }
        //    return ret;
        //}

        public bool subscribe(IObserver observer)
        {
            if (observer == null) { return false; }
            bool ans;
            lock (notificationLock)
            {
                ans = observers.Add(observer);
            }
            if (ans)
            {
                notifyAllObservers();
            }
            return ans;
        }

        public bool unsbscribe(IObserver observer)
        {
            if (observer == null) { return false; }
            lock (notificationLock)
            {
                return observers.Remove(observer);
            }
        }

        public void notifyAllObservers()
        {
            lock (notificationLock)
            {
                if (notifications.Count != 0 & observers.Count != 0)
                {
                    List<Notification> notificationsToSend = getAllMessages();
                    foreach (IObserver curr in observers)
                    {
                        curr.update(notificationsToSend);
                    }
                }
            }
        }

        public static void sendMessageToAllOwners(int storeId, string msg)
        {
            List<Member> members = ConnectionStubTemp.members.Values.ToList();
            foreach (Member currMember in members)
            {
                if (currMember.isStoresOwner(storeId))
                {
                    currMember.addMessage(msg);
                }
            }
        }

        public static void sendMessageToAllManagers(int storeId, string msg)
        {
            List<Member> members = ConnectionStubTemp.members.Values.ToList();
            foreach (Member member in members)
            {
                LinkedList<StoreManager> managers = member.storeManaging;
                foreach (StoreManager manager in managers)
                {
                    if (manager.GetStore().id == storeId)
                    {
                        member.addMessage(msg);
                    }
                }
            }
        }

        public static void sendMessageToAdmin(string msg)
        {
            List<Member> members = ConnectionStubTemp.members.Values.ToList();
            foreach (Member member in members)
            {
                if (member is SystemAdmin)
                {
                    member.addMessage(msg);
                }
            }
        }

        public static void sendMessageToAllMangersAndAdmin(int storeId, string msg)
        {
            sendMessageToAllManagers(storeId, msg);
            sendMessageToAdmin(msg);
        }

        #endregion
    }



    public class Notification : IEntity
    {
        [Include]
        public enum NotificationType{
            NORMAL,
            CREATE_OWNER
        }

        [Key]
        public int id { get; set; }
        public string msg { get; set; }

        public NotificationType notificationType { get; set; }

        public int createOwnerReqeustId { get; set; }

        public Notification() { }
        public Notification(string msg)
        {
            this.msg = msg;
            this.notificationType = NotificationType.NORMAL;
            this.createOwnerReqeustId = -1;
        }
        public Notification(string msg, NotificationType notificationType, int createOwnerRequestId)
        {
            this.msg = msg;
            this.notificationType = notificationType;
            this.createOwnerReqeustId = createOwnerRequestId;
        }

        public override int GetKey()
        {
            return id;
        }
        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is Notification)
            {
                Notification _other = ((Notification)other);
                notificationType = _other.notificationType;
            }
        }

        public override void LoadMe()
        {

        }

    }
}
