using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkshopProject.Category;
using Users;
using WorkshopProject.Log;
using WorkshopProject.Policies;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject
{
    public  static class  WorkShop
    {
        public static List<IBooleanExpression> SystemPolicies = new List<IBooleanExpression>();
        //public  static Dictionary<int,Store> stores = new Dictionary<int, Store>();
        //public static  int id = 0;
        public static Repo repo = new Repo();

        /// <summary>
        /// Search products. if int is -1 - ignore. if string is null - ignore.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="priceRange"></param>
        /// <param name="ranking"></param>
        /// <param name="storeRanking"></param>
        /// <returns>list of products</returns>
        public static List<Product>  search(string name, string category, double startPrice,double endPrice, int productRanking, int storeRanking)
        {
            List<Product> matched_products = new List<Product>();
            foreach (Store store in GetStores())
            {
                List<Product> res = store.searchProducts(name,category,startPrice,endPrice,
                    productRanking, storeRanking);
                matched_products = matched_products.Concat(res).ToList();
                   
            }
            return matched_products;
        }


        /// <summary>
        /// Get store from DB
        /// </summary>
        /// <param name="store_id"></param>
        /// <returns>Store if exist. otherwise return null</returns>
        public static  Store getStore(int store_id)
        {
            try
            {
                return GetStoreById(store_id);
            }
            catch (KeyNotFoundException ignore)
            {
                return null;
            }


        }

        public static bool deleteStore(int storeId)
        {
            bool ret = (GetStoreById(storeId) != null);
            Remove(storeId);
            return ret;
        }
       
        public static int createNewStore(string name, int rank, Boolean isActive, Member owner)
        {
            Store store = new Store(0,name, rank, isActive);
            AddStore(store);
            int currID = store.id;
            //id++;
            owner.addStore(store);

            repo.Update<Member>(owner);

            Logger.Log("event", logLevel.INFO,"store " + currID + " has added");
            return currID;
        }

        public static bool closeStore(int storeId, Member owner)
        {
            try
            {
                owner.closeStore(GetStoreById(storeId));
                GetStoreById(storeId).isActive = false;
            }catch(Exception ignore)
            {
                return false;
            }
            Logger.Log("event", logLevel.INFO, "store " + storeId + " has closed");
            
            return true;
        }


        /// <summary>
        /// get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product if exist in the WorkShop. otherwise return null</returns>
        internal static Product getProduct(int productId)
        {
            foreach(Store store in GetStores())
            {
                if (store.GetStockAsDictionary().ContainsKey(productId))
                    return store.GetStockAsDictionary()[productId];
            }
            return null;
        }

        public static Dictionary<Store, Product> findProduct(int productId)
        {
            Product product;
            Dictionary<Store, Product> sroreProduct = new Dictionary<Store, Product>();
            //foreach (KeyValuePair<int, Store> s in stores)
            foreach (Store store in GetStores())
            {
                //Store store = s.Value;
                product = store.findProduct(productId);
                if (product != null)
                {
                    sroreProduct[store] = product;
                    return sroreProduct;
                }
            }
            return null;
        }

        public static Policystatus addSystemPolicy(User user, IBooleanExpression policy)
        {
            if (user is SystemAdmin)
            {
                if(!IBooleanExpression.confirmListConsist(policy,SystemPolicies))
                    return Policystatus.UnauthorizedUser;
                int newPolicyId = IBooleanExpression.checkExpression(policy);
                if (newPolicyId > 0)
                {
                    SystemPolicies.Add(policy);
                    return Policystatus.Success;
                }
                return Policystatus.BadPolicy;
            }
            return Policystatus.UnauthorizedUser;
        }

        public static Policystatus removeSystemPolicy(User user, int policyid)
        {
            if (user is SystemAdmin)
            {
                IBooleanExpression temp = new TrueCondition();
                temp.id = policyid;

                if (SystemPolicies.Remove(temp))
                    return Policystatus.Success;
            }
            return Policystatus.UnauthorizedUser;
        }

        public static List<Store> GetStores()
        {
            if (useStub())
            {
                return getDbStub().GetList();
            }
            return repo.GetList<Store>();
        }

        public static Store GetStoreById(int id)
        {
            if (useStub())
            {
                return getDbStub().Get(id);
            }
            return (Store) repo.Get<Store>(id);
        }

        public static void Remove(int id)
        {
            if (useStub())
            {
                getDbStub().Remove(id);
                return;
            }
            repo.Remove<Store>(GetStoreById(id));
        }

        public static void AddStore(Store e)
        {
            if (useStub())
            {
                getDbStub().Add(e);
                return;
            }
            repo.Add<Store>(e);
        }

        public static void AddProductToDB(Product e)
        {
            if (useStub())
            {
               // getDbStub().Add(e);
                return;
            }
            repo.Add<Product>(e);
        }

        public static void Update(Store store)
        {
            if (useStub())
            {
                
                return;
            }
            repo.Update<Store>(store);
        }

        private static bool useStub()
        {
            return DataAccessDriver.UseStub;
        }
        private static DbListStub<Store> getDbStub()
        {
            return DataAccessDriver.Stores;
        }
    }
}