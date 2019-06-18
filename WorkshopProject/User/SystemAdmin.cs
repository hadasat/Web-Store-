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
using WorkshopProject.DataAccessLayer;

namespace Users
{
    public class SystemAdmin : Member
    {
        public SystemAdmin() : base() { } /* added for DB */

        public SystemAdmin(string username) : base(username) { }

        public SystemAdmin(string username, DateTime birthdate, string country) : base(username, birthdate, country) { }
        
        public SystemAdmin(Member member)
        {
            this.username = member.username;
            this.birthdate = member.birthdate;
            this.country = member.country;
            this.id = member.id;
            this.storeManaging = member.storeManaging;
            this.notifications = member.notifications;
            this.observers = member.observers;
            this.notificationLock = member.notificationLock;
        }

        public bool RemoveUser(string userName)
        {
            Member member;
            try
            {
                member = ConnectionStubTemp.getMember(userName);
            }
            catch (WorkShopDbException dbExc)
            {
                throw dbExc;
            }
            catch (Exception ex)
            {
                Logger.Log("event", logLevel.INFO, "Admin fail removed user: " + userName + " user doen't exist");
                throw new Exception(ex.ToString());
            }
            if (member.isStoresManagers())
            {
                int count = member.storeManaging.Count-1;
                StoreManager st = member.storeManaging.ElementAt(count);
                try
                {
                    for (; count > -1; count--, st = member.storeManaging.ElementAt(count))
                    {
                        if (ConnectionStubTemp.getAllOwnersCount(st.store) == 1/*st.GetFather() == null*/)///change to super father
                        {
                            Store store = st.GetStore();
                            st.removeManagerAsAdminOwner(st);
                            WorkShop.closeStore(store.id, member);
                        }
                        else
                        {
                            StoreManager father = st.GetFather();
                            father.removeManagerAsAdmin(st);
                        }
                    }
                } catch (Exception ex)
                {

                }
                ConnectionStubTemp.removeMember(member);
                Logger.Log("event", logLevel.INFO, "Admin succesfully removed user: " + userName);
                return ConnectionStubTemp.removeUser(userName, this);
            }
            else
            {
                Logger.Log("event", logLevel.INFO, "Admin succesfully removed user: " + userName);
                ConnectionStubTemp.removeMember(member);
                return ConnectionStubTemp.removeUser(userName, this);
            }

        }

        public override bool isAdmin()
        {
            return true;
        }
    }



}

