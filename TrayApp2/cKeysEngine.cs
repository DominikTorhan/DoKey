using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoKey.FS;

namespace TrayApp2 {

  public class cInput {
    public KeyEventData KeyEventData { get; set; } 
    public AppState AppState { get; set; }
  }

  public class cOutput { 
    public SendDoKey sendDoKey { get; set; } 
    public AppState AppState { get; set; }
    public bool PreventKeyProcess { get; set; }

    public override string ToString() {
      var prevent = PreventKeyProcess ? "p " : "";
      var send = sendDoKey != null ? sendDoKey.Send : "";
      return prevent + send + " " + AppState.ToLog();
    }
  }

  public class cKeysEngine {

    public cInput input { get; set; }
    public cSettings settings { get; set; }

    private InputKey inputKey => input.KeyEventData.InputKey;
    private cOutput outputOld => new cOutput { AppState = input.AppState};
    private string keys => inputKey.Key;
    private bool isUp => input.KeyEventData.KeyEventType.IsUp;
    private string firstStep => input.AppState.FirstStep;
    private State State => input.AppState.State;
    private Modificators modificators => input.AppState.Modificators;
    private bool isCapital => input.AppState.Modificators.Caps;

    public cOutput ProcessKey() {

      var output = ProcessModificators();
      if (output != null) return output;
       
      output = ProcessSetModeOff();
      if (output != null) return output;

      output = ProcessModeChange();
      if (output != null) return output;

      output = ProcessEsc();
      if (output != null) return output;

      if (State.IsOff) return outputOld;

      output = ProcessNormalAndInsertWithCapital();
      if (output != null) return output;

      output = ProcessNormalMode();
      if (output != null) return output;

      return outputOld;
    } 
 
    private cOutput ProcessModificators() {

      if (!inputKey.IsModifier) return null;  
      //if (!cUtils.IsModifierKey(keys)) return null;

      if (inputKey.IsCapital) return ProcessCapital();

      var modificators = NextModificators();

      var r = NextOutput();
      
      if (inputKey.IsAlt && isUp) {
        if (input.AppState.PreventAltUp) {
          modificators = this.modificators;
          r.AppState = new AppState(r.AppState.State, modificators, r.AppState.FirstStep, false, r.AppState.PreventEscOnCapsUp);
          //r.AppState.PreventAltUp = false;
        }
      }

      //r.AppState.Modificators = modificators;
      r.AppState = new AppState(r.AppState.State, modificators, r.AppState.FirstStep, r.AppState.PreventAltUp, r.AppState.PreventEscOnCapsUp);

      return r;
    }

    private Modificators NextModificators() {
      bool alt = this.modificators.Alt;
      bool control = this.modificators.Control;
      bool shift = this.modificators.Shift;
      bool win = this.modificators.Win;
      bool caps = this.modificators.Caps;
      if (inputKey.IsAlt) alt = !isUp;
      if (inputKey.IsControl) control = !isUp;
      if (inputKey.IsShift) shift = !isUp;
      if (inputKey.IsWin) win = !isUp; 
      if (inputKey.IsCapital) caps = !isUp;
      return new Modificators(alt, control, shift, win, caps);
    }

    private cOutput ProcessCapital() {

      var sendKeys = "";
      var preventEscOnNextCapitalUp = input.AppState.PreventEscOnCapsUp;

      if (isUp) {
        if (input.AppState.PreventEscOnCapsUp) {
          preventEscOnNextCapitalUp = false;
        } else {
          sendKeys = "{ESC}";
        }
      }
      
      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.sendDoKey = new SendDoKey(sendKeys);

      r.AppState = new AppState(r.AppState.State, NextModificators(), "", r.AppState.PreventAltUp, preventEscOnNextCapitalUp);

      return r;
      
    }

    private cOutput ProcessEsc() {

      if (!inputKey.IsEsc) return null;
      if (State.IsOff) return null;
      if (isUp) return null;
      if (isCapital) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.AppState = new AppState(cUtils.GetPrevState(State), r.AppState.Modificators, "", r.AppState.PreventAltUp, r.AppState.PreventEscOnCapsUp);

      return r;

    }
 
    private string NormalModeKeysToString() {

      var alt = (modificators.Alt) ? "%" : "";
      return firstStep + alt + keys.ToString();

    }

    private bool IsDownFirstStep() => firstStep == "" && settings.IsTwoStep(keys);

    private cOutput ProcessNormalMode() {

      if (!State.IsNormal) return null;
      if (isUp) return null;
      if (modificators.Win) return null;

      var isDownFirstStep = IsDownFirstStep();

      var firstStepNext = isDownFirstStep ? keys : "";
  

      var sendDoKey = GetSendDoKey(isDownFirstStep);

      var preventNextAltUp = sendDoKey.IsAlt;
      var preventKeyProcess = inputKey.IsLetterOrDigit || !sendDoKey.IsEmpty;

      var r = NextOutput();
      r.sendDoKey = sendDoKey;
      r.PreventKeyProcess = preventKeyProcess;
      r.AppState = new AppState(r.AppState.State, r.AppState.Modificators, firstStepNext, preventNextAltUp, r.AppState.PreventEscOnCapsUp);
      return r;

    } 
 
    private SendDoKey GetSendDoKey(bool isDownFirstStep) {

      if (isDownFirstStep) return new SendDoKey("");

      var trigger = NormalModeKeysToString();

      return settings.GetSendKeyNormal(trigger);

    }

    private cOutput ProcessSetModeOff() {

      if (!isCapital) return null;
      if (isUp) return null;
      if (keys != settings.ModeOffKey) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.AppState = new AppState(State.Off, r.AppState.Modificators, "", r.AppState.PreventAltUp, true);

      return r;


    }

    private cOutput ProcessNormalAndInsertWithCapital() {

      if (State.IsOff) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var sendKeys = settings.GetSendKeyCaps(keys.ToString());

      if (sendKeys.IsEmpty) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.sendDoKey = sendKeys;
      r.AppState = new AppState(r.AppState.State, r.AppState.Modificators, "", r.AppState.PreventAltUp, true);
      return r;

    }

    private cOutput ProcessModeChange() {

      if (keys != settings.ModeChangeKey) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.AppState = new AppState(cUtils.GetNextState(State), r.AppState.Modificators, "", r.AppState.PreventAltUp, true);
      return r;

    }

    private cOutput NextOutput() => new cOutput { AppState = NextState() };
    private AppState NextState() => input.AppState;

  }
}

