#pragma warning disable 219

//#define ATTEMPT
//#define DEBUG


using System;

using System.Data;
using Logger;

//        gVars.log.wrLn("ut.Tbl2_Ini.constructor: vv: {0} \n", vv);

namespace Logger
{

  public class usingLogger{

        protected string me {
           get{
                return this.GetType().ToString();
           }
        }
        protected Loger l; 
        public usingLogger(Loger log){
           l = log;
        }

        public void WriteLine (String Format              ///< строка с форматом сообщения
                             , params object[] Segments   ///< параметры сообщения
                    ){
            if (l!=null)
                l.WriteLine( String.Format("{0}->{1}", me, Format)
                    , Segments);
        }

        public void WriteException(Exception e)
        {
            if (l!=null) {
              l.Write("{0}", me);
              l.WriteException( e);
            }
        }


        public void WriteLine (IMPORTANCELEVEL Importance  ///< важность данного сообщения
                             , String Format              ///< строка с форматом сообщения
                             , params object[] Segments   ///< параметры сообщения
                    ){
            if (l!=null)
                l.WriteLine(Importance
                  , String.Format("{0}->{1}", me, Format)
                    , Segments);
        }
  
  }
  
}
