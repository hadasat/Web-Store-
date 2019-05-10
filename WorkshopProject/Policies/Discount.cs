using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    public class Discount
    {
        public Discount successor;
        public IOutcome outcome;
        public IBooleanExpression condition;
        public int id;

        public static int DiscountCounter = 1;

        public Discount(IBooleanExpression condition, IOutcome outcome)
        {
            this.condition = condition;
            this.outcome = outcome;
        }

        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            if (condition.evaluate(products, user)){
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
            return products;
        }

        public void AddSuccessor(Discount successor)
        {
            this.successor = successor;
        }

        public static int checkDiscount(Discount dis)
        {
            dis.id = DiscountCounter++;
            return dis.id;
        }

        public Discount removeDiscount(int discountId)
        {
            if (this.id == discountId)
                return null;
            if (successor != null)
                successor.removeDiscount(discountId);
            return this;
        }
    }
}
