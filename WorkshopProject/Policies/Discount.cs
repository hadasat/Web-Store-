using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.UsesrsN
;

namespace WorkshopProject.Policies
{
    public class Discount
    {
        protected Discount successor;
        protected IOutcome outcome;
        protected IBooleanExpression condition;

        public Discount(IBooleanExpression condition, IOutcome outcome)
        {
            this.condition = condition;
            this.outcome = outcome;
        }

        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            List<ProductAmountPrice> modifiedList = outcome.Apply(products, user);
            if (successor != null)
            {
                return successor.Apply(modifiedList, user);
            }
            else
            {
                return modifiedList;
            }
        }

        public void AddSuccessor(Discount successor)
        {
            this.successor = successor;
        }
    }
}
