using DoKey.FS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DoKey.FS.Domain;

namespace TrayApp2 {

  public class cUtils {

    //oemcomma    ,
    //oemperiod   .
    //oem2        / 
    //oem1        ;
    //oem7        '
    //oem4        [
    //oem6        ]
    //oem5        \
    //oemminus    -
    //oemplus     =

    public static bool IsIgnoredKey(Keys key, Keys modifiers) {

      switch (key) {
        //case Keys.Capital:
        case Keys.Down:
        case Keys.Left:
        case Keys.Up:
        case Keys.Right:
        case Keys.PageDown:
        case Keys.PageUp:
        case Keys.End:
        case Keys.Home:
        case Keys.Back:
        case Keys.Delete:
        case Keys.Enter:
        case Keys.Tab:
        case Keys.LControlKey:
        case Keys.F1:
        case Keys.F2:
        case Keys.F3:
        case Keys.F4:
        case Keys.F5:
        case Keys.F6:
        case Keys.F7:
        case Keys.F8:
        case Keys.F9:
        case Keys.F10:
        case Keys.F11:
        case Keys.F12:
          return true;
      }

      if (modifiers == Keys.Control) {
        switch (key) {
          case Keys.C:
          case Keys.X:
          case Keys.Z:
          case Keys.V:
          case Keys.F:
          case Keys.R:
          case Keys.T:
            return true;
        }
      }

      return false;

    }

    public static State GetNextState(State state) {

      if (state == State.Insert) return State.Insert;
      if (state == State.Normal) return State.Insert;

      return State.Normal;

    }

    public static State GetPrevState(State state) {

      if (state == State.Insert) return State.Normal;
      if (state == State.Normal) return State.Normal;

      return State.Off;

    }

  }
}

