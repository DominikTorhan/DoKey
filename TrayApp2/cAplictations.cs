using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrayApp2 {
  public class cAplictations {

    public static IEnumerable<string> Cln() {
      yield return "devenv";
      yield return "notepad";
      yield return "explorer";
      yield return "Evernote";

    }

  }
}
