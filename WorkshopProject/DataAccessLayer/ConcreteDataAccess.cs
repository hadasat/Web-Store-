using Shopping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer.Context;
using WorkshopProject.Policies;
using System.ServiceModel.DomainServices.Server;

namespace WorkshopProject.DataAccessLayer
{
    public class DataAccessNonPersistent : IDataAccess
    {
        protected bool isProduction;
        protected bool local;

        public DataAccessNonPersistent() : this(false)
        {
        }

        public DataAccessNonPersistent(bool isProduction)
        {
            this.isProduction = isProduction;
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

        //not working currently
        public bool CheckConnection()
        {
            return getContext().CheckConnection();
        }


        public virtual void SetMode(bool isProduction)
        {
            this.isProduction = isProduction;
        }

        public virtual bool GetMode()
        {
            return isProduction;
        }

        public virtual Member GetMember(int key)
        {
            Member ret = null;
            using (WorkshopDBContext ctx = getContext())
            {
                ret = GetMember(key, ctx);
            }
            return ret;
        }

        protected virtual Member GetMember(int key, WorkshopDBContext ctx)
        {
            Member ret = null;
            IQueryable<Member> q = getQueryableWithIncludes(ctx.Set<Member>());
            ret = q
                .Where(e => e.id == key).FirstOrDefault();

            if (ret != null)
            {
                ret.LoadMe();
            }
            return ret;
        }

        public virtual Store GetStore(int key)
        {
            Store ret = null;
            using (WorkshopDBContext ctx = getContext())
            {
                ret = GetStore(key, ctx);
            }
            return ret;
        }

        protected virtual Store GetStore(int key, WorkshopDBContext ctx)
        {
            Store ret = null;

            IQueryable<Store> q = getQueryableWithIncludes(ctx.Set<Store>());
            ret = q
                //.Include(s => s.Stocks)
                //.Include(s => s.purchasePolicy)
                //.Include(s => s.discountPolicy)
                //.Include(s => s.storePolicy)
                .Where(e => e.id == key).FirstOrDefault();
            if (ret != null)
            {
                ret.LoadMe();
            }

            return ret;
        }

        public virtual Product GetProduct(int key)
        {
            Product ret = null;
            using (WorkshopDBContext ctx = getContext())
            {
                ret = GetProduct(key, ctx);
            }
            return ret;
        }

        protected virtual Product GetProduct(int key, WorkshopDBContext ctx)
        {
            Product ret = null;

            IQueryable<Product> q = getQueryableWithIncludes(ctx.Set<Product>());
            ret = q
                .Where(e => e.id == key).FirstOrDefault();
            if (ret != null)
            {
                ret.LoadMe();
            }
            return ret;
        }

        public virtual DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] paramaters) where T : IEntity
        {
            return getContext().Database.SqlQuery<T>(sql, paramaters);
        }

        public virtual bool SaveEntity<T>(T entity, int key) where T : IEntity
        {
            using (WorkshopDBContext ctx = getContext())
            {
                SaveEntity<T>(entity,  key, ctx);
            }
            return true;
        }

        protected virtual bool SaveEntity<T>(T entity, int key, WorkshopDBContext ctx) where T : IEntity
        {
            DbSet set = ctx.Set<T>();
            IEntity existingEntity = (IEntity)set.Find(entity.GetKey());
            if (existingEntity == null)
            {
                set.Add(entity);
            }
            else
            {
                DbEntityEntry<IEntity> attachedEntry = ctx.Entry(existingEntity);
                attachedEntry.CurrentValues.SetValues(entity);
                attachedEntry.Entity.Copy(entity);
                //ctx.SaveChanges();
            }
            ctx.SaveChanges();

            return true;
        }

        public virtual bool RemoveEntity<T>(int key) where T : IEntity
        {
            using (WorkshopDBContext ctx = getContext())
            {
                return RemoveEntity<T>(key, ctx);
            }
        }

        protected virtual bool RemoveEntity<T>(int key, WorkshopDBContext ctx) where T : IEntity
        {
            if (key <= 0)
            {
                key = -1;
            }

            DbSet<T> set = ctx.Set<T>();

            T entity = set.Find(key);
            if (entity == null)
            {
                return false;
            }
            else
            {
                set.Remove(entity);
            }
            ctx.SaveChanges();

            return true;
        }


        public virtual bool Delete()
        {
            return getContext().Database.Delete();
        }


        protected string getTableNameFromDbSet<T>(DbContext context) where T : class
        {
            Type type = typeof(T);
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.FirstOrDefault()
                .Fragments.FirstOrDefault()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            string ret = (string)table.MetadataProperties["Table"].Value ?? table.Name;

            return ret;
        }

        protected IQueryable<T> getQueryableWithIncludes<T>(DbSet<T> set) where T : class
        {
            IQueryable<T> ret = set;
            var propsToLoad = GetPropsToLoad(typeof(T));
            foreach (var prop in propsToLoad)
            {
                ret = ret.Include(prop);
            }
            return ret;
        }


        protected IEnumerable<string> GetPropsToLoad(Type type)
        {
            HashSet<Type> _visitedTypes = new HashSet<Type>();
            _visitedTypes.Add(type);
            var propsToLoad = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                              .Where(p => p.GetCustomAttributes(typeof(IncludeAttribute), true).Any());

            foreach (var prop in propsToLoad)
            {
                yield return prop.Name;

                if (_visitedTypes.Contains(prop.PropertyType))
                    continue;

                foreach (var subProp in GetPropsToLoad(prop.PropertyType))
                {
                    yield return prop.Name + "." + subProp;
                }
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
        public override bool Delete()
        {
            return base.Delete();
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Member GetMember(int key)
        {
            return GetMember(key, getContext());
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool GetMode()
        {
            return base.GetMode();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Store GetStore(int key)
        {
            return GetStore(key, getContext());
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Product GetProduct(int key)
        {
            return GetProduct(key, getContext());
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool RemoveEntity<T>(int key) 
        {
            return RemoveEntity<T>(key, getContext());
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool SaveEntity<T>(T entity, int key)
        {
            SaveEntity<T>(entity, key, getContext());
            return true;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void SetMode(bool isProduction)
        {
            base.SetMode(isProduction);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] paramaters)
        {
            return base.SqlQuery<T>(sql, paramaters);
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




//https://www.trycatchfail.com/2013/02/02/a-generic-entity-framework-5-repository-with-eager-loading/

public class IncludeAttribute : Attribute { }
/*
public class PropertyIncluder <TEntity> where TEntity : class
{
    private readonly Func<DbQuery<TEntity>, DbQuery<TEntity>> _includeMethod;
    private readonly HashSet<Type> _visitedTypes;

    public PropertyIncluder()
    {
        //Recursively get properties to include
        _visitedTypes = new HashSet<Type>();
        var propsToLoad = GetPropsToLoad(typeof(TEntity)).ToArray();

        _includeMethod = d =>
        {
            var dbSet = d;
            foreach (var prop in propsToLoad)
            {
                dbSet = dbSet.Include(prop);
            }

            return dbSet;
        };
    }

    private IEnumerable<string> GetPropsToLoad(Type type)
    {
        _visitedTypes.Add(type);
        var propsToLoad = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(p => p.GetCustomAttributes(typeof(IncludeAttribute), true).Any());

        foreach (var prop in propsToLoad)
        {
            yield return prop.Name;

            if (_visitedTypes.Contains(prop.PropertyType))
                continue;

            foreach (var subProp in GetPropsToLoad(prop.PropertyType))
            {
                yield return prop.Name + "." + subProp;
            }
        }
    }

    public DbQuery<TEntity> BuildQuery(DbSet<TEntity> dbSet)
    {
        return _includeMethod(dbSet);
    }
}




    */













/*ARCHIVE
 
 
    
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


    */
