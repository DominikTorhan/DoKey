using System;
using System.Windows.Forms;
using DoKey.CoreCS;

namespace DoKey.App
{
  internal class TrayApp : Form
  {
    private readonly NotifyIcon trayIcon;
    private readonly ContextMenu trayMenu;
    private readonly App app;

    public TrayApp()
    {
      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", (s, e) => Exit());
      //trayMenu.MenuItems.Add("Open Settings", (s, e) => OpenSettings());
      trayIcon = new NotifyIcon
      {
        Text = "DoKey",
        Icon = new Icons().GetIcon(State.Off),
        ContextMenu = trayMenu,
        Visible = true
      };
      app = new App(RefreshIcon, Exit, new Logger());
    }

    private void RefreshIcon(State state)
    {
      trayIcon.Icon = new Icons().GetIcon(state);
    }

    private void Exit()
    {
      Application.Exit();
    }

    protected override void OnLoad(EventArgs e)
    {
      Visible = false;
      ShowInTaskbar = false;
      base.OnLoad(e);
    }

    protected override void Dispose(bool isDisposing)
    {
      app.Dispose();
      if (isDisposing)
      {
        trayIcon.Dispose();
        trayMenu.Dispose();
      }
      base.Dispose(isDisposing);
    }

  }
}
