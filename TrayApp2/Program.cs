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
    private StateEnum State;
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

      mSettings = new cSettings();

      appState = new cAppState();

      State = StateEnum.Off;

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

      var dele = new WinEventDelegate(WinEventProc);
      IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);

      //var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      //path += @"\AppData\Roaming\...DT\config\";
      //
      //FileSystemWatcher fileSystemWatcher = new FileSystemWatcher {
      //  Path = path
      //};
      //fileSystemWatcher.Changed += FileSystemWatcher_Changed;
      //fileSystemWatcher.EnableRaisingEvents = true;

      SetupKeyboardHooks();

    }


    public void SetupKeyboardHooks() {
      mKeyboardHook = new cKeyboardHook();
      mKeyboardHook.KeyboardPressed += OnKeyPressed;
    }

    private void PerformEscape() {

      State = GetStateBackward(State);
      SetIcon();

    }

    private void SetIcon() => trayIcon.Icon = GetIcon(State);

    private StateEnum GetStateBackward(StateEnum xState) {
      
      if (xState == StateEnum.Insert) return StateEnum.Normal;

      return StateEnum.Off;

    }

    private void SetInsertMode() {

      State = StateEnum.Insert;
      SetIcon();

    }

    private void SetOffMode() {

      State = StateEnum.Off;
      SetIcon();

    }

    private bool KeyPressedInsertMode(Keys key) {

      if (!appState.isControl) return false;

      if (key == Keys.Z) return true;

      var sendKey = cUtils.GetSendKeyByKeyInsertModeWithControl(key);

      if (sendKey == "") return false;

      SendKeys.Send(sendKey);

      return true;

    }


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

    private void OnKeyPressed(object sender, cKeyboardHookEvent e) {
      //Debug.WriteLine(e.KeyboardData.VirtualCode);

      var key = (Keys)e.KeyboardData.VirtualCode;

      Console.WriteLine(key);

      cInput input = new cInput {
        eventData = new cEventData {
          keys = key,
          isUp = IsUp(e.KeyboardState)
        },
        stateData = mStateData
      };

      var output = new cKeysEngine{input = input}.ProcessKey();

      mStateData = output.StateData;

      Console.WriteLine(output.StateData.ToString());

      return;

      //ModifierKeys.

      MutateStateByModifiKeys(key, e.KeyboardState);

      if (cUtils.IsIgnoredKey(key)) return;

      if (e.KeyboardState == cKeyboardHook.KeyboardState.SysKeyUp) return;
      if (e.KeyboardState == cKeyboardHook.KeyboardState.KeyUp) {
        //if (cUtils.IsWin(key) && State != StateEnum.Off) e.Handled = true;
        return;
      }

      //if (IsToggleKey(key)) {
      //  switch (State) {
      //    case StateEnum.Off: SetNormalMode(); break;
      //    case StateEnum.Normal: SetInsertMode(); break;
      //    case StateEnum.Insert: SetOffMode(); break;
      //  }
      //  e.Handled = true;
      //  return;
      //}

      if (State == StateEnum.Off) return;

      if (cUtils.IsModifierKey(key)) {
        if (State == StateEnum.Normal) e.Handled = true;
        if (State == StateEnum.Insert) {
          if (e.KeyboardState == cKeyboardHook.KeyboardState.SysKeyDown) return;
          if (cUtils.IsControl(key)) e.Handled = true;
          //if (cUtils.IsAlt(key)) e.Handled = true;
        }
        return;
      }

      if (key == Keys.Escape) {
        SetOffMode();
        e.Handled = true;
        return;
      }

      var x = Control.ModifierKeys;

      if (State == StateEnum.Insert) {
        if (KeyPressedInsertMode(key)) {
          e.Handled = true;
        }
        return;
      }

      if (cUtils.IsInsertKey(key)) {
        SetInsertMode();
        e.Handled = true;
      }

      Console.WriteLine("keyCode" + e.KeyboardData.VirtualCode + " " + e.KeyboardData.TimeStamp);
      Console.WriteLine("keyStare" + e.KeyboardState.ToString());

      string sendKey;

      if (appState.isShift) {
        sendKey = cUtils.GetSendKeyByKeyNormalModeWithShift(key);
      } else if (appState.isControl) {
        sendKey = cUtils.GetSendKeyByKeyNormalModeWithControl(key);
      } else {
        sendKey = cUtils.GetSendKeyByKeyNormalMode(key, mSettings);
      }

      if (sendKey != "") {
        e.Handled = true;
        SendKeys.Send(sendKey);
        Console.WriteLine("SendKeys: " + sendKey);
        return;
      }

      if (cUtils.IsLetterKey(key)){
        e.Handled = true;
      }

      return;

      if (e.KeyboardData.VirtualCode == 81) {
        e.Handled = true;
      }

      if (e.KeyboardData.VirtualCode != cKeyboardHook.VkSnapshot)
        return;

      // seems, not needed in the life.
      //if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown &&
      //    e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
      //{
      //    MessageBox.Show("Alt + Print Screen");
      //    e.Handled = true;
      //}
      //else



      if (e.KeyboardState == cKeyboardHook.KeyboardState.KeyDown) {
        MessageBox.Show("Print Screen");
        e.Handled = true;
      }
    }



    private void MutateStateByModifiKeys(Keys key, cKeyboardHook.KeyboardState state) {

      if (cUtils.IsShift(key)) appState.isShift = state == cKeyboardHook.KeyboardState.KeyDown;
      if (cUtils.IsControl(key)) appState.isControl = state == cKeyboardHook.KeyboardState.KeyDown;
      if (cUtils.IsWin(key)) appState.isWin = state == cKeyboardHook.KeyboardState.KeyDown;
      if (cUtils.IsAlt(key)) appState.isAlt = state == cKeyboardHook.KeyboardState.SysKeyDown;

    }

    private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e) {

      return;

      //if (e.FullPath != FullPath_Msg) return;
      //
      //var pText = File.ReadAllText(FullPath_Msg);
      //
      //var pLast = pText.Last();
      //
      //if (pLast == 'i') {
      //  SetInsertMode();
      //} else if (pLast == 'n') {
      //  SetNormalMode();
      //}

    }

    private void SetNormalMode() {


      State = StateEnum.Normal;

      WriteStateFile("1");
      trayIcon.Icon = GetIconNormalMode();

    }

    protected override void WndProc(ref Message msg) {
      //if (aMessage.Msg == WM_AMESSAGE) {
      //  //WM_AMESSAGE Dispatched
      //  //Let’s do something here
      //  //...
      //}

      Console.WriteLine(msg.ToString());

      if (msg.Msg == 0x401) {
        MessageBox.Show(msg.ToString());
      }

      base.WndProc(ref msg);

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

    private string LastActiveProcess = "";

    public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {

      Console.WriteLine(hWinEventHook);

      var pActiveProcess = GetActiveProcessFileName();

      if (LastActiveProcess == pActiveProcess) return;

      var isProcessEnabled = IsModuleNameEnabledToUseVim(pActiveProcess);

      //State = isProcessEnabled ? StateEnum.Normal : StateEnum.Off;
      State = StateEnum.Off;

      //trayIcon.Icon = GetIcon(pActiveProcess);
      SetIcon();

      //WriteStateFile(text);

      LastActiveProcess = pActiveProcess;

      //Log.Text += GetActiveWindowTitle() + "\r\n";
    }

    private void WriteStateFile(string xText) {

      //File.WriteAllText(FullPath_State, xText);

    }

    private Icon GetIcon(string xModuleName) {

      if (!IsModuleNameEnabledToUseVim(xModuleName)) return GetIconOff();

      return GetIconNormalMode();

    }

    private Icon GetIcon(StateEnum xState) {

      switch (xState) {
        case StateEnum.Off: return GetIconOff();
        case StateEnum.Normal: return GetIconNormalMode();
        case StateEnum.Insert: return GetIconInsertMode();
      }

      return null;

    }

    private bool IsModuleNameEnabledToUseVim(string xModuleName) {

      return cAplictations.Cln().Contains(xModuleName);

    }

    private string GetActiveProcessFileName() {
      IntPtr hwnd = GetForegroundWindow();
      uint pid;
      GetWindowThreadProcessId(hwnd, out pid);
      Process p = Process.GetProcessById((int)pid);

      var name = p.ProcessName;

      return name;

      //p.MainModule.FileName.Dump();
      return p.MainModule.ModuleName;
      return p.MainModule.FileName;

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
