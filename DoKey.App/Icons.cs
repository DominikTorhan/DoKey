using DoKey.CoreCS;
using DoKey.Properties;
using System.Drawing;

namespace DoKey
{
  public class Icons
  {
    private Icon GetIconOff()
    {
      return new Icon(Resources.Off, 40, 40);
    }

    private Icon GetIconNormalMode()
    {
      return new Icon(Resources.Normal, 40, 40);
    }

    private Icon GetIconInsertMode()
    {
      return new Icon(Resources.Insert, 40, 40);
    }

    public Icon GetIcon(State xState)
    {
      if (xState == State.Off) return GetIconOff();
      if (xState == State.Normal) return GetIconNormalMode();
      if (xState == State.Insert) return GetIconInsertMode();
      return null;
    }
  }
}
