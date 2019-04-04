using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingFramework.UnitTests.JsonTests
{
    [TestClass]
    public class JsonTests
    {
        [TestMethod]
        [TestCategory("JSON")]
        public void test1()
        {
            string json = @"{
              'Name': 'Bad Boys',
              'ReleaseDate': '1995-4-7T00:00:00',
              'Genres': [
                'Action',
                'Comedy'
              ]
            }";
            dynamic obj = JObject.Parse(json);
            Assert.AreEqual(obj.Name, "Bad Boys");
        }


    }
}
