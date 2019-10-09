using DoKey.CoreCS;
using DoKey.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoKey
{
  public class Icons
  {
    private Icon GetIconOff() => new Icon(Resources.Off, 40, 40);
    private Icon GetIconNormalMode() => new Icon(Resources.Normal, 40, 40);
    private Icon GetIconInsertMode() => new Icon(Resources.Insert, 40, 40);

    public Icon GetIcon(State xState)
    {
      if (xState == State.Off) return GetIconOff();
      if (xState == State.Normal) return GetIconNormalMode();
      if (xState == State.Insert) return GetIconInsertMode();
      return null;
    }
  }
}
