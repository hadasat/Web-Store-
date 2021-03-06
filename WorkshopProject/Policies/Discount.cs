﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.Policies
{
    public class Discount : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public virtual Discount successor { get; set; }
        [Include]
        public virtual IOutcome outcome { get; set; }
        [Include]
        public virtual IBooleanExpression condition { get; set; }

        public static int DiscountCounter = 1;


        public Discount() { }
        public Discount(IBooleanExpression condition, IOutcome outcome)
        {
            this.condition = condition;
            this.outcome = outcome;
        }

        public override int GetKey()
        {
            return id;
        }

        public override void SetKey(int key)
        {
            id = key;
        }
        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            if (condition.evaluate(products, user)){
                List<ProductAmountPrice> filterdList = condition.filter.getFilteredItems(products);
                List<ProductAmountPrice> modifiedList = outcome.Apply(filterdList, user);
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

        public override bool Equals(object obj)
        {
            if (obj is Discount)
                if (id == ((Discount)obj).id)
                    return true;
            return false;
        }

        public override int GetHashCode()
        {
            //return base.GetHashCode();
            return id;
        }

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is Discount)
            {
                Discount _other = ((Discount)other);
                successor = _other.successor;
                outcome = _other.outcome;
                condition = _other.condition;
            }
        }

        public override void LoadMe()
        {
            successor.LoadMe();
            outcome.LoadMe();
            condition.LoadMe();
        }

        public static bool confirmListConsist(Discount signalExp, List<Discount> exps)
        {
            foreach (Discount exp in exps)
            {
                if (!signalExp.condition.checkConsistent(exp.condition))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return "if " + condition + " then "+ outcome;
        }
    }
}
