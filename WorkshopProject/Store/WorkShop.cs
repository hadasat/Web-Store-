using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkshopProject.Category;
using Users;
using WorkshopProject.Log;
using WorkshopProject.DataAccessLayer;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace WorkshopProject
{
    public  static class  WorkShop
    {
        //public  static Dictionary<int,Store> stores = new Dictionary<int, Store>();
        //public static  int id = 0;
        static IDataAccess dal = DataAccessDriver.GetDataAccess();


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
            string sql = "select * from Stores";
            SqlParameter sqlparam = new SqlParameter();
            DbRawSqlQuery<Store> query = dal.SqlQuery<Store>(sql, sqlparam);
            List<Store> stores = query.ToList();

            foreach (Store store in stores)
            {
                List<Product> res = store.searchProducts(name, category, startPrice, endPrice,
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
            //try
            //{
            //    return stores[store_id];
            //}
            //catch (KeyNotFoundException ignore)
            //{
            //    return null;
            //}
            return dal.GetEntity<Store>(store_id);


        }


       
        public static int createNewStore(string name, int rank, Boolean isActive, Member owner)
        {
            Store store = new Store(name, rank, isActive);
            //db
            dal.SaveEntity(store, store.id);  //may throw an exeption
            //stores.Add(id,store);
            int currID = store.id;
           // id++;
            owner.addStore(store);
            //dal.SaveMember(owner);
            Logger.Log("file", logLevel.INFO,"store " + currID + " has added");
            return currID;
        }

        public static bool closeStore(int storeId, Member owner)
        {
            
                Store s = dal.GetEntity<Store>(storeId);
                owner.closeStore(s);
                s.changeActiveState(false);
                //dal.SaveMember(owner);
                dal.SaveEntity(s, s.id);
            
            Logger.Log("file", logLevel.INFO, "store " + storeId + " has closed");
            
            return true;
        }

        


        /// <summary>
        /// get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product if exist in the WorkShop. otherwise return null</returns>
        internal static Product getProduct(int productId)
        {
            //foreach(Store store in stores.Values)
            //{
            //    if (store.GetStock().ContainsKey(productId))
            //        return store.GetStock()[productId];
            //}
            //return null;
            return dal.GetEntity<Product>(productId);
        }

        public static Dictionary<Store, Product> findProduct(int productId)
        {
            //Product product;
            //Dictionary<Store, Product> storeProduct = new Dictionary<Store, Product>();
            //foreach (KeyValuePair<int, Store> s in stores)
            //{
            //    Store store = s.Value;
            //    product = store.findProduct(productId);
            //    if (product != null)
            //    {
            //        storeProduct[store] = product;
            //        return storeProduct;
            //    }
            //}
            Dictionary<Store, Product> storeProduct = new Dictionary<Store, Product>();
            Product p = dal.GetEntity<Product>(productId);
            if (p == null)
                return null;
            Store s = dal.GetEntity<Store>(p.storeId);
            storeProduct[s] = p;
            return storeProduct;
        }

        public static void RemoveStoreFromDB(int storeId)
        {
            dal.RemoveEntity<Store>(storeId);
        }
    }
}