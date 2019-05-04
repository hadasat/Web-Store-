using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.Policies
{
    public class MaxAmount : IBooleanExpression
    {
        //public string name = "MaxAmount";
        public int amount;

        public MaxAmount() {/*for json*/ }

        public MaxAmount(int amount, ItemFilter filter)
        {
            this.filter = filter;
            this.amount = amount;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            List<ProductAmountPrice> relevantProducts = filter.getFilteredItems(products);
            return relevantProducts.Count <= amount;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class MinAmount : IBooleanExpression
    {
       // public string name = "MinAmount";
        public int amount;

        public MinAmount() {/*for json*/ }

        public MinAmount(int amount, ItemFilter filter)
        {
            this.filter = filter;
            this.amount = amount;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            List<ProductAmountPrice> relevantProducts = filter.getFilteredItems(products);
            return relevantProducts.Count >= amount;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class UserAge : IBooleanExpression
    {
        public int age;
        //public string name = "UserAge";

        public UserAge() {/*for json*/ }

        public UserAge(int age, ItemFilter filter)
        {
            this.filter = filter;
            this.age = age;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            //TODO: UserAge (IBooleanExpression) age
            return true;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class UserCountry : IBooleanExpression
    {
        public string country;
        //public string name = "UserCountry";

        public UserCountry() {/*for json*/ }

        public UserCountry(string country, ItemFilter filter)
        {
            this.filter = filter;
            this.country = country;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            //TODO: UserAge (country) age
            return true;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class TrueCondition : IBooleanExpression
    {
        //public string name = "TrueCondition";

        public TrueCondition()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return true;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class FalseCondition : IBooleanExpression
    {
        //public string name = "FalseCondition";
        public FalseCondition()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return false;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class AndExpression : IBooleanExpression
    {
        //public string name = "AndExpression";

        public AndExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return (firstChild.evaluate(cart, products) && secondChild.evaluate(cart, products));
        }

    }

    public class OrExpression : IBooleanExpression
    {
       // public string name = "OrExpression";

        public OrExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return (firstChild.evaluate(cart, products) || secondChild.evaluate(cart, products));
        }

    }

    public class XorExpression : IBooleanExpression
    {
        //public string name = "XorExpression";


        public XorExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return firstChild.evaluate(cart, products) ^ secondChild.evaluate(cart, products);
        }


    }
}
