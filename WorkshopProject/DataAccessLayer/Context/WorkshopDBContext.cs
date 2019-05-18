using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Users;
using Tansactions;

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

    }

    public class WorkshopProductionDBContext : WorkshopDBContext
    {
        public WorkshopProductionDBContext() : base("WorkshopProductionDB")
        {
            Database.SetInitializer<WorkshopProductionDBContext>(new CreateDatabaseIfNotExists<WorkshopProductionDBContext>());
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        //jonathan: why is Tranaction static?
        //public DbSet<Transaction> Transactions { get; set; }
    }

    public class WorkshopTestDBContext : WorkshopDBContext
    {
        public WorkshopTestDBContext() : base("WorkshopTestDB")
        {
            Database.SetInitializer<WorkshopTestDBContext>(new DropCreateDatabaseAlways<WorkshopTestDBContext>());
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        //jonathan: why is Tranaction static?
        //public DbSet<Transaction> Transactions { get; set; }
    }
}
