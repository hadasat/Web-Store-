using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.DataAccessLayer
{
    //https://blog.goyello.com/2011/11/23/entity-framework-invalid-operation/

    //https://docs.microsoft.com/en-us/ef/ef6/saving/concurrency

    public class Repo
    {
        //private static WorkshopDBContext ctx;


        public Repo()
        {
            //if (ctx == null) { ctx = new WorkshopDBContext(); }
        }

        private WorkshopDBContext getContext()
        {
            return DataAccessDriver.getContext();
        }

        public virtual IEntity Get<T>(int key) where T : IEntity
        {
            return getContext().Set<T>().Find(key);
        }

        public virtual List<T> GetList<T>() where T : IEntity
        {
            return getContext().Set<T>().ToList<T>();
        }

        public virtual void Update<T>(T entity) where T : IEntity
        {
            WorkshopDBContext ctx = getContext();
            ctx.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }


        /// <summary>
        /// Provides with id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
 
        /*
        public virtual void Add<T>(List<T> entity) where T : IEntity
        {
            WorkshopDBContext ctx = getContext();
            ctx.Set<T>().AddRange(entity);
            SaveChanges();
        }*/

        public virtual void Add<T>(T entity) where T : IEntity
        {
            WorkshopDBContext ctx = getContext();
            ctx.Set<T>().Add(entity);
            SaveChanges();
        }

        public virtual void Remove<T>(T entity) where T : IEntity
        {
            WorkshopDBContext ctx = getContext();
            ctx.Set<T>().Remove(entity);
        }

        /*
        public virtual void Remove<T>(List<T> entity) where T : IEntity
        {
            WorkshopDBContext ctx = getContext();
            ctx.Set<T>().RemoveRange(entity);
        }
        */

        //public void Clear<T>() where T : IEntity
        //{
        //    ctx.Set<T>().RemoveRange(GetList<T>());
        //    ctx.ChangeTracker.DetectChanges();
        //    ctx.SaveChanges();
        //}

        public virtual void Delete()
        {
            WorkshopDBContext ctx = getContext();
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                //do nothing
            }
            //ctx.Dispose();
            //ctx = new WorkshopTestDBContext();
            MurderAllConnections(ctx);

            ctx.Database.Delete();
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                //do nothing
            }
            
        }

        public virtual void SaveChanges()
        {
            WorkshopDBContext ctx = getContext();
            ctx.ChangeTracker.DetectChanges();

            //use database wins
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);


           // ctx.SaveChanges();
        }

        //https://www.axian.com/2015/06/24/entity-framework-6-taming-the-open-connection-conundrum/
        private void MurderAllConnections(WorkshopDBContext context)
        {
            try
            {
                // FIRST: Build a connection using the DB Context's current connection.
                SqlConnectionStringBuilder sqlConnBuilder = new SqlConnectionStringBuilder(context.Database.Connection.ConnectionString);
                // Set the catalog to master so that the DB can be dropped
                sqlConnBuilder.InitialCatalog = "master";
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnBuilder.ConnectionString))
                {
                    sqlConnection.Open();
                    string dbName = context.Database.Connection.Database;
                    // Build up the SQL string necessary for dropping database connections. This statement is doing a couple of things:
                    // 1) Tests to see if the DB exists in the first place.
                    // 2) If it does, sets single user mode, which kills all connections.
                    string sql = @"IF EXISTS(SELECT NULL FROM sys.databases WHERE name = '" + dbName + "') BEGIN ALTER DATABASE [" + dbName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE END";
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConnection))
                    {
                        // Run and done.
                        sqlCmd.CommandType = System.Data.CommandType.Text;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Something bad happened.
                throw new Exception("Hey, boss, the UnitTestInitializer failed. You want I should fix it?");
            }
        }
    }

}
