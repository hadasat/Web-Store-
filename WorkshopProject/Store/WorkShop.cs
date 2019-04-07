using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkshopProject.Category;
using Users;

namespace WorkshopProject
{
    public  static class  WorkShop
    {
       public  static Dictionary<int,Store> stores = new Dictionary<int, Store>();
        public static  int id = 0;
       

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
            foreach (Store store in stores.Values)
            {
                /*Store store = getStore(store_id);*/
                Dictionary<int, Product> products = store.GetStock();
                foreach (Product item in products.Values) {
                    if ((name == null || name == item.name) && (category == null || category == item.category)
                        && (endPrice == -1 || endPrice > item.getPrice()) && (startPrice == -1 || startPrice < item.getPrice())
                        && (storeRanking==-1 || storeRanking<store.rank) && (productRanking == -1 || productRanking<item.rank))
                    {
                        //All the non-empty search filters has been matched
                        matched_products.Add(item);
                    }
                }
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
                return stores[store_id];
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }


        }

       
        public static void createNewStore(string name, int rank, Boolean isActive, Member owner)
        {
            Store store = new Store(id,name, rank, isActive);
            stores.Add(id,store);
            id++;
            owner.addStore(store);
        }

        public static bool closeStore(int storeId, Member owner)
        {
            try
            {
                owner.closeStore(stores[storeId]);
                stores[storeId].isActive = false;
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public static Dictionary<Store,Product> findProduct(int productId)
        {
            Product product;
            Dictionary<Store, Product> sroreProduct = new Dictionary<Store, Product>();
            foreach(KeyValuePair<int,Store> s in stores)
            {
                Store store = s.Value;
                product = store.findProduct(productId);
                if (product != null)
                {
                    sroreProduct[store]= product;
                    return sroreProduct;
                }
            }
            return null;
        }
    }
}