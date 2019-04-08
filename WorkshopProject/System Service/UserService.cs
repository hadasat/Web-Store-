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
            
            throw new NotImplementedException();
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
                generateMessageFormatJason(exception.ToString());
            }
            return successJason();
        }

        internal string login(string username, string password)
        {
            try
            {
                user.loginMember(username, password);
            }
            catch (Exception exception)
            {
                generateMessageFormatJason(exception.ToString());
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
                generateMessageFormatJason(exception.ToString());
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
                generateMessageFormatJason(exception.ToString());
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
                generateMessageFormatJason(exception.ToString());
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
                generateMessageFormatJason(exception.ToString());
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
