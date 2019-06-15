using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.System_Service;

namespace TestingFramework
{
    [TestClass]
    public class SetupAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            DataAccessDriver.setProduction(false);
            DataAccessDriver.setLocal(false);
            new Repo().Delete();  
        
        }
    }
}
