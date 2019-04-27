using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject;

namespace External_Services
{
    public static class ConsistencyAdapter
    {
        public static bool checkConsistency(User user,List<PurchasePolicy> purches, List<DiscountPolicy> discount)
        {
            return Consistency.consistency(purches, discount);
        }

    }

    public static class Consistency
    {
        public static bool consistency(List<PurchasePolicy> purches, List<DiscountPolicy> discount)
        {
            return true;
        }
    }

}
