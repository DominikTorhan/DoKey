using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrayApp2 {

  public class cSendKeys {

    public static string Create(string str, cModificators modificators) {

      var alt = modificators.isAlt ? "%" : "";
      var control = modificators.isControl ? "^" : "";

      return alt + control + str;
    }

  }

}
