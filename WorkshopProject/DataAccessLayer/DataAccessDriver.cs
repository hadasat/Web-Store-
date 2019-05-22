﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public static class DataAccessDriver
    {
        public static bool Persistent { get; set; } = true;
        public static string connectionTestDB { get; set; } = "Data Source=.\\SQLEXPRESS;Initial Catalog=WorkshopTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string connectionProductionDB { get; set; } = "Data Source=.\\SQLEXPRESS;Initial Catalog=WorkshopProductionDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static IDataAccess GetDataAccess()
        {
            if (Persistent)
            {
                return new DataAccessPersistent();
            }
            else
            {
                return new DataAccessNonPersistent();
            }       
        }
    
           
        
    }
}