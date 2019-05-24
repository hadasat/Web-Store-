using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.DataAccessLayer.Examples
{
    class DataAccessExamples
    {
        void main()
        {
            IDataAccess dal = DataAccessDriver.GetDataAccess();





            Member member = new Member();
            member.username = "DataAccessExample";

            /**SAVING THE NEW MEMBER**/
            //Hover over the "IDataAccess" to see a list of all possible exceptions
            try
            {
                dal.SaveMember(member);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                //someone already updated the object before you. 
                //you must get the object again
                Console.WriteLine(concurrencyException.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            //now the DB has given us a primary key for the member
            int memberId = member.id;






            /**CHANGING MEMBER ATTRIBUTES**/
            member.username = "DataAccessExample2";
            try
            {
                dal.SaveMember(member);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                //someone already updated the object before you. 
                //you must get the object again
                Console.WriteLine(concurrencyException.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }



            /**GETTING THE MEMBER**/
            Member sameMember = dal.GetMember(memberId);





            /**SQL QUERIES**/
            string nameToSearch = "DataAccessExample2";
            string sql = "select * from Members where username = @name";
            SqlParameter sqlparam = new SqlParameter("@name", nameToSearch);
            DbRawSqlQuery<Member> query = dal.SqlQuery<Member>(sql, sqlparam);
            Member memberQuery = query.FirstOrDefault();
        }

    }
}
