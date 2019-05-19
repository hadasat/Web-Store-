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

namespace WorkshopProject.DataAccessLayer
{
    public class DataAccessNonPersistent
    {
        private bool isProduction; 

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

            bool isNew = (set.Find(entity.ID) == null);
            if (isNew)
            {
                set.Add(entity);
            }
            else {
                ctx.Entry(entity).State = EntityState.Modified;
            }

            ctx.SaveChanges();
            return true;
        }

        public Member GetMember(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Member> set = ctx.Members;

            return set.Find(id);
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

            bool isNew = (set.Find(entity.id) == null);
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

        public Store GetStore(int id)
        {
            WorkshopDBContext ctx = getContext();
            DbSet<Store> set = ctx.Members;

            return set.Find(id);
        }








        private WorkshopDBContext getContext()
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


}
