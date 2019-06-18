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

        [TestMethod]
        [TestCategory("POLICIES")]
        [TestCategory("Regretion")]
        public void checkConsistency()
        {
            int min = 10, max = 5;
            TrueCondition trueCondition = new TrueCondition();
            FalseCondition falseCondition = new FalseCondition();
            Assert.IsFalse(trueCondition.checkConsistent(falseCondition),"1");
            Assert.IsFalse(falseCondition.checkConsistent(trueCondition),"2");

            MinAmount minAmount = new MinAmount(min, new AllProductsFilter());
            MaxAmount maxAmount = new MaxAmount(max, new AllProductsFilter());
            Assert.IsFalse(minAmount.checkConsistent(maxAmount),"3");
            Assert.IsFalse(maxAmount.checkConsistent(minAmount),"4");

            List<IBooleanExpression> list = new List<IBooleanExpression>();
            list.Add(trueCondition);
            list.Add(minAmount);

            Assert.IsFalse(IBooleanExpression.confirmListConsist(falseCondition, list));
            Assert.IsFalse(IBooleanExpression.confirmListConsist(maxAmount, list));

            List<int> list1 = new List<int>();
            list1.Add(1);
            List<int> list2 = new List<int>();
            list2.Add(2);
            ProductListFilter f1 = new ProductListFilter(list1);
            ProductListFilter f2 = new ProductListFilter(list2);
            MinAmount minAmount2 = new MinAmount(min, f1);
            MaxAmount maxAmount2 = new MaxAmount(max, f2);

            Assert.IsFalse(maxAmount.checkConsistent(minAmount2));
            Assert.IsTrue(maxAmount2.checkConsistent(minAmount2));
            f2.productIds = list1;
            Assert.IsFalse(maxAmount2.checkConsistent(minAmount2));

            minAmount.amount = max;
            maxAmount.amount = min;
            Assert.IsTrue(minAmount.checkConsistent(maxAmount), "5");
        }

    }
}