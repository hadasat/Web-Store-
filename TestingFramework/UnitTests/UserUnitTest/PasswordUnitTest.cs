using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password.Tests
{
    [TestClass()]
    public class TestPasswordHandler
    {

        int ID = 1111;
        const int PASS_LENGTH = 8;
        string password;
        Tuple<byte[], byte[]> SaltesAndPepperEntry;
        PasswordHandler passwordHandler = new PasswordHandler();

        [TestInitialize]
        public void Init()
        {
            password = CreatePassword(PASS_LENGTH);

        }

        [TestCleanup]
        public void Cealup()
        {
            passwordHandler.RemoveEntry(ID);
        }

        [TestMethod()]
        [TestCategory("Users_Password")]
        public void hashPassword_Test()
        {
            passwordHandler.hashPassword(password, ID);
            SaltesAndPepperEntry = passwordHandler.GetEntry(ID);
            byte[] salt = SaltesAndPepperEntry.Item1;
            byte[] pepper = SaltesAndPepperEntry.Item2;
            Assert.IsTrue(salt.Length == PASS_LENGTH);
            Assert.IsTrue(pepper.Length == PASS_LENGTH);
            Assert.IsTrue(pepper != Encoding.ASCII.GetBytes(password));
        }

        [TestMethod()]
        [TestCategory("Users_Password")]
        public void IdentifyPassword_Test()
        {
            bool result = passwordHandler.IdentifyPassword(password, ID);
            Assert.IsTrue(result);
        }



        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
