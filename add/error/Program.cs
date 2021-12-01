#pragma warning disable 642

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;
using Args;
using ut;
using Logger;

namespace test
{

    class Program
    {

/*        static  Program(){
          var format = new System.Globalization.NumberFormatInfo();
          format.NumberDecimalSeparator = ".";
        }  */

        static public ArgFlg  hlpF ;
        static public ArgFlg  dbgF ;
        static public ArgFlg  vF ;
        static public ArgStr  nm ;
        static public ArgIntMM    logLvl ;
        static public ArgFloat    num ;
//        static public ArgIntMM  max ;
//        static public ArgInt   sleep ;
//        static public int     current = 3; 

        static  Program (){
           hlpF   =  new ArgFlg(false, "?","help",    "to see this help");
           vF     =  new ArgFlg(false, "v",  "verbose", "additional info");
           dbgF   =  new ArgFlg(false, "d",  "debug",   "debug mode");
           nm =  new ArgStr("eeee",    "t",  "text",   "parameter for text", "tttt");
           num =  new ArgFloat(1.0,    "n",  "number",   "parameter for float");
           logLvl =  new ArgIntMM(1,    "l",  "log",   "log level", "LLL");
//           sleep  =  new ArgIntMM(125,  "s",  "sleep",   "msecs to sleep", "SSS");
           logLvl.setMin(1);
           logLvl.setMax(8);
//           max =  new ArgIntMM(1000,  "m",  "max",   "to count prime numbers up to MAX", "MAX");
//           max.setMin(1);

        }
        static public  void usage(){
           Args.Arg.mkVHelp("to show modal window", "...", vF

                ,hlpF
                ,dbgF
                ,vF
                ,logLvl
                ,nm
                ,num
                );
           Environment.Exit(1);
        }

        [STAThread]                
        static void Main(string[] args)
        {
           for (int i = 0; i<args.Length; i++){
             if (hlpF.check(ref i, args))
               usage();
             else if (dbgF.check(ref i, args))
               ;
             else if (vF.check(ref i, args))
               ;
             else if (logLvl.check(ref i, args))
               ;
             else if (nm.check(ref i, args))
               ;
             else if (num.check(ref i, args))
               ;
           }
           string field = "input data";
           string variable = "first field1 ";




           //Param 
           DateTime st = DateTime.Now;
           int ii = 1;
           string title = "";	
           using (LOGGER l = new LOGGER(LOGGER.uitoLvl(logLvl))){
              l.cnslLvl = IMPORTANCELEVEL.Debug;
              l.WriteLine(IMPORTANCELEVEL.Spam, "cycle was started" );

              {

                if (vF)
                  l.cnslLvl = IMPORTANCELEVEL.Stats;

                OkCancel f = new OkCancel("exit the application?");
                dd:

                DialogResult rc = f.ShowDialog();
                if (rc ==  DialogResult.OK){
                   l.WriteLine(IMPORTANCELEVEL.Info, "You've pressed OK");

                }
                else {
                   l.WriteLine(IMPORTANCELEVEL.Info, "You've pressed Cancel. Press yet more");
                   goto dd;
                }   


              }
              DateTime fn = DateTime.Now;
              Process curProc = Process.GetCurrentProcess();
              l.WriteLine(IMPORTANCELEVEL.Stats, "time of work/memory are {0} secs/{1} kBytes"
                   , (fn - st).TotalSeconds, curProc.PeakVirtualMemorySize64/1024 );

           }
        }
}}
