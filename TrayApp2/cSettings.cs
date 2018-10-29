using DoKey.FS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2 {
  public class cSettings {

    private KeysList KeysList; 

    public string ModeChangeKey = "f";
    public string ModeOffKey = "q";

    public cSettings(string path) {

      KeysList = DoKeyModule.CreateKeysList;

    }

    internal bool IsTwoStep(string key) {

      string x = "uif";

      return x.Contains(key.ToLower());

    } 
 
    public SendDoKey GetSendKeyNormal(string key) {
      return DoKeyModule.GetSendKeyNormal(KeysList, key);
    }
    public SendDoKey GetSendKeyCaps(string key) {
      return DoKeyModule.GetSendKeyCaps(KeysList, key);
    }

  }
}
