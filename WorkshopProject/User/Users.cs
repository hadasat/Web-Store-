using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password;



namespace Users
{

    public static class ConnectionStubTemp
    {

        public static Password.Password pHandler = new Password.Password();

        public static bool identifyUser(string username, string password)
        {
            pHandler.hashPassword(password);
            return true;
        }

        public static bool registerNewUser(string username, string password)
        {
            pHandler.hashPassword(password);
            return true;
        }

        public static Member getMember(string username)
        {
            if (isAnAdmin(username))
                return new SystemAdmin(username);
            else
                return new Member(username);
        }

        public static void logout(string username)
        {
            
        }

        public static bool isAnAdmin(string username)
        {
            return true;
        }

        public static bool removeUser(string username, Member sy)
        {
            if (sy is SystemAdmin)
                return true;
            return false;
        }

    }



    public class User
    {

        public User()
        {

        }

        public Member loginMember(string username, string password)
        {
            bool tryToRegister = ConnectionStubTemp.identifyUser(username, password);
            if (tryToRegister)
                return ConnectionStubTemp.getMember(username);
            else
                return null;
        }

        public void registerNewUser(string username, string password)
        {
            ConnectionStubTemp.registerNewUser(username, password);
        }

    }

    public class Member : User
    {
        public string ID; //why do we need id?
        public string username;

        public Member(string username)
        {
            this.username = username;
        }

        public void logOut()
        {
            ConnectionStubTemp.logout(username);
        }
    }


    public class SystemAdmin : Member
    {
        public SystemAdmin(string username) : base(username)
        {

        }

        public bool RemoveUser(string userName)
        {
            return ConnectionStubTemp.removeUser(userName, this);
        }
    }
}
