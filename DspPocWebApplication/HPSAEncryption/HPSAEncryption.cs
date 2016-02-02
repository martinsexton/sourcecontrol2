using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DspPocEncryption
{
    public class HPSAEncryption : IHPSAEncryption
    {
        private HashAlgorithm hashAlgorithm;
        private const string SHARED_SECRET_KEY = "DPS_Test";

        //Default version of Class using SHA1 Hash Algorithm
        public HPSAEncryption()
            : this(new SHA1CryptoServiceProvider()) { }

        public HPSAEncryption(HashAlgorithm algorithm)
        {
            hashAlgorithm = algorithm;
        }

        public string generateToken(string docContract)
        {
            int monthDay = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            return generateToken(docContract, monthDay, month);
        }


        public string generateToken(string docContract, int monthDay, int month)
        {
            string plainText = SHARED_SECRET_KEY + "-DOC" + docContract + "-" + (monthDay * month);

            byte[] plainTextAsBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] result = hashAlgorithm.ComputeHash(plainTextAsBytes);

            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}
