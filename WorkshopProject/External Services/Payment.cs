using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace External_Services
{
    public static class PaymentAdapter
    {
        public static bool commitPayment(User user,int amount,int storeBankNum, int storeAccountNum, int userCredit,int userCsv, string userExpiryDate)
        {
            return Payment.Pay(amount, storeBankNum, storeAccountNum, userCredit, userCsv, userExpiryDate);
        }
        
    }

    public static class Payment
    {


        public static bool Pay(int amount,int storeBankNum, int storeAccountNum, int userCredit, int userCsv, string userExpiryDate)
        {
            return true;
        }
    }
}
