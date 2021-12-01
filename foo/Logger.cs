using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logger
{
    class Pair 
    {
        public ImportanceLevel lvl;
        //public string msg;
        //public Pair(ImportanceLevel l, string m)
        //{
        //    lvl = l;
        //    msg = m;
        //}
    }

    public class Logger : IDisposable
    {
        string fileName = "";
        Thread log;
        bool working;
        //bool dbg = false;
        //StreamWriter sw = null;
        //public Queue StringQueue = new Queue();
        ImportanceLevel importance = ImportanceLevel.Ignore;

        public Logger(ImportanceLevel il, string fileName)
        {
            working = true;
            importance = il;
            log = new Thread(new ThreadStart(LogMessage));
            log.Priority = ThreadPriority.Lowest;
            //log.Start();

            this.fileName = fileName;
            WriteLine(ImportanceLevel.Warning, "Logger started");
        }

        void LogMessage()
        {
            while (working)
            {
                Thread.Sleep(1000);
            }
        }

        public void Dispose()
        {
            working = false;
            //log.Join();
            WriteLine(ImportanceLevel.Warning, "Logger stopped");
        }

        public void WriteLine(ImportanceLevel importance, string format, params object[] segments)
        {
            if (importance >= this.importance)
            {
                Console.WriteLine("[{0}]:\t{1}", importance, string.Format(format, segments));
            }
        }
    }
}
