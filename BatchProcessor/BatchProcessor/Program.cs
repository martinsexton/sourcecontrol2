using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            ACMActivationWorker worker = new ACMActivationWorker();
            worker.process();

            Console.Read();
        }
    }
}
