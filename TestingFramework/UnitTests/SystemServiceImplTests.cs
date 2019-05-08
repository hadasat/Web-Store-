using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.System_Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using Users;
using Managment;

namespace TestingFramework.UnitTests
{
    [TestClass]
    public class SystemServiceImplTests
    {
        IGodObject godObject = new GodObject();
        UserInterface service = new SystemServiceImpl();
        string userName = "username";
        string password = "passwrod";
        int storeOwner1Id;
        int storeId;

        [TestInitialize]
        public void Init()
        {
            storeOwner1Id = godObject.addMember(userName, password);
            storeId = godObject.addStore("store", 5, storeOwner1Id);
            service.login(userName, password);
        }

        [TestCleanup]
        public void Cleanup()
        {
            godObject.cleanUpAllData();
            service.logout();
        }

        [TestMethod]
        public void GetStoreTest()
        {
            string result = service.GetStore(storeId);
            Store store = JsonHandler.DeserializeObject<Store>(result);
            Assert.AreEqual(store.id, storeId);
        }

        [TestMethod]
        public void GetAllStoresTest()
        {
            string result = service.GetAllStores();
            List<Store> stores = JsonHandler.DeserializeObject<List<Store>>(result);
            Assert.AreEqual(stores.Count, 1);
        }

        [TestMethod]
        public void GetAllManagersTest()
        {
            string result = service.GetAllManagers(storeId);
            List<Member> managers = JsonHandler.DeserializeObject<List<Member>>(result);
        }

        [TestMethod]
        public void GetRolesTest()
        {         
            string result = service.GetRoles();
            List<StoreManager> managers = JsonHandler.DeserializeObject<List<StoreManager>>(result);
        }

        [TestMethod]
        public void GetAllMembersTest()
        {
            string result = service.GetAllMembers();
            List<Member> managers = JsonHandler.DeserializeObject<List<Member>>(result);
        }

        [TestMethod]
        public void SendMessage()
        {
            string result = service.SendMessage(storeOwner1Id, "msg");
            string result2 = service.GetMessages(storeOwner1Id);
            List<string> msgs = JsonHandler.DeserializeObject<List<string>>(result2);
            Assert.AreEqual(msgs.Count, 1);
            Assert.AreEqual(msgs[0], "msg");
        }
    }
}
