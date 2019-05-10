using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.UsesrsN
;

namespace WorkshopProject.Policies
{
    public interface IOutcome
    {
        List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user);
    }
}
