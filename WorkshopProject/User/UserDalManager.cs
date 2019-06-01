using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password;
using Managment;
using WorkshopProject;
using Shopping;
using WorkshopProject.Log;
using WorkshopProject.Communication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkshopProject.DataAccessLayer;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;


namespace Users
{
    class UsersDalManager
    {
        public static IDataAccess dal = DataAccessDriver.GetDataAccess();

        public static bool createMember(Member member)
        {
            /**SAVING THE NEW MEMBER**/
            //Hover over the "IDataAccess" to see a list of all possible exceptions
            try
            {
                dal.SaveEntity(member, member.id);
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                //someone already updated the object before you. 
                //you must get the object again
                return false;
            }
            catch (SqlException sqlException)
            {
                //maybe a connection error
                //see below message example of connection issues
                throw new Exception(sqlException.Message);

            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static bool unactivateMember(Member member)
        {
            member.active = false;
            try
            {
                dal.SaveEntity(member, member.id);
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                //someone already updated the object before you. 
                //you must get the object again
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /*
        public static bool getAllMemebers()
        {
            string sql = "select * from Members";
            DbRawSqlQuery<LinkedList<Member>> query = dal.SqlQuery<LinkedList<Member>>(sql);
        }*/
    }
}
