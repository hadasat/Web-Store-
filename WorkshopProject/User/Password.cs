using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.DataAccessLayer;

namespace Password
{
    public class PasswordHandler
    {
        public static Repo repo = new Repo();
        // Dictionary<ID, Tuple <salt, pepper>
        //private Dictionary<int, Tuple<byte[], byte[]>> saltesAndPepper = new Dictionary<int, Tuple<byte[], byte[]>>();
        //private List<Password> passwordObjList = new List<Password>();
        //CREATING

        /** this is the byte[] that need to be stored **/

        //when register
        public bool hashPassword(string password, int ID)
        {
            byte[] bytesPass = Encoding.ASCII.GetBytes(password);
            byte[] currSalt = CreateSalt(bytesPass.Length);
            byte[] pepper = GenerateSaltedHash(bytesPass, currSalt);
            //saltesAndPepper.Add(ID,new Tuple<byte[], byte[]>(currSalt, pepper));
            Password passwordEntry = new Password(ID, currSalt, pepper);
            Add(passwordEntry);
            return true;
        }

        //when sign in
        public bool IdentifyPassword(string password, int ID)
        {
            byte[] bytesPass = Encoding.ASCII.GetBytes(password);
            //Tuple<byte[], byte[]> sAndP = saltesAndPepper[ID];
            Password sAndP = GetPasswordForMember(ID);
            byte[] salt = sAndP.salt;
            byte[] pepper = sAndP.pepper;
            byte[] userPepper = GenerateSaltedHash(bytesPass, salt);
            return CompareByteArrays(pepper, userPepper);
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



        //compare


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


        public void Add(Password entity)
        {
            if (useStub())
            {
                DataAccessDriver.Passwords.GetList().Add(entity);
            }
            else
            {
                repo.Add<Password>(entity);
                //return passwordObjList;
            }
        }

        public Password GetEntry(int ID)
        {
            foreach (Password p in GetList())
            {
                if (p.id == ID)
                {
                    return p;
                }
            }
            return null;
        }

        public Password GetPasswordForMember(int memberId)
        {
            foreach (Password p in GetList())
            {
                if (p.memberId == memberId)
                {
                    return p;
                }
            }
            return null;
        }

        public void RemoveEntry(int ID)
        {
            Password toRemove = GetEntry(ID);
            repo.Remove<Password>(toRemove);
        }

        public void RemovePasswordFromMember(int memberId)
        {
            try
            {
                Password toRemove = GetPasswordForMember(memberId);
                repo.Remove<Password>(toRemove);
            }
            catch
            {

            }
        }

        private ICollection<Password> GetList()
        {
            if (useStub())
            {
                return DataAccessDriver.Passwords.GetList();
            }
            else
            {
                return repo.GetList<Password>();
                //return passwordObjList;
            }
        }

        private static bool useStub()
        {
            return DataAccessDriver.UseStub;
        }
    }

    public class Password : IEntity
    {
        [Key]
        public int id { get; set; }
        [MaxLength(128)]
        public byte[] salt { get; set; }
        [MaxLength(128)]
        public byte[] pepper { get; set; }
        public int memberId { get; set; }

        public Password()
        {

        }


        public Password(int memberId, byte[] salt, byte[] pepper)
        {
            this.memberId = memberId;
            this.salt = salt;
            this.pepper = pepper;
        }

        public override int GetKey()
        {
            return id;
        }

        public override void LoadMe()
        {
            //do nothing
        }

        public override void SetKey(int key)
        {
            id = key;
        }
    }
}
