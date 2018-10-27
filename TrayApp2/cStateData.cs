using DoKey.FS;
using System.Windows.Forms;

namespace TrayApp2 {
  public class cStateData {

    public Keys firstStep { get; set; }
    public bool isCapital { get; set; }
    public StateEnum state { get; set; }
    public bool preventEscOnNextCapitalUp { get; set; }
    public bool preventNextAltUp { get; set; }
    public Modificators modificators { get; set; }
    //public bool PreventNextWinUp { get; set; }

    //public bool isShift { get; set; }
    //public bool isShift { get; set; }

    public cStateData() {
      this.firstStep = Keys.None;
      this.isCapital = false;
      this.state = StateEnum.Off;
      this.preventEscOnNextCapitalUp = false;
      this.preventNextAltUp = false;
      this.modificators = new Modificators(false, false, false, false);
    }


    public override string ToString() {
      var modificators = "mod: " + (this.modificators.Alt ? " alt " : "") + (this.modificators.Control ? " Control " : "") 
        + (this.modificators.Shift ? " Shift " : "") + (this.modificators.Win ? " win " : "");
      return modificators + "cap: " + isCapital + " first: " + firstStep + " state: " + state;
    }

    public cStateData Clone() =>
      new cStateData {
        isCapital = isCapital,
        preventEscOnNextCapitalUp = preventEscOnNextCapitalUp,
        preventNextAltUp = preventNextAltUp,
        state = state,
        firstStep = firstStep, 
        modificators = modificators,
      };

  }
}

