using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password;
using Managment;
using WorkshopProject;
using Shopping;
using WorkshopProject.Log;
using WorkshopProject.Communication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users
{

    



    public class User : Permissions
    {
        [Key]
        public int id { get; set; }

        public ShoppingBasket shoppingBasket;


        public User()
        {
            this.shoppingBasket = new ShoppingBasket();
            Logger.Log("file", logLevel.INFO, "New user been created");
        }

        public virtual bool hasAddRemoveDiscountPermission(Store store)
        {
            return false;
        }

        public virtual bool hasAddRemoveDiscountPolicies(Store store)
        {
            return false;
        }

        public virtual bool hasAddRemoveProductsPermission(Store store)
        {
            return false;
        }


        public virtual bool hasAddRemovePurchasingPolicies(Store store)
        {
            return false;
        }

        public virtual bool hasAddRemoveStorePolicies(Store sore)
        {
            return false;
        }


        public virtual bool hasAddRemoveStoreManagerPermission(Store store)
        {
            return false;
        }


        /*** SERVICE LAYER FUNCTIONS***/

        public Member loginMember(string username, string password)
        {
            //TODO:: load shopping basket


            int ID = ConnectionStubTemp.identifyUser(username, password);
            if (ID == -1)
            {
                //don't log password!
                Logger.Log("file", logLevel.INFO, "user: " + username + "tried to log in and failed");
                throw new Exception("incorrect username or password");
            }
            Logger.Log("file", logLevel.INFO, "user: " + username + "log in and succses");
            return ConnectionStubTemp.getMember(ID);

        }

        public void registerNewUser(string username, string password)
        {
            throw new Exception("can't register a new user without country or password");
            //ConnectionStubTemp.registerNewUser(username, password, DateTime.Today.AddYears(1), "all");
        }

        public void registerNewUser(string username, string password, DateTime birthdate, string country)
        {
            if (country == null || country == "" || birthdate == DateTime.MaxValue)
            {
                throw new Exception("bad country of birth date - can't register user");
            }
            ConnectionStubTemp.registerNewUser(username, password, birthdate, country);
        }

        /****************************************************************/

    }


    interface Permissions
    {
        bool hasAddRemovePurchasingPolicies(Store store);

        bool hasAddRemoveStorePolicies(Store sore);

        bool hasAddRemoveDiscountPolicies(Store store);

        bool hasAddRemoveStoreManagerPermission(Store store);

        bool hasAddRemoveProductsPermission(Store store);
    }
}




    





