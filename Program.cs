#pragma warning disable 642

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using Args;
using Logger;

/// \brief ����������� ������� ��������� ��� ������������ \nm.
/// 
namespace test {

///
/// \brief �������� ����� ����� Main � ����-����.
///  ����� ����� ���� �������� ���������� ���������� ��� ���������� ������� ����-�����.

    class Program
    {

/*        static  Program(){
          var format = new System.Globalization.NumberFormatInfo();
          format.NumberDecimalSeparator = ".";
        }  */

        static public ArgFlg  hlpF ;   ///<������ ��������� ����-�����.
        static public ArgFlg  dbgF ;  ///<���������-��������� ������ ��� ������ ������ ���������
        static public ArgFlg  vF ;    ///<�������������� ����� � �������.
        static public ArgIntMM    logLvl ; ///<����� ������ ������� �������������� ������.
        static public ArgStr      logNm ;  ///<����� ������ ������� �������������� ������ (� ������ ���������� �� ������������).
        static public ArgIntMM  max ;     ///<������������ �����, ������� ����� ��������� �� ��������.
        static public ArgInt   sleep ;    ///<����������� ��� ��������� �����, ������������ ������� �����.
        static public int     current = 3;///<������� ���������� �� ���������� �������� ���� ������� ������

        static  Program (){       /// ����������� �����������
            string lLvl = "log level names:{"+Loger.ILList()+"}";             
           hlpF   =  new ArgFlg(false, "?","help",    "to see this help");
           vF     =  new ArgFlg(false, "v",  "verbose", "additional info");
           dbgF   =  new ArgFlg(false, "d",  "debug",   "debug mode");
           logLvl =  new ArgIntMM(1,    "l",  "log",  "log level", "LLL");
           logNm  =  new ArgStr  ("Error",    "ln",  "logName",   lLvl, "NNN");
           sleep  =  new ArgIntMM(125,  "s",  "sleep",   "msecs to sleep", "SSS");
           logLvl.setMin(1);
           logLvl.setMax(8);
           max =  new ArgIntMM(1000,  "m",  "max",   "to count prime numbers up to MAX", "MAX");
           max.setMin(1);

        }
        /// \brief  ��������� ������ ��������� �� ������������� ����-�����.
        static public  void usage(){ /// 
           Args.Arg.mkVHelp("to start 2 threads with Monitor class", "", vF

                ,hlpF
                ,dbgF
                ,vF
       //         ,logLvl
                ,logNm
                ,sleep                                    
                ,max
                );
           Environment.Exit(1);
        }

        ///
        /// \brief  ����� ��� ������������ \nm.
        /// ����� Loger �������� ����������� ���������� IDisposable
        /// � ������� ����� ���������� � ��������������
        /// ������������ ��������� `using (Loger l = new Loger(logLvl)){}`,
        /// ������� ����������� �������� �������������� ��� ������ ������ `Dispose()`.
        /// � ������ ��������� ��� ������� ���� `primer` - `a` � `b` , �����������
        ///  ��������� ����� `primer.t` ��� �������� ������� �����,
        /// ������� ���������� � ������ ��������� � ��������� ������� ������ 
        /// (� ������� �������� `Info`) �  ��������� � ��������� ������
        /// (� ������� �������� `Debug`). 
        ///  ������ �������� ����� ����������� ����������� `a.t.Join();`, 
        ///  �� ���������� ������������� ������ `primer.t`.
        ///
        [STAThread]                
        public static void Main(string[] args)
        {
           IMPORTANCELEVEL x = IMPORTANCELEVEL.Error;

           for (int i = 0; i<args.Length; i++){
             if (hlpF.check(ref i, args))
               usage();
             else if (dbgF.check(ref i, args))
               ;
             else if (vF.check(ref i, args))
               ;
             else if (logLvl.check(ref i, args))
               ;
             else if (logNm.check(ref i, args)) {
                  x = Loger.strtoLvl(logNm);
             }
             else if (sleep.check(ref i, args))
               ;
             else if (max.check(ref i, args))
               ;
           }
           DateTime st = DateTime.Now;
           using (Loger l = new Loger(logNm, dbgF)){
//                l.WriteLine(IMPORTANCELEVEL.Info ,"Hello >{0}<", l.cnslLvl);

              if (vF)
                l.setCnslLvl ("Stats");
//                l.cnslLvl = IMPORTANCELEVEL.Stats;
              primer a = new primer("first", l, ThreadPriority.Normal);
              primer b = new primer("second", l);
     //       a.t.Priority = ThreadPriority.Lowest;
              a.t.Join();
              l.WriteLine(IMPORTANCELEVEL.Stats,"thread '{0}' finished with {1}/{2} numbers/primers"
                        , "first", a.numbers, a.primers);
              b.t.Join();
              l.WriteLine(IMPORTANCELEVEL.Stats,"thread '{0}' finished with {1}/{2} numbers/primers"
                       , "second", b.numbers, b.primers);
              DateTime fn = DateTime.Now;
              l.WriteLine(IMPORTANCELEVEL.Stats, "time of work is {0} secs"
                   , (fn - st).TotalSeconds);

           }
        }
    }
  
    
/// \brief ������� ������� ����� � ��������� �����.

    class primer  {
      public Thread t = null;     ///<
      public int    numbers = 0; ///< ���������� ����� �����, ����������� ������
      public int    primers = 0; ///< ���������� ������� �����, ���������� ������
      Loger log = null;

      public primer (string nm    ///<�������� �����;
                   , Loger l     ///<������ ��� ������ ���������;
                   , ThreadPriority p=ThreadPriority.Lowest
      ){
         log  = l;
         t =  new Thread(work); 
         t.Priority = p;
         t.Start(nm);                
      }

/// \brief �������, ������������� � �����.
/// ������� 
/// ����������� ����������� ����������
/// `Program.current`. ����� ������������� ������� �����������
/// ���������� ���������� ������������ ����� `Monitor`,
/// ����� `Monitor.Enter(typeof(Program))` �������� ��������� ������ � ����������� ���������� `Program`,
/// � ����� `Monitor.Exit(typeof(Program))`  ������������ �, ������, ���� ������ � ����
/// ���������� ������� �����.
///
      public  void work(object o){

        string  name =(string) o;
        int i = 0;
        while (i < Program.max){

          Interlocked.Increment(ref numbers);
        
          Monitor.Enter (typeof(Program));
          try{
            i = Program.current++;
          }
          finally{
            Monitor.Exit (typeof(Program));
          }
//          lock (typeof(Program)){
//            numbers++;
//            i = Program.current++;
//          }
          bool f = true;
          for (int j = i-1; j > 1; j--)
          {                    
              if (i % j == 0)
              {
                  f = false;
                  break;
              }
          }
          Thread.Sleep(Program.sleep);
          if (f) {
             Interlocked.Increment(ref primers);
             log.WriteLine(IMPORTANCELEVEL.Info,"{0}: next prime is {1}", name, i);
          }      
          else                           
             log.WriteLine(IMPORTANCELEVEL.Debug,"{0}: {1} is not a prime", name, i);
        }
      }
    }
}
