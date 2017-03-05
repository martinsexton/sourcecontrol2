using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessor
{
    class ACMActivationWorker
    {
        private string[] filenames;
        Repository repo = new Repository();
        List<string> firstBatch = new List<string>();
        List<string> secondBatch = new List<string>();

        public void process()
        {
            inialise();
            partitionFiles();

            //processFilesSequentially();

            Task.Factory.StartNew(processFile, firstBatch);
            Task.Factory.StartNew(processFile, secondBatch);

        }

        private void inialise()
        {
            filenames = Directory.GetFiles("C:\\Batch", "*", System.IO.SearchOption.AllDirectories);
        }

        private void partitionFiles()
        {
            for (int i = 1; i < filenames.Length+1; i++)
            {
                if (i % 2 == 0)
                {
                    firstBatch.Add(filenames[i-1]);
                }
                else
                {
                    secondBatch.Add(filenames[i-1]);
                }
            }
        }

        private void processFile(object files)
        {
            foreach(var file in (List<string>)files){
                var reader = new StreamReader(File.OpenRead(file));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    repo.AddRecord(values[0], values[1], DateTime.Now, file);
                }
            }
        }

        private void processFilesSequentially()
        {
            foreach (var file in filenames)
            {
                var reader = new StreamReader(File.OpenRead(file));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    repo.AddRecord(values[0], values[1], DateTime.Now, file);
                }
            }
        }
    }
}
