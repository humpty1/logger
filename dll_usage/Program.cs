using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Logger;

namespace ConsoleApplication2
{
    class Program
    {
        static bool dFlag = false;
        //Обьявили глобально для получения доступа в классе ProgExp со своими исключениями
        static LOGGER Logger = null;//new LOGGER(IMPORTANCELEVEL.Warning);
        [STAThread]
        static void Main(string[] args)
        {

        if (args.Length > 1) {
          if (args[0] == "-?") {
            Console.WriteLine("to test logger \nusage:\n app.exe [-?] [-d] \n");
            Environment.Exit(1);

          }
          else if (args[0] == "-d"){
            dFlag = true;
          }
        
        
        
        
        
        }
      System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly(); 
      System.Version ver = asm.GetName().Version;

       string MyName  = asm.GetName().Name;

       MyName //= Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)
       +=".Logger.test.log";


            Thread.Sleep(2000);
            Logger = new LOGGER(IMPORTANCELEVEL.Warning, dFlag, MyName);
         
            for (int i = 0; i < 10; i++)
            {
                Logger.WriteLine(IMPORTANCELEVEL.Info,"this is {0} iteration", i);
                Console.Write("\rthis is {0:00} iteration", i);
                Thread.Sleep(500);
            }


            Thread.Sleep(100);
            Logger.Stop();
            Thread.Sleep(100);
            IMPORTANCELEVEL iL = IMPORTANCELEVEL.Spam;
                Console.Write("\nsecond logger in debug mode for {0} level\n\n", iL);

          //  Logger = new LOGGER(iL, true);
            Logger = new LOGGER(iL);

						Logger.WriteLine(IMPORTANCELEVEL.Warning	," to use debug mode of logger" );

						Logger.cnslLvl = IMPORTANCELEVEL.Spam;
						Logger.cnslLvl = IMPORTANCELEVEL.Ignore;
						Logger.cnslLvl = IMPORTANCELEVEL.Error;
						Logger.cnslLvl = IMPORTANCELEVEL.Info;
						Logger.cnslLvl = IMPORTANCELEVEL.FatalError;
						Logger.cnslLvl = IMPORTANCELEVEL.Debug;
						Logger.cnslLvl = IMPORTANCELEVEL.Warning;
						Logger.cnslLvl = IMPORTANCELEVEL.Stats;

            for (uint i = 0; i < 9; i++)
            {
                Logger.WriteLine(LOGGER.uitoLvl(i),
                   "this is {0} iteration {1}>={2} =={3}  ", 
                     i, LOGGER.uitoLvl(i), Logger.cnslLvl, LOGGER.uitoLvl(i)>= Logger.cnslLvl);
                Thread.Sleep(500);
            }


//						Logger.cnslLvl = IMPORTANCELEVEL.Info;
						
            for (uint i = 0; i < 0; i++)
            {
                Logger.WriteLine(LOGGER.uitoLvl(i),"this is {0} iteration {1}", i, LOGGER.uitoLvl(i));
            }
						
						Logger.WriteLine(IMPORTANCELEVEL.Spam	," no any message" );
						Logger.WriteLine(IMPORTANCELEVEL.Warning," to  finish of usage debug mode of logger" );
            Logger.Dispose();
						Logger.WriteLine(IMPORTANCELEVEL.Warning," to  debug" );
        }
        //специфический класс со своими исключениями
        class ProgExcp : Exception
        {
            public string msg;//print
            public ProgExcp(string s)
            {
                msg = s;
            }
            public static void WriteMyException(object o, Exception e)
            {
                ProgExcp MyEx = e as ProgExcp;
                LOGGER l = o as LOGGER;
                if (MyEx != null && l != null)
                    l.WriteLine(IMPORTANCELEVEL.Error, "Add info: {0}", MyEx.msg);//записываем в логер исключение
            }
        }
    }
}
