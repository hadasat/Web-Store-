using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;
using WorkshopProject.Policies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.UnitTests.PoliciesJsonTests
{
    [TestClass]
    public class PoliciesJsonTests
    {
        [TestMethod]
        [TestCategory("JSON")]
        public void BooleanExpressionLeafSerializeTest()
        {
            ItemFilter filter = new CategoryFilter("cat");
            IBooleanExpression leaf = new MaxAmount(10, filter);

            string json = JsonConvert.SerializeObject(leaf, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            IBooleanExpression result = JsonConvert.DeserializeObject<IBooleanExpression>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            Assert.IsInstanceOfType(result, typeof(MaxAmount));
            Assert.AreEqual(((MaxAmount)result).amount, 10);
        }

        [TestMethod]
        [TestCategory("JSON")]
        public void BooleanExpressionComplexSerializeTest()
        {
            ItemFilter filter1 = new CategoryFilter("cat");
            IBooleanExpression leaf1 = new MaxAmount(10, filter1);
            ItemFilter filter2 = new AllProductsFilter();
            IBooleanExpression leaf2 = new UserCountry("Wakanda forever", filter2);
            IBooleanExpression complex = new XorExpression();
            leaf1.id = 1;
            leaf2.id = 2;
            complex.addChildren(leaf1, leaf2);

            string json = JsonConvert.SerializeObject(complex, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            IBooleanExpression result = JsonConvert.DeserializeObject<IBooleanExpression>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            Assert.IsInstanceOfType(result, typeof(XorExpression));
            Assert.IsInstanceOfType(((XorExpression)result).firstChild, typeof(MaxAmount));
            Assert.IsInstanceOfType(((XorExpression)result).secondChild, typeof(UserCountry));
        }


        [TestMethod]
        [TestCategory("JSON")]
        public void DiscountSerializeTest()
        {
            ItemFilter filter1 = new CategoryFilter("cat");
            IBooleanExpression leaf1 = new MaxAmount(10, filter1);
            ItemFilter filter2 = new AllProductsFilter();
            IBooleanExpression leaf2 = new UserCountry("Wakanda forever", filter2);
            IBooleanExpression complex = new XorExpression();
            //we add manually ids for the leaves becuase there is no stub for them at the moment. 
            //Not adding these ids will cause the json serializer to falsly think it has a loop (parent and children have the same id -> .Equals() = true)
            leaf2.id = 10;
            leaf1.id = 11;
            complex.addChildren(leaf1, leaf2);

            //TODO: when there are concrete Outcomes, we can test this
            //IOutcome outcome = new 
            //Discount discount = new Discount(complex,)



            string json = JsonConvert.SerializeObject(complex, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            IBooleanExpression result = JsonConvert.DeserializeObject<IBooleanExpression>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            Assert.IsInstanceOfType(result, typeof(XorExpression));
            Assert.IsInstanceOfType(((XorExpression)result).firstChild, typeof(MaxAmount));
            Assert.IsInstanceOfType(((XorExpression)result).secondChild, typeof(UserCountry));
        }



    }
}
