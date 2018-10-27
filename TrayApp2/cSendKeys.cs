using DoKey.FS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrayApp2 {

  public class cSendKeys {

    public static string Create(string str, Modificators modificators) {

      if (str == "") return "";

      var alt = modificators.Alt ? "%" : "";
      var control = modificators.Control ? "^" : "";

      return alt + control + str;
    }

  }

}
