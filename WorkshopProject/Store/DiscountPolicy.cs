using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    class DiscountPolicy
    {

       public int amount;


        public DiscountPolicy()
        {
            amount = 0;
        }

        public DiscountPolicy(int discountAmount)
        {
            amount = discountAmount;
        }
    }
}
