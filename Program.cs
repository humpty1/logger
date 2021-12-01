#pragma warning disable 642

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using Args;
using Logger;

/// \brief Именованная область видимости для тестирования \nm.
/// 
namespace test {

///
/// \brief содержит точку входа Main в юнит-тест.
///  Кроме этого файл содержит глобальные переменные для управления работой юнит-теста.

    class Program
    {

/*        static  Program(){
          var format = new System.Globalization.NumberFormatInfo();
          format.NumberDecimalSeparator = ".";
        }  */

        static public ArgFlg  hlpF ;   ///<выдать подсказку юнит-теста.
        static public ArgFlg  dbgF ;  ///<открывать-закрывать журнал при каждом выводе сообщения
        static public ArgFlg  vF ;    ///<дополнительный вывод в консоль.
        static public ArgIntMM    logLvl ; ///<чтобы задать уровень журналирования числом.
        static public ArgStr      logNm ;  ///<чтобы задать уровень журналирования именем (в данном приложении не используется).
        static public ArgIntMM  max ;     ///<максимальное целое, которое будем проверять на простоту.
        static public ArgInt   sleep ;    ///<милисекунды для засыпания среда, вычисляющего простые числа.
        static public int     current = 3;///<текущий претендент на выполнение свойства быть простым числом

        static  Program (){       /// статический конструктор
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
        /// \brief  программа выдачи подсказки по использованию юнит-теста.
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
        /// \brief  Метод для тестирования \nm.
        /// Класс Loger является наследником интерфейса IDisposable
        /// и поэтому может вызываться с использованием
        /// специального оператора `using (Loger l = new Loger(logLvl)){}`,
        /// который гарантирует закрытие журналирования при помощи метода `Dispose()`.
        /// В методе создаются два объекта типа `primer` - `a` и `b` , запускающих
        ///  различных среда `primer.t` для подсчета простых чисел,
        /// которые записывают в журнал сообщения о найденных простых числах 
        /// (с уровнем важности `Info`) и  сообщения о непростых числах
        /// (с уровнем важности `Debug`). 
        ///  Работа главного среда блокируется операторами `a.t.Join();`, 
        ///  до завершения выполняющихся средов `primer.t`.
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
  
    
/// \brief подсчет простых чисел в отдельном среде.

    class primer  {
      public Thread t = null;     ///<
      public int    numbers = 0; ///< количество целых чисел, проверенных средом
      public int    primers = 0; ///< количество простых чисел, полученных средом
      Loger log = null;

      public primer (string nm    ///<название среда;
                   , Loger l     ///<журнал для вывода сообщений;
                   , ThreadPriority p=ThreadPriority.Lowest
      ){
         log  = l;
         t =  new Thread(work); 
         t.Priority = p;
         t.Start(nm);                
      }

/// \brief функция, выполняющийся в среде.
/// Фукнция 
/// увеличивает статическую переменную
/// `Program.current`. Чтобы гарантировать строгую очередность
/// увеличения переменной используется класс `Monitor`,
/// метод `Monitor.Enter(typeof(Program))` которого блокирует доступ к статическим переменным `Program`,
/// а метод `Monitor.Exit(typeof(Program))`  разблокирует и, значит, дает доступ к этим
/// переменным другому среду.
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
