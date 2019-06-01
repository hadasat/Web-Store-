using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public static class DataAccessDriver
    {

        public static string remoteAdress = "Data Source=10.0.0.60;User ID = workshop; Password=********";
        public static string localAdress = "Data Source=.\\SQLEXPRESS;";

        public static bool Persistent { get; set; } = true;
        public static bool Local { get; set; } = true;
        public static bool Production { get; set; } = false;

        public static string connectionTestDB { get; set; } = "Initial Catalog=WorkshopTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static string connectionProductionDB { get; set; } = "Data Source=.\\SQLEXPRESS;Initial Catalog=WorkshopProductionDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //public static IDataAccess GetDataAccess()
        //{
        //    if (Persistent)
        //    {
        //        return new DataAccessStatic(Production);
        //    }
        //    else
        //    {
        //        return new DataAccessNonPersistent(Production);
        //    }       
        //}    
        

        

        public static string getConnectionString(bool Production)
        {
            string adress = Local ? localAdress : remoteAdress;
            string db = Production ? connectionProductionDB : connectionTestDB;
            return String.Concat(adress, db);
        }
    }
}






//public static string connectionTestDB { get; set; } = "Data Source=10.0.0.60;User ID=workshop;Password=********;Initial Catalog=WorkshopTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//public static string connectionTestDB { get; set; } = "Data Source=10.0.0.60;User ID=workshop;Password=********;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//public static string connectionTestDB { get; set; } = "Data Source=.\\SQLEXPRESS;Initial Catalog=WorkshopTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";