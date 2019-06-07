using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public class DbListStub<T> where T : IEntity
    {
        private int idGen;
        private List<T> db;
        public DbListStub()
        {
            idGen = 0;
            db = new List<T>();
        }

        public T Get(int id)
        {
            foreach(IEntity e in db)
            {
                if(e.GetKey() == id)
                {
                    return (T)e;
                }
            }
            return null;
        }

        public List<T> GetList()
        {
            return db;
        }

        public void Remove(int id)
        {
            T toRemove = Get(id);
            if(toRemove == null)
            {
                return; // false;
            }
            db.Remove(toRemove);
            //return true;
        }

        public void Add(T e)
        {
            e.SetKey(++idGen);
            db.Add(e);
        }

        public void Delete()
        {
            db.Clear();
        }

        public void SaveChanges()
        {
            //do nothing
        }
    }
}
