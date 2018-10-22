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

  public class SendKeysStrings {
    public static string ArrowUp = "{UP}";
    public static string ArrowDown = "{DOWN}";
    public static string ArrowLeft = "{LEFT}";
    public static string ArrowRight = "{RIGHT}";
    public static string Backspace = "{BKSP}";
    public static string Delete = "{DEL}";
    public static string End = "{END}";
    public static string CrtlEnd = "^{END}";
    public static string Enter = "{ENTER}";
    public static string EndSpaceEnter = "{End} {ENTER}";
    public static string UpEndEnter = "{UP}{End}{ENTER}";
    public static string Escape = "{ESC}";
    public static string Home = "{HOME}";
    public static string CrtlHome = "^{HOME}";
    public static string DoubleHome = "{HOME} 2";
    public static string PageDown = "{PGDN}";
    public static string PageUp = "{PGUP}";
    public static string HalfPageDown = "{Down 20}";
    public static string HalfPageUp = "{Up 20}";
    public static string Tab = "{TAB}";
    public static string CrtlZ = "^z";

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

    public static bool IsInsertKey(Keys keys) {
      switch (keys) {
        case Keys.A:
        case Keys.I:
        case Keys.O:
        case Keys.C:
          return true;
        default:
          return false;
      }
      
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

    public static bool IsToggleKey(bool isControl, Keys key) {

      if (!isControl) return false;
      if (key != Keys.Space) return false;

      return true;

    }

    public static StateEnum GetNextState(StateEnum state) {

      if (state == StateEnum.Insert) return StateEnum.Off;

      return state + 1;

    }

    public static bool IsModifierKey(Keys key) {

      if (IsAlt(key)) return true;
      if (IsShift(key)) return true;
      if (IsControl(key)) return true;
      if (IsWin(key)) return true;

      return false;

    }

    public static bool IsAlt(Keys key) {

      switch (key) {
        case Keys.Alt:
        case Keys.LMenu:
        case Keys.RMenu:
          return true;
        default:
          return false;
      }

    }

    public static bool IsShift(Keys key) {

      switch (key) {
        case Keys.LShiftKey:
        case Keys.ShiftKey:
        case Keys.RShiftKey:
        case Keys.Shift:
          return true;
        default:
          return false;
      }

    }

    public static bool IsControl(Keys key) {

      switch (key) {
        case Keys.Control:
        case Keys.ControlKey:
        case Keys.LControlKey:
        case Keys.RControlKey:
          return true;
        default:
          return false;
      }

    }

    public static bool IsWin(Keys key) {

      switch (key) {
        case Keys.LWin:
        case Keys.RWin:
          return true;
        default:
          return false;
      }

    }

    public static bool IsEsc(Keys keys) {
      switch (keys) {
        case Keys.Escape:
        case Keys.Capital:
          return true;
        default:
          return false;
      }
    }

    public static string GetSendKeyByKeyInsertModeWithControl(Keys key, cSettings settings) {

      return settings.SendKeyInsert(key.ToString());

      //switch (key) {
      //  case Keys.H: return SendKeysStrings.Backspace;
      //  case Keys.J: return SendKeysStrings.Enter;
      //  case Keys.K: return SendKeysStrings.UpEndEnter;
      //  case Keys.L: return SendKeysStrings.Delete;
      //  case Keys.I: return SendKeysStrings.Tab;
      //  case Keys.U: return SendKeysStrings.CrtlZ;
      //  case Keys.Oemcomma: return SendKeysStrings.Home;
      //  case Keys.OemPeriod: return SendKeysStrings.End;
      //  default: return "";
      //}
    }


    public static string GetSendKeyByKeyNormalModeWithShift(Keys keys) {

      //shift
      switch (keys) {
        case Keys.D4: return SendKeysStrings.End;
        case Keys.D6: return SendKeysStrings.Home;
        case Keys.D0: return SendKeysStrings.DoubleHome;
        case Keys.I: return SendKeysStrings.Home;//insert mode
        case Keys.A: return SendKeysStrings.End;//insert mode
        case Keys.X: return SendKeysStrings.Backspace;
        case Keys.G: return SendKeysStrings.CrtlEnd;
        case Keys.J: return SendKeysStrings.EndSpaceEnter;
        default:
          return "";
      }

    }

    public static string GetSendKeyByKeyNormalModeWithControl(Keys keys) {

      switch (keys) {
        case Keys.H: return SendKeysStrings.Home;
        case Keys.J: return SendKeysStrings.HalfPageDown;
        case Keys.K: return SendKeysStrings.HalfPageUp;
        case Keys.L: return SendKeysStrings.End;
        case Keys.F: return SendKeysStrings.PageDown;
        case Keys.B: return SendKeysStrings.PageUp;
        case Keys.D: return SendKeysStrings.HalfPageDown;
        case Keys.U: return SendKeysStrings.HalfPageUp;
        default:
          return "";
      }

    }


  }
}
