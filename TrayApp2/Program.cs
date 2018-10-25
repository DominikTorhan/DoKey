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
      Application.Run(new SysTrayApp());
    }

    private cAppState appState;
    //private StateEnum State;
    private NotifyIcon trayIcon;
    private ContextMenu trayMenu;


    delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    [DllImport("user32.dll")]
    static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    private const uint WINEVENT_OUTOFCONTEXT = 0;
    private const uint EVENT_SYSTEM_FOREGROUND = 3;

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    private cKeyboardHook mKeyboardHook;
    private cSettings mSettings;

    public SysTrayApp() {

      mSettings = new cSettings("json1.json");

      appState = new cAppState();

      //State = StateEnum.Off;

      // Create a simple tray menu with only one item.
      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", OnExit);
      trayMenu.MenuItems.Add("Test", Test);

      // Create a tray icon. In this example we use a
      // standard system icon for simplicity, but you
      // can of course use your own custom icon too.
      trayIcon = new NotifyIcon();
      trayIcon.Text = "TrayApp2";
      trayIcon.Icon = GetIconOff();

      // Add menu to tray icon and show it.
      trayIcon.ContextMenu = trayMenu;
      trayIcon.Visible = true;

      SetupKeyboardHooks();

    }


    public void SetupKeyboardHooks() {
      mKeyboardHook = new cKeyboardHook();
      mKeyboardHook.KeyboardPressed += OnKeyPressed;
    }

    private void SetIcon() => trayIcon.Icon = GetIcon(mStateData.state);

    private bool IsUp(cKeyboardHook.KeyboardState keyState) {

      switch (keyState) {
        case cKeyboardHook.KeyboardState.KeyUp:
        case cKeyboardHook.KeyboardState.SysKeyUp:
          return true;
        default:
          return false;
      }

    }

    cStateData mStateData = new cStateData();

    private cModificators CreateModificators() {

      return new cModificators {
        isAlt = Control.ModifierKeys == Keys.Alt,
        isShift = Control.ModifierKeys == Keys.Shift,
        isControl = Control.ModifierKeys == Keys.Control,
      };

    }

    bool TempIgnoreWithModificators(cModificators modificators, Keys keys) {

      if (keys == Keys.N) return false;
      if (keys == Keys.M) return false;
      if (keys == Keys.OemPeriod) return false;
      if (keys == Keys.Oemcomma) return false;
      if (keys == Keys.H) return false;
      if (keys == Keys.J) return false;
      if (keys == Keys.K) return false;
      if (keys == Keys.L) return false;

      if (modificators.isAlt) return true;
      if (modificators.isControl) return true;

      return false;

    }

    private void OnKeyPressed(object sender, cKeyboardHookEvent e) {
      //Debug.WriteLine(e.KeyboardData.VirtualCode);

      var key = (Keys)e.KeyboardData.VirtualCode;

      var isCapsLockToggled = Control.IsKeyLocked(Keys.CapsLock);
      if (isCapsLockToggled) {
        if (key == Keys.Capital) return;
      }

      var modificators = CreateModificators();

      if (TempIgnoreWithModificators(modificators, key)) return;

      if (cUtils.IsIgnoredKey(key)) return;

      var isUp = IsUp(e.KeyboardState);

      cInput input = new cInput {
        eventData = new cEventData {
          keys = key,
          isUp = isUp
        },
        stateData = mStateData,
        modificators = modificators
      };

      var upOrDown = isUp ? "up" : "down";
      Console.WriteLine(key + " " + upOrDown);
      Console.WriteLine(input.modificators);

      var output = new cKeysEngine{
        input = input,
        settings = mSettings
      }.ProcessKey();

      mStateData = output.StateData;

      Console.WriteLine(output.StateData.ToString());

      if (output.PreventKeyProcess) {
        Console.WriteLine("  prevent");
        e.Handled = true;
      }

      if (!string.IsNullOrEmpty(output.SendKeys)) {
        Console.WriteLine(output.SendKeys);
        SendKeys.Send(output.SendKeys);
      }

      SetIcon();

      return;

    }

    //protected override void WndProc(ref Message msg) {
    //  //if (aMessage.Msg == WM_AMESSAGE) {
    //  //  //WM_AMESSAGE Dispatched
    //  //  //Let’s do something here
    //  //  //...
    //  //}

    //  Console.WriteLine(msg.ToString());

    //  if (msg.Msg == 0x401) {
    //    MessageBox.Show(msg.ToString());
    //  }

    //  base.WndProc(ref msg);

    //}

    private Icon GetIconOff() {
      return new Icon(Resources.Off, 40, 40);
    }

    private Icon GetIconNormalMode() {
      return new Icon(Resources.Normal, 40, 40);
    }

    private Icon GetIconInsertMode() {
      return new Icon(Resources.Insert, 40, 40);
    }

    private Icon GetIcon(StateEnum xState) {

      switch (xState) {
        case StateEnum.Off: return GetIconOff();
        case StateEnum.Normal: return GetIconNormalMode();
        case StateEnum.Insert: return GetIconInsertMode();
      }

      return null;

    }

    //private bool IsModuleNameEnabledToUseVim(string xModuleName) {

    //  return cAplictations.Cln().Contains(xModuleName);

    //}

    //private string GetActiveProcessFileName() {
    //  IntPtr hwnd = GetForegroundWindow();
    //  uint pid;
    //  GetWindowThreadProcessId(hwnd, out pid);
    //  Process p = Process.GetProcessById((int)pid);

    //  var name = p.ProcessName;

    //  return name;

    //  //p.MainModule.FileName.Dump();
    //  return p.MainModule.ModuleName;
    //  return p.MainModule.FileName;

    //}


    protected override void OnLoad(EventArgs e) {
      Visible = false; // Hide form window.
      ShowInTaskbar = false; // Remove from taskbar.

      base.OnLoad(e);
    }

    private void OnExit(object sender, EventArgs e) {
      Application.Exit();
    }

    private void Test(object sender, EventArgs e) {
      var x = GetForegroundWindow();

      MessageBox.Show(x.ToString());
    }

    protected override void Dispose(bool isDisposing) {

      mKeyboardHook?.Dispose();

      if (isDisposing) {
        // Release the icon resource.
        trayIcon.Dispose();
      }

      base.Dispose(isDisposing);
    }
  }
}
