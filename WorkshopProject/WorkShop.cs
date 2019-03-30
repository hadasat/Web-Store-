using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkshopProject.Category;

namespace WorkshopProject
{
    class WorkShop
    {
        List<Store> stores;
        List<User> users;

        public WorkShop()
        {
            stores = new List<Store>();
            users = new List<User>();
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
            foreach (Store store_id in stores)
            {
                Store store = getStore(store_id);
                Dictionary<int, Product> products = store.products;
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
        private Store getStore(Store store_id)
        {
            throw new NotImplementedException();
        }
    }
}
