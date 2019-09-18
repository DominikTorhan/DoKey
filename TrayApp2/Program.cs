using DoKey.FS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrayApp2.Properties;
using static DoKey.FS.Domain;

namespace TrayApp2
{
  class SysTrayApp : Form
  {

    [STAThread]
    public static void Main()
    {
      try
      {
        Application.Run(new SysTrayApp());
      }
      catch
      {
        Application.Exit();
      }
    }

    private NotifyIcon trayIcon;
    private ContextMenu trayMenu;
    private cKeyboardHook mKeyboardHook;
    private Configuration Configuration;
    private bool IsSending = false;
    //private SendKey mSendDoKeyLast;
    private AppState AppState = new AppState();

    //private StreamWriter LogStream;

    public SysTrayApp()
    {

      Configuration = new Configuration();

      // Create a simple tray menu with only one item.
      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", (s, e) => OnExit());
      trayMenu.MenuItems.Add("Open Settings", (s, e) => OpenSettings());
      trayMenu.MenuItems.Add("Reload Settings", (s, e) => ReloadSettings());

      // Create a tray icon. In this example we use a
      // standard system icon for simplicity, but you
      // can of course use your own custom icon too.
      trayIcon = new NotifyIcon
      {
        Text = "DoKey",
        Icon = GetIconOff(),

        // Add menu to tray icon and show it.
        ContextMenu = trayMenu,
        Visible = true
      };

      //LogStream = CreateLogStream();

      SetupKeyboardHooks();

    }

    public void SetupKeyboardHooks()
    {
      mKeyboardHook = new cKeyboardHook();
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
      //if (Control.ModifierKeys == Keys.Control)
      //{
      //  //do rozwiązania
      //  if (key == Keys.Q) return;
      //  if (key == Keys.K) return;
      //  if (key == Keys.I) return;
      //}

      if (TryOpenSettingsFile(key, AppState)) { e.Handled = true; return; }
      if (TryExitApp(key, AppState)) { e.Handled = true; return; }
      
      if (cUtils.IsIgnoredKey(key, Control.ModifierKeys)) return;

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

      return new cKeysEngine
      {
        AppState = AppState,
        KeyEventData = keyEventData,
        Configuration = Configuration,
      }.ProcessKey();

    }

    private Icon GetIconOff() => new Icon(Resources.Off, 40, 40);
    private Icon GetIconNormalMode() => new Icon(Resources.Normal, 40, 40);
    private Icon GetIconInsertMode() => new Icon(Resources.Insert, 40, 40);

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
        // Release the icon resource.
        trayIcon.Dispose();
      }
      //LogStream?.Close();

      base.Dispose(isDisposing);
    } 

  }
}
