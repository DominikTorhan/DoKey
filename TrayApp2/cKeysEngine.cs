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
    public bool isWin { get; set; }
    public StateEnum state { get; set; }
    public bool PreventNextWinUp { get; set; }

    //public bool isShift { get; set; }
    //public bool isShift { get; set; }

    public override string ToString() {
      return "Win: " + isWin + " state: " + state;
    }

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
    private bool isWin => input.stateData.isWin;
    private bool isUp => input.eventData.isUp;
    private StateEnum state => input.stateData.state;

    public cOutput ProcessKey() {

      var output = ProcessWin();
      if (output != null) return output;

      output = ProcessSpace();
      if (output != null) return output;

      if (state == StateEnum.Off) return outputOld;

      if (state == StateEnum.Normal) {
        var sendKeys = cUtils.GetSendKeyByKeyNormalMode(keys, settings);
        return new cOutput {
          StateData = outputOld.StateData,
          SendKeys = sendKeys,
          PreventKeyProcess = sendKeys != null
        };
      }

      return outputOld;
    }

    private cOutput ProcessWin() {

      if (!cUtils.IsWin(keys)) return null;

      var PreventNextWinUp = input.stateData.PreventNextWinUp;
      var PreventKeyProcess = false;

      if (PreventNextWinUp && isUp) {
        //PreventNextWinUp = false;
        //PreventKeyProcess = true;
      }

      return new cOutput {
        StateData = new cStateData {
          state = state,
          isWin = !isUp,
          PreventNextWinUp = PreventNextWinUp
        },
        PreventKeyProcess = PreventKeyProcess
      };

    }

    private cOutput ProcessSpace() {

      if (keys != Keys.Space) return null;
      if (!isWin) return outputOld;

      if (!isUp) return new cOutput {
        StateData = outputOld.StateData,
        PreventKeyProcess = true
      };

      return new cOutput {
        StateData = new cStateData {
          state = cUtils.GetNextState(state),
          isWin = isWin,
          PreventNextWinUp = true,
        },
        PreventKeyProcess = true,
      };


    }

  }
}

