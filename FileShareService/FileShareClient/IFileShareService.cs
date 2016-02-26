using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileShareClient
{
    public interface IFileShareService
    {
        bool uploadFile(byte[] fileContentAsBytes, string filename);
        byte[] downloadFile(string filename);
    }
}
