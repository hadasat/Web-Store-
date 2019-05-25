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

namespace Users
{
    public class SystemAdmin : Member
    {
        public SystemAdmin() : base() { } /* added for DB */

        public SystemAdmin(string username, int ID) : base(username, ID) { }

        public SystemAdmin(string username, int ID, DateTime birthdate, string country) : base(username, ID, birthdate, country) { }

        public bool RemoveUser(string userName)
        {
            Member member;
            try
            {
                member = ConnectionStubTemp.getMember(userName);
            }
            catch (Exception ex)
            {
                Logger.Log("file", logLevel.INFO, "Admin fail removed user: " + userName + " user doen't exist");
                throw new Exception(ex.ToString());
            }
            if (member.isStoresManagers())
            {
                foreach (StoreManager st in storeManaging)
                {
                    if (st.GetFather() == null)///change to super father
                    {
                        Store store = st.GetStore();
                        WorkShop.closeStore(store.id, member);
                    }
                    else
                    {
                        StoreManager father = st.GetFather();
                        father.removeManager(st);
                    }
                }
                ConnectionStubTemp.removeMember(member);
                Logger.Log("file", logLevel.INFO, "Admin succesfully removed user: " + userName);
                return ConnectionStubTemp.removeUser(userName, this);
            }
            else
            {
                Logger.Log("file", logLevel.INFO, "Admin succesfully removed user: " + userName);
                ConnectionStubTemp.removeMember(member);
                return ConnectionStubTemp.removeUser(userName, this);
            }

        }
    }



}

