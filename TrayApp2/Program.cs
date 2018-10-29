﻿using DoKey.FS;
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

      // Create a simple tray menu with only one item.
      trayMenu = new ContextMenu();
      trayMenu.MenuItems.Add("Exit", OnExit);
      trayMenu.MenuItems.Add("Test", Test);

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
     
    KeyEventData CreateKetEventData(cKeyboardHookEvent e) {
      var key = (Keys)e.KeyboardData.VirtualCode;
      var x = IsUp(e.KeyboardState) ? KeyEventType.Up : KeyEventType.Down;
      return new KeyEventData(key.ToString(), x);
    }

    private void OnKeyPressed(object sender, cKeyboardHookEvent e) {
      //Debug.WriteLine(e.KeyboardData.VirtualCode);

      var key = (Keys)e.KeyboardData.VirtualCode;

      var isCapsLockToggled = Control.IsKeyLocked(Keys.CapsLock);
      if (isCapsLockToggled) {
        if (key == Keys.Capital) return;
      }

      if (Control.ModifierKeys == Keys.Control) return;
      //if (Control.ModifierKeys == Keys.Shift) return;

      if (cUtils.IsIgnoredKey(key)) return;

      //var isUp = IsUp(e.KeyboardState);

      cInput input = new cInput { 
        KeyEventData = CreateKetEventData(e),
        //eventData = new cEventData {
        //  keys = key,
        //  isUp = isUp
        //},
        stateData = mStateData,
      };

      //var upOrDown = isUp ? "up" : "down";
      //Console.WriteLine(key + " " + upOrDown);
      

      var output = new cKeysEngine{
        input = input,
        settings = mSettings
      }.ProcessKey();

      if (output == null) return;

      mStateData = output.StateData;

      Console.WriteLine(output.StateData.ToString());

      if (output.PreventKeyProcess) {
        Console.WriteLine("  prevent");
        e.Handled = true;
      }

      if (output.sendDoKey != null && !string.IsNullOrEmpty(output.sendDoKey.Send)) {
        Console.WriteLine(output.sendDoKey.ToLog);
        SendKeys.Send(output.sendDoKey.Send);
      }

      SetIcon();

      return;

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

    private Icon GetIcon(StateEnum xState) {

      switch (xState) {
        case StateEnum.Off: return GetIconOff();
        case StateEnum.Normal: return GetIconNormalMode();
        case StateEnum.Insert: return GetIconInsertMode();
      }

      return null;

    }

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
