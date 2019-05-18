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
    public static class DuplicateHandler
    {

        private static ObjectMapper map = new ObjectMapper();

        public static Member GetMember(int id, WorkshopDBContext ctx)
        {

            return null;
        }

        
        public static bool AddMember(Member member, WorkshopDBContext ctx)
        {

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static T Get<T>(int id, DbSet<T> db) where T : class
        {
            T ret;
            //ret = map.TryGetMember(id);
            if (ret != null)
            {
                return ret;
            }




            return null;
        }

    }
}
