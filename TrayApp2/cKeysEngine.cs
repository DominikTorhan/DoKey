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
    public bool isESC { get; set; }
    public StateEnum state { get; set; }
    //public bool isShift { get; set; }
    //public bool isShift { get; set; }

    public override string ToString() {
      return "Esc: " + isESC + " state: " + state;
    }

  }

  public class cOutput {
    public cStateData StateData { get; set; }
  }

  public class cKeysEngine {

    public cInput input { get; set; }

    private cOutput outputOld => new cOutput { StateData = input.stateData };
    private Keys keys => input.eventData.keys;
    private bool isEsc => input.stateData.isESC;
    private bool isUp => input.eventData.isUp;
    private StateEnum state => input.stateData.state;

    public cOutput ProcessKey() {

      if (cUtils.IsEsc(keys)) {
        return new cOutput {
          StateData = new cStateData {
            state = state,
            isESC = !isUp,
          }
        };
      }

      

      var output = ManageSpace();
      if (output != null) return output;

      return outputOld;
    }

    private cOutput ManageSpace() {

      if (keys != Keys.Space) return null;
      if (!isEsc) return outputOld;
      if (!isUp) return outputOld;

      return new cOutput {
        StateData = new cStateData {
          state = cUtils.GetNextState(state),
          isESC = isEsc,
        }
      };


    }

  }
}

