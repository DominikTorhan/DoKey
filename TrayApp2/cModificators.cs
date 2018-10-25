using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrayApp2 {

  public class cModificators {

    public bool isControl { get; set; }
    public bool isAlt { get; set; }
    public bool isShift { get; set; }

    public override string ToString() {
      return $"modificators: control: {isControl} alt: {isAlt} shift: {isShift}";
    }
  }

}
