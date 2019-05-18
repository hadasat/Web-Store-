using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    /// <summary>
    /// Holds a (weak) map of all object in memory.
    /// Equality of Keys is defined with:
    /// GetHashCode(), Equals()
    /// source: https://stackoverflow.com/questions/11562996/comparing-object-used-as-key-in-dictionary
    /// </summary>
    static class ObjectMapper
    {
        private static ConditionalWeakTable<object, ClassData> map;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static object TryAdd(object obj)
        {
            //object 

            return null;
        }

    }





    //https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.conditionalweaktable-2?redirectedfrom=MSDN&view=netframework-4.8
    internal class ClassData
    {
        public DateTime CreationTime;
        public object Data;

        public ClassData()
        {
            CreationTime = DateTime.Now;
            this.Data = new object();
        }
    }
}
