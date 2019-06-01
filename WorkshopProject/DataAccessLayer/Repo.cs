using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public class Repo
    {
        private WorkshopDBContext ctx;

        public Repo()
        {
            ctx = new WorkshopTestDBContext();
        }

        public IEntity Get<T>() where T : IEntity
        {
            return null;



        }

        public void Save<T>() where T : IEntity {

        }

        public void Remove<T>() where T : IEntity
        {

        }

        public void SaveChanges()
        {

        }

    }
}
