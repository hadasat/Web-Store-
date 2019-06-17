using Managment;
using Newtonsoft.Json.Linq;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TansactionsNameSpace;
using Users;
using WorkshopProject.Communication;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.Log;
using WorkshopProject.Policies;

namespace WorkshopProject.System_Service
{
    public class LoginProxy
    {
        public bool loggedIn { get; set; }
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


        public bool IsManageStore(int storeId)
        {
            if (!loggedIn)
                return false;
            return UserService.isManageStore(user, storeId);
        }

        public bool IsAdmin()
        {
            if (!loggedIn)
                return false;
            return UserService.isAdmin(user);
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

        public async Task<Transaction> BuyShoppingBasket(string cardNumber, int month, int year, string holder, int ccv, int id, string name, string address, string city, string country, string zip)
        {
            return await TransactionService.BuyShoppingBasket(user,cardNumber,month,year,holder,ccv,id,name,address,city,country,zip);
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
            catch (WorkShopDbException dbExc)
            {
                throw dbExc;
            }
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

        public List<Product> getAllProductsForStore (int storeId)
        {
            Store storeAns = StoreService.GetStore(storeId);
            if (storeAns == null)
            {
                return new List<Product>();
            }
            else
            {
                return storeAns.getAllProducst();
            }
        }

        public string GetAllStores()
        {
            //Store s2 = StoreService.GetStore(2);
            //foreach (Stock st in s2.Stocks)
            //{
            //    int i = st.id;
            //}

            /*
            List<Store> ls = StoreService.GetAllStores();
            //jonathan: i just wrote this for debug
            foreach (Store s in ls)
            {
                foreach (Stock st in s.Stocks)
                {
                    int i = st.id;
                }
            }
            */

            List<Store> ls = StoreService.GetAllStores();
            return JsonHandler.SerializeObject(ls);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info">info ["data"]</param>
        /// <returns></returns>
        private IBooleanExpression createLeaf(JObject info)
        {
            string leafType = (string)info["type"];
            //create products filter
            string products = (string)info["products"];
            ItemFilter filter;
            if (products == "-1")
            {
                filter = new AllProductsFilter();
            }
            else
            {
                string[] productsArr = products.Split(',').ToArray();
                List<int> productsList = new List<int>();
                foreach (string curr in productsArr)
                {
                    productsList.Add(Convert.ToInt32(curr));
                }
                filter = new ProductListFilter(productsList);
            }

            switch (leafType)
            {
                case "min":
                    return new MinAmount((int)info["data"],filter);
                case "max":
                    return new MaxAmount((int)info["data"], filter);
                case "country":
                    return new UserCountry((string)info["data"], filter);
                case "age":
                    return new UserAge((int)info["data"], filter);
                default:
                    Logger.Log("error", logLevel.ERROR, "can't create leaf");
                    throw new Exception("can't create leaf");
            }
        }

        private IBooleanExpression createComplexExpression(JObject info)
        {
            string primeType = (string)info["type"];
            IBooleanExpression firstChild = null, secondChild = null, toReturn;
            switch (primeType)
            {
                case "leaf":
                    return createLeaf((JObject)info["data"]);
                case "and":
                    firstChild = createComplexExpression((JObject)info["firstChild"]);
                    secondChild = createComplexExpression((JObject)info["secondChild"]);
                    toReturn = new AndExpression();
                    break;
                case "or":
                    firstChild = createComplexExpression((JObject)info["firstChild"]);
                    secondChild = createComplexExpression((JObject)info["secondChild"]);
                    toReturn = new OrExpression();
                    break;
                case "xor":
                    firstChild = createComplexExpression((JObject)info["firstChild"]);
                    secondChild = createComplexExpression((JObject)info["secondChild"]);
                    toReturn = new XorExpression();
                    break;
                default:
                    throw new Exception("unknon purchasing type");
            }
            if (firstChild != null && secondChild != null)
            {
                toReturn.addChildren(firstChild, secondChild);
                return toReturn;
            }
            else
            {
                throw new Exception("can't create children in policy type");
            }
        }

        public IBooleanExpression createPurchasingPolicy(JObject info)
        {
            return createComplexExpression(info);
        }

        public Discount createDiscount(JObject info)
        {
            IBooleanExpression condition = createComplexExpression(info);
            int productId = (int)info["outcome"]["product"];
            IOutcome outcome;
            if (productId == -1)
            {
                outcome = new Percentage((double)info["outcome"]["amount"]);
            }
            else
            {
                outcome = new FreeProduct(productId, (int)info["outcome"]["amount"]);
            }

            return new Discount(condition, outcome);
        }

        public Policystatus addDiscountPolicy (int storeId, Discount discount)
        {
            if (!loggedIn)
                notLoggedInException();
            return PolicyService.addDiscountPolicy(user, storeId, discount);
        }

        public Policystatus addPurchasingPolicy(int storeId, IBooleanExpression toAdd)
        {
            if (!loggedIn)
                notLoggedInException();
            return PolicyService.addPurchasingPolicy(user, storeId, toAdd);
        }

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

        public string getPoliciesString(int storeId)
        {
            List<Discount> discounts = PolicyService.getAllDiscount(user, storeId);
            List<IBooleanExpression> polices = PolicyService.getAllStorePolicies(user, storeId);
            string ans = "";
            foreach (Discount curr in discounts)
            {
                ans += curr.ToString() + "\n";
            }

            foreach (IBooleanExpression curr in polices)
            {
                ans += curr.ToString() + "\n";
            }

            return ans;
        }

    }
}
