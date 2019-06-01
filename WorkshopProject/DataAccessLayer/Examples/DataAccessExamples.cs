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
    public static class DataAccessExamples
    {
        
        //public static void main()
        //{
        //    IDataAccess dal = DataAccessDriver.GetDataAccess();



        //    Member member = new Member();
        //    member.username = "DataAccessExample";

        //    /**SAVING THE NEW MEMBER**/
        //    //Hover over the "IDataAccess" to see a list of all possible exceptions
        //    try
        //    {
        //        dal.SaveEntity(member, member.id);
        //    }
        //    catch (DbUpdateConcurrencyException concurrencyException)
        //    {
        //        //someone already updated the object before you. 
        //        //you must get the object again
        //        Console.WriteLine(concurrencyException.Message);
        //        return;
        //    }
        //    catch (SqlException sqlException)
        //    {
        //        //maybe a connection error
        //        //see below message example of connection issues
        //        Console.WriteLine(sqlException.Message);
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return;
        //    }

        //    //now the DB has given us a primary key for the member
        //    int memberId = member.id;






        //    /**CHANGING MEMBER ATTRIBUTES**/
        //    member.username = "DataAccessExample2";
        //    try
        //    {
        //        dal.SaveEntity(member, member.id);
        //    }
        //    catch (DbUpdateConcurrencyException concurrencyException)
        //    {
        //        //someone already updated the object before you. 
        //        //you must get the object again
        //        Console.WriteLine(concurrencyException.Message);
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return;
        //    }



        //    /**GETTING THE MEMBER**/
        //    Member sameMember = dal.GetMember(member.id);



        //    /**SQL QUERIES**/
        //    string nameToSearch = "DataAccessExample2";
        //    string sql = "select * from Members where username = @name";
        //    SqlParameter sqlparam = new SqlParameter("@name", nameToSearch);
        //    DbRawSqlQuery<Member> query = dal.SqlQuery<Member>(sql, sqlparam);
        //    Member memberQuery = query.FirstOrDefault();
        //}
    

    }

    /*
    //This is what returns when we get a connection failure:

    TestingFramework.UnitTests.DataAccessTests.DataAccessTest.GetMemberTest threw exception: 
    System.Data.SqlClient.SqlException: A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server) ---> System.ComponentModel.Win32Exception: The network path was not found

    */

}
