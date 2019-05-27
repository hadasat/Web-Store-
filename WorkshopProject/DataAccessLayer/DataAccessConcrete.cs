using Shopping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer.Context;
using WorkshopProject.Policies;

namespace WorkshopProject.DataAccessLayer
{
    public class DataAccessNonPersistent : IDataAccess
    {
        protected bool isProduction; 

        public DataAccessNonPersistent() : this(false)
        {
        }

        public DataAccessNonPersistent(bool isProduction)
        {
            this.isProduction = isProduction;
        }


        public virtual void SetMode(bool isProduction)
        {
            this.isProduction = isProduction;
        }

        public virtual bool GetMode()
        {
            return isProduction;
        }


        public virtual DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] paramaters)
        {
            return getContext().Database.SqlQuery<T>(sql, paramaters);
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] paramaters)
        {
            return getContext().Database.ExecuteSqlCommand(sql, paramaters);
        }

        public virtual bool Delete()
        {
            return getContext().Database.Delete();
        }



        public virtual bool SaveMember(Member entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Member> set = ctx.Members;

            return Save(entity, entity.id, ctx, set);
        }

        public virtual Member GetMember(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Member> set = ctx.Members;

            return set.Find(id);
        }

        public virtual bool RemoveMember(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Member> set = ctx.Members;

            return Remove(id, ctx, set);
        }

        public virtual bool SaveStore(Store entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Store> set = ctx.Stores;

            return Save(entity, entity.id, ctx, set);
        }

        public virtual Store GetStore(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Store> set = ctx.Stores;

            return set.Find(id);
        }


        public virtual bool RemoveStore(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Store> set = ctx.Stores;

            return Remove(id, ctx, set);
        }

        public virtual bool SaveProduct(Product entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Product> set = ctx.Products;

            return Save(entity, entity.id, ctx, set);
        }

        public virtual Product GetProduct(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Product> set = ctx.Products;

            return set.Find(id);
        }

        public virtual bool RemoveProduct(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Product> set = ctx.Products;

            return Remove(id, ctx, set);
        }

        public virtual bool SaveDiscount(Discount entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Discount> set = ctx.Discounts;

            return Save(entity, entity.id, ctx, set);
        }

        public virtual Discount GetDiscount(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Discount> set = ctx.Discounts;

            return set.Find(id);
        }

        public virtual bool RemoveDiscount(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Discount> set = ctx.Discounts;

            return Remove(id, ctx, set);
        }

        public virtual bool SavePolicy(IBooleanExpression entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<IBooleanExpression> set = ctx.PurchasingPolicies;

            return Save(entity, entity.id, ctx, set);
        }

        public virtual IBooleanExpression GetPolicy(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<IBooleanExpression> set = ctx.PurchasingPolicies;

            return set.Find(id);
        }

        public virtual bool RemovePolicy(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<IBooleanExpression> set = ctx.PurchasingPolicies;

            return Remove(id, ctx, set);
        }

        public virtual bool SaveShoppingBasket(ShoppingBasket entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingBasket> set = ctx.ShoppingBaskets;

            return Save(entity, entity.id, ctx, set);
        }

        public virtual ShoppingBasket GetShoppingBasket(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingBasket> set = ctx.ShoppingBaskets;

            return set.Find(id);
        }

        public virtual bool RemoveShoppingBasket(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingBasket> set = ctx.ShoppingBaskets;

            return Remove(id, ctx, set);
        }

        
        public virtual bool SaveShoppingCart(ShoppingCart entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingCart> set = ctx.ShoppingCarts;

            return Save(entity, entity.id, ctx, set);
        }

        public virtual ShoppingCart GetShoppingCart(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingCart> set = ctx.ShoppingCarts;

            return set.Find(id);
        }

        public virtual bool RemoveShoppingCart(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingCart> set = ctx.ShoppingCarts;

            return Remove(id, ctx, set);
        }


        protected virtual bool Save<T>(T entity, int id, WorkshopDBContext ctx, DbSet<T> set) where T : class
        {
            T exsitingEntity = set.Find(id);
            if (exsitingEntity == null)
            {
                set.Add(entity);
            }
            else
            {
                ctx.Entry(exsitingEntity).CurrentValues.SetValues(entity);
                //ctx.Entry(entity).State = EntityState.Modified;
            }

            ctx.SaveChanges();
            return true;
        }


        protected virtual bool Remove<T>(int id, WorkshopDBContext ctx, DbSet<T> set) where T : class
        {
            T entity = set.Find(id);
            if (entity == null)
            {
                return true;
            }
            else
            {
                set.Remove(entity);
                ctx.SaveChanges();
                return true;
            }
        }

        public virtual WorkshopDBContext getContext()
        {
            if (isProduction)
            {
                return new WorkshopProductionDBContext();
            }
            else
            {
                return new WorkshopTestDBContext();
            }
        }

       
    }

    public class DataAccessStatic : DataAccessNonPersistent
    {
        protected static WorkshopProductionDBContext productionContext; //= new WorkshopProductionDBContext();
        protected static WorkshopTestDBContext testContext; //= new WorkshopProductionDBContext();

        public DataAccessStatic() : base(false)
        {
        }

        public DataAccessStatic(bool isProduction) : base(isProduction)
        {
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public override DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] paramaters)
        {
            return base.SqlQuery<T>(sql, paramaters);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int ExecuteSqlCommand(string sql, params object[] paramaters)
        {
            return base.ExecuteSqlCommand(sql, paramaters);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Delete()
        {
            return base.Delete();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override bool Save<T>(T entity, int id, WorkshopDBContext ctx, DbSet<T> set)
        {
            return base.Save(entity,id , ctx, set);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override bool Remove<T>(int id, WorkshopDBContext ctx, DbSet<T> set) 
        {
            return base.Remove(id, ctx, set);
        }

        public override WorkshopDBContext getContext()
        {
            if (isProduction)
            {
                if(productionContext == null)
                {
                    productionContext = new WorkshopProductionDBContext();
                }
                return productionContext;
            }
            else
            {
                if (testContext == null)
                {
                    testContext = new WorkshopTestDBContext();
                }
                return testContext;
            }
        }
    }
}
