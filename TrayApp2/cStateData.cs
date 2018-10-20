using System.Windows.Forms;

namespace TrayApp2 {
  public class cStateData {

    public Keys firstStep { get; set; }
    public bool isCapital { get; set; }
    public StateEnum state { get; set; }
    public bool preventEscOnNextCapitalUp { get; set; }
    //public bool PreventNextWinUp { get; set; }

    //public bool isShift { get; set; }
    //public bool isShift { get; set; }

    public cStateData() {
      this.firstStep = Keys.None;
      this.isCapital = false;
      this.state = StateEnum.Off;
      this.preventEscOnNextCapitalUp = false;
    }


    public override string ToString() {
      return "cap: " + isCapital + " first: " + firstStep + " state: " + state;
    }

    public cStateData Clone() =>
      new cStateData {
        isCapital = isCapital,
        preventEscOnNextCapitalUp = preventEscOnNextCapitalUp,
        state = state,
        firstStep = firstStep,
      };

  }
}

