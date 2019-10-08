using DoKey.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DoKey.CoreCS;
//using DoKey.Core;
//using static DoKey.Core.Domain;

namespace DoKey.App
{
  internal class TrayApp : Form
  {

    private readonly NotifyIcon trayIcon;
    private readonly ContextMenu trayMenu;
    //private KeyboardHook mKeyboardHook;
    private readonly App _app;

    //private bool IsSending = false;
    //private AppState AppState = new AppState(State.Off, new Modificators(false, false, false, false, false), "", false);

    private Icon GetIconOff() => new Icon(Resources.Off, 40, 40);
    private Icon GetIconNormalMode() => new Icon(Resources.Normal, 40, 40);
    private Icon GetIconInsertMode() => new Icon(Resources.Insert, 40, 40);


    public TrayApp()
    {

      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", (s, e) => OnExit());
      //trayMenu.MenuItems.Add("Open Settings", (s, e) => OpenSettings());
      trayIcon = new NotifyIcon
      {
        Text = "DoKey",
        Icon = GetIconOff(),
        ContextMenu = trayMenu,
        Visible = true
      };
      _app = new App();
      _app.Initialize(RefreshIcon, OnExit);
    }

    private void RefreshIcon(State state) => trayIcon.Icon = GetIcon(state);

    private Icon GetIcon(State xState)
    {

      if (xState == State.Off) return GetIconOff();
      if (xState == State.Normal) return GetIconNormalMode();
      if (xState == State.Insert) return GetIconInsertMode();

      return null;

    }

    protected override void OnLoad(EventArgs e)
    {
      Visible = false; // Hide form window.
      ShowInTaskbar = false; // Remove from taskbar.
      base.OnLoad(e);
    }

    private void OnExit()
    {
      Application.Exit();
    }

    protected override void Dispose(bool isDisposing)
    {
      _app.Dispose();
      if (isDisposing)
      {
        trayIcon.Dispose();
        trayMenu.Dispose();
      }
      base.Dispose(isDisposing);
    }

  }
}
