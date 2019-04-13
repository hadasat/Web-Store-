using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Users;
using Managment;
using Password;

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
        internal string AddStoreManager(int storeId, string username, string roles)
        {
            Roles rolesOb = createRole(roles);
            if(rolesOb == null)
                return generateMessageFormatJason("illegal roles was enterd");
            try
            {
                ((Member)user).addManager(username, rolesOb, storeId);
            } catch (Exception exception)
            {
                generateMessageFormatJason(exception.ToString());
            }
            return successJason();
        }

        private Roles createRole(string roles)
        {
            bool b1, b2, b3, b4, b5, b6, b7, b8;
            if (roles.Length != 8) return null; 
            if (roles[0] == 't') b1 = true; else if (roles[0] == 'f') b1 = false; else return null;
            if (roles[1] == 't') b2 = true; else if (roles[1] == 'f') b2 = false; else return null;
            if (roles[2] == 't') b3 = true; else if (roles[2] == 'f') b3 = false; else return null;
            if (roles[3] == 't') b4 = true; else if (roles[3] == 'f') b4 = false; else return null;
            if (roles[4] == 't') b5 = true; else if (roles[4] == 'f') b5 = false; else return null;
            if (roles[5] == 't') b6 = true; else if (roles[5] == 'f') b6 = false; else return null;
            if (roles[6] == 't') b7 = true; else if (roles[6] == 'f') b7 = false; else return null;
            if (roles[7] == 't') b8 = true; else if (roles[7] == 'f') b8 = false; else return null;
            return new Roles(b1, b2, b3, b4, b5, b6, b7, b8);
        }

        internal string AddStoreOwner(int storeId, string username)
        {
            Roles ownerRoles = new Roles(true, true, true, true, true, true, true, true);
            if(!(user is Member))
                return generateMessageFormatJason("user can't do this");
            try
            {
                ((Member)user).addManager(username, ownerRoles, storeId);
            } catch(Exception exception)
            {
                return generateMessageFormatJason(exception.ToString());
            }
            return successJason();
        }

        internal string login(string username, string password)
        {
            try
            {
                Member member = user.loginMember(username, password);
                user = member;
            }
            catch (Exception exception)
            {
                //TODO change return format
                return generateMessageFormatJason(exception.ToString());
            }
            return successJason();
        }

        internal string logout()
        {
            if (!(user is Member))
                return generateMessageFormatJason("user can't do this");
            try
            {
                ((Member)user).logOut();
            }
            catch (Exception exception)
            {
                return generateMessageFormatJason(exception.ToString());
            }
            return successJason();
        }

        internal string Register(string username, string password)
        {
            try
            {
                user.registerNewUser(username, password);
            }
            catch (Exception exception)
            {
                return generateMessageFormatJason(exception.ToString());
            }
            
            return successJason();
        }

        internal string RemoveStoreManager(int storeId, string username)
        {
            if (!(user is Member))
                return generateMessageFormatJason("user can't do this");
            try
            {
                ((Member)user).removeManager(username, storeId);
            }
            catch (Exception exception)
            {
                return generateMessageFormatJason(exception.ToString());
            }
            return successJason();
        }

        internal string RemoveUser(string username)
        {
            if (!(user is SystemAdmin))
                return generateMessageFormatJason("user or member can't do this");
            try
            {
                ((SystemAdmin)user).RemoveUser(username);
            }
            catch (Exception exception)
            {
                return generateMessageFormatJason(exception.ToString());
            }
            return successJason();
        }


        private string generateMessageFormatJason(string message)
        {
            return JsonConvert.SerializeObject(new Message(message));
        }

        private string successJason()
        {
            return JsonConvert.SerializeObject(new Message("Success"));
        }
    }
}
