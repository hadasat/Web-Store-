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
        public int amount;


        public MaxAmount() {/*for json*/ }

        public MaxAmount(int amount, ItemFilter filter)
        {
            this.filter = filter;
            this.amount = amount;
            this.id = Idcounter++;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products,User user)
        {
            List<ProductAmountPrice> relevantProducts = filter.getFilteredItems(products);
            return relevantProducts.Count <= amount;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (id == policyId)
                return null;
            return this;
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
            this.id = Idcounter++;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products,User user)
        {
            List<ProductAmountPrice> relevantProducts = filter.getFilteredItems(products);
            return relevantProducts.Count >= amount;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (id == policyId)
                return null;
            return this;
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
            this.id = Idcounter++;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products, User user)
        {
            if(user is Member) {
                List<ProductAmountPrice> filterProdact = filter.getFilteredItems(products);
                DateTime userBirthDate = ((Member)user).birthdate;
                DateTime minBirthDate = DateTime.Today.AddYears(-age);
                if((filterProdact.Count > 0 && userBirthDate > minBirthDate) || (filterProdact.Count == 0))
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

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (id == policyId)
                return null;
            return this;
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
            this.id = Idcounter++;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products, User user)
        {
            if (user is Member)
            {
                List<ProductAmountPrice> filterProdact = filter.getFilteredItems(products);
                string userCountry = ((Member)user).country;
                DateTime minBirthDate = DateTime.Today.AddYears(-age);
                if ((filterProdact.Count > 0 && userCountry.Equals(country)) || (filterProdact.Count == 0))
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

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (id == policyId)
                return null;
            return this;
        }
    }

    public class TrueCondition : IBooleanExpression
    {
        //public string name = "TrueCondition";

        public TrueCondition()
        {
        }

        public TrueCondition(bool value)
        {
            this.id = Idcounter++;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products, User user)
        {
            return true;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (id == policyId)
                return null;
            return this;
        }
    }

    public class FalseCondition : IBooleanExpression
    {
        //public string name = "FalseCondition";
        public FalseCondition()
        {
            
        }

        public FalseCondition(bool value)
        {
            this.id = Idcounter++;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products, User user)
        {
            return false;
        }

        public override void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            //this exception is intended!
            throw new NotImplementedException();
        }

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (id == policyId)
                return null;
            return this;
        }
    }

    public class AndExpression : IBooleanExpression
    {
        //public string name = "AndExpression";

        public AndExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products,User user)
        {
            return (firstChild.evaluate(cart, products,user) && secondChild.evaluate(cart, products, user));
        }

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (this.id == policyId)
                return null;
            else
            {
                firstChild = firstChild.removePolicy(policyId);
                secondChild = secondChild.removePolicy(policyId);
                if (firstChild == null)
                    return secondChild;
                else if (secondChild == null)
                    return firstChild;
                return this;
            } 

        }

    }

    public class OrExpression : IBooleanExpression
    {
        // public string name = "OrExpression";

        public OrExpression() { }

        public OrExpression(bool value)
        {
            this.id = Idcounter++;
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products, User user)
        {
            return (firstChild.evaluate(cart, products, user) || secondChild.evaluate(cart, products, user));
        }

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (this.id == policyId)
                return null;
            else
            {
                firstChild = firstChild.removePolicy(policyId);
                secondChild = secondChild.removePolicy(policyId);
                if (firstChild == null)
                    return secondChild;
                else if (secondChild == null)
                    return firstChild;
                return this;
            }

        }

    }

    public class XorExpression : IBooleanExpression
    {
        //public string name = "XorExpression";
        public XorExpression(bool value)
        {
            this.id = Idcounter++;
        }

        public XorExpression()
        {
        }

        public override bool evaluate(ShoppingCart cart, List<ProductAmountPrice> products, User user)
        {
            return firstChild.evaluate(cart, products,user) ^ secondChild.evaluate(cart, products,user);
        }

        public override IBooleanExpression removePolicy(int policyId)
        {
            if (this.id == policyId)
                return null;
            else
            {
                firstChild = firstChild.removePolicy(policyId);
                secondChild = secondChild.removePolicy(policyId);
                if (firstChild == null)
                    return secondChild;
                else if (secondChild == null)
                    return firstChild;
                return this;
            }
        }

    }
}
