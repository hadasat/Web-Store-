using Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Users;
using WorkshopProject.Log;

namespace WorkshopProject.System_Service
{
     public static class InitSystem
    {
        private static string filePath = "../../initSystem.txt";
        private static string word = @"\b[a-zA-Z0-9\-\/\ ]+\b";
        private static string commandSyntax = @"^[a-zA-Z0-9\-]+\([a-zA-Z0-9]*(,[[a-zA-Z0-9]+)*\);";

        private static Dictionary<string, string> users = new Dictionary<string, string>();

        public static void updatePath(string new_path) { filePath = new_path; }

        public static string getUserPass(string username)
        {
            string pass;
            if (users.TryGetValue(username, out pass))
                return pass;
            return null;
        }

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
            foreach(Store store in WorkShop.GetStores())
            {
                //Store = s.Value;
                
                if (store.name.Equals(name))
                    return store;
            }
            return null;
        }

        public static Product getProduct(Store store, string name)
        {
            foreach (KeyValuePair<int, Product> s in store.GetStockAsDictionary())
            {
                Product product = s.Value;
                if (product.name.Equals(name))
                    return product;
            }
            return null;
        }

        public static void registerNewUser(string[] fullcommand)
        {
            if (fullcommand.Length == 5) {
                try
                {
                    string username = fullcommand[1];
                    string password = fullcommand[2];
                    string country = fullcommand[3];
                    string stringBirthdate = fullcommand[4];
                    DateTime birthdate = DateTime.Parse(stringBirthdate);
                    if (UserService.Register(username, password, birthdate, country))
                        users.Add(username, password);
                }
                catch (Exception e)
                {
                    Logger.Log("error", logLevel.ERROR,$"Init system: register user {e.Data}");
                }
            }
            else
                Logger.Log("error", logLevel.ERROR, "Init system: register user bad input");
        }

        public static void makeAdmin(string[] fullcommand)
        {
            if (fullcommand.Length == 2)
            {
                try
                {
                    string userName = fullcommand[1];
                    User user = getUser(userName, users[userName]);
                    UserService.MakeAdmin(((Member)user).id);
                }
                catch (Exception e)
                {
                    Logger.Log("error", logLevel.ERROR,$"Init system: make admin {e.Data}");
                }
            }
            else
                Logger.Log("error", logLevel.ERROR, "Init system: make admin bad input");
        }

        public static void openStore(string[] fullcommand)
        {
            if (fullcommand.Length == 3)
            {
                string username = fullcommand[1];
                string storeName = fullcommand[2];
                try
                {
                    User user = getUser(username, users[username]);
                    StoreService.AddStore(user, storeName);
                }
                catch (Exception e)
                {
                    Logger.Log("error", logLevel.ERROR,$"Init system: open store {e.Data}");
                }
            }
            else
                Logger.Log("error", logLevel.ERROR, "Init system: open store bad input");
        }
        public static void addProductBasket(string[] fullcommand)
        {

        }

        public static void addProductStore(string[] fullcommand)
        {
            if (fullcommand.Length == 8)
            {
                string username = fullcommand[1];
                string storeName = fullcommand[2];
                string productName = fullcommand[3];
                string category = fullcommand[4];
                string desc = fullcommand[5];
                int price = int.Parse(fullcommand[6]);
                int amount = int.Parse(fullcommand[7]);
                try
                {
                    User user = getUser(username, users[username]);
                    Store store = getStore(storeName);
                    int storeId = store.id;
                    int productId = StoreService.AddProductToStore(user, storeId, productName, desc, price, category);

                    StoreService.AddProductToStock(user, storeId, productId, amount);
                }
                catch (Exception e)
                {
                    Logger.Log("error", logLevel.ERROR,$"Init system: add Product to Store {e.Data}");
                }
            }
            else
                Logger.Log("error", logLevel.ERROR, "Init system: add Product to Store bad input");
        }

        public static void addStoreOwner(string[] fullcommand)
        {

            if (fullcommand.Length == 4)
            {
                string username = fullcommand[1];
                string storeName = fullcommand[2];
                string newManager = fullcommand[3];
                
                try
                {
                    User user = getUser(username, users[username]);
                    Store store = getStore(storeName);
                    int storeId = store.id;

                    Roles roles = new Roles(true, true, true, false, false, false, false, false, true);
                    string sushiRole = JsonHandler.SerializeObject(roles);

                    UserService.AddStoreManager(user, storeId, newManager, sushiRole);
                }
                catch(Exception e) {
                    Logger.Log("error", logLevel.ERROR,$"Init system: add store owner {e.Data}");
                }
            }
            else
                Logger.Log("error", logLevel.ERROR, "Init system: add store owner bad input");
        }
        
        public static void loginuser(string[] fullcommand)
        {

        }
        
    }
}
