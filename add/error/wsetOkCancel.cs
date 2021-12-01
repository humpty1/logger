#pragma warning disable 219

//
#define PANEL
//#define LAYOUT
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms; 
using System.Text.RegularExpressions; 


namespace ut
{

  public class OkCancel : System.Windows.Forms.Form
  {
    const string _nm = "OkCancel";
    public TableLayoutPanel tPL;
    Padding  _pd ;
    public int _h = ut.SZ.X_FLD;                // the second column
                                                // label:      field
    public int _v = ut.SZ.Y_FLD;                // it is the next row
    public _Button OK_but;  //ok
    public _Button ESC_but;  //cancel
    public System.Drawing.Size bSz;

    protected int     xSize = 0; // эти поля видят наследники
    protected int     ySize = 0; // в табуляциях
    protected         SizeF sizef;

//    protected System.ComponentModel.Container components = null;
    public static int xtab (int i){
      return    SZ.X_SPC*2 +(3+ SZ.X_BUTTON) *i ;
    }
    public static int ytab (int i){
      return    SZ.Y_SPC*2  +(3+ SZ.Y) * i;
    }



    public OkCancel(string q = "question for OkCancel window"){
//    
      Text        = q;

      MinimizeBox = false;
      MaximizeBox = false;
      ControlBox = false;
      AutoScroll  = false;
  //    FormBorderStyle = FormBorderStyle.FixedDialog;
//      AutoSize = true;

//
      _pd =  new Padding(SZ.X_SPC, SZ.Y_SPC,SZ.X_SPC,0);
//      _pd =  new Padding(1);
      Graphics g = CreateGraphics();
      StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);
      sizef = g.MeasureString(q, this.Font, Int32.MaxValue, sf);


      DialogResult = DialogResult.Cancel;

      bSz = new  System.Drawing.Size(  ut.SZ.X_BUTTON+30, ut.SZ.Y+10);

      OK_but = new _Button();
      OK_but.Name = "OK_but";
      OK_but.Text = CNST.BTTN_OK;
      OK_but.DialogResult = DialogResult.OK;
      OK_but.Click    += new System.EventHandler  (_OK_but);

      ESC_but = new _Button();
      ESC_but.Name     = "ESC_but";
      ESC_but.Size     = bSz;
      ESC_but.Text     = CNST.BTTN_ESC;
      ESC_but.DialogResult = DialogResult.Cancel;
    ///  
#if LAYOUT
      tPL = new TableLayoutPanel();
      tPL.ColumnCount = 4;  // две колонки;
      tPL.RowCount  = 1;
      tPL.Padding  = _pd;
      tPL.CellBorderStyle =  TableLayoutPanelCellBorderStyle.Inset;
      tPL.AutoSize = true;
#endif
  //

      ////if (Size.Width < sizef.Width+SZ.X_SPC*2 )
      ////  ClientSize =  new Size((int)(sizef.Width+SZ.X_SPC*2), Size.Height);
     ///// this.MinimumSize = Size;

   /// 
//
    this.AcceptButton = OK_but;       // нажатие ентера как на ок
 // 
    this.CancelButton = ESC_but;        //нажатие esc
//      ESC_but.Dock = DockStyle.Fill;
//      ESC_but.Dock = DockStyle.Left;
                                // x , y
//      OK_but.Dock = DockStyle.Left;
  //
#if PANEL
    Panel p =new Panel();
    Panel p1 =new Panel();
#else 
    Button p1 =new Button();
    Button p = new Button();
#endif
      p.Size = bSz;
      p1.Size = bSz;

#if LAYOUT
      tPL.Controls.Add (OK_but, 0,0);
      tPL.Controls.Add (p, 1,0);
//
      tPL.Controls.Add (p1, 2,0);
      tPL.Controls.Add (ESC_but, 3, 0);


      Controls.Add (tPL);
      ClientSize =  tPL.Size;
      OK_but.Size = bSz;
      ESC_but.Size     = bSz;
//
      tPL.Size = new Size ( tPL.Size.Width, ytab(ySize = 1)+ SZ.Y_SPC*2);
#else
      Controls.Add (OK_but);
      OK_but.Dock   = DockStyle.Left;
      Controls.Add (p);
      Controls.Add (p1);
      Controls.Add (ESC_but);
      ESC_but.Dock   = DockStyle.Right;

#endif
     
      

      

      Location  =  new System.Drawing.Point(ut.SZ.X_SPC, ut.SZ.Y_SPC);
    }



   private void 
   _OK_but (object sender, System.EventArgs e)
   {
#if DEBUG
       Console.WriteLine ("OkCancel._OK_but: \n");
#endif

   }                           


/*
     public 
     void wr ( )
     {
        const string _me = _nm +"::wr";
        string nm = convert.mkVarNm ( Text, Screen.PrimaryScreen.Bounds.Size, true);

#if DEBUG
        gVars.log.wrLn("{0}: name is {1} \n", _me,  nm);
#endif

        gVars.var = new var(nm, false);
        gVars.var.wr(Size, Location);
        gVars.var.close();
     }


     public 
     void rd ( )
     {
        const string _me = _nm +"::wr";
        string nm = convert.mkVarNm ( Text, Screen.PrimaryScreen.Bounds.Size, true);
#if DEBUG
        gVars.log.wrLn("{0}: name is {1} \n",  _me, nm);
        gVars.log.wrLn("{0}: name is {1}, Text is {1} \n",  _me, nm, Text);
#endif
        gVars.var = new var(nm);
        Point p = new Point();
        Size sz = new Size();
        if (gVars.var.Length>0 && gVars.var.rd( ref sz, ref p)) {
          Location = p;
          StartPosition =  FormStartPosition.Manual;
          WindowState = FormWindowState.Normal;
        }
        gVars.var.close();
     }
*/
  }
}
