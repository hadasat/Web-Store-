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
            return AddStoreManager(user, storeId, username, rolesOb);
        }

        public static bool AddStoreManager(User user, int storeId, string username, Roles roles)
        {
            if (roles == null)
                throw new Exception("illegal roles was enterd");
            ((Member)user).addManager(username, roles, storeId);
            return true;
        }


        private static Roles createRole(string roles)
        {
            return JsonHandler.DeserializeObject<Roles>(roles);
        }

        public static bool AddStoreOwner(User user, int storeId, string username)
        {
            Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true,true);
            if(!(user is Member))
                throw new Exception("user can't do this");

            ((Member)user).addStoreOwner(username, ownerRoles, storeId);

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


        public static List<StoreManager> GetRoles(User user)
        {
            if (!(user is Member))
            {
                throw new Exception("only members have roles");
            }

            List<StoreManager> roles = ((Member)user).storeManaging.ToList();

            return roles;
        }

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


        public static List<Member> GetAllMembers()
        {
            return ConnectionStubTemp.members.Values.ToList();
        }

        public static void SendMessage(int memberId, string message)
        {
            Member member = ConnectionStubTemp.getMember(memberId);
            member.addMessage(message);
        }

        public static List<string> GetMessages(int memberId)
        {
            //Member member = ConnectionStubTemp.getMember(memberId);
            //return member.notifications;
            return null;
        }
    }
}
