using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoKey.FS;

namespace TrayApp2 {

  public class cInput {
    public cEventData eventData { get; set; }
    public cStateData stateData { get; set; }
  }

  public class cEventData {
    public Keys keys;
    public bool isUp;
  }

  public class cOutput { 
    public SendDoKey sendDoKey { get; set; }
    public cStateData StateData { get; set; }
    public bool PreventKeyProcess { get; set; }
    //public string SendKeys { get; set; }

    public override string ToString() {
      var prevent = PreventKeyProcess ? "p " : "";
      var send = sendDoKey != null ? sendDoKey.Send : "";
      return prevent + send + " " + StateData;
    }
  }

  public class cKeysEngine {

    public cInput input { get; set; }
    public cSettings settings { get; set; }

    private InputKey inputKey => new InputKey(input.eventData.keys.ToString());
    private cOutput outputOld => new cOutput { StateData = input.stateData };
    private Keys keys => input.eventData.keys;
    private bool isUp => input.eventData.isUp;
    private Keys firstStep => input.stateData.firstStep;
    private StateEnum state => input.stateData.state;
    private Modificators modificators => input.stateData.modificators;
    private bool isCapital => input.stateData.modificators.Caps;

    public cOutput ProcessKey() {

      var output = ProcessModificators();
      if (output != null) return output;
       
      //output = ProcessCapital();
      //if (output != null) return output;

      output = ProcessSetModeOff();
      if (output != null) return output;

      output = ProcessModeChange();
      if (output != null) return output;

      output = ProcessEsc();
      if (output != null) return output;

      if (state == StateEnum.Off) return outputOld;

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
        if (input.stateData.preventNextAltUp) {
          modificators = this.modificators;
          r.StateData.preventNextAltUp = false;
        }
      }

      r.StateData.modificators = modificators;

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
      var preventEscOnNextCapitalUp = input.stateData.preventEscOnNextCapitalUp;

      if (isUp) {
        if (input.stateData.preventEscOnNextCapitalUp) {
          preventEscOnNextCapitalUp = false;
        } else {
          sendKeys = "{ESC}";
        }
      }
      
      var r = NextOutput();
      r.StateData.modificators = NextModificators();
      //r.StateData.isCapital = !isUp;
      r.PreventKeyProcess = true;
      r.sendDoKey = new SendDoKey(sendKeys);
      r.StateData.preventEscOnNextCapitalUp = preventEscOnNextCapitalUp;
      r.StateData.firstStep = Keys.None;
      return r;
      
    }

    private cOutput ProcessEsc() {

      if (!inputKey.IsEsc) return null;
      if (state == StateEnum.Off) return null;
      if (isUp) return null;
      if (isCapital) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.StateData.state = cUtils.GetPrevState(state);
      r.StateData.firstStep = Keys.None;
      return r;

    }
 
    private string NormalModeKeysToString() {

      var firstKeyStr = firstStep != Keys.None ? firstStep.ToString() : "";
      var alt = (modificators.Alt) ? "%" : "";
      return firstKeyStr + alt + keys.ToString();

    }

    private bool IsDownFirstStep() => firstStep == Keys.None && settings.IsTwoStep(keys.ToString());

    private cOutput ProcessNormalMode() {

      if (state != StateEnum.Normal) return null;
      if (isUp) return null;
      if (modificators.Win) return null;

      var isDownFirstStep = IsDownFirstStep();

      //var sendKeys = isDownFirstStep ? "" : settings.GetSendKeyNormal(NormalModeKeysToString());
      var firstStepNext = isDownFirstStep ? keys : Keys.None;
  

      var sendDoKey = GetSendDoKey(isDownFirstStep);
      //sendKeys = cSendKeys.Create(sendKeys, modificators);

      //var preventNextAltUp = sendKeys.Contains("%");
      var preventNextAltUp = sendDoKey.IsAlt;
      var preventKeyProcess = cUtils.IsLetterKey(keys) || !sendDoKey.IsEmpty;

      var r = NextOutput();
      r.StateData.firstStep = firstStepNext;
      r.sendDoKey = sendDoKey;
      r.PreventKeyProcess = preventKeyProcess;
      r.StateData.preventNextAltUp = preventNextAltUp;
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
      r.StateData.state = StateEnum.Off;
      r.StateData.preventEscOnNextCapitalUp = true;
      r.PreventKeyProcess = true;
      r.StateData.firstStep = Keys.None;
      return r;


    }

    private cOutput ProcessNormalAndInsertWithCapital() {

      if (state == StateEnum.Off) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var sendKeys = settings.GetSendKeyCaps(keys.ToString());

      if (sendKeys.IsEmpty) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.StateData.preventEscOnNextCapitalUp = true;
      r.sendDoKey = sendKeys;
      r.StateData.firstStep = Keys.None;
      return r;

    }

    private cOutput ProcessModeChange() {

      if (keys != settings.ModeChangeKey) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var r = NextOutput();
      r.StateData.state = cUtils.GetNextState(r.StateData.state);
      r.StateData.preventEscOnNextCapitalUp = true;
      r.PreventKeyProcess = true;
      r.StateData.firstStep = Keys.None;
      return r;

    }

    private cOutput NextOutput() => new cOutput { StateData = NextState() };
    private cStateData NextState() => input.stateData.Clone();

  }
}

