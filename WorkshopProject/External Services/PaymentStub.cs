using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.External_Services
{
    public static class PaymentStub
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

        public static bool Pay(double price,int storeBankNum, int storeAccountNum, int userCredit, int userCsv, string userExpiryDate)
        {
            return ret;
        }

        public static int Refund(int amount, int storeBankNum, int storeAccountNum, int userCredit, int userCsv, string userExpiryDate)
        {
            return amount;
        }

        public static bool connectionTest()
        {
            return ret;
        }
    }
}
