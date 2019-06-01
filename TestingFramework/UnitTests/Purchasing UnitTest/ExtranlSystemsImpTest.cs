using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.External_Services;

namespace TestingFramework.UnitTests.Purchasing_UnitTest
{
    [TestClass()]
    public class ExtranlSystemsImpTest
    {
        [TestInitialize]
        public void Init()
        {
            //this method is called ONCE before the test run
        }

        [TestCleanup]
        public void Cleanup()
        {
            //this method is called ONCE AFTER the test run
        }

        [TestMethod()]
        [TestCategory("external systems")]
        public async Task test()
        {
            ExternalSystemConnection connection= null;
            try
            {
                try
                {
                    connection = new ExternalSystemConnection();
                    Assert.IsTrue(true);
                }
                catch
                {
                    Assert.IsTrue(false, "failed handshake");
                }

                //payment test
                int transactionId = -1;
                try
                {
                    string cardNumber = "2222333344445555";
                    int month = 4;
                    int year = 2021;
                    string holder = "IsraelIsraelovice";
                    int ccv = 226;
                    int id = 20444444;
                    transactionId = await connection.payment(cardNumber, month, year, holder, ccv, id);
                    Assert.IsTrue(transactionId > -1, "failed payment");
                }
                catch
                {
                    Assert.IsTrue(false, "Payment exception");
                }

                // supply test
                int supplyId = -1;
                try
                {
                    if (transactionId > -1)
                    {
                        string name = "IsraelIsraelovice";
                        string address = "RagerBlvd12";
                        string city = "BeerSheva";
                        string country = "Israel";
                        string zip = "8458527";
                        supplyId = await connection.supply(name, address, city, country, zip);
                        Assert.IsTrue(supplyId > -1, "supply failed");
                    }
                }
                catch
                {
                    Assert.IsTrue(false, "Supply exception");
                }

                //cancel supply test
                try
                {
                    bool ans = await connection.cancelSupply(supplyId);
                    Assert.IsTrue(ans, "failed cancel supply");
                }
                catch
                {
                    Assert.IsTrue(false, "Cancel Supply exception");
                }

                //cancel payment
                try
                {
                    bool ans = await connection.cancelPayment(transactionId);
                    Assert.IsTrue(ans, "failed cancel paymnet");
                }
                catch
                {
                    Assert.IsTrue(false, "Cancel Payment exception");
                }

            }
            finally
            {
                if (connection !=null)
                    connection.Dispose();
            }
        }

    }
}
