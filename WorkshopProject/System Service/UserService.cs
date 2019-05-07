using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Users;
using Managment;
using Password;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WorkshopProject.System_Service
{
    public class UserService
    {
        internal User user;
    
        public UserService(User user)
        {
            this.user = user;
        }
        /// <summary>
        /// Role format will be string of 8 chars f or t examples: tttttttt, ttttffff, ffffffft
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        internal bool AddStoreManager(int storeId, string username, string roles)
        {
            Roles rolesOb = createRole(roles);
            if(rolesOb == null)
                throw new Exception("illegal roles was enterd");
                ((Member)user).addManager(username, rolesOb, storeId);
            return true;
        }

        //private Roles createRole(string roles)
        //{
        //    bool b1, b2, b3, b4, b5, b6, b7, b8;
        //    if (roles.Length != 8) return null; 
        //    if (roles[0] == 't') b1 = true; else if (roles[0] == 'f') b1 = false; else return null;
        //    if (roles[1] == 't') b2 = true; else if (roles[1] == 'f') b2 = false; else return null;
        //    if (roles[2] == 't') b3 = true; else if (roles[2] == 'f') b3 = false; else return null;
        //    if (roles[3] == 't') b4 = true; else if (roles[3] == 'f') b4 = false; else return null;
        //    if (roles[4] == 't') b5 = true; else if (roles[4] == 'f') b5 = false; else return null;
        //    if (roles[5] == 't') b6 = true; else if (roles[5] == 'f') b6 = false; else return null;
        //    if (roles[6] == 't') b7 = true; else if (roles[6] == 'f') b7 = false; else return null;
        //    if (roles[7] == 't') b8 = true; else if (roles[7] == 'f') b8 = false; else return null;
        //    return new Roles(b1, b2, b3, b4, b5, b6, b7, b8);
        //}

        private Roles createRole(string roles)
        {
            JObject json = JObject.Parse(roles);
            bool addRemovePurchasing = (bool)json["addRemovePurchasing"];
            bool addRemoveDiscountPolicy = (bool)json["addRemoveDiscountPolicy"];
            bool addRemoveStoreManger = (bool)json["addRemoveStoreManger"];
            bool closeStore = (bool)json["closeStore"];

            return new Roles(true, addRemoveDiscountPolicy, addRemoveDiscountPolicy, true, closeStore, true, true, true);
        }

        internal bool AddStoreOwner(int storeId, string username)
        {
            Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true);
            if(!(user is Member))
                throw new Exception("user can't do this");

            ((Member)user).addManager(username, ownerRoles, storeId);

            return true;
        }

        internal bool login(string username, string password)
        {
            Member member = user.loginMember(username, password);
            user = member;
            return true;
        }

        internal bool logout()
        {
            if (!(user is Member))
                throw new Exception("user can't do this");

            ((Member)user).logOut();
            return true;
        }

        internal bool Register(string username, string password)
        {
            user.registerNewUser(username, password);
            return true;
        }

        internal bool Register(string username, string password, string country, int age)
        {
            user.registerNewUser(username, password, country, age);
            return true;
        }

        internal bool RemoveStoreManager(int storeId, string username)
        {
            if (!(user is Member))
                throw new Exception("user can't do this");

            ((Member)user).removeManager(username, storeId);
            return true;
        }

        internal bool RemoveUser(string username)
        {
            if (!(user is SystemAdmin))
                throw new Exception("user or member can't do this");

            bool res = ((SystemAdmin)user).RemoveUser(username);
            return true;
        }
    }
}
