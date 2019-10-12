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
      return new InputKey
      {
        key = str,
        isAlt = isAlt(str),
        isCaps = isCaps(str),
        isControl = isControl(str),
        isShift = isShift(str),
        isWin = isWin(str),
        isLetterOrDigit = isLetterOrDigit(str),
        isModif = isModifier(str),
        isEsc = isEsc(str),
        isFirstStep = IsTwoStep(str)
      };
    }

    public static Modificators CreateMoficators()
    {
      return new Modificators
      {
        alt = false,
        control = false,
        shift = false,
        win = false,
        caps = false
      };
    }

    public static AppState CreateAppState()
    {
      return new AppState
      {
        state = State.Off,
        modificators = CreateMoficators(),
        firstStep = "",
        preventEscOnCapsUp = false
      };
    }

    public static Session CreateSession(Func<string> funcReadText)
    {
      var config = new ConfigFileReader(funcReadText).CreateConfigByFile();
      return new Session { config = config, appState = CreateAppState() };
    }

    private static bool IsTwoStep(string key) => twoStep.Contains(key);


  }

}
