using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Users;
using Managment;
using Password;
using Newtonsoft.Json.Linq;

namespace WorkshopProject.System_Service
{
    public static class UserService
    {
        /// <summary>
        /// Role format will be string of 8 chars f or t examples: tttttttt, ttttffff, ffffffft
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool AddStoreManager(User user, int storeId, string username, string roles)
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
    
        private static Roles createRole(string roles)
        {
            JObject json = JObject.Parse(roles);
            bool addRemovePurchasing = (bool)json["addRemovePurchasing"];
            bool addRemoveDiscountPolicy = (bool)json["addRemoveDiscountPolicy"];
            bool addRemoveStoreManger = (bool)json["addRemoveStoreManger"];
            bool closeStore = (bool)json["closeStore"];

            return new Roles(true, addRemoveDiscountPolicy, addRemoveDiscountPolicy, true, closeStore, true, true, true);
        }

        public static bool AddStoreOwner(User user, int storeId, string username)
        {
            Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true);
            if(!(user is Member))
                throw new Exception("user can't do this");

            ((Member)user).addManager(username, ownerRoles, storeId);

            return true;
        }

        public static Member login(string username, string password,User user)
        {
            Member member = user.loginMember(username, password);
            //user = member;
            return member;
        }

        public static bool logout(User user)
        {
            if (!(user is Member))
                throw new Exception("user can't do this");

            ((Member)user).logOut();
            return true;
        }

        public static bool Register(string username, string password)
        {
            (new User()).registerNewUser(username, password);
            return true;
        }

        public static bool Register(string username, string password, DateTime birthdate, string country)
        {
            (new User()).registerNewUser(username, password, birthdate, country);
            return true;
        }

        public static bool RemoveStoreManager(User user, int storeId, string username)
        {
            if (!(user is Member))
                throw new Exception("user can't do this");

            ((Member)user).removeManager(username, storeId);
            return true;
        }

        public static bool RemoveUser(User user, string username)
        {
            if (!(user is SystemAdmin))
                throw new Exception("user or member can't do this");

            bool res = ((SystemAdmin)user).RemoveUser(username);
            return true;
        }


        //TODO: add unit test
        public static List<StoreManager> GetRoles(User user)
        {
            if (!(user is Member))
            {
                throw new Exception("only members have roles");
            }

            List<StoreManager> roles = ((Member)user).storeManaging.ToList();

            return roles;
        }

        //todo amsel add test
        /// <summary>
        /// gets the roles of the user for store with requested id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static Roles getRoleForStore (User user, int storeId)
        {
            if (!(user is Member))
            {
                return null;
            }

            return ((Member)user).getStoreManagerRoles(storeId);
        }

        //TODO: add unit test
        public static List<Member> GetAllMembers()
        {
            return ConnectionStubTemp.members.Values.ToList();
        }

        //TODO: add unit test
        public static void SendMessage(int memberId, string message)
        {
            Member member = ConnectionStubTemp.getMember(memberId);
            member.addMessage(message);
        }

        //TODO delte
        public static List<string> GetMessages(int memberId)
        {
            //Member member = ConnectionStubTemp.getMember(memberId);
            //return member.notifications;
            return null;
        }
    }
}
