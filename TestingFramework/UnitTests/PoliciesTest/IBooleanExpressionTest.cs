using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    /// <summary>
    /// Implements the Composite design pattern.
    /// This is essentially the "Purchasing Policy"
    /// </summary>
    /// 

    [TestClass]
    public class IBooleanExpressionTest
    {
        [TestMethod]
        [TestCategory("POLICIES")]

        public void checkExpressionTest() {
            int currId = IBooleanExpression.Idcounter;
            IBooleanExpression.checkExpression(new TrueCondition());
            int newId = IBooleanExpression.Idcounter;
            Assert.IsTrue(currId < newId);
        }

    }
}