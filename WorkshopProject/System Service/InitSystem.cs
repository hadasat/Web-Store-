using Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.System_Service
{
     public static class InitSystem
    {
        private static string filePath = "../../../hadas.txt";
        private static string word = @"\b[a-zA-Z1-9\-]+\b";
        private static string commandSyntax = @"^[a-zA-Z0-9\-]+\([a-zA-Z0-9]*(,[[a-zA-Z0-9]+)*\);";

        private static Dictionary<string, string> users;

        public static void updatePath(string new_path) { filePath = new_path; }

        public static string[] splitCommand(string sentence)
        {
            Regex rgxword = new Regex(word);
            List<string> args = new List<string>();
            foreach (Match match in rgxword.Matches(sentence))
            args.Add(match.Value);
            return args.ToArray();
        }

        public static void initSystem()
        {
            string[] lines = System.IO.File.ReadAllLines(@filePath);
            if (lines == null)
                return;
            foreach(string line in lines)
            {
                Regex rgx = new Regex(commandSyntax);
                if (rgx.IsMatch(line))
                    break;
                execute(line);
            }
        }

        public static void execute(string line)
        {
            string[] fullcommand = splitCommand(line);
            string command = fullcommand[0];
            switch (command)
            {
                case "register":
                    registerNewUser(fullcommand);
                    break;
                case "make-admin":
                    makeAdmin(fullcommand);
                    break;
                case "open-store":
                    openStore(fullcommand);
                    break;
                case "add-product-basket":
                    addProductBasket(fullcommand);
                    break;
                case "add-store-product":
                    addProductStore(fullcommand);
                    break;
                case "add-store-owner":
                    addStoreOwner(fullcommand);
                    break;
                case "login-user":
                    loginuser(fullcommand);
                    break;
            }
        }

        public static User getUser(string username, string password)
        {
            int userId = ConnectionStubTemp.identifyUser(username, password);
            User user = ConnectionStubTemp.getMember(userId);
            return user;
        }

        public static Store getStore(string name)
        {
            foreach(KeyValuePair<int,Store> s in WorkShop.stores)
            {
                Store store = s.Value;
                if (store.name.Equals(name))
                    return store;
            }
            return null;
        }

        public static void registerNewUser(string[] fullcommand)
        {
            if (fullcommand.Length == 4) {
                string username = fullcommand[0];
                string password = fullcommand[1];
                string country = fullcommand[2];
                string stringBirthdate = fullcommand[2];
                DateTime birthdate = DateTime.Parse(stringBirthdate);
                if (UserService.Register(username, password, birthdate, country))
                    users.Add(username, password);
            }
        }

        public static void makeAdmin(string[] fullcommand)
        {

        }

        public static void openStore(string[] fullcommand)
        {
            if (fullcommand.Length == 2)
            {
                string username = fullcommand[0];
                string storeName = fullcommand[1];
                try
                {
                    User user = getUser(username, users[username]);
                    StoreService.AddStore(user, storeName);
                }
                catch { }
            }
        }
        public static void addProductBasket(string[] fullcommand)
        {

        }

        public static void addProductStore(string[] fullcommand)
        {
            if (fullcommand.Length == 7)
            {
                string username = fullcommand[0];
                string storeName = fullcommand[1];
                string productName = fullcommand[2];
                string category = fullcommand[3];
                string desc = fullcommand[4];
                int price = int.Parse(fullcommand[5]);
                int amount = int.Parse(fullcommand[6]);
                try
                {
                    User user = getUser(username, users[username]);
                    Store store = getStore(storeName);
                    int storeId = store.id;
                    int productId = StoreService.AddProductToStore(user, storeId, productName, desc, price, category);

                    StoreService.AddProductToStock(user, storeId, productId, amount);
                }
                catch { }
            }
            
        }

        public static void addStoreOwner(string[] fullcommand)
        {

            if (fullcommand.Length == 3)
            {
                string username = fullcommand[0];
                string storeName = fullcommand[1];
                string newManager = fullcommand[2];
                
                try
                {
                    User user = getUser(username, users[username]);
                    Store store = getStore(storeName);
                    int storeId = store.id;

                    Roles roles = new Roles(true, true, true, false, false, false, false, false, true);
                    string sushiRole = JsonHandler.SerializeObject(roles);

                    UserService.AddStoreManager(user, storeId, newManager, sushiRole);
                }
                catch { }
            }
        }
        
        public static void loginuser(string[] fullcommand)
        {

        }
        
    }
}
