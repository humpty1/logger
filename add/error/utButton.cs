using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms; 
using System.Collections; 
using System.Text.RegularExpressions; 


namespace ut
{



 public class _Button : System.Windows.Forms.Button  
//* Класс используется, что бы открыть  метод OnClick класса родителя.

 
  {


   public   _Button () : base()
   {

   }

   public new bool  Enabled {
    get {
      return base.Enabled;
    }
    set {
      base.Enabled = value;
    }
   }

   public void restoreEnabled(){
   }



   public     _Button 
     (                          //*  для создания кнопок стандартного размера
                                //  в заданном месте формы
       string nm,               //
       string txt,              //
       int x,                   //
       int y                    //
     ){
      Name = nm;
      Size = new System.Drawing.Size(  ut.SZ.X_BUTTON, ut.SZ.Y);
      Text = txt;
      Location = new System.Drawing.Point(x, y);
   }
/*
   public delegate void ButtonHandler (object sender, uint p);
   public  event  ButtonHandler Call;

   public void _OnCall (int i){ OnCall((uint)i);}

   public void _OnCall (uint i){ OnCall(i);}


   protected virtual void OnCall (uint i) {
//     Invalidate();
     if (Call != null) {
        Call(this, i);
     }
   
   }
*/

 }
}