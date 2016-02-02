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
        string generateToken(string docContract);
        string generateToken(string docContract, int monthDay, int month);
    }
}
