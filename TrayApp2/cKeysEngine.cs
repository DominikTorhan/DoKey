using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoKey.FS;

namespace TrayApp2
{

  public class cOutput
  {
    public SendDoKey sendDoKey { get; set; }
    public AppState AppState { get; set; }
    public bool PreventKeyProcess { get; set; }

    public string GetStr()
    {
      string pStr = sendDoKey?.Send ?? "";
      string pModif = AppState.Modificators.ToStr;
      if (!PreventKeyProcess) pStr += "{GO}";
      if (pModif != "") pStr += "(" + AppState.Modificators.ToStr + ")";
      return pStr;
    }

  }

  public class cKeysEngine
  {

    public Configuration Configuration { get; set; }
    public KeyEventData KeyEventData { get; set; }
    public AppState AppState { get; set; }
    public SendDoKey SendDoKeyLast { get; set; }

    private InputKey inputKey => KeyEventData.InputKey;
    private cOutput outputOld => new cOutput { AppState = AppState };
    private string keys => inputKey.Key;
    private bool isUp => KeyEventData.KeyEventType.IsUp;
    private string firstStep => AppState.FirstStep;
    private State State => AppState.State;
    private Modificators modificators => AppState.Modificators;
    private bool isCapital => AppState.Modificators.Caps;

    public cOutput ProcessKey()
    {

      var output = ProcessModificators();
      if (output != null) return output;

      output = ProcessSetModeOff();
      if (output != null) return output;

      output = ProcessModeChange();
      if (output != null) return output;

      output = ProcessEsc();
      if (output != null) return output;

      if (State == State.Off) return outputOld;

      output = ProcessNormalAndInsertWithCapital();
      if (output != null) return output;

      output = ProcessNormalMode();
      if (output != null) return output;

      return outputOld;
    }

    private cOutput ProcessModificators()
    {

      if (!inputKey.IsModifier) return null;
      //if (!cUtils.IsModifierKey(keys)) return null;

      if (inputKey.IsCapital) return ProcessCapital();

      var modificators = this.modificators.GetNextModificators(inputKey, isUp);

      var r = NextOutput();

      //if (inputKey.IsAlt && isUp) { 
      //  if (SendDoKeyLast.IsAlt)        {
      //    modificators = this.modificators;
      //    r.AppState = new AppState(r.AppState.State, modificators, r.AppState.FirstStep, r.AppState.PreventEscOnCapsUp);
      //  }
      //}

      //r.AppState.Modificators = modificators;
      r.AppState = new AppState(r.AppState.State, modificators, r.AppState.FirstStep, r.AppState.PreventEscOnCapsUp);

      return r;
    }

    private cOutput ProcessCapital()
    {

      var sendKeys = "";
      var preventEscOnNextCapitalUp = AppState.PreventEscOnCapsUp;

      if (isUp)
      {
        if (AppState.PreventEscOnCapsUp)
        {
          preventEscOnNextCapitalUp = false;
        }
        else
        {
          sendKeys = "{ESC}";
        }
      }

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.sendDoKey = new SendDoKey(sendKeys);

      var modif = this.modificators.GetNextModificators(inputKey, isUp);

      r.AppState = new AppState(r.AppState.State, modif, "", preventEscOnNextCapitalUp);

      return r;

    }

    private cOutput ProcessEsc()
    {

      if (!inputKey.IsEsc) return null;
      if (State != State.Insert) return null;
      if (isUp) return null;
      if (isCapital) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.AppState = new AppState(cUtils.GetPrevState(State), r.AppState.Modificators, "", r.AppState.PreventEscOnCapsUp);

      return r;

    }

    private string NormalModeKeysToString() => firstStep + keys.ToString();
    private bool IsDownFirstStep() => firstStep == "" && Configuration.IsTwoStep(keys);

    private cOutput ProcessNormalMode()
    {

      if (State != State.Normal) return null;
      if (isUp) return null;
      if (modificators.Win) return null;

      var isDownFirstStep = IsDownFirstStep();

      var firstStepNext = isDownFirstStep ? keys : "";

      var sendDoKey = GetSendDoKey(isDownFirstStep);

      //var preventNextAltUp = sendDoKey.IsAlt;
      var preventKeyProcess = inputKey.IsLetterOrDigit || !sendDoKey.IsEmpty;

      var r = NextOutput();
      r.sendDoKey = sendDoKey;
      r.PreventKeyProcess = preventKeyProcess;
      r.AppState = new AppState(r.AppState.State, r.AppState.Modificators, firstStepNext, r.AppState.PreventEscOnCapsUp);
      return r;

    }

    private SendDoKey GetSendDoKey(bool isDownFirstStep)
    {

      if (isDownFirstStep) return new SendDoKey("");

      var trigger = NormalModeKeysToString();

      var doKey = Configuration.GetSendKeyNormal(trigger);

      //if (modificators.Shift)
      //{
      //  if (!doKey.IsShiftAllowed) return new SendDoKey("");
      //}
      //if (modificators.Alt)
      //{
      //  if (!doKey.IsAltAllowed) return new SendDoKey("");
      //}

      return doKey;

    }

    private cOutput ProcessSetModeOff()
    {

      if (!isCapital) return null;
      if (isUp) return null;
      if (keys != Configuration.ModeOffKey) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.AppState = new AppState(State.Off, r.AppState.Modificators, "", true);

      return r;


    }

    private cOutput ProcessNormalAndInsertWithCapital()
    {

      if (State == State.Off) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var sendKeys = Configuration.GetSendKeyCaps(keys.ToString());

      if (sendKeys.IsEmpty) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.sendDoKey = sendKeys;
      r.AppState = new AppState(r.AppState.State, r.AppState.Modificators, "", true);
      return r;

    }

    private cOutput ProcessModeChange()
    {

      if (keys != Configuration.ModeChangeKey) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.AppState = new AppState(cUtils.GetNextState(State), r.AppState.Modificators, "", true);
      return r;

    }

    private cOutput NextOutput() => new cOutput { AppState = NextState() };
    private AppState NextState() => AppState;

  }
}

