using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2 {

  internal enum StateEnum {
    Off = 0,
    Normal = 1,
    Insert = 2
  }

  internal class SendKeysStrings {
    internal static string ArrowUp = "{UP}";
    internal static string ArrowDown = "{DOWN}";
    internal static string ArrowLeft = "{LEFT}";
    internal static string ArrowRight = "{RIGHT}";
    internal static string Backspace = "{BKSP}";
    internal static string Delete = "{DEL}";
    internal static string End = "{END}";
    internal static string CrtlEnd = "^{END}";
    internal static string Enter = "{ENTER}";
    internal static string EndSpaceEnter = "{End} {ENTER}";
    internal static string UpEndEnter = "{UP}{End}{ENTER}";
    internal static string Escape = "{ESC}";
    internal static string Home = "{HOME}";
    internal static string CrtlHome = "^{HOME}";
    internal static string DoubleHome = "{HOME} 2";
    internal static string PageDown = "{PGDN}";
    internal static string PageUp = "{PGUP}";
    internal static string HalfPageDown = "{Down 20}";
    internal static string HalfPageUp = "{Up 20}";
    internal static string Tab = "{TAB}";
    internal static string CrtlZ = "^z";

  }

  internal class cUtils {

    internal static bool IsIgnoredKey(Keys key) {

      switch (key) {
        case Keys.Capital:
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
          return true;
      }

      return false;

    }

    internal static bool IsInsertKey(Keys keys) {
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

    internal static bool IsLetterKey(Keys key) {

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
      }

      return char.IsLetterOrDigit(chr);

    }

    internal static bool IsModifierKey(Keys key) {

      if (IsAlt(key)) return true;
      if (IsShift(key)) return true;
      if (IsControl(key)) return true;
      if (IsWin(key)) return true;

      return false;

    }

    internal static bool IsAlt(Keys key) {

      switch (key) {
        case Keys.Alt:
        case Keys.LMenu:
        case Keys.RMenu:
          return true;
        default:
          return false;
      }

    }

    internal static bool IsShift(Keys key) {

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

    internal static bool IsControl(Keys key) {

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

    internal static bool IsWin(Keys key) {

      switch (key) {
        case Keys.LWin:
        case Keys.RWin:
          return true;
        default:
          return false;
      }

    }

    internal static string GetSendKeyByKeyInsertModeWithControl(Keys key) {

      switch (key) {
        case Keys.H: return SendKeysStrings.Backspace;
        case Keys.J: return SendKeysStrings.Enter;
        case Keys.K: return SendKeysStrings.UpEndEnter;
        case Keys.L: return SendKeysStrings.Delete;
        case Keys.I: return SendKeysStrings.Tab;
        case Keys.U: return SendKeysStrings.CrtlZ;
        case Keys.Oemcomma: return SendKeysStrings.Home;
        case Keys.OemPeriod: return SendKeysStrings.End;
        default: return "";
      }
    }

    internal static string GetSendKeyByKeyNormalMode(Keys key, cSettings settings) {

      return settings.SendKeyNormal(key.ToString());

      //bez shifta
      //switch (keys) {
      //  case Keys.H: return SendKeysStrings.ArrowLeft;
      //  case Keys.J: return SendKeysStrings.ArrowDown;
      //  case Keys.K: return SendKeysStrings.ArrowUp;
      //  case Keys.L: return SendKeysStrings.ArrowRight;
      //  case Keys.A: return SendKeysStrings.ArrowRight;//insert mode
      //  case Keys.X: return SendKeysStrings.Delete;
      //  case Keys.U: return SendKeysStrings.CrtlZ;
      //  default:
      //    return "";
      //}

    }

    internal static string GetSendKeyByKeyNormalModeWithShift(Keys keys) {

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

    internal static string GetSendKeyByKeyNormalModeWithControl(Keys keys) {

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
