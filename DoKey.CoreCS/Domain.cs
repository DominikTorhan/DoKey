using System.Collections.Generic;

namespace DoKey.CoreCS
{

  public enum State { Off, Normal, Insert }
  public enum KeyEventType { Down, Up }

  public class InputKey
  {
    public string key;
    public bool isCaps;
    public bool isAlt;
    public bool isControl;
    public bool isShift;
    public bool isWin;
    public bool isModif;
    public bool isEsc;
    public bool isLetterOrDigit;
    public bool isFirstStep;
  }

  public class KeyEventData
  {
    public InputKey inputKey;
    public KeyEventType keyEventType;
    public override string ToString()
    {
      return $"{keyEventType} {inputKey.key}";
    }
  }

  public class MappedKey
  {
    public string trigger;
    public string send;
    public bool isCaps;
  }

  public class CommandKey
  {
    public string trigger;
    public string run;
  }

  public class Modificators
  {
    public bool control;
    public bool shift;
    public bool alt;
    public bool win;
    public bool caps;
    public override string ToString()
    {
      var con = this.control ? "^" : "";
      var sht = this.shift ? "+" : "";
      var alt = this.alt ? "%" : "";
      var win = this.win? "w" : "";
      var cap = this.caps ? "c" : "";
      return con + sht + alt + win + cap;
    }
  }


  public class AppState
  {
    public State state;
    public Modificators modificators;
    public string firstStep;
    public bool preventEscOnCapsUp;
    public override string ToString()
    {
      return $"{(int)state} {modificators} {firstStep} {preventEscOnCapsUp}";
    }
  }

  public class KeysEngineResult
  {
    public AppState appState;
    public string send;
    public bool preventKeyProcess;
    public override string ToString()
    {
      return $"{appState} {send} {preventKeyProcess}";
    }
  }

  public class Config
  {
    public IEnumerable<MappedKey> mappedKeys;
    public IEnumerable<CommandKey> commandKeys;
  }

  public class Session
  {
    public Config config;
    public AppState appState;
  }

}
