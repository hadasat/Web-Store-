using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    //https://blog.goyello.com/2011/11/23/entity-framework-invalid-operation/
    public class Repo
    {
        private WorkshopDBContext ctx;

        public Repo()
        {
            ctx = new WorkshopTestDBContext();
        }

        public IEntity Get<T>(int key) where T : IEntity
        {
            return ctx.Set<T>().Find(key);
        }

        public List<T> GetList<T>() where T : IEntity
        {
            return ctx.Set<T>().ToList<T>();
        }

        public void Update<T>(T entity) where T : IEntity
        {
            ctx.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Add<T>(List<T> entity) where T : IEntity
        {
            ctx.Set<T>().AddRange(entity);
            SaveChanges();
        }

        public void Add<T>(T entity) where T : IEntity
        {
            ctx.Set<T>().Add(entity);
            SaveChanges();
        }

        public void Remove<T>(T entity) where T : IEntity
        {
            ctx.Set<T>().Remove(entity);
        }

        public void Remove<T>(List<T> entity) where T : IEntity
        {
            ctx.Set<T>().RemoveRange(entity);
        }

        //public void Clear<T>() where T : IEntity
        //{
        //    ctx.Set<T>().RemoveRange(GetList<T>());
        //    ctx.ChangeTracker.DetectChanges();
        //    ctx.SaveChanges();
        //}


        public void SaveChanges()
        {
            ctx.ChangeTracker.DetectChanges();
            ctx.SaveChanges();
        }

    }
}
