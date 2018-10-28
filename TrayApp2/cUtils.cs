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

    public static bool IsLetterKey(Keys key) {

      var chr = (char)key;

      switch (key) {
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
          return false;
        case Keys.Oemcomma:
        case Keys.OemPeriod:
        case Keys.OemQuestion:
          return true;
      }

      return char.IsLetterOrDigit(chr);

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

    //public static bool IsModifierKey(Keys key) {

    //  if (IsAlt(key)) return true;
    //  if (IsShift(key)) return true;
    //  if (IsControl(key)) return true;
    //  if (IsWin(key)) return true;
    //  if (IsCaps(key)) return true;

    //  return false;

    //} 

    //public static bool IsCaps(Keys key) {

    //  switch (key) {
    //    case Keys.Capital:
    //      return true;
    //    default:
    //      return false;
    //  }

    //}

    //public static bool IsAlt(Keys key) {

    //  switch (key) {
    //    case Keys.Alt:
    //    case Keys.LMenu:
    //    case Keys.RMenu:
    //      return true;
    //    default:
    //      return false;
    //  }

    //}

    //public static bool IsShift(Keys key) {

    //  switch (key) {
    //    case Keys.LShiftKey:
    //    case Keys.ShiftKey:
    //    case Keys.RShiftKey:
    //    case Keys.Shift:
    //      return true;
    //    default:
    //      return false;
    //  }

    //}

    //public static bool IsControl(Keys key) {

    //  switch (key) {
    //    case Keys.Control:
    //    case Keys.ControlKey:
    //    case Keys.LControlKey:
    //    case Keys.RControlKey:
    //      return true;
    //    default:
    //      return false;
    //  }

    //}

    //public static bool IsWin(Keys key) {

    //  switch (key) {
    //    case Keys.LWin:
    //    case Keys.RWin:
    //      return true;
    //    default:
    //      return false;
    //  }

    //}

    //public static bool IsEsc(Keys keys) {
    //  switch (keys) {
    //    case Keys.Escape:
    //    case Keys.Capital:
    //      return true;
    //    default:
    //      return false;
    //  }
    //}

  }
}
