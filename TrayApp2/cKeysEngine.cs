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

    private cOutput outputOld => new cOutput { StateData = input.stateData };
    private Keys keys => input.eventData.keys;
    private bool isUp => input.eventData.isUp;
    private Keys firstStep => input.stateData.firstStep;
    private bool isCapital => input.stateData.isCapital;
    private StateEnum state => input.stateData.state;
    private Modificators modificators => input.stateData.modificators;

    private Modificators NextModificators() {
      bool alt = this.modificators.Alt;
      bool control = this.modificators.Control;
      bool shift = this.modificators.Shift;
      bool win = this.modificators.Win;
      if (cUtils.IsAlt(keys)) alt = !isUp;
      if (cUtils.IsControl(keys)) control = !isUp;
      if (cUtils.IsShift(keys)) shift = !isUp;
      if (cUtils.IsWin(keys)) win = !isUp;
      return new Modificators(alt, control, shift, win);
    }

    public cOutput ProcessKey() {

      var output = ProcessModificators();
      if (output != null) return output;
       
      output = ProcessCapital();
      if (output != null) return output;

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

      if (!cUtils.IsModifierKey(keys)) return null;

      var modificators = NextModificators();

      var r = NextOutput();

      if (cUtils.IsAlt(keys) && isUp) {
        if (input.stateData.preventNextAltUp) {
          modificators = this.modificators;
          r.StateData.preventNextAltUp = false;
        }
      }

      r.StateData.modificators = modificators;

      return r;
    }

    private cOutput ProcessCapital() {

      if (keys != Keys.Capital) return null;

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
      r.StateData.isCapital = !isUp;
      r.PreventKeyProcess = true;
      r.sendDoKey = new SendDoKey(sendKeys);
      r.StateData.preventEscOnNextCapitalUp = preventEscOnNextCapitalUp;
      r.StateData.firstStep = Keys.None;
      return r;
      
    }

    private cOutput ProcessEsc() {

      if (!cUtils.IsEsc(keys)) return null;
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
      return firstKeyStr + keys.ToString();

    }

    private bool IsDownFirstStep() => firstStep == Keys.None && settings.IsTwoStep(keys.ToString());

    private cOutput ProcessNormalMode() {

      if (state != StateEnum.Normal) return null;
      if (isUp) return null;
      if (modificators.Win) return null;

      var isDownFirstStep = IsDownFirstStep();

      var sendKeys = isDownFirstStep ? "" : settings.GetSendKeyNormal(NormalModeKeysToString());
      var firstStepNext = isDownFirstStep ? keys : Keys.None;
  
      var preventKeyProcess = cUtils.IsLetterKey(keys) || sendKeys != "";

      sendKeys = cSendKeys.Create(sendKeys, modificators);

      var preventNextAltUp = sendKeys.Contains("%");

      var r = NextOutput();
      r.StateData.firstStep = firstStepNext;
      r.sendDoKey = new SendDoKey(sendKeys);
      r.PreventKeyProcess = preventKeyProcess;
      r.StateData.preventNextAltUp = preventNextAltUp;
      return r;

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

      if (sendKeys == "") return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.StateData.preventEscOnNextCapitalUp = true;
      r.sendDoKey = new SendDoKey(sendKeys);
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

