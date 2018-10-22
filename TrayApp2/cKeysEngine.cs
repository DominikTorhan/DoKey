using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    public cStateData StateData { get; set; }
    public bool PreventKeyProcess { get; set; }
    public string SendKeys { get; set; }

    public override string ToString() {
      var prevent = PreventKeyProcess ? "p " : "";
      return prevent + SendKeys + " " + StateData;
    }
  }

  public class cKeysEngine {

    public cInput input { get; set; }
    public cSettings settings { get; set; }

    private cOutput outputOld => new cOutput { StateData = input.stateData };
    private Keys keys => input.eventData.keys;
    private Keys firstStep => input.stateData.firstStep;
    private bool isCapital => input.stateData.isCapital;
    private bool isUp => input.eventData.isUp;
    private StateEnum state => input.stateData.state;

    public cOutput ProcessKey() {

      var output = ProcessCapital();
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
      r.SendKeys = sendKeys;
      r.StateData.preventEscOnNextCapitalUp = preventEscOnNextCapitalUp;
      return r;
      
    }

    private cOutput ProcessEsc() {

      if (!cUtils.IsEsc(keys)) return null;
      if (state == StateEnum.Off) return null;
      if (isUp) return null;
      if (isCapital) return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.StateData.state = state - 1;
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

      var isDownFirstStep = IsDownFirstStep();

      var sendKeys = isDownFirstStep ? "" : settings.SendKeyNormal(NormalModeKeysToString());
      var firstStepNext = isDownFirstStep ? keys : Keys.None;
  
      var preventKeyProcess = cUtils.IsLetterKey(keys) || sendKeys != "";

      var r = NextOutput();
      r.StateData.firstStep = firstStepNext;
      r.SendKeys = sendKeys;
      r.PreventKeyProcess = preventKeyProcess;
      return r;

    }

    private cOutput ProcessNormalAndInsertWithCapital() {

      if (state == StateEnum.Off) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var sendKeys = cUtils.GetSendKeyByKeyInsertModeWithControl(keys, settings);

      if (sendKeys == "") return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.StateData.preventEscOnNextCapitalUp = true;
      r.SendKeys = sendKeys;
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
      return r;

    }

    private cOutput NextOutput() => new cOutput { StateData = NextState() };
    private cStateData NextState() => input.stateData.Clone();

  }
}

