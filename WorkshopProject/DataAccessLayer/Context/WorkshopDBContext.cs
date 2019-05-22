﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Users;
using Tansactions;
using WorkshopProject.Policies;
using Shopping;

namespace WorkshopProject.DataAccessLayer.Context
{
    public class WorkshopDBContext : DbContext
    {
        public WorkshopDBContext(string DbName) : base(DbName)
        {
            Database.SetInitializer<WorkshopDBContext>(new CreateDatabaseIfNotExists<WorkshopDBContext>());

            // OTHER OPTIONS:
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here..
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<IBooleanExpression> Policies { get; set; }
        public DbSet<ShoppingBasket> ShoppingBaskets { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        //jonathan: why is Tranaction static?
        //public DbSet<Transaction> Transactions { get; set; }
    }

    public class WorkshopProductionDBContext : WorkshopDBContext
    {
        //public WorkshopProductionDBContext() : base("WorkshopProductionDB")
        public WorkshopProductionDBContext() : base(DataAccessDriver.connectionProductionDB)
        {
            Database.SetInitializer<WorkshopProductionDBContext>(new CreateDatabaseIfNotExists<WorkshopProductionDBContext>());
        }
    }

    public class WorkshopTestDBContext : WorkshopDBContext
    {
        //public WorkshopTestDBContext() : base("WorkshopTestDB")
        public WorkshopTestDBContext() : base(DataAccessDriver.connectionTestDB)
        {
            //Database.SetInitializer<WorkshopTestDBContext>(new DropCreateDatabaseAlways<WorkshopTestDBContext>());
            Database.SetInitializer<WorkshopTestDBContext>(new CreateDatabaseIfNotExists<WorkshopTestDBContext>());
        }
    }
}