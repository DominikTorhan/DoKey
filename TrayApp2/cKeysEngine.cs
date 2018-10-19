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

  public class cStateData {
    public bool isControl { get; set; }
    public bool isCapital { get; set; }
    public StateEnum state { get; set; }
    public bool preventEscOnNextCapitalUp { get; set; }
    //public bool PreventNextWinUp { get; set; }

    //public bool isShift { get; set; }
    //public bool isShift { get; set; }

    public override string ToString() {
      return "  isCapital: " + isCapital + " isControl: " + isControl + " state: " + state;
    }

    public cStateData Clone() =>
      new cStateData {
        isCapital = isCapital,
        isControl = isControl,
        preventEscOnNextCapitalUp = preventEscOnNextCapitalUp,
        state = state
      };

  }

  public class cOutput {
    public cStateData StateData { get; set; }
    public bool PreventKeyProcess { get; set; }
    public string SendKeys { get; set; }

  }

  public class cKeysEngine {

    public cInput input { get; set; }
    public cSettings settings { get; set; }

    private cOutput outputOld => new cOutput { StateData = input.stateData };
    private Keys keys => input.eventData.keys;
    private bool isControl => input.stateData.isControl;
    private bool isCapital => input.stateData.isCapital;
    private bool isUp => input.eventData.isUp;
    private StateEnum state => input.stateData.state;

    public cOutput ProcessKey() {

      var output = ProcessCapital();
      if (output != null) return output;

      //output = ProcessControl();
      //if (output != null) return output;

      output = ProcessModeChange();
      if (output != null) return output;

      output = ProcessEsc();
      if (output != null) return output;

      if (state == StateEnum.Off) return outputOld;

      output = ProcessNormalMode();
      if (output != null) return output;

      output = ProcessInsertMode();
      if (output != null) return output;

      return outputOld;
    }

    private cOutput ProcessCapital() {

      //return null;

      if (keys != Keys.Capital) return null;
      //if (state != StateEnum.Insert) return null;

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

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.StateData.state = state - 1;
      return r;

      //var stateData = new cStateData {
      //  isControl = outputOld.StateData.isControl,
      //  state = state - 1,
      //};

      //return new cOutput {
      //  StateData = stateData,
      //  SendKeys = "",
      //  PreventKeyProcess = true,
      //};

    }

    private cOutput ProcessNormalMode() {

      if (state != StateEnum.Normal) return null;
      if (isUp) return null;

      var sendKeys = cUtils.GetSendKeyByKeyNormalMode(keys, settings);

      var preventKeyProcess = cUtils.IsLetterKey(keys) || sendKeys != "";

      return new cOutput {
        StateData = outputOld.StateData,
        SendKeys = sendKeys,
        PreventKeyProcess = preventKeyProcess
      };

    }

    private cOutput ProcessInsertMode() {

      if (state != StateEnum.Insert) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var sendKeys = cUtils.GetSendKeyByKeyInsertModeWithControl(keys, settings);

      if (sendKeys == "") return null;

      var r = NextOutput();
      r.PreventKeyProcess = true;
      r.StateData.preventEscOnNextCapitalUp = true;
      r.SendKeys = sendKeys;
      return r;

      var preventKeyProcess = sendKeys != "";

      return new cOutput {
        StateData = outputOld.StateData,
        SendKeys = sendKeys,
        PreventKeyProcess = preventKeyProcess
      };

    }

    //private cOutput ProcessControl() {

    //  if (!cUtils.IsControl(keys)) return null;

    //  //var preventKeyProcess = state == StateEnum.Insert;
    //  var preventKeyProcess = false;

    //  return new cOutput {
    //    StateData = new cStateData {
    //      state = state,
    //      isControl = !isUp,
    //      //PreventNextWinUp = outputOld.StateData.PreventNextWinUp
    //    },
    //    PreventKeyProcess = preventKeyProcess
    //  };

    //}

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

    //private cOutput ProcessWin() {

    //  if (!cUtils.IsWin(keys)) return null;
    //  //if (keys != Keys.Space) return null;
    //  if (!isControl) return outputOld;

    //  if (!isUp) return new cOutput {
    //    StateData = outputOld.StateData,
    //    //PreventKeyProcess = true
    //  };

    //  return new cOutput {
    //    StateData = new cStateData {
    //      state = cUtils.GetNextState(state),
    //      isControl = isControl,
    //      //PreventNextWinUp = true,
    //    },
    //    //PreventKeyProcess = true,
    //  };


    //}

  }
}

