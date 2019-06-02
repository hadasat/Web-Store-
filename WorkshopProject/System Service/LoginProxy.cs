using Managment;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TansactionsNameSpace;
using Users;
using WorkshopProject.Communication;
using WorkshopProject.Policies;

namespace WorkshopProject.System_Service
{
    public class LoginProxy
    {
        bool loggedIn;
        public User user { get; set; }

        public static readonly string successMsg  = "success";
        public static readonly string failureMsg = "failure";

        public LoginProxy()
        {
            user = new User();
            loggedIn = false;
        }

        public void updateMember(User member)
        {
            user = member;
        }

        public bool AddProductToBasket(int storeId, int productId, int amount)
        {
            return TransactionService.AddProductToBasket(user, storeId, productId, amount);
        }

        public bool AddProductToStock(int storeId, int productId, int amount)
        {
            if (!loggedIn)
                notLoggedInException();
            return StoreService.AddProductToStock(user, storeId, productId, amount);
        }

        public int AddProductToStore(int storeId, string name, string desc, double price, string category)
        {
            if (!loggedIn)
                notLoggedInException();
            return StoreService.AddProductToStore(user, storeId, name, desc, price, category);
        }

        public int AddStore(string storeName)
        {
            if (!loggedIn)
                notLoggedInException();

            return StoreService.AddStore(user, storeName);

        }

        public bool AddStoreManager(int storeId, string userToAdd, string roles)
        {
            if (!loggedIn)
                notLoggedInException();
            return UserService.AddStoreManager(user, storeId, userToAdd, roles);
        }

        public bool AddStoreOwner(int storeId, string userToAdd)
        {
            if (!loggedIn)
                notLoggedInException();
            int ownerShipRequestID =  UserService.AddStoreOwner(user, storeId, userToAdd);
            if(ownerShipRequestID == -1)
            {
                //this means there was only one user that was an owner so he approved himself the request,
                //no need to de anything except for updating the memebr on succses
                return true;
            } else
            {
                //in this case:
                //needs to save the request id and that the client side will be able to show
                //the rigth request to the relevant user, and he will use the 
                //ApproveOwnershipRequest or DisApproveOwnershipRequest

            }

            return true;
        }

        public bool ApproveOwnershipRequest(int requestID)
        {
            if (!loggedIn)
                notLoggedInException();
            return UserService.ApproveOwnershipRequest(user, requestID);
        }

        public bool DisApproveOwnershipRequest(int requestID)
        {
            if (!loggedIn)
                notLoggedInException();
            return UserService.DisApproveOwnershipRequest(user, requestID);
        }

        public Transaction BuyShoppingBasket(int cardNumber, int month, int year, string holder, int ccv, int id, string name, string address, string city, string country, string zip)
        {
            return TransactionService.BuyShoppingBasket(user,cardNumber,month,year,holder,ccv,id,name,address,city,country,zip);
        }

        public bool ChangeProductInfo(int storeId, int productId, string name, string desc, double price, string category, int amount)
        {
            if (!loggedIn)
                notLoggedInException();
            return StoreService.ChangeProductInfo(user, storeId, productId, name, desc, price, category, amount);
        }

        public bool closeStore(int storeID)
        {
            if (!loggedIn)
                notLoggedInException();
            return StoreService.CloseStore(user, storeID);
        }

        public string GetProductInfo(int productId)
        {
            return JsonHandler.SerializeObject(StoreService.GetProductInfo(productId));
        }

        public ShoppingCart GetShoppingCart(int storeId)
        {
            return TransactionService.GetShoppingCart(user, storeId);
        }

        public string GetShoppingBasket()
        {
            return JsonHandler.SerializeObject(TransactionService.GetShoppingBasket(user));
        }

        public string login(string username, string password)
        {
            Member ret;
            try { ret = UserService.login(username, password,user); }
            catch (Exception e) { return e.Message; }
            if (ret != null)
            {
                updateMember(ret);
                if (user is Member)
                {
                    loggedIn = true;
                }
                else
                    loggedIn = false;
            }
            return loggedIn ? successMsg : failureMsg;
        }

        public Member loginEx(string username, string password)
        {
            Member ret;
            ret = UserService.login(username, password,user);
            if (ret != null)
            {
                updateMember(ret);
                if (user is Member)
                {
                    loggedIn = true;
                }
                else
                    loggedIn = false;
            }
            return ret;
        }

        public bool logout()
        {
            if (!loggedIn)
                notLoggedInException();
            loggedIn = false;
            bool ret;
            ret = UserService.logout(user);
            updateMember(new User());
            return ret;
        }

        //public bool Register(string username, string password)
        //{
        //    return UserService.Register(username, password);
        //}
        public bool Register(string username, string password,DateTime birthdath, string country)
        {
            return UserService.Register(username, password, birthdath,country);
        }

        public bool RemoveProductFromStore(int storeId, int productId)
        {
            if (!loggedIn)
                notLoggedInException();
            return StoreService.RemoveProductFromStore(user, storeId, productId);
        }

        public bool removePurchasingPolicy(int storeId)
        {
            //TODO
            throw new NotImplementedException();
        }

        public bool RemoveStoreManager(int storeId, string managerName)
        {
            if (!loggedIn)
                notLoggedInException();
            return UserService.RemoveStoreManager(user, storeId, managerName);
        }

        public bool RemoveUser(string usernameToRemove)
        {
            if (!loggedIn)
                notLoggedInException();
            return UserService.RemoveUser(user, usernameToRemove);
        }

        public List<Product> SearchProducts(string name, string category, string keyword, double startPrice, double endPrice, int productRank, int storeRank)
        {
            return StoreService.SearchProducts(name, category, keyword, startPrice, endPrice, productRank, storeRank);
        }

        public bool SetProductAmountInBasket(int storeId, int productId, int amount)
        {
            return TransactionService.SetProductAmountInBasket(user, storeId, productId, amount);
        }

        public string GetStore(int storeId)
        {
            Store storeAnse  = StoreService.GetStore(storeId);
            if (storeAnse == null)
            {
                throw new Exception ("store not found");
            }
            else
            {
                return JsonHandler.SerializeObject(storeAnse);
            }
        }

        public string GetAllStores()
        {
            return JsonHandler.SerializeObject(StoreService.GetAllStores());
        }

        //TODO wolf delete?
        public List<Member> GetAllManagers(int storeId)
        {
            return StoreService.getAllManagers(storeId);
        }
        
        //TODO wolf delete?
        public List<Member> GetAllOwners(int storeId)
        {
            return StoreService.getAllManagers(storeId);
        }

        public List<StoreManager> GetRoles()
        {
            if (!loggedIn)
                notLoggedInException();
            return UserService.GetRoles(user);
        }

        public List<Member> GetAllMembers()
        {
            return UserService.GetAllMembers();
        }

        public bool SendMessage(int memberId, string message)
        {
            UserService.SendMessage(memberId, message);
            return true;
        }

        public bool subscribeAsObserver (IObserver observer)
        {
            if (!loggedIn) { return false; }

            return ((Member)user).subscribe(observer);
        }

        public bool unSubscribeAsObserver (IObserver observer)
        {
            if (!loggedIn) { return false; }

            return ((Member)user).unsbscribe(observer);
        }


        //**********POLICIES*********************

        //policies
        public Policystatus addDiscountPolicy(int storeId, string policy)
        {
            if (!loggedIn)
                notLoggedInException();
            Discount dicountPolicy = JsonHandler.DeserializeObject<Discount>(policy);
            return PolicyService.addDiscountPolicy(user, storeId, dicountPolicy);
        }

        public Policystatus removeDiscountPolicy(int storeId, int policyId)
        {
            if (!loggedIn)
                notLoggedInException();
            return PolicyService.removeDiscountPolicy(user, storeId, policyId);            
        }

        public Policystatus addPurchasingPolicy(int storeId, string policy)
        {
            if (!loggedIn)
                notLoggedInException();
            IBooleanExpression purchasingPolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policy);
            return PolicyService.addPurchasingPolicy(user, storeId, purchasingPolicy);

        }

        public Policystatus removePurchasingPolicy(int storeId, int policyId)
        {
            if (!loggedIn)
                notLoggedInException();
            return PolicyService.removePurchasingPolicy(user, storeId, policyId);
           
        }

        public Policystatus addStorePolicy(int storeId, string policy)
        {
            if (!loggedIn)
                notLoggedInException();
            IBooleanExpression storePolicy = JsonHandler.DeserializeObject<IBooleanExpression>(policy);
            return PolicyService.addStorePolicy(user, storeId, storePolicy);
        }

        public Policystatus removeStorePolicy(int storeId, int policyId)
        {
            if (!loggedIn)
                notLoggedInException();
            return PolicyService.removeStorePolicy(user, storeId, policyId);
        }


        private void notLoggedInException()
        {
            throw new Exception("User not logged in");
        }


        public Roles getRolesForStore(int storeId)
        {
            if (!loggedIn)
            {
                return null;
            }
            return UserService.getRoleForStore(user,storeId);
        }

        public Policystatus addSystemPolicy(User user, IBooleanExpression policies)
        {
            if (!loggedIn)
                notLoggedInException();
            return PolicyService.addSystemPolicy(user, policies);
        }

        public Policystatus removeSystemPolicy(User user, int policyId)
        {
            if (!loggedIn)
                notLoggedInException();
            return WorkShop.removeSystemPolicy(user, policyId);

        }

    }
}
