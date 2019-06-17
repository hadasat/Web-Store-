using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    [TestClass]
    public class ConcreteBooleanExpressionTest
    {
        AllProductsFilter filter = new AllProductsFilter();
        List<ProductAmountPrice> list = new List<ProductAmountPrice>();

        int amount = 10;
        Product p;
        ProductAmountPrice pam;

        Member user = new Member("hadas", DateTime.Today.AddYears(-20), "A");

        [TestInitialize]
        public void Init()
        {
            p = new Product("first", 10, "h", "g", 10, 10, 10);
            pam= new ProductAmountPrice(p, amount, 10);
            list.Add(pam); 
        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }

        [TestMethod]
        public void MaxAmountevaluateTest() {
            MaxAmount max = new MaxAmount(5, filter);
            Assert.IsFalse(max.evaluate(list, user));

            max = new MaxAmount(15, filter);
            Assert.IsTrue(max.evaluate(list, user));

        }

        [TestMethod]
        public void MinAmountevaluateTest() {
            MinAmount min = new MinAmount(15, filter);
            Assert.IsFalse(min.evaluate(list, user));

            min = new MinAmount(5, filter);
            Assert.IsTrue(min.evaluate(list, user));
        }

        [TestMethod]
        public void UserAgeevaluateTest() {
            UserAge age = new UserAge(15, filter);
            Assert.IsFalse(age.evaluate(list, user));

            age = new UserAge(25, filter);
            Assert.IsTrue(age.evaluate(list, user));
        }

        [TestMethod]
        public void UserCountryevaluateTest() {
            UserCountry country = new UserCountry("A", filter);
            Assert.IsFalse(country.evaluate(list, user));

            country = new UserCountry("B", filter);
            Assert.IsTrue(country.evaluate(list, user));
        }

        [TestMethod]
        public void TrueevaluateTest() {
            TrueCondition trueCondition = new TrueCondition();
            Assert.IsTrue(trueCondition.evaluate(list,user));
        }

        [TestMethod]
        public void FalseevaluateTest() {
            FalseCondition falseCondition = new FalseCondition();
            Assert.IsFalse(falseCondition.evaluate(list, user));
        }

        [TestMethod]
        public void ANDevaluateTest() {
            FalseCondition falseCondition = new FalseCondition();
            TrueCondition trueCondition = new TrueCondition();

            AndExpression and = new AndExpression();
            and.addChildren(trueCondition, trueCondition);
            Assert.IsTrue(and.evaluate(list, user));

            and.addChildren(falseCondition, trueCondition);
            Assert.IsFalse(and.evaluate(list, user));

            and.addChildren(trueCondition, falseCondition);
            Assert.IsFalse(and.evaluate(list, user));

            and.addChildren(falseCondition, falseCondition);
            Assert.IsFalse(and.evaluate(list, user));
        }

        [TestMethod]
        public void ORevaluateTest() {
            FalseCondition falseCondition = new FalseCondition();
            TrueCondition trueCondition = new TrueCondition();

            OrExpression or = new OrExpression();
            or.addChildren(falseCondition, trueCondition);
            Assert.IsTrue(or.evaluate(list, user));

            or.addChildren(trueCondition, falseCondition);
            Assert.IsTrue(or.evaluate(list, user));

            or.addChildren(trueCondition, trueCondition);
            Assert.IsTrue(or.evaluate(list, user));

            or.addChildren(falseCondition, falseCondition);
            Assert.IsFalse(or.evaluate(list, user));
        }

        [TestMethod]
        public void XORevaluateTest() {

            FalseCondition falseCondition = new FalseCondition();
            TrueCondition trueCondition = new TrueCondition();

            XorExpression xor = new XorExpression();
            xor.addChildren(trueCondition, trueCondition);
            Assert.IsFalse(xor.evaluate(list, user));

            xor.addChildren(trueCondition, falseCondition);
            Assert.IsTrue(xor.evaluate(list, user));

            xor.addChildren(falseCondition, trueCondition);
            Assert.IsTrue(xor.evaluate(list, user));

            xor.addChildren(falseCondition, falseCondition);
            Assert.IsFalse(xor.evaluate(list, user));
        }
    }
}