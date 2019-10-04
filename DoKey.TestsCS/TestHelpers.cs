using DoKey.CoreCS;
using System;

namespace DoKey.TestsCS
{
  public static class TestHelpers
  {

    public static Modificators CreateModificators(string str)
    {
      //^+%cw
      return new Modificators
      {
        control = str.Contains("^"),
        alt = str.Contains("%"),
        shift = str.Contains("+"),
        caps = str.Contains("c"),
        win = str.Contains("w"),
      };
    }

    public static AppState CreateAppState(string str)
    {
      //0 null m=
      //1 null m=%
      //1 f m=%^c prevent
      var strs = str.Split();
      var state = (State)Convert.ToInt32(strs[0]);
      var firstStep = strs[1] == "null" ? "" : strs[1];
      return new AppState
      {
        state = state,
        firstStep = firstStep,
        modificators = CreateModificators(strs[2]),
        preventEscOnCapsUp = str.Contains("prevent")
      };
    }

  }
}
