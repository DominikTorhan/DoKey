using DoKey.Core;
using DoKey.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static DoKey.Core.Domain;

namespace DoKey.App
{
  internal class TrayApp : Form
  {

    private readonly NotifyIcon trayIcon;
    private readonly ContextMenu trayMenu;
    private KeyboardHook mKeyboardHook;
    private readonly App _app;

    private bool IsSending = false;
    private AppState AppState = new AppState(State.Off, new Modificators(false, false, false, false, false), "", false);

    private Icon GetIconOff() => new Icon(Resources.Off, 40, 40);
    private Icon GetIconNormalMode() => new Icon(Resources.Normal, 40, 40);
    private Icon GetIconInsertMode() => new Icon(Resources.Insert, 40, 40);

    public TrayApp()
    {

      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", (s, e) => OnExit());
      trayMenu.MenuItems.Add("Open Settings", (s, e) => OpenSettings());
      //trayMenu.MenuItems.Add("Reload Settings", (s, e) => ReloadSettings());

      trayIcon = new NotifyIcon
      {
        Text = "DoKey",
        Icon = GetIconOff(),
        ContextMenu = trayMenu,
        Visible = true
      };

      SetupKeyboardHooks();

      _app = new App();
      _app.Initialize();

    }

    public void SetupKeyboardHooks()
    {
      mKeyboardHook = new KeyboardHook();
      mKeyboardHook.KeyboardPressed += OnKeyPressed;
    }

    private void SetIcon() => trayIcon.Icon = GetIcon(AppState.state);

    private KeyEventData CreateKetEventData(KeyboardHookEvent e)
    {
      var key = (Keys)e.KeyboardData.VirtualCode;
      var x = e.IsUp ? KeyEventType.Up : KeyEventType.Down;
      var inputKey = Core.DomainOperations.CreateInputKey(key.ToString());
      return new KeyEventData(inputKey, x);
    }

    private bool TryOpenSettingsFile(Keys key, AppState appState)
    {
      if (!appState.modificators.caps) return false;
      if (key != Keys.Oem2) return false;
      OpenSettings();
      return true;

    }

    private bool TryExitApp(Keys key, AppState appState)
    {
      if (!appState.modificators.caps) return false;
      if (key != Keys.Back) return false;
      OnExit();
      return true;

    }

    private void OnKeyPressed(object sender, KeyboardHookEvent e)
    {

      if (IsSending) return;

      Keys key = (Keys)e.KeyboardData.VirtualCode;

      if (Control.IsKeyLocked(Keys.CapsLock) && key == Keys.Capital) return;

      KeyEventData keyEventData = CreateKetEventData(e);

      var output = _app.Work(keyEventData);

      if (TryOpenSettingsFile(key, AppState)) { e.Handled = true; return; }
      if (TryExitApp(key, AppState)) { e.Handled = true; return; }
      
      if (output == null) return;

      AppState = output.appState;

      if (output.preventKeyProcess)
      {
        e.Handled = true;
      }

      if (!string.IsNullOrEmpty(output.send))
      {
        IsSending = true;
        SendKeys.Send(output.send);
        IsSending = false;
      }

      SetIcon();

      return;

    }

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

    private void OpenSettings()
    {

      Process.Start(DoKey.Core.Domain.filePathNew);

    }

    //private void ReloadSettings()
    //{
    //  Configuration = new Configuration();
    //}

    protected override void Dispose(bool isDisposing)
    {

      mKeyboardHook?.Dispose();

      if (isDisposing)
      {
        trayIcon.Dispose();
        trayMenu.Dispose();
      }

      base.Dispose(isDisposing);

    }

  }
}
