using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.System_Service
{
    public class UserService
    {
        internal User user;
    
        public UserService(User user)
        {
            this.user = user;
        }

        internal string AddStoreManager(int storeId, string user, string roles)
        {
            throw new NotImplementedException();
        }

        internal string AddStoreOwner(int storeId, string user)
        {
            throw new NotImplementedException();
        }

        internal string login(string username, string password)
        {

            throw new NotImplementedException();
        }

        internal string logout()
        {
            throw new NotImplementedException();
        }

        internal string Register(string user, string password)
        {
            throw new NotImplementedException();
        }

        internal string RemoveStoreManager(int storeId, string user)
        {
            throw new NotImplementedException();
        }

        internal string RemoveUser(string user)
        {
            throw new NotImplementedException();
        }
    }
}
