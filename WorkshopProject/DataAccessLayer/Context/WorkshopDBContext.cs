using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Users;
using TansactionsNameSpace;
using WorkshopProject.Policies;
using Shopping;
using Managment;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace WorkshopProject.DataAccessLayer.Context
{
    public class WorkshopDBContext : DbContext
    {
        public WorkshopDBContext(string DbName) : base(DbName)
        {
            Database.SetInitializer<WorkshopDBContext>(new CreateDatabaseIfNotExists<WorkshopDBContext>());
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 30;

            this.Database.CommandTimeout = 30;

            // OTHER OPTIONS:
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here..
        }

        public bool CheckConnection()
        {
            try
            {
                this.Database.Connection.Open();
                this.Database.Connection.Close();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<IBooleanExpression> PurchasingPolicies { get; set; }
        public DbSet<ShoppingBasket> ShoppingBaskets { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        //jonathan: why is Tranaction static?
        //public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<StoreManager> StoreManagers { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<IOutcome> PolicyOutcomes { get; set; }
        public DbSet<ItemFilter> PolicyFilters { get; set; }
       // public DbSet<ShoppingCartAndStore> ProductsInCarts { get; set; }
       // public DbSet<JsonShoppingCartValue> CartsInBaskets { get; set; }
        //public DbSet<ShoppingCartDeal> ShoppingCarDeals { get; set; }
        //public DbSet<ProductAmountPrice> ProductAmountPrices { get; set; }
    }

    public class WorkshopProductionDBContext : WorkshopDBContext
    {
        public WorkshopProductionDBContext() : base(DataAccessDriver.getConnectionString())
        {
            Database.SetInitializer<WorkshopProductionDBContext>(new CreateDatabaseIfNotExists<WorkshopProductionDBContext>());
        }
    }

    public class WorkshopTestDBContext : WorkshopDBContext
    {
        public WorkshopTestDBContext() : base(DataAccessDriver.getConnectionString())
        {
            Database.SetInitializer<WorkshopTestDBContext>(new CreateDatabaseIfNotExists<WorkshopTestDBContext>());
        }
    }
}
