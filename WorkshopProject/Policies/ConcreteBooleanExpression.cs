using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    public class MaxAmount : IBooleanExpression
    {
        //public string name = "MaxAmount";
        public int amount { get; set; }


        public MaxAmount() : base() {/*for json*/ }

        public MaxAmount(int amount, ItemFilter filter) : base()
        {
            this.filter = filter;
            this.amount = amount;
            
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            List<ProductAmountPrice> relevantProducts = filter.getFilteredItems(products);
            int sumProduct = ProductAmountPrice.sumProduct(relevantProducts);
            return sumProduct <= amount;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override bool checkConsistent(IBooleanExpression exp)
        {
            if (exp is MinAmount)
            {
                MinAmount min = (MinAmount)exp;
                if (this.amount < min.amount)
                    return false;
            }
            return true;
        }

        public override int GetKey()
        {
            return id;
        }

        public override string ToString()
        {
            return "Maximum amount: " + amount + "for products: "+ filter.ToString();
        }

    }

    public class MinAmount : IBooleanExpression
    {
        // public string name = "MinAmount";
        public int amount { get; set; }

        public MinAmount() : base() {/*for json*/ }

        public MinAmount(int amount, ItemFilter filter) : base()
        {
            this.filter = filter;
            this.amount = amount;
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            List<ProductAmountPrice> relevantProducts = filter.getFilteredItems(products);
            int sumProduct = ProductAmountPrice.sumProduct(relevantProducts);
            return sumProduct >= amount;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override bool checkConsistent(IBooleanExpression exp)
        {
            if (exp is MaxAmount)
            {
                MaxAmount max = (MaxAmount)exp;
                if (this.amount > max.amount)
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return "Minimum amount: " + amount + "for products: " + filter.ToString();
        }
    }

    public class UserAge : IBooleanExpression
    {
        public int age { get; set; }
        //public string name = "UserAge";

        public UserAge() : base() {/*for json*/ }

        public UserAge(int age, ItemFilter filter) : base()
        {
            this.filter = filter;
            this.age = age;
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            if (user is Member)
            {
                List<ProductAmountPrice> filterProdact = filter.getFilteredItems(products);
                DateTime userBirthDate = ((Member)user).birthdate;
                DateTime minBirthDate = DateTime.Today.AddYears(-age);
                if ((filterProdact.Count > 0 && userBirthDate > minBirthDate) || (filterProdact.Count == 0))
                {
                    return true;
                }
            }
            return false;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override bool checkConsistent(IBooleanExpression exp)
        {
            return true;
        }

        public override string ToString()
        {
            return "the user age should be: " + age + " for items: " + filter.ToString();
        }

    }
    public class UserCountry : IBooleanExpression
    {
        public string country { get; set; }
        //public string name = "UserCountry";

        public UserCountry() : base() {/*for json*/ }

        public UserCountry(string country, ItemFilter filter) : base()
        {
            this.filter = filter;
            this.country = country;
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            if (user is Member)
            {
                List<ProductAmountPrice> filterProdact = filter.getFilteredItems(products);
                string userCountry = ((Member)user).country;
                if ((filterProdact.Count > 0 && !userCountry.Equals(country)) || (filterProdact.Count == 0))
                {
                    return true;
                }
            }
            return false;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override bool checkConsistent(IBooleanExpression exp)
        {
            return true;
        }

        public override string ToString()
        {
            return "the user country should be:" + country + " for items: "+ filter.ToString();
        }

    }

    public class TrueCondition : IBooleanExpression
    {
        //public string name = "TrueCondition";

        public TrueCondition() : base()
        {
        }

        public TrueCondition(bool value)
        {
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            return true;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }
        public override bool checkConsistent(IBooleanExpression exp)
        {
            if (exp is FalseCondition)
                return false;
            return true;
        }
    }

    public class FalseCondition : IBooleanExpression
    {
        //public string name = "FalseCondition";
        public FalseCondition() : base()
        {

        }

        public FalseCondition(bool value)
        {
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            return false;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override bool checkConsistent(IBooleanExpression exp)
        {
            if (exp is TrueCondition)
                return false;
            return true;
        }

    }

    public class AndExpression : IBooleanExpression
    {
        //public string name = "AndExpression";

        public AndExpression() : base()
        {
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            return (getFirstChild().evaluate(products, user) && getSecondChild().evaluate(products, user));
        }


        public override bool checkConsistent(IBooleanExpression exp)
        {
            foreach (IBooleanExpression child in this.children)
            {
                if (!child.checkConsistent(exp))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return getFirstChild().ToString() + " AND " + getSecondChild().ToString() + "for items: "+filter.ToString();
        }
    }

    public class OrExpression : IBooleanExpression
    {
        // public string name = "OrExpression";

        public OrExpression() : base() { }

        public OrExpression(bool value)
        {
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            return (getFirstChild().evaluate(products, user) || getSecondChild().evaluate(products, user));
        }

        public override bool checkConsistent(IBooleanExpression exp)
        {
            return true;
        }

        public override string ToString()
        {
            return getFirstChild().ToString() + " OR " + getSecondChild().ToString() + "for items: " + filter.ToString(); ;
        }
    }

    public class XorExpression : IBooleanExpression
    {
        //public string name = "XorExpression";
        public XorExpression(bool value) : base()
        {
        }


        public XorExpression()
        {
        }

        public override bool evaluate(List<ProductAmountPrice> products, User user)
        {
            bool firstExp = getFirstChild().evaluate(products, user);
            bool secondExp = getSecondChild().evaluate(products, user);
            return firstExp ^ secondExp;
        }
        public override bool checkConsistent(IBooleanExpression exp)
        {
            return true;
        }

        public override string ToString()
        {
            return getFirstChild().ToString() + " XOR " + getSecondChild().ToString() + "for items: " + filter.ToString(); ;
        }
    }
}
