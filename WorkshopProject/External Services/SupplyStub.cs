using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace External_Services
{
    public static class SupplyStub
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

        public static bool supply(string sourceAddress, string targetAddress)
        {
            return ret;
        }

        public static bool connectionTest()
        {
            return ret;
        }
    }
}
