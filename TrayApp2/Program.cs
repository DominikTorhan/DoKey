using System;
using System.Windows.Forms;

namespace DoKey
{
  class SysTrayApp : Form
  {

    [STAThread]
    public static void Main()
    {
      try
      {
        Application.Run(new TrayApp());
      }
      catch
      {
        Application.Exit();
      }
    }

  }
}
