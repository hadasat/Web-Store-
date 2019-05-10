using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.UsesrsN
;

namespace WorkshopProject.Policies
{
    public class FreeProduct : IOutcome
    {
        public FreeProduct()
        {
            //TODO FreeProduct
        }

        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            //TODO FreeProduct
            throw new NotImplementedException();
        }
    }


    public class Percentage : IOutcome
    {
        public Percentage()
        {
            //TODO Percentage
        }

        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            //TODO Percentage
            throw new NotImplementedException();
        }
    }
}
