using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer.Context;

namespace WorkshopProject.DataAccessLayer
{
    public class DataAccess
    {
        private bool isProduction; 

        public DataAccess() : this(false)
        {
        }

        public DataAccess(bool isProduction)
        {
            this.isProduction = isProduction;
            //productionContext = new WorkshopProductionDBContext();
            //testContext = new WorkshopTestDBContext();
        }

        /// <summary>
        /// Sets the mode of the DataAccess.
        /// True => Production
        /// False => Test
        /// </summary>
        /// <param name="isProduction"></param>
        public void SetMode(bool isProduction)
        {
            this.isProduction = isProduction;
        }

        /// <summary>
        /// Gets the mode of the DataAccess.
        /// True => Production
        /// False => Test
        /// </summary>
        /// <param name="isProduction"></param>
        public bool GetMode()
        {
            return isProduction;
        }

        /// <summary>
        /// Saves an object in the database
        /// </summary>
        /// <exception cref="System.Exception">Thrown when...</exception>
        /// <param name="isProduction"></param>
        public bool Save(Member member)
        {
            return true;
        }

        public Member GetMember(int id)
        {


            return null;
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
