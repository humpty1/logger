using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
//using System.Windows.Forms;
using System.Data;
using System.IO;



namespace Logger  /// Именованная область видимости библитеки Журналирование.
{
    /// \brief Перечисление уровней важности сообщения
    public enum IMPORTANCELEVEL {
       //bla  ///< забыл откуда взялось
      //, 
       Spam       ///< мусорные сообщения
      , Debug       ///< отладка
      , Warning     ///< предупреждения
      , Stats       ///< менне важная информация
      , Error       ///< ошибки приложения
      , FatalError  ///<катастрофические ошибки, делающие навозможным функционирвание приложения
      , Info       ///<очень важная информация
      ,  Ignore ///< для отсутствия вывода вообще
     };

    ///  \brief вспомогательный класс. Пара (уровень важности, сообщение) ставится в очередь сообщений.
    class pair {
       public  IMPORTANCELEVEL lvl;
       public string  msg;
       public pair (IMPORTANCELEVEL l, string  m){
          lvl = l; 
          msg = m; 
       }
    }




///
///  \brief синоним для класса LOGGER
///
/// Лично мне не нравятся названия классов записанные большими буквами,
/// но уже написано слишком много проектов с журналированием,
/// что бы можно было заменить название.
/// Сообщения для журналирования не выводятся в файл журнала, а
///  ставятся в очередь сообщений.
/// Из очереди сообщений в файл их выводит специальный  сред, 
/// который работает с самым низким приоритетом  - `ThreadPriority.Lowest`
///  и засыпает  `Thread.Sleep(10)` при отсутствии сообщений в очереди.
///
    public class Loger : LOGGER
    {
        ///\brief версия библиотеки
        static public void version (out int major, out int minor, out int build){
             System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly(); 
             System.Version ver = asm.GetName().Version;
             major =  ver.Major;
             minor =  ver.Minor;
             build =  ver.Build;
        }
        ///
        /// В конструкторе главный параметр уровень важности
        /// При помощи него можно задать какие сообщения будут сохраняться
        /// в журнале, а какие - нет.
        /// Если явно не задается имя журнала, то оно будет совпадать с именем приложения,
        /// за исключением расширения, оно будет не _exe_, а _log_.

        public Loger(
               IMPORTANCELEVEL ImportanceLevel   ///< минимальный уровень важности сохраняемых сообщений;
            ,  bool lDbg                         ///< отладка  библиотеки журналирования, если установлен в `true`, то файл журнала закрывается после каждого вывода. Приводит к существенному замедлению работы журналирования;
            ,  string flNm)                      ///<  имя файла журнала. 
        : base(ImportanceLevel,  lDbg, flNm){}


        public Loger() :base ("Spam", false, null)
        {
        }

        public Loger(string  impLevel         ///< минимальный уровень важности как текст, при неправильно заданном уровне будет установлен в Error
        ) :base (impLevel, false, null)
        {
        }

        public Loger(string impLevel         ///< минимальный уровень важности, как текст
          , bool lDbg
        ) :base (impLevel, lDbg, null)
        {
        }

        public void setCnslLvl(string lvl) {
          IMPORTANCELEVEL ImportanceLevel= IMPORTANCELEVEL.Error;
          Enum.TryParse(lvl, out ImportanceLevel);
          cnslLvl = ImportanceLevel;
        }

        public void WriteLine( String Format)  // попробовать с инвоке
        {
           base.WriteLine(IMPORTANCELEVEL.Debug, Format);
        }
    }
    
    public class LOGGER : IDisposable
    {
        string filename = "";
        bool  dbg       = false;              ///< закрывать-открывать файл после каждого вывода
        StreamWriter sw = null;
        Queue StringQueue = new Queue();     ///< Очередь сообщений
        // Установленный уровень важности (сообщения ниже установленного уровня - игнорируются)
        IMPORTANCELEVEL ImportanceLevel = IMPORTANCELEVEL.Error;
        /// \brief дублирование вывода в консоль, менее важные сообщения будут игнорироваться
        public  IMPORTANCELEVEL cnslLvl = IMPORTANCELEVEL.Ignore;
        // Поток для логера
        Thread Log;
        // Bool для выхода из бесконечного цикла в потоке логера
        bool Working = false;
        int Counter  = 0;
//        List<string> Items;


        ///  \brief  Констурктор, создаст и запустит логер
        public LOGGER(string impLevel,  bool lDbg, string fn)
        {
          IMPORTANCELEVEL ImportanceLevel= IMPORTANCELEVEL.Error;
          Enum.TryParse(impLevel, out ImportanceLevel);
          mkLog(ImportanceLevel,   lDbg,  fn);
        }

/*
        ///  \brief  Констурктор, создаст и запустит логер
        public LOGGER(string impLevel,  bool lDbg, string fn)
        {
          IMPORTANCELEVEL ImportanceLevel= IMPORTANCELEVEL.Error;
          if (Enum.TryParse(impLevel, out ImportanceLevel))  {
            cnslLvl = ImportanceLevel;
          }
          else {
            cnslLvl = IMPORTANCELEVEL.Error;
          }
          mkLog(ImportanceLevel,   lDbg,  fn);

        }  */

        public LOGGER(IMPORTANCELEVEL ImportanceLevel,  bool lDbg, string fn)

        {
           mkLog(ImportanceLevel,   lDbg,  fn);
        }

        void mkLog(IMPORTANCELEVEL ImportanceLevel,  bool lDbg, string fn)
//        public LOGGER(IMPORTANCELEVEL ImportanceLevel, List<string> items, string fn)
        {
            int attemp = 0;

            if (fn == null) {
//                string  MyName = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)
//                +"/"+Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath)
//                +".log";
//System.IO.Path.GetDirectoryName( 
 //     System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase );
                string  MyName = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath)
                +"/"+Path.GetFileNameWithoutExtension(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath)
                +".log";




                fn = MyName.ToLower();
            }
            this.dbg = lDbg;
            this.filename = fn;
//            this.Items = items;
            this.ImportanceLevel = ImportanceLevel;
            this.Working = true;
            // Создание и старт потока логера
            Log = new Thread(new System.Threading.ThreadStart(LogMessage));
            Log.Priority = ThreadPriority.Lowest;
            repeat: 
              if (attemp > 5)    {
                Console.Error.WriteLine("Error while opening file '{0}'/{1}", filename, attemp);
                throw new Exception();
              }
            try {
              this.sw = new StreamWriter(filename, true);
            }
            catch (Exception){

                filename = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath)
                +"/"+Path.GetFileNameWithoutExtension(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath)
                + attemp.ToString()
                +".log";
               attemp++;
               goto repeat;
            }
            Log.Start();
            WriteLine(IMPORTANCELEVEL.Warning
               , "Logger was started in {0}, console/window {1}/{2} application"
                      , Path.GetFileName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath)
                      , PeReaderExtensions.IsConsoleApp()
                      , PeReaderExtensions.IsGuiApp());             
            
         }


        /// \brief устарел  
        public LOGGER(uint lvl) :this (uitoLvl(lvl), false, null)
        {
        }                        
        /// \brief устарел  
        public LOGGER(int lvl) :this (uitoLvl((uint)lvl), false, null)
        {
        }

        public LOGGER(IMPORTANCELEVEL ImportanceLevel) :this (ImportanceLevel, false, null)
        {
        }
        public LOGGER(IMPORTANCELEVEL ImportanceLevel, string flNm) :this (ImportanceLevel, false, flNm)
        {
        }
        public LOGGER(IMPORTANCELEVEL ImportanceLevel, bool lDbg) :this (ImportanceLevel, lDbg, null)
        {
        }
        /// \brief создание строчки времени в нужном формате.
        string  mkNow(){
             return String.Format(
                   "[{0:00}.{1:00}.{2:00} {3:00}:{4:00}:{5:00}]: "
                   // "[" + //DateTime.Now.DayOfWeek + 
                    ,DateTime.Now.Day 
                    //+ "." + 
                    ,DateTime.Now.Month 
                    //+"." + 
                    ,DateTime.Now.Year 
                    //+" " + 
                    ,DateTime.Now.Hour 
                    //+":" + 
                    ,DateTime.Now.Minute 
                    //+":" + 
                    ,DateTime.Now.Second 
                    //+"] "
                    );
        }

        /// \brief для удобства заменты    `Console.WriteLine()` на `Loger.WriteLine()`
        public void Write( String Format, params object[] Segments)
        {
            if (Working )
            {
                // Форматирование даты
                // Форматирование сообщения
                String Message = mkNow()
                 + "\t" + string.Format(Format, Segments);
                // Добавление сообщения в очередь
                lock (this)
                {
                    StringQueue.Enqueue(new pair (IMPORTANCELEVEL.Ignore, Message));
                }
            }
        }
        /// \brief для удобства заменты    `Console.WriteLine()` на `Loger.WriteLine()`
        public void WriteLine( String Format, params object[] Segments)
        {
           WriteLine(IMPORTANCELEVEL.Debug, Format, Segments);
        }

        /// \brief Метод-аналог `Console.WriteLine()` с пользовательскими параметрами
        ///
        /// Метод блокирует доступ к журналу оператором     `lock (this) {}`,
        /// поэтому чтение из очереди сообщений  (LogMessage()) и поставка в очередь
        /// выполняются по очереди

        public void WriteLine(IMPORTANCELEVEL Importance  ///< важность данного сообщения
                             , String Format              ///< строка с форматом сообщения
                             , params object[] Segments   ///< параметры сообщения
             ){
            // Проверка уровня важности сообщения
            if (Working && Importance >= this.ImportanceLevel)
            {
                // Форматирование даты
                // Форматирование сообщения
                String Message = mkNow()
                 + String.Format("[{0}]\t", Importance) + "\t" + string.Format(Format, Segments);
                // Добавление сообщения в очередь
                lock (this)
                {
                    StringQueue.Enqueue(new pair (Importance, Message));
                }
            }
        }

        /// \brief  метод вывода сообщений в файл журнала. Работает в отдельном потоке.
        ///
        /// Нет особой нужды, чтоб этот метод был публичным
        /// Для блокирования очереди сообщений  испольуется оператор  `lock (this){ }`
        /// В случае отсутствия сообщений в очереди, сред засыпает на некоторое время.
        ///
        public void LogMessage()        {
            pair p=null;
            // Бесконечный цикл вывода сообщений раз в секунду
            while (Working)
            {
                // Вывод всех сообщений, если есть
               lock (this)
               {
                  if (StringQueue.Count > 0)
                  {
                    p = (pair)StringQueue.Dequeue();
                    sw.WriteLine(p.msg);
                    if (p.lvl >= cnslLvl) {
                      Console.Error.WriteLine(p.msg);
                      }
                    if (dbg)  {
                       sw.Close();
                         sw = new StreamWriter(filename, true);
                      }
                    ++Counter;
                    p=null;
  //                    Items.Add(s);
                  }
                  else 
                      Thread.Sleep(10);//Console.WriteLine("{0} сообщений", StringQueue.Count);
               }
                   // Ждем  что бы не нагружать процессор   
                //  Thread.Sleep(1000);    
              //           удалил так как поставил низкий приоритет
            }
       }

        /// \brief Метод остановки  логера и его среда
        public void Stop()///Стандартное название функции деструктор, освобождает важные ресурсы, которые не относяться к памяти
        {
            Dispose();
        }

        /// \brief Метод остановки  логера и его среда
        public void Dispose()
        {
            pair p=null;
            // Пытаемся остановить логер
            this.Working = false;
            if (Log != null)
                Log.Join();

            if (sw == null) {
               Console.Error.WriteLine(mkNow() + "Logger destructor is here...Logger has been closed alredy!");
               while (StringQueue.Count > 0)
               {
                   // Если есть еще не выведенные сообщения, 
                   //
                       p = (pair)StringQueue.Dequeue();
                       Console.Error.WriteLine(p.msg);
                       p=null;
   //               Items.Add((String)StringQueue.Dequeue());
               }

               return;
            }

            if (this.ImportanceLevel <= IMPORTANCELEVEL.Warning)
              sw.WriteLine( mkNow()
                          + String.Format("[{0}]\t", IMPORTANCELEVEL.Warning) 
                           + "\tLogger destructor is here..."
            );
            while (StringQueue.Count > 0)
            {
                // Если есть еще не выведенные сообщения, 
                //
                    p = (pair)StringQueue.Dequeue();
                    if (p.lvl >= cnslLvl ) {
                            Console.Error.WriteLine(p.msg);
                        }
                    sw.WriteLine(p.msg);
                    p=null;
//               Items.Add((String)StringQueue.Dequeue());
            }
            if (this.ImportanceLevel <= IMPORTANCELEVEL.Warning)
              sw.WriteLine( mkNow()
                          + String.Format("[{0}]\t", IMPORTANCELEVEL.Warning) 
                           + "\tLogger  is stopped"
            );
            sw.WriteLine("\n.");
            sw.Close();
        }

        public delegate void MyExceptionEventHendler(object sender, Exception e);//print
        public MyExceptionEventHendler MyException;
        //public void AddMyException(object obj)
        //{
        //    MyException+=
        //}

        public void WriteException(Exception e)
        {
            WriteLine(IMPORTANCELEVEL.Error,
               "Exception message: {0}\tException StackTrace: {1}", e.Message, e.StackTrace);
            if (MyException != null)
                MyException(this, e);
        }



        /// \brief метод выдает  список уровней важности для подсказки оператору.
        public static string ILList() {  
                   Array all = Enum.GetValues(typeof(IMPORTANCELEVEL));
//           Console.WriteLine("length of arrray:  {0}", all.GetLength(0));
           List<string> a =  new List<string>();
           foreach (object i in all)
             a.Add(i.ToString());

           return  String.Join(" ", a);
        }

        public static IMPORTANCELEVEL strtoLvl(string  code) {
               IMPORTANCELEVEL x = IMPORTANCELEVEL.Error;
               try {
                 x = (IMPORTANCELEVEL) Enum.Parse(typeof(IMPORTANCELEVEL), code);
               }
               catch {
                 x = IMPORTANCELEVEL.Error;
               }
           return  x;
        }
        public static IMPORTANCELEVEL uitoLvl(int  code) {
           return  uitoLvl((uint)  code);
        }
        public static IMPORTANCELEVEL uitoLvl(uint  code) {
          IMPORTANCELEVEL rc = IMPORTANCELEVEL.Spam;
          switch (code) {
            case 0 :  rc =IMPORTANCELEVEL.Ignore; break;
            case 1 :  rc =IMPORTANCELEVEL.Info; break;
            case 2 :  rc =IMPORTANCELEVEL.FatalError; break;
            case 3 :  rc =IMPORTANCELEVEL.Error; break;
            case 4 :  rc =IMPORTANCELEVEL.Stats; break;
            case 5 :  rc =IMPORTANCELEVEL.Warning; break;
            case 6 :  rc =IMPORTANCELEVEL.Debug; break;
            case 7 :  rc =IMPORTANCELEVEL.Spam; break;
          }
          return  rc;
        }


    }

}