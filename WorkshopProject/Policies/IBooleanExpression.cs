using Newtonsoft.Json.Linq;
using Shopping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.Policies
{
    public enum Policystatus {Success, UnauthorizedUser, UnactiveStore, BadPolicy, InconsistPolicy};
    /// <summary>
    /// Implements the Composite design pattern.
    /// This is essentially the "Purchasing Policy"
    /// </summary>
    public abstract class IBooleanExpression : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public ItemFilter filter { get; set; }

        [NotMapped]
        public virtual IBooleanExpression firstChild { get { return children[0]; } set => children[0] = value; }

        [NotMapped]
        public virtual IBooleanExpression secondChild { get { return children[1]; } set => children[1] = value; }

        [Include]
        public virtual List<IBooleanExpression> children { get; set; } 


        public static int Idcounter = 1;

        public IBooleanExpression()
        {
            if(children == null)
            {
                children = new List<IBooleanExpression>();
                children.Add(null);
                children.Add(null);
            }
        }

        public override int GetKey()
        {
            return id;
        }

        public override void SetKey(int key)
        {
            id = key;
        }

        public abstract bool evaluate(List<ProductAmountPrice> products,User user);

        public abstract bool checkConsistent(IBooleanExpression exp);

        public virtual void addChildren(IBooleanExpression firstChild, IBooleanExpression secondChild)
        {
            children[0] = firstChild;
            children[1] = (secondChild);
        }

        public virtual IBooleanExpression getFirstChild()
        {
            return children[0];
        }

        public virtual IBooleanExpression getSecondChild()
        {
            return children[1];
        }

        public static int checkExpression(IBooleanExpression exp)
        {
            //may check if the expression is valid
            return Idcounter++;
        }

        public override bool Equals(object obj)
        {
            if (obj is IBooleanExpression)
                if (id == ((IBooleanExpression)obj).id)
                    return true;
            return false;
        }

        public override int GetHashCode()
        {
            //return base.GetHashCode();
            int result = id;
            return result;
        }

        //public static IBooleanExpression FromJson(string json)
        //{
        //    JObject boolExp = JObject.Parse(json);
        //    Type type = Type.GetType((string)boolExp["name"]);
        //    //JObject firstChild = boolExp["firstChild"];



        //    return null;
        //}

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is IBooleanExpression)
            {
                IBooleanExpression _other = ((IBooleanExpression)other);
                filter = _other.filter;
                children = _other.children;
            }
        }

        public override void LoadMe()
        {
            foreach (IEntity obj in children)
            {
                obj.LoadMe();
            }

            filter.LoadMe();
        }

        public static bool confirmListConsist(IBooleanExpression signalExp,List<IBooleanExpression> exps)
        {
            foreach(IBooleanExpression exp in exps){
                if (!signalExp.checkConsistent(exp))
                    return false;
            }
            return true;
        }
    }

}
