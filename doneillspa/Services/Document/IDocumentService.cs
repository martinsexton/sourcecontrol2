using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Services.Document
{
    public interface IDocumentService
    {
        void SaveDocument(string filename, byte[] content);
    }
}
