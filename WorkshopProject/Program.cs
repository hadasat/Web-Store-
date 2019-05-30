using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TansactionsNameSpace;
using Users;
using WorkshopProject.Communication;
using WorkshopProject.DataAccessLayer.Context;
using WorkshopProject.DataAccessLayer.Examples;
using WorkshopProject.Examples;
using WorkshopProject.System_Service;


namespace WorkshopProject
{
    public class Program
    {
        static void Main(string[] args)
        {

            //LogExample.RunMe();

            //DataAccessExamples.main();

            //Console.ReadLine();
            //WorkshopDBContext ctx = new WorkshopProductionDBContext();
            //ctx.Members.Add(new Member());

            Setup();
            CommunicationManager manager = new CommunicationManager();
        }


        static void Setup()
        {
            string username = "username";
            string password = "password";
            string storename = "store";


            LoginProxy service = new LoginProxy();
            try
            {
                service.Register(username, password, DateTime.Now, "shit");
                Console.WriteLine("Registering: " + username + ":" + password);

                Member member = service.loginEx(username, password);
                int storeId = service.AddStore(storename);
                Console.WriteLine("Opening store: " + storename + ":" + storeId);

                int product1Id = service.AddProductToStore(storeId, "product1", "testProduct", 10.0, "test");
                service.AddProductToStock(storeId, product1Id, 10);
                Console.WriteLine("Adding product: " + "product1" + ":" + product1Id);

                int product2Id = service.AddProductToStore(storeId, "product2", "testProduct", 5.0, "test");
                service.AddProductToStock(storeId, product1Id, 5);
                Console.WriteLine("Adding product: " + "product2" + ":" + product2Id);

                service.logout();
                Console.WriteLine("Setup Done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

