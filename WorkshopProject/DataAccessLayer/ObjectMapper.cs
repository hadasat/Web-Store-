using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.DataAccessLayer
{
    /// <summary>
    /// Holds a (weak) map of all object in memory.
    /// Equality of Keys is defined with:
    /// GetHashCode(), Equals()
    /// source: https://stackoverflow.com/questions/11562996/comparing-object-used-as-key-in-dictionary
    /// </summary>
    public class ObjectMapper
    {
        private ConditionalWeakTable<Integer, Member> Members;
        private ConditionalWeakTable<Integer, Store> Stores;
        private ConditionalWeakTable<Integer, Product> Products;

        public ObjectMapper()
        {
            Members = new ConditionalWeakTable<Integer, Member>();
            Stores = new ConditionalWeakTable<Integer, Store>();
            Products = new ConditionalWeakTable<Integer, Product>();
        }

        public bool AddMember(Member member)
        {
            return Add(new Integer(member.ID), member, Members);
        }

        public bool AddMember(Store store)
        {
            return Add(new Integer(store.id), store, Stores);
        }

        public bool AddMember(Product product)
        {
            return Add(new Integer(product.id), product, Products);
        }


        public Member TryGetMember(Integer id)
        {
            return TryGet(id, Members);
        }

        public Store TryGetStore(Integer id)
        {
            return TryGet(id, Stores);
        }

        public Product TryGetProducts(Integer id)
        {
            return TryGet(id, Products);
        }


        /// <summary>
        /// Adds the object to the give map.
        /// Fails if Key already exists in map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private bool Add<T>(Integer id, T obj, ConditionalWeakTable<Integer, T> map) where T : class
        {
            if (TryGet<T>(id, map) != null)
            {
                return false;
            }
            else
            {
                map.Add(id, obj);
                return true;
            }
        }

        private T TryGet<T>(Integer id, ConditionalWeakTable<Integer, T> map) where T : class
        {
            T ret;
            if (map.TryGetValue(id, out ret))
            {
                return ret;
            }
            return null;
        }
    }

    //https://social.msdn.microsoft.com/Forums/en-US/5bd74f79-4c7b-40c4-9623-082b66240ee6/is-there-a-integer-class-or-some-other-like-this-in-c?forum=csharplanguage
    public class Integer
    {
        int value = 0;

        public Integer(int value)
        {
            this.value = value;
        }

        public static implicit operator Integer(int value)
        {
            return new Integer(value);
        }

        public static implicit operator int(Integer integer)
        {
            return integer.value;
        }

        public static int operator +(Integer one, Integer two)
        {
            return one.value + two.value;
        }

        public static Integer operator +(int one, Integer two)
        {
            return new Integer(one + two);
        }

        public static int operator -(Integer one, Integer two)
        {
            return one.value - two.value;
        }

        public static Integer operator -(int one, Integer two)
        {
            return new Integer(one - two);
        }

        public override bool Equals(object obj)
        {
            //return base.Equals(obj);
            if(obj is Integer)
            {
                return this.value == ((Integer)obj).value;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            //return base.GetHashCode();
            return value;
        }
    }

    /*
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
    */
}
