using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using  Logger;

class Program
{
    //Обьявили глобально для получения доступа в классе ProgExp со своими исключениями
    static LOGGER Logger = null;//new LOGGER(IMPORTANCELEVEL.Warning);
    [STAThread]
    static void Main(string[] args)
    {
     Console.ReadLine();
     using (LOGGER Logger = new LOGGER(IMPORTANCELEVEL.Warning)) {
     //   LOGGER L = new LOGGER(IMPORTANCELEVEL.Spam);
    //    LOGGER L1 = new LOGGER(IMPORTANCELEVEL.Spam);
    ///    LOGGER L2 = new LOGGER(IMPORTANCELEVEL.Spam);
     //   LOGGER L3 = new LOGGER(IMPORTANCELEVEL.Spam);
      //  LOGGER L4 = new LOGGER(IMPORTANCELEVEL.Spam);
   

     using (LOGGER L = new LOGGER(IMPORTANCELEVEL.Warning) )
     { 
             L.WriteLine(IMPORTANCELEVEL.Error, "{0} - {1} - {2}", "Arg 1", "Arg 2", "Arg 3");
        
        using (LOGGER L2 = new LOGGER(IMPORTANCELEVEL.Warning) )
        { 
             L2.WriteLine(IMPORTANCELEVEL.Error, "{0} - {1} - {2}", "Arg 1", "Arg 2", "Arg 3");
        // DLL файл подключен
        // Создание/запуск логера
        // Уровни важности по убыванию - FatalError, Error, Warning, Debug, Spam
        // (например, передав уровеь Error, будем получать только Error и FatalError)
        //LOGGER Logger = new LOGGER(IMPORTANCELEVEL.Warning);
        //добавляем в делегат функцию
        Logger.MyException += ProgExcp.WriteMyException;
        //Logger.WriteException(e);

        // Имитация подачи сообщений
        Logger.WriteLine(IMPORTANCELEVEL.Error, "{0} - {1} - {2}", "Arg 1", "Arg 2", "Arg 3");
        Thread.Sleep(1000);
        Logger.WriteLine(IMPORTANCELEVEL.Warning, "{0} - {1}", "Arg 1", "Arg 2");
        Thread.Sleep(2000);
        Logger.WriteLine(IMPORTANCELEVEL.Debug, "{0} - {1}", "Arg 1", "Arg 2");
        Thread.Sleep(5000);
        Logger.WriteLine(IMPORTANCELEVEL.Error, "{0} - {1} - {2}", "Arg 1", "Arg 2", "Arg 3");
        Thread.Sleep(100);
        Logger.WriteLine(IMPORTANCELEVEL.Warning, "{0} - {1}", "Arg 1", "Arg 2");
        Thread.Sleep(1000);
        Logger.WriteLine(IMPORTANCELEVEL.Debug, "{0} - {1}", "Arg 1", "Arg 2");
        Thread.Sleep(500);
        Logger.WriteLine(IMPORTANCELEVEL.Error, "{0} - {1} - {2}", "Arg 1", "Arg 2", "Arg 3");
        Thread.Sleep(1000);
        Logger.WriteLine(IMPORTANCELEVEL.Warning, "{0} - {1}", "Arg 1", "Arg 2");
        Thread.Sleep(2000);
        Logger.WriteLine(IMPORTANCELEVEL.Debug, "{0} - {1}", "Arg 1", "Arg 2");
        Thread.Sleep(500);
        Logger.WriteLine(IMPORTANCELEVEL.Error, "{0} - {1} - {2}", "Arg 1", "Arg 2", "Arg 3");
        Thread.Sleep(1000);
        Logger.WriteLine(IMPORTANCELEVEL.Warning, "{0} - {1}", "Arg 1", "Arg 2");
        Thread.Sleep(1000);
        Logger.WriteLine(IMPORTANCELEVEL.Debug, "{0} - {1}", "Arg 1", "Arg 2");

        //Выход
        //Logger.Stop(); 

        try
        {
            //здесь должна возникнуть ошибка в коде
            throw new ProgExcp("Специфическая информация oб ошибках из моего приложения");
        }
        catch (ProgExcp e)
        {
            Logger.WriteException(e);
            L.WriteException(e);
        }




 ///       Logger.LogMessage();
       // L.Stop(); 
        Thread.Sleep(10000);
      //  Logger.Stop(); 
      }
     }
     }
     Thread.Sleep(10000);
    
    }
    //специфический класс со своими исключениями
    class ProgExcp: Exception
    {
        public string msg;//print
        public ProgExcp(string s)
        {
            msg = s;
        }
        public static void WriteMyException( object o, Exception e)
        {                                               
            ProgExcp MyEx = e as ProgExcp;
            LOGGER   l    = o as LOGGER;
            if(MyEx!=null && l != null)
                l.WriteLine(IMPORTANCELEVEL.Error, "Add info: {0}", MyEx.msg);//записываем в логер исключение
        }
    }
}
