﻿using Shopping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer.Context;
using WorkshopProject.Policies;

namespace WorkshopProject.DataAccessLayer
{
    public class DataAccessNonPersistent 
    {
        protected bool isProduction; 

        public DataAccessNonPersistent() : this(false)
        {
        }

        public DataAccessNonPersistent(bool isProduction)
        {
            this.isProduction = isProduction;
        }

        /// <summary>
        /// Sets the mode of the DataAccess.
        /// True: Production.
        /// False: Test.
        /// </summary>
        /// <param name="isProduction"></param>
        public void SetMode(bool isProduction)
        {
            this.isProduction = isProduction;
        }

        /// <summary>
        /// Gets the mode of the DataAccess.
        /// True: Production.
        /// False: Test.
        /// </summary>
        /// <param name="isProduction"></param>
        public bool GetMode()
        {
            return isProduction;
        }

        /// <summary>
        /// Saves an object in the database. If it is new - it is added into the DB. If it is not new it will be updated.
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
        /// <exception cref="DbEntityValidationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="isProduction"></param>
        public bool SaveMember(Member entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Member> set = ctx.Members;

            return Save(entity, entity.ID, ctx, set);
        }

        public Member GetMember(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Member> set = ctx.Members;

            return set.Find(id);
        }

        public bool RemoveMember(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Member> set = ctx.Members;

            return Remove(id, ctx, set);
        }


        /// <summary>
        /// Saves an object in the database. If it is new - it is added into the DB. If it is not new it will be updated.
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
        /// <exception cref="DbEntityValidationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="isProduction"></param>
        public bool SaveStore(Store entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Store> set = ctx.Stores;

            return Save(entity, entity.id, ctx, set);
        }

        public Store GetStore(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Store> set = ctx.Stores;

            return set.Find(id);
        }


        public bool RemoveStore(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Store> set = ctx.Stores;

            return Remove(id, ctx, set);
        }

        /// <summary>
        /// Saves an object in the database. If it is new - it is added into the DB. If it is not new it will be updated.
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
        /// <exception cref="DbEntityValidationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="isProduction"></param>
        public bool SaveProduct(Product entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Product> set = ctx.Products;

            return Save(entity, entity.id, ctx, set);
        }

        public Product GetProduct(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Product> set = ctx.Products;

            return set.Find(id);
        }

        public bool RemoveProduct(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Product> set = ctx.Products;

            return Remove(id, ctx, set);
        }


        /// <summary>
        /// Saves an object in the database. If it is new - it is added into the DB. If it is not new it will be updated.
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
        /// <exception cref="DbEntityValidationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="isProduction"></param>
        public bool SaveDiscount(Discount entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Discount> set = ctx.Discounts;

            return Save(entity, entity.id, ctx, set);
        }

        public Discount GetDiscount(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Discount> set = ctx.Discounts;

            return set.Find(id);
        }

        public bool RemoveDiscount(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Discount> set = ctx.Discounts;

            return Remove(id, ctx, set);
        }


        /// <summary>
        /// Saves an object in the database. If it is new - it is added into the DB. If it is not new it will be updated.
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
        /// <exception cref="DbEntityValidationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="isProduction"></param>
        public bool SavePolicy(IBooleanExpression entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<IBooleanExpression> set = ctx.Policies;

            return Save(entity, entity.id, ctx, set);
        }

        public IBooleanExpression GetPolicy(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<IBooleanExpression> set = ctx.Policies;

            return set.Find(id);
        }

        public bool RemovePolicy(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<IBooleanExpression> set = ctx.Policies;

            return Remove(id, ctx, set);
        }

        
        /// <summary>
        /// Saves an object in the database. If it is new - it is added into the DB. If it is not new it will be updated.
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
        /// <exception cref="DbEntityValidationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="isProduction"></param>
        public bool SaveShoppingBasket(ShoppingBasket entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingBasket> set = ctx.ShoppingBaskets;

            return Save(entity, entity.id, ctx, set);
        }

        public ShoppingBasket GetShoppingBasket(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingBasket> set = ctx.ShoppingBaskets;

            return set.Find(id);
        }

        public bool RemoveShoppingBasket(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingBasket> set = ctx.ShoppingBaskets;

            return Remove(id, ctx, set);
        }

        
        /// <summary>
        /// Saves an object in the database. If it is new - it is added into the DB. If it is not new it will be updated.
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
        /// <exception cref="DbEntityValidationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="isProduction"></param>
        public bool SaveShoppingCart(ShoppingCart entity)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingCart> set = ctx.ShoppingCarts;

            return Save(entity, entity.id, ctx, set);
        }

        public ShoppingCart GetShoppingCart(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingCart> set = ctx.ShoppingCarts;

            return set.Find(id);
        }

        public bool RemoveShoppingCart(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<ShoppingCart> set = ctx.ShoppingCarts;

            return Remove(id, ctx, set);
        }


        protected virtual bool Save<T>(T entity, int id, WorkshopDBContext ctx, DbSet<T> set) where T : class
        {
            bool isNew = (set.Find(id) == null);
            if (isNew)
            {
                set.Add(entity);
            }
            else
            {
                ctx.Entry(entity).State = EntityState.Modified;
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
                return true;
            }
        }

        protected virtual WorkshopDBContext getContext()
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

    public class DataAccessPersistent : DataAccessNonPersistent
    {
        protected static WorkshopProductionDBContext productionContext; //= new WorkshopProductionDBContext();
        protected static WorkshopTestDBContext testContext; //= new WorkshopProductionDBContext();

        protected override WorkshopDBContext getContext()
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