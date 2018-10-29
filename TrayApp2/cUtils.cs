using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2 {

  public enum StateEnum {
    Off = 0,
    Normal = 1,
    Insert = 2
  }

  public class cUtils {

    public static bool IsIgnoredKey(Keys key) {

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

      return false;

    }

    public static StateEnum GetNextState(StateEnum state) {

      if (state == StateEnum.Insert) return StateEnum.Insert;

      return state + 1;

    }

    public static StateEnum GetPrevState(StateEnum state) {

      if (state == StateEnum.Insert) return StateEnum.Normal;
      if (state == StateEnum.Normal) return StateEnum.Normal;

      return StateEnum.Off;

    }

  }
}
