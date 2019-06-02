using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer.Context;

namespace WorkshopProject.DataAccessLayer
{
    /*

    /// <summary>
    /// Handles adds/removes/saves from the DbSets of a given WorkshopDbContext
    /// </summary>
    public class ContextHandler
    {
        public ContextHandler()
        {

        }

        public bool AddMember(Member member, WorkshopDBContext ctx)
        {
            return Add(new Integer(member.ID), member, ctx.Members);
        }

        public bool AddMember(Store store, WorkshopDBContext ctx)
        {
            return Add(new Integer(store.id), store, ctx.Stores);
        }

        public bool AddMember(Product product, WorkshopDBContext ctx)
        {
            return Add(new Integer(product.id), product, ctx.Products);
        }


        public Member GetMember(int id, WorkshopDBContext ctx)
        {
            return Get(id, ctx.Members);
        }

        public Store GetStore(int id, WorkshopDBContext ctx)
        {
            return Get(id, ctx.Stores);
        }

        public Product GetProducts(int id, WorkshopDBContext ctx)
        {
            return Get(id, ctx.Products);
        }


        /// <summary>
        /// Adds the object to the give map.
        /// Fails if Key already exists in map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <param name="map"></param>
        /// <returns>True if added to map.
        /// False if object with key is already in map</returns>
        private bool Add<T>(Integer id, T obj, DbSet <T> set) where T : class
        {
            T ret = set.Find(id);
            if (ret != null)
            {
                return false;
            }
            else
            {
                //map.Add(id, obj);
                return true;
            }
        }

        /// <summary>
        /// Tries to get an object by its id (key).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="map"></param>
        /// <returns>Null if object not in map</returns>
        private T Get<T>(Integer id, DbSet<T> set) where T : class
        {
            return set.Find(id);
        }
    }
    */
}
