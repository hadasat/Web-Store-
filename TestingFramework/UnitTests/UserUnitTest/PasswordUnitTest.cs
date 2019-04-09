using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password.Tests
{

    //TODO: this test class crashes the regression
    //[TestClass()]
    public class TestPasswordHandler
    {

        //int ID = 11111;
        //const int PASS_LENGTH = 8;
        //string password;
        //Tuple<byte[], byte[]> SaltesAndPepperEntry;
        //PasswordHandler passwordHandler = new PasswordHandler();

        ////[TestInitialize]
        //public void Init()
        //{
        //    password = CreatePassword(PASS_LENGTH);

        //}

        ////[TestCleanup]
        //public void Cealup()
        //{
        //    passwordHandler.RemoveEntry(ID);
        //}

        //[TestMethod()]
        //[TestCategory("Users_Password")]
        //public void hashPassword_Test()
        //{
        //    try
        //    {
        //        Init();
        //        bool res = passwordHandler.hashPassword(password, ID);
        //        SaltesAndPepperEntry = passwordHandler.GetEntry(ID);
        //        byte[] salt = SaltesAndPepperEntry.Item1;
        //        byte[] pepper = SaltesAndPepperEntry.Item2;
        //        Assert.IsTrue(res);
        //    }
        //    finally
        //    {
        //        Cealup();
        //    }

        //}

        //[TestMethod()]
        //[TestCategory("Users_Password")]
        //public void IdentifyPassword_Test()
        //{
        //    try
        //    {
        //        Init();
        //        for (int i = 0; i < 100; i++)
        //        {
        //            password = CreatePassword(PASS_LENGTH);
        //            bool res = passwordHandler.hashPassword(password, ID);
        //            if (res)
        //            {
        //                bool result = passwordHandler.IdentifyPassword(password, ID);
        //                Assert.IsTrue(result);
        //            }
        //            else
        //                Assert.IsTrue(false);
        //            passwordHandler.RemoveEntry(ID);
        //        }
        //    }
        //    finally
        //    {
        //        Cealup();
        //    }

        //}



        //private string CreatePassword(int length)
        //{
        //    try
        //    {
        //        Init();
        //        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //        StringBuilder res = new StringBuilder();
        //        Random rnd = new Random();
        //        while (0 < length--)
        //        {
        //            res.Append(valid[rnd.Next(valid.Length)]);
        //        }
        //        return res.ToString();
        //    }
        //    finally
        //    {
        //        Cealup();
        //    }

        //}
    }
}
