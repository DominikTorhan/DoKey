using System;
using static DoKey.CoreCS.KeysUtils;

namespace DoKey.CoreCS
{
  public static class DomainUtils
  {
    public const string modeChangeKey = "f";
    public const string modeOffKey = "q";
    public const string twoStep = "qwertasdfgbui";

    public static State GetNextState(State state)
    {
      if (state == State.Insert) return State.Insert;
      if (state == State.Normal) return State.Insert;
      return State.Normal;
    }

    public static State GetPrevState(State state)
    {
      if (state == State.Insert) return State.Normal;
      if (state == State.Normal) return State.Normal;
      return State.Off;
    }

    public static State? GetNextStateByKey(State state, string key)
    {
      if (key == modeOffKey) return State.Off;
      if (key == modeChangeKey) return GetNextState(state);
      return null;
    }

    public static InputKey CreateInputKey(string stra)
    {
      string str = stra.ToLower();
      return new InputKey(str, isCaps(str), isAlt(str), isControl(str), isShift(str), isWin(str),
        isModifier(str), isEsc(str), isLetterOrDigit(str), IsTwoStep(str));
    }

    public static Modificators CreateMoficators()
    {
      return new Modificators(false, false, false, false, false);
    }

    public static AppState CreateAppState()
    {
      return new AppState(State.Off, CreateMoficators(), "", false);
    }

    public static Session CreateSession(Func<string> funcReadText)
    {
      var config = new ConfigFileReader(funcReadText).CreateConfigByFile();
      return new Session(config, CreateAppState());
    }

    private static bool IsTwoStep(string key)
    {
      return twoStep.Contains(key);
    }
  }

}
