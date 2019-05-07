using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Users;
using Microsoft.Extensions.Configuration;

namespace doneillspa.Services.Document
{
    public class DropBoxDocumentService : IDocumentService
    {
        private IConfiguration Configuration;

        public DropBoxDocumentService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void SaveDocument(string filename, byte[] content)
        {
            using (var dbx = new DropboxClient(Configuration["documentservice:key"]))
            {
                using (var mem = new MemoryStream(content))
                {
                    var updated = dbx.Files.UploadAsync(
                        "/Reports/" + filename,
                        mode: WriteMode.Overwrite.Instance,
                        body: mem).Result;

                    string revision = updated.Rev;
                }
            }
        }
    }
}
