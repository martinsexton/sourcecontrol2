using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DspPocEncryption
{
    public interface IHPSAEncryption
    {
        string encrypt(string plainText);
    }
}
