using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;
using WorkshopProject.Policies;

namespace WorkshopProject.External_Services
{

    public static class ConsistencyStub
    {
        public static bool ret = true;

        public static bool getRet()
        {
            return ret;
        }

        public static void setRet(bool newRet)
        {
            ret = newRet;
        }

        public static bool checkConsistency(User user,List<Discount> discount, List<IBooleanExpression> purchase,
            List<IBooleanExpression> storePolicy,ShoppingCart basket)
        {
            return ret;
        }

        public static bool connectionTest()
        {
            return ret;
        }
    }

}
