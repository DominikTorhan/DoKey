using DoKey.FS;
using System.Windows.Forms;

namespace TrayApp2 {
  public class cStateData {

    public string firstStep { get; set; }
    public StateEnum state { get; set; }
    public State State { get; set; }
    public bool preventEscOnNextCapitalUp { get; set; }
    public bool preventNextAltUp { get; set; }
    public Modificators modificators { get; set; }

    public cStateData() {
      this.State = State.Off;
      this.firstStep = "";
      this.state = StateEnum.Off;
      this.preventEscOnNextCapitalUp = false;
      this.preventNextAltUp = false;
      this.modificators = new Modificators(false, false, false, false, false);
    }


    public override string ToString() {
      var modificators = this.modificators.ToLog;
      return modificators + " first: " + firstStep + " state: " + state;
    }

    public cStateData Clone() =>
      new cStateData {
        preventEscOnNextCapitalUp = preventEscOnNextCapitalUp,
        preventNextAltUp = preventNextAltUp,
        state = state,
        firstStep = firstStep, 
        modificators = modificators,
      };

  }
}

