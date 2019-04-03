using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkshopProject.Category;
using Users;

namespace WorkshopProject
{
    class WorkShop
    {
       public  Dictionary<int,Store> stores;
        Dictionary<int,User> users;

        public WorkShop()
        {
            stores = new Dictionary<int, Store>();
            users = new Dictionary<int, User>();
        }

        /// <summary>
        /// Search products. if int is -1 - ignore. if string is null - ignore.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="priceRange"></param>
        /// <param name="ranking"></param>
        /// <param name="storeRanking"></param>
        /// <returns>list of products</returns>
        List<Product> search(string name, Categories category, int priceRange, int ranking, int storeRanking)
        {
            List<Product> matched_products = new List<Product>();
            foreach (Store store in stores.Values)
            {
                /*Store store = getStore(store_id);*/
                Dictionary<int, Product> products = store.GetStock();
                foreach (Product item in products.Values) {
                    if ((name == null || name == item.name) && (category == Categories.None || category == item.category)
                        && (priceRange == -1 || priceRange > item.price) && (storeRanking==-1 || storeRanking<store.rank))
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
        public Store getStore(int store_id)
        {
            return stores[store_id];
        }

        public User getUser(int user_id)
        {
            return users[user_id];
        }

        public void createNewStore(int id,string name, int rank, Boolean isActive)
        {
            stores.Add(id,new Store(id,name, rank, isActive));
        }
    }
}
