using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace Logger
{
    // Перечисление уровней важности сообщения
    public enum IMPORTANCELEVEL { Spam, Debug, Warning,  Error, FatalError, Info };

    public class LOGGER:IDisposable
    {
        // Очередь сообщений
        public Queue StringQueue = new Queue();
        // Установленный уровень важности (сообщения ниже установленного уровня - игнорируются)
        IMPORTANCELEVEL ImportanceLevel;
        // Поток для логера
        Thread Log;
        // Bool для выхода из бесконечного цикла в потоке логера
        bool Working = false;
        int Counter = 0;

        // Констурктор, создаст и запустит логер
        public LOGGER(IMPORTANCELEVEL ImportanceLevel)
        {
            this.ImportanceLevel = ImportanceLevel;
            this.Working = true;
            // Создание и старт потока логера
            Log = new Thread(new System.Threading.ThreadStart(LogMessage));
            Log.Start();
            WriteLine(IMPORTANCELEVEL.Info ,"Logger started!");
        }

        // Метод-аналог Console.WriteLine() с пользовательскими аргументами:
        // IMPORTANCELEVEL Importance - важность данного сообщения
        // String Format - форматирование строки
        // params object[] Segments - сегменты (аргументы) строки
        // (аналогично записи - Console.WriteLine("Format", Segments);)
        public void WriteLine(IMPORTANCELEVEL Importance, String Format, params object[] Segments)
        {
            // Проверка уровня важности сообщения
            if (Working && Importance >= this.ImportanceLevel)
            {
                lock(this) {
                  // Форматирование даты
                  String Date = String.Format(
                      "[" + //DateTime.Now.DayOfWeek + 
                      DateTime.Now.Day + 
                      "." + DateTime.Now.Month + 
                      "." + DateTime.Now.Year + 
                      " " + DateTime.Now.Hour + 
                      ":" + DateTime.Now.Minute + 
                      ":" + DateTime.Now.Second + 
                      "] ");
                  // Форматирование сообщения
                  String Message = Date 
                   + String.Format("[{0}]\t", Importance) +"\t"+ string.Format(Format, Segments);
                  // Добавление сообщения в очередь
                  StringQueue.Enqueue(Message);
                }
            }
        }

        // Логер. Работает в отдельном потоке
        public void LogMessage()
        {

            // Бесконечный цикл вывода сообщений раз в секунду
            while (Working)
            {
                // Вывод всех сообщений, если есть
                if (StringQueue.Count > 0)  {
                   Console.WriteLine( (String)StringQueue.Dequeue());
                   ++Counter;
                }

                // Ждем  что бы не нагружать процессор
                Thread.Sleep(10);
            }

            // Если вышли из бесконечного цикла, говорим об этом

            Console.WriteLine( "[{0}] [{1}] Logger stopped! (There was {2} messages)", 
                     DateTime.Now,
                     IMPORTANCELEVEL.Info,
                          ++Counter);
        }

        // Метод остановки потока логера
        public void Stop()//Стандартное название функции деструктор, освобождает важные ресурсы, которые не относяться к памяти
        {
            Console.Error.WriteLine("111Trying to stop logger");
            Dispose();
        }
        public void Dispose()
        {
            // Пытаемся остановить логер
            Console.Error.WriteLine("Trying to stop logger");

            while (StringQueue.Count > 0)
            {
                // Если есть еще не выведенные сообщения, 
                // говорим что логер занят и пробуем еще раз через 1 сек.
                Console.Error.WriteLine("Logger is busy");
   //
                Console.WriteLine( (String)StringQueue.Dequeue());
                //Stop();
            }
                // Если сообщений больше нет, останавливаем логер
            Console.Error.WriteLine("Stoping logger...");
            this.Working = false;
            Log.Join();
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
         
    }
    
}