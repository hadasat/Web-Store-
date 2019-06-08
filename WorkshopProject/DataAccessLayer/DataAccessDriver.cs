using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public static class DataAccessDriver
    {
        private static string remoteAdress = "Data Source=10.0.0.60;User ID = workshop; Password=********";
        private static string localAdress = "Data Source=.\\SQLEXPRESS;";
        private static string TestDB = "Initial Catalog=WorkshopTestDB;";
        private static string ProductionDB = "Initial Catalog=WorkshopProductionDB;";

        private static bool Persistent = true;
        private static bool Local = true;
        private static bool Production = false;
        public static bool UseStub = false;

        private static string connectionString = "Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        private static WorkshopDBContext ctx;

        public static void setPersistent(bool val)
        {
            if(Persistent != val)
            {
                ctx.SaveChanges();
                ctx.Dispose();
                ctx = null;
                Persistent = val;
            } 
        }

        public static void setLocal(bool val)
        {
            if (Local != val)
            {
                ctx.SaveChanges();
                ctx.Dispose();
                ctx = null;
                Local = val;
            }
        }

        public static void setProduction(bool val)
        {
            if (Production != val)
            {
                ctx.SaveChanges();
                ctx.Dispose();
                ctx = null;
                Production = val;
            }
        }

        public static WorkshopDBContext getContext()
        {

            if (Persistent)
            {
                if (ctx == null)
                {
                    ctx = new WorkshopTestDBContext(getConnectionString());
                }
                return ctx;
            }
            else
            {
                return new WorkshopTestDBContext(getConnectionString());
            }
        }

        public static string getConnectionString()
        {
            string address = Local ? localAdress : remoteAdress;
            string db = Production ? ProductionDB : TestDB;
            return String.Concat(address, db, connectionString);
        }


        /**STUB**/

        public static void clearStub()
        {
            Members.Delete();
            Stores.Delete();
            Passwords.Delete();
        }
        
        public static DbListStub<Users.Member> Members = new DbListStub<Users.Member>();
        public static DbListStub<Store> Stores = new DbListStub<Store>();
        public static DbListStub<Password.Password> Passwords = new DbListStub<Password.Password>();


    }
}






//public static string connectionTestDB { get; set; } = "Data Source=10.0.0.60;User ID=workshop;Password=********;Initial Catalog=WorkshopTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//public static string connectionTestDB { get; set; } = "Data Source=10.0.0.60;User ID=workshop;Password=********;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//public static string connectionTestDB { get; set; } = "Data Source=.\\SQLEXPRESS;Initial Catalog=WorkshopTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";




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