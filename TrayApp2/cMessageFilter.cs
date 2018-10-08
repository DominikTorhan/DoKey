using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2 {

  public class cMessageFilter : IMessageFilter {
    public bool PreFilterMessage(ref Message m) {
      // Intercept the left mouse button down message.
      if (m.Msg == 513) {
        MessageBox.Show("WM_LBUTTONDOWN is: " + m.Msg);
        return true;
      }
      return false;
    }
  }
}
