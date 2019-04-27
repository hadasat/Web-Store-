using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace External_Services
{
    public static class SupplyAdapter
    {
        public static bool commitSupply(User user,string sourceAddress, int destAddress)
        {
            return Supply.supply(sourceAddress, destAddress);
        }
    }

    public static class Supply
    {
        public static bool supply(string sourceAddress, int targetAddress)
        {
            return true;
        }
    }
}
