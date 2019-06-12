using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public abstract class IEntity
    {
        public abstract int GetKey();
        //{
        //    string propName = "id";
        //    return (int) this.GetType().GetProperty(propName).GetValue(this, null);
        //}

        public abstract void SetKey(int key);

        public virtual void Copy(IEntity other)
        {
            var myProperties = this.GetType().GetProperties();
            var otherProperties = other.GetType().GetProperties();

            foreach (var myProperty in myProperties)
            {
                foreach (var otherProperty in otherProperties)
                {
                    if (myProperty.Name == otherProperty.Name && otherProperty.PropertyType == myProperty.PropertyType)
                    {
                        myProperty.SetValue(this, otherProperty.GetValue(this), null);

                       //myProperty.SetValue(this, otherProperty.GetValue(otherProperty, new object[] { }), new object[] { });
                        break;
                    }
                }
            }

        }

        public abstract void LoadMe();
    }
}




/*
https://www.pluralsight.com/guides/property-copying-between-two-objects-using-reflection

    public class PropertyCopier<TParent, TChild> where TParent : class
                                            where TChild : class
{
    public static void Copy(TParent parent, TChild child)
    {
        var parentProperties = parent.GetType().GetProperties();
        var childProperties = child.GetType().GetProperties();

        foreach (var parentProperty in parentProperties)
        {
            foreach (var childProperty in childProperties)
            {
                if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                {
                    childProperty.SetValue(child, parentProperty.GetValue(parent));
                    break;
                }
            }
        }
    }
}


    */
