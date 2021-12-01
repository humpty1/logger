#pragma warning disable 219

//#define ATTEMPT
//#define DEBUG


using System;
using System.Timers; 

using System.Data;
using System.Drawing;
using System.Windows.Forms; 
using System.Collections; 
using System.Text.RegularExpressions; 
using System.IO;
using System.Threading;
using System.Globalization;

//        gVars.log.wrLn("ut.Tbl2_Ini.constructor: vv: {0} \n", vv);

namespace ut
{
  public class warning {

   static public void 
   NRD(object sender, EventArgs e)
   {
       MessageBox.Show("this action is not ready","warning");
   }

   static public void 
   NTD(object sender, EventArgs e)
   {
       MessageBox.Show("nothing to do","warning");
   }
  }

  public struct CNST {
   public const
   bool _debug = false;        // for temporary output

   
   public const   
   int  OK = 0;

   public const   
   int  NOTITEM = 1;

   public const   
   int  CANCEL  = 10;

   public const   
   int  WPAR  = 3;

   public const   
   int  ERR  = 5;

   public const   
   int  NRD  = 6;

   public const
   int  UNK_KEY = -1;
   public const
   int  UNK_KEY2 = 0;

   public const
//   string UNK_TXT = "not set";
   string UNK_TXT = "";

   public const
   string UNK_DATE = "dd.mm.yyyy";
   public const
   string YES = "yes";
   public const
   string NO =  "no";

   public const
   string PAYMENT_TP = "11";

   public const   
   int  WHEEL_DELTA= 120;

   public const string 
     BTTN_OK    =   "OK";
   public const string 
     BTTN_ESC   =   "Cancel";
   public const string 
     BTTN_EXIT  =  "Close";
   public const string 
     BTTN_ADD    =   "Add";
   public const string 
     BTTN_DEL   =   "Delete";
   public const string 
     BTTN_EDIT  =  "Edit";
   public const string 
     BTTN_REF   =  "Refresh";
   public const string 
     BTTN_FLT  =  "Filter";
   public const string 
     BTTN_ORD  =  "Order";
   public const string 
     BTTN_EXP  =  "Export";
   public const string 
     BTTN_FLS  =  "Files";
   
   public const string 
     NULL   =   "<none>";

   public const string 
     NSF   =   "'<missing at server>'";

   public const string 
     stop0   =   "*stop*";
   public const string 
     stop1   =   "_STOP_";
   public const string 
     stop2   =   "Continue";
   public const string 
     oldService   =   "unisay";      // old service unisay +> its not necessary to put button stream in project view
 }

  public struct SZ {
   public const
   int  TD_BUF = 500; // 45; //17; //32  17;//105; //

   public const   
   int  Y = 24;

   public const   
   int  X_BUTTON = 72;

   public const   
   int  X_LABEL1  = 64;

   public const   
   int  X_LABEL2  = 124;

   public const   
   int  X_TEXT   = 124;

   public const   
   int  X_FLD  = 1;
   public const   
   int  Y_FLD  = 2;

   public const   
   int  X_SPC  = 8;
   public const   
   int  Y_SPC  = 4;
 }
 
}
