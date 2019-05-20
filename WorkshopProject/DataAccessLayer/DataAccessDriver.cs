using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public static class DataAccessDriver
    {
        private static bool persistent = true;

        public static IDataAccess GetDataAccess()
        {
            if (persistent)
            {
                return new DataAccessPersistent();
            }
            else
            {
                return new DataAccessNonPersistent();
            }       
        }

        //public static ApplicationParameters.ConnectionStringName connectionString;
    }
}
