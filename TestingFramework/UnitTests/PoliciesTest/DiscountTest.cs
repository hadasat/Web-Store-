using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    [TestClass]
    public class DiscountTest
    {
        [TestMethod]
        [TestCategory("POLICIES")]
        public void DiscountApplyTest()
        {
        }

        [TestMethod]
        [TestCategory("POLICIES")]
        public void checkDiscountTest()
        {
            int currId = Discount.DiscountCounter;
            Discount.checkDiscount(new Discount(new TrueCondition(),new FreeProduct(1,1)));
            int newId = Discount.DiscountCounter;
            Assert.IsTrue(currId < newId);
        }

       
    }
}
