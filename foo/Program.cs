using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;

namespace TestUnit
{
    class Program 
    {
        static void Main(string[] args)
        {
            using (Logger.Logger l = new Logger.Logger(ImportanceLevel.Debug, "log.txt"))
            {
                l.WriteLine(ImportanceLevel.Debug, "Message1 1 - {0}, 2 - {1}", "param1", 1);
                l.WriteLine(ImportanceLevel.Debug, "Message2 1 - {0}, 2 - {1}", "param2", 2);
                l.WriteLine(ImportanceLevel.Debug, "Message3 1 - {0}, 2 - {1}", "param3", 3);
                l.WriteLine(ImportanceLevel.Debug, "Message4 1 - {0}, 2 - {1}", "param4", 4);
                l.WriteLine(ImportanceLevel.Debug, "Message5 1 - {0}, 2 - {1}", "param5", 5);
                Console.ReadKey();
            }
        }
    }
}
