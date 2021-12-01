using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
//using System.Windows.Forms;
using System.Data;
using System.IO;



namespace Logger  /// ����������� ������� ��������� ��������� ��������������.
{
    /// \brief ������������ ������� �������� ���������
    public enum IMPORTANCELEVEL {
       //bla  ///< ����� ������ �������
      //, 
       Spam       ///< �������� ���������
      , Debug       ///< �������
      , Warning     ///< ��������������
      , Stats       ///< ����� ������ ����������
      , Error       ///< ������ ����������
      , FatalError  ///<���������������� ������, �������� ����������� ��������������� ����������
      , Info       ///<����� ������ ����������
      ,  Ignore ///< ��� ���������� ������ ������
     };

    ///  \brief ��������������� �����. ���� (������� ��������, ���������) �������� � ������� ���������.
    class pair {
       public  IMPORTANCELEVEL lvl;
       public string  msg;
       public pair (IMPORTANCELEVEL l, string  m){
          lvl = l; 
          msg = m; 
       }
    }




///
///  \brief ������� ��� ������ LOGGER
///
/// ����� ��� �� �������� �������� ������� ���������� �������� �������,
/// �� ��� �������� ������� ����� �������� � ���������������,
/// ��� �� ����� ���� �������� ��������.
/// ��������� ��� �������������� �� ��������� � ���� �������, �
///  �������� � ������� ���������.
/// �� ������� ��������� � ���� �� ������� �����������  ����, 
/// ������� �������� � ����� ������ �����������  - `ThreadPriority.Lowest`
///  � ��������  `Thread.Sleep(10)` ��� ���������� ��������� � �������.
///
    public class Loger : LOGGER
    {
        ///\brief ������ ����������
        static public void version (out int major, out int minor, out int build){
             System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly(); 
             System.Version ver = asm.GetName().Version;
             major =  ver.Major;
             minor =  ver.Minor;
             build =  ver.Build;
        }
        ///
        /// � ������������ ������� �������� ������� ��������
        /// ��� ������ ���� ����� ������ ����� ��������� ����� �����������
        /// � �������, � ����� - ���.
        /// ���� ���� �� �������� ��� �������, �� ��� ����� ��������� � ������ ����������,
        /// �� ����������� ����������, ��� ����� �� _exe_, � _log_.

        public Loger(
               IMPORTANCELEVEL ImportanceLevel   ///< ����������� ������� �������� ����������� ���������;
            ,  bool lDbg                         ///< �������  ���������� ��������������, ���� ���������� � `true`, �� ���� ������� ����������� ����� ������� ������. �������� � ������������� ���������� ������ ��������������;
            ,  string flNm)                      ///<  ��� ����� �������. 
        : base(ImportanceLevel,  lDbg, flNm){}


        public Loger() :base ("Spam", false, null)
        {
        }

        public Loger(string  impLevel         ///< ����������� ������� �������� ��� �����, ��� ����������� �������� ������ ����� ���������� � Error
        ) :base (impLevel, false, null)
        {
        }

        public Loger(string impLevel         ///< ����������� ������� ��������, ��� �����
          , bool lDbg
        ) :base (impLevel, lDbg, null)
        {
        }

        public void setCnslLvl(string lvl) {
          IMPORTANCELEVEL ImportanceLevel= IMPORTANCELEVEL.Error;
          Enum.TryParse(lvl, out ImportanceLevel);
          cnslLvl = ImportanceLevel;
        }

        public void WriteLine( String Format)  // ����������� � ������
        {
           base.WriteLine(IMPORTANCELEVEL.Debug, Format);
        }
    }
    
    public class LOGGER : IDisposable
    {
        string filename = "";
        bool  dbg       = false;              ///< ���������-��������� ���� ����� ������� ������
        StreamWriter sw = null;
        Queue StringQueue = new Queue();     ///< ������� ���������
        // ������������� ������� �������� (��������� ���� �������������� ������ - ������������)
        IMPORTANCELEVEL ImportanceLevel = IMPORTANCELEVEL.Error;
        /// \brief ������������ ������ � �������, ����� ������ ��������� ����� ��������������
        public  IMPORTANCELEVEL cnslLvl = IMPORTANCELEVEL.Ignore;
        // ����� ��� ������
        Thread Log;
        // Bool ��� ������ �� ������������ ����� � ������ ������
        bool Working = false;
        int Counter  = 0;
//        List<string> Items;


        ///  \brief  �����������, ������� � �������� �����
        public LOGGER(string impLevel,  bool lDbg, string fn)
        {
          IMPORTANCELEVEL ImportanceLevel= IMPORTANCELEVEL.Error;
          Enum.TryParse(impLevel, out ImportanceLevel);
          mkLog(ImportanceLevel,   lDbg,  fn);
        }

/*
        ///  \brief  �����������, ������� � �������� �����
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
            // �������� � ����� ������ ������
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


        /// \brief �������  
        public LOGGER(uint lvl) :this (uitoLvl(lvl), false, null)
        {
        }                        
        /// \brief �������  
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
        /// \brief �������� ������� ������� � ������ �������.
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

        /// \brief ��� �������� �������    `Console.WriteLine()` �� `Loger.WriteLine()`
        public void Write( String Format, params object[] Segments)
        {
            if (Working )
            {
                // �������������� ����
                // �������������� ���������
                String Message = mkNow()
                 + "\t" + string.Format(Format, Segments);
                // ���������� ��������� � �������
                lock (this)
                {
                    StringQueue.Enqueue(new pair (IMPORTANCELEVEL.Ignore, Message));
                }
            }
        }
        /// \brief ��� �������� �������    `Console.WriteLine()` �� `Loger.WriteLine()`
        public void WriteLine( String Format, params object[] Segments)
        {
           WriteLine(IMPORTANCELEVEL.Debug, Format, Segments);
        }

        /// \brief �����-������ `Console.WriteLine()` � ����������������� �����������
        ///
        /// ����� ��������� ������ � ������� ����������     `lock (this) {}`,
        /// ������� ������ �� ������� ���������  (LogMessage()) � �������� � �������
        /// ����������� �� �������

        public void WriteLine(IMPORTANCELEVEL Importance  ///< �������� ������� ���������
                             , String Format              ///< ������ � �������� ���������
                             , params object[] Segments   ///< ��������� ���������
             ){
            // �������� ������ �������� ���������
            if (Working && Importance >= this.ImportanceLevel)
            {
                // �������������� ����
                // �������������� ���������
                String Message = mkNow()
                 + String.Format("[{0}]\t", Importance) + "\t" + string.Format(Format, Segments);
                // ���������� ��������� � �������
                lock (this)
                {
                    StringQueue.Enqueue(new pair (Importance, Message));
                }
            }
        }

        /// \brief  ����� ������ ��������� � ���� �������. �������� � ��������� ������.
        ///
        /// ��� ������ �����, ���� ���� ����� ��� ���������
        /// ��� ������������ ������� ���������  ����������� ��������  `lock (this){ }`
        /// � ������ ���������� ��������� � �������, ���� �������� �� ��������� �����.
        ///
        public void LogMessage()        {
            pair p=null;
            // ����������� ���� ������ ��������� ��� � �������
            while (Working)
            {
                // ����� ���� ���������, ���� ����
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
                      Thread.Sleep(10);//Console.WriteLine("{0} ���������", StringQueue.Count);
               }
                   // ����  ��� �� �� ��������� ���������   
                //  Thread.Sleep(1000);    
              //           ������ ��� ��� �������� ������ ���������
            }
       }

        /// \brief ����� ���������  ������ � ��� �����
        public void Stop()///����������� �������� ������� ����������, ����������� ������ �������, ������� �� ���������� � ������
        {
            Dispose();
        }

        /// \brief ����� ���������  ������ � ��� �����
        public void Dispose()
        {
            pair p=null;
            // �������� ���������� �����
            this.Working = false;
            if (Log != null)
                Log.Join();

            if (sw == null) {
               Console.Error.WriteLine(mkNow() + "Logger destructor is here...Logger has been closed alredy!");
               while (StringQueue.Count > 0)
               {
                   // ���� ���� ��� �� ���������� ���������, 
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
                // ���� ���� ��� �� ���������� ���������, 
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



        /// \brief ����� ������  ������ ������� �������� ��� ��������� ���������.
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