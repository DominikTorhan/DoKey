using DoKey.CoreCS;
using System;

namespace DoKey.TestsCS
{
  public static class TestHelpers
  {

    public static Modificators CreateModificators(string str)
    {
      //^+%cw
      var control = str.Contains("^");
      var alt = str.Contains("%");
      var shift = str.Contains("+");
      var caps = str.Contains("c");
      var win = str.Contains("w");
      return new Modificators(control, shift, alt, win, caps);
    }

    public static AppState CreateAppState(string str)
    {
      //0 null m=
      //1 null m=%
      //1 f m=%^c prevent
      var strs = str.Split();
      var state = (State)Convert.ToInt32(strs[0]);
      var firstStep = strs[1] == "null" ? "" : strs[1];
      return new AppState(state, CreateModificators(strs[2]), firstStep, str.Contains("prevent"));
    }

    public static Func<string> GetFuncGetConfig()
    {
      string path = @"C:\Users\dominik\Source\Repos\DoKey\DoKey.App\config.txt";
      return () => System.IO.File.ReadAllText(path);
    }

    public static Config CreateConfig()
    {
      return new ConfigFileReader(GetFuncGetConfig()).CreateConfigByFile();
    }

  }
}
