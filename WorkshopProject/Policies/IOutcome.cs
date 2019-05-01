using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    public abstract class IOutcome
    {
        protected IOutcome successor;

        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            List<ProductAmountPrice> modifiedList = myApply(products, user);
            if(successor != null)
            {
                return successor.Apply(modifiedList, user);
            }
            else
            {
                return modifiedList;
            }
        }

        public void AddSuccessor(IOutcome successor)
        {
            this.successor = successor;
        }

        protected abstract List<ProductAmountPrice> myApply(List<ProductAmountPrice> products, User user);
    }
}
