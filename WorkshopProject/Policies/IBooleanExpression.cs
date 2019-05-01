using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WorkshopProject.Policies
{
    /// <summary>
    /// Implements the Composite design pattern.
    /// This is essentially the "Purchasing Policy"
    /// </summary>
    public abstract class IBooleanExpression
    {
        protected ItemFilter filter;

        public abstract bool evaluate(ShoppingCart cart,  List<ProductAmountPrice> products);

        public abstract void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild);
    }

}
