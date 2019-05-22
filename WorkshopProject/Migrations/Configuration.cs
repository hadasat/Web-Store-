namespace WorkshopProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WorkshopProject.DataAccessLayer.Context.WorkshopTestDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WorkshopProject.DataAccessLayer.Context.WorkshopTestDBContext";
        }

        protected override void Seed(WorkshopProject.DataAccessLayer.Context.WorkshopTestDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
