using System;
using System.Collections.Generic;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Member GetMember(int id, WorkshopDBContext ctx)
        {

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool AddMember(Member member, WorkshopDBContext ctx)
        {

            return true;
        }



    }
}
