using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkshopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.System_Service;

namespace OwnershipRequest.Test
{
    [TestClass()]
    public class TestStoreManager
    {
        Member firstStoreOwner, SecondStoreOwner, ThirdStoreOwner, LastStoreOwner;
        GodObject god = new GodObject();
        int ownerId1;
        int ownerId2;
        int ownerId3;
        int ownerId4;
        int storeId;
        Store store;

        //[TestInitialize]
        public void Init()
        {
            ownerId1 = god.addMember("username1", "password1");
            ownerId2 = god.addMember("username2", "password2");
            ownerId3 = god.addMember("username3", "password3");
            ownerId4 = god.addMember("username4", "password4");
            firstStoreOwner = ConnectionStubTemp.getMember(ownerId1);
            SecondStoreOwner = ConnectionStubTemp.getMember(ownerId2);
            ThirdStoreOwner = ConnectionStubTemp.getMember(ownerId3);
            LastStoreOwner = ConnectionStubTemp.getMember(ownerId4);

            storeId = WorkShop.createNewStore("best shop", 1, true, firstStoreOwner);
            store = WorkShop.getStore(storeId);
        }

        //[TestCleanup]
        public void Cealup()
        {
            god.removeMember(ownerId1);
            god.removeMember(ownerId2);
            god.removeMember(ownerId3);
            god.removeMember(ownerId4);
            try
            {
                //ConnectionStubTemp.removeMember(firstStoreOwner);
                //ConnectionStubTemp.removeMember(SecondStoreOwner);
                //ConnectionStubTemp.removeMember(ThirdStoreOwner);
                //ConnectionStubTemp.removeMember(LastStoreOwner);
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod()]
        [TestCategory("Ownership_Request")]
        public void OwnershiprequestJust1_test()
        {
            Init();
            
            ConnectionStubTemp.createOwnershipRequest(store, firstStoreOwner, SecondStoreOwner);
            Assert.IsTrue(SecondStoreOwner.isStoresOwner(storeId));
            Cealup();
        }
        
        [TestMethod()]
        [TestCategory("Ownership_Request")]
        public void OwnershiprequestmoreThan1_test()
        {
            Init();
            int requestId1, requestId2;
            ConnectionStubTemp.createOwnershipRequest(store, firstStoreOwner, SecondStoreOwner);
            Assert.IsTrue(SecondStoreOwner.isStoresOwner(storeId));

            requestId1 = ConnectionStubTemp.createOwnershipRequest(store, firstStoreOwner, ThirdStoreOwner);
            SecondStoreOwner.approveOwnershipRequest(requestId1);
            Assert.IsTrue(ThirdStoreOwner.isStoresOwner(storeId));

            requestId2 = requestId1 = ConnectionStubTemp.createOwnershipRequest(store, ThirdStoreOwner, LastStoreOwner);
            firstStoreOwner.approveOwnershipRequest(requestId2);
            SecondStoreOwner.approveOwnershipRequest(requestId2);
            Assert.IsTrue(LastStoreOwner.isStoresOwner(storeId));
            Cealup();
        }

    
        
        [TestMethod()]
        [TestCategory("Ownership_Request")]
        public void OwnershiprequestmoreThan1Disapproved_test()
        {
            Init();
            int requestId1, requestId2, requestId3;
            ConnectionStubTemp.createOwnershipRequest(store, firstStoreOwner, SecondStoreOwner);
            Assert.IsTrue(SecondStoreOwner.isStoresOwner(storeId));

            requestId1 = ConnectionStubTemp.createOwnershipRequest(store, firstStoreOwner, ThirdStoreOwner);
            SecondStoreOwner.disapproveOwnershipRequest(requestId1);
            Assert.IsFalse(ThirdStoreOwner.isStoresOwner(storeId));

            requestId3 = ConnectionStubTemp.createOwnershipRequest(store, firstStoreOwner, ThirdStoreOwner);
            SecondStoreOwner.approveOwnershipRequest(requestId3);
            Assert.IsTrue(ThirdStoreOwner.isStoresOwner(storeId));

            requestId2 = requestId1 = ConnectionStubTemp.createOwnershipRequest(store, ThirdStoreOwner, LastStoreOwner);
            firstStoreOwner.approveOwnershipRequest(requestId2);
            SecondStoreOwner.disapproveOwnershipRequest(requestId2);
            Assert.IsFalse(LastStoreOwner.isStoresOwner(storeId));
            Cealup();
        }
        

    }


}
