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

            string json = JsonConvert.SerializeObject(leaf, /*Formatting.Indented,*/ new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            IBooleanExpression result = JsonConvert.DeserializeObject<IBooleanExpression>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            Assert.IsInstanceOfType(result, typeof(MaxAmount));
        }

        [TestMethod]
        [TestCategory("JSON")]
        public void BooleanExpressionComplexSerializeTest()
        {
            string json = @"{
                'id': '1',
                'msg': 'abc',
                'arr': [{ 
                    'id': '2',
                    'msg': 'def'
                    }]
            }";
            JObject obj = JObject.Parse(json);
            int id = (int)obj["id"];
            string msg = (string)obj["msg"];
            Assert.AreEqual(id, 1);
            Assert.AreEqual(msg, "abc");

            JArray arr = (JArray)obj["arr"];
            id = (int)arr[0]["id"];
            msg = (string)arr[0]["msg"];
            Assert.AreEqual(id, 2);
            Assert.AreEqual(msg, "def");
        }

    }
}
