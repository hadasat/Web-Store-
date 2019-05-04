using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;

namespace External_Services
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

        public static bool checkConsistency(List<PurchasePolicy> policies, List<DiscountPolicy> discounts,ShoppingCart cart)
        {
            return ret;
        }

        public static bool connectionTest()
        {
            return ret;
        }
    }

}
