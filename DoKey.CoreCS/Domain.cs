using System.Collections.Generic;

namespace DoKey.CoreCS
{

  public enum State { Off, Normal, Insert }
  public enum KeyEventType { Down, Up }

  public class InputKey
  {
    public readonly string key;
    public readonly bool isCaps;
    public readonly bool isAlt;
    public readonly bool isControl;
    public readonly bool isShift;
    public readonly bool isWin;
    public readonly bool isModif;
    public readonly bool isEsc;
    public readonly bool isLetterOrDigit;
    public readonly bool isFirstStep;

    public InputKey(string key, bool isCaps, bool isAlt, bool isControl, bool isShift, bool isWin, bool isModif, bool isEsc, bool isLetterOrDigit, bool isFirstStep)
    {
      this.key = key;
      this.isCaps = isCaps;
      this.isAlt = isAlt;
      this.isControl = isControl;
      this.isShift = isShift;
      this.isWin = isWin;
      this.isModif = isModif;
      this.isEsc = isEsc;
      this.isLetterOrDigit = isLetterOrDigit;
      this.isFirstStep = isFirstStep;
    }
  }

  public class MappedKey
  {
    public readonly string trigger;
    public readonly string send;
    public readonly bool isCaps;

    public MappedKey(string trigger, string send, bool isCaps)
    {
      this.trigger = trigger;
      this.send = send;
      this.isCaps = isCaps;
    }
  }

  public class CommandKey
  {
    public readonly string trigger;
    public readonly string run;

    public CommandKey(string trigger, string run)
    {
      this.trigger = trigger;
      this.run = run;
    }
  }

  public class Modificators
  {
    public readonly bool control;
    public readonly bool shift;
    public readonly bool alt;
    public readonly bool win;
    public readonly bool caps;

    public Modificators(bool control, bool shift, bool alt, bool win, bool caps)
    {
      this.control = control;
      this.shift = shift;
      this.alt = alt;
      this.win = win;
      this.caps = caps;
    }

    public override string ToString()
    {
      var conStr = control ? "^" : "";
      var shtStr = shift ? "+" : "";
      var altStr = alt ? "%" : "";
      var winStr = win ? "w" : "";
      var capStr = caps ? "c" : "";
      return conStr + shtStr + altStr + winStr + capStr;
    }
  }

  public class AppState
  {
    public readonly State state;
    public readonly Modificators modificators;
    public readonly string firstStep;
    public readonly bool preventEscOnCapsUp;
    public AppState(State state, Modificators modificators, string firstStep, bool preventEscOnCapsUp)
    {
      this.state = state;
      this.modificators = modificators;
      this.firstStep = firstStep;
      this.preventEscOnCapsUp = preventEscOnCapsUp;
    }

    public override string ToString()
    {
      return $"{(int)state} {modificators} {firstStep} {preventEscOnCapsUp}";
    }
  }

  public class KeysProcessorResult
  {
    public readonly AppState appState;
    public readonly string send;
    public readonly bool preventKeyProcess;

    public KeysProcessorResult(AppState appState, string send, bool preventKeyProcess)
    {
      this.appState = appState;
      this.send = send;
      this.preventKeyProcess = preventKeyProcess;
    }

    public override string ToString()
    {
      return $"{appState} {send} {preventKeyProcess}";
    }
  }

  public class Config
  {
    public readonly IEnumerable<MappedKey> mappedKeys;
    public readonly IEnumerable<CommandKey> commandKeys;

    public Config(IEnumerable<MappedKey> mappedKeys, IEnumerable<CommandKey> commandKeys)
    {
      this.mappedKeys = mappedKeys;
      this.commandKeys = commandKeys;
    }
  }

  public class Session
  {
    public readonly Config config;
    public readonly AppState appState;
    public Session(Config config, AppState appState)
    {
      this.config = config;
      this.appState = appState;
    }
  }

}
