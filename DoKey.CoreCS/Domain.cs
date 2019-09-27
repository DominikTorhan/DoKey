using System.Collections.Generic;

namespace DoKey.CoreCS
{

  public enum State { Off, Normal, Insert }
  public enum KeyEventType { Down, Up }

  public class InputKey
  {
    string key;
    bool isCaps;
    bool isAlt;
    bool isControl;
    bool isShift;
    bool isWin;
    bool isModif;
    bool isEsc;
    bool isLetterOrDigit;
  }

  public class KeyEventData
  {
    InputKey inputKey;
    KeyEventType keyEventType;
  }

  public class MappedKey
  {
    public string trigger;
    public string send;
    public bool isCaps;
  }

  public class CommandKey
  {
    string trigger;
    string run;
  }

  public class Modificators
  {
    bool alt;
    bool control;
    bool shift;
    bool win;
    bool caps;
  }


  public class AppState
  {
    State state;
    Modificators modificators;
    string firstStep;
    bool eventEscOnCapsUp;
  }

  public class KeysEngineResult
  {
    AppState appState;
    string send;
    bool preventKeyProcess;
  }

  public class Config
  {
    public IEnumerable<MappedKey> mappedKeys;
    public IEnumerable<CommandKey> commandKeys;
  }

  public class Session
  {
    Config config;
    AppState appState;
  }

  public class Domain
  {
    public const string filePathNew = "config.txt";
    public const string modeChangeKey = "f";
    public const string modeOffKey = "q";
    public const string twoStep = "qwertasdfgbui";

    State GetNextState(State state)
    {
      if (state == State.Insert) return State.Insert;
      if (state == State.Normal) return State.Insert;
      return State.Normal;
    }

    State GetPrevState(State state)
    {
      if (state == State.Insert) return State.Normal;
      if (state == State.Normal) return State.Normal;
      return State.Off;
    }

    State? GetNextStateByKey(State state, string key)
    {
      if (key == modeOffKey) return State.Off;
      if (key == modeChangeKey) return GetNextState(state);
      return null;
    }

  }

}
