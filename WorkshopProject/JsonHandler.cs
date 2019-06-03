using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    /// <summary>
    /// This class is used to standerdize all the json calls, to keep the formats consistent
    /// </summary>
    public static class JsonHandler
    {
        private static Formatting format = Formatting.Indented;
        private static JsonSerializerSettings settings = new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All};

        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, format, settings);
        }

        public static string SerializeObjectDynamic(object value)
        {
            return JsonConvert.SerializeObject(value, format);
        }

        public static T DeserializeObject<T>(string json){
             return JsonConvert.DeserializeObject<T>(json, settings);
        }

    }
}
