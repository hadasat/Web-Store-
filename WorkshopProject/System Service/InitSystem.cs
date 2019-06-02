using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WorkshopProject.System_Service
{
     public static class InitSystem
    {
        private static string filePath = "../../../hadas.txt";
        private static string word = @"\b[a-zA-Z1-9\-]+\b";
        private static string commandSyntax = @"^[a-zA-Z0-9\-]+\([a-zA-Z0-9]*(,[[a-zA-Z0-9]+)*\);";

        public static void updatePath(string new_path) { filePath = new_path; }

        public static string[] splitCommand(string sentence)
        {
            Regex rgxword = new Regex(word);
            List<string> args = new List<string>();
            foreach (Match match in rgxword.Matches(sentence))
            {
                args.Add(match.Value);
            }
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
                    appointManager(fullcommand);
                    break;
                case "open-store":
                    openStore(fullcommand);
                    break;
                case "add-product-basket":
                    addProductBasket(fullcommand);
                    break;
                case "add-product-stock":
                    addProductStock(fullcommand);
                    break;
                case "add-store-product":
                    addProductStore(fullcommand);
                    break;
                case "add-store-owner":
                    addStoreOwner(fullcommand);
                    break;
                case "add-store-manager":
                    addStoreManager(fullcommand);
                    break;
                case "login-user":
                    loginuser(fullcommand);
                    break;

            }
        }

        public static void registerNewUser(string[] fullcommand)
        {

        }

        public static void makeAdmin(string[] fullcommand)
        {

        }

        public static void openStore(string[] fullcommand)
        {

        }
        public static void addProductBasket(string[] fullcommand)
        {

        }
        public static void addProductStock(string[] fullcommand)
        {

        }

        public static void addProductStore(string[] fullcommand)
        {

        }

        public static void addStoreOwner(string[] fullcommand)
        {

        }

        public static void addStoreManager(string[] fullcommand)
        {

        }

        public static void appointManager(string[] fullcommand)
        {

        }

        public static void loginuser(string[] fullcommand)
        {

        }
        
    }
}
