﻿using DoKey.FS;
using DoKey.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static DoKey.FS.Domain;

namespace DoKey
{
  internal class TrayApp : Form
  {

    private NotifyIcon trayIcon;
    private ContextMenu trayMenu;
    private KeyboardHook mKeyboardHook;
    private Configuration Configuration;
    private bool IsSending = false;
    private AppState AppState = new AppState();

    private Icon GetIconOff() => new Icon(Resources.Off, 40, 40);
    private Icon GetIconNormalMode() => new Icon(Resources.Normal, 40, 40);
    private Icon GetIconInsertMode() => new Icon(Resources.Insert, 40, 40);

    public TrayApp()
    {

      Configuration = new Configuration();

      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", (s, e) => OnExit());
      trayMenu.MenuItems.Add("Open Settings", (s, e) => OpenSettings());
      trayMenu.MenuItems.Add("Reload Settings", (s, e) => ReloadSettings());

      trayIcon = new NotifyIcon
      {
        Text = "DoKey",
        Icon = GetIconOff(),
        ContextMenu = trayMenu,
        Visible = true
      };

      SetupKeyboardHooks();

    }

    public void SetupKeyboardHooks()
    {
      mKeyboardHook = new KeyboardHook();
      mKeyboardHook.KeyboardPressed += OnKeyPressed;
    }

    private void SetIcon() => trayIcon.Icon = GetIcon(AppState.State);

    private KeyEventData CreateKetEventData(KeyboardHookEvent e)
    {
      var key = (Keys)e.KeyboardData.VirtualCode;
      var x = e.IsUp ? KeyEventType.Up : KeyEventType.Down;
      return new KeyEventData(key.ToString(), x);
    }

    private bool TryOpenSettingsFile(Keys key, AppState appState)
    {
      if (!appState.Modificators.Caps) return false;
      if (key != Keys.Oem2) return false;
      OpenSettings();
      return true;

    }

    private bool TryExitApp(Keys key, AppState appState)
    {
      if (!appState.Modificators.Caps) return false;
      if (key != Keys.Back) return false;
      OnExit();
      return true;

    }

    private void OnKeyPressed(object sender, KeyboardHookEvent e)
    {

      if (IsSending) return;

      Keys key = (Keys)e.KeyboardData.VirtualCode;

      if (Control.IsKeyLocked(Keys.CapsLock) && key == Keys.Capital) return;

      if (TryOpenSettingsFile(key, AppState)) { e.Handled = true; return; }
      if (TryExitApp(key, AppState)) { e.Handled = true; return; }

      if (Utils.IsIgnoredKey(key, Control.ModifierKeys)) return;

      KeyEventData keyEventData = CreateKetEventData(e);

      var output = ProcessKey(keyEventData);

      if (output == null) return;

      AppState = output.appState;

      //Log(output.AppState.ToLog());

      if (output.preventKeyProcess)
      {
        //Log("  prevent");
        e.Handled = true;
      }

      if (!string.IsNullOrEmpty(output.send))
      {
        //Log(output.sendDoKey.ToLog);

        //mSendDoKeyLast = output.sendKey;
        IsSending = true;
        SendKeys.Send(output.send);
        IsSending = false;
      }

      SetIcon();

      return;

    }

    private KeysEngineResult ProcessKey(KeyEventData keyEventData)
    {

      return new KeysEngine
      {
        AppState = AppState,
        KeyEventData = keyEventData,
        Configuration = Configuration,
      }.ProcessKey();

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

      Process.Start(Configuration.FilePath);

    }

    private void ReloadSettings()
    {
      Configuration = new Configuration();
    }

    protected override void Dispose(bool isDisposing)
    {

      mKeyboardHook?.Dispose();

      if (isDisposing)
      {
        trayIcon.Dispose();
      }

      base.Dispose(isDisposing); 

    }

  }
}
