using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DoKey.CoreCS;

namespace DoKey.App
{
  internal class TrayApp : Form
  {
    private readonly NotifyIcon trayIcon;
    private readonly ContextMenu trayMenu;
    private readonly App app;
    private readonly string configFile = "config.txt";

    public TrayApp()
    {
      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", (s, e) => Exit());
      trayMenu.MenuItems.Add("ShowConfigFile", (s, e) => ShowConfigFile());
      trayIcon = new NotifyIcon
      {
        Text = "DoKey",
        Icon = new Icons().GetIcon(State.Off),
        ContextMenu = trayMenu,
        Visible = true
      };
      app = new App(CreateAppConfig());
    }

    private AppConfig CreateAppConfig()
    {
      return new AppConfig
      {
        actionExit = Exit,
        actionShowConfigFile = ShowConfigFile,
        actionRefreshIcon = RefreshIcon,
        actionSend = SendKeys.Send,
        funcGetConfigText = () => File.ReadAllText(configFile),
        actionLog = log => Debug.WriteLine(log), 
      };
    }

    private void RefreshIcon(State state)
    {
      trayIcon.Icon = new Icons().GetIcon(state);
    }

    private void Exit()
    {
      Application.Exit();
    }

    private void ShowConfigFile()
    {
      Process.Start(configFile);
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
