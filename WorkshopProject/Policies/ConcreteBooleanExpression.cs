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
        private int amount;

        public MaxAmount(int amount, ItemFilter filter)
        {
            this.filter = filter;
            this.amount = amount;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            //TODO:
            return true;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class MinAmount : IBooleanExpression
    {
        private int amount;

        public MinAmount(int amount, ItemFilter filter)
        {
            this.filter = filter;
            this.amount = amount;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            //TODO:
            return true;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
    }

    public class UserAge : IBooleanExpression
    {
        private int age;

        public UserAge(int age, ItemFilter filter)
        {
            this.filter = filter;
            this.age = age;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            //TODO:
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
        private string country;

        public UserCountry(string country, ItemFilter filter)
        {
            this.filter = filter;
            this.country = country;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            //TODO:
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
        private IBooleanExpression firstChild;
        private IBooleanExpression secondChild;

        public AndExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return (firstChild.evaluate(cart, products) && secondChild.evaluate(cart, products));
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            this.firstChild = firstChild;
            this.secondChild = secondChild;
        }
    }

    public class OrExpression : IBooleanExpression
    {
        private IBooleanExpression firstChild;
        private IBooleanExpression secondChild;

        public OrExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return (firstChild.evaluate(cart, products) || secondChild.evaluate(cart, products));
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            this.firstChild = firstChild;
            this.secondChild = secondChild;
        }
    }

    public class XorExpression : IBooleanExpression
    {
        private IBooleanExpression firstChild;
        private IBooleanExpression secondChild;

        public XorExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products)
        {
            return firstChild.evaluate(cart, products) ^ secondChild.evaluate(cart, products);
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            this.firstChild = firstChild;
            this.secondChild = secondChild;
        }
    }
}
