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

        //Default version of Class using SHA1 Hash Algorithm
        public HPSAEncryption()
            : this(new SHA1CryptoServiceProvider()){}

        public HPSAEncryption(HashAlgorithm algorithm)
        {
            hashAlgorithm = algorithm;
        }

        public string encrypt(string plainText)
        {
            byte[] plainTextAsBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] result = hashAlgorithm.ComputeHash(plainTextAsBytes);

            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}
