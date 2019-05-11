using Newtonsoft.Json.Linq;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

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
        public int id;

        public static int Idcounter = 1;

        public abstract bool evaluate(List<ProductAmountPrice> products,User user);

        public virtual void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            this.firstChild = firstChild;
            this.secondChild = secondChild;
        }

        public static int checkExpression(IBooleanExpression exp)
        {
            //may check if the expression is valid
            return Idcounter++; 
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
