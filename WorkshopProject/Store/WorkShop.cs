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
        public static List<Product>  search(string name, Categories category, int priceRange, int ranking, int storeRanking)
        {
            List<Product> matched_products = new List<Product>();
            foreach (Store store in stores.Values)
            {
                /*Store store = getStore(store_id);*/
                Dictionary<int, Product> products = store.GetStock();
                foreach (Product item in products.Values) {
                    if ((name == null || name == item.name) && (category == Categories.None || category == item.category)
                        && (priceRange == -1 || priceRange > item.getPrice()) && (storeRanking==-1 || storeRanking<store.rank))
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
        /// <returns>Store</returns>
        public static  Store getStore(int store_id)
        {
            return stores[store_id];
        }

       
        public static void createNewStore(string name, int rank, Boolean isActive, Member owner)
        {
            Store store = new Store(id,name, rank, isActive);
            stores.Add(id,store);
            id++;
            owner.addStore(store);
        }

        public static void closeStore(int storeId, Member owner)
        {
            owner.closeStore(stores[storeId]);
            stores[storeId].isActive = false;
        }
    }
}