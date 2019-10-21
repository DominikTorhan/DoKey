using System;
using System.Windows.Forms;

namespace DoKey.App
{
  internal static class Program
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
