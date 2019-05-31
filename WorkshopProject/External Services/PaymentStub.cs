using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.External_Services
{
    public class PaymentStub : IPayment
    {
        public bool ret;

        public PaymentStub (bool active)
        {
            ret = active;
        }


        public bool getRet()
        {
            return ret;
        }

        public void setRet(bool newRet)
        {
            ret = newRet;
        }

        public double Pay(double price, int storeBankNum, int storeAccountNum, int userCredit, int userCsv, string userExpiryDate)
        {
            return price;
        }

        public double Refund(double amount, int storeBankNum, int storeAccountNum, int userCredit, int userCsv, string userExpiryDate)
        {
            return amount;
        }

        public bool connectionTest()
        {
            return ret;
        }

        public Task<int> payment(int cardNumber, int month, int year, string holder, int ccv, int id)
        {
            return ret ? Task.FromResult(10000) : Task.FromResult (- 1);
        }

        public Task<bool> cancelPayment(int transactionId)
        {
            return Task.FromResult(ret);
        }
    }
}
