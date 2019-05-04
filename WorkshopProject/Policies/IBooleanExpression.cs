using Newtonsoft.Json.Linq;
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
        public ItemFilter filter;
        public IBooleanExpression firstChild;
        public IBooleanExpression secondChild;

        public abstract bool evaluate(ShoppingCart cart,  List<ProductAmountPrice> products);

        public virtual void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            this.firstChild = firstChild;
            this.secondChild = secondChild;
        }

        //public static IBooleanExpression FromJson(string json)
        //{
        //    JObject boolExp = JObject.Parse(json);
        //    Type type = Type.GetType((string)boolExp["name"]);
        //    //JObject firstChild = boolExp["firstChild"];



        //    return null;
        //}
    }

}
