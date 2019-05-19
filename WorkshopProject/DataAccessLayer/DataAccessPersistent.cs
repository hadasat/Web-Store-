using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.DataAccessLayer.Context;

namespace WorkshopProject.DataAccessLayer
{
    public class DataAccessPersistent : DataAccessNonPersistent
    {
        protected static WorkshopDBContext productionContext = new WorkshopProductionDBContext();
        protected static WorkshopDBContext testContext = new WorkshopProductionDBContext();

        protected override WorkshopDBContext getContext()
        {
            if (isProduction)
            {
                return productionContext;
            }
            else
            {
                return testContext;
            }
        }
    }
}
