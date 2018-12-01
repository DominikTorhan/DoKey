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

namespace TrayApp2 {
  class SysTrayApp : Form {

    [STAThread]
    public static void Main() {
      try {
        Application.Run(new SysTrayApp());
      } catch {
        Application.Exit();
      }
    }

    private NotifyIcon trayIcon;
    private ContextMenu trayMenu;
    private cKeyboardHook mKeyboardHook;
    private Configuration Configuration;
    private StreamWriter LogStream;

    public SysTrayApp() {

      Configuration = new Configuration();

      // Create a simple tray menu with only one item.
      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", (s, e) => OnExit());
      trayMenu.MenuItems.Add("Open Settings", (s, e) => OpenSettings());
      trayMenu.MenuItems.Add("Reload Settings", (s,e) => ReloadSettings());

      // Create a tray icon. In this example we use a
      // standard system icon for simplicity, but you
      // can of course use your own custom icon too.
      trayIcon = new NotifyIcon {
        Text = "TrayApp2",
        Icon = GetIconOff(),

        // Add menu to tray icon and show it.
        ContextMenu = trayMenu,
        Visible = true
      };

      LogStream = CreateLogStream();

      SetupKeyboardHooks();

    }
    AppState AppState = new AppState();


    public void SetupKeyboardHooks() {
      mKeyboardHook = new cKeyboardHook();
      mKeyboardHook.KeyboardPressed += OnKeyPressed;
    }

    private void SetIcon() => trayIcon.Icon = GetIcon(AppState.State);

     
    KeyEventData CreateKetEventData(KeyboardHookEvent e) {
      var key = (Keys)e.KeyboardData.VirtualCode;
      var x = e.IsUp ? KeyEventType.Up : KeyEventType.Down;
      return new KeyEventData(key.ToString(), x);
    }

    private bool IsSending = false;
    
    private void OnKeyPressed(object sender, KeyboardHookEvent e) {

      //if (IsSending) return;
      
      var key = (Keys)e.KeyboardData.VirtualCode;

      if (key == Keys.J) {
        var x = 0;
      }
      
      if (Control.IsKeyLocked(Keys.CapsLock) && key == Keys.Capital) return;
      if (Control.ModifierKeys == Keys.Control) {
        return;
      }

      if (cUtils.IsIgnoredKey(key, Control.ModifierKeys)) return;

      var keyEventData = CreateKetEventData(e);

      var output = ProcessKey(keyEventData);

      if (output == null) return;

      AppState = output.AppState;

      Log(output.AppState.ToLog());

      if (output.PreventKeyProcess) {
        Log("  prevent");
        e.Handled = true;
      }

      if (output.sendDoKey != null && !string.IsNullOrEmpty(output.sendDoKey.Send)) {
        Log(output.sendDoKey.ToLog);
        IsSending = true;
        SendKeys.Send(output.sendDoKey.Send);
        IsSending = false;
      }

      SetIcon();

      return;

    }

    private void Log(string str) {

      Console.WriteLine(str);
      LogStream.WriteLine(str);

    }

    private StreamWriter CreateLogStream() {

      var dt = DateTime.Now;
      var path = $"{dt.Year}{dt.Month}{dt.Day}{dt.Hour}{dt.Minute}{dt.Second}"+ "log.txt";

      return new StreamWriter(path);

    }

    private cOutput ProcessKey(KeyEventData keyEventData) {

      Log(keyEventData.ToLog());

      return new cKeysEngine { 
        AppState = AppState, 
        KeyEventData = keyEventData,
        Configuration = Configuration
      }.ProcessKey();

    }

    private Icon GetIconOff() {
      return new Icon(Resources.Off, 40, 40);
    }

    private Icon GetIconNormalMode() {
      return new Icon(Resources.Normal, 40, 40);
    }

    private Icon GetIconInsertMode() {
      return new Icon(Resources.Insert, 40, 40);
    }

    private Icon GetIcon(State xState) {

      if (xState.IsOff) return GetIconOff();
      if (xState.IsNormal) return GetIconNormalMode();
      if (xState.IsInsert) return GetIconInsertMode();

      return null;

    }

    protected override void OnLoad(EventArgs e) {
      Visible = false; // Hide form window.
      ShowInTaskbar = false; // Remove from taskbar.

      base.OnLoad(e);
    }

    private void OnExit() {
      Application.Exit();
    }

    private void OpenSettings() {

      Process.Start(Configuration.FilePath);

    }

    private void ReloadSettings() {
      Configuration = new Configuration();
    }

    protected override void Dispose(bool isDisposing) {

      mKeyboardHook?.Dispose();

      if (isDisposing) {
        // Release the icon resource.
        trayIcon.Dispose();
      }
      LogStream?.Close();

      base.Dispose(isDisposing);
    }
  }
}
