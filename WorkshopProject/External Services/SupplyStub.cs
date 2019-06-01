using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.External_Services
{
    public  class SupplyStub : ISupply
    {
        public  bool ret ;

        public SupplyStub (bool active)
        {
            ret = active;
        }

        public  bool getRet()
        {
            return ret;
        }

        public  void setRet(bool newRet)
        {
            ret = newRet;
        }

        public  bool supply(string sourceAddress, string targetAddress)
        {
            return ret;
        }

        public  bool connectionTest()
        {
            return ret;
        }

        public Task<int> supply(string name, string address, string city, string country, string zip)
        {
            return ret ? Task.FromResult(10000) : Task.FromResult(-1);
        }

        public Task<bool> cancelSupply(int transactionId)
        {
            return Task.FromResult(ret);
        }
    }
}
