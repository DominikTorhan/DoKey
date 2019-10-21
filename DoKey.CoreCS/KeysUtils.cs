namespace DoKey.CoreCS
{
  public static class KeysUtils
  {

    public static bool isCaps(string str)
    {
      return str == "capital";
    }

    public static bool isAlt(string str)
    {
      if (str == "lmenu") return true;
      if (str == "rmenu") return true;
      return false;
    }

    public static bool isShift(string str)
    {
      if (str == "lshiftkey") return true;
      if (str == "rshiftkey") return true;
      if (str == "shiftkey") return true;
      return false;
    }

    public static bool isControl(string str)
    {
      if (str == "controlkey") return true;
      if (str == "lcontrolkey") return true;
      if (str == "rcontrolkey") return true;
      return false;
    }

    public static bool isWin(string str)
    {
      if (str == "lwin") return true;
      if (str == "rwin") return true;
      return false;
    }

    public static bool isModifier(string str)
    {
      if (isAlt(str)) return true;
      if (isShift(str)) return true;
      if (isControl(str)) return true;
      if (isWin(str)) return true;
      if (isCaps(str)) return true;
      return false;
    }

    public static bool isEsc(string str)
    {
      return str == "escape" || isCaps(str);
    }

    public static bool isLetterOrDigit(string str)
    {
      switch (str)
      {
        case "q": return true;
        case "w": return true;
        case "e": return true;
        case "r": return true;
        case "t": return true;
        case "y": return true;
        case "u": return true;
        case "i": return true;
        case "o": return true;
        case "p": return true;
        case "a": return true;
        case "s": return true;
        case "d": return true;
        case "f": return true;
        case "g": return true;
        case "h": return true;
        case "j": return true;
        case "k": return true;
        case "l": return true;
        case "z": return true;
        case "x": return true;
        case "c": return true;
        case "v": return true;
        case "b": return true;
        case "n": return true;
        case "m": return true;
        case "d1": return true;
        case "d2": return true;
        case "d3": return true;
        case "d4": return true;
        case "d5": return true;
        case "d6": return true;
        case "d7": return true;
        case "d8": return true;
        case "d9": return true;
        case "d0": return true;
        case "f1": return false;
        case "f2": return false;
        case "f3": return false;
        case "f4": return false;
        case "f5": return false;
        case "f6": return false;
        case "f7": return false;
        case "f8": return false;
        case "f9": return false;
        case "f10": return false;
        case "f11": return false;
        case "f12": return false;
        case "oemcomma": return true;
        case "oemperiod": return true;
        case "oemquestion": return true;
      }
      return false;
    }

  }
}
