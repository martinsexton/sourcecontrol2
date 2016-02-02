using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DspPocEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();



            string passSecret = "DPS_Test";
            string docContractNumber = "12345"; //confirm if this is string or numeric
            int day = 25;
            int month = 1;
            int numSecret = (day * month);
            string plainText = passSecret + "-DOC" + docContractNumber + "-" + numSecret;

            HPSAEncryption encryptor = new HPSAEncryption();

            Console.WriteLine(plainText);
            Console.WriteLine(encryptor.encrypt(plainText));
            Console.ReadKey();


        }
    }
}
