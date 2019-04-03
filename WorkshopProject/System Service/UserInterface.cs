using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.System_Service
{
    interface UserInterface
    {

        void logout(int user_id);
        Boolean login(string username, string hashPassword);
        Boolean buyProduct(int user_id, int shoppingBasket_id);
        Boolean createNewManager(int user_id, int newManager_id);
        Boolean removeManager(int user_id, int managerToRemove_id);
    }
}
