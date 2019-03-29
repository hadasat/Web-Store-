using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Password
{
    public class PasswordHandler
    {

        private Dictionary<string, byte[]> saltes = new Dictionary<string, byte[]>(); //TMP

        
        //CREATING

        /** this is the byte[] that need to be stored **/
        public byte[] hashPassword(string password, string ID)
        {
            byte[] bytesPass = Encoding.ASCII.GetBytes(password);
            byte[] currSalt = CreateSalt(bytesPass.Length);
            saltes.Add(ID, currSalt);
            return GenerateSaltedHash(bytesPass, currSalt);
            
        }
        /** salt needs to be stored as well alongside the hash**/
        private byte[] CreateSalt(int size)
        {
            Random r = new Random();
            byte[] bSalt = new byte[size];
            for (int i = 0; i < size; i++)
            {
                bSalt[i] = (byte)r.Next(0, 255);
            }
            return bSalt;
        }


        private byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            { 
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }



        //DECHIPERING

        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }



    }
}
